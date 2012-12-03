using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace NinjaVersusSlime
{
    class Player
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
        Texture2D image;
        Camera camera;
        Level level;
        bool grounded = false;
        bool jumping = false;
        bool dead = false;
        Clip death;
        Clip walk;
        Clip jump;
        Clip idle;
        Clip currentClip;
        AnimationPlayer animPlayer;
        SpriteEffects direction = SpriteEffects.None;
        public SoundEffect scream;

        public Rectangle FeetBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y + 50, 50, 20);
            }
        }

        public Rectangle PlayerBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, image.Width, image.Height);
            }
        }

        public Player(ContentManager content, Camera camera, Level level)
        {
            image = content.Load<Texture2D>("TileMain");

            death = new Clip(content.Load<Texture2D>("Player/death"), 4);
            walk = new Clip(content.Load<Texture2D>("Player/walk"), 5);
            jump = new Clip(content.Load<Texture2D>("Player/jump"), 1);
            idle = new Clip(content.Load<Texture2D>("Player/idle"), 2);
            scream = content.Load<SoundEffect>("Scream");

            animPlayer = new AnimationPlayer();

            currentClip = idle;

            this.camera = camera;
            this.level = level;
        }

        public void Update(GameTime gameTime)
        {
            if (!dead)
                GatherInput();

            // This is unacceptable, start level over IMMEDIATELY
            if (Position.Y >= level.bottom)
            {
                scream.Play();
                level.endLevel.Invoke(0);
            }

            HandleJump();

            CheckWalls();

            if (dead)
            {
                currentClip = death;
                animPlayer.keepPlaying = false;
            }
            else if (jumping || !grounded)
                currentClip = jump;
            else if (Velocity.X != 0.0f)
                currentClip = walk;
            else
                currentClip = idle;

            animPlayer.PlayContinueClip(currentClip, gameTime, 5);

            if (Velocity.X > 0)
                direction = SpriteEffects.None;
            else if (Velocity.X < 0)
                direction = SpriteEffects.FlipHorizontally;

            Position += Velocity;

            CheckCollision();

            camera.CenterOn(Position);
        }

        public void ForceJump()
        {
            Velocity = new Vector2(0, -30);
        }

        private void CheckCollision()
        {
            Rectangle feet = FeetBox;
            Vector2 feetMiddle = new Vector2(feet.Center.X, feet.Y);

            Tile checkTile;

            try
            {
                checkTile =
                    level.tiles[(int)(feetMiddle.X / Tile.TileSize.X), (int)(feetMiddle.Y / Tile.TileSize.Y)];
            }
            catch
            {
                return;
            }

            // Ground
            if (checkTile.Behavior == TileBehavior.ground)
            {
                grounded = true;
                jumping = false;
                Velocity.Y = 0;
                Position.Y = checkTile.Position.Y - Tile.TileSize.Y;
            }
            else
            {
                grounded = false;
            }
        }

        private void GatherInput()
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (mouse.X >= Helper.Resolution.X - (Helper.Resolution.X / 4.0f))
                    Velocity.X = 5.0f;
                else if (mouse.X <= Helper.Resolution.X / 4.0f)
                    Velocity.X = -5.0f;
                else
                {
                    if (grounded)
                    {
                        jumping = true;
                        Velocity.Y = -45.0f;
                    }
                }
            }
            else
            {
                Velocity.X = 0;
            }
        }

        private void HandleJump()
        {
            if (jumping || !grounded)
            {
                Velocity.Y += Helper.Gravity;
            }
        }

        public void Kill(GameTime gameTime)
        {
            if (dead)
                return;
            dead = true;
            Velocity.X = 0;
            level.StartDeathClock(gameTime);
            scream.Play();
        }

        private void CheckWalls()
        {
            Rectangle box = PlayerBox;
            try
            {
                if (Velocity.X < 0)
                {
                    Tile left = level.tiles[(int)(box.Left / Tile.TileSize.X), (int)(box.Center.Y / Tile.TileSize.Y)];
                    if (left.Behavior == TileBehavior.ground)
                    {
                        Velocity.X = 0;
                        Position.X = left.Position.X + Tile.TileSize.X - 1f;
                    }
                }
                else if (Velocity.X > 0)
                {
                    Tile right = level.tiles[(int)(box.Right / Tile.TileSize.X), (int)(box.Center.Y / Tile.TileSize.Y)];
                    if (right.Behavior == TileBehavior.ground)
                    {
                        Velocity.X = 0;
                        Position.X = right.Position.X - Tile.TileSize.X + 1f;
                    }
                }
            }
            catch
            {
                return;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            animPlayer.Draw(camera.GetRelativePosition(Position - new Vector2(0, 50)), 
                1.0f, gameTime, direction, spriteBatch);
            //spriteBatch.Draw(image, camera.GetRelativePosition(Position), Color.White);
        }
    }
}
