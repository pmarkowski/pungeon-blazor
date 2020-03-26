using System.Collections.Generic;
using Pungeon.Web.Dungeons;

namespace Pungeon.Web.Tests.Dungeons.AStar
{
    public static class GridFactory
    {
        public static Grid GridFromCharArray(char[,] charGrid)
        {
            Grid grid = new Grid();
            for (int y = 0; y < charGrid.GetLength(0); y++)
            {
                for (int x = 0; x < charGrid.GetLength(1); x++)
                {
                    grid[x, y] = charGrid[y, x];
                }
            }
            return grid;
        }
    }
}