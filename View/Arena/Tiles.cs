using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Model.Arena;

namespace MyGame.View.Arena
{
    public interface ITileView
    {
        void Draw(SpriteBatch spriteBatch, int screenX, int screenY);
        void Update(GameTime gameTime);
    }

    public class FireTileView : ITileView
    {
        private FireTile _model;
        private Texture2D _texture;
        private const int tileSize = 96;

        public FireTileView(FireTile model, Texture2D texture)
        {
            _model = model;
            _texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            // Можно анимацию
        }

        public void Draw(SpriteBatch spriteBatch, int screenX, int screenY)
        {
            spriteBatch.Draw(_texture, new Rectangle(screenX, screenY, tileSize, tileSize), Color.White);
        }


    }

    public class MudTileView : ITileView
    {
        private MudTile _model;
        private Texture2D _texture;
        private const int tileSize = 96;

        public MudTileView(MudTile model, Texture2D texture)
        {
            _model = model;
            _texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            // Можно анимацию
        }

        public void Draw(SpriteBatch spriteBatch, int screenX, int screenY)
        {
            spriteBatch.Draw(_texture, new Rectangle(screenX, screenY, tileSize, tileSize), Color.White);
        }

    }

    public class PitTileView : ITileView
    {
        private PitTile _model;
        private Texture2D _texture;
        private const int tileSize = 96;

        public PitTileView(PitTile model, Texture2D texture)
        {
            _model = model;
            _texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            // Можно анимацию
        }

        public void Draw(SpriteBatch spriteBatch, int screenX, int screenY)
        {
            spriteBatch.Draw(_texture, new Rectangle(screenX, screenY, tileSize, tileSize), Color.White);
        }

    }

    public class FloorTileView : ITileView
    {
        private FloorTile _model;
        private Texture2D _texture;
        private const int tileSize = 96;

        public FloorTileView(FloorTile model, Texture2D texture)
        {
            _model = model;
            _texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            // Можно анимацию
        }

        public void Draw(SpriteBatch spriteBatch, int screenX, int screenY)
        {
            spriteBatch.Draw(_texture, new Rectangle(screenX, screenY, tileSize, tileSize), Color.White);
        }

    }
}