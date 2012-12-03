using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaVersusSlime
{
    class Clip
    {
        public Texture2D texture;
        public int frameLength;
        public Clip(Texture2D texture, int numberOfFrames)
        {
            frameLength = numberOfFrames;
            this.texture = texture;
        }

        public Rectangle GetSourceRectangle(int frame)
        {
            while (frame >= frameLength)
            {
                frame -= frameLength;
            }
            while (frame < 0)
            {
                frame += frameLength;
            }

            Rectangle returnVal = new Rectangle();
            returnVal.Width = texture.Width / frameLength;
            returnVal.Height = texture.Height;
            returnVal.X = texture.Width / frameLength * frame;
            return returnVal;
        }
    }
}
