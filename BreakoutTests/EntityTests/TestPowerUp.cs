using NUnit.Framework;
using DIKUArcade.Events;
using Breakout.BreakoutStates; 
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using Breakout;

namespace BreakoutTests {

    public class PowerUpTests {
        private Entity entity;
        private PowerUp powerUp;

        [SetUp]
        public void InitiatePowerUp() {
            powerUp = new PowerUp(new Vec2F(0f, 0.005f),2);
            entity = new Entity( new DynamicShape(
                new Vec2F(0,0),
                new Vec2F(1f,0)
            ),null);
        }

        [Test]

        public void TestDeletion() {
            powerUp.Move();
            Assert.AreEqual(false, powerUp.IsDeleted());

            powerUp.Move();
            Assert.AreEqual(true, powerUp.IsDeleted());
        }

        [Test]
        public void TestCollissions() {
            powerUp.Move();

            powerUp.CheckCollision(entity);
            Assert.AreEqual(true, powerUp.IsDeleted());
        }
    }
}