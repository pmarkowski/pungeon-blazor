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
            StringBuilder builder = new StringBuilder();
            int yStart = grid.GetMinimumY();
            int height = grid.GetMaximumY();
            int xStart = grid.GetMinimumX();
            int width = grid.GetMaximumX();
            for (int y = yStart; y <= height; y++)
            {
                builder.Append("<div class='tile-row'>");
                for (int x = xStart; x <= width; x++)
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
                        case '+':
                            tileDiv = "<div class='tile connector'></div>";
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