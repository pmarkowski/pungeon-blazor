namespace Pungeon.Web.Dungeons
{
    public class Grid
    {
        private char[,] _grid;

        public Grid(int width, int height)
        {
            _grid = new char[height, width];
            InitializeCharGrid(_grid);
        }

        public Grid(char[,] grid)
        {
            _grid = grid;
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

        public char this[int x, int y]
        {
            get
            {
                return _grid[y, x];
            }
            set
            {
                _grid[y, x] = value;
            }
        }

        public int GetHeight()
        {
            return _grid.GetLength(0);
        }

        public int GetWidth()
        {
            return _grid.GetLength(1);
        }
    }
}