using Microsoft.Xna.Framework;
using MyGame.Model.Arena;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Model
{
    public class AStarPathfinder
    {
        private readonly ArenaManager arena;
        private readonly int width;
        private readonly int height;
        private static readonly (int dx, int dy)[] directions =
                {(-1,  0), (1,  0), (0, -1), (0, 1),
                (-1, -1), (-1, 1), (1, -1), (1, 1)};

        public AStarPathfinder(ArenaManager arena)
        {
            this.arena = arena;
            width = arena.width;
            height = arena.height;
        }

        private class Node
        {
            public Point Position;
            public Node Parent;
            public float walkedDistance;
            public float distanceToTarget;
            public float PathCost => walkedDistance + distanceToTarget;
        }

        private static float Heuristic(Point a, Point b)
        {
            int dx = Math.Abs(a.X - b.X);
            int dy = Math.Abs(a.Y - b.Y);
            int min = Math.Min(dx, dy);
            int max = Math.Max(dx, dy);
            return 1.4142f * min + (max - min);
        }

        public List<Point> FindPath(Point start, Point goal)
        {
            var openSet = new List<Node>();
            var closedSet = new HashSet<Point>();

            var startNode = new Node { Position = start, walkedDistance = 0, distanceToTarget = Heuristic(start, goal), Parent = null };
            openSet.Add(startNode);

            var allNodes = new Dictionary<Point, Node> { [start] = startNode };

            while (openSet.Count > 0)
            {
                var current = openSet.OrderBy(n => n.PathCost).First();

                if (current.Position == goal)
                {
                    var path = new List<Point>();
                    var node = current;
                    while (node != null)
                    {
                        path.Add(node.Position);
                        node = node.Parent;
                    }
                    path.Reverse();

                    return SmoothPath(path);
                }

                openSet.Remove(current);
                closedSet.Add(current.Position);

                foreach (var neighborPos in GetNeighbors(current.Position))
                {
                    if (closedSet.Contains(neighborPos))
                        continue;

                    if (!IsWalkable(neighborPos))
                        continue;

                    float cost = (neighborPos.X != current.Position.X && neighborPos.Y != current.Position.Y) ? 1.4142f : 1f;
                    float tentativeG = current.walkedDistance + cost;

                    if (!allNodes.TryGetValue(neighborPos, out var neighborNode))
                    {
                        neighborNode = new Node
                        {
                            Position = neighborPos,
                            Parent = current,
                            walkedDistance = tentativeG,
                            distanceToTarget = Heuristic(neighborPos, goal)
                        };
                        allNodes[neighborPos] = neighborNode;
                        openSet.Add(neighborNode);
                    }
                    else if (tentativeG < neighborNode.walkedDistance)
                    {
                        neighborNode.walkedDistance = tentativeG;
                        neighborNode.Parent = current;
                        if (!openSet.Contains(neighborNode))
                            openSet.Add(neighborNode);
                    }
                }
            }

            return null;
        }

        private IEnumerable<Point> GetNeighbors(Point pos)
        {
            foreach (var (dx, dy) in directions)
            {
                int nx = pos.X + dx;
                int ny = pos.Y + dy;

                if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                    continue;

                var neighbor = new Point(nx, ny);
                if (!IsWalkable(neighbor))
                    continue;

                if (dx != 0 && dy != 0)
                {
                    var tile1 = new Point(pos.X + dx, pos.Y);
                    var tile2 = new Point(pos.X, pos.Y + dy);

                    if (!IsWalkable(tile1) || !IsWalkable(tile2))
                        continue;
                }

                yield return neighbor;
            }
        }



        public bool IsWalkable(Point pos)
        {
            var tile = arena.GetTileAt(pos.X, pos.Y);
            if (tile == null)
                return false;

            return tile is not PitTile && tile is not FireTile;
        }

        private List<Point> SmoothPath(List<Point> path)
        {
            if (path == null || path.Count < 3)
                return path;

            var smoothPath = new List<Point>();
            int currentIndex = 0;
            smoothPath.Add(path[currentIndex]);

            while (currentIndex < path.Count - 1)
            {
                int nextIndex = path.Count - 1;
                for (int i = path.Count - 1; i > currentIndex; i--)
                {
                    if (BrazenchemAlgorithm(path[currentIndex], path[i]))
                    {
                        nextIndex = i;
                        break;
                    }
                }

                smoothPath.Add(path[nextIndex]);
                currentIndex = nextIndex;
            }

            return smoothPath;
        }

        private bool BrazenchemAlgorithm(Point start, Point end)
        {
            var x0 = start.X;
            var y0 = start.Y;
            var x1 = end.X;
            var y1 = end.Y;

            var distanceX = Math.Abs(x1 - x0);
            var distanceY = Math.Abs(y1 - y0);

            var movementX = x0 < x1 ? 1 : -1;
            var movementY = y0 < y1 ? 1 : -1;

            var deviation = distanceX - distanceY;

            while (x0 != x1 || y0 != y1)
            {
                Point current = new(x0, y0);
                if (!IsWalkable(current))
                    return false;

                var deviation2 = 2 * deviation;
                var nextX = x0;
                var nextY = y0;

                if (deviation2 > -distanceY)
                {
                    deviation -= distanceY;
                    nextX += movementX;
                }
                if (deviation2 < distanceX)
                {
                    deviation += distanceX;
                    nextY += movementY;
                }

                if (nextX != x0 && nextY != y0)
                {
                    var side1 = new Point(x0, nextY);
                    var side2 = new Point(nextX, y0);

                    if (!IsWalkable(side1) || !IsWalkable(side2))
                        return false;
                }
                x0 = nextX;
                y0 = nextY;
            }
            return IsWalkable(new Point(x1, y1));
        }
    }
}
