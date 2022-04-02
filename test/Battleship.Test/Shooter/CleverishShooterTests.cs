using Battleship.Shooter;

namespace Battleship.Test.Shooter
{
    public class CleverishShooterTests : BaseShooterTests
    {
        protected override IShooter CreateShooter()
        {
            return new CleverishShooter();
        }
    }
}
