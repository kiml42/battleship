namespace Battleship.Grid
{
    /// <summary>
    /// Defines the settings for what should be shown to the player
    /// </summary>
    public struct GridMaskSettings
    {
        public readonly bool ShowSinks;

        public readonly bool ShowLengthsOfHitShips;

        public GridMaskSettings(bool showSinks, bool showLengthsOfHitShips)
        {
            ShowSinks = showSinks;
            ShowLengthsOfHitShips = showLengthsOfHitShips;
        }
    }
}
