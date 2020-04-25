using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Room
    {
        public Guid Id { get; }
        public List<Space> Spaces { get; set; }

        public Room()
        {
            Id = Guid.NewGuid();
            Spaces = new List<Space>();
        }

        public int GetWidth()
        {
            return Spaces.Any() ? Spaces.Max(space => space.RelativePosition.X + space.Size.Width) : 0;
        }

        public int GetHeight()
        {
            return Spaces.Any() ? Spaces.Max(space => space.RelativePosition.Y + space.Size.Height) : 0;
        }
    }
}