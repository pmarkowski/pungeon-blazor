using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pungeon.Web.Dungeons.AStar;

namespace Pungeon.Web.Dungeons.Rendering
{
    public static class AsciiRenderer
    {
        public static string RenderToHtml(Grid grid)
        {
            return GridToHtml(grid);
        }

        private static string GridToHtml(Grid grid)
        {
            StringBuilder builder = new StringBuilder();

            for (int y = 0; y < grid.GetHeight(); y++)
            {
                builder.Append("<div class='tile-row'>");
                for (int x = 0; x < grid.GetWidth(); x++)
                {
                    char tile = grid[x, y];
                    string tileDiv;
                    switch (tile)
                    {
                        case ' ':
                            tileDiv = "<div class='tile empty'></div>";
                            builder.Append(tileDiv);
                            break;
                        case '#':
                            tileDiv = "<div class='tile full'></div>";
                            builder.Append(tileDiv);
                            break;
                        default:
                            break;
                    }
                }
                builder.Append("</div>");
            }

            return builder.ToString();
        }
    }
}