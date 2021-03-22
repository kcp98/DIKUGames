namespace Galaga.GalagaStates {
    public class StateTransformer {
        public static GameStateType TransformStringToState(string state) {
            return GameStateType.GamePaused;
        }

        public static string TransformStateToString(GameStateType state) {
            return "";
        }
    }
}
