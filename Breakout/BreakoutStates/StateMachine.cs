using DIKUArcade.Events;
using DIKUArcade.State;
using DIKUArcade.Timers;

namespace Breakout.BreakoutStates {
    public class StateMachine : IGameEventProcessor {

        public IGameState ActiveState { get; private set; } 

        public StateMachine() {
            ActiveState = MainMenu.GetMainMenu();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.Message) {
                case "GameRunning":
                    StaticTimer.ResumeTimer();
                    ActiveState = GameRunning.GetGameRunning();
                    break;
                case "GamePaused":
                    StaticTimer.PauseTimer();
                    GamePaused.GetGamePaused().ResetState();
                    ActiveState = GamePaused.GetGamePaused();
                    break;
                case "MainMenu":
                    ActiveState = MainMenu.GetMainMenu();
                    break;
                case "GameOver":
                    GameOver.GetGameOver().ResetState();
                    ActiveState = GameOver.GetGameOver();
                    break;
                default:
                    break;
            }
        }
    }
}