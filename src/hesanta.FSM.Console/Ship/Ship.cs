using hesanta.Drawing;
using hesanta.Drawing.Engine;
using hesanta.FSM.States;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace hesanta.FSM.Sample
{
    public partial class Ship : EngineObject<string>
    {
        private float bulletVelocity = 50;

        public IFSM FSM { get; set; }
        public float Velocity { get; set; } = 2.5f;
        public Bullet Bullet { get; protected set; }
        public bool Left { get; protected set; }
        public bool Right { get; private set; }
        public bool Destroyed { get; internal set; }

        public Ship(IGraphicsEngine<string> engine) : base(engine)
        {
            FSM = new ShipFSM(this);
            Bullet = new Bullet(engine, this, bulletVelocity);
        }

        public override void InternalDraw(params object[] args)
        {
            ConsoleKey? pressedKey = args.Any() ? args[0] as ConsoleKey? : null;
            FSM.Update();

            Left = false;
            Right = false;
            if (pressedKey == ConsoleKey.LeftArrow)
            {
                Left = true;
                Right = false;
            }
            if (pressedKey == ConsoleKey.RightArrow)
            {
                Right = true;
                Left = false;
            }
            if (pressedKey == ConsoleKey.Spacebar && !Bullet.Shooting)
            {
                Bullet.Shoot = true;
            }

            if (!Destroyed)
                DrawString($@"
  _  
 /-\ 
´---`
", new SolidBrush(Color.Aqua), Position);
            Bullet.Draw();
        }
    }
}
