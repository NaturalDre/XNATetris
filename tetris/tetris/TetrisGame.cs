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
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        KeyboardState _previousKeyboardState;

        TetrisModel _tetrisModel;
        TetrisView _tetrisView;
        TetrisController _tetrisController;

        public TetrisGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = TetrisModel.BoardRows * TetrisView.CellSizeInPixels;
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
            _previousKeyboardState = new KeyboardState();
            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _tetrisModel = new TetrisModel();

            _tetrisModel.AddBlockFactory(BlockFactories.CreateIBlock);
            _tetrisModel.AddBlockFactory(BlockFactories.CreateJBlock);
            _tetrisModel.AddBlockFactory(BlockFactories.CreateLBlock);
            _tetrisModel.AddBlockFactory(BlockFactories.CreateOBlock);
            _tetrisModel.AddBlockFactory(BlockFactories.CreateSBlock);
            _tetrisModel.AddBlockFactory(BlockFactories.CreateTBlock);
            _tetrisModel.AddBlockFactory(BlockFactories.CreateZBlock);


            _tetrisView = new TetrisView(this, _tetrisModel);
            _tetrisController = new TetrisController(_tetrisModel);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            _tetrisModel = null;
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboarsState = Keyboard.GetState();
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _tetrisModel.Update(gameTime);
            _tetrisController.Update(gameTime);

            if (_previousKeyboardState.IsKeyUp(Keys.N) && currentKeyboarsState.IsKeyDown(Keys.N))
                _tetrisModel.StartGame();

            if (_tetrisModel.GameState == TetrisModel.GameStates.GameOver)
            {
                // Do something ... maybe ...
            }

            base.Update(gameTime);

            _previousKeyboardState = currentKeyboarsState;
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _tetrisView.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
