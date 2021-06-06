using NUnit.Framework;
using DIKUArcade.Math;
using Breakout;

namespace BreakoutTests {

    public class PowerUpTests {
        private PowerUp powerUp;
        private Player player;

        [SetUp]
        public void InitiatePowerUp() {
            powerUp = new PowerUp(new Vec2F(0.5f, 0.085f), 2);
            player = new Player();
        }

        [Test]

        public void TestDeletion() {
            powerUp.Move();
            Assert.AreEqual(false, powerUp.IsDeleted());

            for (int i = 0; i < 100; i++)  {
                powerUp.Move();
            }

            Assert.AreEqual(true, powerUp.IsDeleted());
        }

        [Test]
        public void TestCollissions() {
            powerUp.Move();

            powerUp.CheckCollision(player);
            Assert.AreEqual(true, powerUp.IsDeleted());
        }

        [Test]
        public void TestMovement() {
            Vec2F startPos = powerUp.Shape.Position.Copy();
            powerUp.Move();
            Assert.AreEqual(powerUp.Shape.Position.X, startPos.X);
            Assert.Less(powerUp.Shape.Position.Y, startPos.Y);
        }
    }
}