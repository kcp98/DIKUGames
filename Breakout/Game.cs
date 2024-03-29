using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using Breakout.BreakoutStates;
using System.Collections.Generic;

namespace Breakout {
    public class Game : DIKUGame {

        private StateMachine stateMachine;

        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);
            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.PlayerEvent,
                GameEventType.GameStateEvent,
                GameEventType.TimedEvent
            });
            stateMachine = new StateMachine();
            BreakoutBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
        }

        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            stateMachine.ActiveState.HandleKeyEvent(action, key);
        }

        public override void Update() {
            BreakoutBus.GetBus().ProcessEvents();
            stateMachine.ActiveState.UpdateState();
            window.Title = "Breakout - " + stateMachine.ActiveState;
        }

        public override void Render() {
            stateMachine.ActiveState.RenderState();
        }
    }
}