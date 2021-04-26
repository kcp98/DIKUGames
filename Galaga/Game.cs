using DIKUArcade;
using System.Collections.Generic;
using Galaga.GalagaStates;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;



namespace Galaga {
    // NOTE: We implement the IGameEventProcessor interface!
    public class Game : DIKUGame, IGameEventProcessor { //IGameEventProcessor<object> {
        private StateMachine stateMachine;

        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);
 
            GalagaBus.GetBus().InitializeEventBus(
                new List<GameEventType> {
                    GameEventType.WindowEvent,
                    GameEventType.TimedEvent,
                    GameEventType.InputEvent, 
                    GameEventType.GameStateEvent,
                    GameEventType.PlayerEvent
                }
            );
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);


            // Initializing the StateMachine
            stateMachine = new GalagaStates.StateMachine();
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
        }

        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            stateMachine.ActiveState.HandleKeyEvent(action, key);
        }

        public void ProcessEvent(GameEvent gameEvent) {} 

        public override void Update() {
            GalagaBus.GetBus().ProcessEvents();
            stateMachine.ActiveState.UpdateState();
        }

        public override void Render() {
            stateMachine.ActiveState.RenderState();
        }
    }
}