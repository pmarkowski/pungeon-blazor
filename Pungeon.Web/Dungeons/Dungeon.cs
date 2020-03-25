using System;
using System.Collections.Generic;
using System.Linq;
using Pungeon.Web.Dungeons.AStar;

namespace Pungeon.Web.Dungeons
{
    public class Dungeon
    {
        public List<DungeonRoom> Rooms { get; set; }
        public List<Connection> Connections { get; set; }

        public Dungeon()
        {
            Rooms = new List<DungeonRoom>();
            Connections = new List<Connection>();
        }

        public int GetWidth()
        {
            return Rooms.Any() ? Rooms.Max(room => room.RelativePosition.X + room.Room.GetWidth()) : 0;
        }

        public int GetHeight()
        {
            return Rooms.Any() ? Rooms.Max(room => room.RelativePosition.Y + room.Room.GetHeight()) : 0;
        }

        public Grid ToGrid()
        {
            Grid grid = new Grid(GetWidth(), GetHeight());

            foreach (DungeonRoom room in Rooms)
            {
                HollowOutRoomInGrid(grid, room);
            }

            foreach (Connection connection in Connections)
            {
                Connector connector1 = GetConnectorInDungeonSpace(connection.ConnectorId1);
                Connector connector2 = GetConnectorInDungeonSpace(connection.ConnectorId2);

                var path = AStarAlgorithm.FindPath(
                    grid,
                    connector1.RelativePosition,
                    connector2.RelativePosition);

                HollowOutPathInGrid(grid, path);
            }

            return grid;
        }

        private Connector GetConnectorInDungeonSpace(string connectorId)
        {
            return Rooms.SelectMany(room =>
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

        private static void HollowOutRoomInGrid(Grid grid, DungeonRoom room)
        {
            foreach (Space space in room.Room.Spaces)
            {
                HollowOutSpaceInGrid(
                    grid,
                    space,
                    room.RelativePosition.X,
                    room.RelativePosition.Y);
            }
        }

        private static void HollowOutSpaceInGrid(Grid grid, Space space, int xOffset, int yOffset)
        {
            int yStart = yOffset + space.RelativePosition.Y;

            for (int y = yStart; y < yStart + space.Size.Height; y++)
            {
                int xStart = xOffset + space.RelativePosition.X;

                for (int x = xStart; x < xStart + space.Size.Width; x++)
                {
                    grid[x, y] = ' ';
                }
            }
        }

        private static void HollowOutPathInGrid(Grid grid, List<RelativePosition> path)
        {
            foreach (RelativePosition position in path)
            {
                int y = position.Y;
                int x = position.X;

                grid[x, y] = ' ';
            }
        }
    }
}