using NUnit.Framework;
using Breakout;
using DIKUArcade.Events;
using DIKUArcade.Entities;

namespace BreakoutTests {
    [TestFixture]
    public class PlayerTesting {
        Player player;

        [SetUp]
        public void InitiatePlayer() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();   
            player = new Player();
        }

        [Test]
        // Testing that the player starts in the bottom-center of the screen.
        public void InitialPositionTest() {
            Assert.LessOrEqual(player.Shape.Position.X, 0.5f);
            Assert.GreaterOrEqual(
                player.Shape.Position.X + player.Shape.Extent.X, 0.5f);
            Assert.LessOrEqual(player.Shape.Position.Y, 0.5f);    
        }

        // Testing that the player can move using the right/left arrows or the a/d keys.
        [Test]
        public void TestMoveRight() {  
            player.ProcessEvent(new GameEvent {
                EventType  = GameEventType.PlayerEvent,
                StringArg1 = "KeyPress",
                Message    = "Right"
            });
            float before = player.Shape.Position.X;
            player.Move();
            Assert.Greater(player.Shape.Position.X, before);
        }
        [Test]
        // Testing that the player can move using the right/left arrows or the a/d keys.
        public void TestMoveLeft() {        
            player.ProcessEvent(new GameEvent {
                EventType  = GameEventType.PlayerEvent,
                StringArg1 = "KeyPress",
                Message    = "Left"
            });
            float before = player.Shape.Position.X;
            player.Move();
            Assert.Less(player.Shape.Position.X, before);
        }
        
        [Test] 
        // Testing that the player cannot leave the screen.
        public void TestLeftSide() {  
            player.ProcessEvent(new GameEvent {
                EventType  = GameEventType.PlayerEvent,
                StringArg1 = "KeyPress",
                Message    = "Left"
            });
            for (int i = 0; i < 100; i++) {
                player.Move();
            }
            Assert.GreaterOrEqual(player.Shape.Position.X, 0f);
        }

        [Test] 
        // Testing that the player cannot leave the screen.
        public void TestRightSide() {  
            player.ProcessEvent(new GameEvent {
                EventType  = GameEventType.PlayerEvent,
                StringArg1 = "KeyPress",
                Message    = "Right"
            });
            for (int i = 0; i < 100; i++) {
                player.Move();
            }   
            Assert.LessOrEqual(player.Shape.Position.X, 1f);
        }
        
        [Test]
        public void TestWidePowerUp() {
            Assert.AreEqual(0.15, player.mutableXtent, 0.1);
            //Player size after the powerup.
            player.WidenPlayer();
            Assert.AreEqual(0.225,player.mutableXtent, 0.1);
        }

        [Test]
        // Testing that the player is an entity and is rectangular
        public void ShapeTest() {
            Assert.IsInstanceOf<Entity>(player);
            Assert.Greater(player.Shape.Extent.X, player.Shape.Extent.Y);
        }
    }
}