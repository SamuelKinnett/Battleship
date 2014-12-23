using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Game
    {
        Rendering rendering;
        Player player;
        Computer computer;

        public Game()
        {
            rendering = new Rendering();
            player = new Player();
            computer = new Computer();
        }

        public void Run()
        {
            bool programLoop = true; 

            while(programLoop)
            {
                mainMenu();
                gameLoop();
            }
        }

        /// <summary>
        /// This method handles the main menu of the game.
        /// </summary>
        private void mainMenu()
        {
            int userInput;

            bool exitMenu = false;
            rendering.DrawMenu();
            while (!exitMenu)
            {
                userInput = (int)Console.ReadKey(true).Key; //returns the keycode for the key pressed by the user.
                switch (userInput)
                {
                    case (65): //a
                        exitMenu = true;
                        break;

                    case (66): //b
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// This method handles the main game loop
        /// </summary>
        private void gameLoop()
        {
            bool exitGame = false;
            rendering.DrawGameWindow();

            while (!exitGame) //main game loop
            {
                rendering.DrawGameScreens(player);

                Console.ReadLine();
            }
        }
    }
}
