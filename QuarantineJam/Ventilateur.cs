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
    class Ventilateur:PhysicalObject
    {
        Rectangle WindBox;
        int direction;
        public Ventilateur(Vector2 Spawn, int direction = -1):base(new Vector2(174, 73), Spawn)
        {
            this.direction = direction;
            CurrentSprite = new Sprite(ventilo_sprite);
            if (direction == 1) CurrentSprite.direction = -1;
            Gravity = 1f;
        }

        public override void Update(GameTime gameTime, World world, Player player)
        {
            WindBox = new Rectangle((int)FeetPosition.X + (direction - 1) * 400, (int)FeetPosition.Y - 200, 800, 200);
            CurrentSprite.UpdateFrame(gameTime);
            foreach (PhysicalObject p in world.Stuff) if (WindBox.Contains(p.Hurtbox.Center)) p.ApplyForce(new Vector2(direction * 0.1f, 0));

            if (player.Hurtbox.Intersects(WindBox))
            {
                player.ApplyForce(new Vector2(direction * 4.2f, 0));
            }
            
            base.Update(gameTime, world, player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Wind, WindBox,
                            new Rectangle((WindBox.X + lifetime * 10 * direction * -1) % Wind.Width,
                                          WindBox.Y % Wind.Height,
                                          (int)(WindBox.Width),
                                          (int)(WindBox.Height)),
                            Color.White,
                            0f,
                            new Vector2(0, 0),
                            SpriteEffects.None,
                            0f); 
            base.Draw(spriteBatch);
        }
    }
}
