using hesanta.Drawing.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class Enemy : EngineObject<string>
    {
        private readonly Ship ship;
        private float bulletVelocity = 10;

        public IFSM FSM { get; set; }
        public bool Destroyed { get; set; } = false;
        public bool Initiated { get; set; } = false;
        public float Velocity { get; set; } = 15;
        public Bullet Bullet { get; protected set; }


        public Enemy(IGraphicsEngine<string> engine, Ship ship) : base(engine)
        {
            this.ship = ship ?? throw new ArgumentNullException(nameof(ship));

            FSM = new EnemyFSM(this, ship);
            Bullet = new Bullet(engine, this, bulletVelocity, false);
        }

        public override void InternalDraw(params object[] args)
        {
            FSM.Update();
            DrawString($@"
/òó\
 \/
", new SolidBrush(Color.GreenYellow), new PointF(Position.X, Position.Y + 1));
            Bullet.Draw();
        }
    }
}
