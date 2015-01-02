using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace Battleships
{
    class Player
    {
        public int[,] map {get; set;}
        public int[,] enemyMap {get; set;} //the map of the enemy fleet known to the player. 
        Battleship[] ships;

        public Player()
        {
            map = new int[10, 10];
            enemyMap = new int[10, 10];
            ships = new Battleship[5];

            Array.Clear(map, 0, map.Length);
            Array.Clear(enemyMap, 0, enemyMap.Length);

            ships[0] = new Battleship("Aircraft Carrier", 5);
            ships[1] = new Battleship("Battleship", 4);
            ships[2] = new Battleship("Submarine", 3);
            ships[3] = new Battleship("Cruiser", 3);
            ships[4] = new Battleship("Patrol Boat", 2);
        }

        /// <summary>
        /// This method allows the player to place their ships in their grid.
        /// </summary>
        public void PlaceShips(Rendering rendering)
        {
            int shipLength;
            int shipOldX;
            int shipOldY;
            int shipX;
            int shipY;
            int userInput;
            bool shipPlaced;
            bool vertical;

            Stream beep = Battleships.Properties.Resources.SelectionSound;
            Stream select = Battleships.Properties.Resources.SelectionConfirm;
            Stream error = Battleships.Properties.Resources.Error;
            SoundPlayer beepPlayer = new SoundPlayer(beep);
            SoundPlayer selectPlayer = new SoundPlayer(select);
            SoundPlayer errorPlayer = new SoundPlayer(error);

            for(int shipNumber = 0; shipNumber < 5; shipNumber++)
            {
                shipLength = ships[shipNumber].length;
                shipX = 0;
                shipY = 0;
                vertical = true;

                shipPlaced = false;

                Console.BackgroundColor = ConsoleColor.Black;
                //Console.SetCursorPosition(24, 2);
                //Console.Write("Place:");
                //Console.SetCursorPosition(24, 3);
                //Console.Write("                ");
                //Console.SetCursorPosition(24, 3);
                //Console.Write(ships[shipNumber].name);
                Console.SetCursorPosition(23, 5);
                Console.Write("(use arrows + space)");

                //(hopefully not) Broken code.
                rendering.UpdateLog("Place " + ships[shipNumber].name);

                while(!shipPlaced)
                {
                    rendering.DrawGameScreens(this);
                    rendering.DrawShipPlacement(shipX, shipY, shipLength, vertical);

                    userInput = (int)Console.ReadKey(true).Key;
                    beepPlayer.Play();

                    switch(userInput)
                    {
                        case 82: //'r' - rotate
                            if (vertical)
                            {
                                if (shipX + shipLength > 10)
                                {
                                    shipX = 10 - shipLength;
                                }
                            }
                            else
                            {
                                if (shipY + shipLength > 10)
                                {
                                    shipY = 10 - shipLength;
                                }
                            }
                            vertical = !vertical;
                            break;

                        case 37: //left arrow
                            if (shipX - 1 >= 0)
                            {
                                shipX--;
                            }
                            break;

                        case 38: //up arrow
                            if (shipY - 1 >= 0)
                            {
                                shipY--;
                            }
                            break;

                        case 39: //right arrow
                            if (vertical)
                            {
                                if(shipX + 1 < 10)
                                {
                                    shipX++;
                                }
                            }
                            else
                            {
                                if(shipX + shipLength < 10)
                                {
                                    shipX++;
                                }
                            }
                            break;

                        case 40: //down arrow
                            if (vertical)
                            {
                                 if (shipY + shipLength < 10)
                                 {
                                     shipY++;
                                 }
                            }
                            else
                            {
                                if (shipY + 1 < 10)
                                {
                                    shipY++;
                                }
                            }
                            break;

                        case 32: //Space bar
                            
                            if (ShipCollision(shipX, shipY, shipLength, vertical) == false)
                            {
                                selectPlayer.Play(); //play selection sound
                                for (int c = 0; c < shipLength; c++)
                                {
                                    if(vertical)
                                    {
                                        map[shipX, shipY + c] = shipNumber + 1; //this will be a unique identifier in order to allow quick lookups of hit ships.
                                    }
                                    else
                                    {
                                        map[shipX + c, shipY] = shipNumber + 1; //this will be a unique identifier in order to allow quick lookups of hit ships.
                                    }
                                }
                                ships[shipNumber].PlaceShip(shipX, shipY, vertical);
                                shipPlaced = true;
                            }
                            else
                            {
                                errorPlayer.Play();
                            }
                            break;
                    }
                }
            }
            rendering.UpdateLog("All ships placed!");
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.SetCursorPosition(24, 2);
            //Console.Write("       ");
            //Console.SetCursorPosition(24, 3);
            //Console.Write("                ");
            Console.SetCursorPosition(23, 5);
            Console.Write("                    ");

            beep.Dispose();
            beepPlayer.Dispose();
            select.Dispose();
            selectPlayer.Dispose();
            error.Dispose();
            errorPlayer.Dispose();
        }

        /// <summary>
        /// This method returns true if the ships current placement would result in a collision with an existing ship.
        /// </summary>
        /// <param name="shipX"></param>
        /// <param name="shipY"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        private bool ShipCollision(int shipX, int shipY, int shipLength, bool vertical)
        {
            bool collision = false;

            for (int c = 0; c < shipLength; c++)
            {
                if (vertical)
                {
                    if (map[shipX, shipY + c] != 0)
                    {
                        collision = true;
                    }
                }
                else
                {
                    if (map[shipX + c, shipY] != 0)
                    {
                        collision = true;
                    }
                }
            }

            return collision;
        }

        /// <summary>
        /// This method returns true if all of the player's ships have been destroyed.
        /// </summary>
        /// <returns></returns>
        public bool AllShipsDestroyed()
        {
            for (int shipNumber = 0; shipNumber < 5; shipNumber++)
            {
                if (ships[shipNumber].destroyed == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This method handles hits, determining which ships, if any, were hit and updating the instances accordingly. It will return a 0 if no ships were destroyed and a 1 if some were.
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public int SquareHit(int posX, int posY, Computer computer, Rendering rendering)
        {
            int hitShipID;

            Stream explosion = Battleships.Properties.Resources.Explosion;
            Stream missile = Battleships.Properties.Resources.Missile;

            SoundPlayer explosionPlayer = new SoundPlayer(explosion);
            SoundPlayer missilePlayer = new SoundPlayer(missile);

            missilePlayer.Play();
            System.Threading.Thread.Sleep(500);

            if (map[posX, posY] != 0 && map[posX, posY] < 6) //if the map square is a ship.
            {
                hitShipID = map[posX, posY] - 1;
                if (ships[hitShipID].ShipHit() == 0)
                {
                    computer.playerMap[posX, posY] = 2;
                    map[posX, posY] = 7;
                    explosionPlayer.Play();
                    rendering.DrawGameScreens(this);
                    rendering.UpdateLog("The enemy shot hits!");

                    explosion.Dispose();
                    missile.Dispose();
                    explosionPlayer.Dispose();
                    missilePlayer.Dispose();
                    return 0;
                }
                else
                {
                    for (int count = 0; count < ships[hitShipID].length; count++ )
                    {
                        computer.playerMap[ships[hitShipID].gridPositions[count, 0], ships[hitShipID].gridPositions[count, 1]] = 3; //Make it known to the computer that a ship has been destroyed.
                    }
                    //computer.playerMap[posX, posY] = 2;
                    map[posX, posY] = 7;
                    rendering.DrawGameScreens(this);
                    rendering.UpdateLog(ships[hitShipID].name + " destroyed!");
                    explosionPlayer.Play();
                    System.Threading.Thread.Sleep(150);
                    explosionPlayer.Play();
                    System.Threading.Thread.Sleep(150);
                    explosionPlayer.Play();
                    System.Threading.Thread.Sleep(150);

                    explosion.Dispose();
                    missile.Dispose();
                    explosionPlayer.Dispose();
                    missilePlayer.Dispose();
                    return 1;
                }
            }
            else
            {
                computer.playerMap[posX, posY] = 1;
                map[posX, posY] = 8;
                rendering.DrawGameScreens(this);
                rendering.UpdateLog("The enemy shot misses!");

                explosion.Dispose();
                missile.Dispose();
                explosionPlayer.Dispose();
                missilePlayer.Dispose();
                return 0;
            }
        }

        /// <summary>
        /// This method handles the player making a shot on the enemy ships.
        /// </summary>
        public void TakeShot(Computer computer, Rendering rendering)
        {
            int xSelection = 0;
            int ySelection = 0;
            bool shotFired = false;
            int userInput;

            Stream select = Battleships.Properties.Resources.SelectionSound;
            Stream confirm = Battleships.Properties.Resources.SelectionConfirm;
            Stream cancel = Battleships.Properties.Resources.SelectionCancel;
            Stream missile = Battleships.Properties.Resources.Missile;
            Stream error = Battleships.Properties.Resources.Error;

            SoundPlayer selectPlayer = new SoundPlayer(select);
            SoundPlayer confirmPlayer = new SoundPlayer(confirm);
            SoundPlayer cancelPlayer = new SoundPlayer(cancel);
            SoundPlayer missilePlayer = new SoundPlayer(missile);
            SoundPlayer errorPlayer = new SoundPlayer(error);

            while(shotFired == false)
            {
                rendering.DrawGameScreens(this);
                rendering.UpdateLog("Select Target");

                bool innerLoop = true;

                while(innerLoop)
                {
                    userInput = (int)Console.ReadKey(true).Key;
                    if(userInput < 75 && userInput > 64) //if the key pressed is a to j
                    {
                        selectPlayer.Play();
                        xSelection = userInput - 65; //converts the keycode to an x co-ordinate;
                        innerLoop = false;
                    }
                }

                rendering.DrawTarget(this, xSelection);
                innerLoop = true;

                while(innerLoop)
                {
                    userInput = (int)Console.ReadKey(true).Key;
                    if (userInput < 58 && userInput > 47) //if the key pressed is 0 to 9
                    {
                        selectPlayer.Play();
                        ySelection = userInput - 48;
                        innerLoop = false;
                    }
                }

                rendering.DrawTarget(this, xSelection, ySelection);
                rendering.UpdateLog("Ready to Fire");
                innerLoop = true;

                while (innerLoop)
                {
                    userInput = (int)Console.ReadKey(true).Key;

                    if (userInput == 32 || userInput == 13) //spacebar or enter
                    {
                        if (enemyMap[xSelection, ySelection] != 0)
                        {
                            errorPlayer.Play();
                            rendering.UpdateLog("Error: You've already fired at that square!");
                        }
                        else
                        {
                            missilePlayer.Play();
                            System.Threading.Thread.Sleep(500);
                            computer.SquareHit(xSelection, ySelection, this, rendering);
                            shotFired = true;
                        }
                        innerLoop = false;
                    }
                    else if (userInput == 8) //backspace
                    {
                        cancelPlayer.Play();
                        rendering.UpdateLog("Shot cancelled");
                        System.Threading.Thread.Sleep(1000);
                        innerLoop = false;
                    }
                }
            }

            select.Dispose();
            selectPlayer.Dispose();
            confirm.Dispose();
            confirmPlayer.Dispose();
            cancel.Dispose();
            cancelPlayer.Dispose();
            missile.Dispose();
            missilePlayer.Dispose();
            error.Dispose();
            errorPlayer.Dispose();

        }

        /// <summary>
        /// This method returns the number of intact ships.
        /// </summary>
        /// <returns></returns>
        public int remainingShips()
        {
            int totalShips = 0;

            for (int currentShip = 0; currentShip < 5; currentShip++)
            {
                if (ships[currentShip].destroyed == false)
                {
                    totalShips++;
                }
            }

            return totalShips;
        }

    }
}
