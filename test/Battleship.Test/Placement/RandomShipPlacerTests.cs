using Battleship.Grid;
using Battleship.Placement;
using System;
using System.Linq;
using Xunit;

namespace Battleship.Test.Placement
{
    public class RandomShipPlacerTests
    {
        [Fact]
        public void TryPlaceShips_PlacesExpectedShips()
        {
            var sizesToPlace = new uint[] { 2, 3, 4, 4, 5 };

            var totalHorizontalCount = 0;
            var totalVerticalCount = 0;
            const int repeats = 100;
            for (int i = 0; i < repeats; i++)
            {
                var grid = new GridState(10, 10);
                var placer = new RandomShipPlacer();
                var result = placer.TryPlaceShips(grid, sizesToPlace);

                Assert.True(result);

                var groups = sizesToPlace.GroupBy(i=>i);

                Assert.All(groups, group =>
                {
                    var length = group.Key;
                    var expectedCount = group.Count();
                    Assert.Equal(expectedCount, grid.Ships.Count(s => s.Length == length));
                });

                var horizontalCount = grid.Ships.Count(s => s.Orientation == Orientation.Horizontal);
                var verticalCount = grid.Ships.Count(s => s.Orientation == Orientation.Vertical);
                totalHorizontalCount += horizontalCount;
                totalVerticalCount += verticalCount;
            }

            // check that the number of horizontals and verticals are close to half each
            var totalShipsPlaced = totalHorizontalCount + totalVerticalCount;
            Assert.Equal(repeats * sizesToPlace.Length, totalShipsPlaced);
            var fortiethPercentile = totalShipsPlaced * 0.4;
            var sixtiethPercentile = totalShipsPlaced * 0.6;
            Assert.InRange(totalHorizontalCount, fortiethPercentile, sixtiethPercentile);
            Assert.InRange(totalVerticalCount, fortiethPercentile, sixtiethPercentile);
        }

        [Fact]
        public void TryPlaceShips_ReturnsFalseWithAnyShipsThatDontFIt()
        {
            var sizesToPlace = new uint[] { 11 };

            var grid = new GridState(10, 10);

            var placer = new RandomShipPlacer();

            var result = placer.TryPlaceShips(grid, sizesToPlace);

            Assert.False(result);

            Assert.Empty(grid.Ships);
        }

        [Fact]
        public void TryPlaceShips_ReturnsFalseIfItCantFitAllTheShips()
        {
            var sizesToPlace = new uint[] { 2, 3, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, };

            var grid = new GridState(10, 10);

            var placer = new RandomShipPlacer();

            var result = placer.TryPlaceShips(grid, sizesToPlace);

            Assert.False(result);

            //TODO decide how it should handle not managing to place all the ships.
            //var groups = sizesToPlace.GroupBy(i=>i);

            //Assert.All(groups, group =>
            //{
            //    var length = group.Key;
            //    var expectedCount = group.Count();
            //    Assert.Equal(expectedCount, grid.Ships.Count(s => s.Length == length));
            //});
        }
    }
}
