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
            char[,] map = new char[,]
            {
                { '#', '#', '#'},
                { '#', '#', '#'},
                { '#', '#', '#'}
            };

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(2, 0));

            Assert.Equal(3, path.Count);
        }

        [Fact]
        public void FindPath_DiagonalLine_FindsShortestPath()
        {
            char[,] map = new char[,]
            {
                { '#', '#', '#'},
                { '#', '#', '#'},
                { '#', '#', '#'}
            };

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(2, 2));

            Assert.Equal(5, path.Count);
        }

        [Fact]
        public void FindPath_HorizontalLineBlocked_GoesAroundBlock()
        {
            char[,] map = new char[,]
            {
                { '#', ' ', '#'},
                { '#', '#', '#'},
                { '#', '#', '#'}
            };

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(2, 0));

            Assert.Equal(5, path.Count);
        }

        [Fact]
        public void FindPath_CompletelyBlocked_ReturnsNull()
        {
            char[,] map = new char[,]
            {
                { '#', ' ', '#'},
                { '#', ' ', '#'},
                { '#', ' ', '#'}
            };

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(2, 0));

            Assert.Null(path);
        }

        [Fact]
        public void FindPath_StartSameAsEnd_ReturnsSelf()
        {
            char[,] map = new char[,]
            {
                { '#', ' ', '#'},
                { '#', ' ', '#'},
                { '#', ' ', '#'}
            };

            var path = AStarAlgorithm.FindPath(
                map,
                new RelativePosition(0, 0),
                new RelativePosition(0, 0));

            Assert.Single(path);
        }
    }
}