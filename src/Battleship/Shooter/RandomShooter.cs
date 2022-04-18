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

            var index = _random.Next(0, validTargets.Count());

            var coordinate = validTargets.Skip(index).First();

            return new Point(coordinate.X, coordinate.Y);
        }
    }
}
