using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using CPI311.GameEngine;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Assignment3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Assignment3 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Model model;
        Random random;
        Texture2D texture;
        Light light;
        Renderer renderer;

        //ContentManager content;
        Transform cameraTransform;
        Camera camera;

        List<Rigidbody> rigidbodies;
        List<Collider> colliders;
        List<Transform> transforms;
        List<GameObject> gameObjects;
        List<Renderer> renderers;
        List<Tuple<double, double>> frames;
        List<Vector3> LastPositions;

        BoxCollider boxCollider;

        private bool SColor;
        private bool SText;

        int lastSecondCollisions = 0;
        int numberCollisions = 0;
        bool haveThreadRunning = false;


        public Assignment3()
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
            Time.Initialize();
            InputManager.Initialize();
            random = new Random();
            camera = new Camera();
            light = new Light();
            transforms = new List<Transform>();
            rigidbodies = new List<Rigidbody>();
            colliders = new List<Collider>();
            gameObjects = new List<GameObject>();
            renderers = new List<Renderer>();
            LastPositions = new List<Vector3>();
            boxCollider = new BoxCollider();
            ThreadPool.QueueUserWorkItem(
                                new WaitCallback(CollisionReset));

            haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(
                new WaitCallback(CollisionReset));



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
            Camera camera;
           // Light light;
           // BoxCollider boxCollider;
            //List<GameObject> gameObjects;


            model = Content.Load<Model>("Sphere");
            texture = Content.Load<Texture2D>("Square");
            font = Content.Load<SpriteFont>("font");
            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;

           


            AddSphere();


            boxCollider = new BoxCollider();
            boxCollider.Size = 10;


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

            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
            //if (objectTransform.LocalPosition.Y < 0 && rigidbody.Velocity.Y < 0)
            //    rigidbody.Impulse = -new Vector3(0,rigidbody.Velocity.Y,0) * 2.1f * rigidbody.Mass;

            if (InputManager.IsKeyPressed(Keys.Space))
            {
                AddSphere();
            }
            if (InputManager.IsKeyPressed(Keys.Down))
                RemoveSphere();
            if (InputManager.IsKeyPressed(Keys.Left))
                ChangingSpeed(1f);
            if (InputManager.IsKeyPressed(Keys.Right))
                ChangingSpeed(5f);
            if (InputManager.IsKeyPressed(Keys.Space))
                SColor = !SColor;
            if (InputManager.IsKeyPressed(Keys.LeftShift))
                SText = !SText;

            foreach (GameObject rigidBody in gameObjects)
                rigidBody.Update();

            // On a new thread --
            // To fix: Assignment 4
            // 1. Binary spheres (fixed (see (A))
            // 2. Enery in the system increases (how to fix?)
            Vector3 normal;
            /*for (int i = 0; i < transforms.Count; i++)
            {
                if (boxCollider.Collides(colliders[i], out normal))
                {
                    numberCollisions++;
                    // Lab 7: include mass in equation
                    if (Vector3.Dot(normal, rigidbodies[i].Velocity) < 0)
                        rigidbodies[i].Impulse += Vector3.Dot(normal, rigidbodies[i].Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < transforms.Count; j++)
                {
                    if (colliders[i].Collides(colliders[j], out normal))
                    {
                        // Lab 7: include mass in equation
                        numberCollisions++;
                        // do resolution ONLY if they are colliding into one another
                        // if normal is from i to j
                        //dot(normal, vi) > 0 & dot(normal, vj) < 0) (A)
                        if (Vector3.Dot(normal, rigidbodies[i].Velocity) > 0 &&
                            Vector3.Dot(normal, rigidbodies[j].Velocity) < 0)
                            return;
                        Vector3 velocityNormal = Vector3.Dot(normal, rigidbodies[i].Velocity - rigidbodies[j].Velocity)
                                        * -2 * normal * rigidbodies[i].Mass * rigidbodies[j].Mass;
                        rigidbodies[i].Impulse += velocityNormal / 2;
                        rigidbodies[j].Impulse += -velocityNormal / 2;
                    }
                }
            }*/
            // on a new thread --
            for (int i = 0; i < gameObjects.Count; i++)
            {
                Rigidbody Ri = gameObjects[i].Rigidbody;
                Collider cI = gameObjects[i].Collider;

                if (boxCollider.Collides(cI, out normal))
                {
                    numberCollisions++;
                    // Lab 7: include mass in equation
                    if (Vector3.Dot(normal, Ri.Velocity) < 0)
                        Ri.Impulse += Vector3.Dot(normal, Ri.Velocity) * -2 * normal; // reflection vector
                }
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    Rigidbody RJ = gameObjects[j].Rigidbody;
                    Collider CJ = gameObjects[j].Collider;

                    if (cI.Collides(CJ, out normal))
                    {
                        // Lab 7: include mass in equation
                        numberCollisions++;

                        if (Vector3.Dot(normal, Ri.Velocity) > 0 && Vector3.Dot(normal, RJ.Velocity) < 0)
                            return;

                        Vector3 velocityNormal = Vector3.Dot(normal, Ri.Velocity - RJ.Velocity)
                                        * -2 * normal * Ri.Mass * RJ.Mass;
                        Ri.Impulse += velocityNormal / 2;
                        RJ.Impulse += -velocityNormal / 2;
                    }
                }
            }

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

            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw();

            spriteBatch.Begin();
            //spriteBatch.DrawString(font, "Collision: " + lastSecondCollisions, Vector2.Zero, Color.Black);
            spriteBatch.DrawString(font, "Shift: turn on Control panel", new Vector2(500, 10), Color.Black);
            spriteBatch.DrawString(font, "<up>: add ball, <down>: Remove ball", new Vector2(500, 30), Color.Black);
            spriteBatch.DrawString(font, "<right> or <left> Change Speed", new Vector2(500, 50), Color.Black);
            //spriteBatch.DrawString(font, "Frame Rate: " + 1 / (gameTime.ElapsedGameTime.TotalSeconds), new Vector2(0, 50), Color.Black);

            spriteBatch.End();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                Transform transform = gameObjects[i].Get<Rigidbody>().Transform;
                if (SColor)
                {
                    float SP = gameObjects[i].Get<Rigidbody>().Velocity.Length();
                    float SPVal = MathHelper.Clamp(SP / 20f, 0, 1);
                    (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = new Vector3(SPVal, SPVal, 1);
                }
            }


            if (SText)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Collision: " + lastSecondCollisions, Vector2.Zero, Color.Black);
                spriteBatch.DrawString(font, "Ball Count: " + gameObjects.Count, new Vector2(0, 25), Color.Black);
                spriteBatch.DrawString(font, "Frame Rate: " + 1 / (gameTime.ElapsedGameTime.TotalSeconds), new Vector2(0, 50), Color.Black);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void AddSphere()
        {
            GameObject gameObject = new GameObject();
            
            Transform transform = new Transform();
            transform.LocalScale *= random.Next(1, 10) * 0.25f;
            transform.Position = Vector3.Zero;
            Rigidbody rigidbody = new Rigidbody();
            
            rigidbody.Transform = transform;
            rigidbody.Mass = 0.5f + (float)random.NextDouble();
            // Lab 7: random mass
            rigidbody.Acceleration = Vector3.Down * 9.81f;
           // rigidbody.Velocity = new Vector3((float)random.NextDouble() * 5, (float)random.NextDouble() * 5, (float)random.NextDouble() * 5);

            Vector3 direction = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5);
            SphereCollider sphereCollider = new SphereCollider();
            
            sphereCollider.Radius = gameObject.Transform.LocalScale.Y;
            sphereCollider.Transform = transform;
            Texture2D texture = Content.Load<Texture2D>("Square");

            light.Transform = new Transform();

            light.Transform.LocalPosition = Vector3.Backward * 10 + Vector3.Right * 5;
           

            renderer = new Renderer(model, gameObject.Transform, camera, Content,GraphicsDevice, light,1, "SimpleShading", 20f, texture);

            gameObject.Add(rigidbody);
            gameObject.Add<Collider>(sphereCollider);
            gameObject.Add(renderer);
           // gameObject.Add(transform);
            gameObject.Add(camera);
            gameObjects.Add(gameObject);
            
            
        }

        private void ChangingSpeed(float s)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Get<Rigidbody>().Velocity = new Vector3(gameObject.Get<Rigidbody>().Velocity.X * s, gameObject.Get<Rigidbody>().Velocity.Y * s, gameObject.Get<Rigidbody>().Velocity.Z * s);
            }
        }
        private void RemoveSphere()
        {
            gameObjects.Remove(gameObjects[0]);
        }

        private void CollisionReset(Object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                numberCollisions = 0;
                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}
