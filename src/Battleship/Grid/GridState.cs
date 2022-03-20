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

        public List<ShotResult> ShotResults { get; } = new List<ShotResult>();

        /// <summary>
        /// The current state of all the coordinates in the grid.
        /// Can be indexed with [y][x]
        /// </summary>
        public List<List<CoordinateState>> CoordinateStates
        {
            get {
                return Enumerable.Range(0, (int)Height)
                    .Select(rowIndex =>
                {
                    return Enumerable.Range(0, (int)Width)
                    .Select(columnIndex => {
                        var ship = ShipAt((uint)columnIndex, (uint)rowIndex);
                        var shots = ShotResults.Where(s => s.X == columnIndex && s.Y == rowIndex);
                        return new CoordinateState((uint)columnIndex, (uint)rowIndex, ship, shots);
                    }).ToList();
                }).ToList();
            }
        }

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
            return Ships.SingleOrDefault(s => s.FullCoordinates.Contains(point));
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

        public ShotResult Shoot(Point coordinate)
        {
            // TODO test this
            return Shoot((uint)coordinate.X, (uint)coordinate.Y);
        }

        public ShotResult Shoot(uint x, uint y)
        {
            var ship = ShipAt(x, y);

            var remaining = ship?.FullCoordinates.Where(l => !ShotResults.Any(s => s.X == l.X && s.Y == l.Y));

            var isSink = remaining?.Count() == 1 && remaining.Any(l => l.X == x && l.Y == y);

            var result = new ShotResult(x, y, ship != null, isSink, ShotResults.Count);

            this.ShotResults.Add(result);

            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            int rowLength = 0;
            foreach (var row in CoordinateStates)
            {
                var cells = row.Select(coordinate => coordinate.ToString());
                var rowString = "|" + string.Join("|", cells) + "|";
                rowLength = rowString.Length;
                sb.AppendLine(new string('-', rowLength));
                sb.AppendLine(rowString);
            }
            sb.AppendLine(new string('-', rowLength));
            return sb.ToString();
        }
    }
}
