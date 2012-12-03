using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace NinjaVersusSlime
{
    public delegate void standard(int indexAdd);

    class Level
    {
        public Tile[,] tiles;
        public float bottom = 0.0f;
        ContentManager content;
        public Player player;
        Camera camera;
        Vector2 exit = Vector2.Zero;
        public standard endLevel;
        List<Vector2> clouds = new List<Vector2>();
        Texture2D cloudTexture;
        GroundScroll scroll;
        List<Enemy> enemies = new List<Enemy>();
        float deathInitialTime = 0.0f;
        bool startedDeathClock = false;

        #region tile resources
        Texture2D tileMain;
        Texture2D slimeMain;
        Texture2D slimeTop;

        private void LoadTileResources()
        {
            tileMain = content.Load<Texture2D>("TileMain");
            slimeMain = content.Load<Texture2D>("SlimeMain");
            slimeTop = content.Load<Texture2D>("SlimeTop");
        }
        #endregion

        public Level(int levelIndex, ContentManager content, standard endLevel)
        {
            string level = Levels.LevelData.LevelText[levelIndex];
            this.content = content;
            camera = new Camera();
            LoadLevel(level);

            cloudTexture = content.Load<Texture2D>("Cloud");
            for (int n = 0; n < tiles.GetLength(0) * tiles.GetLength(1) / 20.0f; n++)
            {
                clouds.Add(new Vector2(Helper.rnd.Next((int)(tiles.GetLength(0) * Tile.TileSize.X / 0.2f)),
                    Helper.rnd.Next((int)(tiles.GetLength(1) * Tile.TileSize.Y / 0.2f))));
            }

            scroll = new GroundScroll(content, tiles.GetLength(1));

            this.endLevel = endLevel;
        }

        private void LoadLevel(string level)
        {
            List<string> lines = level.Split('\n', '\r').ToList<string>();
            int count = 0;
            foreach (string str in lines)
            {
                if (str == "")
                {
                    count++;
                }
            }
            for (int n = 0; n < count; n++)
                lines.Remove("");

            int width = lines[0].Length;
            int height = lines.Count;

            tiles = new Tile[width, height];

            LoadTileResources();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y] = LoadTile(x, y, lines[y][x]);
                }
            }

            bottom = tiles.GetLength(1) * Tile.TileSize.Y + Tile.TileSize.Y;
        }

        private Tile LoadTile(int x, int y, char type)
        {
            Vector2 p = new Vector2(x, y) * Tile.TileSize;
            switch (type)
            {
                case '.':
                    return new Tile(TileBehavior.air, null, p, TileScript.none);
                case 'T':
                    return new Tile(TileBehavior.ground, tileMain, p, TileScript.none);
                case '1':
                    player = new Player(content, camera, this);
                    player.Position = p;
                    return new Tile(TileBehavior.air, null, p, TileScript.none);
                case 'x':
                    return new Tile(TileBehavior.air, null, p, TileScript.end);
                case 'e':
                    Enemy e = new Enemy(content, camera, this);
                    e.Position = p;
                    enemies.Add(e);
                    return new Tile(TileBehavior.air, null, p, TileScript.none);
                case 'w':
                    return new Tile(TileBehavior.air, slimeTop, p, TileScript.none);
                case 's':
                    return new Tile(TileBehavior.air, slimeMain, p, TileScript.kill);
                default:
                    throw new Exception("Invalid tile");
            }
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            if (startedDeathClock)
            {
                if ((float)gameTime.TotalGameTime.TotalSeconds - deathInitialTime > 3.0f)
                {
                    endLevel.Invoke(0);
                }
            }

            foreach (Tile tile in tiles)
            {
                if (tile.InTile(new Vector2(player.PlayerBox.Center.X, player.PlayerBox.Center.Y)))
                {
                    if (tile.Script == TileScript.end)
                    {
                        endLevel.Invoke(1);
                    }
                    else if (tile.Script == TileScript.kill)
                    {
                        player.scream.Play();
                        endLevel.Invoke(0);
                    }
                }
            }
            foreach (Enemy e in enemies)
                e.Update(gameTime);
        }

        public void StartDeathClock(GameTime gameTime)
        {
            startedDeathClock = true;
            deathInitialTime = (float)gameTime.TotalGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int index = 0;
            foreach (Vector2 v in clouds)
            {
                spriteBatch.Draw(cloudTexture, camera.GetRelativePosition(v) * 0.2f, null, Color.White, 0.0f,
                    Vector2.Zero, 1.0f, (SpriteEffects)(index % 2), 0.0f);
                index++;
            }

            scroll.Draw(spriteBatch, camera);

            foreach (Tile t in tiles)
            {
                if (t != null)
                    t.Draw(spriteBatch, camera);
            }

            player.Draw(spriteBatch, gameTime);
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch, gameTime);
            }
        }
    }
}
