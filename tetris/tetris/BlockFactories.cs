using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris
{
    static class BlockFactories
    {
        public static Block CreateIBlock()
        {
            Block block = new Block(Color.Cyan, 4);

            // - - - -
            // X X X X
            // - - - -
            // - - - - 
            block.Rotations[0].SetFilled(0, new Point(0, 1));
            block.Rotations[0].SetFilled(1, new Point(1, 1));
            block.Rotations[0].SetFilled(2, new Point(2, 1));
            block.Rotations[0].SetFilled(3, new Point(3, 1));

            // - - X -
            // - - X -
            // - - X -
            // - - X -
            block.Rotations[1].SetFilled(0, new Point(2, 0));
            block.Rotations[1].SetFilled(1, new Point(2, 1));
            block.Rotations[1].SetFilled(2, new Point(2, 2));
            block.Rotations[1].SetFilled(3, new Point(2, 3));

            // - - - -
            // - - - - 
            // X X X X
            // - - - -
            block.Rotations[2].SetFilled(0, new Point(0, 2));
            block.Rotations[2].SetFilled(1, new Point(1, 2));
            block.Rotations[2].SetFilled(2, new Point(2, 2));
            block.Rotations[2].SetFilled(3, new Point(3, 2));

            // - X - - 
            // - X - -
            // - X - - 
            // - X - -
            block.Rotations[3].SetFilled(0, new Point(1, 0));
            block.Rotations[3].SetFilled(1, new Point(1, 1));
            block.Rotations[3].SetFilled(2, new Point(1, 2));
            block.Rotations[3].SetFilled(3, new Point(1, 3));

            return block;
        }

        public static Block CreateJBlock()
        {
            Block block = new Block(Color.Blue, 3);

            // X - -
            // X X X 
            // - - -
            block.Rotations[0].SetFilled(0, new Point(0, 0));
            block.Rotations[0].SetFilled(1, new Point(0, 1));
            block.Rotations[0].SetFilled(2, new Point(1, 1));
            block.Rotations[0].SetFilled(3, new Point(2, 1));

            // - X X 
            // - X -
            // - X -
            block.Rotations[1].SetFilled(0, new Point(1, 0));
            block.Rotations[1].SetFilled(1, new Point(2, 0));
            block.Rotations[1].SetFilled(2, new Point(1, 1));
            block.Rotations[1].SetFilled(3, new Point(1, 2));

            // - - -
            // X X X
            // - - X
            block.Rotations[2].SetFilled(0, new Point(0, 1));
            block.Rotations[2].SetFilled(1, new Point(1, 1));
            block.Rotations[2].SetFilled(2, new Point(2, 1));
            block.Rotations[2].SetFilled(3, new Point(2, 2));

            // - X -
            // - X -
            // X X -
            block.Rotations[3].SetFilled(0, new Point(1, 0));
            block.Rotations[3].SetFilled(1, new Point(1, 1));
            block.Rotations[3].SetFilled(2, new Point(1, 2));
            block.Rotations[3].SetFilled(3, new Point(0, 2));

            return block;
        }

        public static Block CreateLBlock()
        {
            Block block = new Block(Color.Orange, 3);

            // - - -
            // - - X
            // X X X
            block.Rotations[0].SetFilled(0, new Point(2, 0));
            block.Rotations[0].SetFilled(1, new Point(0, 1));
            block.Rotations[0].SetFilled(2, new Point(1, 1));
            block.Rotations[0].SetFilled(3, new Point(2, 1));

            // - X -
            // - X -
            // - X X
            block.Rotations[1].SetFilled(0, new Point(1, 0));
            block.Rotations[1].SetFilled(1, new Point(1, 1));
            block.Rotations[1].SetFilled(2, new Point(1, 2));
            block.Rotations[1].SetFilled(3, new Point(2, 2));

            // - - -
            // X X X
            // X - -
            block.Rotations[2].SetFilled(0, new Point(0, 1));
            block.Rotations[2].SetFilled(1, new Point(1, 1));
            block.Rotations[2].SetFilled(2, new Point(2, 1));
            block.Rotations[2].SetFilled(3, new Point(0, 2));

            // X X -
            // - X -
            // - X
            block.Rotations[3].SetFilled(0, new Point(0, 0));
            block.Rotations[3].SetFilled(1, new Point(1, 0));
            block.Rotations[3].SetFilled(2, new Point(1, 1));
            block.Rotations[3].SetFilled(3, new Point(1, 2));

            return block;
        }

        public static Block CreateOBlock()
        {
            Block block = new Block(Color.Yellow, 4);

            // - X X - 
            // - X X -
            // - - - -
            block.Rotations[0].SetFilled(0, new Point(1, 0));
            block.Rotations[0].SetFilled(1, new Point(2, 0));
            block.Rotations[0].SetFilled(2, new Point(1, 1));
            block.Rotations[0].SetFilled(3, new Point(2, 1));

            // Lazy way of doing it for now. All rotations for
            // the O block are the same...
            block.Rotations[1].SetFilled(0, new Point(1, 0));
            block.Rotations[1].SetFilled(1, new Point(2, 0));
            block.Rotations[1].SetFilled(2, new Point(1, 1));
            block.Rotations[1].SetFilled(3, new Point(2, 1));

            block.Rotations[2].SetFilled(0, new Point(1, 0));
            block.Rotations[2].SetFilled(1, new Point(2, 0));
            block.Rotations[2].SetFilled(2, new Point(1, 1));
            block.Rotations[2].SetFilled(3, new Point(2, 1));

            block.Rotations[3].SetFilled(0, new Point(1, 0));
            block.Rotations[3].SetFilled(1, new Point(2, 0));
            block.Rotations[3].SetFilled(2, new Point(1, 1));
            block.Rotations[3].SetFilled(3, new Point(2, 1));

            return block;
        }
        
        public static Block CreateSBlock()
        {
            Block block = new Block(Color.Green, 3);

            // - X X
            // X X -
            // - - -
            block.Rotations[0].SetFilled(0, new Point(1, 0));
            block.Rotations[0].SetFilled(1, new Point(2, 0));
            block.Rotations[0].SetFilled(2, new Point(0, 1));
            block.Rotations[0].SetFilled(3, new Point(1, 1));

            // - X -
            // - X X
            // - - X
            block.Rotations[1].SetFilled(0, new Point(1, 0));
            block.Rotations[1].SetFilled(1, new Point(1, 1));
            block.Rotations[1].SetFilled(2, new Point(2, 1));
            block.Rotations[1].SetFilled(3, new Point(2, 2));

            // - - -
            // - X X
            // X X -
            block.Rotations[2].SetFilled(0, new Point(1, 1));
            block.Rotations[2].SetFilled(1, new Point(2, 1));
            block.Rotations[2].SetFilled(2, new Point(0, 2));
            block.Rotations[2].SetFilled(3, new Point(1, 2));

            // X - -
            // X X -
            // - X -
            block.Rotations[3].SetFilled(0, new Point(0, 0));
            block.Rotations[3].SetFilled(1, new Point(0, 1));
            block.Rotations[3].SetFilled(2, new Point(1, 1));
            block.Rotations[3].SetFilled(3, new Point(1, 2));

            return block;
        }

        public static Block CreateTBlock()
        {
            Block block = new Block(Color.Purple, 3);

            // - X -
            // X X X
            // - - -
            block.Rotations[0].SetFilled(0, new Point(1, 0));
            block.Rotations[0].SetFilled(1, new Point(0, 1));
            block.Rotations[0].SetFilled(2, new Point(1, 1));
            block.Rotations[0].SetFilled(3, new Point(2, 1));

            // - X -
            // - X X
            // - X -
            block.Rotations[1].SetFilled(0, new Point(1, 0));
            block.Rotations[1].SetFilled(1, new Point(1, 1));
            block.Rotations[1].SetFilled(2, new Point(2, 1));
            block.Rotations[1].SetFilled(3, new Point(1, 2));

            // - - -
            // X X X
            // - X -
            block.Rotations[2].SetFilled(0, new Point(0, 1));
            block.Rotations[2].SetFilled(1, new Point(1, 1));
            block.Rotations[2].SetFilled(2, new Point(2, 1));
            block.Rotations[2].SetFilled(3, new Point(1, 2));

            // - X -
            // X X -
            // - X
            block.Rotations[3].SetFilled(0, new Point(1, 0));
            block.Rotations[3].SetFilled(1, new Point(0, 1));
            block.Rotations[3].SetFilled(2, new Point(1, 1));
            block.Rotations[3].SetFilled(3, new Point(1, 2));

            return block;
        }

        public static Block CreateZBlock()
        {
            Block block = new Block(Color.Red, 3);


            // X X -
            // - X X
            // - - -
            block.Rotations[0].SetFilled(0, new Point(0, 0));
            block.Rotations[0].SetFilled(1, new Point(1, 0));
            block.Rotations[0].SetFilled(2, new Point(1, 1));
            block.Rotations[0].SetFilled(3, new Point(2, 1));

            // - - X
            // - X X
            // - X -
            block.Rotations[1].SetFilled(0, new Point(2, 0));
            block.Rotations[1].SetFilled(1, new Point(1, 1));
            block.Rotations[1].SetFilled(2, new Point(2, 1));
            block.Rotations[1].SetFilled(3, new Point(1, 2));

            // - - -
            // X X - 
            // - X X
            block.Rotations[2].SetFilled(0, new Point(0, 1));
            block.Rotations[2].SetFilled(1, new Point(1, 1));
            block.Rotations[2].SetFilled(2, new Point(1, 2));
            block.Rotations[2].SetFilled(3, new Point(2, 2));

            // - X -
            // X X -
            // X - -
            block.Rotations[3].SetFilled(0, new Point(1, 0));
            block.Rotations[3].SetFilled(1, new Point(0, 1));
            block.Rotations[3].SetFilled(2, new Point(1, 1));
            block.Rotations[3].SetFilled(3, new Point(0, 2));

            return block;
        }
        
    }
}
