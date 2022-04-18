namespace Battleship.Grid
{
    public class ShotResult : IShotResult
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsHit { get; private set; }
        public bool? IsSink { get; private set; }
        public int Index { get; private set; }

        public ShotResult(int x, int y, bool isHit, bool isSink, int index)
        {
            X = x;
            Y = y;
            IsHit = isHit;
            IsSink = isSink;
            Index = index;
        }

        public override string ToString()
        {
            var hitString = IsHit ? "Hit" : "Miss";
            var sinkString = IsSink == true ? " & Sink" : "";
            return $"{X},{Y} - {hitString}{sinkString}";
        }
    }
}
