using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using CPI311.GameEngine;
using System.Collections.Generic;
using System;

namespace Lab08
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab08 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SoundEffect soundEffect;
        Model model;
        GraphicsDevice graphics1;

        Camera camera, topDownCamera;
        List<Transform> transforms;
        List<Collider> colliders;
        List<Camera> cameras;

        Effect effect;
        Texture2D texture;


        public Lab08()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            spriteBatch = new SpriteBatch(GraphicsDevice);
            soundEffect = Content.Load<SoundEffect>("Gun");
            model = Content.Load<Model>("Sphere");
            texture = Content.Load<Texture2D>("Square");
            effect = Content.Load<Effect>("SimpleShading");
            
            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            transforms = new List<Transform>();
            colliders = new List<Collider>();
            cameras = new List<Camera>();

            ScreenManager.Setup(true, 1920, 1080);

            Transform transform = new Transform();
            transforms.Add(transform);
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Transform = transform;
            sphereCollider.Radius = 2.5f * transform.LocalScale.Y;
            colliders.Add(sphereCollider);

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5;
            camera.Position = new Vector2(0f, 0f);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;

            topDownCamera = new Camera();
            topDownCamera.Transform = new Transform();
            topDownCamera.Transform.LocalPosition = Vector3.Up * 10;
            topDownCamera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            topDownCamera.Position = new Vector2(0.5f, 0f);
            topDownCamera.Size = new Vector2(0.5f, 1f);
            topDownCamera.AspectRatio = topDownCamera.Viewport.AspectRatio;

            cameras.Add(topDownCamera);
            cameras.Add(camera);




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

            Ray ray = camera.ScreenPointToWorldRay(InputManager.GetMousePosition());
            foreach (Collider collider in colliders)
            {
                collider.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
                collider.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
                collider.Transform.Rotate(Vector3.Forward, Time.ElapsedGameTime);
                if (collider.Intersects(ray) != null)
                {
                    effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                    (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                                                 Color.Blue.ToVector3();
                }
                else
                {
                    effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());
                    (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                                                 Color.Red.ToVector3();
                }
            }


            if (InputManager.IsMousePressed(0))
            {
                SoundEffectInstance instance = soundEffect.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.DepthStencilState = new DepthStencilState();
            GraphicsDevice.Viewport = camera.Viewport;
            foreach (Camera camera in cameras)
            {
                GraphicsDevice.DepthStencilState = new DepthStencilState();
                GraphicsDevice.Viewport = camera.Viewport;
                Matrix view = camera.View;
                Matrix projection = camera.Projection;

                effect.CurrentTechnique = effect.Techniques[1];
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 + Vector3.Right * 5);
                effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
                effect.Parameters["Shininess"].SetValue(20f);
                effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
                effect.Parameters["SpecularColor"].SetValue(new Vector3(0, 0, 0.5f));
                effect.Parameters["DiffuseTexture"].SetValue(texture);

                foreach (Transform transform in transforms)
                {
                    effect.Parameters["World"].SetValue(transform.World);
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        foreach (ModelMesh mesh in model.Meshes)
                            foreach (ModelMeshPart part in mesh.MeshParts)
                            {
                                GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                                GraphicsDevice.Indices = part.IndexBuffer;
                                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                                    part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                            }
                    }
                }


                base.Draw(gameTime);
            }
        }
    }
}
