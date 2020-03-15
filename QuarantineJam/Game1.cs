using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuarantineJam
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D rectangle; // used for debug

        World world;

        Vector2 CameraDestination = new Vector2(0, 700), CameraPosition = new Vector2(0, 700);
        Rectangle ViewRectangle;
        float Zoom = 0.8f;
        Player player;
        Matrix Camera;
        SpriteFont rouliFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            
            Camera = Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(CameraPosition.X, CameraPosition.Y, 0);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            World.LoadContent(Content);
            Player.LoadContent(Content);
            PhysicalObject.LoadContent(Content);
            rectangle = Content.Load<Texture2D>("texture_chelou");
            rouliFont = Content.Load<SpriteFont>("Rouli");

            world = new World(player);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
#if DEBUG
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.D)) CameraDestination.X += 8;
            if (ks.IsKeyDown(Keys.Q)) CameraDestination.X -= 8;
            if (ks.IsKeyDown(Keys.Z)) CameraDestination.Y -= 8;
            if (ks.IsKeyDown(Keys.S)) CameraDestination.Y += 8;
            if (ks.IsKeyDown(Keys.NumPad9)) Zoom *= 1.05f;
            if (ks.IsKeyDown(Keys.NumPad8)) Zoom *= 0.95f;
#endif
            if (!world.Bounds.Contains(ViewRectangle)) // camera oob
            {
                CameraPosition = world.Bounds.Location.ToVector2();
                CameraDestination = CameraPosition;
            }
            //else
            {
                if (player.Hurtbox.Center.Y < ViewRectangle.Center.Y - 100) MoveCamera(new Vector2(0, -5));
                else if (player.Hurtbox.Center.Y > ViewRectangle.Center.Y + 100) MoveCamera(new Vector2(0, 10));

                if (player.Hurtbox.Center.X > ViewRectangle.Center.X && player.PlayerDirection == 1) MoveCamera(new Vector2(10, 0));
                else if (player.Hurtbox.Center.X < ViewRectangle.Center.X && player.PlayerDirection == -1) MoveCamera(new Vector2(-10, 0));
            }

            CameraPosition = CameraPosition * 0.5f + CameraDestination * 0.5f;
            Camera = Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(Zoom * (-CameraPosition.X), Zoom * (-CameraPosition.Y), 0);  ;
            ViewRectangle = new Rectangle(0, 0, 1280, 720);
            ViewRectangle.Location = Vector2.Transform(ViewRectangle.Location.ToVector2(), Matrix.Invert(Camera)).ToPoint();
            ViewRectangle.Size = (ViewRectangle.Size.ToVector2() *1/Zoom).ToPoint();
            //ViewRectangle.Inflate(-128 * 1/Zoom, -72 * 1/Zoom);
           



            player.Update(gameTime, world, player);
            world.Update(gameTime, player);
            Input.Update(Keyboard.GetState());
            SoundEffectPlayer.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null,  SamplerState.LinearWrap, transformMatrix: Camera);
            GraphicsDevice.Clear(Color.LightGray);

            world.Draw(spriteBatch);
            player.Draw(spriteBatch);
            DrawRectangle(spriteBatch, ViewRectangle, Color.Red * 0.5f);
            DrawRectangle(spriteBatch, world.Bounds, Color.Green * 0.5f);
            DrawRectangle(spriteBatch, new Rectangle((int)CameraPosition.X, (int)CameraPosition.Y, 15, 15), Color.Black);

            spriteBatch.End();
            spriteBatch.Begin();

            spriteBatch.DrawString(rouliFont, world.countBees().ToString(), new Vector2(50, 50), Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangleToDraw, Color color, Texture2D texture = null)
        {
            if (texture == null) texture = rectangle;
            spriteBatch.Draw(texture, rectangleToDraw,
                new Rectangle(rectangleToDraw.X % texture.Width,
                              rectangleToDraw.Y % texture.Height,
                              (int)(rectangleToDraw.Width),
                              (int)(rectangleToDraw.Height)),
                color,
                0f,
                new Vector2(0, 0),
                SpriteEffects.None,
                0f);
        }

        private void MoveCamera(Vector2 Movement)
        {
            ViewRectangle.Offset(Movement * 2f);
            if (world.Bounds.Contains(ViewRectangle)) CameraDestination += Movement;
            else CameraDestination = CameraPosition;
            ViewRectangle.Offset(-Movement * 2f);
        }
    }
}
