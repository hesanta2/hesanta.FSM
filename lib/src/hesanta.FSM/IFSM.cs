using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hesanta.FSM
{
    public interface IFSM
    {
        FSMStatus Status { get; }
        IState CurrentState { get; }
        IEnumerable<IState> States { get; }
        IEnumerable<ITransition> Transitions { get; }

        void SetState(IState state);
        void AddTransition(ITransition trasition);
        void AddTransitions(params ITransition[] transitions);
        Task RunAsync();
        void Update();
    }
}
