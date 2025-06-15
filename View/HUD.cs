using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Model;

namespace MyGame.View
{
    public class HUD
    {
        private readonly SpriteBatch spriteBatch;
        private readonly GameModel model;
        private readonly SpriteFont font;

        public HUD(SpriteBatch spriteBatch, GameModel model, SpriteFont font)
        {
            this.spriteBatch = spriteBatch;
            this.model = model;
            this.font = font;
        }

        public void Draw(GameTime gameTime)
        {
            if (!model.IsGameOver())
                DrawHeroHPBar();   
            else
                DrawGameOver();
        }

        private void DrawHeroHPBar()
        {
            var barWidth = 200;
            var barHeight = 20;

            var position = new Vector2(10, 40);

            var backgroundColor = Color.Gray;
            var healthColor = Color.Red;

            var healthBarWidth = (int)(barWidth * (model.Hero.HP / 100f));

            if (healthBarWidth <= 0)
                healthBarWidth = 1;

            spriteBatch.Draw(CreateTexture(barWidth, barHeight, backgroundColor), position, Color.White);
            spriteBatch.Draw(CreateTexture(healthBarWidth, barHeight, healthColor), position, Color.White);
        }

        private void DrawGameOver()
        {
            var position = new Vector2(300, 200);

            var gameOverText = "YOU DIED";

            spriteBatch.DrawString(font, gameOverText, position, Color.Red);
        }

        private Texture2D CreateTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];

            for (int i = 0; i < data.Length; i++)
                data[i] = color;

            texture.SetData(data);
            return texture;
        }
    }
}
