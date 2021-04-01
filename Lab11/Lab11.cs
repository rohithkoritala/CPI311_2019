using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using CPI311.GameEngine;

namespace Lab11
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lab11 : Game
    {

        public class Scene
        {
            public delegate void CallMethod();
            public CallMethod Update;
            public CallMethod Draw;
            public Scene(CallMethod update, CallMethod draw)
            { Update = update; Draw = draw; }
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        SpriteFont font;
        Color background = Color.Blue;
       Button exitButton;

        List<GUIElement> guiElements;
        Dictionary<String, Scene> scenes;
        Scene currentScene;


        public Lab11()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(graphics);
            scenes = new Dictionary<string, Scene>();
            guiElements = new List<GUIElement>();
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

            scenes.Add("Menu", new Scene(MainMenuUpdate, MainMenuDraw));
            scenes.Add("Play", new Scene(PlayUpdate, PlayDraw));
            currentScene = scenes["Menu"];

            texture = Content.Load<Texture2D>("Square");
            font = Content.Load<SpriteFont>("font");

            GUIGroup group = new GUIGroup();
             exitButton = new Button();
            exitButton.Texture = texture;
            exitButton.Text = "Exit";
            exitButton.Bounds = new Rectangle(50, 50, 300, 20);

            exitButton.Action += ExitGame;
            // guiElements.Add(exitButton);
            group.Children.Add(exitButton);

            CheckBox optionBox = new CheckBox();
            optionBox.Texture = texture;
            optionBox.Box = texture;
            optionBox.Bounds = new Rectangle(50, 75, 300, 20);
            optionBox.Text = "Full Screen";
            optionBox.Action += MakeFullScreen;
            group.Children.Add(optionBox);

            guiElements.Add(group);

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
            Time.Update(gameTime);
            InputManager.Update();
         //   if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            currentScene.Update();
            exitButton.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);

            spriteBatch.Begin();
           
           // exitButton.Draw(spriteBatch, font);
            spriteBatch.End();
            currentScene.Draw();
            base.Draw(gameTime);
        }

        void MakeFullScreen(GUIElement element)
        {
            ScreenManager.Setup(!ScreenManager.IsFullScreen, ScreenManager.Width + 1, ScreenManager.Height + 1);
        }

        void ExitGame(GUIElement element)
        {
            background = (background == Color.White ? Color.Blue : Color.White);
            currentScene = scenes["Play"];
        }
        void MainMenuUpdate()
        {
            foreach (GUIElement element in guiElements)
                element.Update();
        }
        void MainMenuDraw()
        {
            spriteBatch.Begin();
            foreach (GUIElement element in guiElements)
                element.Draw(spriteBatch, font);
            spriteBatch.End();
        }
        void PlayUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currentScene = scenes["Menu"];
        }
        void PlayDraw()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Play Mode! Press \"Esc\" to go back",
                                                Vector2.Zero, Color.Black);
            spriteBatch.End();


        }

    }
    }
