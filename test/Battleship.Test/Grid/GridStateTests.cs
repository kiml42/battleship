using Battleship.Grid;
using System.Collections.Generic;
using Xunit;

namespace Battleship.Test.Grid
{
    public class GridStateTests
    {
        [Theory]
        [MemberData(nameof(GetGridPositions))]
        public void CanPlaceShip(bool expected, uint x, uint y, Direction direction, uint length)
        {
            var state = new GridState(10, 10);

            var result = state.CanPlaceShip(x, y, direction, length);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetGridPositions))]
        public void TryPlaceShip(bool expected, uint x, uint y, Direction direction, uint length)
        {
            var state = new GridState(10, 10);

            var result = state.TryPlaceShip(x, y, direction, length);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetGridPositions()
        {
            //In grid, short enough
            yield return new object[] { true, 5, 5, Direction.Right, 4 };
            yield return new object[] { true, 6, 5, Direction.Right, 4 };
            yield return new object[] { true, 6, 6, Direction.Down, 4 };
            yield return new object[] { true, 3, 3, Direction.Up, 4 };
            yield return new object[] { true, 3, 3, Direction.Left, 4 };
            yield return new object[] { true, 0, 0, Direction.Right, 4 };
            yield return new object[] { true, 0, 0, Direction.Down, 4 };

            //Starts off grid
            yield return new object[] { false, 10, 5, Direction.Left, 4 };
            yield return new object[] { false, 5, 10, Direction.Left, 4 };

            //Starts in grid, but goes off
            yield return new object[] { false, 8, 8, Direction.Right, 4 };
            yield return new object[] { false, 8, 8, Direction.Down, 4 };
            yield return new object[] { false, 2, 2, Direction.Left, 4 };
            yield return new object[] { false, 2, 2, Direction.Up, 4 };
        }
    }
}
