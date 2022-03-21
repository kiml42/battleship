using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Battleship.Test.Grid
{
    public class MaskedGridStateTests : BaseGridStateTests<MaskedGridState>
    {
        protected override MaskedGridState CreateGrid(uint width, uint height, out GridState underlyingGrid)
        {
            underlyingGrid = new GridState(width, height);
            var masked = new MaskedGridState(underlyingGrid);
            return masked;
        }
    }
}
