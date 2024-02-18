using System.Collections.Generic;
using System.Linq;

namespace Battleship.Grid
{
    public class MaskedGridState : BaseGridState
    {
        private readonly GridState _grid;

        public MaskedGridState(GridState grid, GridMaskSettings settings = default)
        {
            _grid = grid;
            _settings = settings;
        }

        public override int Width => _grid.Width;

        public override int Height => _grid.Height;

        public override IEnumerable<IShotResult> ShotResults => _grid.ShotResults.Select(s => (IShotResult)new MaskedShotResult(s, _settings));

        public override int RemainingTargetCoordinates => _grid.RemainingTargetCoordinates;

        public override IEnumerable<int> OriginalShips => _grid.OriginalShips;

        public override CoordinateState[][] CoordinateStates => _grid.CoordinateStates;

        private GridMaskSettings _settings;

        public void ApplySettings(GridMaskSettings settings)
        {
            _settings = settings;
        }

        public override ShotResult Shoot(int x, int y)
        {
            return _grid.Shoot(x, y);
        }

        public override ShipLocation ShipAt(int x, int y)
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

        public override string ToString()
        {
            return base.ToString(_settings);
        }
    }
}
