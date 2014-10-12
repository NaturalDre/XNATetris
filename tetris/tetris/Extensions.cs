using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tetris
{
    static class Extensions
    {
        // Shorthand for calling Game.Services.GetService();
        public static T GetService<T>(this Microsoft.Xna.Framework.Game game)
        {
            Debug.Assert(game != null);
            return (T)game.Services.GetService(typeof(T));
        }

       
    }
}
