using System;

namespace hesanta.FSM.States
{
    public class EndState : State, IEndState
    {
        public EndState(Action execute = null) : base("End", execute) { }
    }
}
