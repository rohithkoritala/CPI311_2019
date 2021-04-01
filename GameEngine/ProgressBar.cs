using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CPI311.GameEngine
{
    public class ProgressBar: Sprite
    {
        public float Value { get; set; }
        public Color FillColor { get; set; }
        public float Speed { get; set; }
        public bool isActive { get; set; }

        public ProgressBar(Texture2D texture,Color newColor, float speed = 2)
            : base(texture)
        {
            Speed = speed;
            Value = texture.Width;
            if (newColor == Color.Red)
                Value = 1; 
            FillColor = newColor;
            isActive = false;
        }

        public override void Update()
        {
            base.Update();
            if(isActive == true)
            {
                if (FillColor == Color.Gold)
                {
                    Value -= Speed * Time.ElapsedGameTime;
                }

                if (FillColor == Color.Violet)
                {
                    Value -= Speed * Time.ElapsedGameTime;
                }

            }
            
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(isActive == true)
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, (int)(Value), Texture.Height), FillColor, Rotation, Origin, Scale, Effect, Layer);
        }

    
    
    }
}
