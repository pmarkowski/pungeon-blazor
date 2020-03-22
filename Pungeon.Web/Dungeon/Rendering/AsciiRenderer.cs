using System;
using System.Text;

namespace Pungeon.Web.Dungeon.Rendering
{
    public static class AsciiRenderer
    {
        public static string Render(Room room)
        {
            int padding = 2;
            char[,] charGrid = new char[
                room.GetHeight() + padding,
                room.GetWidth() + padding];
            InitializeCharGrid(charGrid);

            foreach (Space space in room.Spaces)
            {
                HollowOutSpaceInGrid(charGrid, space);
            }

            return GridToString(charGrid);
        }

        private static void HollowOutSpaceInGrid(char[,] charGrid, Space space)
        {
            for (int i = space.RelativePosition.Y; i < space.RelativePosition.Y + space.Size.Height; i++)
            {
                for (int j = space.RelativePosition.X; j < space.RelativePosition.X + space.Size.Width; j++)
                {
                    charGrid[i, j] = ' ';
                }
            }
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

        private static string GridToString(char[,] charGrid)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < charGrid.GetLength(0); i++)
            {
                for (int j = 0; j < charGrid.GetLength(1); j++)
                {
                    builder.Append(charGrid[i, j]);
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}