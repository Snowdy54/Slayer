using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.View.Arena
{
    public class ArenaView
    {
        private ITileView[,] tileViews;
        private int width, height;
        private Camera camera;

        public ArenaView(ITileView[,] tileViews, int width, int height, Camera camera)
        {
            this.tileViews = tileViews;
            this.width = width;
            this.height = height;
            this.camera = camera;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var screenPos = camera.GetScreenPosition(x, y);
                    tileViews[x, y].Draw(spriteBatch, screenPos.X, screenPos.Y);
                }
            }
        }



        public void Update(GameTime gameTime)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    tileViews[x, y]?.Update(gameTime);

        }

    }

}
