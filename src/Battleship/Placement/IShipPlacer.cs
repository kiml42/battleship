using Battleship.Grid;

namespace Battleship.Placement
{
    public interface IShipPlacer
    {
        bool TryPlaceShips(GridState grid, params int[] shipLengths);
    }
}
