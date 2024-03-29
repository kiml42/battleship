﻿using Battleship.Grid;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Battleship.Test
{
    public class ShipLocationTests
    {
        [Fact]
        public void FullCoordinates_Horizontal()
        {
            int length = 4;
            var ship = new ShipLocation(1, 1, length, Orientation.Horizontal);

            var result = ship.FullCoordinates;

            Assert.Equal(length, result.Count());
            Assert.Contains(new Point(1, 1), result);
            Assert.Contains(new Point(2, 1), result);
            Assert.Contains(new Point(3, 1), result);
            Assert.Contains(new Point(4, 1), result);
        }

        [Fact]
        public void FullCoordinates_Vertical()
        {
            int length = 4;
            var ship = new ShipLocation(6, 1, length, Orientation.Vertical);

            var result = ship.FullCoordinates;

            Assert.Equal(length, result.Count());
            Assert.Contains(new Point(6, 1), result);
            Assert.Contains(new Point(6, 2), result);
            Assert.Contains(new Point(6, 3), result);
            Assert.Contains(new Point(6, 4), result);
        }
    }
}
