using NUnit.Framework;
using Breakout;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace BreakoutTests {
    [TestFixture]
    public class BallTesting {
        Ball ball;

        [SetUp]
        public void InitiateBall() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
            ball = new Ball();
        }

        [Test]
        public void TestCollissions() {
            for (int i = 0; i < 11; i++ ) {
                ball = new Ball();
                ball.Move();
                Vec2F dir = ball.Shape.AsDynamicShape().Direction.Copy();
                Entity entity = new Entity(
                    new DynamicShape(
                        new Vec2F(-0.085f + i * 0.01f, 0.115f),
                        new Vec2F(0.1f, 0.1f)
                    ), null
                );
                // Ball should register collision
                Assert.True(ball.CheckCollision(entity));

                // Ball should change direction upon collision
                Assert.AreNotEqual(ball.Shape.AsDynamicShape().Direction, dir);
                
                // Speed should not change
                Assert.LessOrEqual(ball.Shape.AsDynamicShape().Direction.Length(),    0.0101f);
                Assert.GreaterOrEqual(ball.Shape.AsDynamicShape().Direction.Length(), 0.0099f);
                
                // No velocity should exist solely on one axis
                Assert.AreNotEqual(ball.Shape.AsDynamicShape().Direction.X, 0f);
                Assert.AreNotEqual(ball.Shape.AsDynamicShape().Direction.Y, 0f);
            }
        }
    }
}