using hesanta.Drawing;
using hesanta.Drawing.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class Enemies : EngineObject<string>
    {
        private const int Count = 20;
        private readonly Ship ship;

        public IList<Enemy> EnemiesList { get; set; }
        public float Velocity { get; set; } = 15;
        public IFSM FSM { get; set; }
        public bool Initiated
        {
            set
            {
                foreach (var enemy in EnemiesList)
                {
                    enemy.Initiated = true;
                }
            }
        }

        public Enemies(IGraphicsEngine<string> engine, Ship ship) : base(engine)
        {
            this.ship = ship ?? throw new ArgumentNullException(nameof(ship));
            FSM = new EnemiesFSM(this);
        }

        public void DrawEnemies()
        {
            for (int x = 0; x < Count; x++)
            {
                var firstEnemy = EnemiesList.First();
                var enemy = EnemiesList[x];
                if (enemy.Destroyed) { continue; }

                enemy.Position.X = Position.X + x % 10 * firstEnemy.Size.Width * 1.5f;
                enemy.Position.Y = Position.Y + x / 10 * firstEnemy.Size.Height * 1.5f;
                enemy.Draw();
            }
        }

        public override void InternalDraw(params object[] args)
        {
            FSM.Update();

            if (EnemiesList == null)
            {
                EnemiesList = new List<Enemy>();
                for (int x = 0; x < Count; x++)
                {
                    var enemy = new Enemy(Engine, ship);
                    enemy.Draw();
                    enemy.Position.X = Position.X + x % Count / 2 * enemy.Size.Width * 1.5f;
                    enemy.Position.Y = Position.Y + x / Count / 2 * enemy.Size.Height * 1.5f;
                    EnemiesList.Add(enemy);
                    boundsList.Add(new RectangleF(enemy.Position.X, enemy.Position.Y, enemy.Size.Width, enemy.Size.Height));
                }
            }

            DrawEnemies();
        }
    }
}
