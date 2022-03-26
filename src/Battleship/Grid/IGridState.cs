using System.Collections.Generic;
using System.Drawing;

namespace Battleship.Grid
{
    public interface IGridState
    {
        uint Width { get; }
        uint Height { get; }

        public List<List<CoordinateState>> CoordinateStates { get; }

        public IEnumerable<IShotResult> ShotResults { get; }
        int RemainingTargetCoordinates { get; }

        ShotResult Shoot(Point coordinate)
        {
            // TODO test this
            return Shoot((uint)coordinate.X, (uint)coordinate.Y);
        }

        ShotResult Shoot(uint x, uint y);
    }
}
