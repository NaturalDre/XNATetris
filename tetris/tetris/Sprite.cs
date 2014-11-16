using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BytesOfPi.Graphics
{
    internal sealed class Sprite : ISprite
    {
        private Texture2D _texture;
        private Vector2 _position = Vector2.Zero;
        private Vector2 _origin = Vector2.Zero;
        private Rectangle? _sourceRectangle;
        private float _scale = 1.0f;

        public Sprite()
        {
            LayerDepth = 0.0f;
            Rotation = 0.0f;
        }

        public Sprite(Texture2D texture, Vector2 position)
            : this()
        {
            Debug.Assert(texture != null);
            Texture = texture;
            Position = position;
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Rotation { get; set; }

        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public Rectangle? SourceRectangle
        {
            get { return _sourceRectangle; }
            set { _sourceRectangle = value; }
        }

        // TODO: Check against a scale of 0?
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public float LayerDepth { get; set; }

        /// <summary>
        ///     The Texture2D being used. Passing in null will
        ///     cause everything to be reset to an empty sprite.
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                // Settings the texture to null means there will be nothing to display,
                // so we reset this instance to its default settings.
                if (Texture != null && value == null)
                {
                    Reset();
                    return;
                }

                // If it isn't null, we'll assume the user wants to keep the settings,
                // so we only update the texture.
                // NOTE: We might want to add an OnTextureChanged event/virtual function,
                //          because classes that derive from this class might need
                //          to update their state as a result. e.g.
                _texture = value;
            }
        }

        /// <summary>
        ///     Reload the sprite using another texture. This
        ///     means your sprite will have default settings.
        ///     The sprite must be not be null for a reload to occur.
        /// </summary>
        /// <param name="texture"> The texture you want to use. </param>
        /// <returns>
        ///     True if the load was successful, false otherwise.
        /// </returns>
        public bool Load(Texture2D texture)
        {
            // TODO: Everything in this function still needs testing.
            if (texture == null)
                return false;

            Reset();
            Texture = texture;
            return true;
        }

        private void Reset()
        {
            Position = Vector2.Zero;
            Rotation = 0.0f;
            Origin = Vector2.Zero;
            SourceRectangle = null;
            Scale = 1.0f;
            LayerDepth = 0;
            Texture = null;
        }
    }
}
