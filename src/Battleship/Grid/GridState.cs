using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Battleship.Grid
{
    public class GridState : BaseGridState
    {
        private readonly uint _width;
        public override uint Width => _width;

        private readonly uint _height;
        public override uint Height => _height;

        public List<ShipLocation> Ships { get; } = new List<ShipLocation>();

        public GridState(uint width, uint height)
        {
            _width = width;
            _height = height;
        }

        private IEnumerable<Point> AllCoordinatesWithShips => Ships.SelectMany(s => s.FullCoordinates);

        private readonly List<IShotResult> _shotResults = new List<IShotResult>();
        public override IEnumerable<IShotResult> ShotResults => _shotResults;

        protected override bool _showUnHitShipLocations => true;

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

        public override ShipLocation ShipAt(uint x, uint y)
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

        public override ShotResult Shoot(uint x, uint y)
        {
            var ship = ShipAt(x, y);

            var remaining = ship?.FullCoordinates.Where(l => !ShotResults.Any(s => s.X == l.X && s.Y == l.Y));

            var isSink = remaining?.Count() == 1 && remaining.Any(l => l.X == x && l.Y == y);

            var result = new ShotResult(x, y, ship != null, isSink, ShotResults.Count());

            _shotResults.Add(result);

            return result;
        }
    }
}
