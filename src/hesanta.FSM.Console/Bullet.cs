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
        public IFSM FSM { get; set; }
        public IEngineObject<string> Emisor { get; protected set; }
        public float Velocity { get; set; }
        public bool Shoot { get; set; } = false;
        public bool Shooting { get; set; } = false;
        public bool Destroyed { get; set; } = false;
        public bool UpDirection { get; internal set; }

        public Bullet(IGraphicsEngine<string> engine, IEngineObject<string> emisor, float velocity, bool upDirection = true) : base(engine)
        {
            this.Emisor = emisor ?? throw new ArgumentNullException(nameof(emisor));
            this.Velocity = velocity;
            UpDirection = upDirection;
            FSM = new BulletFSM(this);
        }

        public override void InternalDraw(params object[] args)
        {
            FSM.Update();

            if (Shooting)
            {
                DrawString(UpDirection ? "^" : "v", new SolidBrush(Color.Aqua), Position);
            }
        }
    }
}
