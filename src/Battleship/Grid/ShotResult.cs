namespace Battleship.Grid
{
    public struct ShotResult
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }
        public bool IsHit { get; private set; }
        public int Index { get; private set; }

        public ShotResult(uint x, uint y, bool isHit, int index)
        {
            X = x;
            Y = y;
            IsHit = isHit;
            Index = index;
        }
    }
}
