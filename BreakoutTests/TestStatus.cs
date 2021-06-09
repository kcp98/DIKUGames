using NUnit.Framework;
using Breakout;

namespace BreakoutTests {
    [TestFixture]
    public class StatusTest {

        [Test]
        public void TestPoints() {
            Assert.AreEqual(0, Status.GetStatus().points);

            Status.GetStatus().AddPoints(-2);
            Assert.AreEqual(0, Status.GetStatus().points);

            Status.GetStatus().AddPoints(2);
            Assert.AreEqual(2, Status.GetStatus().points);

            Status.GetStatus().Reset();
            Assert.AreEqual(0, Status.GetStatus().points);
        }

        [Test]
        public void TestLives() {
            Status.GetStatus().Reset();
            Assert.AreEqual(3, Status.GetStatus().lives);

            Status.GetStatus().ExtraLife();
            Assert.AreEqual(4, Status.GetStatus().lives);

            Status.GetStatus().GetLife();
            Assert.AreEqual(3, Status.GetStatus().lives);
        }
    }
}