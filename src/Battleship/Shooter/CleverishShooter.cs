using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Battleship.Shooter
{
    public class CleverishShooter : IShooter
    {
        private readonly IShooter _fallbackShooter = new RandomShooter();

        public Point PickTarget(IGridState grid)
        {
            var validTargets = grid.UntargetedCoordinates;

            if (!validTargets.Any())
            {
                return _fallbackShooter.PickTarget(grid);
            }

            var shipsLeft = grid.OriginalShips;

            var shortestShip = shipsLeft.Any() ? shipsLeft.Min() : 1;

            // the number of ways a ship could be partially in a grid square.
            var maxPossibleConfigurations = (shortestShip - 1) * 4;

            CoordinateState bestCoordinate = validTargets.First();
            uint bestNumberOfConfigurations = 0;
            foreach (var coordinate in validTargets)
            {
                var numberOfConfigurationsForCoordinate = GetNumberOfConfigurations(coordinate, shortestShip, grid);
                if(numberOfConfigurationsForCoordinate < bestNumberOfConfigurations)
                {
                    bestCoordinate = coordinate;
                    bestNumberOfConfigurations = numberOfConfigurationsForCoordinate;
                }
                if (bestNumberOfConfigurations == maxPossibleConfigurations) break;
            }

            return new Point((int)bestCoordinate.X, (int)bestCoordinate.Y);
        }

        private static uint GetNumberOfConfigurations(CoordinateState coordinate, uint shipLength, IGridState grid)
        {
            var distanceFromCoordinate = shipLength - 1;

            var distanceToTheLeft = 0;
            for (int deltaX = 1; deltaX < distanceFromCoordinate; deltaX++)
            {
                if (deltaX > coordinate.X) break;
                var x = coordinate.X - deltaX;

                var otherCoordinateoordintate = grid.CoordinateStates[(int)coordinate.Y][(int)x];
                if (otherCoordinateoordintate.Shot != null && (!otherCoordinateoordintate.Shot.IsHit || otherCoordinateoordintate.Shot.IsSink == true))
                    break;
                distanceToTheLeft = deltaX;
            }

            var distanceToTheRight = 0;
            for (int deltaX = 1; deltaX < distanceFromCoordinate; deltaX++)
            {
                var x = coordinate.X + deltaX;
                if (x >= grid.Width) break;

                var otherCoordinateoordintate = grid.CoordinateStates[(int)coordinate.Y][(int)x];
                if (otherCoordinateoordintate.Shot != null && (!otherCoordinateoordintate.Shot.IsHit || otherCoordinateoordintate.Shot.IsSink == true))
                    break;
                distanceToTheLeft = deltaX;
            }

            var minX = Math.Max(0, coordinate.X - distanceFromCoordinate);
            var maxX = Math.Min(grid.Width-1, coordinate.X + distanceFromCoordinate);

            var minY = Math.Max(0, coordinate.Y - distanceFromCoordinate);
            var maxY = Math.Min(grid.Height - 1, coordinate.Y + distanceFromCoordinate);

            var coordintaesToConsider = new List<CoordinateState>();

            for (uint x = minX; x <= maxX; x++)
            {
                if (x == coordinate.X) continue;
                var otherCoordinate = grid.CoordinateStates[(int)coordinate.Y][(int)x];
                coordintaesToConsider.Add(otherCoordinate);
            }

            for (uint y = minY; y <= maxY; y++)
            {
                if (y == coordinate.Y) continue;
                var otherCoordinate = grid.CoordinateStates[(int)y][(int)coordinate.X];
                coordintaesToConsider.Add(otherCoordinate);
            }

            // count all the considered coordinates that either haven't been shot at yet, or are a hit but not a sink.
            return (uint)coordintaesToConsider.Count(c => c.Shot == null || (c.Shot.IsHit && c.Shot.IsSink != true));
        }
    }
}
