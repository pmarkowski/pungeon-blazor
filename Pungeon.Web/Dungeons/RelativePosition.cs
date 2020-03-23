namespace Pungeon.Web.Dungeons
{
    public class RelativePosition
    {
        private int _x;
        private int _y;

        public int X
        {
            get => _x;
            set
            {
                if (value < 0)
                {
                    throw new System.ArgumentOutOfRangeException(
                        nameof(value),
                        "Relative positions may not hold negative values.");
                }

                _x = value;
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                if (value < 0)
                {
                    throw new System.ArgumentOutOfRangeException(
                        nameof(value),
                        "Relative positions may not hold negative values.");
                }

                _y = value;
            }
        }

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