using Battleship.Grid;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Battleship.Test.Grid
{
    public class MaskedShotResultTests
    {
        [Theory]
        [MemberData(nameof(TestConfigurations))]
        public void ToString_Test(string expected, ShotResult shot, GridMaskSettings mask)
        {
            var maskedShot = new MaskedShotResult(shot, mask);

            var result = maskedShot.ToString();

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(TestConfigurations_WithoutExpectedStrings))]
        public void IsSink_Test(ShotResult shot, GridMaskSettings mask)
        {
            var maskedShot = new MaskedShotResult(shot, mask);

            var result = maskedShot.IsSink;

            if (mask.ShowSinks)
            {
                Assert.NotNull(result);
                Assert.Equal(shot.IsSink, result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> TestConfigurations_WithoutExpectedStrings()
        {
            return TestConfigurations().Select(c => new object[] { c[1], c[2] });
        }

        public static IEnumerable<object[]> TestConfigurations()
        {
            var notHit = new ShotResult(0, 0, false, false, 0);
            var hit = new ShotResult(0, 0, true, false, 0);
            var sink = new ShotResult(0, 0, true, true, 0);

            var showNothing = new GridMaskSettings(false);
            var showSinks = new GridMaskSettings(true);

            yield return new object[] { "0,0 - ", notHit, showNothing };
            yield return new object[] { "0,0 - ", notHit, showSinks };
            yield return new object[] { "0,0 - Hit", hit, showNothing };
            yield return new object[] { "0,0 - Hit", hit, showSinks };
            yield return new object[] { "0,0 - Hit", sink, showNothing };
            yield return new object[] { "0,0 - Hit & Sink", sink, showSinks };
        }
    }
}
