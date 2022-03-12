namespace Battleship.Grid
{
    public class GridState
    {
        public uint Width { get; private set; }
        public uint Height { get; private set; }

        public GridState(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

        public bool CanPlaceShip(uint x, uint y, Direction direction, uint length)
        {
            if (x >= Width || y >= Width)
                return false;   //Start is off the grid.
            switch (direction)
            {
                case Direction.Up:
                    if (length > y + 1)
                        return false;
                    break;
                case Direction.Down:
                    if (y + length > Height)
                        return false;
                    break;
                case Direction.Left:
                    if (length > x + 1)
                        return false;
                    break;
                case Direction.Right:
                    if (x + length > Width)
                        return false;
                    break;
            }
            return true;
        }

        public bool TryPlaceShip(uint x, uint y, Direction direction, uint length)
        {
            return CanPlaceShip(x, y, direction, length);
        }
    }
}
