using System;

namespace Pungeon.Web.Dungeons
{
    public class Tile
    {
        public char Character { get; set; }
        public Guid? ParentSpaceId { get; set; }
    }
}