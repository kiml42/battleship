namespace Battleship.Grid
{
    public struct ShotResult
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }
        public bool IsHit { get; private set; }
        public bool IsSink { get; private set; }
        public int Index { get; private set; }

        public ShotResult(uint x, uint y, bool isHit, bool isSink, int index)
        {
            X = x;
            Y = y;
            IsHit = isHit;
            IsSink = isSink;
            Index = index;
        }

        public override string ToString()
        {
            var hitString = IsHit ? "Hit" : "";
            var sinkString = IsSink ? " & Sink" : "";
            return $"{X},{Y} - {hitString}{sinkString}";
        }
    }
}
