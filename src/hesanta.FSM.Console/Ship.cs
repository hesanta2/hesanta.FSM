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
        public float Velocity { get; set; } = 2.5f;
        private float bulletVelocity = 50;

        public Bullet BulletObject { get; protected set; }
        public ShipFSM FSM { get; set; }
        public bool Left { get; protected set; }
        public bool Right { get; private set; }

        public Ship(IGraphicsEngine<string> engine) : base(engine)
        {
            FSM = new ShipFSM(this);
            BulletObject = new Bullet(engine, this, bulletVelocity);
        }

        public override void InternalDraw(params object[] args)
        {
            ConsoleKey?  pressedKey = args.Any() ? args[0] as ConsoleKey? : null;
            FSM.Update();

            Left = false;
            Right = false;
            if (pressedKey == ConsoleKey.LeftArrow && Position.X > 0)
            {
                Left = true;
                Right = false;
            }
            if (pressedKey == ConsoleKey.RightArrow && Position.X < Engine.Graphics.Width - Size.Width)
            {
                Right = true;
                Left = false;
            }

            DrawString($@"
  _  
 /-\ 
´---`
", new SolidBrush(Color.Aqua), Position);
            BulletObject?.Draw(pressedKey);
        }
    }
}
