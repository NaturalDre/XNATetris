using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Tetris
{
    /// <summary>
    /// Data representation of a single rotation.
    /// </summary>
    /// <remarks> This class keeps track of all the filled-in
    /// points of a rotation. </remarks>
    public sealed class Rotation
    {
        readonly List<Point> _filledCells;
        readonly int _size = 0;

        public Rotation(List<Point> filledCells, int size)
        {
            Debug.Assert(filledCells.Count != 0);
            this._filledCells = filledCells;
            this._size = size;
        }

        public int FilledCellCount { get { return this._filledCells.Count; } }
        public int Size { get { return this._size; } }

        /// <summary>
        /// Is (row, column) a filled position on this rotation?
        /// </summary>
        public bool IsCellFilled(int row, int column)
        {
            return this._filledCells.Contains(new Point(column, row));
        }

        /// <summary>
        /// Is (row, column) a filled position on this rotation?
        /// </summary>
        public bool IsCellFilled(Point cellPosition)
        {
            return this.IsCellFilled(cellPosition.Y, cellPosition.X);
        }
    }

    public sealed class Block
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
        private readonly Color _color = TetrisModel.EmptySpaceColor;
        private readonly int _size;

        /// <summary>
        /// A list of all possible rotations;
        /// </summary>
        private readonly List<Rotation> _rotationList = new List<Rotation>();
        /// <summary>
        /// The current rotation this object is presenting.
        /// </summary>
        private int _currentRotationId;
        private Point _position = Point.Zero;

        /// <summary>
        /// Create a new instance of the block.
        /// </summary>
        /// <param name="game"> Instance of the TetrisGame object. </param>
        /// <param name="size"> The amount of Rows and Columns each rotation has. </param>
        /// <exception cref="Exception"> Thrown if the size of each Rotation doesn't 
        /// match our size. </exception>
        private Block(List<Rotation> rotations, Color color, int size)
        {
            //this.tetrisGame = game;

            Debug.Assert(color != TetrisModel.EmptySpaceColor);
            Debug.Assert(rotations.Count != 0);

            if (rotations.Any(rotationItem => rotationItem.Size != size))
                throw new Exception("All rotations do not match the block size.");

            _color = color;
            _rotationList = rotations;
            _size = size;
            ChangeCurrentRotation(0);
            // Initial spawning location
            Column = 3;
        }

        public Block(Color color, int size, params List<Point>[] lists)
        {
            Debug.Assert(color != TetrisModel.EmptySpaceColor);
            Debug.Assert(lists.Length != 0);

            _color = color;
            _size = size;

            foreach (var list in lists)
            {
                Debug.Assert(list.Count != 0);
                _rotationList.Add(new Rotation(list, _size));
            }
        }

        public Color Color
        {
            get { return _color; }
        }

        /// <summary>
        ///     Returns a list containing representations of each rotation of this block.
        /// </summary>
        public List<Rotation> Rotations
        {
            get { return _rotationList; }
        }


        /// <summary>
        ///     The ID used to identify the rotation we're currently representing.
        /// </summary>
        private int CurrentRotationID
        {
            get { return _currentRotationId; }
        }

        private void ChangeCurrentRotation(int rotationID)
        {
            _currentRotationId = EnforceRotationIDLimits(rotationID);
        }

        /// <summary>
        ///     Returns an object representing the current rotation.
        /// </summary>
        public Rotation CurrentRotation
        {
            get { return Rotations[CurrentRotationID]; }
        }


        public Rotation GetNextRotation(RotationDirections direction)
        {
            int id = CurrentRotationID;
            if (direction == RotationDirections.Right)
                id = EnforceRotationIDLimits(id + 1);
            else if (direction == RotationDirections.Left)
                id = EnforceRotationIDLimits(id - 1);

            return Rotations[id];
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
                id = RotationsPerBlock - 1;
                // There are no IDs >= the amount of rotations
            else if (id >= RotationsPerBlock)
                // If the requested ID is greater than the maximum
                // rotation ID, we want to return the lowest rotation ID.
                id = 0;

            return id;
        }

        /// <summary>
        ///     The row that this Block is current located.
        /// </summary>
        public int Row
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        /// <summary>
        ///     The column that this Block is currently located.
        /// </summary>
        public int Column
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public void MoveTo(Point position)
        {
            _position = position;
        }

        public Point Location
        {
            get { return _position; }
        }


        /// <summary>
        ///     The amount of rows that each rotation of this block has.
        /// </summary>
        public int Size
        {
            get { return _size; }
        }

        /// <summary>
        ///     Rotate the block counterclockwise. Will perform checks and wallkicks.
        /// </summary>
        public void RotateLeft()
        {
            ChangeCurrentRotation(CurrentRotationID - 1);
        }

        /// <summary>
        ///     Rotate the block clockwise. WIll perform checks and wallkicks.
        /// </summary>
        public void RotateRight()
        {
            ChangeCurrentRotation(CurrentRotationID + 1);
        }

        /// <summary>
        ///     Is position (row, column) filled in?
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool IsCellFilled(int row, int column)
        {
            return CurrentRotation.IsCellFilled(row, column);
        }
    }
}
