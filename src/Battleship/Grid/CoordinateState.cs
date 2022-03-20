using System.Collections.Generic;

namespace Battleship.Grid
{
    public struct CoordinateState
    {
        public uint X { get; private set; }

        public uint Y { get; private set; }

        public ShipLocation Ship {get; private set; }
        public IEnumerable<ShotResult> Shots {get; private set; }

        public CoordinateState(uint x, uint y, ShipLocation ship, IEnumerable<ShotResult> shots)
        {
            X = x;
            Y = y;
            Ship = ship;
            Shots = shots;
        }
    }
}
