namespace Battleship.Grid
{
    public class MaskedShotResult : IShotResult
    {
        public uint X => _shot.X;
        public uint Y => _shot.Y;
        public bool IsHit => _shot.IsHit;
        public bool? IsSink => _settings.ShowSinks ? _shot.IsSink : null;
        public int Index => _shot.Index;

        private readonly IShotResult _shot;

        private readonly GridMaskSettings _settings;

        public MaskedShotResult(IShotResult shot, GridMaskSettings settings)
        {
            _shot = shot;
            _settings = settings;
        }

        public override string ToString()
        {
            var hitString = IsHit ? "Hit" : "";
            var sinkString = IsSink == true ? " & Sink" : "";
            return $"{X},{Y} - {hitString}{sinkString}";
        }
    }
}
