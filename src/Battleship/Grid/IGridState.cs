using System.Collections.Generic;
using System.Drawing;

namespace Battleship.Grid
{
    public interface IGridState
    {
        int Width { get; }
        int Height { get; }

        public CoordinateState[][] CoordinateStates { get; }

        public IEnumerable<CoordinateState> FlattenedCoordinateStates { get; }

        public IEnumerable<CoordinateState> UntargetedCoordinates { get; }

        public IEnumerable<IShotResult> ShotResults { get; }
        int RemainingTargetCoordinates { get; }

        /// <summary>
        /// Lengths of all the ships that have been placed on the grid.
        /// </summary>
        IEnumerable<int> OriginalShips { get; }

        ShotResult Shoot(Point coordinate);

        ShotResult Shoot(int x, int y);
    }
}
