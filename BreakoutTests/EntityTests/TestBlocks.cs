using NUnit.Framework;
using Breakout;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace GalagaTests {
    [TestFixture]
    public class BlockTesting {
        private Blocks block;
        //private GameEventBus eventBus;

        [SetUp]
        public void InitiatePlayer() {
            block   = new Blocks(new Vec2F(0.7f, 0.3f), null, 3);
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