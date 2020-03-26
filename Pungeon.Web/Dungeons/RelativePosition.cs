namespace Pungeon.Web.Dungeons
{
    public class RelativePosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public RelativePosition()
        {
        }

        public RelativePosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}