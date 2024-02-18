using Battleship.Shooter;

namespace Battleship.Test.Shooter
{
    public class RandomShooterTests : BaseShooterTests
    {
        protected override IShooter CreateShooter()
        {
            return new RandomShooter();
        }
    }
}
