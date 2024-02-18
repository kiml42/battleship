namespace Battleship.Grid
{
    /// <summary>
    /// Defines the settings for what should be shown to the player
    /// </summary>
    public readonly struct GridMaskSettings
    {
        public readonly bool ShowSinks;

        public bool ShowLengthsOfAllShips => _shipLengthIndicationSetting == ShipLengthIndicationSetting.All;
        public bool ShowLengthsOfHitShips => _shipLengthIndicationSetting == ShipLengthIndicationSetting.Hit || ShowLengthsOfAllShips;
        public bool ShowLengthsOfShipsWhenSunk => _shipLengthIndicationSetting == ShipLengthIndicationSetting.Sink || _shipLengthIndicationSetting == ShipLengthIndicationSetting.Hit || ShowLengthsOfAllShips;

        private readonly ShipLengthIndicationSetting _shipLengthIndicationSetting;

        public GridMaskSettings(bool showSinks, ShipLengthIndicationSetting shipLengthIndicationSetting = ShipLengthIndicationSetting.None)
        {
            ShowSinks = showSinks;
            _shipLengthIndicationSetting = shipLengthIndicationSetting;
        }

        public static GridMaskSettings ShowAll => new GridMaskSettings(true, ShipLengthIndicationSetting.All);

        public enum ShipLengthIndicationSetting
        {
            None,
            Hit,
            Sink,
            All 
        }
    }
}
