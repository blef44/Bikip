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
        private static Vector2 beeHurtboxSize { get; } = new Vector2(42, 33);
        static Sprite idle;
        public Bee(Vector2 FeetPosition) : base(beeHurtboxSize, FeetPosition)
        {
            
        }
        public new static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            idle = new Sprite(Content.Load<Texture2D>("bee_draft"));
        }

        public override void Update(GameTime gameTime, World world)
        {
            CurrentSprite = idle;
            base.Update(gameTime, world);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            idle.DrawFromFeet(spriteBatch, FeetPosition);
        }
    }
}
