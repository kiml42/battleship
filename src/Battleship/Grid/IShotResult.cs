namespace Battleship.Grid
{
    public interface IShotResult
    {
        public uint X { get; }
        public uint Y { get; }
        public bool IsHit { get; }
        public bool? IsSink { get; }
        public int Index { get; }
    }
}
