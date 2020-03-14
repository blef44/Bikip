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
        Player player;
        Matrix Camera;

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
            world = new World(player);
            Camera = Matrix.CreateScale(2.0f/3) * Matrix.CreateTranslation(0, 0, 0);
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
            Bee.LoadContent(Content);
            rectangle = Content.Load<Texture2D>("texture_chelou");
            // TODO: use this.Content to load your game content here
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

            player.Update(gameTime, world);
            world.Update(gameTime);
            Input.Update(Keyboard.GetState());
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: Camera);
            GraphicsDevice.Clear(Color.LightGray);
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            world.Draw(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangleToDraw, Color color, Texture2D texture = null)
        {
            if (texture == null) texture = rectangle;
            spriteBatch.Draw(texture, rectangleToDraw, texture.Bounds, color, 0f, Vector2.Zero, default, default);
        }
    }
}
