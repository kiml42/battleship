using Battleship.Grid;
using System;
using System.Drawing;
using System.Linq;

namespace Battleship.Shooter
{
    public class RandomShooter : IShooter
    {
        private readonly Random _random = new();

        public Point PickTarget(IGridState grid)
        {
            var validTargets = grid.UntargetedCoordinates;
            if (!validTargets.Any())
            {
                validTargets = grid.FlattenedCoordinateStates;
            }

            var coordinate = validTargets.OrderBy(_ => _random.NextDouble()).First();

            return new Point((int)coordinate.X, (int)coordinate.Y);
        }
    }
}
