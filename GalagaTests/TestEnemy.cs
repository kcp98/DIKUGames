using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace GalagaTests {
    [TestFixture]
    public class EnemyTesting {
        private Enemy enemy;

        [SetUp]
        public void InitiateEnemy() {
            enemy = new Enemy(
                new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f)),
                null,
                null
            );
        }

        [Test]
        public void TestEnragedAndHit()  {
            Assert.AreEqual(false, enemy.enraged);
            enemy.Hit(true);
            Assert.AreEqual(false, enemy.enraged);
            enemy.Hit(true);
            Assert.AreEqual(true, enemy.enraged);
            enemy.Hit(true);
            Assert.AreEqual(true, enemy.enraged);
            enemy.Hit(true);
        }
    }
}