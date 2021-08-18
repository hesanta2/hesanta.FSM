using System;

namespace hesanta.FSM.States
{
    public class State : IState
    {
        public string Name { get; }
        public Action Execute { get; set; }

        public State(string name, Action execute = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            Name = name;
            Execute = execute;
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
