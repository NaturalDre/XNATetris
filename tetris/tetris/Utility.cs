﻿using System;

namespace Tetris
{
    static class Utility
    {
        public static TimeSpan FpsToTimeSpan(int fps)
        {
            return TimeSpan.FromMilliseconds(1000 / fps);
        }

        public static int ConvertTo1DIndex(int row, int column, int totalColumns)
        {
            return (row * totalColumns) + column;
        }
    }
}
