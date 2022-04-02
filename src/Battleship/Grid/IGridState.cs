using System.Collections.Generic;
using System.Drawing;

namespace Battleship.Grid
{
    public interface IGridState
    {
        uint Width { get; }
        uint Height { get; }

        public List<List<CoordinateState>> CoordinateStates { get; }

        public IEnumerable<CoordinateState> FlattenedCoordinateStates { get; }

        public IEnumerable<CoordinateState> UntargetedCoordinates { get; }

        public IEnumerable<IShotResult> ShotResults { get; }
        int RemainingTargetCoordinates { get; }

        ShotResult Shoot(Point coordinate);

        ShotResult Shoot(uint x, uint y);
    }
}
