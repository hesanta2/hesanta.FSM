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
        private static bool colored = true;

        private static IGraphics<string> graphics;
        private static IGraphicsEngine<string> engine;
        private static Legend legend;
        private static Ship ship;
        private static Enemies enemies;

        static void Main(string[] args)
        {
            StartLoop();
        }

        private static void StartLoop()
        {
            graphics = new Graphics(120, 30);
            engine = new GraphicsEngineASCII(graphics);

            InitEngineObjects();

            engine.Update = (pressedKey) =>
            {
                ProcessKeys(pressedKey);
                if (pressedKey == ConsoleKey.Escape) { engine.EngineRunning = false; }
                graphics.Clear();

                ship.Draw(pressedKey);
                enemies.Draw();
                legend.Draw(colored, ship, enemies);

                Render(graphics, engine);

                if (ship.Destroyed || !enemies.EnemiesList.Any( e => !e.Destroyed) || enemies.Position.Y == ship.Position.Y)
                {
                    InitEngineObjects();
                }
            };

            engine.Start();
        }

        private static void InitEngineObjects()
        {
            engine.Reset();
            ship = new Ship(engine);
            ship.Draw();
            ship.Position.X = graphics.Width / 2;
            ship.Position.Y = graphics.Height - ship.Size.Height;

            legend = new Legend(engine);
            legend.Draw(colored, ship);

            enemies = new Enemies(engine, ship);
            enemies.Draw();
            enemies.Position.X = 40;
            enemies.Position.Y = 2;
            enemies.Initiated = true;
        }

        private static void Render(IGraphics<string> graphics, IGraphicsEngine<string> engine)
        {
            System.Console.CursorVisible = false;
            System.Console.SetCursorPosition(0, 0);
            if (colored)
            {
                engine.Flush((string output, Color color) =>
                {
                    VTConsole.Write(output, color);
                });
            }
            else
            {
                VTConsole.Write(graphics.Output, Color.White);
            }
        }

        private static void ProcessKeys(ConsoleKey? key)
        {
            if (!colored)
            {
                colored = key == ConsoleKey.C;
            }
            else
            {
                colored = !(key == ConsoleKey.B);
            }
        }
    }

}
