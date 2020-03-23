using System.Collections.Generic;
using Pungeon.Web.Dungeons;
using Xunit;

namespace Pungeon.Web.Tests.Dungeons
{
    public class DungeonTests
    {
        [Fact]
        public void GetHeight_OverlappingRooms_ReturnsLargestRoom()
        {
            RelativePosition zeroPosition = new RelativePosition(0, 0);
            Dungeon dungeon = new Dungeon()
            {
                Rooms = new List<DungeonRoom>()
                {
                    new DungeonRoom()
                    {
                        RelativePosition = zeroPosition,
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = zeroPosition,
                                    Size = new Size(2, 3)
                                }
                            }
                        }
                    },
                    new DungeonRoom()
                    {
                        RelativePosition = zeroPosition,
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = zeroPosition,
                                    Size = new Size(5, 10)
                                }
                            }
                        }
                    }
                }
            };

            int height = dungeon.GetHeight();

            Assert.Equal(10, height);
        }

        [Fact]
        public void GetHeight_OffsetRooms_ReturnsLargestSumOfYAndRoomHeight()
        {
            RelativePosition zeroPosition = new RelativePosition(0, 0);
            Dungeon dungeon = new Dungeon()
            {
                Rooms = new List<DungeonRoom>()
                {
                    new DungeonRoom()
                    {
                        RelativePosition = new RelativePosition(0, 10),
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = zeroPosition,
                                    Size = new Size(2, 3)
                                }
                            }
                        }
                    },
                    new DungeonRoom()
                    {
                        RelativePosition = zeroPosition,
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = zeroPosition,
                                    Size = new Size(5, 10)
                                }
                            }
                        }
                    }
                }
            };

            int height = dungeon.GetHeight();

            Assert.Equal(13, height);
        }

        [Fact]
        public void GetHeight_NoRooms_ReturnsZero()
        {
            Dungeon dungeon = new Dungeon();

            int height = dungeon.GetHeight();

            Assert.Equal(0, height);
        }

        [Fact]
        public void GetWidth_OverlappingRooms_ReturnsLargestRoom()
        {
            RelativePosition zeroPosition = new RelativePosition(0, 0);
            Dungeon dungeon = new Dungeon()
            {
                Rooms = new List<DungeonRoom>()
                {
                    new DungeonRoom()
                    {
                        RelativePosition = zeroPosition,
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = zeroPosition,
                                    Size = new Size(20, 3)
                                }
                            }
                        }
                    },
                    new DungeonRoom()
                    {
                        RelativePosition = zeroPosition,
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = zeroPosition,
                                    Size = new Size(5, 10)
                                }
                            }
                        }
                    }
                }
            };

            int width = dungeon.GetWidth();

            Assert.Equal(20, width);
        }

        [Fact]
        public void GetWidth_OffsetRooms_ReturnsLargestSumOfXAndRoomWidth()
        {
            RelativePosition zeroPosition = new RelativePosition(0, 0);
            Dungeon dungeon = new Dungeon()
            {
                Rooms = new List<DungeonRoom>()
                {
                    new DungeonRoom()
                    {
                        RelativePosition = zeroPosition,
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = zeroPosition,
                                    Size = new Size(20, 3)
                                }
                            }
                        }
                    },
                    new DungeonRoom()
                    {
                        RelativePosition = zeroPosition,
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = new RelativePosition(23, 0),
                                    Size = new Size(5, 10)
                                }
                            }
                        }
                    }
                }
            };

            int width = dungeon.GetWidth();

            Assert.Equal(28, width);
        }
        
        [Fact]
        public void GetWidth_NoRooms_ReturnsZero()
        {
            Dungeon dungeon = new Dungeon();

            int width = dungeon.GetWidth();

            Assert.Equal(0, width);
        }
    }
}