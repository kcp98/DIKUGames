using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace GalagaTests {
    [TestFixture]
    public class PlayerTesting {
        private Player player;

        [SetUp]
        public void InitiatePlayer() {
            try {
                GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                    GameEventType.InputEvent, GameEventType.PlayerEvent, GameEventType.GameStateEvent
                });
            } catch {}
            player = new Player(
                new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f)), null
            );
            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

        }

        private void PositionEquals(float x, float y) {
            Vec2F pos = player.GetPosition();
            Assert.AreEqual( // Accounting for floating point precision
                ((int)(1000f *     x), (int)(1000f *     y)),
                ((int)(1000f * pos.X), (int)(1000f * pos.Y))
            );
        }

        private void RegisterMovement(string msg, string arg) {
            GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    EventType  = GameEventType.PlayerEvent,
                    StringArg1 = arg,
                    Message    = msg
                });
            GalagaBus.GetBus().ProcessEventsSequentially();
        }

        [TestCase("KEY_DOWN",   0f, -0.02f)]        
        [TestCase("KEY_LEFT",  -0.02f,  0f)]
        [TestCase("KEY_UP",     0f,  0.02f)]
        [TestCase("KEY_RIGHT",  0.02f,  0f)]
        public void TestMove(string dir, float dx, float dy) {

            RegisterMovement(dir, "KEY_PRESS"); // Starts moving
            player.Move(); player.Move();
            PositionEquals(0.5f + dx, 0.5f + dy);

            RegisterMovement(dir, "KEY_RELEASE"); // Stops moving
            player.Move(); player.Move();
            PositionEquals(0.5f + dx, 0.5f + dy);

            int border = 25;
            if (dir == "KEY_RIGHT" || dir == "KEY_UP") {
                border -= 5;
            }
            RegisterMovement(dir, "KEY_PRESS"); // Reaches border
            for (var i = 0; i < 60; i++) { player.Move(); }
            PositionEquals(0.5f + border * dx, 0.5f + border * dy);
        }
    }
}