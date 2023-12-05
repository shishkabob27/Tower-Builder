using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    internal class Text
    {
        private SpriteFont Font;

        private string Value;

        private Vector2 Position;

        private int FadeSpeed;

        public int Opacity;

        public void Initialize(SpriteFont font, Vector2 position, string value, int fade)
        {
            this.Font = font;
            this.Position = position;
            this.Value = value;
            this.FadeSpeed = fade;
            this.Opacity = 255;
        }

        public void Update()
        {
            this.Position.Y = this.Position.Y - 0.5f;
            this.Opacity -= this.FadeSpeed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.Font, this.Value ?? "", new Vector2(this.Position.X + 1f, this.Position.Y), new Color(0, 0, 0, this.Opacity));
            spriteBatch.DrawString(this.Font, this.Value ?? "", new Vector2(this.Position.X - 1f, this.Position.Y), new Color(0, 0, 0, this.Opacity));
            spriteBatch.DrawString(this.Font, this.Value ?? "", new Vector2(this.Position.X, this.Position.Y + 1f), new Color(0, 0, 0, this.Opacity));
            spriteBatch.DrawString(this.Font, this.Value ?? "", new Vector2(this.Position.X, this.Position.Y - 1f), new Color(0, 0, 0, this.Opacity));
            spriteBatch.DrawString(this.Font, this.Value ?? "", this.Position, new Color(this.Opacity, this.Opacity, this.Opacity, this.Opacity));
        }
    }
}
