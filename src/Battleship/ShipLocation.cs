using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class ShipLocation
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }
        public Direction Direction { get; private set; }
        public uint Length { get; private set; }

        public ShipLocation(uint x, uint y, Direction direction, uint length)
        {
            X = x;
            Y = y;
            Direction = direction;
            Length = length;
        }
    }
}
