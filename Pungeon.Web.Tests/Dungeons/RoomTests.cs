using System.Collections.Generic;
using Pungeon.Web.Dungeons;
using Xunit;

namespace Pungeon.Web.Tests.Dungeons
{
    public class RoomTests
    {
        [Fact]
        public void GetHeight_OverlappingSpaces_ReturnsLargestHeight()
        {
            Position zeroPosition = new Position(0, 0);
            Room room = new Room()
            {
                Spaces = new List<Space>
                {
                    new Space
                    {
                        RelativePosition = zeroPosition,
                        Size = new Size(2, 3)
                    },
                    new Space
                    {
                        RelativePosition = zeroPosition,
                        Size = new Size(3, 4)
                    },
                    new Space
                    {
                        RelativePosition = zeroPosition,
                        Size = new Size(4, 5)
                    }
                }
            };

            int height = room.GetHeight();

            Assert.Equal(5, height);
        }

        [Fact]
        public void GetHeight_OffsetSpaces_ReturnsLargestSumOfYAndHeight()
        {
            Room room = new Room()
            {
                Spaces = new List<Space>
                {
                    new Space
                    {
                        RelativePosition = new Position(8, 9),
                        Size = new Size(2, 3)
                    },
                    new Space
                    {
                        RelativePosition = new Position(4, 5),
                        Size = new Size(3, 4)
                    },
                    new Space
                    {
                        RelativePosition = new Position(0, 0),
                        Size = new Size(4, 5)
                    }
                }
            };

            int height = room.GetHeight();

            Assert.Equal(9 + 3, height);
        }

        [Fact]
        public void GetHeight_NoSpaces_ReturnsZero()
        {
            Room room = new Room();

            int height = room.GetHeight();

            Assert.Equal(0, height);
        }

        [Fact]
        public void GetWidth_OverlappingSpaces_ReturnsLargestWidth()
        {
            Position zeroPosition = new Position(0, 0);
            Room room = new Room()
            {
                Spaces = new List<Space>
                {
                    new Space
                    {
                        RelativePosition = zeroPosition,
                        Size = new Size(2, 3)
                    },
                    new Space
                    {
                        RelativePosition = zeroPosition,
                        Size = new Size(3, 4)
                    },
                    new Space
                    {
                        RelativePosition = zeroPosition,
                        Size = new Size(4, 5)
                    }
                }
            };

            int width = room.GetWidth();

            Assert.Equal(4, width);
        }

        [Fact]
        public void GetWidth_OffsetSpaces_ReturnsLargestSumOfXAndWidth()
        {
            Room room = new Room()
            {
                Spaces = new List<Space>
                {
                    new Space
                    {
                        RelativePosition = new Position(8, 9),
                        Size = new Size(2, 3)
                    },
                    new Space
                    {
                        RelativePosition = new Position(4, 5),
                        Size = new Size(3, 4)
                    },
                    new Space
                    {
                        RelativePosition = new Position(0, 0),
                        Size = new Size(4, 5)
                    }
                }
            };

            int width = room.GetWidth();

            Assert.Equal(8 + 2, width);
        }
        
        [Fact]
        public void GetWidth_NoSpaces_ReturnsZero()
        {
            Room room = new Room();

            int width = room.GetWidth();

            Assert.Equal(0, width);
        }
    }
}