using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    internal class GameOver
    {
        private Texture2D solidColor;

        private SpriteFont font;

        private Viewport viewport;

        private Rectangle titleSafe;

        private int textStep;

        private int opacity;

        private int pauseTime;

        public int Step;

        public void Initialize(Texture2D pixel, SpriteFont sf, Viewport view, Rectangle safe)
        {
            this.solidColor = pixel;
            this.font = sf;
            this.viewport = view;
            this.titleSafe = safe;
            this.textStep = 150;
            this.opacity = 0;
            this.pauseTime = 0;
            this.Step = 0;
        }

        public void Update()
        {
            if (this.Step == 1 && this.opacity <= 255)
            {
                this.opacity++;
            }
            else if (this.Step == 3 && this.opacity >= 0)
            {
                this.opacity -= 5;
            }
            else if (this.Step == 3 && this.opacity <= 0)
            {
                this.Step = 0;
            }
            else if (this.pauseTime >= this.textStep * 3)
            {
                this.Step = 2;
            }
            else
            {
                this.pauseTime++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Step > 0)
            {
                spriteBatch.Draw(this.solidColor, new Rectangle(0, 0, this.viewport.Width, this.viewport.Height), new Color(0, 0, 0, this.opacity));
                if (this.pauseTime > 25)
                {
                    spriteBatch.DrawString(this.font, "Poor Craftimal.", new Vector2((float)(this.titleSafe.X + 10), (float)(this.titleSafe.Y + 50)), new Color(this.opacity, this.opacity, this.opacity, this.opacity));
                }
                if (this.pauseTime >= this.textStep)
                {
                    spriteBatch.DrawString(this.font, "You can't touch the sun!", new Vector2((float)(this.titleSafe.X + 10), (float)(this.titleSafe.Y + 100)), new Color(this.opacity, this.opacity, this.opacity, this.opacity));
                }
                if (this.pauseTime >= this.textStep * 2)
                {
                    spriteBatch.DrawString(this.font, "Now look at you...", new Vector2((float)(this.titleSafe.X + 10), (float)(this.titleSafe.Y + 150)), new Color(this.opacity, this.opacity, this.opacity, this.opacity));
                }
            }
        }
    }
}
