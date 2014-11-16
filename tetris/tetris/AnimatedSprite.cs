using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BytesOfPi.Graphics
{
    internal class AnimatedSprite : ISprite
    {
        private Sprite _sprite = new Sprite();
        private TimeSpan _timeToNextFrame = TimeSpan.FromSeconds(1);
        private TimeSpan _fps = TimeSpan.FromSeconds(1);
        private int _currentFrame = 1;
        private int _rows = 1;
        private int _columns = 1;
        private int _beginFrame = 1;
        private int _endFrame = 1;

        public AnimatedSprite()
        {
        }

        public AnimatedSprite(Texture2D texture, TimeSpan fps, int rows, int columns)
            : this(texture, fps, rows, columns, 1, (rows * columns) - 1)
        {
        }

        public AnimatedSprite(Texture2D texture, TimeSpan fps, int rows, int columns,
            int beginFrame, int endFrame)
        {
            Debug.Assert(texture != null);
            Sprite.Texture = texture;
            CurrentFrame = 1;
            Fps = fps;
            TimeToNextFrame = fps;
            Rows = rows;
            Columns = columns;
            BeginFrame = beginFrame;
            EndFrame = endFrame;
        }


        public bool Enabled { get; set; }
        // Start or Stop animating
        public bool PauseAnimation { get; set; }
        // How long until the next frame?
        public TimeSpan TimeToNextFrame
        {
            get { return _timeToNextFrame; }
            protected set { _timeToNextFrame = value; }
        }

        // Get the current FPS
        public TimeSpan Fps
        {
            get { return _fps; }
            set
            {
                if (value.Milliseconds <= 0)
                    _fps = TimeSpan.FromSeconds(1);
                else
                    _fps = value;
            }
        }

        // The frame current being displayed.
        public int CurrentFrame
        {
            get { return _currentFrame; }
            set
            {
                // The current frame should never be less than 0 or the beginning frame.
                if (value <= 0 || value < BeginFrame)
                    _currentFrame = BeginFrame;
                else
                    _currentFrame = value;
                // The current frame should never be greated than 0 or the ending frame.
                if (value > FrameCount || value > EndFrame)
                    _currentFrame = EndFrame;
            }
        }

        public int Rows
        {
            get { return _rows; }
            set
            {
                if (value < 1)
                    _rows = 1;
                else
                    _rows = value;
            }
        }

        public int Columns
        {
            get { return _columns; }
            set { _columns = value < 1 ? 1 : value; }
        }

        public int BeginFrame
        {
            get { return _beginFrame; }
            set
            {
                if (value <= 0 || value > EndFrame)
                    _beginFrame = 1;
                else
                    _beginFrame = value;
            }
        }

        public int EndFrame
        {
            get { return _endFrame; }
            set
            {
                if (value <= 0 || value > EndFrame)
                    _endFrame = FrameCount;
                else
                    _endFrame = value;
            }
        }

        public int FrameCount
        {
            get { return Rows * Columns; }
        }

        private Sprite Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }

        public Texture2D Texture
        {
            get { return Sprite.Texture; }
        }

        public Vector2 Position
        {
            get { return Sprite.Position; }
            set { Sprite.Position = value; }
        }

        public float Rotation
        {
            get { return Sprite.Rotation; }
            set { Sprite.Rotation = value; }
        }

        public Vector2 Origin
        {
            get { return Sprite.Origin; }
            set { Sprite.Origin = value; }
        }

        public Rectangle? SourceRectangle
        {
            get { return Sprite.SourceRectangle; }
            set { Sprite.SourceRectangle = value; }
        }

        public float Scale
        {
            get { return Sprite.Scale; }
            set { Sprite.Scale = value; }
        }

        public float LayerDepth
        {
            get { return Sprite.LayerDepth; }
            set { Sprite.LayerDepth = value; }
        }

        public void Update(GameTime gameTime)
        {
            if (PauseAnimation)
                return;

            TimeToNextFrame -= gameTime.ElapsedGameTime;

            if (TimeToNextFrame.Milliseconds <= 0)
            {
                NextFrame();
                // Start counting to the next frame again. We make sure to add the negative
                // value of TimeToNextFrame to not lose any extra time.
                TimeToNextFrame = (Fps + TimeToNextFrame);
            }
        }

        private void NextFrame()
        {
            // Check to see if we're at the last frame in the animation sheet, or
            // if we're at the last frame the user wants animated.
            if ((CurrentFrame >= FrameCount) || CurrentFrame >= EndFrame)
                CurrentFrame = BeginFrame; // If either is true, we start at the BeginFrame.
            else
                ++CurrentFrame;

            // Code below needs testing.

            Rectangle source;
            {
                var framePosition = new Point();

                if ((CurrentFrame % Columns) == 0)
                {
                    framePosition.X = Columns;
                    framePosition.Y = CurrentFrame / Columns;
                }
                else
                {
                    framePosition.X = CurrentFrame % Columns;
                    framePosition.Y = (CurrentFrame / Columns) + 1;
                }

                --framePosition.X;
                --framePosition.Y;

                int frameWidth = Texture.Width / Columns;
                int frameHeight = Texture.Height / Rows;

                source.X = (framePosition.X * frameWidth);
                source.Y = (framePosition.Y * frameHeight);

                source.Width = frameWidth;
                source.Height = frameHeight;
            }

            SourceRectangle = source;
        }

        protected void Reset()
        {
            // TODO: Implement resetting everything to defaults
            throw new NotImplementedException();
        }
    }
}
