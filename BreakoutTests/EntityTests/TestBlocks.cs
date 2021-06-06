using NUnit.Framework;
using DIKUArcade.Math;
using Breakout.Blocks;
using DIKUArcade.Entities;

namespace BreakoutTests {
    [TestFixture]
    public class BlockTesting {
        private Block defaultBlock;
        private Unbreakable unbreakableBlock;
        private Hardened hardendBlock;

        [SetUp]
        public void InitiateBlocks() {}

        [Test]
        public void BlockNotHit(){
            defaultBlock = new Block(
                new Vec2F(0.7f, 0.3f),
                new Vec2F(0.7f, 0.3f),
                "green-block.png");

            Assert.AreEqual(false, defaultBlock.IsDeleted());        
        }

        [Test]
        public void DestroyingDefaul(){

            defaultBlock = new Block(
                new Vec2F(0.7f, 0.3f),
                new Vec2F(0.7f, 0.3f),
                "green-block.png");

            defaultBlock.GetHit();

            Assert.AreEqual(true, defaultBlock.IsDeleted());        
        }

        [Test]
        public void HardenedBlockOneHit(){
            hardendBlock = new Hardened(
                new Vec2F(0.7f, 0.3f),
                new Vec2F(0.7f, 0.3f),
                "green-block.png"
            );
                
            hardendBlock.GetHit();

            Assert.AreEqual(false, hardendBlock.IsDeleted());        
        }
        [Test]
        public void HardenedBlockTwoHits(){
            hardendBlock = new Hardened(
                new Vec2F(0.7f, 0.3f),
                new Vec2F(0.7f, 0.3f),
                "green-block.png"
            );
                
            hardendBlock.GetHit();
            hardendBlock.GetHit();

            Assert.AreEqual(true, hardendBlock.IsDeleted());        
            
        }

        [Test]
        public void UnbreakableBlock(){
            unbreakableBlock = new Unbreakable(
                new Vec2F(0.7f, 0.3f),
                new Vec2F(0.7f, 0.3f),
                "green-block.png"
            );
                
            for (int i = 0; i < 100; i++)
            {
                unbreakableBlock.GetHit();
            }

            Assert.AreEqual(false, unbreakableBlock.IsDeleted());        
        }

        [Test]
        public void BlockIsEntity(){

            defaultBlock = new Block(
                new Vec2F(0.7f, 0.3f),
                new Vec2F(0.7f, 0.3f),
                "green-block.png");

            defaultBlock.GetHit();

            Assert.IsInstanceOf<Entity>(defaultBlock);        
        }
    }
}