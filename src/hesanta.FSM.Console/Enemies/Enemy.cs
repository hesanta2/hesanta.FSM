using hesanta.Drawing.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace hesanta.FSM.Sample
{
    public class Enemy : EngineObject<string>
    {
        private string enemyString = $@"   
/òó\
 \/
";
        private readonly Ship ship;
        private float bulletVelocity = 10;
        private bool destroyInitiated = false;

        public IFSM FSM { get; set; }
        public bool Destroyed { get; set; } = false;
        public bool Destroying { get; set; } = false;
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
            if (!Destroying && !destroyInitiated)
            {
                DrawString(enemyString, new SolidBrush(Color.GreenYellow), new PointF(Position.X, Position.Y + 1));
                spriteCount = 0;
            }
            else
            {
                if (destroyStopwatch == null)
                {
                    destroyStopwatch = new Stopwatch();
                    destroyStopwatch.Start();
                }
                DrawString(GetDrawDestroyingString(), new SolidBrush(Color.Orange), new PointF(Position.X - 4, Position.Y));
            }
            Bullet.Draw();
        }

        public void Destroy()
        {
            if (!destroyInitiated)
            {
                Destroying = true;
                destroyInitiated = true;
            }
        }

        private string[] destroySprites = { $@"
           
           
     *     
           
           
          
", $@"
           
   \   /  
  ─  *  ─  
   /   \  
           
", $@"
  \     /
   \   /
───     ───
   /   \
  /     \
" };
        private Stopwatch destroyStopwatch;
        private int spriteCount = 0;
        private string GetDrawDestroyingString()
        {
            if (destroyStopwatch.ElapsedMilliseconds > 50)
            {
                destroyStopwatch.Restart();
                if (spriteCount < destroySprites.Length - 1)
                {
                    spriteCount++;
                }
                else
                {
                    Destroying = false;
                }
            }

            return destroySprites[spriteCount];
        }
    }
}
