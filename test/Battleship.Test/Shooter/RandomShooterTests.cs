using Battleship.Grid;
using Battleship.Shooter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Battleship.Test.Shooter
{
    public class RandomShooterTests
    {
        private const int GRID_WIDTH = 10;
        private const int GRID_HEIGHT = 10;

        [Fact]
        public void PickTarget_PicksTargetInGrid()
        {
            var grid = new GridState(GRID_WIDTH, GRID_HEIGHT);
            var shooter = new RandomShooter();

            var target = shooter.PickTarget(grid);

            Assert.InRange(target.X, 0, GRID_WIDTH - 1);
            Assert.InRange(target.Y, 0, GRID_HEIGHT - 1);
        }

        [Theory]
        [InlineData(2,2)]
        [InlineData(10,10)]
        [InlineData(10,2)]
        [InlineData(2,10)]
        [InlineData(100, 100)]
        public void PickTarget_PicksTargetAtEdgesSomeTImes(uint width, uint height)
        {
            var grid = new GridState(width, height);
            var shooter = new RandomShooter();

            var targets = Enumerable.Range(0, (int)width * (int)height * 10).Select(_ => shooter.PickTarget(grid));

            var minX = targets.Min(t => t.X);
            var minY = targets.Min(t => t.Y);
            var maxX = targets.Max(t => t.X);
            var maxY = targets.Max(t => t.Y);

            Assert.Equal(0, minX);
            Assert.Equal(0, minY);

            Assert.Equal((int)width - 1, maxX);
            Assert.Equal((int)height - 1, maxY);
        }

        [Fact]
        public void PickTarget_OnlyHitsALreadyShotTargetsAfterHittingEverySpace()
        {
            IGridState grid = new GridState(GRID_WIDTH, GRID_HEIGHT);
            var shooter = new RandomShooter();

            var previousTargets = new List<Point>();
            for (int i = 0; i < GRID_WIDTH * GRID_HEIGHT; i++)
            {
                var newTarget = shooter.PickTarget(grid);
                grid.Shoot(newTarget);

                Assert.DoesNotContain(newTarget, previousTargets);
                previousTargets.Add(newTarget);
            }

            var repeatShot = shooter.PickTarget(grid);
            Assert.Contains(repeatShot, previousTargets);
        }
    }
}
