using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class EnemyFSM : FSM
    {
        private readonly Enemy enemy;
        private readonly Ship ship;
        private Random random;
        private Stopwatch stopwatch;

        public EnemyFSM(Enemy enemy, Ship ship)
        {
            this.enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            this.ship = ship ?? throw new ArgumentNullException(nameof(ship));

            random = new Random((int)DateTime.Now.Ticks);
            stopwatch = new Stopwatch();
            stopwatch.Start();
            InitFSM();
        }

        private void InitFSM()
        {
            var idle = new StartState("Idle");
            var shoot = new State("Shoot", () =>
            {
                enemy.Bullet.Shoot = true;
                stopwatch.Restart();
            });
            var destroy = new EndState("Destroy", () =>
            {
                enemy.Destroyed = true;
                ship.Bullet.Destroyed = true;
            });

            var idleToDestroy = new Transition(idle, destroy, () =>
            {
                if (
                    enemy.Initiated &&
                    ship.Bullet.Position.Y <= enemy.Position.Y &&
                    ship.Bullet.Position.X >= enemy.Position.X &&
                    ship.Bullet.Position.X <= enemy.Position.X + enemy.Size.Width
                )
                {
                    return destroy;
                }

                return null;
            });

            var idleToShoot = new Transition(idle, shoot, () =>
            {
                var randomValue = random.Next(100);
                var randomSeconds = random.Next(5000) + 2000;
                if (randomValue > 98 && !enemy.Bullet.Shooting && stopwatch.ElapsedMilliseconds > randomSeconds)
                {
                    return shoot;
                }

                return null;
            });

            var shootToIdle = new Transition(shoot, idle, () =>
            {
                return idle;
            });

            AddTransitions(idleToDestroy, idleToShoot, shootToIdle);
        }

    }
}
