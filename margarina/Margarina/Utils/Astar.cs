using System;
using System.Collections.Generic;
using System.Linq;
using Margarina.Models.World;

namespace Margarina.Utils
{
    public static class Astar
    {
        private const string CollisionLayerName = "collision";

        public static List<Point> FindPath(Level map, Point origin, Point destination)
        {
            var layer = map.Layers.Single(ly => ly.Name == CollisionLayerName);

            var openNodes = new List<Node>();
            var closedNodes = new List<Node>();

            var startTile = layer.GetTileAt(origin);

            if (startTile == null)
            {
                return new List<Point>();
            }

            var start = new Node(layer.GetTileAt(origin), null);

            openNodes.Add(start);

            var goal = new Node(destination.X, destination.Y);
            Node current = default;

            while (openNodes.Any() && !closedNodes.Contains(goal))
            {
                current = openNodes.First();
                openNodes.Remove(current);
                var successors = GetSuccessors(layer, current);

                foreach (var successor in successors)
                {
                    if (!closedNodes.Contains(successor) && successor.IsWalkable)
                    {
                        if (!openNodes.Contains(successor))
                        {
                            successor.Accumulated = current.Accumulated + successor.Cost;
                            successor.RunHeuristic(destination);
                            openNodes.Add(successor);
                            openNodes = openNodes.OrderBy(nd => nd.Weight).ToList();
                        }
                    }
                }

                closedNodes.Add(current);
            }

            if (!closedNodes.Contains(goal))
            {
                return new List<Point>();
            }

            var node = current;
            var path = new List<Point>();
            while (node != null && !node.Equals(start))
            {
                path.Add(new Point(node.X, node.Y));
                node = node.Parent;
            }

            return path;
        }

        private static List<Node> GetSuccessors(Layer layer, Node parent)
        {
            var successors = new List<Node>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (layer.IsInBounds(parent.X + i, parent.Y + j))
                    {
                        var tile = layer.GetTileAt(parent.X + i, parent.Y + j);

                        if (tile != null)
                        {
                            successors.Add(new Node(tile, parent));
                        }
                    }
                }
            }

            return successors;
        }

        private class Node : Tile
        {
            public double Accumulated { get; set; }
            public double Heuristic { get; set; }
            public double Weight => Accumulated + Heuristic;
            public Node Parent { get; }
            public bool IsWalkable => Id == 0;
            public double Cost { get; }

            public Node(Tile tile, Node parent) : base(tile)
            {
                Parent = parent;
                Cost = 1;

                if (parent != null && parent.X != tile.X && parent.Y != tile.Y)
                {
                    Cost = 1.9; // make the diagonals cost more than straight lines or else the character will use them when unnecessary
                }
            }

            public Node(int x, int y) : base(0, x, y) { }

            public void RunHeuristic(Point goal)
            {
                Heuristic = Math.Sqrt(Math.Pow(X - goal.X, 2) + Math.Pow(Y - goal.Y, 2));
            }

            public override bool Equals(object obj)
            {
                return obj is Node node &&
                       X == node.X &&
                       Y == node.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }
    }
}
