using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Battleship.Test.Grid
{
    public class GridStateTests : BaseGridStateTests<GridState>
    {
        #region CanPlaceShip
        [Theory]
        [MemberData(nameof(GetGridPositions))]
        public void CanPlaceShip(bool expected, uint x, uint y, uint length, Orientation orientation)
        {
            var grid = CreateGrid(10, 10, out _);

            var result = grid.CanPlaceShip(x, y, length, orientation);

            Assert.Equal(expected, result);
        }
        # endregion CanPlaceShip

        #region TryPlaceShip
        [Theory]
        [MemberData(nameof(GetGridPositions))]
        public void TryPlaceShip_InEmptyGrid(bool expected, uint x, uint y, uint length, Orientation orientation)
        {
            var grid = CreateGrid(10, 10, out _);

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
        [InlineData(true, 5, 5, 4, Orientation.Horizontal, 5, 8, 4, Orientation.Horizontal)]
        [InlineData(true, 0, 0, 4, Orientation.Horizontal, 6, 9, 4, Orientation.Horizontal)]

        //Ships touch but don't overlap
        [InlineData(true, 5, 5, 4, Orientation.Horizontal, 5, 6, 4, Orientation.Horizontal)]
        [InlineData(true, 0, 5, 4, Orientation.Horizontal, 4, 5, 4, Orientation.Horizontal)]
        [InlineData(true, 0, 5, 4, Orientation.Horizontal, 4, 5, 4, Orientation.Vertical)]

        //ships in same direction
        [InlineData(false, 5, 5, 4, Orientation.Horizontal, 5, 5, 4, Orientation.Horizontal)] //right on top
        [InlineData(false, 5, 5, 4, Orientation.Horizontal, 7, 5, 3, Orientation.Horizontal)]
        [InlineData(false, 7, 5, 3, Orientation.Horizontal, 5, 5, 4, Orientation.Horizontal)]

        //crossed ships
        [InlineData(false, 5, 5, 4, Orientation.Horizontal, 6, 4, 4, Orientation.Vertical)]
        public void TryPlaceShip_WithExistingShip(
            bool canPlaceShip2,
            uint x1, uint y1, uint length1, Orientation direction1,
            uint x2, uint y2, uint length2, Orientation direction2)
        {
            var grid = CreateGrid(10, 10, out _);

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
        #endregion TryPlaceShip

        #region ShipAt
        [Fact]
        public void ShipAt_returnsNullForEmptyGrid()
        {
            var grid = CreateGrid(10, 10, out _);

            var result = grid.ShipAt(0, 0);

            Assert.Null(result);
        }

        [Fact]
        public void ShipAt_returnsNullForEmptyCell()
        {
            var grid = CreateGrid(10, 10, out _);

            var ship1 = grid.TryPlaceShip(0, 0, 0, Orientation.Horizontal);
            Assert.NotNull(ship1);
            Assert.Contains(ship1, grid.Ships);

            var result = grid.ShipAt(2, 2);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ShipAt_returnsTheShipAtTheLocation(uint x)
        {
            var grid = CreateGrid(10, 10, out _);

            var ship1 = grid.TryPlaceShip(0, 0, 4, Orientation.Horizontal);
            Assert.NotNull(ship1);
            Assert.Contains(ship1, grid.Ships);

            var result = grid.ShipAt(x, 0);

            Assert.Equal(ship1, result);
        }
        #endregion ShipAt

        #region CoordinateStates
        [Theory]
        [InlineData(1,6)]
        [InlineData(6,1)]
        [InlineData(10,10)]
        [InlineData(2,10)]
        [InlineData(10,2)]
        public override void CoordinateStates_ReturnsExpectedCount(uint width, uint height)
        {
            var grid = CreateGrid(width, height, out _);

            var result = grid.CoordinateStates;

            Assert.Equal((int)height, result.Count);
            Assert.All(result, row =>
            {
                Assert.Equal((int)width, row.Count);
            });
            Assert.Equal((int)width * (int)height, result.Sum(row => row.Count));
        }
        #endregion

        #region ToString
        [Fact]
        public void ToString_includesExpectedDetails()
        {
            var grid = CreateGridForToStringTests();

            var result = grid.ToString();

            StandardToStringAssertions(result, " 3 ", "X2 ", "X2S");
        }
        #endregion

        protected override GridState CreateGrid(uint width, uint height, out GridState underlyingGrid)
        {
            underlyingGrid = new GridState(width, height);
            return underlyingGrid;
        }
    }
}
