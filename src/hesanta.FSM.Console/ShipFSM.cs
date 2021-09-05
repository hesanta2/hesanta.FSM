using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class ShipFSM : FSM
    {
        private readonly Ship ship;

        public ShipFSM(Ship ship)
        {
            this.ship = ship ?? throw new ArgumentNullException(nameof(ship));
            InitFSM();
        }

        private void InitFSM()
        {
            var idle = new StartState("Idle");
            var right = new State("Right", () =>
            {
                ship.Position.X += ship.Velocity;
            });
            var left = new State("Left", () =>
            {
                ship.Position.X -= ship.Velocity;
            });

            var idleToLeft = new Transition(idle, left, () =>
            {
                if (ship.Left)
                {
                    return left;
                }

                return null;
            });
            var idleToRight = new Transition(idle, right, () =>
            {
                if (ship.Right)
                {
                    return right;
                }

                return null;
            });
            var rightToIdle = new Transition(right, idle, () =>
            {
                if (!ship.Right)
                {
                    return idle;
                }
                return null;
            });
            var leftToIdle = new Transition(left, idle, () =>
            {
                if (!ship.Left)
                {
                    return idle;
                }
                return null;
            });

            AddTransitions(idleToLeft, idleToRight, rightToIdle, leftToIdle);
        }

    }
}
