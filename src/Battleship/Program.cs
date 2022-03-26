using Battleship.Grid;
using Battleship.Placement;
using System;
using System.Linq;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Battleship!");

            var grid = new GridState(10, 10);
            var shipPlacer = new RandomShipPlacer();
            shipPlacer.TryPlaceShips(grid, 5, 4, 4, 3, 2);

            var maskedGrid = new MaskedGridState(grid, new GridMaskSettings(true, false));
            Console.WriteLine(maskedGrid);

            while (maskedGrid.ShotResults.Count() < 100)
            {
                var input = Console.ReadLine();
                var parts = input.Split(",");
                if(parts.Length == 2 && uint.TryParse(parts[0], out var x) && uint.TryParse(parts[1], out var y))
                {
                    var result = maskedGrid.Shoot(x, y);
                    Console.WriteLine(result);
                    Console.WriteLine(maskedGrid);
                } else
                {
                    Console.WriteLine("Invalid shot. Use \"X,Y\"");
                }
            }
        }
    }
}
