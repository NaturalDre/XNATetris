using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    static class Utility
    {
        public static TimeSpan FpsToTimeSpan(int fps)
        {
            return TimeSpan.FromMilliseconds(1000 / fps);
        }
    }
}
