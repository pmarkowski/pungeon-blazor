using System;
using Microsoft.AspNetCore.Components;

namespace Pungeon.Web.ViewUtilities
{
    public static class LineDivCreator
    {
        public static RenderFragment CreateLine(int x1, int y1, int x2, int y2)
        {
            var a = x1 - x2;
            var b = y1 - y2;
            var c = Math.Sqrt(a * a + b * b);

            var sx = (x1 + x2) / 2;
            var sy = (y1 + y2) / 2;

            var x = sx - c / 2;
            var y = sy;

            var alpha = Math.PI - Math.Atan2(-b, a);

            return CreateLineElement(x, y, c, alpha);
        }

        private static RenderFragment CreateLineElement(double x, int y, double length, double angle)
        {
            string styles = "width: " + (length + 4) + "px; "
               + "-moz-transform: rotate(" + angle + "rad); "
               + "-webkit-transform: rotate(" + angle + "rad); "
               + "-o-transform: rotate(" + angle + "rad); "  
               + "-ms-transform: rotate(" + angle + "rad); "  
               + "position: absolute; "
               + "top: " + y + "px; "
               + "left: " + (x-2) + "px; ";

            RenderFragment lineElement;
            lineElement = renderBuilder =>
            {
                renderBuilder.OpenElement(1, "div");
                renderBuilder.AddAttribute(1, "class", "wall-segment");
                renderBuilder.AddAttribute(2, "style", styles);
                renderBuilder.CloseElement();
            };
            return lineElement;
        }
    }
}