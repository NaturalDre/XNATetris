using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Tetris
{
    interface ISprite
    {
        Texture2D Texture { get; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Vector2 Origin { get; set; }
        Rectangle? SourceRectangle { get; set; }
        float Scale { get; set; }
        float LayerDepth { get; set; }
    }
}
