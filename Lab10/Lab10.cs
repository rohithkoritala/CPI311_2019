using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Lab10
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab10 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TerrainRenderer terrain;
        Camera camera;
        Effect effect;


        public Lab10()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
           
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(graphics);
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

            terrain = new TerrainRenderer(Content.Load<Texture2D>("Heightmap"),
                Vector2.One * 100, Vector2.One * 200);

            terrain.NormalMap = Content.Load<Texture2D>("Normalmap");
            float height = terrain.GetHeight(new Vector2(0.5f, 0.5f));
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);
            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f,0.1f,0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.3f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0, 0, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);
            

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;
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
            Time.Update(gameTime);
            InputManager.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (InputManager.IsKeyDown(Keys.W)) // move forward
                camera.Transform.LocalPosition += camera.Transform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.S)) // move backwars
                camera.Transform.LocalPosition += camera.Transform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.A)) // rotate left
                camera.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D)) // rotate right
                camera.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Q)) // look up
                camera.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.E)) // look down
                camera.Transform.Rotate(Vector3.Left, Time.ElapsedGameTime);
            camera.Transform.LocalPosition = new Vector3(
                camera.Transform.LocalPosition.X,
                terrain.GetAltitude(camera.Transform.LocalPosition),
                camera.Transform.LocalPosition.Z) + Vector3.Up;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["LightPosition"].SetValue(camera.Transform.Position + Vector3.Up * 10);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
