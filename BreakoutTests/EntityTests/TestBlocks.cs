using NUnit.Framework;
using DIKUArcade.Math;
using Breakout.Blocks;

namespace BreakoutTests {
    [TestFixture]
    public class BlockTesting {
        private Block block;
        //private GameEventBus eventBus;

        [SetUp]
        public void InitiateBlocks() {
            block = new Block(
                new Vec2F(0.7f, 0.3f),
                new Vec2F(0.7f, 0.3f),
                "green-block.png"
            );
            //eventBus = new GameEventBus();
        }

        [Test]
        public void Test1(){}

        [Test]
        public void Test2(){

            block.GetHit();
            block.GetHit();
            block.GetHit();
            block.GetHit();

            Assert.AreEqual(true, block.IsDeleted());        
        }
    }
}