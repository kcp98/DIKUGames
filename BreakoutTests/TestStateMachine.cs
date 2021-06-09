using Breakout.BreakoutStates;
using NUnit.Framework;
using DIKUArcade.Events;
using System.Collections.Generic;
using DIKUArcade.State;

namespace BreakoutTests {

    [TestFixture]
    public class StateMachineTesting {

        private StateMachine stateMachine;
        private GameEventBus eventBus;

        [SetUp]
        public void InitiateStateMachine() {
            stateMachine = new StateMachine();
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.GameStateEvent
            });
            eventBus.Subscribe(GameEventType.GameStateEvent, stateMachine);
        }
        
        [Test]
        public void TestStateSwitchesAndInterfaceImplementations() {
            Assert.IsInstanceOf<MainMenu>(stateMachine.ActiveState);
            Assert.IsInstanceOf<IGameState>(stateMachine.ActiveState);

            eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GameRunning"
            });
            eventBus.ProcessEventsSequentially();
            Assert.IsInstanceOf<GameRunning>(stateMachine.ActiveState);
            Assert.IsInstanceOf<IGameState>(stateMachine.ActiveState);

            eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GamePaused"
            });
            eventBus.ProcessEventsSequentially();
            Assert.IsInstanceOf<GamePaused>(stateMachine.ActiveState);
            Assert.IsInstanceOf<IGameState>(stateMachine.ActiveState);

            eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GameOver"
            });
            eventBus.ProcessEventsSequentially();
            Assert.IsInstanceOf<GameOver>(stateMachine.ActiveState);
            Assert.IsInstanceOf<IGameState>(stateMachine.ActiveState);

            eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "MainMenu"
            });
            eventBus.ProcessEventsSequentially();
            Assert.IsInstanceOf<MainMenu>(stateMachine.ActiveState);
        }
    }
}