using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NinjaVersusSlime
{
    class Camera
    {
        public Vector2 Position = Vector2.Zero;

        public void CenterOn(Vector2 position)
        {
            Position = -Helper.Resolution / 2f + position;
        }

        public Vector2 GetRelativePosition(Vector2 gamePosition)
        {
            return gamePosition - Position;
        }
    }
}
