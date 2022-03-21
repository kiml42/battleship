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
            var grid = new GridState(10, 10);

            var result = grid.CanPlaceShip(x, y, length, orientation);

            Assert.Equal(expected, result);
        }
        # endregion CanPlaceShip

        #region TryPlaceShip
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
        #endregion TryPlaceShip

        #region ShipAt
        [Fact]
        public void ShipAt_returnsNullForEmptyGrid()
        {
            var grid = new GridState(10, 10);

            var result = grid.ShipAt(0, 0);

            Assert.Null(result);
        }

        [Fact]
        public void ShipAt_returnsNullForEmptyCell()
        {
            var grid = new GridState(10, 10);

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
            var grid = new GridState(10, 10);

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
        public void CoordinateStates_ReturnsExpectedCount(uint width, uint height)
        {
            var grid = new GridState(width, height);

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
            var grid = new GridState(4, 2);

            Assert.NotNull(grid.TryPlaceShip(0, 0, 3, Orientation.Horizontal));
            Assert.NotNull(grid.TryPlaceShip(3, 0, 2, Orientation.Vertical));
            grid.Shoot(0, 1);
            grid.Shoot(3, 0);
            grid.Shoot(3, 1);

            var result = grid.ToString();
            Assert.NotEmpty(result);
            Assert.EndsWith(Environment.NewLine, result);

            result = result.TrimEnd();

            var rows = result.Split(Environment.NewLine);
            Assert.Equal(5, rows.Length);

            const int expectedRowLength = 17;
            // even numbered rows are just "-"s as grid lines.
            var expectedHorizontalDividerRow = new string('-', expectedRowLength);
            Assert.Equal(expectedHorizontalDividerRow, rows[0]);
            Assert.Equal(expectedHorizontalDividerRow, rows[2]);
            Assert.Equal(expectedHorizontalDividerRow, rows[4]);

            // all rows are of expected length
            Assert.All(rows, row =>
            {
                Assert.Equal(expectedRowLength, row.Length);
            });

            var topRowCells = rows[1].Split("|");
            Assert.Equal(6, topRowCells.Length);
            Assert.Equal("", topRowCells[0]);   // empty strings at the end because of the "|"s at both ends of the string.
            Assert.Equal(" 3 ", topRowCells[1]);
            Assert.Equal(" 3 ", topRowCells[2]);
            Assert.Equal(" 3 ", topRowCells[3]);
            Assert.Equal("X2 ", topRowCells[4]);    // "X" for hit
            Assert.Equal("", topRowCells[5]);

            var bottomRowCells = rows[3].Split("|");
            Assert.Equal(6, bottomRowCells.Length);
            Assert.Equal("", bottomRowCells[0]);
            Assert.Equal("O  ", bottomRowCells[1]); //"O" for miss
            Assert.Equal("   ", bottomRowCells[2]);
            Assert.Equal("   ", bottomRowCells[3]);
            Assert.Equal("X2S", bottomRowCells[4]); //"S" for sink
            Assert.Equal("", bottomRowCells[5]);
        }
        #endregion

        protected override GridState CreateGrid(uint width, uint height, out GridState underlyingGrid)
        {
            underlyingGrid = new GridState(width, height);
            return underlyingGrid;
        }
    }
}
