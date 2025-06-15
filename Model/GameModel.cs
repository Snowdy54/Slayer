using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Model.Arena;
using MyGame.Model.EnemyLogic;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MyGame.Model
{
    public class GameModel
    {
        public Hero Hero;
        public EnemyManager EnemyManager;
        public TimeManager TimeManager;
        public Move Mover;
        public bool GameOver;
        public ArenaManager Arena;
        public AStarPathfinder Pathfinder;
        private const int TileSize = 96;
        private const int TileCenter = TileSize / 2;

        private const int width = 100;
        private const int height = 100;
        private const int centerX = width / 2;
        private const int centerY = height / 2;

        public GameModel()
        {
            Hero = new Hero(new Vector2(centerX * TileSize + TileCenter, centerY * TileSize + TileCenter));

            Arena = new ArenaManager(width, height, null);
            Arena.GenerateRandomMap();

            Pathfinder = new AStarPathfinder(Arena);

            EnemyManager = new EnemyManager(Hero, Pathfinder);

            Arena.SetEnemyManager(EnemyManager);

            Mover = new Move(Hero, Vector2.Zero);

            TimeManager = new TimeManager();
            TimeManager.Register(Mover);
            TimeManager.Register(EnemyManager);
            TimeManager.Register(Arena);

            EnemyManager.LoadEnemies(new List<Vector2>
            {
                new((centerX + 1) * TileSize + TileCenter, (centerY + 2) * TileSize + TileCenter),
                new((centerX + 1) * TileSize + TileCenter, (centerY + 1) * TileSize + TileCenter)
            });
        }

        public bool IsGameOver() => Hero.IsDead();

        public void Update(GameTime gameTime)
        {
            if (!GameOver)
            {
                TimeManager.Update(gameTime);
                double deltaSeconds = gameTime.ElapsedGameTime.TotalSeconds;

                if (Hero.CanApplyTileEffect(deltaSeconds))
                    Arena.InteractWithTile(Hero);

                if (IsGameOver())
                    GameOver = true;
            }
        }

        public List<Entity> GetTargets() => EnemyManager.GetActiveEnemies();
    }
}
