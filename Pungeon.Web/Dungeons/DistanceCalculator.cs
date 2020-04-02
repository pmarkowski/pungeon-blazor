using System;

namespace Pungeon.Web.Dungeons
{
    public static class DistanceCalculator
    {
        public static int GetManhattanDistance(
            RelativePosition position1,
            RelativePosition position2) =>
                Math.Abs(position2.X - position1.X) +
                Math.Abs(position2.Y - position1.Y);
    }
}