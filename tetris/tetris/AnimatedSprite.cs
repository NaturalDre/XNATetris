using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BytesOfPi.Graphics
{
    class AnimatedSprite : ISprite
    {
        Sprite sprite = new Sprite();
        TimeSpan timeToNextFrame = TimeSpan.FromSeconds(1);
        TimeSpan fps = TimeSpan.FromSeconds(1);
        int currentFrame = 1;
        int rows = 1;
        int columns = 1;
        int beginFrame = 1;
        int endFrame = 1;

        public AnimatedSprite()
        {

        }

        public AnimatedSprite(Texture2D texture, TimeSpan fps,int rows, int columns)
            : this(texture, fps, rows, columns, 1, (rows * columns) - 1)
        {

        }

        public AnimatedSprite(Texture2D texture, TimeSpan fps, int rows, int columns,
            int beginFrame, int endFrame)
        {
            Debug.Assert(texture != null);
            this.Sprite.Texture = texture;
            this.CurrentFrame = 1;
            this.Fps = fps;
            this.TimeToNextFrame = fps;
            this.Rows = rows;
            this.Columns = columns;
            this.BeginFrame = beginFrame;
            this.EndFrame = endFrame;
        }


        public bool Enabled { get; set; }
        // Start or Stop animating
        public bool PauseAnimation { get; set; }
        // How long until the next frame?
        public TimeSpan TimeToNextFrame
        {
            get { return this.timeToNextFrame; }
            protected set { this.timeToNextFrame = value; }
        }
        // Get the current FPS
        public TimeSpan Fps
        {
            get { return this.fps; }
            set
            {
                if (value.Milliseconds <= 0)
                    this.fps = TimeSpan.FromSeconds(1);
                else
                    this.fps = value;
            }
        }
        // The frame current being displayed.
        public int CurrentFrame
        {
            get { return this.currentFrame; }
            set
            {
                // The current frame should never be less than 0 or the beginning frame.
                if (value <= 0 || value < this.BeginFrame)
                    this.currentFrame = this.BeginFrame;
                else
                    this.currentFrame = value;
                // The current frame should never be greated than 0 or the ending frame.
                if (value > this.FrameCount || value > this.EndFrame)
                    this.currentFrame = this.EndFrame;
            }
        }

        public int Rows
        {
            get { return this.rows; }
            set
            {
                if (value < 1)
                    this.rows = 1;
                else
                    this.rows = value;
            }
        }

        public int Columns
        {
            get { return this.columns; }
            set
            {
                if (value < 1)
                    this.columns = 1;
                else
                    this.columns = value;
            }
        }

        public int BeginFrame
        {
            get { return this.beginFrame; }
            set
            {
                if (value <= 0 || value > this.EndFrame)
                    this.beginFrame = 1;
                else
                    this.beginFrame = value;
            }
        }

        public int EndFrame
        {
            get { return this.endFrame; }
            set
            {
                if (value <= 0 || value > this.EndFrame)
                    this.endFrame = this.FrameCount;
                else
                    this.endFrame = value;
            }
        }

        public int FrameCount
        {
            get { return this.Rows * this.Columns; }
        }

        private Sprite Sprite
        {
            get { return this.sprite; }
            set { this.sprite = value; }
        }

        public Texture2D Texture
        {
            get { return this.Sprite.Texture; }
        }

        public Vector2 Position
        {
            get { return this.Sprite.Position; }
            set { this.Sprite.Position = value; }
        }

        public float Rotation
        {
            get { return this.Sprite.Rotation; }
            set { this.Sprite.Rotation = value; }
        }

        public Vector2 Origin
        {
            get { return this.Sprite.Origin; }
            set { this.Sprite.Origin = value; }
        }

        public Rectangle? SourceRectangle
        {
            get { return this.Sprite.SourceRectangle; }
            set { this.Sprite.SourceRectangle = value; }
        }

        public float Scale
        {
            get { return this.Sprite.Scale; }
            set { this.Sprite.Scale = value; }
        }

        public float LayerDepth
        {
            get { return this.Sprite.LayerDepth; }
            set { this.Sprite.LayerDepth = value; }
        }

        public void Update(GameTime gameTime)
        {
            if (this.PauseAnimation == true)
                return;

            this.TimeToNextFrame -= gameTime.ElapsedGameTime;

            if (this.TimeToNextFrame.Milliseconds <= 0)
            {
                NextFrame();
                // Start counting to the next frame again. We make sure to add the negative
                // value of TimeToNextFrame to not lose any extra time.
                this.TimeToNextFrame = (this.Fps + this.TimeToNextFrame);
            }

        }

        private void NextFrame()
        {
            // Check to see if we're at the last frame in the animation sheet, or
            // if we're at the last frame the user wants animated.
            if ((this.CurrentFrame >= this.FrameCount) || this.CurrentFrame >= this.EndFrame)
                this.CurrentFrame = this.BeginFrame; // If either is true, we start at the BeginFrame.
            else
                ++this.CurrentFrame;

            // Code below needs testing.

            Rectangle source;
            {
                Point framePosition = new Point();

                if ((this.CurrentFrame % this.Columns) == 0)
                {
                    framePosition.X = this.Columns;
                    framePosition.Y = this.CurrentFrame / this.Columns;
                }
                else
                {
                    framePosition.X = this.CurrentFrame % this.Columns;
                    framePosition.Y = (this.CurrentFrame / this.Columns) + 1;
                }

                --framePosition.X;
                --framePosition.Y;

                int frameWidth = this.Texture.Width / this.Columns;
                int frameHeight = this.Texture.Height / this.Rows;

                source.X = (framePosition.X * frameWidth);
                source.Y = (framePosition.Y * frameHeight);

                source.Width = frameWidth;
                source.Height = frameHeight;
            }

            this.SourceRectangle = source;
        }

        protected void Reset()
        {
            // TODO: Implement resetting everything to defaults
            throw new NotImplementedException();
        }
    }
}
