using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;

namespace FinalProject
{
    public class Ship2: GameObject
    {
        public Model Model;
        public Matrix[] Transforms;
        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;
        private const float VelocityScale = 5.0f;
        public bool isActive = true;


        public Vector3 direction;
        public float speed = GameConstants.Playerspeed;
        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity = Vector3.Zero;
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }
                if (rotation != newVal)
                {
                    rotation = newVal;
                    RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2) *
                                                                Matrix.CreateRotationZ(rotation);
                }
            }
        }

        public Ship2() { }

        public Ship2(ContentManager Content, Camera camera, GraphicsDevice graphicDevice, Light light)
        {
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);


            // add renderer

            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("A-Wing"), Transform, camera, Content, graphicDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);

            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius;
            Add<Collider>(sphereCollider);

            //isActive = true;



        }

        public void Update(float delta)
        {
            Position += direction * speed * GameConstants.Playerspeed * delta;
            if (Position.X > GameConstants.PlayfieldSizeX) Position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (Position.X < -GameConstants.PlayfieldSizeX) Position.X += 2 * GameConstants.PlayfieldSizeX;
            if (Position.Y > GameConstants.PlayfieldSizeY) Position.Y -= 2 * GameConstants.PlayfieldSizeY;
            if (Position.Y < -GameConstants.PlayfieldSizeY) Position.Y += 2 * GameConstants.PlayfieldSizeY;

        }


        public void Update(GameTime gameTime)
        {

            // Rotate the model using the left thumbstick, and scale it down.
            if (InputManager.IsKeyDown(Keys.Down))
                Position += RotationMatrix.Forward * Time.ElapsedGameTime * speed;

            // if (InputManager.IsKeyDown(Keys.W))
            //   Position += RotationMatrix.Forward * Time.ElapsedGameTime * GameConstants.Playerspeed;

            if (InputManager.IsKeyDown(Keys.Left))
                Rotation += 0.1f;

            if (InputManager.IsKeyDown(Keys.Up))
                Position += RotationMatrix.Backward * Time.ElapsedGameTime * speed;


            if (InputManager.IsKeyDown(Keys.Right))
                Rotation -= 0.1f;


            // Finally, add this vector to our velocity.
            //Velocity += RotationMatrix.Forward * VelocityScale ;
            base.Update();

        }

    }
}
