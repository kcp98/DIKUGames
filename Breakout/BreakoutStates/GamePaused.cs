using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.Events;


namespace Breakout.BreakoutStates {
    public class GamePaused : IGameState {

        private static GamePaused instance;

        private Entity background;
        private Text[] menuButtons;
        private int activeButton = 0;

        /// <summary> Get the GamePaused instance.
        /// If null then first instantiates the instance. </summary>
        public static GamePaused GetGamePaused() {
            return GamePaused.instance ?? (
                GamePaused.instance = new GamePaused()
            );
        }

        private GamePaused() {
            background = new Entity(
                new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            menuButtons = new Text[2]{
                new Text("MAIN MENU",   new Vec2F(0.33f, 0.2f), new Vec2F(0.5f, 0.5f)),
                new Text("RESUME GAME", new Vec2F(0.33f, 0.1f), new Vec2F(0.5f, 0.5f))
            };

        }

        /// <summary> Reset the button selection. </summary>
        public void ResetState() {
            activeButton = 0;
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
                    activeButton = 0;
                    break;
                case KeyboardKey.Down:
                    activeButton = 1;
                    break;
                case KeyboardKey.Enter:
                    string state = "MainMenu";
                    if (activeButton == 1)
                        state = "GameRunning";
                        
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.GameStateEvent,
                        Message   = state
                    });
                    break;
                default: 
                    break;
            }
        }
    }
}