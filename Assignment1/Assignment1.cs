using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System;

namespace Assignment1
{
    public class Assignment1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AnimatedSprite player;
        ProgressBar progBar;
        ProgressBar distBar;
        SpriteFont font;
        float timeLeft = 90f;
        float distanceWalked = 0;
        Sprite bonus;
        Random random = new Random();
        

        public Assignment1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

 
        protected override void Initialize()
        {
            InputManager.Initialize();
            
            Time.Initialize();
            bonus = new Sprite(Content.Load<Texture2D>("Square"));
            bonus.Position = new Vector2(100, 100);
            
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new AnimatedSprite(Content.Load<Texture2D>("explorer"), new Vector2(100, 100), 32, 32);
            progBar = new ProgressBar(Content.Load<Texture2D>("Square"),Color.Green,2);
            distBar = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Red, 2);
            font = Content.Load<SpriteFont>("Font1");
            progBar.Position = new Vector2(50, 50);
            distBar.Position = new Vector2(100, 50);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Vector2 playerprePos = player.Position;
            player.Update(gameTime);

            progBar.Update();
            distanceWalked += Math.Abs((player.Position - playerprePos).Length());
            distBar.Value = distanceWalked/ 150f ;
            if(Math.Abs((player.Position - bonus.Position).Length()) < 50)
            {
                progBar.Value += 3.5f;
                int randX = random.Next(0, GraphicsDevice.Viewport.Width);
                int randY = random.Next(0, GraphicsDevice.Viewport.Height);
                bonus.Position = new Vector2(randX, randY);
            }
            
            Time.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            bonus.Draw(spriteBatch);
            distBar.Draw(spriteBatch);
            progBar.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Time: " + progBar.Value,new Vector2(50,90),Color.White );
            spriteBatch.DrawString(font, "Distance: " + distBar.Value, new Vector2(50, 120), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
