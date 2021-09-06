using hesanta.Drawing.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            var colored = args?.Length > 0 ? (bool)args[0] : true;
            var ship = args?.Length > 1 ? args[1] as Ship : null;
            var enemies = args?.Length > 2 ? args[2] as Enemies : null;

            Position.X = 1;
            Position.Y = 1;

            DrawString($@"
FPS: {Engine.FPS}
Deltatime: {Math.Round(Engine.DeltaTime, 4)}
Colored(keys: c/b): {colored}
Ship: {ship?.FSM?.CurrentState}
Ship.Bullet: {ship?.Bullet.FSM?.CurrentState}
Enemies: {enemies?.FSM.CurrentState}
", textBrush, Position);
            DrawRectangle(pen, new PointF(Position.X - 1, Position.Y - 1), Size.Width + 2, Size.Height + 2, false);
        }
    }
}
