using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Dungeon
    {
        public Size Size { get; set; }
        public List<Space> Spaces { get; set; }
        public List<WallSegment> Walls { get; set; }

        public Dungeon()
        {
            Size = new Size(0, 0);
            Spaces = new List<Space>();
            Walls = new List<WallSegment>();
        }

        public void RemoveSpace(Guid spaceId)
        {
            int removalIndex = Spaces.FindIndex(space => space.Id == spaceId);
            Spaces.RemoveAt(removalIndex);
        }

        public void SetSpacePosition(Guid spaceId, Position newPosition)
        {
            Spaces.Single(space => space.Id == spaceId).Position = newPosition;
        }

        public Space GetSpace(Guid spaceId)
        {
            return Spaces.Single(space => space.Id == spaceId);
        }
    }
}