using Battleship.Grid;
using System.Collections.Generic;
using Xunit;

namespace Battleship.Test.Grid
{
    public class GridStateTests
    {
        [Theory]
        [MemberData(nameof(GetGridPositions))]
        public void CanPlaceShip(bool expected, uint x, uint y, uint length, Orientation orientation)
        {
            var grid = new GridState(10, 10);

            var result = grid.CanPlaceShip(x, y, length, orientation);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetGridPositions))]
        public void TryPlaceShip_InEmptyGrid(bool expected, uint x, uint y, uint length, Orientation orientation)
        {
            var grid = new GridState(10, 10);

            var result = grid.TryPlaceShip(x, y, length, orientation);

            if (expected)
            {
                Assert.NotNull(result);
                var placedShip = Assert.Single(grid.Ships);
                Assert.Equal(placedShip, result);
            }
            else
            {
                Assert.Null(result);
                Assert.Empty(grid.Ships);
            }
        }

        [Theory]
        // ships don't even touch
        [InlineData(true, 5, 5, 4, Orientation.Vertical, 5, 8, 4, Orientation.Vertical)]
        [InlineData(true, 0, 0, 4, Orientation.Vertical, 6, 9, 4, Orientation.Vertical)]

        //Ships touch but don't overlap
        [InlineData(true, 5, 5, 4, Orientation.Vertical, 5, 6, 4, Orientation.Vertical)]
        [InlineData(true, 0, 5, 4, Orientation.Vertical, 4, 5, 4, Orientation.Vertical)]
        [InlineData(true, 0, 5, 4, Orientation.Vertical, 4, 5, 4, Orientation.Horizontal)]

        //ships in same direction
        [InlineData(false, 5, 5, 4, Orientation.Vertical, 5, 5, 4, Orientation.Vertical)] //right on top
        [InlineData(false, 5, 5, 4, Orientation.Vertical, 7, 5, 3, Orientation.Vertical)]
        [InlineData(false, 7, 5, 3, Orientation.Vertical, 5, 5, 4, Orientation.Vertical)]

        //crossed ships
        [InlineData(false, 5, 5, 4, Orientation.Vertical, 6, 4, 4, Orientation.Horizontal)]
        public void TryPlaceShip_WithExistingShip(
            bool canPlaceShip2,
            uint x1, uint y1, uint length1, Orientation direction1,
            uint x2, uint y2, uint length2
, Orientation direction2)
        {
            var grid = new GridState(10, 10);

            var ship1 = grid.TryPlaceShip(x1, y1, length1, direction1);
            Assert.NotNull(ship1);
            Assert.Contains(ship1, grid.Ships);

            var ship2 = grid.TryPlaceShip(x2, y2, length2, direction2);

            if (canPlaceShip2)
            {
                Assert.NotNull(ship2);
                Assert.Contains(ship1, grid.Ships);
                Assert.Contains(ship2, grid.Ships);
            }
            else
            {
                Assert.Null(ship2);
                Assert.Single(grid.Ships, s => s == ship1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Each call returns an object array meaning {shouldBeAbleToPlaceShip, y, y, direction, shipLength}</returns>
        public static IEnumerable<object[]> GetGridPositions()
        {
            //In grid, short enough
            yield return new object[] { true, 5, 5, 4, Orientation.Vertical };
            yield return new object[] { true, 6, 5, 4, Orientation.Vertical };
            yield return new object[] { true, 6, 6, 4, Orientation.Horizontal };
            yield return new object[] { true, 3, 3, 4, Orientation.Vertical };
            yield return new object[] { true, 3, 3, 4, Orientation.Horizontal };
            yield return new object[] { true, 0, 0, 4, Orientation.Vertical };
            yield return new object[] { true, 0, 0, 4, Orientation.Horizontal };

            //Starts off grid
            yield return new object[] { false, 10, 5, 4, Orientation.Horizontal };
            yield return new object[] { false, 5, 10, 4, Orientation.Horizontal };

            //Starts in grid, but goes off
            yield return new object[] { false, 8, 8, 4, Orientation.Vertical };
            yield return new object[] { false, 8, 8, 4, Orientation.Horizontal };
        }
    }
}
