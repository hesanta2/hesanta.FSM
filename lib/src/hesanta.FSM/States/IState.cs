using System;

namespace hesanta.FSM.States
{
    public interface IState
    {
        Action Execute { get; set; }
    }
}