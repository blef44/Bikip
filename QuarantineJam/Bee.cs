using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bikip
{
    public class Bee : PhysicalObject
    {
        private Vector2 brownianPosition, brownianVelocity; // position et vitesse relative, pour faire un mouvement brownien
        static Random random = new Random();
        private float BeeBounceFactor { get; } = 0.5f;
        private static Vector2 beeHurtboxSize { get; } = new Vector2(42, 33);
        public Bee(Vector2 FeetPosition) : base(beeHurtboxSize, FeetPosition)
        {
            brownianPosition = new Vector2(0, 0);
            CurrentSprite = new Sprite(bee);
            AirFriction = 0.95f;
            GroundFriction = 1;
            XTreshold = 0;
        }

        public Bee(Vector2 FeetPosition, Vector2 Speed) : base(beeHurtboxSize, FeetPosition)
        {
            brownianPosition = new Vector2(0, 0);
            CurrentSprite = new Sprite(bee);
            AirFriction = 0.95f;
            GroundFriction = 1;
            Velocity = Speed;
            XTreshold = 0;
        }

        public override void Update(GameTime gameTime, World world, Player player)
        {
            if (!world.Bounds.Contains(Hurtbox)) ApplyForce(-2 * Vector2.Normalize(Hurtbox.Center.ToVector2() - world.Bounds.Center.ToVector2()));
            else foreach (PhysicalObject p in world.Stuff)
            {
                Vector2 distance = FeetPosition - p.FeetPosition;
                if (p != this && p is Bee && distance.Length() <= 50 && Velocity.Length() < 5)
                {
                    //Console.WriteLine("collision between two bees");
                    if (distance.Length() <= 0.0001f) distance = new Vector2(random.Next(0, 2) * 2 - 1, random.Next(0, 2) * 2 - 1);
                    Vector2 bounce = BeeBounceFactor * distance / (distance.Length() * distance.Length());
                    ApplyForce(bounce);
                    p.ApplyForce(-bounce);
                }
            }

            Velocity.Y *= AirFriction;
            CurrentSprite.UpdateFrame(gameTime);
            // mouvement brownien
            brownianVelocity += new Vector2((float)random.Next(-5, 5) / 200, (float)random.Next(-5, 5) / 200) - 0.002f * brownianPosition;
            brownianPosition += brownianVelocity;

            base.Update(gameTime, world, player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentSprite.DrawFromFeet(spriteBatch, FeetPosition + brownianPosition);
        }

        public void AttractFromPlayer(Player player)
        {
            Vector2 distance = player.Hurtbox.Center.ToVector2() - Hurtbox.Center.ToVector2() ;
            if ((distance).Length() <= 200)
            {
                ApplyForce(distance/distance.Length()/distance.Length() * 40);
            }
        }
    }
}
