using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Tetris
{
    static class BlockFactories
    {
        public static Block CreateIBlock()
        {
            // - - - -
            // X X X X
            // - - - -
            // - - - - 
            List<Point> firstRotation = new List<Point>()
            {
                new Point(0, 1), new Point(1, 1),
                new Point(2, 1), new Point(3, 1)
            };

            // - - X -
            // - - X -
            // - - X -
            // - - X -
            List<Point> secondRotation = new List<Point>()
            {
                new Point(2, 0), new Point(2, 1),
                new Point(2, 2), new Point(2, 3)
            };

            // - - - -
            // - - - - 
            // X X X X
            // - - - -
            List<Point> thirdRotation = new List<Point>()
            {
                new Point(0, 2), new Point(1, 2),
                new Point(2, 2), new Point(3, 2)
            };

            // - X - - 
            // - X - -
            // - X - - 
            // - X - -
            List<Point> fourthRotation = new List<Point>()
            {
                new Point(1, 0), new Point(1, 1),
                new Point(1, 2), new Point(1, 3)
            };

            Block block = new Block(Color.Cyan, 4,
                firstRotation,
                secondRotation,
                thirdRotation,
                fourthRotation);

            return block;
        }

        public static Block CreateJBlock()
        {
            // X - -
            // X X X 
            // - - -
            List<Point> firstRotation = new List<Point>
            { 
                new Point(0,0), new Point(0,1), 
                new Point(1,1), new Point(2,1)
            };

            // - X X 
            // - X -
            // - X -
            List<Point> secondRotation = new List<Point>
            { 
                new Point(1,0), new Point(2,0), 
                new Point(1,1), new Point(1,2)
            };

            // - - -
            // X X X
            // - - X
            List<Point> thirdRotation = new List<Point>
            { 
                new Point(0, 1), new Point(1, 1),
                new Point(2, 1), new Point(2, 2)
            };

            // - X -
            // - X -
            // X X 
            List<Point> fourthRotation = new List<Point>
            { 
                new Point(1, 0), new Point(1, 1),
                new Point(1, 2), new Point(0, 2)
            };

            Block block = new Block(Color.Blue, 3,
                firstRotation,
                secondRotation,
                thirdRotation,
                fourthRotation);

            return block;
        }

        public static Block CreateLBlock()
        {
            // - - -
            // - - X
            // X X X
            List<Point> firstRotation = new List<Point>()
            {
                new Point(2, 0), new Point(0, 1),
                new Point(1, 1), new Point(2, 1)
            };

            // - X -
            // - X -
            // - X X
            List<Point> secondRotation = new List<Point>()
            {
                new Point(1, 0), new Point(1, 1),
                new Point(1, 2), new Point(2, 2)
            };

            // - - -
            // X X X
            // X - -
            List<Point> thirdRotation = new List<Point>()
            {
                new Point(0, 1), new Point(1, 1),
                new Point(2, 1), new Point(0, 2)
            };

            // X X -
            // - X -
            // - X -
            List<Point> fourthRotation = new List<Point>()
            {
                new Point(0, 0), new Point(1, 0),
                new Point(1, 1), new Point(1, 2)
            };

            Block block = new Block(Color.Orange, 3,
                firstRotation,
                secondRotation,
                thirdRotation,
                fourthRotation);

            return block;
        }

        public static Block CreateOBlock()
        {


            // - X X - 
            // - X X -
            // - - - -
            List<Point> rotation = new List<Point>()
            {
                new Point(1, 0), new Point(2, 0),
                new Point(1, 1), new Point(2, 1)
            };


            // Lazy way of doing it for now. All rotations for
            // the O block are the same...
            Block block = new Block(Color.Yellow, 4,
                rotation,
                rotation,
                rotation,
                rotation);

            return block;
        }

        public static Block CreateSBlock()
        {
            // - X X
            // X X -
            // - - -
            List<Point> firstRotation = new List<Point>()
            {
                new Point(1, 0), new Point(2, 0),
                new Point(0, 1), new Point(1, 1)
            };

            // - X -
            // - X X
            // - - X
            List<Point> secondRotation = new List<Point>()
            {
                new Point(1, 0), new Point(1, 1),
                new Point(2, 1), new Point(2, 2)
            };

            // - - -
            // - X X
            // X X -
            List<Point> thirdRotation = new List<Point>()
            {
                new Point(1, 1), new Point(2, 1),
                new Point(0, 2), new Point(1, 2)
            };

            // X - -
            // X X -
            // - X -
            List<Point> fourthRotation = new List<Point>()
            {
                new Point(0, 0), new Point(0, 1),
                new Point(1, 1), new Point(1, 2)
            };

            Block block = new Block(Color.Green, 3,
                firstRotation,
                secondRotation,
                thirdRotation,
                fourthRotation);

            return block;
        }

        public static Block CreateTBlock()
        {
            // - X -
            // X X X
            // - - -
            List<Point> firstRotation = new List<Point>()
            {
                new Point(1, 0), new Point(0, 1),
                new Point(1, 1), new Point(2, 1)
            };

            // - X -
            // - X X
            // - X -
            List<Point> secondRotation = new List<Point>()
            {
                new Point(1, 0), new Point(1, 1),
                new Point(2, 1), new Point(1, 2)
            };

            // - - -
            // X X X
            // - X -
            List<Point> thirdRotation = new List<Point>()
            {
                new Point(0, 1), new Point(1, 1),
                new Point(2, 1), new Point(1, 2)
            };

            // - X -
            // X X -
            // - X
            List<Point> fourthRotation = new List<Point>()
            {
                new Point(1, 0), new Point(0, 1),
                new Point(1, 1), new Point(1, 2)
            };

            Block block = new Block(Color.Purple, 3,
                firstRotation,
                secondRotation,
                thirdRotation,
                fourthRotation);

            return block;
        }

        public static Block CreateZBlock()
        {
            // X X -
            // - X X
            // - - -
            List<Point> firstRotation = new List<Point>()
            {
                new Point(0, 0), new Point(1, 0),
                new Point(1, 1), new Point(2, 1)
            };

            // - - X
            // - X X
            // - X -
            List<Point> secondRotation = new List<Point>()
            {
                new Point(2, 0), new Point(1, 1),
                new Point(2, 1), new Point(1, 2)
            };

            // - - -
            // X X - 
            // - X X
            List<Point> thirdRotation = new List<Point>()
            {
                new Point(0, 1), new Point(1, 1),
                new Point(1, 2), new Point(2, 2)
            };
            
            // - X -
            // X X -
            // X - -
            List<Point> fourthRotation = new List<Point>()
            {
                new Point(1, 0), new Point(0, 1),
                new Point(1, 1), new Point(0, 2)
            };

            Block block = new Block(Color.Red, 3,
                firstRotation,
                secondRotation,
                thirdRotation,
                fourthRotation);

            return block;
        }

    }
}
