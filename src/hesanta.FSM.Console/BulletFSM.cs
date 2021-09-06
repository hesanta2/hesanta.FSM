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
            var idle = new StartState("Idle", () =>
            {
                bullet.Shooting = false;
                bullet.Destroyed = false;
                bullet.Position.Y = bullet.UpDirection ? bullet.Emisor.Position.Y + 1 : bullet.Emisor.Position.Y + bullet.Emisor.Size.Height + 1;
                bullet.Position.X = bullet.Emisor.Position.X + bullet.Emisor.Size.Width / 2;
            });
            var shooting = new State("Shooting", () =>
            {
                bullet.Shooting = true;
                var direction = bullet.UpDirection ? -1 : 1;

                bullet.Position.Y += bullet.Velocity * direction * bullet.Engine.DeltaTime;
            });

            var destroyed = new State("Destroyed", () =>
            {
            });

            var idleToshootingTransition = new Transition(idle, shooting, () =>
            {
                if (bullet.Shoot)
                {
                    bullet.Shoot = false;
                    return shooting;
                }

                return null;
            });
            var shootingTodestroyedTransition = new Transition(shooting, destroyed, () =>
            {
                if (bullet.Destroyed)
                {
                    return destroyed;
                }

                if ((bullet.UpDirection && bullet.Position.Y > 0) || (!bullet.UpDirection && bullet.Position.Y < bullet.Engine.Graphics.Height))
                {
                    return null;
                }

                return destroyed;
            });

            var destroyedToIdle = new Transition(destroyed, idle, () =>
            {
                return idle;
            });


            AddTransitions(idleToshootingTransition, shootingTodestroyedTransition, destroyedToIdle);
        }

    }
}
