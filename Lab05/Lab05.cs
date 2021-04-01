using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Lab05
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab05 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model model;
        Transform parentTransform;
        Transform childTransform;
        Transform cameraTransform;
        Camera camera;
        Effect effect;
        Texture2D texture;
        int i = 0;

        Transform model2transform;

        public Lab05()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
            Time.Initialize();
            InputManager.Initialize();
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
            texture = Content.Load<Texture2D>("Square");
            effect = Content.Load<Effect>("SimpleShading");
            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();
            parentTransform = new Transform();
            childTransform = new Transform();
            childTransform.Parent = parentTransform;
            childTransform.LocalPosition = Vector3.Right * 10;
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;
            
        }

        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
           

            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
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
                    parentTransform.Rotate(Vector3.Up, Time.ElapsedGameTime * 5);
                if (InputManager.IsKeyDown(Keys.Left))
                    parentTransform.Rotate(Vector3.Down, Time.ElapsedGameTime * 5);
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
                
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix view = camera.View;
            Matrix projection = camera.Projection;

            effect.CurrentTechnique = effect.Techniques[2]; //"0" is the first technique
            effect.Parameters["World"].SetValue(parentTransform.World);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 +
                                                                     Vector3.Right * 5);
            effect.Parameters["CameraPosition"].SetValue(cameraTransform.Position);
            effect.Parameters["Shininess"].SetValue(20f);
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f, 0, 0));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0, 0, 0.5f));
            effect.Parameters["DiffuseTexture"].SetValue(texture);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        GraphicsDevice.Indices = part.IndexBuffer;
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, part.VertexOffset,part.StartIndex, part.PrimitiveCount);
                    }
            }


            base.Draw(gameTime);
        }
    }
}
