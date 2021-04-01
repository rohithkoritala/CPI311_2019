using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using CPI311.GameEngine;

namespace FinalProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class FinalProject : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ProgressBar healthBar;
        ProgressBar healthBar1;
        ProgressBar godBar;
        ProgressBar virusBar;
        ProgressBar godBar1;
        ProgressBar virusBar1;

        Ship ship = new Ship();
        Ship2 ship1 = new Ship2();

        Model godPickup; // variables for invincibility pickup 
        Matrix[] godTransforms;
        Pickup[] pickupList = new Pickup[GameConstants.Numgods];
        Random random = new Random();
        Random random1 = new Random();

        Model virusPickup; // variables for virus slowdown pickup
        Matrix[] virusTransforms;
        Pickup[] pickupList1 = new Pickup[GameConstants.Numvirus];


        Model bulletModel; // variables for player1 bullet
        Matrix[] bulletTransforms;
        Bullet[] bulletList = new Bullet[GameConstants.NumBullets];
        float delta = 20f;

        Model bulletModel1; // variables for player2 bullet
        Matrix[] bulletTransforms1;
        Bullet[] bulletList1 = new Bullet[GameConstants.NumBullets];

        int score;
        int score1;

        SoundEffectInstance soundEngineInstance;
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance1;
        SoundEffect soundEngine1;


        SoundEffect soundHyperspaceActivation;
        SoundEffect soundExplosion2;
        SoundEffect soundGod;
        SoundEffect soundVirus;
        SoundEffect virusDestroy;
        SoundEffect soundExplosion3;

        SoundEffect soundWeaponsFire;
        SoundEffect soundWeaponsFire1;

        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);
        Matrix projectionMatrix;
        Matrix viewMatrix;

        ParticleManager particleManager;
        Texture2D particleTex;
        Effect particleEffect;
        SpriteFont font;

        Texture2D stars;

        public FinalProject()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 960;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
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
            score = 0;
            score1 = 0;
            
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
             MathHelper.ToRadians(45.0f),
             GraphicsDevice.DisplayMode.AspectRatio,
               GameConstants.CameraHeight - 1000.0f,
               GameConstants.CameraHeight + 1000.0f);
           
            viewMatrix = Matrix.CreateLookAt(cameraPosition,
                Vector3.Zero, Vector3.Up);
            Vector2 pos = new Vector2(-7000, 0);
            Vector2 pos1 = new Vector2(7000, 0);
            ship.Position = new Vector3(pos, 0);
            ship1.Position = new Vector3(pos1, 0);
            ResetVirus();
            ResetGod();
            

            base.Initialize();
        }

        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection =  projectionMatrix;
                    effect.View =  viewMatrix;
                }
            }
            return absoluteTransforms;
        }

        //************* Reset class for first pickup ********************************************
        private void ResetGod()
        {
            float xStart;
            float yStart;
            for (int i = 0; i < GameConstants.Numgods; i++)
            {
                pickupList[i].isActive = true;
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                yStart =
                    (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                pickupList[i].position = new Vector3(xStart, yStart, 0.0f);
                double angle = random.NextDouble() * 2 * Math.PI;
                pickupList[i].direction.X = -(float)Math.Sin(angle);
                pickupList[i].direction.Y = (float)Math.Cos(angle);
                pickupList[i].speed = GameConstants.AsteroidMinSpeed +
                   (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;

            }
        }

        //******************************* Reset method for virus pickup *********************************************
        private void ResetVirus()
        {
            float xStart1;
            float yStart1;
            for (int i = 0; i < GameConstants.Numvirus; i++)
            {
                pickupList1[i].isActive = true;
                if (random1.Next(2) == 0)
                {
                    xStart1 = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart1 = (float)GameConstants.PlayfieldSizeX;
                }
                yStart1 =
                    (float)random1.NextDouble() * GameConstants.PlayfieldSizeY;
                pickupList1[i].position = new Vector3(xStart1, yStart1, 0.0f);
                double angle = random1.NextDouble() * 2 * Math.PI;
                pickupList1[i].direction.X = -(float)Math.Sin(angle);
                pickupList1[i].direction.Y = (float)Math.Cos(angle);
                pickupList1[i].speed = GameConstants.AsteroidMinSpeed1 +
                   (float)random1.NextDouble() * GameConstants.AsteroidMaxSpeed1;

            }

        }
        //**************************************************************************************

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            particleManager = new ParticleManager(GraphicsDevice, 100);
            particleEffect = Content.Load<Effect>("ParticleShader-complete");
            particleTex = Content.Load<Texture2D>("fire");


            ship.Model = Content.Load<Model>("p1_wedge");
            ship.Transforms = SetupEffectDefaults(ship.Model);
            ship1.Model = Content.Load<Model>("A-Wing");
            ship1.Transforms = SetupEffectDefaults(ship1.Model);
            


            healthBar = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Green, 2);
            healthBar1 = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Green, 2);
            healthBar.Position = new Vector2(50, 50);
            healthBar1.Position = new Vector2(1150, 50);
            godBar = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Gold, 2);
            virusBar = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Violet, 2);
            godBar.Position = new Vector2(100, 100);
            virusBar.Position = new Vector2(50, 100);
            godBar1 = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Gold, 2);
            virusBar1 = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Violet, 2);
            godBar1.Position = new Vector2(1150, 100);
            virusBar1.Position = new Vector2(1000, 100);

            healthBar.isActive = true;
            healthBar1.isActive = true;

            godPickup = Content.Load<Model>("god");
            godTransforms = SetupEffectDefaults(godPickup);
            virusPickup = Content.Load<Model>("virus");
            virusTransforms = SetupEffectDefaults(virusPickup);

            bulletModel = Content.Load<Model>("bullet");
            bulletTransforms = SetupEffectDefaults(bulletModel);
            bulletModel1 = Content.Load<Model>("bullet1");
            bulletTransforms1 = SetupEffectDefaults(bulletModel1);

            soundEngine = Content.Load<SoundEffect>("engine_2");
            soundEngineInstance = soundEngine.CreateInstance();
            soundEngine1 = Content.Load<SoundEffect>("engine3");
            soundEngineInstance1 = soundEngine1.CreateInstance();

            soundWeaponsFire = Content.Load<SoundEffect>("fireboom");
            soundWeaponsFire1 = Content.Load<SoundEffect>("bruh");
            soundHyperspaceActivation =
                Content.Load<SoundEffect>("hyperspace_activate");

            soundExplosion2 = Content.Load<SoundEffect>("explosion2");
            soundGod = Content.Load<SoundEffect>("omae");
            soundVirus = Content.Load<SoundEffect>("virusSound");
            virusDestroy = Content.Load<SoundEffect>("glass");

            font = Content.Load<SpriteFont>("font");
            stars = Content.Load<Texture2D>("Galaxy");

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

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Time.Update(gameTime);
            InputManager.Update();
            UpdateInput();

            // Add velocity to the current position.
            ship.Position += ship.Velocity;
            // Bleed off velocity over time.
            ship.Velocity *= 0.95f;


            // Add velocity to the current position.
            ship1.Position += ship.Velocity;
            ship1.Velocity *= 0.95f;

            for (int i = 0; i < GameConstants.Numgods; i++)
            {
                if (pickupList[i].isActive)
                {
                    pickupList[i].Update(timeDelta);
                }

            }

            for (int i = 0; i < GameConstants.Numvirus; i++)
            {
                if (pickupList1[i].isActive)
                {
                    pickupList1[i].Update(timeDelta);
                }

            }

            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList[i].isActive)
                {
                    bulletList[i].Update(timeDelta);
                }
            }

            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList1[i].isActive)
                {
                    bulletList1[i].Update(timeDelta);
                }
            }

//************************ bullets can destroy virus pickups ***********************************
            for (int i = 0; i < pickupList1.Length; i++)
            {
                if (pickupList1[i].isActive)
                {
                    BoundingSphere virusSphere = new BoundingSphere(pickupList1[i].position, virusPickup.Meshes[0].BoundingSphere.Radius * GameConstants.AsteroidBoundingSphereScale);
                    for (int j = 0; j < bulletList.Length; j++)
                    {
                        if (bulletList[j].isActive)
                        {
                            BoundingSphere bulletSphere = new BoundingSphere(bulletList[j].position, bulletModel.Meshes[0].BoundingSphere.Radius);
                           
                            if (virusSphere.Intersects(bulletSphere) )
                            {
                               pickupList1[i].isActive = false;                                                                     
                               bulletList[j].isActive = false;                                                                              
                               virusDestroy.Play();
                              // pickupList1[i].isActive = false;
                              // bulletList[j].isActive = false;                                       
                               break; 
                            }                                                                                          
                        }
                    }
                }
            }

            for (int i = 0; i < pickupList1.Length; i++)
            {
                if (pickupList1[i].isActive)
                {
                    BoundingSphere virusSphere = new BoundingSphere(pickupList1[i].position, virusPickup.Meshes[0].BoundingSphere.Radius * GameConstants.AsteroidBoundingSphereScale);
                    for (int j = 0; j < bulletList.Length; j++)
                    {
                        if (bulletList1[j].isActive)
                        {
                            BoundingSphere bulletSphere1 = new BoundingSphere(bulletList1[j].position, bulletModel1.Meshes[0].BoundingSphere.Radius);

                            if (virusSphere.Intersects(bulletSphere1))
                            {
                                pickupList1[i].isActive = false;
                                bulletList1[j].isActive = false;
                                virusDestroy.Play();
                                pickupList1[i].isActive = false;
                                bulletList1[j].isActive = false;
                                break;
                            }
                        }
                    }
                }
            }



//********************** Bullet damaging players *****************************************************

            if (ship1.isActive)
            {
                BoundingSphere shipSphere1 = new BoundingSphere(
                 ship1.Position, ship1.Model.Meshes[0].BoundingSphere.Radius *
                                      GameConstants.ShipBoundingSphereScale);
                for (int j = 0; j < bulletList.Length; j++)
                {
                    if (bulletList[j].isActive)
                    {
                        BoundingSphere bulletSphere = new BoundingSphere(bulletList[j].position, bulletModel.Meshes[0].BoundingSphere.Radius);

                        if (bulletSphere.Intersects(shipSphere1))
                        {
                            bulletList[j].isActive = false;
                            if(godBar1.isActive == false)
                            {
                                healthBar1.Value -= 0.5f;
                            }
                            soundExplosion2.Play();
                            bulletList[j].isActive = false;
                            break;
                        }
                    }
                }
            }

            if (ship.isActive)
            {
                BoundingSphere shipSphere = new BoundingSphere(
                 ship.Position, ship.Model.Meshes[0].BoundingSphere.Radius *
                                      GameConstants.ShipBoundingSphereScale);
                for (int j = 0; j < bulletList1.Length; j++)
                {
                    if (bulletList1[j].isActive)
                    {
                        BoundingSphere bulletSphere1 = new BoundingSphere(bulletList1[j].position, bulletModel1.Meshes[0].BoundingSphere.Radius);

                        if (bulletSphere1.Intersects(shipSphere))
                        {

                            bulletList1[j].isActive = false;
                            if(godBar.isActive == false)
                            {
                                healthBar.Value -= 0.5f;
                            }                            
                            soundExplosion2.Play();
                            bulletList1[j].isActive = false;
                            
                            break;
                        }
                    }
                }

            }


            ship.Update(gameTime);
            ship1.Update(gameTime);

            //************************************ god pickup interaction *******************************************

            if (ship1.isActive)
            {
                BoundingSphere shipSphere1 = new BoundingSphere(
                 ship1.Position, ship1.Model.Meshes[0].BoundingSphere.Radius *
                                      GameConstants.ShipBoundingSphereScale);
                for (int j = 0; j < pickupList.Length; j++)
                {
                    if (pickupList[j].isActive)
                    {
                        BoundingSphere pickupSphere2 = new BoundingSphere(pickupList[j].position, godPickup.Meshes[0].BoundingSphere.Radius);

                        if (pickupSphere2.Intersects(shipSphere1))
                        {
                            
                            pickupList[j].isActive = false;
                            healthBar1.Value = 100f;
                            godBar1.isActive = true;                            
                            godBar1.Value = godBar1.Width;
                            soundGod.Play();
                            pickupList[j].isActive = false;
                            break;
                        }
                    }
                }
            }

          if (ship.isActive)
            {
                BoundingSphere shipSphere = new BoundingSphere(
                 ship.Position, ship.Model.Meshes[0].BoundingSphere.Radius *
                                      GameConstants.ShipBoundingSphereScale);
                for (int j = 0; j < pickupList.Length; j++)
                {
                    if (pickupList[j].isActive)
                    {
                        BoundingSphere pickupSphere = new BoundingSphere(pickupList[j].position, godPickup.Meshes[0].BoundingSphere.Radius);

                        if (pickupSphere.Intersects(shipSphere))
                        {
                            pickupList[j].isActive = false;
                            healthBar.Value = 100f;
                            godBar.isActive = true;
                            godBar.Value = godBar.Width; 
                            soundGod.Play();
                            pickupList[j].isActive = false;
                            break;
                        }
                    }
                }
            }
//************************************* virus pickup interaction *****************************************
            if (ship.isActive)
            {
                BoundingSphere shipSphere = new BoundingSphere(
                 ship.Position, ship.Model.Meshes[0].BoundingSphere.Radius *
                                      GameConstants.ShipBoundingSphereScale);
                for (int j = 0; j < pickupList1.Length; j++)
                {
                    if (pickupList1[j].isActive)
                    {
                        BoundingSphere pickupSphere1 = new BoundingSphere(pickupList1[j].position, virusPickup.Meshes[0].BoundingSphere.Radius);

                        if (pickupSphere1.Intersects(shipSphere))
                        {
                            pickupList1[j].isActive = false;
                            ship.speed -= 500f;
                            virusBar.isActive = true;
                            virusBar.Value = virusBar.Width;
                            soundVirus.Play();
                            pickupList1[j].isActive = false;
                            break;
                        }
                    }
                }
            }

            if (ship1.isActive)
            {
                BoundingSphere shipSphere1 = new BoundingSphere(
                 ship1.Position, ship1.Model.Meshes[0].BoundingSphere.Radius *
                                      GameConstants.ShipBoundingSphereScale);
                for (int j = 0; j < pickupList1.Length; j++)
                {
                    if (pickupList1[j].isActive)
                    {
                        BoundingSphere pickupSphere1 = new BoundingSphere(pickupList1[j].position, virusPickup.Meshes[0].BoundingSphere.Radius);

                        if (pickupSphere1.Intersects(shipSphere1))
                        {
                            pickupList1[j].isActive = false;
                            ship1.speed -= 500f;
                            virusBar1.isActive = true;
                            virusBar1.Value = virusBar1.Width;
                            soundVirus.Play();
                            pickupList1[j].isActive = false;
                            break;
                        }
                    }
                }
            }
//***************************************************************************************************************************
                                   
            if (healthBar.Value == 0)
            {
                Vector2 posi = new Vector2(-7000, 0);
                ship.Position = new Vector3(posi, 0);
                ship.Velocity = Vector3.Zero;
                ship.Rotation = 0.0f;
                soundHyperspaceActivation.Play();                
                score1++;
                healthBar.Value = healthBar.Width;
               
            }
          


            if(healthBar1.Value == 0)
            {
                Vector2 posi1 = new Vector2(7000, 0);
                ship1.Position = new Vector3(posi1, 0);
                ship1.Velocity = Vector3.Zero;
                ship1.Rotation = 0.0f;
                soundHyperspaceActivation.Play();
                score++;
                healthBar1.Value = healthBar1.Width;
            }


            if(godBar.Value <= 0)
            {
                godBar.isActive = false;                
            }

            if(godBar1.Value <= 0)
            {
                godBar1.isActive = false;
            }

            if (virusBar.Value <= 0)
            {
                virusBar.isActive = false;
                ship.speed = 3500f;
            }

            if (virusBar1.Value <= 0)
            {
                virusBar1.isActive = false;
                ship1.speed = 3500f;
            }

            godBar.Update();
            virusBar.Update();
            healthBar.Update();
            healthBar1.Update();
            godBar1.Update();
            virusBar1.Update();
            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            //*********************** Player sound state controls ****************************
            if (InputManager.IsKeyPressed(Keys.W)) // Player 1
            {
                if (soundEngineInstance.State == SoundState.Stopped)
                {
                    soundEngineInstance.Volume = 0.75f;
                    soundEngineInstance.IsLooped = true;
                    soundEngineInstance.Play();
                }
                else
                    soundEngineInstance.Resume();
            }
            else if (InputManager.IsKeyReleased(Keys.W)) // Player 1
            {
                if (soundEngineInstance.State == SoundState.Playing)
                    soundEngineInstance.Pause();
            }


            if (InputManager.IsKeyPressed(Keys.Up)) // Player 2
            {
                if (soundEngineInstance1.State == SoundState.Stopped)
                {
                    soundEngineInstance1.Volume = 0.75f;
                    soundEngineInstance1.IsLooped = true;
                    soundEngineInstance1.Play();
                }
                else
                    soundEngineInstance1.Resume();
            }
            else if (InputManager.IsKeyReleased(Keys.Up)) // Player 2
            {
                if (soundEngineInstance1.State == SoundState.Playing)
                    soundEngineInstance1.Pause();
            }

            //*********************** returning to map **********************************
            if (InputManager.IsKeyDown(Keys.Z)) // Player 1
            {
                ship.Position = Vector3.Zero;
                ship.Velocity = Vector3.Zero;
                ship.Rotation = 0.0f;
                soundHyperspaceActivation.Play();
            }

            if (InputManager.IsKeyDown(Keys.O)) // Player 2
            {
                ship1.Position = Vector3.Zero;
                ship1.Velocity = Vector3.Zero;
                ship1.Rotation = 0.0f;
                soundHyperspaceActivation.Play();
            }

            //********************* Bullet shooting ***************************************************
            if (ship.isActive && InputManager.IsKeyDown(Keys.Space))  // Player 1
            {
                //add another bullet.  Find an inactive bullet slot and use it
                //if all bullets slots are used, ignore the user input
                for (int i = 0; i < GameConstants.NumBullets; i++)
                {
                    if (!bulletList[i].isActive)
                    {
                        bulletList[i].direction = ship.RotationMatrix.Forward;
                        bulletList[i].speed = GameConstants.BulletSpeedAdjustment;
                        bulletList[i].position = ship.Position + (200 * bulletList[i].direction);
                        bulletList[i].isActive = true;
                        soundWeaponsFire.Play();
                        break; //exit the loop
                    }
                }
            }

            if (ship1.isActive && InputManager.IsKeyPressed(Keys.P)) // Player 2
            {
                //add another bullet.  Find an inactive bullet slot and use it
                //if all bullets slots are used, ignore the user input
                for (int i = 0; i < GameConstants.NumBullets; i++)
                {
                    if (!bulletList1[i].isActive)
                    {
                        bulletList1[i].direction = ship1.RotationMatrix.Backward;
                        bulletList1[i].speed = GameConstants.BulletSpeedAdjustment;
                        bulletList1[i].position = ship1.Position + (200 * bulletList1[i].direction);
                        bulletList1[i].isActive = true;
                        soundWeaponsFire1.Play();          
                        break; //exit the loop
                    }
                }
            }


        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
          //  GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spriteBatch.Begin();
            spriteBatch.Draw(stars, new Rectangle(0, 0, 1280, 960), Color.White);
            if(godBar1.isActive == true)
            {
                godBar1.Draw(spriteBatch);
            }
            
            if(virusBar1.isActive == true)
            {
                virusBar1.Draw(spriteBatch);
            }            
            healthBar.Draw(spriteBatch);
            healthBar1.Draw(spriteBatch);
            if(godBar.isActive == true)
            {
                godBar.Draw(spriteBatch);
            }
            if(virusBar.isActive == true)
            {
                virusBar.Draw(spriteBatch);
            }
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(100, 50), Color.GreenYellow);
            spriteBatch.DrawString(font, "Score: " + score1, new Vector2(1050, 50), Color.GreenYellow);
            if(score == 3)
            {
                spriteBatch.DrawString(font, "Player 1 Wins with a lead of " + (score - score1), new Vector2(500, 500), Color.GreenYellow);
                healthBar.isActive = false;
                healthBar1.isActive = false;
                godBar.isActive = false;
                godBar1.isActive = false;
                virusBar.isActive = false;
                virusBar1.isActive = false;
                for(int i = 0; i < pickupList1.Length; i++)
                {
                    pickupList1[i].isActive = false;
                }
                for (int i = 0; i < pickupList.Length; i++)
                {
                    pickupList[i].isActive = false;
                }
            }
            

            if(score1 == 3)
            {
                spriteBatch.DrawString(font, "Player 2 Wins with a lead of " + (score1 - score), new Vector2(500, 500), Color.GreenYellow);
                healthBar.isActive = false;
                healthBar1.isActive = false;
                godBar.isActive = false;
                godBar1.isActive = false;
                virusBar.isActive = false;
                virusBar1.isActive = false;
                for (int i = 0; i < pickupList1.Length; i++)
                {
                    pickupList1[i].isActive = false;
                }
                for (int i = 0; i < pickupList.Length; i++)
                {
                    pickupList[i].isActive = false;
                }
            }
            


            spriteBatch.End();

          /*  GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;*/


            if (ship.isActive) // draw the ship
            {
                Matrix shipTransformMatrix = ship.RotationMatrix * Matrix.CreateTranslation(ship.Position);
                DrawModel(ship.Model, shipTransformMatrix, ship.Transforms);
            }

            if (ship1.isActive) // draw the ship
            {
                Matrix shipTransformMatrix1 = ship1.RotationMatrix * Matrix.CreateTranslation(ship1.Position);
                DrawModel(ship1.Model, shipTransformMatrix1, ship1.Transforms);
            }

            for (int i = 0; i < GameConstants.Numvirus; i++)
            {
                if (pickupList1[i].isActive)
                {
                    Matrix pickupTransform1 =
                    Matrix.CreateTranslation(pickupList1[i].position);
                    DrawModel(virusPickup, pickupTransform1, virusTransforms);
                }
            }

            for (int i = 0; i < GameConstants.Numgods; i++)
            {
                if (pickupList[i].isActive)
                {
                    Matrix pickupTransform =
                    Matrix.CreateTranslation(pickupList[i].position);
                    DrawModel(godPickup, pickupTransform, godTransforms);
                }
            }

            
            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList[i].isActive)
                {
                    Matrix bulletTransform =
                      Matrix.CreateTranslation(bulletList[i].position);
                    DrawModel(bulletModel, bulletTransform, bulletTransforms);
                }
            }

            for (int i = 0; i < GameConstants.NumBullets; i++)
            {
                if (bulletList1[i].isActive)
                {
                    Matrix bulletTransform1 =
                      Matrix.CreateTranslation(bulletList1[i].position);
                    DrawModel(bulletModel1, bulletTransform1, bulletTransforms1);
                }
            }


           /* GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            particleEffect.CurrentTechnique = particleEffect.Techniques["particle"];
            particleEffect.CurrentTechnique.Passes[0].Apply();
            particleEffect.Parameters["ViewProj"].SetValue(viewMatrix * projectionMatrix);
            particleEffect.Parameters["World"].SetValue(Matrix.Identity);
            particleEffect.Parameters["CamIRot"].SetValue(
            Matrix.Invert(Matrix.CreateFromQuaternion(camera.Transform.Rotation)));
            particleEffect.Parameters["Texture"].SetValue(particleTex);
            particleManager.Draw(GraphicsDevice);*/


            base.Draw(gameTime);
        }

        public static void DrawModel(Model model, Matrix modelTransform,
                                               Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                        absoluteBoneTransforms[mesh.ParentBone.Index] *
                        modelTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }




    }
}
