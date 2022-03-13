using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Placement
{
    public class RandomShipPlacer : IShipPlacer
    {
        private readonly Random rng = new();
        private const int MAX_TRIES_TO_PLACE_EACH_SHIP = 50;

        public bool TryPlaceShips(GridState grid, params uint[] shipLengths)
        {
            var sortedLengths = shipLengths.OrderByDescending(l => l);

            foreach (var length in sortedLengths)
            {
                ShipLocation shipLocation;
                var i = 0;
                do
                {
                    var x = (uint)rng.Next(0, (int)grid.Width - 1);
                    var y = (uint)rng.Next(0, (int)grid.Height - 1);
                    var orientation = rng.Next(0, 2) == 0 ? Orientation.Horizontal : Orientation.Vertical;

                    shipLocation = grid.TryPlaceShip(x, y, length, orientation);

                } while (shipLocation == null && i++ < MAX_TRIES_TO_PLACE_EACH_SHIP);

                if (shipLocation == null)
                    return false;
            }
            return true;
        }
    }
}
