using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab03 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model model;
        Vector3 torusPosition;
        Vector3 torusRotation;
        Vector3 torusScale = new Vector3(1,1,1);
        Matrix world;
        Matrix view;
        Matrix projection;
        Vector2 cameraSize = new Vector2(1,1);
        Vector2 cameraCenter = new Vector2(0,0);
        Vector3 cameraPosition = Vector3.Backward * 10;
        bool isPerspective = true;
        bool isART = true;
        SpriteFont font;
        public Lab03()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            InputManager.Initialize();
            Time.Initialize();


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
            model = Content.Load<Model>("Torus");
            font = Content.Load<SpriteFont>("font");
            

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();
            Time.Update(gameTime);

            if (InputManager.IsKeyDown(Keys.LeftShift) || InputManager.IsKeyDown(Keys.RightShift))
            {
                // Scale the Torus
                if (InputManager.IsKeyDown(Keys.Up))   torusScale += Vector3.One * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Down)) torusScale -= Vector3.One * Time.ElapsedGameTime;

                if (InputManager.IsKeyDown(Keys.W)) cameraSize += Vector2.UnitY * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.S)) cameraSize -= Vector2.UnitY * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.D)) cameraSize += Vector2.UnitX * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.A)) cameraSize -= Vector2.UnitX * Time.ElapsedGameTime;
            }
            else if (InputManager.IsKeyDown(Keys.LeftControl) || InputManager.IsKeyDown(Keys.RightControl))
            {

                if (InputManager.IsKeyDown(Keys.Up))    torusRotation += Vector3.Forward * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Down))  torusRotation += Vector3.Backward * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Left))  torusRotation += Vector3.Left * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Right)) torusRotation += Vector3.Right * Time.ElapsedGameTime;

                // Change camera center
                // (0,0) would be true center. Anything else skews the camera
                if (InputManager.IsKeyDown(Keys.W)) cameraCenter += Vector2.UnitY * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.S)) cameraCenter -= Vector2.UnitY * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.D)) cameraCenter += Vector2.UnitX * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.A)) cameraCenter -= Vector2.UnitX * Time.ElapsedGameTime;
            }

            else
            {
                if (InputManager.IsKeyDown(Keys.Up)) torusPosition += Vector3.Up * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Down)) torusPosition += Vector3.Down * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Left)) torusPosition += Vector3.Left * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Right)) torusPosition += Vector3.Right * Time.ElapsedGameTime;



                if (InputManager.IsKeyDown(Keys.W)) cameraPosition += Vector3.Up * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.A)) cameraPosition += Vector3.Left * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.S)) cameraPosition += Vector3.Down * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.D)) cameraPosition += Vector3.Right * Time.ElapsedGameTime;

            }


           view = Matrix.CreateLookAt(cameraPosition, cameraPosition + Vector3.Forward, Vector3.Up);

           Vector2 topLeft = cameraCenter - cameraSize;
           Vector2 bottomRight = cameraCenter + cameraSize;

           if (isPerspective ^= InputManager.IsKeyPressed(Keys.Tab))
               projection = Matrix.CreatePerspectiveOffCenter(topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y, 1f, 100f);
           else
                projection = Matrix.CreateOrthographicOffCenter(topLeft.X * 10, bottomRight.X * 10, topLeft.Y * 10, bottomRight.Y * 10, 1f, 100f);



            //     projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1.33f, 0.1f, 100f);

            //projection = Matrix.CreateOrthographicOffCenter(MathHelper.PiOver2, 1.33f, 0.1f, 100f);

            if (isART ^= InputManager.IsKeyPressed(Keys.Space))
                world = Matrix.CreateScale(torusScale) * Matrix.CreateFromYawPitchRoll(torusRotation.Y, torusRotation.X, torusRotation.Z) * Matrix.CreateTranslation(torusPosition);
            else
                world = Matrix.CreateTranslation(torusPosition) * Matrix.CreateFromYawPitchRoll(torusRotation.Y, torusRotation.X, torusRotation.Z) * Matrix.CreateScale(torusScale);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

           




            //world = Matrix.CreateScale;
            model.Draw(world, view, projection);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Camera: WASD (move), Shift+WASD (size), Ctrl+WASD (center)", Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Model: Arrows (translate), Shift+Up (or) Down (scale), Ctrl+Arrows (rotate)", Vector2.UnitY * 20, Color.White);
            spriteBatch.DrawString(font, (isPerspective ? "Perspective" : "Orthographic") +
                                            " (Tab to change)\n" +
                                            (isART ? "Scale * Rotate * Translate" : "Translate * Rotate * Scale") +
                                            " (Space to change)", Vector2.UnitY * 40, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
