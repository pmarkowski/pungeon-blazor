using System;

namespace Pungeon.Web.Dungeons
{
    public class Space
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Position Position { get; set; }
        public Size Size { get; set; }
    }
}