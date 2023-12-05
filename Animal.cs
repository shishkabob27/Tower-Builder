using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    internal class Animal
    {
        public Texture2D NormalSpriteLeft;

        public Texture2D NormalSpriteRight;

        public Texture2D GhostSpriteLeft;

        public Texture2D GhostSpriteRight;

        public void Initialize(Texture2D t1, Texture2D t2, Texture2D t3, Texture2D t4)
        {
            NormalSpriteRight = t1;
            NormalSpriteLeft = t2;
            GhostSpriteRight = t3;
            GhostSpriteLeft = t4;
        }
    }

}
