using Pungeon.Web.Dungeons;
using Xunit;

namespace Pungeon.Web.Tests.Dungeons
{
    public class RelativePositionTests
    {
        [Fact]
        public void Constructor_PositiveXValue_SetsX()
        {
            RelativePosition relativePosition = new RelativePosition(2, 5);

            Assert.Equal(2, relativePosition.X);
        }

        [Fact]
        public void Constructor_PositiveYValue_SetsX()
        {
            RelativePosition relativePosition = new RelativePosition(2, 5);

            Assert.Equal(5, relativePosition.Y);
        }

        [Fact]
        public void Constructor_NegativeXValue_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new RelativePosition(-1, 0));
        }

        [Fact]
        public void Constructor_NegativeYValue_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new RelativePosition(0, -1));
        }

        [Fact]
        public void SetX_PositiveValue_SetsX()
        {
            RelativePosition position = new RelativePosition();

            position.X = 4;

            Assert.Equal(4, position.X);
        }

        [Fact]
        public void SetX_NegativeValue_ThrowsException()
        {
            RelativePosition position = new RelativePosition();

            Assert.Throws<System.ArgumentOutOfRangeException>(() => position.X = -1);
        }

        [Fact]
        public void SetY_PositiveValue_SetsY()
        {
            RelativePosition position = new RelativePosition();

            position.Y = 3;

            Assert.Equal(3, position.Y);
        }

        [Fact]
        public void SetY_NegativeValue_ThrowsException()
        {
            RelativePosition position = new RelativePosition();

            Assert.Throws<System.ArgumentOutOfRangeException>(() => position.Y = -1);
        }
    }
}