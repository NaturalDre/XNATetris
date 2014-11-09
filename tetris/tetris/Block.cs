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
    /// Represents a rotation. It contains the point for each filled spot on
    /// a block's grid.
    /// </summary>
    public class Rotation
    {
        /// <summary>
        /// The amount of points filled with a color in a tetris block (Always 4).
        /// </summary>
        public const int FilledCellsPerBlock = 4;
        private List<Point> filledCells;

        public Rotation()
        {
            this.filledCells =
                new List<Point>(
                    Enumerable.Repeat<Point>(Point.Zero, FilledCellsPerBlock));
        }

        public Rotation(Rotation other)
        {
            this.filledCells = new List<Point>(other.filledCells);
        }

        /// <summary>
        /// Checks whether a cell is filled. (0-based)
        /// </summary>
        /// <param name="row"> The row of the cell. </param>
        /// <param name="column"> The column of the cell. </param>
        /// <returns></returns>
        public bool IsCellFilled(int row, int column)
        {
            return this.filledCells.Contains(new Point(column, row));
        }

        /// <summary>
        /// Move 1 of the 4 filled cells to another location in the rotation grid. (0-based)
        /// </summary>
        /// <param name="cellIndex"> The cell you want to move. </param>
        /// <param name="moveToCell"> Where you want the cell moved to. </param>
        public void SetFilled(int cellIndex, Point moveToCell)
        {
            Debug.Assert(cellIndex >= 0 && cellIndex < 4);
            this.filledCells[cellIndex] = moveToCell;
        }

        /// <summary>
        /// Move 1 of the 4 filled cells to another location in the rotation grid. (0-based)
        /// </summary>
        /// <param name="cellIndex"> The cell you want to move. </param>
        /// <param name="moveToRow"> The row you want to move it to. </param>
        /// <param name="moveToColumn"> The column you want to move it to. </param>
        public void MoveFilledCell(int cellIndex, int moveToRow, int moveToColumn)
        {
            Debug.Assert(cellIndex >= 0 && cellIndex < 4);
            this.filledCells[cellIndex] = new Point(moveToColumn, moveToRow);
        }
    }

    sealed class Block
    {
        // Directions a block can rotate.
        public enum RotationDirections
        {
            Right,
            Left
        };


        // public  Func<Block> Factory();
        /// <summary>
        /// The amount of different rotations a block has.
        /// It is 4 for every piece using the SRS system.
        /// http://tetris.wikia.com/wiki/SRS
        /// </summary>
        public const byte RotationsPerBlock = 4;
        //private readonly TetrisGame tetrisGame;
        private readonly Color color = TetrisModel.EmptySpaceColor;
        private readonly int size;

        /// <summary>
        /// A list of all possible rotations;
        /// </summary>
        private List<Rotation> rotations = new List<Rotation>();
        /// <summary>
        /// The current rotation this object is presenting.
        /// </summary>
        private int currentRotationID;
        private Point position = Point.Zero;

        /// <summary>
        /// Create a new instance of the block.
        /// </summary>
        /// <param name="game"> Instance of the TetrisGame object. </param>
        /// <param name="size"> The amount of Rows and Columns each rotation has. </param>
        /// <param name="rotationCount"> The amount of rotations this block has. </param>
        public Block(Color color, int size)
        {
            //this.tetrisGame = game;

            Debug.Assert(color != TetrisModel.EmptySpaceColor);
            this.color = color;

            for (int i = 0; i < Block.RotationsPerBlock; i++)
                this.rotations.Add(new Rotation());

            this.size = size;
            this.ChangeCurrentRotation(0);
            // Initial spawning location
            this.Column = 3;
        }

        public Color Color
        {
            get { return this.color; }
        }

        /// <summary>
        /// Returns a list containing representations of each rotation of this block.
        /// </summary>
        public List<Rotation> Rotations { get { return this.rotations; } }


        /// <summary>
        /// The ID used to identify the rotation we're currently representing.
        /// </summary>
        private int CurrentRotationID { get { return this.currentRotationID; } }

        private void ChangeCurrentRotation(int rotationID)
        {
            this.currentRotationID = Block.EnforceRotationIDLimits(rotationID);
        }

        /// <summary>
        /// Returns an object representing the current rotation.
        /// </summary>
        public Rotation CurrentRotation { get { return this.Rotations[this.CurrentRotationID]; } }


        public Rotation GetNextRotation(RotationDirections direction)
        {
            int id = this.CurrentRotationID;
            if (direction == RotationDirections.Right)
                id = Block.EnforceRotationIDLimits(id + 1);
            else if (direction == RotationDirections.Left)
                id = Block.EnforceRotationIDLimits(id - 1);

            return this.Rotations[id];
        }


        /// <summary>
        /// Enforces rotation ID limits. A few functions use this exact code, so
        /// a helper function was made to avoid repeated code.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Returns the ID passed in if it falls within the limit.
        ///     Returns 0 if it falls below the limits.
        ///     Returns Block.RotationsPerBlock - 1 if it falls above the limit</returns>
        static int EnforceRotationIDLimits(int id)
        {
            // There are no IDs below 0
            if (id < 0)
                // If the requested ID is below zero, we want to
                // return the maximum rotation ID.
                id = Block.RotationsPerBlock - 1;
            // There are no IDs >= the amount of rotations
            else if (id >= Block.RotationsPerBlock)
                // If the requested ID is greater than the maximum
                // rotation ID, we want to return the lowest rotation ID.
                id = 0;

            return id;
        }

        /// <summary>
        /// The row that this Block is current located.
        /// </summary>
        public int Row
        {
            get { return this.position.Y; }
            set { this.position.Y = value; }
        }
        /// <summary>
        /// The column that this Block is currently located.
        /// </summary>
        public int Column
        {
            get { return this.position.X; }
            set { this.position.X = value; }
        }

        public void MoveTo(Point position) { this.position = position; }

        public Point Location { get { return this.position; } }


        /// <summary>
        /// The amount of rows that each rotation of this block has.
        /// </summary>
        public int Size { get { return this.size; } }
        /// <summary>
        /// Rotate the block counterclockwise. Will perform checks and wallkicks.
        /// </summary>
        public void RotateLeft()
        {
            this.ChangeCurrentRotation(this.CurrentRotationID - 1);
        }
        /// <summary>
        /// Rotate the block clockwise. WIll perform checks and wallkicks.
        /// </summary>
        public void RotateRight()
        {
            this.ChangeCurrentRotation(this.CurrentRotationID + 1);
        }
        /// <summary>
        /// Is position (row, column) filled in?
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool IsCellFilled(int row, int column)
        {
            return this.CurrentRotation.IsCellFilled(row, column);
        }
    }
}
