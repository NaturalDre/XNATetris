using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    /// <summary>
    ///     SRS rotation will be used: http://tetris.wikia.com/wiki/SRS
    ///     This will be a 10x22 board. Rows 0 and 1 will be hidden, per the Tetris rules.
    /// </summary>
    internal class TetrisModel
    {
        public enum GameStates
        {
            NotRunning,
            Running,
            GameOver
        }

        /// <summary>
        ///     How much time should pass for gravity to be applied.
        /// </summary>
        /// <seealso cref="SoftDrop"/>
        /// <seealso cref="HardDrop"/>
        private static readonly TimeSpan GravityCooldown;

        /// <summary>
        ///     The color that represents an empty space on the Tetris board.
        /// </summary>
        public static readonly Color EmptySpaceColor;

        /// <summary>
        ///     The amount of rows the Tetris board should have.
        /// </summary>
        public const int BoardRows = 22;

        /// <summary>
        ///     The amount of columns the Tetris board should have.
        /// </summary>
        public const int BoardColumns = 10;

        /// <summary>
        ///     Holds the Color value of each cell on the Tetris board.
        ///     TetrisModel.EmptySpaceColor means no color has been
        ///     locked/filled-in at that position.
        /// </summary>
        private readonly List<Color> _boardData = new List<Color>(
            BoardRows * BoardColumns);

        private GameStates _gameState;

        /// <summary>
        ///     Time remaining until gravity is applied.
        /// </summary>
        private TimeSpan _gravityTimer = GravityCooldown;

        private Block _currentBlock;
        private Block _nextBlock;
        private readonly List<Func<Block>> _blockFactories = new List<Func<Block>>();
        private readonly Random _random = new Random();
        private bool _canSoftDrop = true;

        static TetrisModel()
        {
            GravityCooldown = new TimeSpan(0, 0, 1);
            EmptySpaceColor = Color.Magenta;
        }

        public TetrisModel()
        {
            // 1 dimensional array to store data for each spot on the tetris board.
            // The Indexer of this class will take a 2D index and covnert it to 1D.
            //_boardData = new List<Color>(
            //    Enumerable.Repeat(
            //        EmptySpaceColor, BoardRows * BoardColumns));

            _gameState = GameStates.NotRunning;
        }

        /// <summary>
        ///     An array of Colors representing each cell on the Tetris board.
        /// </summary>
        public List<Color> BoardData
        {
            get { return _boardData; }
        }

        public GameStates GameState { get { return _gameState; } }


        /// <summary>
        /// The block currently being controlled by the player.
        /// </summary>
        public Block CurrentBlock
        {
            get { return _currentBlock; }
            private set { _currentBlock = value; }
        }

        /// <summary>
        ///  The block that will be spawned after the CurrentBlock is gone.
        /// </summary>
        public Block NextBlock
        {
            get { return _nextBlock; }
            private set { _nextBlock = value; }
        }

        /// <summary>
        ///     Returns the color of cell (row, column).
        /// </summary>
        /// <param name="row"> The row the cell is in (0-based). </param>
        /// <param name="column"> The column the cell is in (0-based). </param>
        /// <returns></returns>
        public Color GetCellColor(int row, int column)
        {
            // Convert the 2D index into a 1D
            int index = Utility.ConvertTo1DIndex(row, column, BoardColumns);
            Debug.Assert(index >= 0 && index < BoardData.Count);

            return BoardData[index];
        }

        private void ChangeCellColor(int row, int column, Color color)
        {
            // Convert the 2D index into a 1D
            int index = Utility.ConvertTo1DIndex(row, column, BoardColumns);
            Debug.Assert(index >= 0 && index < BoardData.Count);
            BoardData[index] = color;
        }


        /// <summary>
        ///     Checks with cell (row, column) is filled.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>
        ///     True if the cell is filled with a part of a block, OR
        ///     row or column exceeds the boundaries
        /// </returns>
        public bool IsCellFilled(int row, int column)
        {
            // Check that a valid cell's state is being requested.
            if (row < 0 || row >= BoardRows
                || column < 0 || column >= BoardColumns)
                return true; // Cells that are out of bounds are considered filled.

            return GetCellColor(row, column) != EmptySpaceColor;
        }

        /// <summary>
        ///     Process game logic for the current frame.
        /// </summary>
        /// <param name="gameTime"> The amount of time passed since the last frame. </param>
        public void Update(GameTime gameTime)
        {

            if (_gameState != GameStates.Running)
                return;

            _gravityTimer -= gameTime.ElapsedGameTime;
            // Check if enough time has passed for gravity to be applied.
            if (_gravityTimer <= TimeSpan.Zero)
            {
                ApplyGravity();
                // We add timeRemaining to gravityTimeSpan so we don't throw away any time.
                // (timeRemaining could have a negative value).
                _gravityTimer = GravityCooldown + _gravityTimer;
                Console.WriteLine("Gravity Applied.");
            }
        }

        public void StartGame()
        {
            if (_gameState == GameStates.Running || _gameState == GameStates.GameOver)
                EndGame();

            _boardData.InsertRange(0, Enumerable.Repeat(EmptySpaceColor, BoardRows * BoardColumns));
            _gameState = GameStates.Running;
            SpawnNextBlock();

        }

        public void EndGame()
        {
            if (_gameState != GameStates.Running || _gameState != GameStates.GameOver)
                return;

            _gameState = GameStates.NotRunning;
            _boardData.Clear();
        }

        public void GameOver()
        {
            _gameState = GameStates.GameOver;
        }

        /// <summary>
        ///     Removes the current block from play and spawns another one. GameState must be running.
        /// </summary>
        private void SpawnNextBlock()
        {
            if (_gameState != GameStates.Running)
                return;

            if (NextBlock == null)
                NextBlock = CreateRandomBlock();


            if (RotationFitsAt(NextBlock.CurrentRotation, NextBlock.Row, NextBlock.Column))
            {
                CurrentBlock = NextBlock;
                NextBlock = CreateRandomBlock();
            }
            else
                GameOver();

        }

        private void ResetGravityTimer()
        {
            _gravityTimer = GravityCooldown;
        }

        private bool RotationFitsAt(Rotation rotation, int row, int column)
        {
            // Loop through each cell of a rotation and see if its filled in
            // cells would collide with one of our filled in cells if the rotation
            // was placed at position (row, column).
            for (int curRow = 0; curRow < rotation.Size; curRow++)
            {
                for (int curColumn = 0; curColumn < rotation.Size; curColumn++)
                {
                    bool collision =
                        IsCellFilled(row + curRow, column + curColumn)
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
            bool shouldLock = !RotationFitsAt(
                CurrentBlock.CurrentRotation,
                CurrentBlock.Row + 1,
                CurrentBlock.Column);

            // If it can't fit, we'll need to lock it in at
            // it's current position.
            if (shouldLock)
            {
                LockCurrentBlock();
                RemoveFilledRows();
                SpawnNextBlock();
            }
            else // Otherwise, we can move it down one row.
                CurrentBlock.Row++;

            // Allow the player to perform a soft drop
            _canSoftDrop = true;
        }

        /// <summary>
        ///     Locks a piece into it's current position on the board.
        /// </summary>
        /// <remarks>
        ///     This does not create a new block, nor does it remove
        ///     the current block.
        /// </remarks>
        private void LockCurrentBlock()
        {
            // Loop through each cell in CurrentBlock's current rotation and
            // fill the corresponding position in on the tetris board.
            for (int cellRow = 0; cellRow < CurrentBlock.Size; cellRow++)
            {
                for (int cellColumn = 0; cellColumn < CurrentBlock.Size; cellColumn++)
                {
                    if (CurrentBlock.CurrentRotation.IsCellFilled(cellRow, cellColumn))
                    {
                        ChangeCellColor(CurrentBlock.Row + cellRow,
                            CurrentBlock.Column + cellColumn,
                            CurrentBlock.Color);
                    }
                }
            }
        }

        public bool MoveLeft()
        {
            if (RotationFitsAt(CurrentBlock.CurrentRotation, CurrentBlock.Row,
                CurrentBlock.Column - 1))
            {
                CurrentBlock.Column--;
                return true;
            }

            return false;
        }

        public bool MoveRight()
        {
            if (RotationFitsAt(CurrentBlock.CurrentRotation, CurrentBlock.Row,
                CurrentBlock.Column + 1))
            {
                CurrentBlock.Column++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to rotate the current block at its current position.
        /// Wallkicks are performed here, so the position of the block
        /// may change before the function returns.
        /// </summary>
        /// <param name="direction">The direction you want the block rotated.</param>
        /// <returns></returns>
        public bool RotateCurrentBlock(Block.RotationDirections direction)
        {
            Rotation nextRotation = CurrentBlock.GetNextRotation(direction);

            // Can we rotate the block in its current position?
            if (RotationFitsAt(nextRotation, CurrentBlock.Row, CurrentBlock.Column))
            {
                if (direction == Block.RotationDirections.Left)
                    CurrentBlock.RotateLeft();
                else
                    CurrentBlock.RotateRight();
            }

            // Will it rotate if it was moved one column to the right?
            else if ((CurrentBlock.Column + 1) < TetrisModel.BoardColumns && // Ensure it can move to the right
                    RotationFitsAt(nextRotation, CurrentBlock.Row, CurrentBlock.Column + 1))
            {
                CurrentBlock.Column++;
                if (direction == Block.RotationDirections.Left)
                    CurrentBlock.RotateLeft();
                else
                    CurrentBlock.RotateRight();
            }
            // Will it rotate if it was moved one column to the left?
            else if ((CurrentBlock.Column - 1) >= 0 && // Ensure it can move to the left
                    RotationFitsAt(nextRotation, CurrentBlock.Row, CurrentBlock.Column - 1))
            {
                CurrentBlock.Column--;
                if (direction == Block.RotationDirections.Left)
                    CurrentBlock.RotateLeft();
                else
                    CurrentBlock.RotateRight();
            }
            else
                return false;

            return true;
        }

        /// <summary>
        ///     Drops the block as far down as possible, but does
        ///     not immedietly apply gravity. Gravity timer resets.
        /// </summary>
        /// <remarks>
        ///     Once a soft drop is performed, another cannot occur
        ///     until gravity is applied.
        /// </remarks>
        public void SoftDrop()
        {
            if (_canSoftDrop)
            {
                CurrentBlock.MoveTo(FindDropPosition());
                ResetGravityTimer();
                _canSoftDrop = false;
            }
        }

        /// <summary>
        ///     Drop the block as far down as possible and
        ///     immedietly apply gravity.
        /// </summary>
        public void HardDrop()
        {
            CurrentBlock.MoveTo(FindDropPosition());
            ApplyGravity();
        }

        /// <summary>
        ///     Add a factory to our list of factories. Newly spawned blocks
        ///     will be created by randomly selecting a factory.
        /// </summary>
        /// <param name="factoryDelegate"></param>
        public void AddBlockFactory(Func<Block> factoryDelegate)
        {
            _blockFactories.Add(factoryDelegate);
        }

        /// <summary>
        ///     Create a Block using one of our factories.
        /// </summary>
        /// <returns></returns>
        private Block CreateRandomBlock()
        {
            Debug.Assert(_blockFactories.Count != 0);
            int index = _random.Next(0, _blockFactories.Count);

            return _blockFactories[index]();
        }

        /// <summary>
        ///     How many cells in the specified row are filled in
        ///     on the Tetris board. If it equals TetrisModel.BoardColumns,
        ///     then the entire row is filled and should be cleared.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private int FillCountAtRow(int row)
        {
            int count = 0;
            for (int column = 0; column < BoardColumns; column++)
            {
                if (IsCellFilled(row, column))
                    ++count;
            }
            return count;
        }

        /// <summary>
        ///     Remove all rows that are filled.
        /// </summary>
        private void RemoveFilledRows()
        {
            // Stores the index of each row that we're going to delete.
            var rowsToDelete = new List<int>();

            // Loop through all rows and mark the ones where every column
            // is filled in for removal.
            for (int row = BoardRows - 1; row >= 0; row--)
            {
                if (FillCountAtRow(row) == BoardColumns)
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
                    BoardData.RemoveRange(row * BoardColumns,
                        BoardColumns);
                }
                // Add an empty cell for every one we removed.
                _boardData.InsertRange(0,
                    Enumerable.Repeat(
                        Color.Magenta, BoardColumns * rowsToDelete.Count));
            }
        }

        /// <summary>
        ///     The location the current block would be if it were to
        ///     drop directly down until it it would lock into place.
        /// </summary>
        /// <returns></returns>
        public Point FindDropPosition()
        {
            Point position = CurrentBlock.Location;
            // Increase the current block's row and check to see if
            // it can fit into the row after that position.
            for (; position.Y < BoardRows; position.Y++)
            {
                if (!RotationFitsAt(CurrentBlock.CurrentRotation,
                    position.Y + 1, position.X))
                    break; // position's current value is the drop position.
            }

            return position;
        }
    }
}
