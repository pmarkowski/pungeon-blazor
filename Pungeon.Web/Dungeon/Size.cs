namespace Pungeon.Web.Dungeon
{
    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Size()
        {
        }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}