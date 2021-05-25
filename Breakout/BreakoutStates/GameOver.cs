using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.Events;
using System;


namespace Breakout.BreakoutStates {
    public class GameOver : IGameState {

        private static GameOver instance = null;

        private Entity background;
        private Text[] menuButtons;
        private int activeButton = 1;

        
        /// <summary> Get the MainMenu instance.
        /// If null then first instantiates the instance. </summary>
        public static GameOver GetGameOver() {
            return GameOver.instance ?? (
                GameOver.instance = new GameOver()
            );
        }

        private GameOver() {
            background = new Entity(
                new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            menuButtons = new Text[3]{
                new Text("SCORE: 0",  new Vec2F(0.3f, 0.3f), new Vec2F(0.5f, 0.5f)),
                new Text("MAIN MENU", new Vec2F(0.3f, 0.2f), new Vec2F(0.5f, 0.5f)),
                new Text("QUIT",      new Vec2F(0.3f, 0.1f), new Vec2F(0.5f, 0.5f))
            };
        }

        public override string ToString() {
            return "Game Over";
        }

        /// <summary> Set a new score. </summary>
        public void SetPoints(int score) {
            menuButtons[0].SetText(string.Format("SCORE: {0}", score));
        }

        /// <summary> Reset the button selection. </summary>
        public void ResetState() {
            activeButton = 1;
        }

        /// <summary> Color the buttons. Active button red. </summary>
        public void UpdateState() {
            foreach (Text item in menuButtons) {
                item.SetColor(System.Drawing.Color.Wheat);
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
                    activeButton = 1;
                    break;
                case KeyboardKey.Down:
                    activeButton = 2;
                    break;
                case KeyboardKey.Enter:
                    if (activeButton == 1) {
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.GameStateEvent,
                            Message   = "MainMenu"
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