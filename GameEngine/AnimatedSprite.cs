using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine
{
    public class AnimatedSprite 
    {
        public Texture2D texture;
        public Rectangle rectangle;
        public Vector2 Position { get; set; }
        public Vector2 origin;
        public Vector2 velocity;

        int currentFrame;
        int FrameHeight;
        int FrameWidth;

        float timer;
        float interval = 75;

        public AnimatedSprite(Texture2D newTexture, Vector2 newPosition, int newFrameHeight, int newFrameWidth)
        {
            texture = newTexture;
            Position = newPosition;
            FrameHeight = newFrameHeight;
            FrameWidth = newFrameWidth;
        }

        public void Update(GameTime gameTime)
        {
            rectangle = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            Position = Position + velocity;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rectangle.Y = 96;
                AnimateRight(gameTime);
                velocity.X = 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rectangle.Y = 64;
                AnimateLeft(gameTime);
                velocity.X = -3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                rectangle.Y = 0;
                AnimateLeft(gameTime);
                velocity.Y = -3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                rectangle.Y = 32;
                AnimateLeft(gameTime);
                velocity.Y = 3;
            }
            else velocity = Vector2.Zero;
        }

        public void AnimateRight(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if(timer > interval )
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 7)
                    currentFrame = 0;
            }
        }

        public void AnimateLeft(GameTime gameTime)
        {
            
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 7)
                    currentFrame = 0;
            }
        }

        public void AnimateUp(GameTime gameTime)
        {

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 7)
                    currentFrame = 0;
            }
        }

        public void AnimateDown(GameTime gameTime)
        {

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 7)
                    currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, rectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }


    }
}
