using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Model;
using MyGame.View.Arena;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.View
{
    public class GameView
    {
        private readonly SpriteBatch spriteBatch;
        private readonly Texture2D heroTexture;
        private readonly Texture2D enemyTexture;
        private readonly EnemyVisual heroVisual;
        private List<EnemyVisual> enemyVisuals = new();
        private readonly GameModel model;
        private readonly HUD hud;
        private ArenaView arenaView;
        private TileFactory tileFactory;
        private Camera camera;

        public GameView(GameModel model, ContentManager content, GraphicsDevice graphicsDevice)
        {

            this.model = model;
            spriteBatch = new SpriteBatch(graphicsDevice);
            heroTexture = content.Load<Texture2D>("mainHero");
            enemyTexture = content.Load<Texture2D>("wood-monster");

            model.Arena.GenerateRandomMap();
            heroVisual = new EnemyVisual(heroTexture, model.Hero);

            hud = new HUD(spriteBatch, model, content.Load<SpriteFont>("MyFont"));

            var fireTexture = content.Load<Texture2D>("fire");
            var mudTexture = content.Load<Texture2D>("mud");
            var pitTexture = content.Load<Texture2D>("pit");
            var floorTexture = content.Load<Texture2D>("floor");

            tileFactory = new TileFactory(fireTexture, mudTexture, pitTexture, floorTexture);

            int width = model.Arena.width;
            int height = model.Arena.height;

            ITileView[,] tileViews = new ITileView[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var tile = model.Arena.GetTileAt(x, y);
                    tileViews[x, y] = tileFactory.CreateView(tile);
                }
            }
            camera = new Camera(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            arenaView = new ArenaView(tileViews, width, height, camera);
        }

        public void Draw(GameTime gameTime)
        {
            enemyVisuals = model.EnemyManager.GetActiveEnemies()
                .Select(e => new EnemyVisual(enemyTexture, e)).ToList();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            arenaView.Draw(spriteBatch);

            heroVisual.Draw(spriteBatch, camera);

            foreach (var visual in enemyVisuals)
                visual.Draw(spriteBatch, camera);

            hud.Draw(gameTime);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            camera.CenterOn(model.Hero.position.X, model.Hero.position.Y);

            arenaView.Update(gameTime);
        }

    }
}
