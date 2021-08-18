using System;

namespace hesanta.FSM.States
{
    public class StartState : State, IStartState
    {
        public StartState(string name = null, Action execute = null) : base(name ?? "Start", execute) { }
    }
}
