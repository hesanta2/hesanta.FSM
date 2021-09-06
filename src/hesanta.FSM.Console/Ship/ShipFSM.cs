using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class ShipFSM : FSM
    {
        private const int leftLimitPixel = 30;
        private readonly Ship ship;

        public ShipFSM(Ship ship)
        {
            this.ship = ship ?? throw new ArgumentNullException(nameof(ship));
            InitFSM();
        }

        private void InitFSM()
        {
            var idle = new StartState("Idle", () =>
            {
                ship.Destroyed = false;
            });
            var right = new State("Right", () =>
            {
                ship.Position.X += ship.Velocity;
            });
            var left = new State("Left", () =>
            {
                ship.Position.X -= ship.Velocity;
            });

            var destroy = new State("Destroy", () =>
            {
                ship.Destroyed = true;
            });

            var idleToLeft = new Transition(idle, left, () =>
            {
                if (ship.Left && ship.Position.X > leftLimitPixel)
                {
                    return left;
                }
                return null;
            });
            var idleToRight = new Transition(idle, right, () =>
            {
                if (ship.Right && ship.Position.X < ship.Engine.Graphics.Width - ship.Size.Width)
                {
                    return right;
                }

                return null;
            });
            var idleToDestroy = new Transition(idle, destroy, () =>
            {
                var enemies = ship.Engine.EngineObjects.FirstOrDefault(engineObject => engineObject as Enemies != null) as Enemies;
                if (enemies != null)
                {
                    foreach (var enemy in enemies.EnemiesList)
                    {
                        if (
                            enemy.Initiated &&
                            enemy.Bullet.Position.Y >= ship.Position.Y &&
                            enemy.Bullet.Position.X >= ship.Position.X &&
                            enemy.Bullet.Position.X <= ship.Position.X + ship.Size.Width
                        )
                        {
                            return destroy;
                        }
                    }
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
            var destroyToIdle = new Transition(destroy, idle, () =>
            {
                return idle;
            });

            AddTransitions(idleToLeft, idleToRight, idleToDestroy, rightToIdle, leftToIdle, destroyToIdle);
        }

    }
}
