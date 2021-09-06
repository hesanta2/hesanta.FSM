using System;

namespace hesanta.FSM.States
{
    public class EndState : State, IEndState
    {
        public EndState(string name = null, Action execute = null) : base(name ?? "End", execute) { }
    }
}
