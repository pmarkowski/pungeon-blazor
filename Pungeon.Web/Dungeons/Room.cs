using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Room
    {
        public List<Space> Spaces { get; set; }

        public int GetWidth()
        {
            return Spaces.Max(space => space.RelativePosition.X + space.Size.Width);
        }

        public int GetHeight()
        {
            return Spaces.Max(space => space.RelativePosition.Y + space.Size.Height);
        }
    }
}