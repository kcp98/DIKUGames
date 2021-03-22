namespace Galaga.GalagaStates {
    public enum GameStateType {
        GameRunning,
        GamePaused,
        MainMenu
    }

    public class StateTransformer {
        public static GameStateType TransformStringToState(string state) {
            return GameStateType.GamePaused;
        }

        public static string TransformStateToString(GameStateType state) {
            return "";
        }
    }
}
