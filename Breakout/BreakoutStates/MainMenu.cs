using DIKUArcade.State;
using DIKUArcade.Input;


using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.Events;
using System;


namespace Breakout.BreakoutStates {
    public class MainMenu : IGameState {

        private static MainMenu instance = null;

        private Entity background;
        private Text[] menuButtons;
        private int activeButton = 0;

        public static MainMenu GetMainMenu() {
            return MainMenu.instance ?? (
                MainMenu.instance = new MainMenu()
            );
        }

        private MainMenu() {
            background = new Entity(
                new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "BreakoutTitleScreen.png"))
            );
            menuButtons = new Text[2]{
                new Text("NEW GAME", new Vec2F(0.33f, 0.2f), new Vec2F(0.5f, 0.5f)),
                new Text("QUIT",     new Vec2F(0.33f, 0.1f), new Vec2F(0.5f, 0.5f))
            };
        }

        public void ResetState() {
            activeButton = 0;
        }

        public void UpdateState() {
            foreach (Text item in menuButtons) {
                item.SetColor(System.Drawing.Color.Wheat);
            }
            menuButtons[activeButton].SetColor(System.Drawing.Color.Red);
        }

        public void RenderState() {
            background.RenderEntity();
            foreach (Text item in menuButtons) {
                item.RenderText();
            }
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyRelease)
                return;
            switch (key) {
                case KeyboardKey.Up:
                    activeButton = 0;
                    break;
                case KeyboardKey.Down:
                    activeButton = 1;
                    break;
                case KeyboardKey.Enter:
                    if (activeButton == 0) {
                        GameRunning.GetGameRunning().ResetState();
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType  = GameEventType.GameStateEvent,
                            StringArg1 = "CHANGE_STATE",
                            Message    = "GameRunning"
                        });
                    }
                    else
                        Environment.Exit(0);
                    break;
                default: 
                    break;
            }
        }
    }
}