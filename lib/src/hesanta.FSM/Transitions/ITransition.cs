using hesanta.FSM.States;
using System;

namespace hesanta.FSM.Transitions
{
    public interface ITransition
    {
        IState StateFrom { get; }
        IState StateTo { get; }
        Func<IState> EvaluateFunc { get; }
    }
}