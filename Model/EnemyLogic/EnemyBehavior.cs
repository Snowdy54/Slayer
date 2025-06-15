using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace MyGame.Model.EnemyLogic
{
    public class EnemyBehavior
    {
        private readonly AStarPathfinder pathfinder;
        private readonly float speed;
        private List<Point> currentPath = new();
        private int pathIndex = 0;
        private readonly Func<Vector2> getPosition;
        private readonly Action<Vector2> setPosition;
        private const int TileSize = 96;
        private const float pathUpdateInterval = 0.4f;

        public EnemyBehavior(Func<Vector2> getPosition, Action<Vector2> setPosition,
            float speed, AStarPathfinder pathfinder)
            {
                this.getPosition = getPosition;
                this.setPosition = setPosition;
                this.speed = speed;
                this.pathfinder = pathfinder;
            }

        private Point? lastHeroTile = null;
        private float pathUpdateTimer = 0f;

        public void Update(float deltaTime, Vector2 heroPos)
        {
            pathUpdateTimer += deltaTime;

            Point currentHeroTile = WorldToTile(heroPos);
            Point enemyTile = WorldToTile(getPosition());

            if (pathUpdateTimer >= pathUpdateInterval)
            {
                if (lastHeroTile == null || lastHeroTile.Value != currentHeroTile)
                {
                    if (pathfinder.IsWalkable(currentHeroTile))
                    {
                        var newPath = pathfinder.FindPath(enemyTile, currentHeroTile);
                        if (newPath != null && newPath.Count > 1)
                        {
                            currentPath = newPath;
                            pathIndex = 1;
                            lastHeroTile = currentHeroTile;
                        }
                    }
                }

                pathUpdateTimer = 0f;
            }
            MoveAlongPath(deltaTime);
        }

        private void MoveAlongPath(float deltaTime)
        {
            if (currentPath == null || pathIndex >= currentPath.Count)
                return;

            var target = TileToWorld(currentPath[pathIndex]);
            var pos = getPosition();

            if (Vector2.Distance(pos, target) < 8f)
            {
                pathIndex++;
                if (pathIndex >= currentPath.Count)
                    return;

                target = TileToWorld(currentPath[pathIndex]);
            }


            MoveTowards(target, deltaTime);
        }

        private void MoveTowards(Vector2 target, float deltaTime)
        {
            var pos = getPosition();
            var direction = target - pos;

            if (direction.LengthSquared() > 0.5f)
            {
                direction.Normalize();
                var movement = direction * speed * deltaTime;

                if (movement.LengthSquared() > (target - pos).LengthSquared())
                    movement = target - pos;

                setPosition(pos + movement);
            }
        }

        private Point WorldToTile(Vector2 worldPos) =>
            new((int)(worldPos.X / TileSize), (int)(worldPos.Y / TileSize));


        private Vector2 TileToWorld(Point tilePos) =>
            new(tilePos.X * TileSize + TileSize / 2, tilePos.Y * TileSize + TileSize / 2);

    }
}
