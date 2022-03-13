using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Battleship.Grid
{
    public class GridState
    {
        public uint Width { get; private set; }
        public uint Height { get; private set; }

        public List<ShipLocation> Ships { get; } = new List<ShipLocation>();

        public GridState(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

        private IEnumerable<Point> AllCoordinatesWithShips => Ships.SelectMany(s => s.FullCoordinates);

        public bool CanPlaceShip(uint x, uint y, uint length, Orientation orientation)
        {
            var ship = new ShipLocation(x, y, length, orientation);
            return CanPlaceShip(ship);
        }

        public ShipLocation TryPlaceShip(uint x, uint y, uint length, Orientation orientation)
        {
            var ship = new ShipLocation(x, y, length, orientation);
            if (!CanPlaceShip(ship))
                return null;
            Ships.Add(ship);
            return ship;
        }

        private bool CanPlaceShip(ShipLocation ship)
        {
            return ship.FullCoordinates.All(p => CanPlacePartOfShip(p));
        }

        private bool CanPlacePartOfShip(Point newShipPart)
        {
            if (newShipPart.X >= Width || newShipPart.Y >= Height)
                return false;
            return AllCoordinatesWithShips.All(p => p != newShipPart);
        }
    }
}
