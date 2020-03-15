using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;


namespace QuarantineJam
{
    public class PhysicalObject
    {
        internal static int IDcount = 0;
        internal int ID, lifetime, push_force = 15;
        internal Vector2 Velocity;
        internal Vector2 FeetPosition;
        internal Rectangle Hurtbox;
        internal Vector2 HurtboxSize;
        internal Sprite PreviousSprite, CurrentSprite;
        public static Sprite bee, ruche1, ruche2, ruche3;
        public static SoundEffect bee_collected, jump;
        internal int SpriteFrames = 0;
        internal static Random r = new Random();
        internal float WallBounceFactor, GroundBounceFactor = 0f, GroundFriction, AirFriction = 1f, Gravity, XTreshold = 0.1f;
        internal bool is_particle = false, wallcollision = false, groundcollision = false, push_player = false, is_solid = false, player_hit = false;

        public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            bee = new Sprite(4, 69, 59, 100, Content.Load<Texture2D>("bee"));
            ruche1 = new Sprite(Content.Load<Texture2D>("ruche1"));
            ruche2 = new Sprite(Content.Load<Texture2D>("ruche2"));
            ruche3 = new Sprite(Content.Load<Texture2D>("ruche3"));
            bee_collected = Content.Load<SoundEffect>("SUCCESS BEEPS Multi Echo Short 02");
            jump = Content.Load<SoundEffect>("highUp");
        }
        public PhysicalObject(Vector2 HurtboxSize, Vector2 FeetPosition, bool isParticle = false)
        {
            is_particle = isParticle;
            IDcount += 1;
            ID = IDcount;
            lifetime = 0;

            this.HurtboxSize = HurtboxSize;
            this.FeetPosition = FeetPosition;

            UpdateHurtbox();
        }

        public void UpdateHurtbox()
        {
            Hurtbox.X = (int)Math.Floor(FeetPosition.X - HurtboxSize.X / 2);
            Hurtbox.Width = (int)Math.Floor(HurtboxSize.X);
            Hurtbox.Y = (int)Math.Floor(FeetPosition.Y - HurtboxSize.Y);
            Hurtbox.Height = (int)Math.Floor(HurtboxSize.Y);
        }

        public virtual void Update(GameTime gameTime, World world, Player player)
        {
            lifetime += 1;

            if (PreviousSprite == CurrentSprite)
                SpriteFrames += 1;
            else SpriteFrames = 0;

            PreviousSprite = CurrentSprite;

            if (IsOnGround(world)) Velocity.X *= GroundFriction;
            else Velocity.X *= AirFriction;

            ApplyForce(new Vector2(0, Gravity));

            if (Velocity.X * Velocity.X < XTreshold) Velocity.X = 0;
            if (Velocity.Y * Velocity.Y < XTreshold) Velocity.Y = 0; // if you remove this bullet will fall for some reason

            CheckCollisions(world);

        }

        public virtual void CheckCollisions(World world)
        {
            Vector2 IntVelocity = Velocity;
            if (IntVelocity.Y > 0) IntVelocity.Y = (float)Math.Ceiling(IntVelocity.Y);
            if (IntVelocity.X > 0) IntVelocity.X = (float)Math.Ceiling(IntVelocity.X);
            if (IntVelocity.Y < 0) IntVelocity.Y = (float)Math.Floor(IntVelocity.Y);
            if (IntVelocity.X < 0) IntVelocity.X = (float)Math.Floor(IntVelocity.X);
                groundcollision = false;

            if (!groundcollision) if (world.CheckCollision(Hurtbox, new Vector2(0, IntVelocity.Y))) // Check collision with the world
                {
                    groundcollision = true;
                    // Should Offset hitbox to ground ?
                }
            if (groundcollision) Velocity.Y = -GroundBounceFactor * Velocity.Y;
            FeetPosition.Y += Velocity.Y; // Apply Y movement
            Hurtbox.Y = (int)Math.Floor(FeetPosition.Y - HurtboxSize.Y);
            Hurtbox.Height = (int)Math.Floor(HurtboxSize.Y);

            wallcollision = false;
            if (world.CheckCollision(Hurtbox, new Vector2(IntVelocity.X, 0)))// Collision X with the world
                {
                    wallcollision = true;
                }
            if(wallcollision) Velocity.X = -WallBounceFactor * Velocity.X;

            FeetPosition.X += Velocity.X;
            Hurtbox.X = (int)Math.Floor(FeetPosition.X - HurtboxSize.X / 2);
            Hurtbox.Width = (int)Math.Floor(HurtboxSize.X);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            CurrentSprite.DrawFromFeet(spriteBatch, new Vector2(FeetPosition.X, FeetPosition.Y + 1));
        }
        public void ApplyForce(Vector2 force)
        {
            Velocity += force;
        }
        public bool IsOnGround(World world)
        {
            if (Velocity.Y >= 0)
            {
                if (world.CheckCollision(Hurtbox, new Vector2(0, Velocity.Y + 2))) return true;
                else
                {
                    return false; // if no object below feets return false
                }
            }
            else return false;
        }

        public bool IsOnWorldGround(World world)
        {
            if (Velocity.Y >= 0)
            {
                return (world.CheckCollision(Hurtbox, new Vector2(0, Velocity.Y + 2))) ;
            }
            else return false;
        }

        public bool CheckCollision(Rectangle OtherObjectHurtbox, Vector2 OtherObjectVelocity)
        {
            Rectangle moved_rectangle = OtherObjectHurtbox;
            moved_rectangle.Offset(OtherObjectVelocity);
            return moved_rectangle.Intersects(Hurtbox);
        }

        public void Bump(PhysicalObject bumper)
        {
            Vector2 Knockback = new Vector2();
            Knockback = new Vector2(Hurtbox.Center.X - bumper.Hurtbox.Center.X, Math.Min(Hurtbox.Center.Y, bumper.Hurtbox.Center.Y) - bumper.Hurtbox.Center.Y);
            if (Knockback == new Vector2(0, 0)) Knockback = new Vector2(0, -1);
            Knockback.Normalize();
            Knockback *= 10;
            //if (!world.CheckCollision(Hurtbox, Knockback)) 
                Velocity = Knockback;
        }

        public void SpawnDebris(Sprite sprite,Vector2 Direction, int Quantity = 3, float Gravity = 1.1f, int DisplayFrames = 40)
        {
            //Console.WriteLine("Juice launched " + Convert.ToString(Direction));
            float Force = Direction.Length();
            if (Force == 0) Force = 25;
            for (int i = 0; i < Quantity; i++)
            {
                Vector2 ProjectionDirection = new Vector2();
                if (Direction == new Vector2(0, 0)) ProjectionDirection = new Vector2(r.Next(-6, 7), r.Next(-15, 5));
                else
                {
                    ProjectionDirection = Direction;
                    ProjectionDirection.X += (float)(r.NextDouble() - 0.5d) * Force * 0.7f;
                    ProjectionDirection.Y += (float)(r.NextDouble() - 1d) * Force *0.6f; ;
                }
                ProjectionDirection.Normalize();
            }
        }

    }
}