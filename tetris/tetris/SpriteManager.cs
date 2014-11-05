using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BytesOfPi.Graphics
{
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private List<Sprite> spriteList = new List<Sprite>();
        private List<AnimatedSprite> animatedSpriteList = new List<AnimatedSprite>();

        public SpriteManager(Game game)
            : base(game)
        {

        }

        protected SpriteBatch SpriteBatch
        {
            get { return this.spriteBatch; }
            set { this.spriteBatch = value; }
        }

        private List<Sprite> Sprites
        {
            get { return this.spriteList; }
        }

        private List<AnimatedSprite> AnimatedSprites
        {
            get { return this.animatedSpriteList; }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            this.Sprites.Clear();
            this.AnimatedSprites.Clear();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (AnimatedSprite animatedSprite in this.AnimatedSprites)
                animatedSprite.Update(gameTime);

            base.Update(gameTime);
        }

        // Loops through all sprites and animated sprites and calls our Draw overload that
        // does the actual drawing of a sprite.
        public override void Draw(GameTime gameTime)
        {
            this.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            foreach (Sprite sprite in this.spriteList)
                this.Draw(sprite);

            foreach (AnimatedSprite animatedSprite in this.animatedSpriteList)
                    this.Draw(animatedSprite);

            this.SpriteBatch.End();
            base.Draw(gameTime);
        }
        // Draw the sprite to the screen.
        private void Draw(ISprite sprite)
        {
            this.SpriteBatch.Draw(sprite.Texture, sprite.Position, sprite.SourceRectangle, Color.White, sprite.Rotation, sprite.Origin, sprite.Scale, SpriteEffects.None, sprite.LayerDepth);
        }

        // Add a non-animated sprite to the manager.
        // TODO:
        //      - Perform checks to prevent adding the same one multiple times.
        public void Add(Sprite sprite)
        {
            this.Sprites.Add(sprite);
        }
        // Add an animated sprite to the manager.
        // TODO:
        //      - Perform checks to prevent adding the same one multiple times.
        public void Add(AnimatedSprite animatedSprite)
        {
            this.AnimatedSprites.Add(animatedSprite);
        }

        public void Remove(Sprite sprite)
        {
            this.Sprites.Remove(sprite);
        }

        public void Remove(AnimatedSprite animatedSprite)
        {
            this.AnimatedSprites.Remove(animatedSprite);
        }

    }
}
