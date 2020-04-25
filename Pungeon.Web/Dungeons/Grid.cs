using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Grid
    {
        private Size _minimumSize;
        private Dictionary<(int x, int y), Tile> _grid;

        public Grid(Dungeon dungeon)
        {
            _grid = new Dictionary<(int x, int y), Tile>();
            _minimumSize = dungeon.Size;
            FillGrid(dungeon);
        }

        public Tile this[int x, int y]
        {
            get
            {
                if (!_grid.ContainsKey((x ,y)))
                {
                    _grid[(x, y)] = new Tile
                    {
                        Character = '#'
                    };
                }

                return _grid[(x, y)];
            }
            set
            {
                _grid[(x, y)] = value;
            }
        }
        
        public int GetMinimumY() => Math.Min(0, _grid.Keys.Where(coord => _grid[coord].Character != '#').Min(coord => coord.y));
        public int GetMaximumY() => Math.Max(_minimumSize.Height, _grid.Keys.Where(coord => _grid[coord].Character != '#').Max(coord => coord.y));
        public int GetMinimumX() => Math.Min(0, _grid.Keys.Where(coord => _grid[coord].Character != '#').Min(coord => coord.x));
        public int GetMaximumX() => Math.Max(_minimumSize.Width, _grid.Keys.Where(coord => _grid[coord].Character != '#').Max(coord => coord.x));

        private void FillGrid(Dungeon dungeon)
        {
            foreach (DungeonRoom room in dungeon.Rooms)
            {
                HollowOutRoomInGrid(room);
            }
        }

        private void HollowOutRoomInGrid(DungeonRoom room)
        {
            // outline spaces with walls
            foreach (Space space in room.Room.Spaces)
            {
                HollowOutSpaceInGrid(
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
                    space,
                    room.RelativePosition.X,
                    room.RelativePosition.Y,
                    ' ',
                    room);
            }
        }

        private void HollowOutSpaceInGrid(Space space, int xOffset, int yOffset, char fill, DungeonRoom parentRoom)
        {
            int yStart = yOffset + space.RelativePosition.Y;
            int xStart = xOffset + space.RelativePosition.X;

            for (int y = yStart; y < yStart + space.Size.Height; y++)
            {

                for (int x = xStart; x < xStart + space.Size.Width; x++)
                {
                    _grid[(x, y)] = new Tile
                    {
                        Character = fill,
                        ParentRoom = parentRoom
                    };
                }
            }
        }
    }
}