using Galaga.GalagaStates;
using Galaga;
using NUnit.Framework;
using DIKUArcade.Events;
using System.Collections.Generic;

namespace GalagaTests {

    [TestFixture]
    public class StateMachineTesting {

        private StateMachine stateMachine;

        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
            // (1) Initialize a GalagaBus with proper GameEventTypes 
            try {
                GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                    GameEventType.InputEvent, GameEventType.PlayerEvent, GameEventType.GameStateEvent
                });
            } catch {}
            // (2) Instantiate the StateMachine
            stateMachine = new StateMachine();
            // (3) Subscribe the GalagaBus to proper GameEventTypes // and GameEventProcessors
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
        }

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
        
        [Test]
        public void TestEventGamePaused() {GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    EventType  = GameEventType.GameStateEvent,
                    StringArg1 = "CHANGE_STATE",
                    Message    = "GAME_PAUSED"
                });
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        [Test]
        public void TestEventMainMenu() {
            GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    EventType  = GameEventType.GameStateEvent,
                    StringArg1 = "CHANGE_STATE",
                    Message    = "MAIN_MENU"
                });
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
        
        [Test]
        public void TestEventGameRunning() {
            GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    EventType  = GameEventType.GameStateEvent,
                    StringArg1 = "CHANGE_STATE",
                    Message    = "MAIN_MENU"
                });
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.Not.InstanceOf<GameRunning>());
        }


    }
}