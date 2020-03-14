using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuarantineJam
{
    public class Bee : PhysicalObject
    {
        private float BeeBounceFactor { get; } = 0.5f;
        private static Vector2 beeHurtboxSize { get; } = new Vector2(42, 33);
        static Sprite idle;
        public Bee(Vector2 FeetPosition) : base(beeHurtboxSize, FeetPosition)
        {
            CurrentSprite = new Sprite(bee);
        }

        public override void Update(GameTime gameTime, World world)
        {
            foreach (PhysicalObject p in world.Stuff)
            {
                if (p != this && p is Bee b && (FeetPosition - b.FeetPosition).Length() <= 50)
                {
                    Console.WriteLine("collision between two bees");
                    Vector2 distance = FeetPosition - b.FeetPosition;
                    Velocity = BeeBounceFactor * Vector2.Normalize(distance);
                    b.Velocity = -BeeBounceFactor * Vector2.Normalize(distance);
                }
            }
            CurrentSprite.UpdateFrame(gameTime);
            base.Update(gameTime, world);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            idle.DrawFromFeet(spriteBatch, FeetPosition);
        }
    }
}
