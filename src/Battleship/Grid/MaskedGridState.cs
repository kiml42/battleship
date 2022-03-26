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

        private GridMaskSettings _settings;

        public void ApplySettings(GridMaskSettings settings)
        {
            _settings = settings;
        }

        public override ShotResult Shoot(uint x, uint y)
        {
            return _grid.Shoot(x, y);
        }

        public override ShipLocation ShipAt(uint x, uint y)
        {
            if (_settings.ShowLengthsOfHitShips)
            {
                if (ShotResults.Any(s => s.X == x && s.Y == y && s.IsHit))
                {
                    return _grid.ShipAt(x, y);
                }
            }
            return null;
        }
    }
}
