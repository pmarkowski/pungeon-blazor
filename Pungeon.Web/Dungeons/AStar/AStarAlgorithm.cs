using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons.AStar
{
    public static class AStarAlgorithm
    {
        public static List<RelativePosition> FindPath(
            Grid grid,
            RelativePosition startPosition,
            RelativePosition endPosition)
        {
            bool foundPath = false;
            Dictionary<(int x, int y), Node> openList = new Dictionary<(int x, int y), Node>();
            HashSet<(int x, int y)> closedList = new HashSet<(int x, int y)>();
            Node current = new Node
            {
                Position = startPosition,
                RunningCost = 0,
                EstimatedRemainingCost =
                    DistanceCalculator.GetManhattanDistance(
                        startPosition,
                        endPosition),
                Parent = null
            };
            openList.Add((current.Position.X, current.Position.Y), current);

            while (openList.Any())
            {
                current = openList.Values.OrderBy(node => node.TotalCost).First();
                int currentLowestCost = current.TotalCost;

                closedList.Add((current.Position.X, current.Position.Y));
                openList.Remove((current.Position.X, current.Position.Y));

                if (closedList.Contains((endPosition.X, endPosition.Y)))
                {
                    foundPath = true;
                    break;
                }

                List<RelativePosition> adjacentWalkablePositions =
                    GetWalkableAdjacentPositions(grid, current.Position);
                currentLowestCost++;

                foreach (RelativePosition position in adjacentWalkablePositions)
                {
                    if (!closedList.Contains((position.X, position.Y)))
                    {
                        if (!(openList.ContainsKey((position.X, position.Y))))
                        {
                            openList.Add(
                                (position.X, position.Y),
                                new Node
                                {
                                    Position = position,
                                    RunningCost = currentLowestCost,
                                    EstimatedRemainingCost =
                                        DistanceCalculator.GetManhattanDistance(position, endPosition),
                                    Parent = current
                                });
                        }
                        else
                        {
                            Node adjacentNode = openList[(position.X, position.Y)];

                            if (currentLowestCost + adjacentNode.EstimatedRemainingCost < adjacentNode.TotalCost)
                            {
                                adjacentNode.RunningCost = currentLowestCost;
                                adjacentNode.Parent = current;
                            }
                        }
                    }
                }
            }

            List<RelativePosition> result = null;

            if (foundPath)
            {
                result = ConstructPath(current);
            }

            return result;
        }

        private static List<RelativePosition> GetWalkableAdjacentPositions(Grid grid, RelativePosition position)
        {
            List<RelativePosition> adjacentWalkablePositions = new List<RelativePosition>();

            int x = position.X;
            int y = position.Y;

            AddPositionToListIfValid(grid, adjacentWalkablePositions, x, y - 1);
            AddPositionToListIfValid(grid, adjacentWalkablePositions, x + 1, y);
            AddPositionToListIfValid(grid, adjacentWalkablePositions, x, y + 1);
            AddPositionToListIfValid(grid, adjacentWalkablePositions, x - 1, y);

            return adjacentWalkablePositions;
        }

        private static void AddPositionToListIfValid(Grid grid, List<RelativePosition> adjacentWalkablePositions, int newX, int newY)
        {
            if (grid[newX, newY].Character != ' ' && grid[newX, newY].Character != '|')
            {
                adjacentWalkablePositions.Add(new RelativePosition(newX, newY));
            }
        }

        private static List<RelativePosition> ConstructPath(Node current)
        {
            List<RelativePosition> path = new List<RelativePosition>();

            while (current.Parent != null)
            {
                path.Add(current.Position);
                current = current.Parent;
            }
            path.Add(current.Position);

            return path;
        }
    }
}