using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    public class Particle
    {
        private Vector2 Distance;

        private Vector2 Origin;

        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }
        
        public Vector2 Velocity { get; set; }

        public float Angle { get; set; }

        public float AngularVelocity { get; set; }

        public Color Color { get; set; }

        public float Size { get; set; }

        public int TTL { get; set; }

        public Particle(Texture2D texture, Rectangle background, Vector2 origin, Color color, Vector2 velocity, float angle, float angularVelocity, float size, int ttl)
        {
            this.Texture = texture;
            this.Origin = origin;
            this.Position = new Vector2((float)background.X + this.Origin.X, (float)background.Y + this.Origin.Y);
            this.Velocity = velocity;
            this.Distance = Vector2.Zero;
            this.Angle = angle;
            this.AngularVelocity = angularVelocity;
            this.Size = size;
            this.TTL = ttl;
            this.Color = color;
        }

        public void Update(Rectangle background)
        {
            if (this.Position.Y + this.Size >= (float)(background.Y + background.Height) - this.Size)
            {
                this.Velocity = new Vector2(this.Velocity.X, -this.Size);
            }
            else
            {
                this.Velocity = new Vector2(this.Velocity.X, this.Velocity.Y + 0.08f);
            }
            this.Distance += this.Velocity;
            this.Position = new Vector2((float)background.X + this.Origin.X, (float)background.Y + this.Origin.Y) + this.Distance;
            this.TTL--;
            this.Angle += this.AngularVelocity;
            this.Size /= 1.008f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = new(0, 0, this.Texture.Width, this.Texture.Height);
            Vector2 vector = new((float)(this.Texture.Width / 2), (float)(this.Texture.Height / 2));
            spriteBatch.Draw(this.Texture, this.Position, new Rectangle?(rectangle), this.Color, this.Angle, vector, this.Size, 0, 0f);
        }
    }
}
