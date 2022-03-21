using Battleship.Grid;
using System.Drawing;

namespace Battleship.Shooter
{
    public interface IShooter
    {
        Point PickTarget(IGridState grid);
    }
}
