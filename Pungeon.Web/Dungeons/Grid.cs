using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Grid
    {
        private Size _minimumSize;
        private Dictionary<(int x, int y), Tile> _grid;

        public Grid()
        {
            _grid = new Dictionary<(int x, int y), Tile>();
            _minimumSize = new Size(0, 0);
        }

        public Grid(Size minimumSize)
        {
            _grid = new Dictionary<(int x, int y), Tile>();
            _minimumSize = minimumSize;
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
    }
}