using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.View
{
    public class Camera
    {
        public int offsetX;
        public int offsetY;

        public int viewportWidth;
        public int viewportHeight;

        private const int TileSize = 96;

        public Camera(int viewportWidth, int viewportHeight)
        {
            this.viewportWidth = viewportWidth;
            this.viewportHeight = viewportHeight;
        }

        public void CenterOn(float pixelX, float pixelY)
        {
            offsetX = (int)(pixelX - viewportWidth / 2);
            offsetY = (int)(pixelY - viewportHeight / 2);
        }

        public Point GetScreenPosition(int tileX, int tileY)
        {
            return new Point(tileX * TileSize - offsetX, tileY * TileSize - offsetY);
        }
    }
}