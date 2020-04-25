using System;
using System.Collections.Generic;

namespace Pungeon.Web.Dungeons
{
    public class Dungeon
    {
        public Size MinimumSize { get; set; }
        public List<DungeonRoom> Rooms { get; set; }

        public Dungeon()
        {
            MinimumSize = new Size(0, 0);
            Rooms = new List<DungeonRoom>();
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

            return grid;
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
        }

        public void RemoveRoom(Guid roomId)
        {
            int removalIndex = Rooms.FindIndex(room => room.Room.Id == roomId);
            Rooms.RemoveAt(removalIndex);
        }
    }
}