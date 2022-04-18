namespace Battleship.Grid
{
    public interface IShotResult
    {
        public int X { get; }
        public int Y { get; }
        public bool IsHit { get; }
        public bool? IsSink { get; }
        public int Index { get; }
    }
}
