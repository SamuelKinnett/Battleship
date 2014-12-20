using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Rendering
    {

        public void DrawMenu()
        {
            Console.Clear();
            Console.WriteLine("Main Menu");
            Console.WriteLine();
            Console.WriteLine("A - Start game");
            Console.WriteLine("B - Exit");
        }

        public void DrawGameScreens()
        {
            Console.Clear();
            
        }

        public void DrawGameWindow()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("╔═════════════════════╤════════════════════╗");
            for(int c = 0; c < 10; c++)
            {
                Console.SetCursorPosition(0, c + 1);
                Console.Write("║");
                Console.SetCursorPosition(22, c + 1);
                Console.Write("│");
                Console.SetCursorPosition(43, c + 1);
                Console.WriteLine("║");
            }
            Console.WriteLine("╟─────────────────────┤                    ║");
            for (int c = 0; c < 10; c++)
            {
                Console.SetCursorPosition(0, c + 12);
                Console.Write("║");
                Console.SetCursorPosition(22, c + 12);
                Console.Write("│");
                Console.SetCursorPosition(43, c + 12);
                Console.WriteLine("║");
            }
            Console.WriteLine("╚═════════════════════╧════════════════════╝");
            Console.SetCursorPosition(22, 6);
            Console.WriteLine("├────────────────────╢");

            //write grid assists to the screen

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;

            for (int x = 1; x < 11; x+= 2)
            {
                Console.SetCursorPosition(x * 2, 1);
                Console.Write((char)(64 + x) + " ");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            for (int x = 2; x < 11; x+= 2)
            {
                Console.SetCursorPosition(x * 2, 1);
                Console.Write((char)(64 + x) + " ");
            }

            Console.SetCursorPosition(0, 0);
        }
    }
}
