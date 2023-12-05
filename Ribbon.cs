using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    internal class Ribbon
    {
        private Texture2D RibbonTexture;

        public int Width;

        public int Height;

        public Vector2 Position;

        public float PositionY;

        public float PositionX;

        public Rectangle HitBox;

        public string Effect;

        public int EffectValue;

        public void Initialize(Texture2D texture, Vector2 position, Rectangle background, string effect, int value)
        {
            this.RibbonTexture = texture;
            this.Width = background.Width;
            this.Height = 50;
            this.PositionX = position.X;
            this.PositionY = position.Y;
            this.Position = new Vector2((float)background.X + this.PositionX, (float)background.Y + this.PositionY);
            this.Effect = effect;
            this.EffectValue = value;
        }

        public void Update(Rectangle background)
        {
            this.Position = new Vector2((float)background.X + this.PositionX, (float)background.Y + this.PositionY);
            this.HitBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.RibbonTexture, this.HitBox, Color.White);
        }
    }
}
