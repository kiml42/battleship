using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Battleship.Grid
{
    public abstract class BaseGridState : IGridState
    {
        private const string ColumnSeparator = "|";

        public abstract int Width { get; }
        public abstract int Height { get; }

        /// <summary>
        /// The current state of all the coordinates in the grid.
        /// Can be indexed with [y][x]
        /// </summary>
        public abstract CoordinateState[][] CoordinateStates { get; }

        public IEnumerable<CoordinateState> FlattenedCoordinateStates => CoordinateStates.SelectMany(c => c);

        public IEnumerable<CoordinateState> UntargetedCoordinates => FlattenedCoordinateStates.Where(c => !c.Shots.Any());

        public abstract IEnumerable<IShotResult> ShotResults { get; }

        public abstract int RemainingTargetCoordinates { get; }
        public abstract IEnumerable<int> OriginalShips { get; }

        public BaseGridState()
        {

        }

        public ShotResult Shoot(Point coordinate)
        {
            return Shoot(coordinate.X, coordinate.Y);
        }

        public abstract ShotResult Shoot(int x, int y);

        public abstract ShipLocation ShipAt(int x, int y);

        public override string ToString()
        {
            var sb = new StringBuilder();

            int rowLength = 0;
            foreach (var row in CoordinateStates)
            {
                var cells = row.Select(coordinate => coordinate.ToString());
                var rowString = ColumnSeparator + string.Join(ColumnSeparator, cells) + ColumnSeparator;
                rowLength = rowString.Length;
                sb.AppendLine(new string('-', rowLength));
                sb.AppendLine(rowString);
            }
            sb.AppendLine(new string('-', rowLength));
            return sb.ToString();
        }

        public string ToStringWithIndices()
        {
            var text = this.ToString();

            var rows = text.Split(Environment.NewLine);

            var rowLength = rows[1].Length;
            var cells = rows[1].Split(ColumnSeparator);
            var cellLength = cells.Max(c => c.Length);

            var columnNumbers = Enumerable.Range(0, Width).Select(c => c.ToString().PadLeft(2).PadRight(cellLength));
            var columnHeadersString = ColumnSeparator + string.Join(" ", columnNumbers) + ColumnSeparator;

            var rowNumbers = Enumerable.Range(0, Height).Select(r => r.ToString());
            var maxRowNumberWidth = rowNumbers.Max(r => r.Length);
            rowNumbers = rowNumbers.Select(r => r.PadLeft(maxRowNumberWidth));

            var padding = new string(' ', maxRowNumberWidth);

            columnHeadersString = padding + columnHeadersString;

            var sb = new StringBuilder(columnHeadersString);
            sb.AppendLine();

            var i = 0;
            foreach (var rowNumber in rowNumbers)
            {
                var dashesRow = rows[i++];
                sb.Append(padding);
                sb.AppendLine(dashesRow);

                var row = rows[i++];
                sb.Append(rowNumber);
                sb.AppendLine(row);
            }
            var finalDashesRow = rows[i++];
            sb.Append(padding);
            sb.AppendLine(finalDashesRow);

            return sb.ToString();
        }
    }
}
