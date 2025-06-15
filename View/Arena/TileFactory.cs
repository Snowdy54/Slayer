using Microsoft.Xna.Framework.Graphics;
using MyGame.Model.Arena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.View.Arena
{
    public class TileFactory
    {
        private Texture2D fireTexture, mudTexture, pitTexture, floorTexture;

        public TileFactory(Texture2D fire, Texture2D mud, Texture2D pit, Texture2D floor)
        {
            fireTexture = fire;
            mudTexture = mud;
            pitTexture = pit;
            floorTexture = floor;
        }

        public ITileView CreateView(Tile tile)
        {
            return tile switch
            {
                FireTile fire => new FireTileView(fire, fireTexture),
                MudTile mud => new MudTileView(mud, mudTexture),
                PitTile pit => new PitTileView(pit, pitTexture),
                FloorTile floor => new FloorTileView(floor, floorTexture),
                _ => null
            };
        }

    }

}
