using System.Diagnostics;
using Microsoft.Xna.Framework;

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

        public static Vector2 ConvertToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

    }
}
