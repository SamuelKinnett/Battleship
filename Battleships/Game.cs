using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

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
            Stream exitMenuSound = Battleships.Properties.Resources.Missile;
            SoundPlayer soundPlayer;
            bool programLoop = true;

            soundPlayer = new SoundPlayer(exitMenuSound);
            while(programLoop)
            {
                mainMenu();
                soundPlayer.Play();
                gameLoop();
            }
        }

        /// <summary>
        /// This method handles the main menu of the game.
        /// </summary>
        private void mainMenu()
        {
            SoundPlayer soundPlayer;
            Stream mainMenuTheme = Battleships.Properties.Resources.Battleships;
            int userInput;

            soundPlayer = new SoundPlayer(mainMenuTheme);
            soundPlayer.PlayLooping();
            bool exitMenu = false;
            rendering.DrawMenu();
            while (!exitMenu)
            {
                userInput = (int)Console.ReadKey(true).Key; //returns the keycode for the key pressed by the user.
                switch (userInput)
                {
                    case (65): //a
                        exitMenu = true;
                        soundPlayer.Stop();
                        break;

                    case (66): //b
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }

            mainMenuTheme.Dispose();
            soundPlayer.Dispose();
        }

        /// <summary>
        /// This method handles the main game loop
        /// </summary>
        private void gameLoop()
        {
            bool exitGame = false;
            rendering.DrawGameWindow();
            rendering.DrawGameScreens(player);
            player.PlaceShips(rendering);
            computer.PlaceShips();
            while (!exitGame) //main game loop
            {
                if(player.AllShipsDestroyed())
                {
                    ComputerVictory();
                }
                if(computer.AllShipsDestroyed())
                {
                    PlayerVictory();
                }
                rendering.DrawGameScreens(player);
                player.TakeShot(computer, rendering);
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void PlayerVictory()
        {

        }

        private void ComputerVictory()
        {

        }
    }
}
