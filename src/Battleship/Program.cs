using Battleship.Grid;
using Battleship.Placement;
using Battleship.Shooter;
using System;
using System.Drawing;
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
            Console.WriteLine(maskedGrid.ToStringWithIndices());
            Console.WriteLine($"{maskedGrid.RemainingTargetCoordinates} targets to find.");
            Console.WriteLine($"Call shot 0");

            var randomShooter = new RandomShooter();
            var cleverishShooter = new CleverishShooter();

            while (maskedGrid.ShotResults.Count() < 100)
            {
                var input = Console.ReadLine();
                Point? target = null;
                switch (input.ToLowerInvariant())
                {
                    case "r":
                        target = randomShooter.PickTarget(grid);
                        break;
                    case "c":
                        target = cleverishShooter.PickTarget(grid);
                        break;
                    default:
                        var parts = input.Split(",");
                        if (parts.Length == 2 && uint.TryParse(parts[0], out var x) && uint.TryParse(parts[1], out var y))
                            target = new Point((int)x, (int)y);
                        break;
                }

                if (target.HasValue)
                {
                    var result = maskedGrid.Shoot(target.Value);
                    Console.WriteLine(maskedGrid.ToStringWithIndices());
                    Console.WriteLine($"Shot number {result.Index}: {result}. {maskedGrid.RemainingTargetCoordinates} targets left to find.");
                    if (maskedGrid.RemainingTargetCoordinates == 0)
                    {
                        Console.WriteLine("You Win!");
                        return;
                    }
                    Console.WriteLine($"Call shot {result.Index + 1}");
                }
                else
                {
                    Console.WriteLine("Invalid shot. Use \"X,Y\" or 'r' to use the random shooter, or 'c' to use the cleverish shooter");
                }

            }
        }
    }
}
