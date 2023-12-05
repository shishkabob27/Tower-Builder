using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    internal class Block
    {
        public Texture2D BlockTexture;

        public int Width;

        public int Height;

        public Color Color;

        public Vector2 Position;

        public float PositionY;

        public float PositionX;

        public void Initialize(Texture2D texture, Vector2 position, Rectangle background, Color color)
        {
            this.BlockTexture = texture;
            this.Width = texture.Width;
            this.Height = texture.Height;
            this.Color = color;
            this.PositionX = position.X;
            this.PositionY = position.Y;
            this.Position = new Vector2((float)background.X + this.PositionX, (float)background.Y + this.PositionY);
        }

        public void Update(Rectangle background)
        {
            this.Position = new Vector2((float)background.X + this.PositionX, (float)background.Y + this.PositionY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.BlockTexture, this.Position, this.Color);
        }
        
    }
}
