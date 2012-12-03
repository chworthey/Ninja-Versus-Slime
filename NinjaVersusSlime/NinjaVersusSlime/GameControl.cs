using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace NinjaVersusSlime
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameControl : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level currentLevel;
        int levelIndex = 0;
        SpriteFont font;
        Texture2D border;
        Texture2D ui;
        SoundEffectInstance music;

        public GameControl()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            Levels.LevelData.LoadLevels();

            font = Content.Load<SpriteFont>("Font");

            Messenger.Load(font, Helper.Resolution / 2f);

            border = Content.Load<Texture2D>("Border");

            ui = Content.Load<Texture2D>("UI");

            music = Content.Load<SoundEffect>("American At Heart").CreateInstance();
            music.IsLooped = true;
            music.Play();

            NextLevel(0);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            currentLevel.Update(gameTime);

            Messenger.Update(gameTime);

            base.Update(gameTime);
        }

        private void NextLevel(int indexAdd)
        {
            levelIndex += indexAdd;
            if (levelIndex >= Levels.LevelData.LevelText.Count)
                levelIndex = 0;
            currentLevel = new Level(levelIndex, Content, NextLevel);
            Messenger.DisplayMessage("Level " + (levelIndex + 1).ToString() + " Start!");
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            currentLevel.Draw(spriteBatch, gameTime);
            Messenger.Draw(spriteBatch);
            spriteBatch.Draw(border, Vector2.Zero, Color.White);
            spriteBatch.Draw(ui, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
