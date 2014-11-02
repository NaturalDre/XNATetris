﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    /// <summary>
    /// SRS rotation will be used: http://tetris.wikia.com/wiki/SRS
    /// This will be a 10x22 board. Rows 0 and 1 will be hidden, per the Tetris rules.
    /// </summary>
    class TetrisModel
    {
        /// <summary>
        /// How much time should pass for gravity to be applied.
        /// </summary>
        private static readonly TimeSpan GravityTimeSpan;
        /// <summary>
        /// The color that represents an empty space on the Tetris board.
        /// </summary>
        public static readonly Color EmptySpaceColor;
        /// <summary>
        /// How much time must pass before movement is allowed again.
        /// Prevents rotating too quickly.
        /// </summary>
        public static readonly TimeSpan MovementCooldown;
        /// <summary>
        /// How much time must pass before rotation is allowed again.
        /// Prevents moving too quickly.
        /// </summary>
        public static readonly TimeSpan RotationCooldown;

        /// <summary>
        /// The amount of rows the Tetris board should have.
        /// Note: The first 2 rows (0 and 1) will not be drawn.
        ///     The top left of the board is (0,0).
        /// </summary>
        public const int BoardRows = 22;
        /// <summary>
        /// The amount of columns the Tetris board should have.
        /// </summary>
        public const int BoardColumns = 10;
        // The size that each cell will be when drawn.
        public  const int CellSizeInPixels = 32;
        /// <summary>
        /// Holds the Color value of each cell on the Tetris board.
        /// TetrisModel.EmptySpaceColor means no color has been 
        /// locked/filled-in at that position.
        /// </summary>
        private readonly List<Color> boardData = new List<Color>(
            TetrisModel.BoardRows * TetrisModel.BoardColumns);

        /// <summary>
        /// Time remaining until gravity is applied.
        /// </summary>
        private TimeSpan gravityTimer = TetrisModel.GravityTimeSpan;
        /// <summary>
        /// Time remaining until the player can move. This only applies if
        /// This.MovementAllowed is false.
        /// </summary>
        private TimeSpan movementTimer = TetrisModel.MovementCooldown;
        /// <summary>
        /// Time remaining until the player can rotate. This only applies if
        /// this.RotationAllowed is false.
        /// </summary>
        private TimeSpan rotationTimer = TetrisModel.RotationCooldown;
        private Block currentBlock = null;
        private List<Func<Block>> blockFactories = new List<Func<Block>>();
        private Random random = new Random();

        static TetrisModel()
        {
            TetrisModel.GravityTimeSpan = new TimeSpan(0, 0, 1);
            TetrisModel.EmptySpaceColor = Color.Magenta;
            TetrisModel.MovementCooldown = new TimeSpan(0, 0, 0, 0, 100);
            TetrisModel.RotationCooldown = new TimeSpan(0, 0, 0, 0, 100);
        }

        public TetrisModel()
        {
            // 1 dimensional array to store data for each spot on the tetris board.
            // The Indexer of this class will take a 2D index and covnert it to 1D.
            this.boardData = new List<Color>(
                Enumerable.Repeat<Color>(
                    TetrisModel.EmptySpaceColor, TetrisModel.BoardRows * TetrisModel.BoardColumns));
        }

        /// <summary>
        /// An array of Colors representing each cell on the Tetris board. 
        /// </summary>
        private List<Color> BoardData { get { return this.boardData; } }

        /// <summary>
        /// The block currently being controlled by the player.
        /// </summary>
        public Block CurrentBlock
        {
            get { return this.currentBlock; }
            set { this.currentBlock = value; }
        }

        /// <summary>
        /// Is the player allowed to move?
        /// </summary>
        public bool MovementAllowed { get; private set; }
        /// <summary>
        /// Is the player allowed to rotate?
        /// </summary>
        public bool RotationAllowed { get; private set; }

        /// <summary>
        /// Get the value of a cell on the Tetris board.
        /// </summary>
        /// <param name="row"> The row the cell is in (0-based). </param>
        /// <param name="column"> The column the cell is in (0-based). </param>
        /// <returns></returns>
        public Color GetBoardData(int row, int column)
        {

            // Convert the 2D index into a 1D
            int index = Utility.ConvertTo1DIndex(row, column, TetrisModel.BoardColumns);
            Debug.Assert(index >= 0 && index < this.BoardData.Count);

            return this.boardData[index];
        }

        private void SetBoardData(int row, int column, Color color)
        {
            // Convert the 2D index into a 1D
            int index = Utility.ConvertTo1DIndex(row, column, TetrisModel.BoardColumns);
            Debug.Assert(index >= 0 && index < this.BoardData.Count);
            this.boardData[index] = color;
        }


        /// <summary>
        /// Checks with cell (row, column) is filled. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns> True if the cell is filled with a part of a block, OR 
        ///     row or column exceeds the boundaries </returns>
        bool IsCellFilled(int row, int column)
        {
            if (row < 0 || row >= TetrisModel.BoardRows
                || column < 0 || column >= TetrisModel.BoardColumns)
                return true;

            if (this.GetBoardData(row, column) != TetrisModel.EmptySpaceColor)
                return true;

            return false;
        }

        /// <summary>
        /// Process game logic for the current frame.
        /// </summary>
        /// <param name="gameTime"> The amount of time passed since the last frame. </param>
        public void Update(GameTime gameTime)
        {
            gravityTimer -= gameTime.ElapsedGameTime;
            // Check if enough time has passed for gravity to be applied.
            if (gravityTimer <= TimeSpan.Zero)
            {
                ApplyGravity();
                // We add timeRemaining to gravityTimeSpan so we don't throw away any time.
                // (timeRemaining could have a negative value).
                gravityTimer = TetrisModel.GravityTimeSpan + gravityTimer;
                Console.WriteLine("Gravity Applied.");
            }

            // If the player is not allowed to rotate, we'll count down until
            // the player can rotate again.
            if (!this.RotationAllowed)
            {
                this.rotationTimer -= gameTime.ElapsedGameTime;
                if (this.rotationTimer <= TimeSpan.Zero)
                {
                    this.RotationAllowed = true;
                    this.rotationTimer = TetrisModel.RotationCooldown;
                }
            }

            // If the player is not allowed to move, we'll count down until
            // the player can move again.
            if (!this.MovementAllowed)
            {
                this.movementTimer -= gameTime.ElapsedGameTime;
                if (this.movementTimer <= TimeSpan.Zero)
                {
                    this.MovementAllowed = true;
                    this.movementTimer = TetrisModel.RotationCooldown;
                }
            }
        }

        public void StartGame()
        {
            this.CurrentBlock = CreateRandomBlock();
        }

        public bool CanFit(int row, int column, Rotation rotation)
        {
            // Loop through each cell of a rotation and see if its filled in
            // cells would collide with one of our filled in cells if the rotation
            // was placed at position (row, column).
            for (int curRow = 0; curRow < Rotation.FilledCellsPerBlock; curRow++)
            {
                for (int curColumn = 0; curColumn < Rotation.FilledCellsPerBlock; curColumn++)
                {
                    bool collision =
                        this.IsCellFilled(row + curRow, column + curColumn)
                        && rotation.IsCellFilled(curRow, curColumn);

                    if (collision)
                        return false;
                }
            }

            return true;
        }

        private void ApplyGravity()
        {
            // Make sure the block can fit into the row we're about to
            // move it down to.
            bool shouldLock = !this.CanFit(
                this.CurrentBlock.Row + 1,
                this.CurrentBlock.Column,
                this.CurrentBlock.Rotation);

            // If it can't fit, we'll need to lock it in at
            // it's current position.
            if (shouldLock)
            {
                this.LockCurrentBlock();
                this.RemoveFilledRows();
                // Test code
                this.CurrentBlock = CreateRandomBlock();
                this.CurrentBlock.Row = 14;
            }
            else // Otherwise, we can move it down one row.
                this.CurrentBlock.Row++;
        }

        /// <summary>
        /// Locks a piece into it's current position on the board.
        /// </summary>
        private void LockCurrentBlock()
        {
            // Loop through each cell in CurrentBlock's current rotation and
            // fill the corresponding position in on the tetris board.
            for (int cellRow = 0; cellRow < Rotation.FilledCellsPerBlock; cellRow++)
            {
                for (int cellColumn = 0; cellColumn < Rotation.FilledCellsPerBlock; cellColumn++)
                {
                    if (this.CurrentBlock.Rotation.IsCellFilled(cellRow, cellColumn))
                    {
                        this.SetBoardData(this.CurrentBlock.Row + cellRow,
                            this.CurrentBlock.Column + cellColumn,
                                this.CurrentBlock.Color);
                    }
                }
            }
        }

        public void MoveLeft()
        {
            if (this.MovementAllowed && this.CanFit(this.CurrentBlock.Row,
                    this.CurrentBlock.Column - 1,
                    this.CurrentBlock.Rotation))
            {
                this.CurrentBlock.Column--;
                this.MovementAllowed = false;
            }
        }

        public void MoveRight()
        {
            if (this.MovementAllowed && this.CanFit(this.CurrentBlock.Row,
                    this.CurrentBlock.Column + 1,
                    this.CurrentBlock.Rotation))
            {
                this.CurrentBlock.Column++;
                this.MovementAllowed = false;
            }
        }

        public void RotateLeft()
        {
            if (this.RotationAllowed && this.CanFit(this.CurrentBlock.Row,
                    this.CurrentBlock.Column,
                    this.CurrentBlock.GetNextRotation()))
            {
                this.CurrentBlock.RotateLeft();
                this.RotationAllowed = false;
            }
        }

        public void RotateRight()
        {
            if (this.RotationAllowed && this.CanFit(this.CurrentBlock.Row,
                    this.CurrentBlock.Column,
                    this.CurrentBlock.GetNextRotation()))
            {
                this.CurrentBlock.RotateRight();
                this.RotationAllowed = false;
            }
        }

        /// <summary>
        /// Add a factory to our list of factories. Newly spawned blocks
        /// will be created by randomly selecting a factory.
        /// </summary>
        /// <param name="factoryDelegate"></param>
        public void AddBlockFactory(Func<Block> factoryDelegate)
        {
            this.blockFactories.Add(factoryDelegate);
        }
        /// <summary>
        /// Create a Block using one of our factories.
        /// </summary>
        /// <returns></returns>
        Block CreateRandomBlock()
        {
            Debug.Assert(this.blockFactories.Count != 0);
            int index = this.random.Next(0, this.blockFactories.Count);

            return this.blockFactories[index]();
        }
        /// <summary>
        /// How many cells in the specified row are filled in
        /// on the Tetris board.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private int GetFillCountOfRow(int row)
        {
            int count = 0;
            for (int column = 0; column < TetrisModel.BoardColumns; column++)
            {
                if (this.IsCellFilled(row, column))
                    ++count;
            }
            return count;
        }

        void RemoveFilledRows()
        {
            List<int> rowsToDelete = new List<int>();

            // Loop through all rows and mark the ones where every column
            // is filled in for removal.
            for (int row = TetrisModel.BoardRows - 1; row >= 0; row--)
            {
                if (GetFillCountOfRow(row) == TetrisModel.BoardColumns)
                    rowsToDelete.Add(row);
                // TO DO?: Break loop if fill count returns 0.
            }

            // Remove the rows from BoardData.
            if (rowsToDelete.Count != 0)
            {
                foreach (int row in rowsToDelete)
                {
                    // row * TetrisModel.BoardColumns will get you the first
                    // index of the row.
                    this.BoardData.RemoveRange(row * TetrisModel.BoardColumns,
                        TetrisModel.BoardColumns);
                }
                // Add an empty cell for every one we removed.
                this.boardData.InsertRange(0,
                    Enumerable.Repeat<Color>(
                        Color.Magenta, TetrisModel.BoardColumns * rowsToDelete.Count));
            }
        }

        public Point GetDropLocation()
        {
            Point position = this.CurrentBlock.Position;

            for (; position.Y < TetrisModel.BoardRows; position.Y++)
            {
                if (!this.CanFit(position.Y + 1, position.X, this.CurrentBlock.Rotation))
                    break;
            }

            return position;
        }
    }
}
