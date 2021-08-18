using hesanta.FSM.States;
using System;

namespace hesanta.FSM.Transitions
{
    public class Transition : ITransition
    {
        public IState StateFrom { get; }
        public IState StateTo { get; }
        public Func<IState> EvaluateFunc { get; }

        public Transition(IState stateFrom, IState stateTo, Func<IState> evaluateFunc)
        {
            StateFrom = stateFrom;
            StateTo = stateTo;
            EvaluateFunc = evaluateFunc;
        }

        public override string ToString()
        {
            return $"{StateFrom} -> {StateTo}";
        }
    }
}
