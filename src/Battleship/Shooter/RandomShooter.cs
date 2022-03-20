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
        private readonly Random random = new Random();

        public Point PickTarget(GridState grid)
        {
            var x = random.Next(0, (int)grid.Width);
            var y = random.Next(0, (int)grid.Height);

            return new Point(x, y);
        }
    }
}
