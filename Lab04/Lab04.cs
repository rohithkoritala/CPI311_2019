using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Lab04
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab04 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model model;
        Model model1;
        private Quaternion localRotation;
        private Matrix world;
        private Vector3 localScale;
        private Vector3 localPosition;
        Transform parentTransform;
        Transform childTransform;
        Transform cameraTransform;
        Camera camera;

        public Lab04()
        {
            graphics = new GraphicsDeviceManager(this);
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
            model1 = Content.Load<Model>("Sphere");

            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            foreach (ModelMesh mesh in model1.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            parentTransform = new Transform();
            childTransform = new Transform();
            childTransform.Parent = parentTransform;
            childTransform.LocalPosition = Vector3.Right * 5;
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;

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


            // Keep rotating my child object
            childTransform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            // Scale the parent if Shift+Up/Down is pressed
            if (InputManager.IsKeyDown(Keys.LeftShift))
            {
                if (InputManager.IsKeyDown(Keys.Up))
                    parentTransform.LocalScale += Vector3.One * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Down))
                    parentTransform.LocalScale -= Vector3.One * Time.ElapsedGameTime;
            }
            else if (InputManager.IsKeyDown(Keys.LeftControl))
            {
                if (InputManager.IsKeyDown(Keys.Right))
                    parentTransform.Rotate(Vector3.Forward, Time.ElapsedGameTime * 5);
                if (InputManager.IsKeyDown(Keys.Left))
                    parentTransform.Rotate(Vector3.Backward, Time.ElapsedGameTime * 5);
                if (InputManager.IsKeyDown(Keys.Up))
                    parentTransform.Rotate(Vector3.Right, Time.ElapsedGameTime * 5);
                if (InputManager.IsKeyDown(Keys.Down))
                    parentTransform.Rotate(Vector3.Left, Time.ElapsedGameTime * 5);
            }
            // Otherwise, move the parent with respect to its axes
            else
            {
                if (InputManager.IsKeyDown(Keys.Right))
                    parentTransform.LocalPosition += parentTransform.Right * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Left))
                    parentTransform.LocalPosition += parentTransform.Left * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Up))
                    parentTransform.LocalPosition += parentTransform.Up * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Down))
                    parentTransform.LocalPosition += parentTransform.Down * Time.ElapsedGameTime;
            }

            // Control the camera
            if (InputManager.IsKeyDown(Keys.W)) // move forward
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.S)) // move backwars
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.A)) // rotate left
                cameraTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D)) // rotate right
                cameraTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Q)) // look up
                cameraTransform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.E)) // look down
                cameraTransform.Rotate(Vector3.Left, Time.ElapsedGameTime);

            InputManager.Update();
            Time.Update(gameTime);
        

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            model.Draw(parentTransform.World, camera.View, camera.Projection);
            model1.Draw(childTransform.World, camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}
