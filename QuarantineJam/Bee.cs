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
        private Vector2 brownianPosition, brownianVelocity; // position et vitesse relative, pour faire un mouvement brownien
        static Random random = new Random();
        private float BeeBounceFactor { get; } = 0.5f;
        private static Vector2 beeHurtboxSize { get; } = new Vector2(42, 33);
        public Bee(Vector2 FeetPosition) : base(beeHurtboxSize, FeetPosition)
        {
            brownianPosition = new Vector2(0, 0);
            CurrentSprite = new Sprite(bee);
            AirFriction = 0.95f;
        }

        public Bee(Vector2 FeetPosition, Vector2 Speed) : base(beeHurtboxSize, FeetPosition)
        {
            brownianPosition = new Vector2(0, 0);
            CurrentSprite = new Sprite(bee);
            AirFriction = 0.95f;
            Velocity = Speed;
        }

        public override void Update(GameTime gameTime, World world, Player player)
        {
            foreach (PhysicalObject p in world.Stuff)
            {
                if (p != this && p is Bee b && (FeetPosition - b.FeetPosition).Length() <= 50)
                {
                    //Console.WriteLine("collision between two bees");
                    Vector2 distance = FeetPosition - b.FeetPosition;
                    if (distance == Vector2.Zero) distance = new Vector2(1, 0);
                    Vector2 bounce = BeeBounceFactor * distance / (distance.Length() * distance.Length());
                    ApplyForce(bounce);
                    b.ApplyForce(-bounce);
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
            Vector2 distance = (player.FeetPosition - new Vector2(0, player.HurtboxSize.Y / 2)) - (FeetPosition - new Vector2(0, HurtboxSize.Y / 2));
            if ((distance).Length() <= 200)
            {

                ApplyForce(distance/distance.Length()/distance.Length() * 40);
            }
        }
    }
}
