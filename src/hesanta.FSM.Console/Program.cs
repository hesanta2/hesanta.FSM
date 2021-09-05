using hesanta.Drawing;
using hesanta.Drawing.ASCII;
using hesanta.Drawing.Engine;
using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using TrueColorConsole;
using Graphics = hesanta.Drawing.ASCII.Graphics;

namespace hesanta.FSM.Sample
{
    partial class Program
    {
        static void Main(string[] args)
        {
            StartLoop();
        }

        private static void StartLoop()
        {
            var graphics = new Graphics(120, 30);
            var engine = new GraphicsEngineASCII(graphics);

            var ship = new Ship(engine);
            ship.Draw();
            ship.Position.X = graphics.Width / 2;
            ship.Position.Y = graphics.Height - ship.Size.Height;
            var legend = new Legend(engine);
            legend.Draw(ship);

            engine.Update = (pressedKey) =>
            {
                if (pressedKey == ConsoleKey.Escape) { engine.EngineRunning = false; }
                graphics.Clear();

                legend.Draw(ship);
                ship.Draw(pressedKey);

                Render(graphics, engine);
            };

            engine.Start();
        }

        private static void Render(IGraphics<string> graphics, IGraphicsEngine<string> engine)
        {
            System.Console.CursorVisible = false;
            System.Console.SetCursorPosition(0, 0);
            engine.Flush((string output, Color color) =>
            {
                VTConsole.Write(output, color);
            });
        }
    }

}
