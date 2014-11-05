using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BytesOfPi.Graphics
{
    sealed class Sprite: ISprite
    {
        Texture2D texture = null;
        Vector2 position = Vector2.Zero;
        float rotation = 0.0f;
        Vector2 origin = Vector2.Zero;
        Rectangle? sourceRectangle = null;
        float scale = 1.0f;
        float layerDepth = 0.0f;

        public Sprite()
        {

        }

        public Sprite(Texture2D texture, Vector2 position)
            : this()
        {
            Debug.Assert(texture != null);
            this.Texture = texture;
            this.Position = position;
        }

        public Vector2 Position 
        { 
            get { return this.position; }
            set { this.position = value; }
        }

        public float Rotation {
            get { return this.rotation; }
            set { this.rotation = value; }
        }

        public Vector2 Origin
        {
            get { return this.origin; }
            set { this.origin = value; }
        }

        public Nullable<Rectangle> SourceRectangle 
        {
            get { return this.sourceRectangle; }
            set { this.sourceRectangle = value; }
        }

        // TODO: Check against a scale of 0?
        public float Scale 
        {
            get { return this.scale; }
            set { this.scale = value; }
        }

        public float LayerDepth 
        {
            get { return this.layerDepth; }
            set { this.layerDepth = value; }
        }
        /// <summary>
        /// The Texture2D being used. Passing in null will
        /// cause everything to be reset to an empty sprite.
        /// </summary>
        public Texture2D Texture 
        {
            get { return texture; }
            set
            {
                // Settings the texture to null means there will be nothing to display,
                // so we reset this instance to its default settings.
                if (this.Texture != null && value == null)
                {
                    this.Reset();
                    return;
                }

                // If it isn't null, we'll assume the user wants to keep the settings,
                // so we only update the texture.
                // NOTE: We might want to add an OnTextureChanged event/virtual function,
                //          because classes that derive from this class might need
                //          to update their state as a result. e.g.
                this.texture = value;
            }
        }

        /// <summary>
        /// Reload the sprite using another texture. This
        /// means your sprite will have default settings.
        /// The sprite must be not be null for a reload to occur.
        /// </summary>
        /// <param name="texture"> The texture you want to use. </param>
        /// <returns> 
        /// True if the load was successful, false otherwise.
        /// </returns>
        public bool Load(Texture2D texture)
        {
            // TODO: Everything in this function still needs testing.
            if (texture == null)
                return false;

            this.Reset();
            this.Texture = texture;
            return true;
        }

        private void Reset()
        {
            this.Position = Vector2.Zero;
            this.Rotation = 0.0f;
            this.Origin = Vector2.Zero;
            this.SourceRectangle = null;
            this.Scale = 1.0f;
            this.LayerDepth = 0;
            this.Texture = null;
        }
    }
}
