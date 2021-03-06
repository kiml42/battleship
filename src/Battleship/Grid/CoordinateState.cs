using System.Collections.Generic;
using System.Linq;

namespace Battleship.Grid
{
    public struct CoordinateState
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public ShipLocation Ship {get; private set; }

        public IEnumerable<IShotResult> Shots {get; private set; }

        /// <summary>
        /// The result of the first shot.
        /// </summary>
        public IShotResult Shot {get; private set; }

        public CoordinateState(int x, int y, ShipLocation ship, IEnumerable<IShotResult> shots)
        {
            X = x;
            Y = y;
            Ship = ship;
            Shots = shots;
            Shot = shots.FirstOrDefault();
        }

        public override string ToString()
        {
            var hitIndicator = Shots.Any()
                ? Shots.First().IsHit ? "X" : "O"
                : " ";
            var shipSize = Ship?.Length.ToString() ?? " ";
            var sinkIndicator = Shots.Any(s => s.IsSink == true) ? "S" : " ";
            var text = hitIndicator + shipSize + sinkIndicator;
            return text;
        }
    }
}
