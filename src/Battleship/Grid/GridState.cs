using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Battleship.Grid
{
    public class GridState : BaseGridState
    {
        private readonly int _width;
        public override int Width => _width;

        private readonly int _height;
        public override int Height => _height;

        public List<ShipLocation> Ships { get; } = new List<ShipLocation>();
        private readonly CoordinateState[][] _coordinateStates;

        /// <inheritdoc/>
        public override CoordinateState[][] CoordinateStates
        {
            get
            {
                return _coordinateStates.Select(row => row.Select(c => c.Clone()).ToArray()).ToArray();
            }
        }

        public GridState(int width, int height)
        {
            _width = width;
            _height = height;

            _coordinateStates = Enumerable.Range(0, Height)
                    .Select(rowIndex =>
                    {
                        return Enumerable.Range(0, Width)
                        .Select(columnIndex =>
                        {
                            return new CoordinateState(columnIndex, rowIndex);
                        }).ToArray();
                    }).ToArray();
        }

        private IEnumerable<Point> AllCoordinatesWithShips => Ships.SelectMany(s => s.FullCoordinates);

        public override IEnumerable<IShotResult> ShotResults => FlattenedCoordinateStates.SelectMany(c => c.Shots);

        public override int RemainingTargetCoordinates => UntargetedCoordinates.Count(c => c.Ship != null);

        public override IEnumerable<int> OriginalShips => Ships.Select(s => s.Length);

        public bool CanPlaceShip(int x, int y, int length, Orientation orientation)
        {
            var ship = new ShipLocation(x, y, length, orientation);
            return CanPlaceShip(ship);
        }

        public ShipLocation TryPlaceShip(int x, int y, int length, Orientation orientation)
        {
            var ship = new ShipLocation(x, y, length, orientation);
            if (!CanPlaceShip(ship))
                return null;
            Ships.Add(ship);

            foreach (var location in ship.FullCoordinates)
            {
                var coordinateState = _coordinateStates[location.Y][location.X];
                coordinateState.Ship = ship;
            }

            return ship;
        }

        public override ShipLocation ShipAt(int x, int y)
        {
            var point = new Point(x, y);
            return Ships.SingleOrDefault(s => s.FullCoordinates.Contains(point));
        }

        private bool CanPlaceShip(ShipLocation ship)
        {
            return ship.FullCoordinates.All(p => CanPlacePartOfShip(p));
        }

        private bool CanPlacePartOfShip(Point newShipPart)
        {
            if (newShipPart.X < 0 || newShipPart.Y < 0 || newShipPart.X >= Width || newShipPart.Y >= Height)
                return false;
            return AllCoordinatesWithShips.All(p => p != newShipPart);
        }

        public override ShotResult Shoot(int x, int y)
        {
            var coordinate = _coordinateStates[y][x];

            var ship = coordinate.Ship;

            var remaining = ship?.FullCoordinates.Where(l => !ShotResults.Any(s => s.X == l.X && s.Y == l.Y));

            var isSink = remaining?.Count() == 1 && remaining.Any(l => l.X == x && l.Y == y);

            var result = new ShotResult(x, y, ship != null, isSink, ShotResults.Count());

            coordinate.AddShot(result);

            return result;
        }
    }
}
