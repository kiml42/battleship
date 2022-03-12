using Battleship.Grid;
using Xunit;

namespace Battleship.Test.Grid
{
    public class GridStateTests
    {
        [Theory]
        //In grid, short enough
        [InlineData(true, 5, 5, Direction.Right, 4)]
        [InlineData(true, 6, 5, Direction.Right, 4)]
        [InlineData(true, 6, 6, Direction.Down, 4)]
        [InlineData(true, 3, 3, Direction.Up, 4)]
        [InlineData(true, 3, 3, Direction.Left, 4)]

        //Starts off grid
        [InlineData(false, 10, 5, Direction.Left, 4)]
        [InlineData(false, 5, 10, Direction.Left, 4)]

        //Starts in grid, but goes off
        [InlineData(false, 8, 8, Direction.Right, 4)]
        [InlineData(false, 8, 8, Direction.Down, 4)]
        [InlineData(false, 2, 2, Direction.Left, 4)]
        [InlineData(false, 2, 2, Direction.Up, 4)]
        public void CanPlaceShip(bool expected, uint x, uint y, Direction direction, uint length)
        {
            var state = new GridState(10, 10);

            var result = state.CanPlaceShip(x, y, direction, length);

            Assert.Equal(expected, result);
        }
    }
}
