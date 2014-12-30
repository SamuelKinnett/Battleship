using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace Battleships
{
    class Computer
    {
        public int[,] map {get; set;}
        public int[,] playerMap {get; set;} //the players fleet known to the computer.
        Battleship[] ships;
        bool Hunting;

        public Computer()
        {
            map = new int[10, 10];
            playerMap = new int[10, 10];
            ships = new Battleship[5];
            Hunting = true;

            Array.Clear(map, 0, map.Length);
            Array.Clear(playerMap, 0, playerMap.Length);

            ships[0] = new Battleship("Aircraft Carrier", 5);
            ships[1] = new Battleship("Battleship", 4);
            ships[2] = new Battleship("Submarine", 3);
            ships[3] = new Battleship("Cruiser", 3);
            ships[4] = new Battleship("Patrol Boat", 2);
        }

        public void PlaceShips()
        {
            Random rand = new Random();

            int length;
            int shipX;
            int shipY;
            bool vertical = false;
            bool shipPlaced;

            for(int shipNumber = 0; shipNumber < 5; shipNumber++)
            {
                shipPlaced = false;
                length = ships[shipNumber].length;

                while (shipPlaced == false)
                {
                    if(rand.Next(0, 1) == 1)
                    {
                        vertical = true;
                    }
                    if (vertical == true)
                    {
                        shipX = rand.Next(0, 10);
                        shipY = rand.Next(0, 10 - length);
                    }
                    else
                    {
                        shipX = rand.Next(0, 10 - length);
                        shipY = rand.Next(0, 10);
                    }
                    
                    if (ShipCollision(shipX, shipY, length, vertical) == false)
                    {
                        for (int c = 0; c < length; c++)
                        {
                            if (vertical)
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
                }
            }
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
                    if (map[shipX, shipY + c] != 0) //if the map square contains a ship
                    {
                        collision = true;
                    }
                }
                else
                {
                    if (map[shipX + c, shipY] != 0) //if the map square contains a ship
                    {
                        collision = true;
                    }
                }
            }

            return collision;
        }

        /// <summary>
        /// This method returns true if all of the Computer's ships have been destroyed.
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
        /// This method handles hits, determining which ships, if any, were hit and updating the instances accordingly.
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public void SquareHit(int posX, int posY, Player player, Rendering rendering)
        {
            int hitShipID;
            Stream explosion = Battleships.Properties.Resources.Explosion;
            SoundPlayer explosionPlayer = new SoundPlayer(explosion);

            if (map[posX, posY] != 0) //if the map square is a ship.
            {
                hitShipID = map[posX, posY] - 1;
                if (ships[hitShipID].ShipHit() == 0)
                {
                    explosionPlayer.Play();
                    rendering.UpdateLog("Your shot hits!");
                    player.enemyMap[posX, posY] = 2;
                }
                else
                {
                    explosionPlayer.Play();
                    System.Threading.Thread.Sleep(150);
                    explosionPlayer.Play();
                    System.Threading.Thread.Sleep(150);
                    explosionPlayer.Play();
                    System.Threading.Thread.Sleep(150);
                    rendering.UpdateLog(ships[hitShipID].name + " destroyed!");
                    player.enemyMap[posX, posY] = 2;
                }
            }
            else
            {
                rendering.UpdateLog("Your shot misses!");
                player.enemyMap[posX, posY] = 1;
            }
        }

        /// <summary>
        /// This method handles the computer's turn.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="rendering"></param>
        public void TakeShot(Player player, Rendering rendering)
        {
            int[,] possibilityMap;
            int currentHighestX = 0;
            int currentHighestY = 0;
            int currentHighestScore = 0;
            int tempScore = 0;

            possibilityMap = CalculatepossiblePlacements();
            
            for(int x = 0; x < 10; x++)
            {
                for(int y = 0; y < 10; y++)
                {
                    tempScore = possibilityMap[x, y];
                    if (tempScore > currentHighestScore)
                    {
                        currentHighestX = x;
                        currentHighestY = y;
                        currentHighestScore = tempScore;
                    }
                }
            }

            player.SquareHit(currentHighestX, currentHighestY, this, rendering);

        }

        /// <summary>
        /// This method works out the number of times a tile could possibly be occupied by a ship.
        /// </summary>
        /// <param name="shipSize"></param>
        /// <param name="possibilityMap"></param>
        /// <returns></returns>
        private int[,] CalculatepossiblePlacements()
        {
            //Vertical placement possibilities
            int[,] possibilityMap = new int[10, 10];
            Array.Clear(possibilityMap, 0, possibilityMap.Length);

            for (int currentShipSize = 0; currentShipSize < 5; currentShipSize++)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10 - currentShipSize; y++)
                    {

                        for (int c = 0; c < currentShipSize; c++)
                        {
                            if (ShipCollision(x, y, currentShipSize, true))
                            {
                                possibilityMap[x, y + c]++; //increment the value in the possibility map, indicating a ship could be here.
                            }
                        }

                    }
                }

                //Horizontal placement possibilities

                for (int x = 0; x < 10 - currentShipSize; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {

                        for (int c = 0; c < currentShipSize; c++)
                        {
                            if (ShipCollision(x, y, currentShipSize, false))
                            {
                                possibilityMap[x + c, y]++; //increment the value in the possibility map, indicating a ship could be here.
                            }
                        }

                    }
                }
            }

            return possibilityMap;

        }

    }
}
