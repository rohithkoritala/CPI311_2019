using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
using System;

namespace Lab09
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab09 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model cube;
        Model sphere;
        AStarSearch search;
        List<Vector3> path;

        Random random = new Random();
        int size = 10;

        Camera camera;

        public Lab09()
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
            Time.Initialize();
            InputManager.Initialize();
            search = new AStarSearch(size, size); // size of grid 

            foreach (AStarNode node in search.Nodes)
                if (random.NextDouble() < 0.2)
                    search.Nodes[random.Next(size), random.Next(size)].Passable = false;

            search.Start = search.Nodes[0, 0];
            search.Start.Passable = true;
            search.End = search.Nodes[size - 1, size - 1];
            search.End.Passable = true;

            
            search.Search(); // A search is made here.

            path = new List<Vector3>();
            AStarNode current = search.End;
            while (current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }
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

            sphere = Content.Load<Model>("Sphere");
            cube = Content.Load<Model>("Box");


            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.One * 7;
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);

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
            Time.Update(gameTime);
            InputManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(InputManager.IsKeyPressed(Keys.Space))
            {
                while (!(search.Start = search.Nodes[random.Next(search.Cols), random.Next(search.Rows)]).Passable) ;
                while (!(search.End = search.Nodes[random.Next(search.Cols), random.Next(search.Rows)]).Passable) ;
                search.Search();
                path.Clear();
                AStarNode current = search.End;
                while (current != null)
                {
                    path.Insert(0, current.Position);
                    current = current.Parent;
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

            Matrix view = camera.View;
            Matrix projection = camera.Projection;

            foreach (AStarNode node in search.Nodes)
                if (!node.Passable)
                    cube.Draw(Matrix.CreateScale(0.5f, 0.05f, 0.5f) *
                       Matrix.CreateTranslation(node.Position), view, projection);

            foreach (Vector3 position in path)
                sphere.Draw(Matrix.CreateScale(0.1f, 0.1f, 0.1f) *
                     Matrix.CreateTranslation(position), view, projection);


            base.Draw(gameTime);
        }
    }
}
