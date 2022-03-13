using System.Collections.Generic;

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

        public bool CanPlaceShip(uint x, uint y, uint length, Orientation orientation)
        {
            if (x >= Width || y >= Width)
                return false;   //Start is off the grid.
            switch (orientation)
            {
                case Orientation.Horizontal:
                    if (y + length > Height)
                        return false;
                    break;
                case Orientation.Vertical:
                    if (x + length > Width)
                        return false;
                    break;
            }
            return true;
        }

        public ShipLocation TryPlaceShip(uint x, uint y, uint length, Orientation orientation)
        {
            if (!CanPlaceShip(x, y, length, orientation))
                return null;
            var ship = new ShipLocation(x, y, length, orientation);
            Ships.Add(ship);
            return ship;
        }
    }
}
