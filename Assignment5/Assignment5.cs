using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System;

namespace Assignment5
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Assignment5 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TerrainRenderer terrain;
        Camera camera;
        Effect effect;
        Light light;
        SpriteFont font;

        Player player;
        int hits = 0;

        Random random = new Random();

        Agent agent;
        Agent agent1;
        Agent agent2;

        public Assignment5()
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
            font = Content.Load<SpriteFont>("Font");

            terrain = new TerrainRenderer(Content.Load<Texture2D>("mazeH2"), Vector2.One * 100, Vector2.One * 200);

            terrain.NormalMap = Content.Load<Texture2D>("mazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);

            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);
            //effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);
            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition += Vector3.Up * 50;
            camera.Transform.Rotate(Vector3.Left, MathHelper.PiOver2 - 0.2f);

            light = new Light();
            light.Transform = new Transform(); //sjould be the same as other laps



            player = new Player(terrain, Content, camera, GraphicsDevice, light);
            agent = new Agent(terrain, Content, camera, GraphicsDevice, light, random);
            agent1 = new Agent(terrain, Content, camera, GraphicsDevice, light, random);
            agent2 = new Agent(terrain, Content, camera, GraphicsDevice, light, random);


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

            if (InputManager.IsKeyDown(Keys.Up))
            {

                camera.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            }
            if (agent.CheckCollision(player))
            {

                hits++;
            }
            if (agent1.CheckCollision(player))
            {

                hits++;
            }
            if (agent2.CheckCollision(player))
            {

                hits++;
            }
            player.Update();
            agent.Update();
            agent1.Update();
            agent2.Update();

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
            effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)

            {

                pass.Apply();
                terrain.Draw();
            }
            player.Draw();
            agent1.Draw();
            agent2.Draw();
            agent.Draw();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Hits: " + hits, new Vector2(50, 50), Color.Red);
            spriteBatch.DrawString(font, "Time" + Time.TotalGameTime.ToString(), new Vector2(50, 20), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
