using System;
using System.Text;

namespace Pungeon.Web.Dungeons.Rendering
{
    public static class AsciiRenderer
    {
        private const int Padding = 1;

        public static string Render(Dungeon dungeon)
        {
            char[,] charGrid = new char[
                dungeon.GetHeight() + Padding * 2,
                dungeon.GetWidth() + Padding * 2
            ];
            InitializeCharGrid(charGrid);

            foreach (DungeonRoom room in dungeon.Rooms)
            {
                RenderRoomInGrid(charGrid, room, Padding, Padding);
            }

            return GridToString(charGrid);
        }

        private static void RenderRoomInGrid(char[,] charGrid, DungeonRoom room, int xOffset, int yOffset)
        {
            foreach (Space space in room.Room.Spaces)
            {
                HollowOutSpaceInGrid(
                    charGrid,
                    space,
                    room.RelativePosition.X + xOffset,
                    room.RelativePosition.Y + yOffset);
            }
        }

        public static string Render(Room room)
        {
            char[,] charGrid = new char[
                room.GetHeight() + Padding,
                room.GetWidth() + Padding];
            InitializeCharGrid(charGrid);

            foreach (Space space in room.Spaces)
            {
                HollowOutSpaceInGrid(charGrid, space);
            }

            return GridToString(charGrid);
        }
        
        private static void HollowOutSpaceInGrid(char[,] charGrid, Space space, int xOffset, int yOffset)
        {
            int yStart = yOffset + space.RelativePosition.Y;

            for (int i = yStart; i < yStart + space.Size.Height; i++)
            {
                int xStart = xOffset + space.RelativePosition.X;

                for (int j = xStart; j < xStart + space.Size.Width; j++)
                {
                    charGrid[i, j] = ' ';
                }
            }
        }

        private static void HollowOutSpaceInGrid(char[,] charGrid, Space space)
        {
            HollowOutSpaceInGrid(charGrid, space, 0, 0);
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