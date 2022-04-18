using System.Collections.Generic;
using System.Linq;

namespace Battleship.Grid
{
    public class CoordinateState
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public ShipLocation Ship {get; set; }

        private List<IShotResult> _shots;

        public IEnumerable<IShotResult> Shots => _shots;

        /// <summary>
        /// The result of the first shot.
        /// </summary>
        public IShotResult Shot => Shots.FirstOrDefault();

        public CoordinateState(int x, int y)
        {
            X = x;
            Y = y;
            _shots = new List<IShotResult>();
        }

        public CoordinateState(int x, int y, ShipLocation ship, IEnumerable<IShotResult> Shots)
        {
            X = x;
            Y = y;
            Ship = ship;
            _shots = Shots.ToList();
        }

        /// <summary>
        /// Creates a copy to be read from without being able to affect the state of teh original.
        /// </summary>
        /// <returns></returns>
        public CoordinateState Clone()
        {
            return new CoordinateState(X, Y, Ship, Shots);
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
