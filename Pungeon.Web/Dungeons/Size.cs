namespace Pungeon.Web.Dungeons
{
    public class Size
    {
        private int _width;
        private int _height;

        public int Width
        {
            get => _width;
            set
            {
                if (value < 0)
                {
                    throw new System.ArgumentOutOfRangeException(
                        nameof(value),
                        "Size dimensions may not hold negative values.");
                }

                _width = value;
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value < 0)
                {
                    throw new System.ArgumentOutOfRangeException(
                        nameof(value),
                        "Size dimensions may not hold negative values.");
                }

                _height = value;
            }
        }

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