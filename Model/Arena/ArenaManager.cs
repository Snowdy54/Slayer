using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Model.EnemyLogic;
using System;
using System.Collections.Generic;

namespace MyGame.Model.Arena
{
    public class ArenaManager : IGameUpdatable
    {
        private Tile[,] tiles;
        public int width, height;
        private TileFactory tileFactory;
        private EventTimeline eventTimeline;
        private EnemyManager enemyManager;
        private const int TileSize = 96;


        public ArenaManager(int width, int height, EnemyManager enemyManager)
        {
            this.width = width;
            this.height = height;
            this.enemyManager = enemyManager;
            tileFactory = new TileFactory();
            tiles = new Tile[width, height];
            eventTimeline = new EventTimeline();
        }

        public void SetEnemyManager(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        public void GenerateRandomMap()
        {
            var rand = new Random();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    var roll = rand.NextDouble();
                    TileType type = TileType.Floor;
                    if (roll < 0.1) type = TileType.Fire;
                    else if (roll < 0.2) type = TileType.Mud;
                    else if (roll < 0.3) type = TileType.Pit;

                    tiles[x, y] = tileFactory.CreateTile(type, new Point(x, y));
                    tiles[x, y].SetArenaManager(this);

                }
        }

        public void Update(GameTime gameTime)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    tiles[x, y]?.Update(gameTime);

            eventTimeline.Update(gameTime);
        }

        public Tile GetTileAt(int x, int y) =>
            x >= 0 && x < width && y >= 0 && y < height ? tiles[x, y] : null;

        public void InteractWithTile(Entity entity)
        {
            // Вычисляем границы героя в мировых координатах
            float left = entity.position.X - entity.size.X / 2;
            float right = entity.position.X + entity.size.X / 2;
            float top = entity.position.Y - entity.size.Y / 2;
            float bottom = entity.position.Y + entity.size.Y / 2;

            // Переводим в индексы тайлов
            int leftTile = (int)(left / TileSize);
            int rightTile = (int)(right / TileSize);
            int topTile = (int)(top / TileSize);
            int bottomTile = (int)(bottom / TileSize);

            // Перебираем все тайлы, на которых стоит герой
            for (int x = leftTile; x <= rightTile; x++)
            {
                for (int y = topTile; y <= bottomTile; y++)
                {
                    var tile = GetTileAt(x, y);
                    tile?.Interact(entity);
                }
            }
        }


        public IEnumerable<Tile> GetNeighbors(Point position)
        {
            int x = position.X;
            int y = position.Y;

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                var nx = x + dx[i];
                var ny = y + dy[i];

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    yield return tiles[nx, ny];
            }
        }

        public void ReplaceTile(Point position, Tile newTile)
        {
            tiles[position.X, position.Y] = newTile;
            newTile.SetArenaManager(this);
        }

        public Point? FindFirstFloorTile()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var tile = GetTileAt(x, y);
                    if (tile is FloorTile)
                        return new Point(x, y);
                }
            }
            return null;
        }


    }
}
