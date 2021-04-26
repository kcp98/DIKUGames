using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;

namespace Breakout {
    public class Game : DIKUGame {

        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);
        }

        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            window.CloseWindow();
        }

        public override void Update() {}

        public override void Render() {}
    }
}