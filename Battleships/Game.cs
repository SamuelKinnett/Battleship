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
        public int turn;

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
                MainMenu();
                soundPlayer.Play();
                GameLoop();
            }
        }

        /// <summary>
        /// This method handles the main menu of the game.
        /// </summary>
        private void MainMenu()
        {
            SoundPlayer soundPlayer;
            Stream mainMenuTheme = Battleships.Properties.Resources.Battleships;
            int userInput;

            soundPlayer = new SoundPlayer(mainMenuTheme);
            soundPlayer.PlayLooping();
            bool exitMenu = false;
            rendering.DrawMenu();
            Console.BackgroundColor = ConsoleColor.Black;
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
        private void GameLoop()
        {
            Stream endMusic;
            SoundPlayer soundPlayer;
            bool exitGame = false;
            rendering.DrawGameWindow();
            rendering.DrawGameScreens(player);
            player.PlaceShips(rendering);
            computer.PlaceShips();
            turn = 0;
            while (!exitGame) //main game loop
            {
                rendering.DrawGameScreens(player);
                rendering.DrawInfoBox(player, computer, turn);
                player.TakeShot(computer, rendering);
                System.Threading.Thread.Sleep(1000);
                rendering.DrawGameScreens(player);
                computer.TakeShot(player, rendering);
                System.Threading.Thread.Sleep(1000);
                if (player.AllShipsDestroyed())
                {
                    endMusic = Battleships.Properties.Resources.BattleshipsLoss;
                    soundPlayer = new SoundPlayer(endMusic);
                    soundPlayer.Play();
                    rendering.DrawVictoryScreen(1);
                    Console.ReadLine();
                    soundPlayer.Stop();
                    endMusic.Dispose();
                    soundPlayer.Dispose();
                    exitGame = true;
                }
                if (computer.AllShipsDestroyed())
                {
                    endMusic = Battleships.Properties.Resources.BattleshipsVictory;
                    soundPlayer = new SoundPlayer(endMusic);
                    soundPlayer.Play();
                    rendering.DrawVictoryScreen(0);
                    Console.ReadLine();
                    soundPlayer.Stop();
                    endMusic.Dispose();
                    soundPlayer.Dispose();
                    exitGame = true;
                }
                turn++;
            }
        }

    }
}
