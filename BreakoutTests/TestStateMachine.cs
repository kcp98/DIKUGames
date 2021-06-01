using Breakout.BreakoutStates;
using Breakout;
using NUnit.Framework;
using DIKUArcade.Events;
using System.Collections.Generic;

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
        public void TestStateSwitches() {
            Assert.IsInstanceOf<MainMenu>(stateMachine.ActiveState);

            eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GameRunning"
            });
            eventBus.ProcessEventsSequentially();
            Assert.IsInstanceOf<GameRunning>(stateMachine.ActiveState);

            eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GamePaused"
            });
            eventBus.ProcessEventsSequentially();
            Assert.IsInstanceOf<GamePaused>(stateMachine.ActiveState);

            eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GameOver"
            });
            eventBus.ProcessEventsSequentially();
            Assert.IsInstanceOf<GameOver>(stateMachine.ActiveState);

            eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "MainMenu"
            });
            eventBus.ProcessEventsSequentially();
            Assert.IsInstanceOf<MainMenu>(stateMachine.ActiveState);


        }
    }
}