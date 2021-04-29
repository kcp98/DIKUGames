using NUnit.Framework;
using Breakout;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace BreakoutTests {
    [TestFixture]
    public class PlayerTesting {
        Player player;
        GameEventBus eventBus;

        float initialPositionX = 0.35f;

        float movementSpeed = Breakout.Player.movementSpeed;

        [SetUp]
        public void InitiatePlayer() {      
            player   = new Player(null);
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(
                new List<GameEventType> {
                    GameEventType.PlayerEvent});
            eventBus.Subscribe(GameEventType.PlayerEvent, player);
        }

        [Test]
        // Testing that the player starts in the bottom-center of the screen.
        public void InitialPositionTest() {
            Assert.AreEqual(player.Shape.Position.X, initialPositionX);
            Assert.True((player.Shape.Position.Y - 0.05f) < 1E-10f);    
        }

        // Testing that the player can move using the right/left arrows or the a/d keys.
        [Test]
        public void TestMoveRight() {  
            player.ProcessEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = "PRESS",
                        Message    = "RIGHT"
                    });
            player.Move();
            Assert.True(player.Shape.Position.X - (initialPositionX + movementSpeed) < 1E-10);
        }
        [Test]
        // Testing that the player can move using the right/left arrows or the a/d keys.
        public void TestMoveLeft() {        
            player.ProcessEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = "PRESS",
                        Message    = "LEFT"
                    });
            player.Move();
            Assert.True((initialPositionX - movementSpeed) - player.Shape.Position.X < 1E-10);
        }
        
        [Test] 
        // Testing that the player cannot leave the screen.
        public void TestLeftSide() {  
            for (int i = 0; i < 100; i++)
            {
                player.ProcessEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = "PRESS",
                        Message    = "LEFT"
                    });
                player.Move();
            }   
    
            Assert.True((player.Shape.Position.X >= 0f)
                 && (0.0f - player.Shape.Position.X < 1E-10));
        }
        [Test] 
        // Testing that the player cannot leave the screen.
        public void TestRightSide() {  
            for (int i = 0; i < 100; i++)
            {
                player.ProcessEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = "PRESS",
                        Message    = "RIGHT"
                    });
                player.Move();
            }   
            Assert.True(0.7f - player.Shape.Position.X < 1E-10);
        }
    }

}