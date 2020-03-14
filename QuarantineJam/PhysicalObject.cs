using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;


namespace MaskGame
{
    public class PhysicalObject
    {
        internal static int IDcount = 0;
        internal int ID, lifetime, push_force = 15;
        internal static Sprite JuiceBoxSprite, JuiceBoxBrokenSprite, verre_debris, TubeSprite, BigTubeSprite, FleurSprite, FleurBumpSprite, PollenSprite, FlagSprite, FissureSprite, HighScoreMachineSprite;
        internal static SoundEffect BrokenGlass1, BrokenGlass2, BrokenGlass3, FlowerBump, RespawnSound;
        internal static Texture2D dontgohere;
        internal Vector2 Velocity;
        internal Vector2 FeetPosition;
        internal Rectangle Hurtbox;
        internal Vector2 HurtboxSize;
        internal Room ObjectRoom;
        internal Sprite PreviousSprite, CurrentSprite;
        internal int SpriteFrames = 0;
        internal static Random r = new Random();
        internal float WallBounceFactor, GroundBounceFactor = 0f, GroundFactor, Gravity, XTreshold = 0.1f;
        internal bool is_particle = false, wallcollision = false, groundcollision = false, push_player = false, is_solid = false, player_hit = false;

        public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            JuiceBoxSprite = new Sprite(9, 53, 70, 120, Content.Load<Texture2D>("caisse"));
            FlagSprite = new Sprite(11, 58, 80, 100, Content.Load<Texture2D>("flag"));
            TubeSprite = new Sprite(Content.Load<Texture2D>("tube1"));
            BigTubeSprite = new Sprite(Content.Load<Texture2D>("tube2"));
            FleurSprite = new Sprite(Content.Load<Texture2D>("flower"));
            FleurBumpSprite = new Sprite(4, 125, 83, 100, Content.Load<Texture2D>("flower_bump"), 1,false);
            JuiceBoxBrokenSprite = new Sprite(Content.Load<Texture2D>("broken_caisse"));
            JuiceBoxBrokenSprite.opacity = 0.5f;
            BrokenGlass1 = Content.Load<SoundEffect>("verrecasse1");
            FlowerBump = Content.Load<SoundEffect>("SUCCESS PICKUP Collect Chime 01");
            verre_debris = new Sprite(Content.Load<Texture2D>("verre_debris"));
            PollenSprite = new Sprite(Content.Load<Texture2D>("flower_particle"));
            FissureSprite = new Sprite(Content.Load<Texture2D>("fissure"));
            HighScoreMachineSprite = new Sprite(2, 123, 217, 500, Content.Load<Texture2D>("highscore_machine"));
            dontgohere = Content.Load<Texture2D>("texture/dontgohere");
            RespawnSound = Content.Load<SoundEffect>("respawn");
        }
        public PhysicalObject(Room SpawnRoom, Vector2 HurtboxSize, Vector2 FeetPosition, bool isParticle = false)
        {
            is_particle = isParticle;
            ObjectRoom = SpawnRoom;
            IDcount += 1;
            ID = IDcount;
            lifetime = 0;

            this.HurtboxSize = HurtboxSize;
            this.FeetPosition = FeetPosition;

            UpdateHurtbox();
           // Console.WriteLine("PhysicalObject construtor of object " + ID.ToString() + " FPosition : " + FeetPosition.ToString() + " The body is " + Hurtbox.ToString());
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

            if (IsOnGround(world)) Velocity.X *= GroundFactor;

            ApplyForce(new Vector2(0, Gravity));

            if (Velocity.X * Velocity.X < XTreshold) Velocity.X = 0;
            if (Velocity.Y * Velocity.Y < XTreshold) Velocity.Y = 0; // if you remove this bullet will fall for some reason

            CheckCollisions(world);

            if (!is_particle) CheckForRoomChange();
        }

        public virtual void CheckCollisions(World world)
        {
            Vector2 IntVelocity = Velocity;
            if (IntVelocity.Y > 0) IntVelocity.Y = (float)Math.Ceiling(IntVelocity.Y);
            if (IntVelocity.X > 0) IntVelocity.X = (float)Math.Ceiling(IntVelocity.X);
            if (IntVelocity.Y < 0) IntVelocity.Y = (float)Math.Floor(IntVelocity.Y);
            if (IntVelocity.X < 0) IntVelocity.X = (float)Math.Floor(IntVelocity.X);
                groundcollision = false;
            foreach (Room r in world.Rooms) foreach (PhysicalObject o in r.Stuff) if (!groundcollision) if (o.is_solid && o != this) // Collision with other objects
                            if (o.CheckCollision(Hurtbox, new Vector2(0, IntVelocity.Y)))
                            {
                                groundcollision = true;
                            }
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
            foreach (Room r in world.Rooms) foreach (PhysicalObject o in r.Stuff) if (!wallcollision) if (o.is_solid && o != this) // Collision X with other objects
                            if (o.CheckCollision(Hurtbox, new Vector2(IntVelocity.X, 0)))
                            {
                                wallcollision = true;
                            }
            if (!wallcollision) if (world.CheckCollision(Hurtbox, new Vector2(IntVelocity.X, 0)))// Collision X with the world
                {
                    wallcollision = true;
                }
            if(wallcollision) Velocity.X = -WallBounceFactor * Velocity.X;

            FeetPosition.X += Velocity.X;
            Hurtbox.X = (int)Math.Floor(FeetPosition.X - HurtboxSize.X / 2);
            Hurtbox.Width = (int)Math.Floor(HurtboxSize.X);
        }

        public virtual void CheckForRoomChange()
        {
            if (FeetPosition.X > ObjectRoom.Exit.X)
            {
                //if (!(this is JuiceParticle)) Console.WriteLine("Object reached exit from his room " + this.ToString() + " " + this.ID);
                ObjectRoom.RemovedStuff.Add(this);
                if (ObjectRoom.NextRoom == null)
                {
                    ObjectRoom.StuffToAddNextRoom.Add(this);
                    ObjectRoom = null;
                }
                else
                {
                    ObjectRoom.NextRoom.NewStuff.Add(this);
                    ObjectRoom = ObjectRoom.NextRoom;
                }

            }
            else if (FeetPosition.X < ObjectRoom.Entrance.X) // else normalement inutile
            {
                //if (!(this is JuiceParticle)) Console.WriteLine("Object reached entrance from his room " + this.ToString() + " " + this.ID);
                if (ObjectRoom.PreviousRoom != null)
                {
                    ObjectRoom.RemovedStuff.Add(this);
                    ObjectRoom.PreviousRoom.NewStuff.Add(this);
                    ObjectRoom = ObjectRoom.PreviousRoom;
                }
            }
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
                    foreach (Room r in world.Rooms) foreach (PhysicalObject o in r.Stuff) if (o.is_solid && o != this) // Collision with other objects
                                if (o.CheckCollision(Hurtbox, new Vector2(0, Velocity.Y + 2))) return true;
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

        public void SpawnJuice(float Quantity, Vector2 Direction)
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
                    //ProjectionDirection = Direction + new Vector2(Direction.Y / Direction.Length() * r.Next(-2,3), Direction.X/Direction.Length() * r.Next(-2, 3)) ;
                    /*ProjectionDirection.Normalize();
                    
                ProjectionDirection.X += (float)(r.NextDouble() - 0.5d) * Math.Abs(ProjectionDirection.Y);
                    ProjectionDirection.Y += (float)(r.NextDouble() - 1d) * Math.Abs(ProjectionDirection.X);*/
                    ProjectionDirection.X += (float)(r.NextDouble() - 0.5d) * Force / 2;
                    ProjectionDirection.Y += (float)(r.NextDouble() - 1d) * Force / 3; ;
                }
                ProjectionDirection.Normalize();
                //Console.WriteLine("Juice BIT launched " + Convert.ToString(Force * ProjectionDirection));
                ObjectRoom.NewStuff.Add(new JuiceParticle(Hurtbox.Center.ToVector2(), Force * ProjectionDirection, ObjectRoom, 1));
            }
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
                ObjectRoom.Particles.Add(new Debris(Hurtbox.Center.ToVector2(), this.ObjectRoom, sprite, Force * ProjectionDirection, Gravity, DisplayFrames));
            }
        }

    }
}