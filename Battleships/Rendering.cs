using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Rendering
    {

        /// <summary>
        /// This method draws the main game menu to the screen.
        /// </summary>
        public void DrawMenu()
        {
            Console.Clear();
            Console.WriteLine("Main Menu");
            Console.WriteLine();
            Console.WriteLine("A - Start game");
            Console.WriteLine("B - Exit");
        }

        /// <summary>
        /// This method draws the computer and player grids to the screen.
        /// </summary>
        /// <param name="player"></param>
        public void DrawGameScreens(Player player)
        {
            DrawComputerShips(player);
            DrawPlayerShips(player);
        }

        /// <summary>
        /// This method draws the window for the CUI to the screen. This includes the status box, grids and instruction box.
        /// </summary>
        public void DrawGameWindow()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("╔═════════════════════╤════════════════════╗");
            for(int c = 0; c < 11; c++)
            {
                Console.SetCursorPosition(0, c + 1);
                Console.Write("║");
                Console.SetCursorPosition(22, c + 1);
                Console.Write("│");
                Console.SetCursorPosition(43, c + 1);
                Console.WriteLine("║");
            }
            Console.WriteLine("╟─────────────────────┤                    ║");
            for (int c = 0; c < 11; c++)
            {
                Console.SetCursorPosition(0, c + 13);
                Console.Write("║");
                Console.SetCursorPosition(22, c + 13);
                Console.Write("│");
                Console.SetCursorPosition(43, c + 13);
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
                Console.SetCursorPosition(x * 2, 13);
                Console.Write((char)(64 + x) + " ");
                if(x % 2 == 1)
                {
                    Console.SetCursorPosition(1, x + 1);
                    Console.Write(x);
                    Console.SetCursorPosition(1, x + 13);
                    Console.Write(x);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            for (int x = 2; x < 11; x+= 2)
            {
                Console.SetCursorPosition(x * 2, 1);
                Console.Write((char)(64 + x) + " ");
                Console.SetCursorPosition(x * 2, 13);
                Console.Write((char)(64 + x) + " ");
                if (x % 2 == 0)
                {
                    if (x == 10)
                    {
                        Console.SetCursorPosition(1, x + 1);
                        Console.Write("A");
                        Console.SetCursorPosition(1, x + 13);
                        Console.Write("A");
                    }
                    else
                    {
                        Console.SetCursorPosition(1, x + 1);
                        Console.Write(x);
                        Console.SetCursorPosition(1, x + 13);
                        Console.Write(x);
                    }
                }
            }

            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// This method draws the player's grid to the CUI.
        /// </summary>
        /// <param name="player"></param>
        private void DrawPlayerShips(Player player)
        {
            int[,] renderMap = player.map;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Console.SetCursorPosition((x * 2) + 2, y + 14);
                    switch(renderMap[x, y])
                    {
                        case 0: //Ocean

                            //render in a checkerboard pattern

                            if (y % 2 == 1)
                            {
                                if(x % 2 == 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    Console.Write("▒▒");
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.Blue;
                                    Console.Write("  ");
                                }
                            }
                            else
                            {
                                if(x % 2 == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    Console.Write("▒▒");
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.Blue;
                                    Console.Write("  ");
                                }
                            }
                            break;

                        case 1: //Ship
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Write("  ");
                            break;

                        case 2: //Destroyed Ship
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("▒▒");
                            break;

                        case 3: //Previously hit location
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("▒▒");
                            break;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// This method draws the enemy player grid to the CUI.
        /// </summary>
        /// <param name="player"></param>
        private void DrawComputerShips(Player player)
        {
            int[,] renderMap = player.enemyMap;

            for(int y = 0; y < 10; y++)
            {
                for(int x = 0; x < 10; x++)
                {
                    Console.SetCursorPosition((x * 2) + 2, y + 2);
                    switch(renderMap[x, y])
                    {
                        case 0: //Unknown

                            //Draw in a checkerboard pattern

                            if (y % 2 == 1)
                            {
                                if (x % 2 == 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    Console.Write("??");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.Blue;
                                    Console.Write("??");
                                }
                            }
                            else
                            {
                                if (x % 2 == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    Console.Write("??");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.Blue;
                                    Console.Write("??");
                                }
                            }
                            break;

                        case 1: //Empty Ocean
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("▒▒");
                            break;

                        case 2: //Ship
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("▒▒");
                            break;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// This method draws a preview of the ship that is being placed in yellow.
        /// </summary>
        /// <param name="oldX"></param>
        /// <param name="oldY"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="length"></param>
        /// <param name="vertical"></param>
        public void DrawShipPlacement(int oldX, int oldY, int x, int y, int length, bool vertical)
        {
            if (vertical)
            {
                for (int c = 2; c < length + 2; c++)
                {
                    Console.SetCursorPosition((oldX * 2) + 2, oldY + length);
                    if (oldY + length % 2 == 1)
                    {
                        if (oldX % 2 == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("▒▒");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("  ");
                        }
                    }
                    else
                    {
                        if (oldX % 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("▒▒");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("  ");
                        }
                    }
                }
                for (int c = 2; c < length + 2; c++)
                {
                    Console.SetCursorPosition(x + 2, y + length);
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  ");
                }
            }
            else
            {
                for (int c = 0; c < length; c++)
                {
                    if (oldY % 2 == 1)
                    {
                        if (oldX % 2 == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("▒▒");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("  ");
                        }
                    }
                    else
                    {
                        if (oldX % 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("▒▒");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("  ");
                        }
                    }
                }
                for (int c = 2; c < length + 2; c++)
                {
                    Console.SetCursorPosition(x + length, y + 2);
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  ");
                }
            }

        }

        public void DrawShipPlacement(int x, int y, int length, bool vertical)
        {
            if (vertical)
            {
                for (int c = 14; c < length + 14; c++)
                {
                    Console.SetCursorPosition((x * 2) + 2, y + c);
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  ");
                }
            }
            else
            {
                for (int c = 2; c < length + 2; c++)
                {
                    Console.SetCursorPosition((x + (c - 1)) * 2, y + 14);
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  ");
                }
            }

        }

    }
}
