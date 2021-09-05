using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class BulletFSM : FSM
    {
        private readonly Bullet bullet;

        public BulletFSM(Bullet bullet)
        {
            this.bullet = bullet;
            InitFSM();
        }

        private void InitFSM()
        {
            var idleState = new StartState("Idle", () =>
            {
                bullet.Shooting = false;
                bullet.Position.Y = bullet.Ship.Position.Y + 1;
                bullet.Position.X = bullet.Ship.Position.X + bullet.Ship.Size.Width / 2;
            });
            var shootingState = new State("Shooting", () =>
            {
                bullet.Shooting = true;
                bullet.Position.Y -= bullet.Velocity * bullet.Engine.DeltaTime;
            });

            var idleToshootingTransition = new Transition(idleState, shootingState, () =>
            {
                if (bullet.Shoot)
                {
                    bullet.Shoot = false;
                    return shootingState;
                }

                return null;
            });
            var shootingToidleTransition = new Transition(shootingState, idleState, () =>
            {
                if (bullet.Position.Y > 0)
                {
                    return null;
                }
                return idleState;
            });

            AddTransitions(idleToshootingTransition, shootingToidleTransition);
        }

    }
}
