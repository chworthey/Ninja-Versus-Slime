using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace NinjaVersusSlime
{
    public enum EnemyState
    {
        waiting = 0,
        walking = 1,
        dead = 2,
    }

    class Enemy
    {
        public Vector2 Position = Vector2.Zero;
        Camera camera;
        Level level;
        SpriteEffects direction;
        EnemyState state = EnemyState.walking;
        float initialWaitTime = 0.0f;
        bool dead = false;
        AnimationPlayer animPlayer;
        Clip walk;
        Clip death;
        Clip idle;
        Clip currentClip;
        SoundEffect squish;

        public Rectangle EnemyBox
        {
            get
            {
                return new Rectangle((int)Position.X + 25, (int)Position.Y + 50, 50, 
                    50);
            }
        }

        public Rectangle HeadBox
        {
            get
            {
                return new Rectangle((int)Position.X + 25, (int)Position.Y + 50 - 5, 50, 20);
            }
        }

        public Enemy(ContentManager content, Camera camera, Level level)
        {
            this.camera = camera;
            this.level = level;
            walk = new Clip(content.Load<Texture2D>("Monsters/e/walk"), 5);
            death = new Clip(content.Load<Texture2D>("Monsters/e/death"), 4);
            idle = new Clip(content.Load<Texture2D>("Monsters/e/idle"), 4);
            squish = content.Load<SoundEffect>("Squish");
            currentClip = walk;
            animPlayer = new AnimationPlayer();
            int d = Helper.rnd.Next(1);
            if (d == 0)
                direction = SpriteEffects.None;
            else
                direction = SpriteEffects.FlipHorizontally;
        }

        public void Update(GameTime gameTime)
        {
            if (dead)
            {
                if (animPlayer.isDone)
                    animPlayer.Freeze();
                animPlayer.PlayContinueClip(currentClip, gameTime, 3);
                return;
            }
            animPlayer.PlayContinueClip(currentClip, gameTime, 3);
            Vector2 velocity = Vector2.Zero;

            checkDieKillPlayer(gameTime);

            if (state == EnemyState.walking)
            {
                currentClip = walk;
                try
                {
                    Tile t;
                    Tile t2;
                    if (direction == SpriteEffects.FlipHorizontally)
                    {
                        t = level.tiles[(int)(EnemyBox.Center.X / Tile.TileSize.X) - 1,
                            (int)(EnemyBox.Center.Y / Tile.TileSize.Y) + 1];
                        t2 = level.tiles[(int)(EnemyBox.Center.X / Tile.TileSize.X) - 1,
                            (int)(EnemyBox.Center.Y / Tile.TileSize.X)];
                    }
                    else
                    {
                        t = level.tiles[(int)(EnemyBox.Center.X / Tile.TileSize.X) + 1,
                            (int)(EnemyBox.Center.Y / Tile.TileSize.Y) + 1];
                        t2 = level.tiles[(int)(EnemyBox.Center.X / Tile.TileSize.X) + 1,
                            (int)(EnemyBox.Center.Y / Tile.TileSize.X)];
                    }
                    if (t.Behavior == TileBehavior.air || t2.Behavior == TileBehavior.ground)
                    {
                        initialWaitTime = (float)gameTime.TotalGameTime.TotalSeconds;
                        state = EnemyState.waiting;
                    }
                }
                catch { }
                if (direction == SpriteEffects.FlipHorizontally)
                    velocity = new Vector2(-1, 0);
                else
                    velocity = new Vector2(1, 0);

                Position += velocity;
            }
            else if (state == EnemyState.waiting)
            {
                currentClip = idle;
                if ((float)gameTime.TotalGameTime.TotalSeconds - initialWaitTime > 2.0f)
                {
                    state = EnemyState.walking;
                    if (direction == SpriteEffects.FlipHorizontally)
                        direction = SpriteEffects.None;
                    else
                        direction = SpriteEffects.FlipHorizontally;
                }
            }
        }

        private void checkDieKillPlayer(GameTime gameTime)
        {
            if (level.player.FeetBox.Intersects(HeadBox))
            {
                dead = true;
                currentClip = death;
                state = EnemyState.dead;
                animPlayer.keepPlaying = false;
                animPlayer.PlayContinueClip(death, gameTime, 3);
                level.player.ForceJump();
                squish.Play();
            }
            else if (level.player.PlayerBox.Intersects(EnemyBox))
                level.player.Kill(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            animPlayer.Draw(camera.GetRelativePosition(Position), 1.0f, gameTime, direction, spriteBatch);
        }
    }
}
