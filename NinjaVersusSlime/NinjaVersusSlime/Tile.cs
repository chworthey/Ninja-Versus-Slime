using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaVersusSlime
{
    public enum TileBehavior
    {
        ground = 0,
        air = 1,
    }

    public enum TileScript
    {
        none = 0,
        end = 1,
        kill = 2,
    }

    class Tile
    {
        public TileBehavior Behavior = TileBehavior.ground;
        public TileScript Script = TileScript.none;

        public static Vector2 TileSize = new Vector2(50, 50);
        public Vector2 Position = Vector2.Zero;

        private Texture2D image;

        public Tile(TileBehavior behavior, Texture2D image, Vector2 position, TileScript script)
        {
            this.Behavior = behavior;
            this.image = image;
            this.Position = position;
            this.Script = script;
        }

        public bool InTile(Vector2 position)
        {
            return position.X >= Position.X && position.X <= Position.X + TileSize.X &&
                position.Y >= Position.Y && position.Y <= Position.Y + TileSize.Y;
        }

        public void Draw(SpriteBatch batch, Camera camera)
        {
            if (image != null)
            {
                batch.Draw(image, camera.GetRelativePosition(Position), Color.White);
            }
        }
    }
}
