using Microsoft.Xna.Framework;
using System;

namespace MyGame.Model.Arena
{
    public enum TileType
    {
        Floor,
        Fire,
        Mud,
        Pit
    }

    public abstract class Tile
    {
        protected ArenaManager arenaManager;

        public void SetArenaManager(ArenaManager arena) =>
            arenaManager = arena;

        public Point Position { get; private set; }

        protected Tile(Point position)
        {
            Position = position;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Interact(Entity entity);
    }

    public class FireTile : Tile
    {
        public FireTile(Point position) : base(position) { }

        public override void Update(GameTime gameTime)
        {
            /*if (new Random().NextDouble() < 0.01)
            {
                foreach (var neighbor in arenaManager.GetNeighbors(Position))
                {
                    if (neighbor is FloorTile)
                        arenaManager.ReplaceTile(neighbor.Position, new FireTile(neighbor.Position));
                }
            }*/
        }


        public override void Interact(Entity entity)
        {
/*           entity.TakeDamage(10);*/
        }
    }

    public class MudTile : Tile
    {
        public MudTile(Point position) : base(position) { }

        public override void Update(GameTime gameTime) { }

        public override void Interact(Entity entity)
        {
/*            entity.MoveSpeed = (int)(entity.MoveSpeed * 0.5); // можно сделать множитель или проверку*/
        }
    }

    public class PitTile : Tile
    {
        public PitTile(Point position) : base(position) { }

        public override void Update(GameTime gameTime) { }

        public override void Interact(Entity entity)
        {
/*            entity.HP = 0;*/
        }
    }

    public class FloorTile : Tile
    {
        public FloorTile(Point position) : base(position) { }

        public override void Update(GameTime gameTime) { }

        public override void Interact(Entity entity) { }
    }

    public class TileFactory
    {
        public Tile CreateTile(TileType type, Point position)
        {
            return type switch
            {
                TileType.Floor => new FloorTile(position),
                TileType.Fire => new FireTile(position),
                TileType.Mud => new MudTile(position),
                TileType.Pit => new PitTile(position),
                _ => new FloorTile(position),
            };
        }
    }
}
