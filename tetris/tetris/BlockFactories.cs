using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris
{
    static class BlockFactories
    {

        public static Block CreateTBlock()
        {
            Block block = new Block(Color.Cyan, 3);

            // - - -
            // X X X
            // - X -
            block.Rotations[0].SetFilled(0, new Point(0, 1));
            block.Rotations[0].SetFilled(1, new Point(1, 1));
            block.Rotations[0].SetFilled(2, new Point(2, 1));
            block.Rotations[0].SetFilled(3, new Point(1, 2));
            // - X -
            // X X -
            // - X
            block.Rotations[1].SetFilled(0, new Point(1, 0));
            block.Rotations[1].SetFilled(1, new Point(0, 1));
            block.Rotations[1].SetFilled(2, new Point(1, 1));
            block.Rotations[1].SetFilled(3, new Point(1, 2));
            // - X -
            // X X X
            // - - -
            block.Rotations[2].SetFilled(0, new Point(1, 0));
            block.Rotations[2].SetFilled(1, new Point(0, 1));
            block.Rotations[2].SetFilled(2, new Point(1, 1));
            block.Rotations[2].SetFilled(3, new Point(2, 1));
            // - X -
            // - X X
            // - X -
            block.Rotations[3].SetFilled(0, new Point(1, 0));
            block.Rotations[3].SetFilled(1, new Point(1, 1));
            block.Rotations[3].SetFilled(2, new Point(2, 1));
            block.Rotations[3].SetFilled(3, new Point(1, 2));

            return block;
        }
        public static Block CreateLBlock()
        {
            Block block = new Block(Color.Orange, 3);

            // - - -
            // X X X
            // X - -
            block.Rotations[0].SetFilled(0, new Point(0, 1));
            block.Rotations[0].SetFilled(1, new Point(1, 1));
            block.Rotations[0].SetFilled(2, new Point(2, 1));
            block.Rotations[0].SetFilled(3, new Point(0, 2));
            // X X -
            // - X -
            // - X
            block.Rotations[1].SetFilled(0, new Point(0, 0));
            block.Rotations[1].SetFilled(1, new Point(1, 0));
            block.Rotations[1].SetFilled(2, new Point(1, 1));
            block.Rotations[1].SetFilled(3, new Point(1, 2));
            // - - -
            // - - X
            // X X X
            block.Rotations[2].SetFilled(0, new Point(2, 0));
            block.Rotations[2].SetFilled(1, new Point(0, 1));
            block.Rotations[2].SetFilled(2, new Point(1, 1));
            block.Rotations[2].SetFilled(3, new Point(2, 1));
            // - X -
            // - X -
            // - X X
            block.Rotations[3].SetFilled(0, new Point(1, 0));
            block.Rotations[3].SetFilled(1, new Point(1, 1));
            block.Rotations[3].SetFilled(2, new Point(1, 2));
            block.Rotations[3].SetFilled(3, new Point(2, 2));

            return block;

        }
    }
}
