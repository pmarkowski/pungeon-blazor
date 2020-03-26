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

        public Grid(char[,] grid)
        {
            _grid = new Dictionary<(int x, int y), char>();
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    _grid[(x, y)] = grid[y, x];
                }
            }
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

        public int GetMinimumY() => _grid.Keys.Min(coord => coord.y);
        public int GetMaximumY() => _grid.Keys.Max(coord => coord.y);
        public int GetMinimumX() => _grid.Keys.Min(coord => coord.x);
        public int GetMaximumX() => _grid.Keys.Max(coord => coord.x);
    }
}