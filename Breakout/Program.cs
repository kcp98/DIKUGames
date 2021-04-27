using DIKUArcade.GUI;

namespace Breakout
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Console.WriteLine("Hello: {0}", args);
            var winArgs = new WindowArgs();
            var game    = new Game(winArgs);
            game.Run();
        }
    }
}
