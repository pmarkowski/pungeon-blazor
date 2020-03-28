using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Grid
    {
        private Dictionary<(int x, int y), char> _grid;

        public Grid()
        {
            _grid = new Dictionary<(int x, int y), char>();
        }

        public char this[int x, int y]
        {
            get
            {
                if (!_grid.ContainsKey((x ,y)))
                {
                    _grid[(x, y)] = '#';
                }

                return _grid[(x, y)];
            }
            set
            {
                _grid[(x, y)] = value;
            }
        }

        public int GetMinimumY() => _grid.Keys.Where(coord => _grid[coord] != '#').Min(coord => coord.y);
        public int GetMaximumY() => _grid.Keys.Where(coord => _grid[coord] != '#').Max(coord => coord.y);
        public int GetMinimumX() => _grid.Keys.Where(coord => _grid[coord] != '#').Min(coord => coord.x);
        public int GetMaximumX() => _grid.Keys.Where(coord => _grid[coord] != '#').Max(coord => coord.x);
    }
}