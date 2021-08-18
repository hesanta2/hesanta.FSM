using hesanta.FSM.States;
using hesanta.FSM.Transitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace hesanta.FSM.Sample
{
    partial class Program
    {
        private static float velocity = 50;
        private static FSM fsm;
        private static string buffer;
        private static ConsoleKeyInfo lastKey = new ConsoleKeyInfo();
        private static Stopwatch stopwatch = new Stopwatch();
        private static Stopwatch stopwatchDelta = new Stopwatch();
        private static float deltaTime = 0.0001f;
        private static int frames;
        private static int currentFps = 0;
        private static int direction = 1;

        static void Main(string[] args)
        {
            fsm = BuildFSM();
            fsm.RunAsync();

            StartLoop();
        }

        private static void StartLoop()
        {
            Clear();
            lastKey = new ConsoleKeyInfo();
            stopwatch.Start();
            while (lastKey.Key != ConsoleKey.Escape)
            {
                Console.CursorVisible = false;
                stopwatchDelta.Restart();
                Clear();
                RenderLegend();
                Render();

                Keys();

                CalculateDeltaAndFrames();
                if (!Console.KeyAvailable) continue;
                lastKey = Console.ReadKey(true);
            }

            Console.Clear();
        }

        private static void Keys()
        {
            y = lastKey.Key == ConsoleKey.UpArrow ? --y : y;
            y = lastKey.Key == ConsoleKey.DownArrow ? ++y : y;
            lastKey = new ConsoleKeyInfo();
        }

        private static void CalculateDeltaAndFrames()
        {
            deltaTime = (float)(stopwatchDelta.Elapsed.TotalMilliseconds / 1000);
            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                currentFps = frames;
                frames = 0;
                stopwatch.Restart();
            }
            frames++;
        }

        private static void RenderLegend()
        {
            Draw(0, 0, $"FPS: {currentFps}");
            Draw(0, 1, $"DeltaTime: {deltaTime}");
            Draw(0, 2, $"Velocity: {velocity}");
            Draw(0, 3, $"X: {x}");
            Draw(0, 4, $"***FSM***");
            Draw(0, 5, $"CurrentState: {fsm.CurrentState}");
        }

        private static void Clear()
        {
            buffer = new string(' ', Console.WindowHeight * Console.WindowWidth);
        }

        private static float x = 0;
        private static float y = 0;
        private static void Render()
        {
            x += velocity * deltaTime * direction;
            Draw(x, 10 + y, "0");

            Console.SetCursorPosition(0, 0);
            Console.Write(buffer);
        }

        private static FSM BuildFSM()
        {
            var fsm = new FSM();

            var rightState = new StartState("Right", () =>
             {
                 direction = 1;
             });
            var leftState = new State("Left", () =>
             {
                 direction = -1;
             });

            var rightTransition = new Transition(rightState, leftState, () =>
            {
                if (x < 100) { return null; }

                return leftState;
            });
            var leftTransition = new Transition(leftState, rightState, () =>
            {
                if (x > 0) { return null; }

                return rightState;
            });

            fsm.AddTransitions(leftTransition, rightTransition);
            return fsm;
        }

        public static void Draw(float x, float y, string text)
        {
            var yOffset = y * Console.WindowWidth;
            var xOffset = x;
            var position = xOffset + yOffset;

            buffer = buffer.Remove((int)position, text.Length);
            buffer = buffer.Insert((int)position, text);
        }
    }

}
