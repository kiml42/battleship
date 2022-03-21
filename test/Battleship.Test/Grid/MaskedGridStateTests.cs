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
        public void ToString_includesExpectedDetails_NoSettingsIsAllOff()
        {
            var grid = CreateGridForToStringTests();

            var settings = new GridMaskSettings(false, false);

            grid.ApplySettings(settings);

            var result = grid.ToString();

            StandardToStringAssertions(result, "   ", "X  ", "X  ");
        }

        [Fact]
        public void ToString_includesExpectedDetails_AllOff()
        {
            var grid = CreateGridForToStringTests();

            var settings = new GridMaskSettings(false, false);

            grid.ApplySettings(settings);

            var result = grid.ToString();

            StandardToStringAssertions(result, "   ", "X  ", "X  ");
        }

        [Fact]
        public void ToString_includesExpectedDetails_JustSinks()
        {
            var grid = CreateGridForToStringTests();

            var settings = new GridMaskSettings(true, false);

            grid.ApplySettings(settings);

            var result = grid.ToString();

            StandardToStringAssertions(result, "   ", "X  ", "X S");
        }

        [Fact]
        public void ToString_includesExpectedDetails_JustLengths()
        {
            var grid = CreateGridForToStringTests();

            var settings = new GridMaskSettings(false, true);

            grid.ApplySettings(settings);

            var result = grid.ToString();

            StandardToStringAssertions(result, "   ", "X2 ", "X2 ");
        }

        [Fact]
        public void ToString_includesExpectedDetails_BothOn()
        {
            var grid = CreateGridForToStringTests();

            var settings = new GridMaskSettings(true, true);

            grid.ApplySettings(settings);

            var result = grid.ToString();

            StandardToStringAssertions(result, "   ", "X2 ", "X2S");
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
