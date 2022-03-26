using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Battleship.Test.Grid
{
    public abstract class BaseGridStateTests<TGrid> where TGrid : IGridState
    {
        #region Shoot
        [Fact]
        public void Shoot_ReturnsMiss()
        {
            var grid = CreateGrid(10, 10, out var underlyingGrid);

            var resut = grid.Shoot(0,2);

            Assert.Equal(0, (int)resut.X);
            Assert.Equal(2, (int)resut.Y);
            Assert.False(resut.IsHit);
            Assert.Equal(0, resut.Index);

            Assert.Single(grid.ShotResults);
        }

        [Fact]
        public void Shoot_ReturnsHit()
        {
            var grid = CreateGrid(10, 10, out var underlyingGrid);

            underlyingGrid.TryPlaceShip(0, 0, 3, Orientation.Vertical);

            var resut = grid.Shoot(0,2);

            Assert.Equal(0, (int)resut.X);
            Assert.Equal(2, (int)resut.Y);
            Assert.True(resut.IsHit);
            Assert.False(resut.IsSink);
            Assert.Equal(0, resut.Index);

            Assert.Single(grid.ShotResults);
        }

        [Fact]
        public void Shoot_ReturnsDuplicateHit()
        {
            var grid = CreateGrid(10, 10, out var underlyingGrid);

            underlyingGrid.TryPlaceShip(0, 0, 3, Orientation.Vertical);

            grid.Shoot(0,2);
            var resut = grid.Shoot(0,2);

            Assert.Equal(0, (int)resut.X);
            Assert.Equal(2, (int)resut.Y);
            Assert.True(resut.IsHit);
            Assert.False(resut.IsSink);
            Assert.Equal(1, resut.Index);

            Assert.Equal(2, grid.ShotResults.Count());
        }

        [Fact]
        public void Shoot_ReturnsSink()
        {
            var grid = CreateGrid(10, 10, out var underlyingGrid);

            underlyingGrid.TryPlaceShip(0, 0, 3, Orientation.Vertical);

            grid.Shoot(0,0);
            grid.Shoot(0,1);
            var resut = grid.Shoot(0,2);

            Assert.Equal(0, (int)resut.X);
            Assert.Equal(2, (int)resut.Y);
            Assert.True(resut.IsHit);
            Assert.True(resut.IsSink);
            Assert.Equal(2, resut.Index);

            Assert.Equal(3, grid.ShotResults.Count());
        }

        [Fact]
        public void Shoot_ReturnsSinkInTheMiddleOfAShip()
        {
            var grid = CreateGrid(10, 10, out var underlyingGrid);

            underlyingGrid.TryPlaceShip(0, 0, 3, Orientation.Vertical);

            grid.Shoot(0,0);
            grid.Shoot(0,2);
            var resut = grid.Shoot(0,1);

            Assert.Equal(0, (int)resut.X);
            Assert.Equal(1, (int)resut.Y);
            Assert.True(resut.IsHit);
            Assert.True(resut.IsSink);
            Assert.Equal(2, resut.Index);

            Assert.Equal(3, grid.ShotResults.Count());
        }
        #endregion

        #region CoordinateStates
        [Theory]
        [InlineData(1,6)]
        [InlineData(6,1)]
        [InlineData(10,10)]
        [InlineData(2,10)]
        [InlineData(10,2)]
        public virtual void CoordinateStates_ReturnsExpectedCount(uint width, uint height)
        {
            var grid = CreateGrid(width, height, out var underlyingGrid);

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
        protected TGrid CreateGridForToStringTests()
        {
            var grid = CreateGrid(4, 2, out var underlyingGrid);

            Assert.NotNull(underlyingGrid.TryPlaceShip(0, 0, 3, Orientation.Horizontal));
            Assert.NotNull(underlyingGrid.TryPlaceShip(3, 0, 2, Orientation.Vertical));
            grid.Shoot(0, 1);
            grid.Shoot(3, 0);
            grid.Shoot(3, 1);

            return grid;
        }

        protected void StandardToStringAssertions(string result, string expectedUnHit3, string expectedHit2, string expectedSink2)
        {
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
            Assert.Equal(expectedUnHit3, topRowCells[1]);
            Assert.Equal(expectedUnHit3, topRowCells[2]);
            Assert.Equal(expectedUnHit3, topRowCells[3]);
            Assert.Equal(expectedHit2, topRowCells[4]);    // "X" for hit
            Assert.Equal("", topRowCells[5]);

            var bottomRowCells = rows[3].Split("|");
            Assert.Equal(6, bottomRowCells.Length);
            Assert.Equal("", bottomRowCells[0]);
            Assert.Equal("O  ", bottomRowCells[1]); //"O" for miss
            Assert.Equal("   ", bottomRowCells[2]);
            Assert.Equal("   ", bottomRowCells[3]);
            Assert.Equal(expectedSink2, bottomRowCells[4]); //"S" for sink
            Assert.Equal("", bottomRowCells[5]);
        }
        #endregion

        #region Remaining Ships
        [Fact]
        public void RemainingTargetCoordinates_ReturnsAllWithNoShots()
        {
            var grid = CreateGrid(3, 3, out var underlyingGrid);
            underlyingGrid.TryPlaceShip(0, 0, 3, Orientation.Horizontal);
            underlyingGrid.TryPlaceShip(0, 1, 2, Orientation.Vertical);

            var result = grid.RemainingTargetCoordinates;

            Assert.Equal(5, result);
        }

        [Fact]
        public void RemainingTargetCoordinates_ReturnsFewereWithHits()
        {
            var grid = CreateGrid(3, 3, out var underlyingGrid);
            underlyingGrid.TryPlaceShip(0, 0, 3, Orientation.Horizontal);
            underlyingGrid.TryPlaceShip(0, 1, 2, Orientation.Vertical);
            grid.Shoot(0, 0);
            grid.Shoot(0, 1);
            grid.Shoot(0, 2);

            var result = grid.RemainingTargetCoordinates;

            Assert.Equal(2, result);
        }
        #endregion
        protected abstract TGrid CreateGrid(uint width, uint height, out GridState underlyingGrid);

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
