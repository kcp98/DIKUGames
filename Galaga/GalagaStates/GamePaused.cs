using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.Events;
using DIKUArcade.Input;

namespace Galaga.GalagaStates {
    public class GamePaused : IGameState {
        private static GamePaused instance = null;
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton = 0;
        private const int maxMenuButtons = 2;
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }

        public void InitializeGameState() {
            // The background
            StationaryShape bgShape = new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f));
            IBaseImage bgImage = new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"));
            backGroundImage = new Entity(bgShape, bgImage);

            // The menu buttons.
            Text continueButton = new Text("CONTINUE", new Vec2F(0.33f,0.2f), new Vec2F(0.5f, 0.5f));
            Text mainMenuButton = new Text("MAIN MENU", new Vec2F(0.33f,0.1f), new Vec2F(0.5f, 0.5f));

            continueButton.SetColor(new Vec3I(255, 255, 255));
            mainMenuButton.SetColor(new Vec3I(255, 255, 255));

            menuButtons = new Text[maxMenuButtons]{continueButton, mainMenuButton};
            menuButtons[activeMenuButton].SetColor(new Vec3I(220, 20, 60));
        }

        public void UpdateState() {}

        public void ResetState() {}

        public void RenderState() {
            InitializeGameState();
            backGroundImage.RenderEntity();
            foreach (var item in menuButtons)
            {
                item.RenderText();
            }
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress) {
                switch (key) {
                    case KeyboardKey.Up:
                        activeMenuButton = 0;
                        break;
                    case KeyboardKey.Down:
                        activeMenuButton = 1;
                        break;
                    case KeyboardKey.Enter:
                        if (activeMenuButton == 0) {
                            GalagaBus.GetBus().RegisterEvent(new GameEvent {
                                EventType  = GameEventType.GameStateEvent,
                                StringArg1 = "CHANGE_STATE",
                                Message    = "GAME_RUNNING"
                            });
                        } else {
                            GalagaBus.GetBus().RegisterEvent(new GameEvent {
                                EventType  = GameEventType.GameStateEvent,
                                StringArg1 = "CHANGE_STATE",
                                Message    = "MAIN_MENU"
                            });
                        }
                        break;
                    default: 
                        break;
                }
            }                
        }
    }   
}