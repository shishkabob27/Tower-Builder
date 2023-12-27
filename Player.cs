using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    internal class Player
    {
        public GamePadState currentGamePadState;

        public GamePadState previousGamePadState;

        public KeyboardState currentKeyboardState;

        public KeyboardState previousKeyboardState;

        public MouseState currentMouseState;

        public MouseState previousMouseState;

        public PlayerIndex Index;

        public int AnimalIndex;

        public Texture2D Sprite;

        public Texture2D SpriteLeft;

        public Texture2D SpriteRight;

        public int HatIndex;

        public Texture2D Hat;

        public int Opacity;

        public Rectangle HitBox;

        public int Height;

        public int Width;

        private float rotation;

        private bool rotationUp;

        private Vector2 origin;

        public Vector2 Position;

        public int Jump;

        public int VelocityUp;

        public int VelocityDown;

        public int AirTime;

        public int WalkingSpeed;

        public bool Scrolling;

        public bool Ghost;

        public bool Editing;

        public int Deleting;

        public int Carrying;

        public int Delay;

        public int ColorIndex;

        public float EditRadius;

        public Viewport View;

        public void Preset(PlayerIndex index)
        {
            this.Index = index;
            this.AnimalIndex = 0;
            this.Ghost = false;
        }

        public void Initialize(Texture2D sprite1, Texture2D sprite2, Texture2D hat, Vector2 position, Viewport view)
        {
            this.SpriteRight = sprite1;
            this.Sprite = sprite1;
            this.SpriteLeft = sprite2;
            this.HatIndex = 0;
            this.Hat = hat;
            this.Opacity = 255;
            this.HitBox = default(Rectangle);
            this.Height = sprite1.Height;
            this.Width = sprite1.Width;
            this.origin = Vector2.Zero;
            this.rotation = 0f;
            this.rotationUp = false;
            this.Position = position;
            this.Jump = 0;
            this.AirTime = 0;
            this.VelocityUp = (this.VelocityDown = 0);
            this.Scrolling = false;
            if (this.Ghost)
            {
                this.WalkingSpeed = 8;
            }
            else
            {
                this.WalkingSpeed = 5;
            }
            this.Editing = false;
            this.Deleting = 0;
            this.Carrying = 0;
            this.Delay = 0;
            this.ColorIndex = 0;
            this.EditRadius = 1.8f;
            this.View = view;
        }

        public void Update()
        {
            if (this.Delay > 0)
            {
                this.Delay--;
            }
            if (this.currentGamePadState.ThumbSticks.Left.X > 0f)
            {
                this.Sprite = this.SpriteRight;
            }
            else if (this.currentGamePadState.ThumbSticks.Left.X < 0f)
            {
                this.Sprite = this.SpriteLeft;
            }
            if (this.VelocityDown > 1)
            {
                this.AirTime++;
            }
            else
            {
                this.AirTime = 0;
            }
            if ((double)Math.Abs(this.currentGamePadState.ThumbSticks.Left.X) > 0.15 && !this.Editing && this.Deleting == 0 && this.Delay < 1 && this.Jump == 0 && this.VelocityDown < 1 && !this.Ghost)
            {
                if (this.rotationUp)
                {
                    if (this.rotation >= -0.04f)
                    {
                        this.rotation -= 0.0075f;
                    }
                    else
                    {
                        this.rotationUp = false;
                    }
                }
                else if (this.rotation <= 0.04f)
                {
                    this.rotation += 0.0075f;
                }
                else
                {
                    this.rotationUp = true;
                }
                if (this.rotation >= 0f)
                {
                    this.origin.Y = 2f;
                }
                else
                {
                    this.origin.Y = 0f;
                }
            }
            else if (!this.Ghost && (this.VelocityUp > 0 || this.VelocityDown > 0))
            {
                if ((this.Sprite == this.SpriteLeft && this.VelocityUp > 0) || (this.Sprite == this.SpriteRight && this.VelocityDown > 0))
                {
                    this.origin.Y = 1f;
                    this.rotation = 0.02f;
                }
                else
                {
                    this.origin.Y = 0f;
                    this.rotation = -0.02f;
                }
            }
            else if (this.Ghost)
            {
                if (this.Sprite == this.SpriteRight)
                {
                    this.origin.Y = 1f;
                    this.rotation = 0.02f;
                }
                else
                {
                    this.origin.Y = 0f;
                    this.rotation = -0.02f;
                }
            }
            else
            {
                this.origin.Y = 0f;
                this.rotation = 0f;
            }
        }

        public void UpdateController()
        {
            this.previousGamePadState = this.currentGamePadState;
            this.currentGamePadState = GamePad.GetState(this.Index);
            this.previousKeyboardState = this.currentKeyboardState;
            this.currentKeyboardState = Keyboard.GetState();
            this.previousMouseState = this.currentMouseState;
            this.currentMouseState = Mouse.GetState();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Texture2D Parachute)
        {
            if (this.AirTime >= 30)
            {
                spriteBatch.Draw(Parachute, new Vector2(position.X, position.Y - (float)Parachute.Height + 25f), Color.White);
            }
            spriteBatch.Draw(this.Sprite, position, default(Rectangle?), new Color(this.Opacity, this.Opacity, this.Opacity, this.Opacity), this.rotation, this.origin, 1f, 0, 0f);
            spriteBatch.Draw(this.Hat, new Vector2(position.X - 9f, position.Y - 33f), default(Rectangle?), new Color(this.Opacity, this.Opacity, this.Opacity, this.Opacity), this.rotation / 1.5f, this.origin, 1f, 0, 0f);
        }
    }
}
