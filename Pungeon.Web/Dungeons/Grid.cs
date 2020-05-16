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
        
        public int GetMinimumY() => Math.Min(0, _grid.Keys.Where(coord => _grid[coord].Character != '#').Select(coord => (int?)coord.y).Min() ?? 0);
        public int GetMaximumY() => Math.Max(_minimumSize.Height, _grid.Keys.Where(coord => _grid[coord].Character != '#').Select(coord => (int?)coord.y).Max() ?? _minimumSize.Height);
        public int GetMinimumX() => Math.Min(0, _grid.Keys.Where(coord => _grid[coord].Character != '#').Select(coord => (int?)coord.x).Min() ?? 0);
        public int GetMaximumX() => Math.Max(_minimumSize.Width, _grid.Keys.Where(coord => _grid[coord].Character != '#').Select(coord => (int?)coord.x).Max() ?? _minimumSize.Width);

        private void FillGrid(Dungeon dungeon)
        {
            foreach (Space space in dungeon.Spaces)
            {
                HollowOutSpaceInGrid(
                    space,
                    ' ',
                    space.Id);
            }
        }

        private void HollowOutSpaceInGrid(Space space, char fill, Guid parentSpaceId)
        {
            int yStart = space.Position.Y;
            int xStart = space.Position.X;

            for (int y = yStart; y < yStart + space.Size.Height; y++)
            {

                for (int x = xStart; x < xStart + space.Size.Width; x++)
                {
                    _grid[(x, y)] = new Tile
                    {
                        Character = fill,
                        ParentSpaceId = parentSpaceId
                    };
                }
            }
        }
    }
}