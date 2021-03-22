using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga.GalagaStates {
    public class GameRunning : IGameState {
        private static MainMenu instance = null;

        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;
        public static MainMenu GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new MainMenu());
        }

        public void GameLoop() {}


        public void InitializeGameState() {}


        public void UpdateGameLogic() {}


        public void RenderState() {}


        public void HandleKeyEvent(string keyValue, string keyAction) {}
    }
}