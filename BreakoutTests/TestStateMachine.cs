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

            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GameRunning"
            });
            BreakoutBus.GetBus().ProcessEventsSequentially();
            Assert.IsInstanceOf<GameRunning>(stateMachine.ActiveState);

            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GamePaused"
            });
            BreakoutBus.GetBus().ProcessEventsSequentially();
            Assert.IsInstanceOf<GamePaused>(stateMachine.ActiveState);
        }
    }
}