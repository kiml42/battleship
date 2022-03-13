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

            var grid = new GridState(10, 10);

            var placer = new RandomShipPlacer();

            var result = placer.TryPlaceShips(sizesToPlace);

            Assert.True(result);

            var groups = sizesToPlace.GroupBy(i=>i);

            Assert.All(groups, group =>
            {
                var length = group.Key;
                var expectedCount = group.Count();
                Assert.Equal(expectedCount, grid.Ships.Count(s => s.Length == length));
            });
        }

        [Fact]
        public void TryPlaceShips_ReturnsFalseIfItCantFitAllTheShips()
        {
            var sizesToPlace = new uint[] { 2, 3, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5 };

            var grid = new GridState(10, 10);

            var placer = new RandomShipPlacer();

            var result = placer.TryPlaceShips(sizesToPlace);

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
