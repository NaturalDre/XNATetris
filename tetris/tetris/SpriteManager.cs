using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BytesOfPi.Graphics
{
    internal class SpriteManager : DrawableGameComponent
    {
        private readonly List<Sprite> _spriteList = new List<Sprite>();
        private readonly List<AnimatedSprite> _animatedSpriteList = new List<AnimatedSprite>();

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected SpriteBatch SpriteBatch { get; set; }

        private List<Sprite> Sprites
        {
            get { return _spriteList; }
        }

        private List<AnimatedSprite> AnimatedSprites
        {
            get { return _animatedSpriteList; }
        }

        public override void Initialize()
        {
            base.Initialize();
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            Sprites.Clear();
            AnimatedSprites.Clear();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (AnimatedSprite animatedSprite in AnimatedSprites)
                animatedSprite.Update(gameTime);

            base.Update(gameTime);
        }

        // Loops through all sprites and animated sprites and calls our Draw overload that
        // does the actual drawing of a sprite.
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            foreach (Sprite sprite in _spriteList)
                Draw(sprite);

            foreach (AnimatedSprite animatedSprite in _animatedSpriteList)
                Draw(animatedSprite);

            SpriteBatch.End();
            base.Draw(gameTime);
        }

        // Draw the sprite to the screen.
        private void Draw(ISprite sprite)
        {
            SpriteBatch.Draw(sprite.Texture, sprite.Position, sprite.SourceRectangle, Color.White, sprite.Rotation,
                sprite.Origin, sprite.Scale, SpriteEffects.None, sprite.LayerDepth);
        }

        // Add a non-animated sprite to the manager.
        // TODO:
        //      - Perform checks to prevent adding the same one multiple times.
        public void Add(Sprite sprite)
        {
            Sprites.Add(sprite);
        }

        // Add an animated sprite to the manager.
        // TODO:
        //      - Perform checks to prevent adding the same one multiple times.
        public void Add(AnimatedSprite animatedSprite)
        {
            AnimatedSprites.Add(animatedSprite);
        }

        public void Remove(Sprite sprite)
        {
            Sprites.Remove(sprite);
        }

        public void Remove(AnimatedSprite animatedSprite)
        {
            AnimatedSprites.Remove(animatedSprite);
        }
    }
}
