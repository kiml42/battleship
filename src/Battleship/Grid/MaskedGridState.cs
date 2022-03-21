using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Grid
{
    public class MaskedGridState : IGridState
    {
        private GridState grid;

        public MaskedGridState(GridState grid)
        {
            this.grid = grid;
        }
    }
}
