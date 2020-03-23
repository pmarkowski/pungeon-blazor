using Pungeon.Web.Dungeons;
using Xunit;

namespace Pungeon.Web.Tests.Dungeons
{
    public class RoomTests
    {
        [Fact]
        public void GetHeight_NoSpaces_ReturnsZero()
        {
            Room room = new Room();

            int height = room.GetHeight();

            Assert.Equal(0, height);
        }
    }
}