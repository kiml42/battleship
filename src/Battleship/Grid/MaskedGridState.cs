using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<IShotResult> ShotResults => _grid.ShotResults.Select(s => (IShotResult)new MaskedShotResult(s, _settings));

        private GridMaskSettings _settings;

        public void ApplySettings(GridMaskSettings settings)
        {
            _settings = settings;
        }

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
