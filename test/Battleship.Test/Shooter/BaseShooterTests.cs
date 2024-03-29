﻿using Battleship.Grid;
using Battleship.Placement;
using Battleship.Shooter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Battleship.Test.Shooter
{
    public abstract class BaseShooterTests
    {
        private const int GRID_WIDTH = 10;
        private const int GRID_HEIGHT = 10;

        protected abstract IShooter CreateShooter();

        [Fact]
        public void PickTarget_PicksTargetInGrid()
        {
            var grid = new GridState(GRID_WIDTH, GRID_HEIGHT);
            var shooter = CreateShooter();

            var target = shooter.PickTarget(grid);

            Assert.InRange(target.X, 0, GRID_WIDTH - 1);
            Assert.InRange(target.Y, 0, GRID_HEIGHT - 1);
        }

        [Theory]
        [InlineData(2,2)]
        [InlineData(10,10)]
        [InlineData(10,2)]
        [InlineData(2,10)]
        [InlineData(20, 20)]
        public void PickTarget_PicksTargetAtEdgesSomeTimes(int width, int height)
        {
            var grid = new GridState(width, height);
            var shooter = CreateShooter();

            var expectedMaxX = width - 1;
            var expectedMaxY = height - 1;

            int? minX = null;
            int? minY = null;
            int? maxX = null;
            int? maxY = null;
            for (int i = 0; i < grid.FlattenedCoordinateStates.Count(); i++)
            {
                var target = shooter.PickTarget(grid);
                grid.Shoot(target); // shoot at the coordinate so it doesn't get picked again.

                minX = Math.Min(target.X, minX ?? target.X);
                minY = Math.Min(target.Y, minY ?? target.Y);
                maxX = Math.Max(target.X, maxX ?? target.X);
                maxY = Math.Max(target.Y, maxY ?? target.Y);

                if (minX == 0 && minY == 0 && maxX == expectedMaxX && maxY == expectedMaxY)
                    break;
            }

            Assert.Equal(0, minX);
            Assert.Equal(0, minY);

            Assert.Equal(expectedMaxX, maxX);
            Assert.Equal(expectedMaxY, maxY);
        }

        [Fact]
        public void PickTarget_OnlyHitsAlreadyShotTargetsAfterHittingEverySpace()
        {
            IGridState grid = new GridState(GRID_WIDTH, GRID_HEIGHT);
            var shooter = CreateShooter();

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
    
        [Fact]
        public void EventuallyWins()
        {
            var shotsToWin = new List<int>();
            var shipsToPlace = new int[]{ 5, 4, 4, 3, 2 };

            var worstPossibleShotsToWin = GRID_WIDTH * GRID_HEIGHT;
            for (int j = 0; j < 1000; j++)
            {
                var grid = new GridState(GRID_WIDTH, GRID_HEIGHT);
                var shipPlacer = new RandomShipPlacer();
                shipPlacer.TryPlaceShips(grid, shipsToPlace);

                var shooter = CreateShooter();

                for (int i = 0; i < worstPossibleShotsToWin * 2; i++)
                {
                    var newTarget = shooter.PickTarget(grid);
                    ((IGridState)grid).Shoot(newTarget);
                    if(grid.RemainingTargetCoordinates == 0)
                    {
                        shotsToWin.Add(i + 1);
                        break;
                    }
                }
                Assert.Equal(0, grid.RemainingTargetCoordinates);
            }

            var bestPossibleShotsToWin = shipsToPlace.Sum(s => s);

            var bestShotsToWin = shotsToWin.Min();
            var averageShotsToWin = shotsToWin.Average();
            var worstShotsToWin = shotsToWin.Max();

            Assert.InRange(bestShotsToWin, bestPossibleShotsToWin, worstPossibleShotsToWin);
            Assert.InRange(averageShotsToWin, bestPossibleShotsToWin, worstPossibleShotsToWin);
            Assert.InRange(worstShotsToWin, bestPossibleShotsToWin, worstPossibleShotsToWin);
        }
    }
}
