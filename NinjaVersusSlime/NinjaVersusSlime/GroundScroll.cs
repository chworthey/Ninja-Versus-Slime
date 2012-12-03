using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaVersusSlime
{
    class GroundScroll
    {
        Texture2D grass;
        Texture2D mountains;
        int height = 0;
        Vector2 initialPos = Vector2.Zero;
        Vector2 resx = Vector2.Zero;
        /// <summary>
        /// It is a constructor for a ground texture that scrolls
        /// </summary>
        /// <param name="content">The content manager that loads stuff</param>
        /// <param name="tileBlockHeight">The height (int) that defines the tiles</param>
        public GroundScroll(ContentManager content, int tileBlockHeight)
        {
            grass = content.Load<Texture2D>("Grass");
            mountains = content.Load<Texture2D>("Mountains");
            height = tileBlockHeight;
            initialPos = new Vector2(0,
                height * Tile.TileSize.Y + Helper.Resolution.Y / 2.0f - grass.Height);
            resx = new Vector2(grass.Width, 0.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Vector2 position = camera.GetRelativePosition(initialPos);
            while (position.X < 0)
                position.X += Helper.Resolution.X;
            while (position.X > Helper.Resolution.X)
                position.X -= Helper.Resolution.X;

            Vector2 mountainOffset = new Vector2(0, -110f);

            spriteBatch.Draw(mountains, position + mountainOffset, Color.White);
            spriteBatch.Draw(mountains, position - resx + mountainOffset, Color.White);
            spriteBatch.Draw(mountains, position + resx + mountainOffset, Color.White);

            spriteBatch.Draw(grass, position, Color.White);
            spriteBatch.Draw(grass, position - resx, Color.White);
            spriteBatch.Draw(grass, position + resx, Color.White);
        }
    }
}
