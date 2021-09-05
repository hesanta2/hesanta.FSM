using hesanta.Drawing.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class Legend : EngineObject<string>
    {
        private SolidBrush textBrush = new SolidBrush(Color.White);
        private Pen pen = new Pen(Color.Green);

        public Legend(IGraphicsEngine<string> engine) : base(engine) { }

        public override void InternalDraw(params object[] args)
        {
            var ship = args?.Length > 0 ? args[0] as Ship : null;

            Position.X = 1;
            Position.Y = 1;

            DrawString($@"
FPS: {Engine.FPS}
Deltatime: {Math.Round(Engine.DeltaTime, 4)}
Ship: {ship.FSM?.CurrentState}
Ship.Bullet: {ship.BulletObject.FSM?.CurrentState}
", textBrush, Position);
            DrawRectangle(pen, new PointF(Position.X - 1, Position.Y - 1), Size.Width + 2, Size.Height + 2, false);
        }
    }
}
