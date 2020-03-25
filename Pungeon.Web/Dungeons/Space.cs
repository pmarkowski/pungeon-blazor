using System.Collections.Generic;

namespace Pungeon.Web.Dungeons
{
    public class Space
    {
        public RelativePosition RelativePosition { get; set; }
        public Size Size { get; set; }
        public List<Connector> Connectors { get; set; }

        public Space()
        {
            Connectors = new List<Connector>();
        }
    }
}