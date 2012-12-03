using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaVersusSlime
{
    class AnimationPlayer
    {
        Clip playing = null;
        public AnimationPlayer()
        {

        }

        float startTime = 0.0f;
        int speed = 24;
        public bool isDone = false;
        public void PlayContinueClip(Clip clp, GameTime gameTime, int framerate)
        {
            if (clp == playing)
                return;
            playing = clp;
            startTime = (float)gameTime.TotalGameTime.TotalSeconds;
            speed = framerate;
            isDone = false;
        }

        bool isFrozen = false;
        public void Freeze()
        {
            isFrozen = true;
        }
        public void UnFreeze()
        {
            isFrozen = false;
        }

        int lastFrm = 0;
        public bool keepPlaying = true;
        public void Draw(Vector2 position, float scale, GameTime gameTime, SpriteEffects flip,
            SpriteBatch spriteBatch)
        {
            if (playing == null)
                return;
            int frm = (int)(((float)gameTime.TotalGameTime.TotalSeconds - startTime) * (float)speed);
            if (isFrozen)
                frm = playing.frameLength;
            lastFrm = frm;
            if (frm >= playing.frameLength)
            {
                isDone = true;
            }
            if (isDone && !keepPlaying)
                frm = playing.frameLength - 1;
            spriteBatch.Draw(playing.texture, position, playing.GetSourceRectangle(frm), Color.White,
                0.0f, Vector2.Zero, scale, flip, 0.0f);
        }
    }
}
