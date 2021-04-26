using System;
using DIKUArcade.GUI;

namespace Galaga
{
    class Program
    {
        static void Main(string[] args)
        {
            var winArgs = new WindowArgs();
            var game    = new Game(winArgs);
            game.Run();
        }
    }
}
