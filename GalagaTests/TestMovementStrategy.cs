using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using System.Collections.Generic;
using Galaga.MovementStrategy;

namespace GalagaTests {
    [TestFixture]
    public class MovementStrategiesTesting {
        private IMovementStrategy down;
        private IMovementStrategy noMove;
        private IMovementStrategy zigZag;
        private Enemy enemy;
        private EntityContainer<Enemy> enemies;


        [SetUp]
        public void InitiateStrategies() {
            down   = new Down();
            noMove = new NoMove();
            zigZag = new ZigZagDown();

            enemy = new Enemy(
                new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f)),
                null, null
            );
            enemies =  new EntityContainer<Enemy>(2);
            enemies.AddEntity(
                new Enemy(
                    new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f)),
                    null, null
                )
            );
            enemies.AddEntity(
                new Enemy(
                    new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f)),
                    null, null
                )
            );
        }

        [TestCase("down",   true)]
        [TestCase("noMove", false)]
        [TestCase("zigZag", true)]
        public void TestMoveEnemy(string strategy, bool shouldMove) {
            switch (strategy) {
                case "down":
                    down.MoveEnemy(enemy);
                    break;
                case "noMove":
                    noMove.MoveEnemy(enemy);
                    break;
                case "zigZag":
                    zigZag.MoveEnemy(enemy);
                    break;                    
            }
            Vec2F newPos = enemy.Shape.Position;
            if (shouldMove) {
                Assert.AreNotEqual(
                    ((int)(1000f *    0.5f), (int)(1000f *    0.5f)),
                    ((int)(10000f * newPos.X), (int)(1000f * newPos.Y))
                );
            } else {
                Assert.AreEqual(
                    ((int)(1000f *   0.5f), (int)(10000f *    0.5f)),
                    ((int)(1000f * newPos.X), (int)(10000f * newPos.Y))
                );
            }
        }


        [TestCase("down",   true)]
        [TestCase("noMove", false)]
        [TestCase("zigZag", true)]
        public void TestMoveEnemies(string strategy, bool shouldMove) {
            switch (strategy) {
                case "down":
                    down.MoveEnemies(enemies);
                    break;
                case "noMove":
                    noMove.MoveEnemies(enemies);
                    break;
                case "zigZag":
                    zigZag.MoveEnemies(enemies);
                    break;                    
            }
            enemies.Iterate(
                enemy => {
                    Vec2F newPos = enemy.Shape.Position;
                    if (shouldMove) {
                        Assert.AreNotEqual(
                            ((int)(1000f *    0.5f), (int)(1000f *    0.5f)),
                            ((int)(1000f * newPos.X), (int)(1000f * newPos.Y))
                        );
                    } else {
                        Assert.AreEqual(
                            ((int)(1000f *   0.5f), (int)(1000f *    0.5f)),
                            ((int)(1000f * newPos.X), (int)(1000f * newPos.Y))
                        );
                    }
                }
            );
        }
    }
}