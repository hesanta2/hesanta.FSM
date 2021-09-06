using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class EnemiesFSM : FSM
    {
        private const float downOffset = 0.25f;
        private const int leftLimitPixel = 30;
        private readonly Enemies enemies;
        private float lastDownPosition = float.MaxValue;

        public EnemiesFSM(Enemies enemies)
        {
            this.enemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
            InitFSM();
        }

        private void InitFSM()
        {
            var idle = new StartState("Idle");
            var right = new State("Right", () =>
            {
                enemies.Position.X += enemies.Velocity * enemies.Engine.DeltaTime;
                lastDownPosition = enemies.Position.Y;
            });
            var left = new State("Left", () =>
            {
                enemies.Position.X -= enemies.Velocity * enemies.Engine.DeltaTime;
                lastDownPosition = enemies.Position.Y;
            });
            var down = new State("Down", () =>
            {
                enemies.Position.Y += enemies.Velocity * enemies.Engine.DeltaTime;
            });

            var idleToLeft = new Transition(idle, right, () =>
            {
                return right;
            });

            var leftToDown = new Transition(left, down, () =>
            {
                if (enemies.Position.X < leftLimitPixel)
                {
                    return down;
                }

                return null;
            });
            var downToRight = new Transition(down, right, () =>
            {
                var downStep = enemies.Size.Height * downOffset;

                if (enemies.Position.Y > lastDownPosition + downStep && enemies.Position.X <= leftLimitPixel)
                {
                    return right;
                }

                return null;
            });
            var rightToDown = new Transition(right, down, () =>
            {
                if (enemies.Position.X > enemies.Engine.Graphics.Width - enemies.Size.Width)
                {
                    return down;
                }

                return null;
            });
            var downToLeft = new Transition(down, left, () =>
            {
                var downStep = enemies.Size.Height * downOffset;

                if (enemies.Position.Y > lastDownPosition + downStep && enemies.Position.X > enemies.Engine.Graphics.Width - enemies.Size.Width)
                {
                    return left;
                }
                return null;
            });

            AddTransitions(idleToLeft, leftToDown, downToRight, rightToDown, downToLeft);
        }

    }
}
