using Battleship.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Shooter
{
    public class RandomShooter : IShooter
    {
        private readonly Random random = new();
        private const uint MAX_RETRIES = 500;

        public Point PickTarget(IGridState grid)
        {
            return PickTarget(grid, MAX_RETRIES);
        }

        public Point PickTarget(IGridState grid, uint remainingRetries)
        {
            var y = random.Next(0, (int)grid.Height);

            //var shotsInRow = grid.ShotResults.Where(s => s.Y == y);

            var x = random.Next(0, (int)grid.Width);

            if(remainingRetries > 0 && grid.ShotResults.Any(r => r.X == x && r.Y == y))
            {
                return PickTarget(grid, --remainingRetries);    // try again to get a valid one.
            }

            return new Point(x, y);
        }

        public Point PickTarget2(GridState grid, uint remainingRetries)
        {
            var y = random.Next(0, (int)grid.Height);

            var shotsInRow = grid.ShotResults.Where(s => s.Y == y);

            var x = random.Next(0, (int)grid.Width);

            if(remainingRetries > 0 && grid.ShotResults.Any(r => r.X == x && r.Y == y))
            {
                return PickTarget(grid, --remainingRetries);    // try again to get a valid one.
            }

            return new Point(x, y);
        }
    }
}
