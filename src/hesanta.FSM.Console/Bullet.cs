using hesanta.Drawing;
using hesanta.Drawing.Engine;
using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Drawing;
using System.Linq;

namespace hesanta.FSM.Sample
{
    public class Bullet : EngineObject<string>
    {
        public Ship Ship { get; protected set; }
        public float Velocity { get; set; }
        public IFSM FSM { get; set; }
        public bool Shoot { get; set; } = false;
        public bool Shooting { get; set; } = false;
        public Bullet(IGraphicsEngine<string> engine, Ship ship, float velocity) : base(engine)
        {
            this.Ship = ship ?? throw new ArgumentNullException(nameof(ship));
            this.Velocity = velocity;

            ship.Draw(engine);
            FSM = new BulletFSM(this);
        }

        public override void InternalDraw(params object[] args)
        {
            ConsoleKey? pressedKey = args.Any() ? args[0] as ConsoleKey? : null;
            FSM.Update();

            if (pressedKey == ConsoleKey.Spacebar && !Shooting)
            {
                Shoot = true;
            }

            if (Shooting)
            {
                DrawString("^", new SolidBrush(Color.Aqua), Position);
            }
        }
    }
}
