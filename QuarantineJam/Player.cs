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
    public class Player : PhysicalObject
    {
        private const float MaxSpeed = 11;
        static Sprite idle, run, brake, fall, rise, roll, top, flip;
        //static SoundEffect ;
        public enum PlayerState { idle, walk, jump, doublejump, flip } //etc
        public PlayerState CurrentState, PreviousState;
        public int state_frames;
        private List<Rectangle> LandingGroundCandidate;
        World world;
        Random random = new Random();

        KeyboardState prevKbState;

        public int PlayerDirection;

        new public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            idle = new Sprite(2, 193, 168, 350, Content.Load<Texture2D>("player_idle"));
            run = new Sprite(5, 193, 168, 55, Content.Load<Texture2D>("player_run"));
            brake = new Sprite(Content.Load<Texture2D>("brake"));
            rise = new Sprite(Content.Load<Texture2D>("rise"));
            fall = new Sprite(Content.Load<Texture2D>("fall"));
            top = new Sprite(Content.Load<Texture2D>("top"));
            roll = new Sprite(6, 232, 168, 50, Content.Load<Texture2D>("roll"));
            flip = new Sprite(14, 193, 239, 120, Content.Load<Texture2D>("flip"), 1, false);
        }
        public Player():base(new Vector2(40, 100), new Vector2(0,0))
        {
            FeetPosition = new Vector2(200, 200);
            CurrentState = PlayerState.idle;
            WallBounceFactor = 0f;
            GroundBounceFactor = 0f;
            GroundFriction = 0.85f;
            Gravity = 0.5f;
            AirFriction = 0.8f;

            Velocity = new Vector2(0, 0);
            PlayerDirection = 1;
            prevKbState = new KeyboardState();
        }

        public override void Update(GameTime gameTime, World world, Player player)
        {
            this.world = world;
            KeyboardState KbState = Keyboard.GetState();

            if (PreviousState == CurrentState) state_frames += 1;
            else state_frames = 0;
            PreviousState = CurrentState;

            switch (CurrentState)
            {
                case PlayerState.idle:
                    /*if (KbState.IsKeyDown(Input.Left) && KbState.IsKeyUp(Input.Right))
                    {
                        if (Velocity.X > -MaxSpeed)
                            ApplyForce(new Vector2(-2f, 0));
                        CurrentState = PlayerState.walk;
                    } else if (KbState.IsKeyDown(Input.Right))
                    {
                        if (Velocity.X < MaxSpeed)
                            ApplyForce(new Vector2(2f, 0));
                        CurrentState = PlayerState.walk;
                    }*/
                    if (!IsOnGround(world)) CurrentState = PlayerState.jump;
                    else if (Input.direction != 0) CurrentState = PlayerState.walk;
                    else if (KbState.IsKeyDown(Input.Jump))
                    {
                        ApplyForce(new Vector2(0, -15f));
                        SoundEffectPlayer.Play(jump, 1f, 0f);
                        if (Input.direction != 0) PlayerDirection = Input.direction;
                        CurrentState = PlayerState.jump;
                    }
                    break;
                case PlayerState.walk:
                    if (KbState.IsKeyDown(Input.Jump))
                    {
                        // Velocity = 0;
                        if (Input.direction != 0) PlayerDirection = Input.direction;
                        ApplyForce(new Vector2(0, -15f));
                        SoundEffectPlayer.Play(jump, 1f, 0f);
                        CurrentState = PlayerState.jump;
                    }
                    else if (!IsOnGround(world)) CurrentState = PlayerState.jump;
                    else if (Input.direction != 0) // player is inputing a direction (either left or right)
                    {
                        PlayerDirection = Input.direction;
                        if (Math.Sign(Velocity.X) * Math.Sign(Input.direction) >= 0) // if inputed direction is the same as current movement direction
                        {
                            if (Velocity.X * Velocity.X < MaxSpeed * MaxSpeed) // if norm of velocity below max speed
                                ApplyForce(new Vector2(Input.direction * 5f, 0));
                        }
                        else // if player is inputing the direction against the current movement (brake)
                            ApplyForce(new Vector2(Input.direction * 5f, 0));
                    }
                    else CurrentState = PlayerState.idle;
                    break;

                case PlayerState.jump:
                    if (Velocity.Y < 0 && Velocity.X != 0) PlayerDirection = Math.Sign(Velocity.X); // raising
                    if (KbState.IsKeyDown(Input.Jump) && !prevKbState.IsKeyDown(Input.Jump))
                    {
                        Velocity.Y = 0;
                        //if (Input.direction == 0) 
                        SoundEffectPlayer.Play(jump, 1f, 0.5f);
                            ApplyForce(new Vector2(0, -15));
                        //else
                        //{
                          //  PlayerDirection = Input.direction;
                          //  ApplyForce(new Vector2(Input.direction * 50, -8));
                      //  }
                        CurrentState = PlayerState.doublejump;
                    }
                    else if (Input.direction != 0) // player is inputing a direction (either left or right)
                    {
                        if ((Velocity.X + Input.direction) * Math.Sign(Velocity.X) <= MaxSpeed) ApplyForce(new Vector2(Input.direction * 5f, 0));
                    }
                    if (IsOnGround(world))
                        CurrentState = PlayerState.idle;
                    break;
                case (PlayerState.doublejump):
                {
                        if (Velocity.Y < 0 && Velocity.X != 0) PlayerDirection = Math.Sign(Velocity.X); // raising

                        if (IsOnGround(world)) CurrentState = PlayerState.idle;
                        else if (Input.direction != 0) // player is inputing a direction (either left or right)
                        {
                            if ((Velocity.X + Input.direction) * Math.Sign(Velocity.X) <= MaxSpeed) ApplyForce(new Vector2(Input.direction * 5f, 0));
                        }
                        break;
                }
                case (PlayerState.flip):
                    {
                        if (CurrentSprite.isOver && state_frames > 200)
                        {
                            world.NextLevel(this);//do something like next level;
                            CurrentState = PlayerState.idle;
                        }
                        break;
                    }
            }
            //
            // SPRITE DETERMINATION
            //
            PreviousSprite = CurrentSprite;
            switch (CurrentState)
            {
                case (PlayerState.idle):
                    {
                        if (Velocity.X * Velocity.X < 1) CurrentSprite = idle;
                        else CurrentSprite = brake;
                        break;
                    }
                case (PlayerState.walk):
                    {
                        CurrentSprite = run;
                        break;
                    }
                case (PlayerState.jump):
                    {
                        if (Velocity.Y < -1) CurrentSprite = rise;
                        else if (Velocity.Y > 1) CurrentSprite = fall;
                        else CurrentSprite = top;
                        break;
                    }
                case (PlayerState.doublejump):
                    {
                        if (Velocity.Y < 3) CurrentSprite = roll;
                        else CurrentSprite = fall;
                        break;
                    }
                case (PlayerState.flip):
                    {
                        CurrentSprite = flip;
                        break;
                    }
            }

            /*if (Input.double_tap_waiting && IsOnGround(world) && CurrentSprite != slug_walk)
            {
                CurrentSprite = slug_charge_attack;
                PlayerDirection = Input.direction;
            }*/

            if (CurrentSprite != PreviousSprite)
            {
                CurrentSprite.ResetAnimation();
                //Console.WriteLine("Switched to sprite " + CurrentSprite.Texture.Name);
            }
            CurrentSprite.direction = PlayerDirection;
            CurrentSprite.UpdateFrame(gameTime);

            foreach (PhysicalObject o in world.Stuff)
            {
                if (o is Bee b)
                {
                    b.AttractFromPlayer(this);
                    if (Hurtbox.Intersects(b.Hurtbox))
                    {
                        //Console.WriteLine("collision between bee and player");
                        world.RemovedStuff.Add(b);
                        SoundEffectPlayer.Play(bee_collected, 0.08f, (float)(r.NextDouble() * 0.2));
                    }
                }
                // do something;
            }

            prevKbState = KbState;
            base.Update(gameTime, world, this);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void CheckCollisions(World world)
        {
            
            Vector2 IntVelocity = Velocity;
            if (IntVelocity.Y > 0) IntVelocity.Y = (float)Math.Ceiling(IntVelocity.Y);
            if (IntVelocity.X > 0) IntVelocity.X = (float)Math.Ceiling(IntVelocity.X);
            if (IntVelocity.Y < 0) IntVelocity.Y = (float)Math.Floor(IntVelocity.Y);
            if (IntVelocity.X < 0) IntVelocity.X = (float)Math.Floor(IntVelocity.X);
            groundcollision = false;


           if (world.CheckCollision(Hurtbox, new Vector2(0, IntVelocity.Y))) // Check collision with the world
            {
                groundcollision = true;
                LandingGroundCandidate = world.CheckCollisionReturnRectangleList(Hurtbox, new Vector2(0, IntVelocity.Y));
                
            }
            if (groundcollision)
            {
                Velocity.Y = 0;
                if(IntVelocity.Y > 0) // if falling
                {
                    Rectangle HigherTopRectangle = LandingGroundCandidate[0];
                    foreach (Rectangle r in LandingGroundCandidate) if (r.Top < HigherTopRectangle.Top) HigherTopRectangle = r;
                    if (!world.CheckCollision(Hurtbox, new Vector2(0, HigherTopRectangle.Top - FeetPosition.Y))) FeetPosition.Y += HigherTopRectangle.Y - FeetPosition.Y;

                }
            }
            FeetPosition.Y += Velocity.Y; // Apply Y movement
            Hurtbox.Y = (int)Math.Floor(FeetPosition.Y - HurtboxSize.Y);
            Hurtbox.Height = (int)Math.Floor(HurtboxSize.Y);

            wallcollision = false;
            if (world.CheckCollision(Hurtbox, new Vector2(IntVelocity.X, 0)))// Collision X with the world
            {
                wallcollision = true;
            }
            if (wallcollision) Velocity.X = -WallBounceFactor * Velocity.X;

            FeetPosition.X += Velocity.X;
            Hurtbox.X = (int)Math.Floor(FeetPosition.X - HurtboxSize.X / 2);
            Hurtbox.Width = (int)Math.Floor(HurtboxSize.X);
            //base.CheckCollisions(world);
        }

        private void Death()
        {

        }
    }
}