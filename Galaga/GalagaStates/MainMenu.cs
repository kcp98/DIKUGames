using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.EventBus;
using System;

namespace Galaga.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton = 0;
        private const int maxMenuButtons = 2;
        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        public MainMenu () {
            InitializeGameState();
        }
        public void GameLoop() {}

        public void UpdateGameLogic() {}

        public void InitializeGameState() {
                // The background of the menu.
                StationaryShape bgShape = new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f));
                IBaseImage bgImage = new Image(Path.Combine("Assets", "Images", "TitleImage.png"));
                backGroundImage = new Entity(bgShape, bgImage);

                // The menu buttons.
                Text newGameButton = new Text("NEW GAME", new Vec2F(0.33f,0.2f), new Vec2F(0.5f, 0.5f));
                Text quitButton = new Text("QUIT", new Vec2F(0.33f,0.1f), new Vec2F(0.5f, 0.5f));

                newGameButton.SetColor(new Vec3I(255, 255, 255));
                quitButton.SetColor(new Vec3I(255, 255, 255));

                menuButtons = new Text[maxMenuButtons]{newGameButton, quitButton};
                menuButtons[activeMenuButton].SetColor(new Vec3I(220, 20, 60));
        }


        public void RenderState() {
            InitializeGameState();
            backGroundImage.RenderEntity();
            foreach (var item in menuButtons) {
                item.RenderText();
            }
        }


        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                    case "KEY_UP":
                        if ( activeMenuButton != 0) {
                             activeMenuButton--;
                        }
                        break;
                    case "KEY_DOWN":
                        if ( activeMenuButton != 1) {
                                activeMenuButton++;
                            }
                        break;
                    case "KEY_ENTER":
                        if (menuButtons[activeMenuButton]==menuButtons[0]) {
                            GameRunning.GetInstance().newGame();
                            GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors( 
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_RUNNING", ""));
                        }
                        else if (menuButtons[activeMenuButton]==menuButtons[1]) {
                            Environment.Exit(0);
                        }
                        break;
                    default: 
                        break;
                }        
            }
        }
    }
}