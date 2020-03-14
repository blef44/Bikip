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
        static Sprite idle;
        //static SoundEffect ;
        public enum PlayerState { idle, walk, jump } //etc
        public PlayerState CurrentState, PreviousState;
        public int state_frames;
        World world;
        Random random = new Random();

        public int PlayerDirection;

        new public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            idle = new Sprite(2, 193, 168, 350, Content.Load<Texture2D>("player_idle"));
        }
        public Player():base(new Vector2(50, 50), new Vector2(0,0))
        {
            FeetPosition = new Vector2(200, 200);
            CurrentState = PlayerState.idle;
            WallBounceFactor = 0f;
            GroundBounceFactor = 0f;
            GroundFactor = 0.95f;
            Gravity = 0.5f;

            Velocity = new Vector2(0, 0);
            PlayerDirection = 1;
        }

        public override void Update(GameTime gameTime, World world)
        {
            this.world = world;

            CurrentSprite = idle;

            if (PreviousState == CurrentState) state_frames += 1;
            else state_frames = 0;
            PreviousState = CurrentState;

            switch (CurrentState)
            {
                
            }
            //
            // SPRITE DETERMINATION
            //
            PreviousSprite = CurrentSprite;
            switch (CurrentState)
            {

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

            foreach (PhysicalObject o in world.Stuff) ;// do something;

            base.Update(gameTime, world);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            idle.DrawFromFeet(spriteBatch, FeetPosition);
        }

        private void Death()
        {

        }
    }
}