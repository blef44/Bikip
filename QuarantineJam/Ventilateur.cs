using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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
        }

        public override void Update(GameTime gameTime, World world, Player player)
        {
            WindBox = new Rectangle((int)FeetPosition.X - (direction - 1) * 800, (int)FeetPosition.Y - 200, 800, 200);
            CurrentSprite.UpdateFrame(gameTime);
            foreach (PhysicalObject p in world.Stuff) if (p.Hurtbox.Intersects(WindBox)) p.ApplyForce(new Vector2(direction * 1, -1));
            
            if (player.Hurtbox.Intersects(WindBox)) player.ApplyForce(new Vector2(direction * 1, -2));
            
            base.Update(gameTime, world, player);
        }
    }
}
