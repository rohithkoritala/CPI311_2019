using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using CPI311.GameEngine;

namespace FinalProject
{
    struct Pickup
    {

        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public bool isActive;
        public Model Model;
        public Matrix[] Transforms;




        public void Update(float delta)
        {
            position += direction * speed * GameConstants.AsteroidSpeedAdjustment * delta;
            if (position.X > GameConstants.PlayfieldSizeX)
                position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (position.X < -GameConstants.PlayfieldSizeX)
                position.X += 2 * GameConstants.PlayfieldSizeX;
            if (position.Y > GameConstants.PlayfieldSizeY)
                position.Y -= 2 * GameConstants.PlayfieldSizeY;
            if (position.Y < -GameConstants.PlayfieldSizeY)
                position.Y += 2 * GameConstants.PlayfieldSizeY;
        }
    }
}
