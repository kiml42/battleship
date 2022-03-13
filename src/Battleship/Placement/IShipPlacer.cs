namespace Battleship.Placement
{
    public interface IShipPlacer
    {
        bool TryPlaceShips(params uint[] shipLengths);
    }
}
