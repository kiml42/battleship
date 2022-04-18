using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class ShipLocation
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Orientation Orientation { get; private set; }
        public int Length { get; private set; }

        /// <summary>
        /// Lists the coordinates of each square that contains part of this ship.
        /// </summary>
        public IEnumerable<Point> FullCoordinates
        {
            get
            {
                return Enumerable.Range(0, Length).Select(i => {
                    var x = X + (Orientation == Orientation.Horizontal ? i : 0);
                    var y = Y + (Orientation == Orientation.Vertical ? i : 0);
                    return new Point(x, y);
                    });
            }
        }

        public ShipLocation(int x, int y, int length, Orientation orientation)
        {
            X = x;
            Y = y;
            Length = length;
            Orientation = orientation;
        }
    }
}
