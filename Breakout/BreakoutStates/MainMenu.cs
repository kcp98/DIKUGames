using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using System;
using System.IO;

namespace Breakout.BreakoutStates {
    public class MainMenu : IGameState {

        private static MainMenu instance;

        private Entity background;
        private Text[] menuButtons;
        private int activeButton = 0;

        /// <summary> Get the MainMenu instance.
        /// If null then first instantiates the instance. </summary>
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
                new Text("NEW GAME", new Vec2F(0.3f, 0.2f), new Vec2F(0.5f, 0.5f)),
                new Text("QUIT",     new Vec2F(0.3f, 0.1f), new Vec2F(0.5f, 0.5f))
            };
        }

        #region IGameState

        /// <summary> Reset the button selection. </summary>
        public void ResetState() {
            activeButton = 0;
        }

        /// <summary> Color the buttons. Active button red. </summary>
        public void UpdateState() {
            foreach (Text menuButton in menuButtons) {
                menuButton.SetColor(System.Drawing.Color.Wheat);
            }
            menuButtons[activeButton].SetColor(System.Drawing.Color.Red);
        }

        /// <summary> Render the background and menu buttons. </summary>
        public void RenderState() {
            background.RenderEntity();
            foreach (Text item in menuButtons) {
                item.RenderText();
            }
        }

        /// <summary> Move or select current button. When this state is active,
        /// then this is passed to the keyhandler of the window. </summary>
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
                            EventType = GameEventType.GameStateEvent,
                            Message   = "GameRunning"
                        });
                    }
                    else
                        Environment.Exit(0);
                    break;
                default: 
                    break;
            }
        }

        #endregion

        /// <summary> Overridden to use for window titles. </summary>
        public override string ToString() {
            return "Main Menu";
        }
    }
}