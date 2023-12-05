using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    public class ParticleEngine
    {
        private Random random;

        private List<Particle> particles;

        private List<Texture2D> textures;

        private Color Color;

        private float speed;

        private int life;

        private Rectangle Background;

        private Vector2 Origin;

        public ParticleEngine(List<Texture2D> textures)
        {
            this.textures = textures;
            this.particles = new List<Particle>();
            this.random = new Random();
        }

        public void Create(Rectangle background, Vector2 location, Color color, int count, float velocity, int lifetime)
        {
            this.Background = background;
            this.Origin = new Vector2(location.X, location.Y);
            this.speed = velocity;
            this.life = lifetime;
            this.Color = color;
            for (int i = 0; i < count; i++)
            {
                this.particles.Add(this.GenerateNewParticle());
            }
        }

        public void Update(Rectangle background)
        {
            for (int i = 0; i < this.particles.Count; i++)
            {
                this.particles[i].Update(background);
                if (this.particles[i].TTL <= 0)
                {
                    this.particles.RemoveAt(i);
                    i--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = this.textures[this.random.Next(this.textures.Count)];
            Vector2 velocity = new(this.speed * (float)(this.random.NextDouble() * 2.0 - 1.0), this.speed * (float)(this.random.NextDouble() * 2.0 - 1.0));
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(this.random.NextDouble() * 2.0 - 1.0);
            float size = (float)this.random.NextDouble();
            int ttl = this.life + this.random.Next(40);
            return new Particle(texture, this.Background, this.Origin, this.Color, velocity, angle, angularVelocity, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.particles.Count; i++)
            {
                this.particles[i].Draw(spriteBatch);
            }
        }
    }
}
