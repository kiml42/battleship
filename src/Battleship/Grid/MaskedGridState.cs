using System.Collections.Generic;

namespace Battleship.Grid
{
    public class MaskedGridState : IGridState
    {
        private readonly GridState _grid;

        public MaskedGridState(GridState grid)
        {
            _grid = grid;
        }


        public uint Width => _grid.Width;

        public uint Height => _grid.Height;

        public List<List<CoordinateState>> CoordinateStates => _grid.CoordinateStates;

        public List<ShotResult> ShotResults => _grid.ShotResults;

        public ShotResult Shoot(uint x, uint y)
        {
            return _grid.Shoot(x, y);
        }

        public override string ToString()
        {
            return _grid.ToString();
        }
    }
}
