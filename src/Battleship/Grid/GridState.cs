using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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

        public ShipLocation ShipAt(uint x, uint y)
        {
            var point = new Point((int)x, (int)y);
            return Ships.FirstOrDefault(s => s.FullCoordinates.Contains(point));
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

        public override string ToString()
        {
            var sb = new StringBuilder();

            int rowLength = 0;
            for (uint y = 0; y < Height; y++)
            {
                var cells = Enumerable.Range(0, (int)Width).Select(x => GetTextForCell((uint)x, y));
                var row = "|" + string.Join("|", cells) + "|";
                rowLength = row.Length;
                sb.AppendLine(new string('-', rowLength));
                sb.AppendLine(row);
            }
            sb.AppendLine(new string('-', rowLength));
            return sb.ToString();
        }

        private string GetTextForCell(uint x, uint y)
        {
            var text = string.Empty;
            var ship = ShipAt(x, y);
            if(ship != null)
            {
                text = ship.Length.ToString();
            }
            return text.PadRight(2).PadLeft(3);
        }
    }
}
