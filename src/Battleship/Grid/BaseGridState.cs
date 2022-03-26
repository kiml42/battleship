using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Grid
{
    public abstract class BaseGridState : IGridState
    {
        public abstract uint Width { get; }
        public abstract uint Height { get; }

        /// <summary>
        /// The current state of all the coordinates in the grid.
        /// Can be indexed with [y][x]
        /// </summary>
        public List<List<CoordinateState>> CoordinateStates
        {
            get
            {
                return Enumerable.Range(0, (int)Height)
                    .Select(rowIndex =>
                    {
                        return Enumerable.Range(0, (int)Width)
                        .Select(columnIndex => {
                            var shots = ShotResults.Where(s => s.X == columnIndex && s.Y == rowIndex);
                            var ship = ShipAt((uint)columnIndex, (uint)rowIndex);
                            return new CoordinateState((uint)columnIndex, (uint)rowIndex, ship, shots);
                        }).ToList();
                    }).ToList();
            }
        }

        public abstract IEnumerable<IShotResult> ShotResults { get; }

        public abstract ShotResult Shoot(uint x, uint y);

        public abstract ShipLocation ShipAt(uint x, uint y);

        public override string ToString()
        {
            var sb = new StringBuilder();

            int rowLength = 0;
            foreach (var row in CoordinateStates)
            {
                var cells = row.Select(coordinate => coordinate.ToString());
                var rowString = "|" + string.Join("|", cells) + "|";
                rowLength = rowString.Length;
                sb.AppendLine(new string('-', rowLength));
                sb.AppendLine(rowString);
            }
            sb.AppendLine(new string('-', rowLength));
            return sb.ToString();
        }
    }
}
