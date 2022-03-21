using System.Drawing;

namespace Battleship.Grid
{
    public interface IGridState
    {
        ShotResult Shoot(Point coordinate)
        {
            // TODO test this
            return Shoot((uint)coordinate.X, (uint)coordinate.Y);
        }

        ShotResult Shoot(uint x, uint y);
    }
}
