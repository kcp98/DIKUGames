using NUnit.Framework;
using Breakout;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace BreakoutTests {
    [TestFixture]
    public class BlockTesting {
        private Block block;
        //private GameEventBus eventBus;

        [SetUp]
        public void InitiateBlocks() {
            block   = new Block(
                new Vec2F(0.7f, 0.3f),
                new Vec2F(0.7f, 0.3f),
                null,
                3
            );
            //eventBus = new GameEventBus();
        }

        [Test]
        public void Test1(){
            Assert.AreEqual(block.health, 15);
            Assert.AreEqual(block.isHit, false);
            block.GetHit("HIT");
            Assert.AreEqual(block.isHit, true);
            block.GetHit("HITT");
            Assert.AreEqual(block.isHit, false);
            block.GetHit("HIT");
            block.Damage();
            Assert.AreEqual(block.health, 14);
            Assert.AreEqual(block.isHit, false);
        }
    }
}