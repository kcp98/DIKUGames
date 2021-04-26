using System;

namespace Galaga.GalagaStates {
    public class StateTransformer {
        public static GameStateType TransformStringToState(string state) {
            string caseSwitch = state;
            GameStateType result;

            switch (caseSwitch) {
                case "GAME_RUNNING":
                    result = GalagaStates.GameStateType.GameRunning;
                    break;
                case "GAME_PAUSED":
                    result = GalagaStates.GameStateType.GamePaused;
                    break;
                case "MAIN_MENU":
                    result = GalagaStates.GameStateType.MainMenu;
                    break;
                default: throw new ArgumentException("Invalid selection of state");
            }
            return result;
        }

        public static string TransformStateToString(GameStateType state) {
            GameStateType caseSwitch = state;
            string result;

            switch (caseSwitch) {
                case GameStateType.GameRunning:
                    result = "GAME_RUNNING";
                    break;
                case GameStateType.GamePaused:
                    result = "GAME_PAUSED";
                    break;
                case GameStateType.MainMenu:
                    result = "MAIN_MENU";
                    break;
                default: throw new ArgumentException("Invalid selection of state");
            }
            return result;
        }
    }
}
