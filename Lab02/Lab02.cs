using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;


namespace Lab02
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab02 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Sprite sprite;
        SpiralMotion spiralMotion;
        //KeyboardState prevState;
        

        public Lab02()
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

        protected override void LoadContent()
        {
   
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //sprite = new Sprite(Content.Load<Texture2D>("Square"));
            spiralMotion = new SpiralMotion(Content.Load<Texture2D>("Square"), Vector2.Zero);


        }

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

            /*  if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                  Exit();
              InputManager.Update();
              base.Update(gameTime);
              //KeyboardState currentState = Keyboard.GetState();
              if (InputManager.IsKeyDown(Keys.Left)) 
                 sprite.Position += Vector2.UnitX * -5;


              if (InputManager.IsKeyPressed(Keys.Right))
                  sprite.Position += Vector2.UnitX * 5;

              if (InputManager.IsKeyPressed(Keys.Up))
                  sprite.Position += Vector2.UnitY * -5;

              if (InputManager.IsKeyPressed(Keys.Down))
                  sprite.Position += Vector2.UnitY * 5;

              if (InputManager.IsKeyDown(Keys.Space))
                  sprite.Rotation += 0.05f;
              sprite.Position = new Vector2(200, 200) + new Vector2( (float)((100*Math.Cos(phase))*Math.Cos(phase)),(float)((100*Math.Sin(phase))*(100*Math.Sin(phase))));
              //prevState = currentState;*/

            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
            spiralMotion.Update();
            base.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spiralMotion.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
