using System;
using System.Collections.Generic;

namespace Pungeon.Web.Dungeon
{
    public class Room
    {
        public List<Space> Spaces { get; set; }

        public int GetWidth()
        {
            int max = 0;

            foreach (Space space in Spaces)
            {
                max = Math.Max(max, space.RelativePosition.X + space.Size.Width);
            }

            return max;
        }

        public int GetHeight()
        {
            int max = 0;

            foreach (Space space in Spaces)
            {
                max = Math.Max(max, space.RelativePosition.Y + space.Size.Height);
            }

            return max;
        }
    }
}