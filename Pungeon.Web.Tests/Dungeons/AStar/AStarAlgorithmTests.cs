using Pungeon.Web.Dungeons;
using Pungeon.Web.Dungeons.AStar;
using Xunit;

namespace Pungeon.Web.Tests.Dungeons.AStar
{
    public class AStarAlgorithmTests
    {
        [Fact]
        public void FindPath_HorizontalLine_FindsShortestPath()
        {
            Grid map = new Grid(new char[,]
            {
                { '#', '#', '#'},
                { '#', '#', '#'},
                { '#', '#', '#'}
            });

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(2, 0));

            Assert.Equal(3, path.Count);
        }

        [Fact]
        public void FindPath_DiagonalLine_FindsShortestPath()
        {
            Grid map = new Grid(new char[,]
            {
                { '#', '#', '#'},
                { '#', '#', '#'},
                { '#', '#', '#'}
            });

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(2, 2));

            Assert.Equal(5, path.Count);
        }

        [Fact]
        public void FindPath_HorizontalLineBlocked_GoesAroundBlock()
        {
            Grid map = new Grid(new char[,]
            {
                { '#', ' ', '#'},
                { '#', '#', '#'},
                { '#', '#', '#'}
            });

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(2, 0));

            Assert.Equal(5, path.Count);
        }

        [Fact]
        public void FindPath_InitiallyBlocked_ReturnsPath()
        {
            Grid map = new Grid(new char[,]
            {
                { '#', ' ', '#'},
                { '#', ' ', '#'},
                { '#', ' ', '#'}
            });

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(2, 0));

            Assert.Equal(5, path.Count);
        }

        [Fact]
        public void FindPath_StartSameAsEnd_ReturnsSelf()
        {
            Grid map = new Grid(new char[,]
            {
                { '#', ' ', '#'},
                { '#', ' ', '#'},
                { '#', ' ', '#'}
            });

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(0, 0));

            Assert.Single(path);
        }

        [Fact]
        public void FindPath_LargeRooms_DoesNotPassSpaces()
        {
            Grid map = new Grid(new char[,]
            {
                { ' ', ' ', ' ', '#', '#' },
                { ' ', ' ', ' ', '#', '#' },
                { ' ', ' ', ' ', '#', '#' },
                { '#', '#', '#', '#', '#' },
                { '#', '#', '#', '#', '#' },
                { '#', '#', ' ', ' ', ' ' },
                { '#', '#', ' ', ' ', ' ' },
                { '#', '#', ' ', ' ', ' ' },
            });

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(3, 1),
                new RelativePosition(1, 6)
            );

            Assert.Equal(8, path.Count);
        }
    }
}