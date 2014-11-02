using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TetrisGame : Microsoft.Xna.Framework.Game
    {
        private const Keys MoveLeftKey = Keys.A;
        private const Keys MoveRightKey = Keys.D;
        private const Keys RotateLeftKey = Keys.Left;
        private const Keys RotateRightKey = Keys.Right;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TetrisModel tetrisModel;
        TetrisView tetrisView;

        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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
            // TODO: Add your initialization logic here

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

            // TODO: use this.Content to load your game content here
            this.tetrisModel = new TetrisModel();
            this.tetrisModel.AddBlockFactory(
                new Func<Block>(BlockFactories.CreateTBlock));
            this.tetrisModel.AddBlockFactory(
                new Func<Block>(BlockFactories.CreateLBlock));
            this.tetrisModel.StartGame();

            this.tetrisView = new TetrisView(this, tetrisModel);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            this.tetrisModel = null;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) == true)
                this.Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            bool moveLeftKeyIsDown = keyboardState.IsKeyDown(TetrisGame.MoveLeftKey);
            bool moveRightKeyIsDown = keyboardState.IsKeyDown(TetrisGame.MoveRightKey);
            bool rotateLeftKeyIsDown = keyboardState.IsKeyDown(TetrisGame.RotateLeftKey);
            bool rotateRightKeyIsDown = keyboardState.IsKeyDown(TetrisGame.RotateRightKey);

            if (moveLeftKeyIsDown && !moveRightKeyIsDown)
                this.tetrisModel.MoveLeft();
            else if (moveRightKeyIsDown && !moveLeftKeyIsDown)
                this.tetrisModel.MoveRight();

            if (rotateLeftKeyIsDown && !rotateRightKeyIsDown)
                this.tetrisModel.RotateLeft();
            else if (rotateRightKeyIsDown && !rotateLeftKeyIsDown)
                this.tetrisModel.RotateRight();

            // TODO: Add your update logic here
            this.tetrisModel.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.tetrisView.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
