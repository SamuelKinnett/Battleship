using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 44;
            Console.WindowHeight = 23;
            Console.Title = "Battleship";
            Console.CursorVisible = false;

            Game game = new Game();

            game.Run();
        }
    }
}
