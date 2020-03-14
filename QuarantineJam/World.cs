using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;


namespace QuarantineJam
{
    public class World
    {
        public List<Rectangle> LoadedWorldHitbox;
        //public static Sprite pipe_texture;
        private Random r;
        private Player saved_player;
        public List<PhysicalObject> Stuff;

           

        public World(Player player)
        {
            r = new Random();
           
            saved_player = player;
            
            LoadedWorldHitbox = new List<Rectangle>() { };
            Stuff = new List<PhysicalObject>();
            Level.InitLevel(0, LoadedWorldHitbox, Stuff);
        }

        public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            
        }


        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

       
        public bool CheckCollision(Rectangle rectangle, Vector2 movement)
        {
            Rectangle moved_rectangle = rectangle;
            //if (movement.X != (int)movement.X) Console.WriteLine("ERREUR : HURTB0X DEPLACEE D'UN CHIFFRE NON ENTIER " + movement.X.ToString());
            moved_rectangle.Offset(movement);
            bool collision = false;
            foreach (Rectangle r in this.LoadedWorldHitbox)
            {
                if (moved_rectangle.Intersects(r))
                {
                    collision = true;
                }
            }
            return collision;
        }
        public bool CheckCollision(Vector2 FeetPosition, Vector2 Size)
        {
            Rectangle new_rectangle = new Rectangle();
            new_rectangle.X = (int)Math.Floor(FeetPosition.X - Size.X / 2);
            new_rectangle.Y = (int)Math.Floor(FeetPosition.Y - Size.Y);
            new_rectangle.Width = (int)Math.Floor(Size.X);
            new_rectangle.Height = (int)Math.Floor(Size.Y);
            bool collision = false;
            foreach (Rectangle r in this.LoadedWorldHitbox)
            {
                if (new_rectangle.Intersects(r))
                {
                    collision = true;
                }
            }
            return collision;
        }

    }
}