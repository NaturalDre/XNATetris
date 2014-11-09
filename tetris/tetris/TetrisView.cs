using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    class TetrisView
    {
        private TetrisGame tetrisGame;
        private TetrisModel tetrisModel;
        private SpriteBatch spriteBatch;
        /// <summary>
        /// Texture that will be used to draw each filled-in cell that makes up a block.
        /// Use Spritebatch to tint it to the desired color.
        /// </summary>
        private readonly Texture2D blockTexture;
        private readonly Texture2D areaTexture;
        private readonly Texture2D backgroundTexture;

        public TetrisView(TetrisGame tetrisGame, TetrisModel tetrisModel)
        {
            Debug.Assert(tetrisGame != null);
            Debug.Assert(tetrisModel != null);

            this.tetrisGame = tetrisGame;
            this.tetrisModel = tetrisModel;
            this.spriteBatch = new SpriteBatch(tetrisGame.GraphicsDevice);

            try 
            { 
                this.blockTexture = this.tetrisGame.Content.Load<Texture2D>(@"Images/Square");
                this.areaTexture = this.tetrisGame.Content.Load<Texture2D>(@"Images/areaBackground");
                this.backgroundTexture = this.tetrisGame.Content.Load<Texture2D>(@"Images/tetrisBackground");
            }
            catch (Microsoft.Xna.Framework.Content.ContentLoadException exception) 
            {
                // Mostly for testing.
                Console.WriteLine("Error loading the textures: {0}", exception.ToString());
                Console.WriteLine("\n\nPress enter to close the game.");
                Console.ReadLine();
                tetrisGame.Exit();
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Test code

            // Draw the background.
            spriteBatch.Draw(this.backgroundTexture, new Vector2(0, 0), Color.White);

            // Draw the area that the pieces will fall inside of.
            Rectangle area = new Rectangle(
                0,
                0,
                TetrisModel.BoardColumns * TetrisModel.CellSizeInPixels,
                TetrisModel.BoardRows * TetrisModel.CellSizeInPixels);
            spriteBatch.Draw(this.areaTexture, area, Color.Black);

            // End test code




            // Loop through each cell on the game board
            for (int row = 0; row < TetrisModel.BoardRows; row++)
            {
                for (int column = 0; column < TetrisModel.BoardColumns; column++)
                {
                    // If cell (row,col) on the Tetris board is occupied with a
                    // color that is NOT Color.Magenta, we will draw it.
                    // Note: Color.Magenta is considered empty space.
                    if (this.tetrisModel.IsCellFilled(row, column))
                        spriteBatch.Draw(
                            this.blockTexture,
                            new Rectangle(
                                column * TetrisModel.CellSizeInPixels,
                                row * TetrisModel.CellSizeInPixels,
                                TetrisModel.CellSizeInPixels,
                                TetrisModel.CellSizeInPixels),
                            this.tetrisModel.GiveCellColor(row, column));
                }
            }

            // Draw the "ghost" piece.
            Point ghostPosition = this.tetrisModel.DropPosition();
            DrawRotation(
                this.tetrisModel.CurrentBlock.Rotation,
                this.tetrisModel.CurrentBlock.Size,
                ghostPosition,
                Color.White);

            // Draw the piece being controlled by the player.
            DrawRotation(this.tetrisModel.CurrentBlock);

            spriteBatch.End();
        }
        /// <summary>
        /// Draw's a rotation at a specified part of the Tetris board.
        /// </summary>
        /// <param name="rotation"> The rotation to draw. </param>
        /// <param name="rotationSize"> How many rows and columns the rotation has. 3 = (3x3). </param>
        /// <param name="position"> The position on the Tetris board to draw the rotation. </param>
        /// <param name="colorTint"> The color of each filled-in cell. </param>
        private void DrawRotation(Rotation rotation, int rotationSize, Point position, Color colorTint)
        {
            for (int rotationRow = 0; rotationRow < rotationSize; rotationRow++)
            {
                for (int rotationColumn = 0; rotationColumn < rotationSize; rotationColumn++)
                {
                    if (rotation.IsCellFilled(rotationRow, rotationColumn))
                        this.spriteBatch.Draw(
                            this.blockTexture,
                            new Rectangle(
                            // Block's use their top-left corner as their origin,
                            // so we add the position of the rotation's filled-in
                            // cells to the origin to get Tetris board cell
                            // they are occupying. We then multiply it by the
                            // specified pixel size of each cell to know
                            // where to draw them on the screen.
                                (position.X + rotationColumn) * TetrisModel.CellSizeInPixels,
                                (position.Y + rotationRow) * TetrisModel.CellSizeInPixels,
                                TetrisModel.CellSizeInPixels,
                                TetrisModel.CellSizeInPixels),
                            colorTint);
                }
            }
        }
        /// <summary>
        /// Helper function to draw a block's rotation based on the block's current position.
        /// </summary>
        /// <param name="block"> The block to draw. </param>
        private void DrawRotation(Block block)
        {
            DrawRotation(
                block.Rotation,
                block.Size,
                block.Position,
                block.Color);
        }
    }
}
