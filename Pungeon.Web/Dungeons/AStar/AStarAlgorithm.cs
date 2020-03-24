using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons.AStar
{
    public static class AStarAlgorithm
    {
        public static List<RelativePosition> FindPath(
            char[,] charGrid,
            RelativePosition startPosition,
            RelativePosition endPosition)
        {
            bool foundPath = false;
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
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
            openList.Add(current);

            while (openList.Any())
            {
                int currentLowestCost = openList.Min(node => node.TotalCost);
                current = openList.First(node => node.TotalCost == currentLowestCost);

                closedList.Add(current);
                int removeIndex = openList.FindIndex(node =>
                    node.Position.X == current.Position.X &&
                    node.Position.Y == current.Position.Y);
                openList.RemoveAt(removeIndex);

                if (ListContainsPosition(closedList, endPosition))
                {
                    foundPath = true;
                    break;
                }

                List<RelativePosition> adjacentWalkablePositions =
                    GetWalkableAdjacentPositions(charGrid, current.Position);
                currentLowestCost++;

                foreach (RelativePosition position in adjacentWalkablePositions)
                {
                    if (!ListContainsPosition(closedList, position))
                    {
                        if (!ListContainsPosition(openList, position))
                        {
                            openList.Add(new Node
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
                            Node adjacentNode = openList.Single(node =>
                                node.Position.X == position.X &&
                                node.Position.Y == position.Y);

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

        private static List<RelativePosition> GetWalkableAdjacentPositions(char[,] charGrid, RelativePosition position)
        {
            List<RelativePosition> adjacentWalkablePositions = new List<RelativePosition>();

            int x = position.X;
            int y = position.Y;

            AddPositionToListIfValid(charGrid, adjacentWalkablePositions, x, y - 1);
            AddPositionToListIfValid(charGrid, adjacentWalkablePositions, x + 1, y);
            AddPositionToListIfValid(charGrid, adjacentWalkablePositions, x, y + 1);
            AddPositionToListIfValid(charGrid, adjacentWalkablePositions, x - 1, y);

            return adjacentWalkablePositions;
        }

        private static void AddPositionToListIfValid(char[,] charGrid, List<RelativePosition> adjacentWalkablePositions, int newX, int newY)
        {
            if (0 <= newY && newY < charGrid.GetLength(0) &&
                0 <= newX && newX < charGrid.GetLength(1) &&
                charGrid[newY, newX] != ' ')
            {
                adjacentWalkablePositions.Add(new RelativePosition(newX, newY));
            }
        }

        private static bool ListContainsPosition(List<Node> list, RelativePosition position)
        {
            return list.Any(node =>
                node.Position.X == position.X &&
                node.Position.Y == position.Y);
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