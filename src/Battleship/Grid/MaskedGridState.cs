using System.Collections.Generic;
using System.Linq;

namespace Battleship.Grid
{
    public class MaskedGridState : BaseGridState
    {
        private readonly GridState _grid;

        public MaskedGridState(GridState grid)
        {
            _grid = grid;
        }

        public override uint Width => _grid.Width;

        public override uint Height => _grid.Height;

        public override IEnumerable<IShotResult> ShotResults => _grid.ShotResults.Select(s => (IShotResult)new MaskedShotResult(s, _settings));

        protected override bool _showUnHitShipLocations => false;

        private GridMaskSettings _settings;

        public void ApplySettings(GridMaskSettings settings)
        {
            _settings = settings;
        }

        public override ShotResult Shoot(uint x, uint y)
        {
            return _grid.Shoot(x, y);
        }

        public override string ToString()
        {
            return _grid.ToString();
        }

        public override ShipLocation ShipAt(uint x, uint y)
        {
            // TODO only return the ship if the mask allows it.
            return _grid.ShipAt(x, y);
        }
    }
}
