using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Battleship.Test
{
    public class ShipLocationTests
    {
        [Fact]
        public void FullCoordinates_Horizontal()
        {
            uint length = 4;
            var ship = new ShipLocation(1, 1, length, Orientation.Horizontal);

            var result = ship.FullCoordinates;

            Assert.Equal((int)length, result.Count());
            Assert.Contains(new Point(1, 1), result);
            Assert.Contains(new Point(2, 1), result);
            Assert.Contains(new Point(3, 1), result);
            Assert.Contains(new Point(4, 1), result);
        }

        [Fact]
        public void FullCoordinates_Vertical()
        {
            uint length = 4;
            var ship = new ShipLocation(6, 1, length, Orientation.Vertical);

            var result = ship.FullCoordinates;

            Assert.Equal((int)length, result.Count());
            Assert.Contains(new Point(6, 1), result);
            Assert.Contains(new Point(6, 2), result);
            Assert.Contains(new Point(6, 3), result);
            Assert.Contains(new Point(6, 4), result);
        }
    }
}
