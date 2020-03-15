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
    class Ruche : PhysicalObject
    {
        public int bee_count { get; private set; }
        private int frame_cooldown, direction;

        public Ruche(Vector2 Spawn, int bee_count = 10, int direction = 1):base(new Vector2(95, 60), Spawn)
        {
            this.bee_count = bee_count;
            this.direction = direction;
            frame_cooldown = 0;
            Gravity = 1f;
        }

        public override void Update(GameTime gameTime, World world, Player player)
        {
            if(frame_cooldown > 0) frame_cooldown -= 1;

            if (player.Hurtbox.Intersects(Hurtbox))
            {
                if(player.Velocity.Y > 1) // player rifght above ruche falling
                {
                    if (frame_cooldown == 0)
                    {
                        frame_cooldown = 10;
                    }
                    if (frame_cooldown > 6) player.Velocity.Y *= 0.7f;
                    if(frame_cooldown == 6)
                    {
                        player.Velocity.Y = -12;
                        player.CurrentState = Player.PlayerState.jump;
                        SoundEffectPlayer.Play(jump);
                    }
                    
                }
            }
            else if (player.Hurtbox.Intersects(Hurtbox)) player.Bump(this);
            
            if(frame_cooldown > 6)
            {
                CurrentSprite = ruche2;
            }
            else if (frame_cooldown > 0)
            {
                CurrentSprite = ruche3;
                if (bee_count > 0)
                {
                    bee_count -= 1;
                    Bee b = new Bee(FeetPosition + new Vector2(- direction * 50, 0), 
                        new Vector2( - direction * r.Next(14, 25), r.Next(-2, 3))); // use (r.Next(0,2) * 2 - 1) to random -1 or 1
                   // b.Velocity = new Vector2(-10, 0);
                    world.NewStuff.Add(b);
                }
            }
            else CurrentSprite = ruche1;
            base.Update(gameTime, world, player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentSprite.DrawFromFeet(spriteBatch, FeetPosition + new Vector2(0, 8));
            //base.Draw(spriteBatch);
        }
    }
}
