using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Battleship.Test.Grid
{
    public class MaskedGridStateTests : BaseGridStateTests<MaskedGridState>
    {

        #region ToString
        [Fact]
        public void ToString_includesExpectedDetails()
        {
            var grid = CreateGrid(4, 2, out var underlyingGrid);

            Assert.NotNull(underlyingGrid.TryPlaceShip(0, 0, 3, Orientation.Horizontal));
            Assert.NotNull(underlyingGrid.TryPlaceShip(3, 0, 2, Orientation.Vertical));
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

        protected override MaskedGridState CreateGrid(uint width, uint height, out GridState underlyingGrid)
        {
            underlyingGrid = new GridState(width, height);
            var masked = new MaskedGridState(underlyingGrid);
            return masked;
        }
    }
}
