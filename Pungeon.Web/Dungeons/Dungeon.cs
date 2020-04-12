using System;
using System.Collections.Generic;
using System.Linq;
using Pungeon.Web.Dungeons.AStar;

namespace Pungeon.Web.Dungeons
{
    public class Dungeon
    {
        public Size MinimumSize { get; set; }
        public List<DungeonRoom> Rooms { get; set; }
        public List<Connection> Connections { get; set; }

        public Dungeon()
        {
            MinimumSize = new Size(0, 0);
            Rooms = new List<DungeonRoom>();
            Connections = new List<Connection>();
        }

        public Grid ToGrid()
        {
            Grid grid = (MinimumSize != null) ?
                new Grid(MinimumSize) :
                new Grid();

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

                if (path != null)
                {
                    HollowOutPathInGrid(grid, path);
                }

                // TODO: Hollowing out the path overwrites this so we write it again
                PlaceConnector(grid, connector1.RelativePosition);
                PlaceConnector(grid, connector2.RelativePosition);
            }

            return grid;
        }

        private void PlaceConnector(Grid grid, RelativePosition relativePosition)
        {
            grid[relativePosition.X, relativePosition.Y] = new Tile
            {
                Character = '+'
            };
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
            // outline spaces with walls
            foreach (Space space in room.Room.Spaces)
            {
                HollowOutSpaceInGrid(
                    grid,
                    new Space()
                    {
                        RelativePosition = new RelativePosition(
                            space.RelativePosition.X - 1,
                            space.RelativePosition.Y - 1
                        ),
                        Size = new Size(
                            space.Size.Width + 2,
                            space.Size.Height + 2
                        )
                    },
                    room.RelativePosition.X,
                    room.RelativePosition.Y,
                    '|',
                    room);
            }
            foreach (Space space in room.Room.Spaces)
            {
                HollowOutSpaceInGrid(
                    grid,
                    space,
                    room.RelativePosition.X,
                    room.RelativePosition.Y,
                    ' ',
                    room);
            }
        }

        private static void HollowOutSpaceInGrid(Grid grid, Space space, int xOffset, int yOffset, char fill, DungeonRoom parentRoom)
        {
            int yStart = yOffset + space.RelativePosition.Y;
            int xStart = xOffset + space.RelativePosition.X;

            for (int y = yStart; y < yStart + space.Size.Height; y++)
            {

                for (int x = xStart; x < xStart + space.Size.Width; x++)
                {
                    grid[x, y] = new Tile
                    {
                        Character = fill,
                        ParentRoom = parentRoom
                    };
                }
            }

            foreach (Connector connector in space.Connectors)
            {
                grid[xStart + connector.RelativePosition.X, yStart + connector.RelativePosition.Y] = new Tile
                {
                    Character = '+',
                    ParentRoom = parentRoom
                };
            }
        }

        private static void HollowOutPathInGrid(Grid grid, List<RelativePosition> path)
        {
            foreach (RelativePosition position in path)
            {
                int y = position.Y;
                int x = position.X;

                grid[x, y] = new Tile
                {
                    Character = ' '
                };
            }
        }
    }
}