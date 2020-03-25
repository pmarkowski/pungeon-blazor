using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pungeon.Web.Dungeons.AStar;

namespace Pungeon.Web.Dungeons.Rendering
{
    public static class AsciiRenderer
    {
        private const int Padding = 1;

        public static string Render(Dungeon dungeon)
        {
            char[,] charGrid = new char[
                dungeon.GetHeight() + Padding * 2,
                dungeon.GetWidth() + Padding * 2
            ];
            InitializeCharGrid(charGrid);

            foreach (DungeonRoom room in dungeon.Rooms)
            {
                RenderRoomInGrid(charGrid, room, Padding, Padding);
            }

            foreach (Connection connection in dungeon.Connections)
            {
                Connector connector1 = GetConnectorInDungeonSpace(dungeon, connection.ConnectorId1);
                Connector connector2 = GetConnectorInDungeonSpace(dungeon, connection.ConnectorId2);

                var path = AStarAlgorithm.FindPath(
                    charGrid,
                    connector1.RelativePosition,
                    connector2.RelativePosition);

                HollowOutPathInGrid(charGrid, path);
            }

            return GridToString(charGrid);
        }

        private static Connector GetConnectorInDungeonSpace(Dungeon dungeon, string connectorId)
        {
            return dungeon.Rooms.SelectMany(room =>
                room.Room.Spaces.SelectMany(space =>
                    space.Connectors.Select(connector =>
                        new Connector
                        {
                            Id = connector.Id,
                            RelativePosition = new RelativePosition(
                                connector.RelativePosition.X + space.RelativePosition.X + room.RelativePosition.X,
                                connector.RelativePosition.Y + space.RelativePosition.Y + room.RelativePosition.Y
                            )
                        })))
                .Single(connector => connector.Id == connectorId);
        }

        private static void HollowOutPathInGrid(char[,] charGrid, List<RelativePosition> path)
        {
            foreach (RelativePosition position in path)
            {
                int y = position.Y;
                int x = position.X;

                charGrid[y, x] = ' ';
            }
        }

        private static void RenderRoomInGrid(char[,] charGrid, DungeonRoom room, int xOffset, int yOffset)
        {
            foreach (Space space in room.Room.Spaces)
            {
                HollowOutSpaceInGrid(
                    charGrid,
                    space,
                    room.RelativePosition.X + xOffset,
                    room.RelativePosition.Y + yOffset);
            }
        }

        private static void HollowOutSpaceInGrid(char[,] charGrid, Space space, int xOffset, int yOffset)
        {
            int yStart = yOffset + space.RelativePosition.Y;

            // TODO: consider renaming these iterators to y and x to have an easier time
            for (int i = yStart; i < yStart + space.Size.Height; i++)
            {
                int xStart = xOffset + space.RelativePosition.X;

                for (int j = xStart; j < xStart + space.Size.Width; j++)
                {
                    charGrid[i, j] = ' ';
                }
            }
        }

        private static void InitializeCharGrid(char[,] charGrid)
        {
            for (int i = 0; i < charGrid.GetLength(0); i++)
            {
                for (int j = 0; j < charGrid.GetLength(1); j++)
                {
                    charGrid[i, j] = '#';
                }
            }
        }

        private static string GridToString(char[,] charGrid)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < charGrid.GetLength(0); i++)
            {
                for (int j = 0; j < charGrid.GetLength(1); j++)
                {
                    builder.Append(charGrid[i, j]);
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}