using Pungeon.Web.Dungeons;
using Xunit;

namespace Pungeon.Web.Tests.Dungeons
{
    public class SizeTests
    {
        [Fact]
        public void Constructor_PositiveWidthValue_SetsWidth()
        {
            Size size = new Size(2, 5);

            Assert.Equal(2, size.Width);
        }

        [Fact]
        public void Constructor_PositiveHeightValue_SetsHeight()
        {
            Size size = new Size(2, 5);

            Assert.Equal(5, size.Height);
        }

        [Fact]
        public void Constructor_NegativeWidthValue_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new Size(-1, 0));
        }

        [Fact]
        public void Constructor_NegativeHeightValue_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new Size(0, -1));
        }

        [Fact]
        public void SetWidth_PositiveValue_SetsWidth()
        {
            Size size = new Size();

            size.Width = 4;

            Assert.Equal(4, size.Width);
        }

        [Fact]
        public void SetWidth_NegativeValue_ThrowsException()
        {
            Size size = new Size();

            Assert.Throws<System.ArgumentOutOfRangeException>(() => size.Width = -1);
        }

        [Fact]
        public void SetHeight_PositiveValue_SetsHeight()
        {
            Size size = new Size();

            size.Height = 3;

            Assert.Equal(3, size.Height);
        }

        [Fact]
        public void SetHeight_NegativeValue_ThrowsException()
        {
            Size size = new Size();

            Assert.Throws<System.ArgumentOutOfRangeException>(() => size.Height = -1);
        }
    }
}