using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace Battleships
{
    class Rendering
    {
        string[] consoleLog;
        int[] consoleLogColour;

        public Rendering()
        {
            consoleLogColour = new int[17];
            consoleLog = new string[17];
        }

        /// <summary>
        /// This method draws the main game menu to the screen.
        /// </summary>
        public void DrawMenu()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            DrawTitle();
            Console.SetCursorPosition(6, 10);
            Console.Write("A - Start game");
            Console.SetCursorPosition(6, 12);
            Console.WriteLine("B - Exit");
        }

        /// <summary>
        /// This method draws a fancy title for the menu
        /// </summary>
        private void DrawTitle()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(3, 2);
            Console.Write("░██  ░█ ░███░███░█  ░███ ░██░█░█░█░██");
            Console.SetCursorPosition(3, 3);
            Console.Write("░█░█░█░█ ░█  ░█ ░█  ░█  ░█  ░█░█░█░█░█");
            Console.SetCursorPosition(3, 4);
            Console.Write("░██ ░███ ░█  ░█ ░█  ░██  ░█ ░███░█░██");
            Console.SetCursorPosition(3, 5);
            Console.Write("░█░█░█░█ ░█  ░█ ░█  ░█    ░█░█░█░█░█");
            Console.SetCursorPosition(3, 6);
            Console.Write("░██ ░█░█ ░█  ░█ ░███░███░██ ░█░█░█░█");
            Console.SetCursorPosition(3, 8);
            Console.Write("░█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀");

            for(int y = 9; y < 22; y++)
            {
                Console.SetCursorPosition(3, y);
                Console.WriteLine("░█");
            }

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition(0, 23);
            Console.WriteLine("                                            ");
            Console.SetCursorPosition(0, 24);
            Console.WriteLine("                                            ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(17, 20);
            Console.Write(" _▄ _▄   █████ ▄▬ ▄_  ");
            Console.SetCursorPosition(17, 21);
            Console.Write("▄▄█▄▄█▄█████████▄▄█▄▄▄");
            Console.SetCursorPosition(18, 22);
            Console.Write("▀██████████████████▀");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
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
                    Console.Write(x - 1);
                    Console.SetCursorPosition(1, x + 13);
                    Console.Write(x - 1);
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
                    Console.SetCursorPosition(1, x + 1);
                    Console.Write(x - 1);
                    Console.SetCursorPosition(1, x + 13);
                    Console.Write(x - 1);
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

                        case 7: //Destroyed Ship
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("▒▒");
                            break;

                        case 8: //Previously hit location
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("▒▒");
                            break;

                        default: //Ship
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Write("  ");
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
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.Write("??");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.Write("??");
                                }
                            }
                            else
                            {
                                if (x % 2 == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.Write("??");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
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

        /// <summary>
        /// This method updates the event log in the console.
        /// </summary>
        /// <param name="textToWrite"></param>
        public void UpdateLog(string textToWrite)
        {
            int numberOfLines = 0;
            int stringLength = 0;
            string[] newConsoleLog = new string[17];
            string remainingString;

            Stream updateSound = Battleships.Properties.Resources.Beep;
            SoundPlayer updateSoundPlayer = new SoundPlayer(updateSound);

            stringLength = textToWrite.Length;
            numberOfLines = (int)Math.Ceiling(stringLength / (double)20);
            remainingString = textToWrite;

            //updateSoundPlayer.Play(); //A tad annoying.
            for (int c = 0; c < numberOfLines; c++)
            {

                if (remainingString.Length > 20)
                {
                    newConsoleLog[c] = textToWrite.Substring((c * 20), 20); //take off a 20 character chunk and add it to the log.
                    remainingString = textToWrite.Substring(stringLength - (stringLength - ((c + 1) * 20)), stringLength - ((c + 1) * 20)); //calculate the remaining string to write.
                }
                else
                {
                    newConsoleLog[c] = textToWrite.Substring((c * 20), remainingString.Length); //add the remaining string to the log.
                    for (int count = remainingString.Length; count < 20; count++)
                    {
                        newConsoleLog[c] += " "; //pad the remaining space with spaces (heh) to 'erase' the previous log beneath.
                    }
                }
                consoleLogColour[c] = 1; //white
            }

            int originalCount = 0; //the index in the old log.

            for (int c = numberOfLines; c < 17; c++)
            {
                newConsoleLog[c] = consoleLog[originalCount];
                consoleLogColour[c] = 0; //grey
                originalCount++;
            }

            for (int c = 0; c < 17; c++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(23, 7 + c);
                if (consoleLogColour[c] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    if (c < 10)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                Console.Write(newConsoleLog[c]);
            }
            Array.Copy(newConsoleLog, consoleLog, newConsoleLog.Length);
            Console.SetCursorPosition(0, 0);

            updateSound.Dispose();
            updateSoundPlayer.Dispose();
        }

        /// <summary>
        /// Draws a line to show the X column selected.
        /// </summary>
        /// <param name="x"></param>
        public void DrawTarget(Player player, int x)
        {
            DrawComputerShips(player);
            Console.BackgroundColor = ConsoleColor.Yellow;
            for (int c = 2; c < 12; c++)
            {
                Console.SetCursorPosition((x + 1) * 2 , c);
                Console.Write("  ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Draws a square over the sqaure that has been targeted.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawTarget(Player player, int x, int y)
        {
            DrawComputerShips(player);
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition((x + 1) * 2, 2 + y);
            Console.Write("  ");
        }

        /// <summary>
        /// This method draws a victory screen for the player (0) or the computer (1)
        /// </summary>
        /// <param name="winner"></param>
        public void DrawVictoryScreen(int winner)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition(0, 23);
            Console.WriteLine("                                            ");
            Console.SetCursorPosition(0, 24);
            Console.WriteLine("                                            ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(17, 20);
            Console.Write(" _▄ _▄   █████ ▄▬ ▄_  ");
            Console.SetCursorPosition(17, 21);
            Console.Write("▄▄█▄▄█▄█████████▄▄█▄▄▄");
            Console.SetCursorPosition(18, 22);
            Console.Write("▀██████████████████▀");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, 2);

            if (winner == 0) //Player victory
            {
                Console.Write("You are victorious!");
            }
            else
            {
                Console.Write("Your fleet has been wiped out...");
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);

        }

        /// <summary>
        /// This method draws information such as the remaining ships and the hit chance to the CUI.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="computer"></param>
        /// <param name="turn"></param>
        public void DrawInfoBox(Player player, Computer computer, int turn)
        {
            double hitChance = 0;
            double remainingTargets = 0;
            double remainingSquares = 0;

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(24, 1);
            Console.Write("Remaining Ships:");
            Console.SetCursorPosition(24, 2);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("You: " + player.remainingShips());
            Console.SetCursorPosition(24, 3);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("CPU: " + computer.remainingShips());
            Console.BackgroundColor = ConsoleColor.Black;

            remainingSquares = 100 - turn;
            for (int currentShip = 0; currentShip < 5; currentShip++)
            {
                remainingTargets += computer.ships[currentShip].health;
            }

            hitChance = Math.Round((double)((remainingTargets / remainingSquares) * 100), 1); //makes the hit chance nicer to look at and stops it from scrolling off the CUI.

            Console.SetCursorPosition(24, 4);
            Console.Write("Hit Chance: " + hitChance + "%");
            Console.SetCursorPosition(0, 0);
        }

    }
}
