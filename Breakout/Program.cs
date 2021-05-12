using DIKUArcade.GUI;

namespace Breakout {
    class Program {
        static void Main() {
            var winArgs = new WindowArgs();
            var game    = new Game(winArgs);
            game.Run();
        }
    }
}
