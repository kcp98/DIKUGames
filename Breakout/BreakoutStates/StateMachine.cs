using DIKUArcade.Events;
using DIKUArcade.State;

namespace Breakout.BreakoutStates {
    public class StateMachine : IGameEventProcessor {

        public IGameState ActiveState { get; private set; } 

        public StateMachine() {
            ActiveState = MainMenu.GetMainMenu();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            SwitchState(gameEvent.Message);
        }

        public void SwitchState(string state) {
            switch (state) {
                case "GameRunning":
                    ActiveState = GameRunning.GetGameRunning();
                    break;
                case "GamePaused":
                    ActiveState = GamePaused.GetGamePaused();
                    break;
                case "MainMenu":
                    ActiveState = MainMenu.GetMainMenu();
                    break;
                default:
                    break;
            }
        }
    }
}