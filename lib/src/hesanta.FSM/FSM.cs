using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hesanta.FSM
{
    public enum FSMStatus
    {
        NotStarted = 0,
        Running = 1,
        Transitioning = 2,
        Finished = 3
    }

    public class FSM : IFSM
    {
        private ICollection<IState> internalStates = new HashSet<IState>();
        private ICollection<ITransition> internalTransitions = new HashSet<ITransition>();
        public FSMStatus Status { get; protected set; } = FSMStatus.NotStarted;
        public IState CurrentState { get; protected set; }
        public IStartState StartState => States.FirstOrDefault(x => x is IStartState) as IStartState;
        public IEnumerable<IState> States => internalStates;
        public IEnumerable<ITransition> Transitions => internalTransitions;

        public FSM() { }

        public void SetState(IState state)
        {
            CurrentState = state;
        }

        public void AddTransition(ITransition transition)
        {
            if (internalTransitions.Contains(transition))
            {
                throw new InvalidOperationException($"Add the same transition '{transition.StateFrom} -> {transition.StateTo}'");
            }

            internalTransitions.Add(transition);

            if (!internalStates.Contains(transition.StateFrom))
            {
                internalStates.Add(transition.StateFrom);
            }
            if (!internalStates.Contains(transition.StateTo))
            {
                internalStates.Add(transition.StateTo);
            }
        }

        public void AddTransitions(params ITransition[] transitions)
        {
            if (transitions == null) { return; }
            foreach (var transition in transitions)
            {
                AddTransition(transition);
            }
        }


        public Task RunAsync()
        {
            if (CurrentState == null)
            {
                CurrentState = StartState;
            }

            return Task.Run(() =>
            {
                if (!States.Any(x => x is IStartState))
                {
                    throw new InvalidOperationException("There is not an state type of 'IStartState'");
                }

                if (CurrentState == null || States.Count() == 0)
                {
                    Status = FSMStatus.NotStarted;
                    return;
                }

                while (!(CurrentState is IEndState))
                {
                    Status = FSMStatus.Running;
                    if (CurrentState.Execute != null) { CurrentState.Execute(); }
                    var stateTransitions = GetStateTransitions(CurrentState);
                    foreach (var transition in stateTransitions)
                    {
                        IState transitionedState = transition.EvaluateFunc();
                        if (transitionedState != null)
                        {
                            CurrentState = transitionedState;
                        }
                    }
                }

                if (CurrentState.Execute != null) { CurrentState.Execute(); }
                Status = FSMStatus.Finished;
            });
        }

        private IEnumerable<ITransition> GetStateTransitions(IState state)
        {
            var transitions = Transitions.Where(x => x.StateFrom == state);
            if (transitions.Count() == 0)
            {
                throw new InvalidCastException($"There is not transitions from this '{state}' state.");
            }

            return transitions;
        }
    }
}
