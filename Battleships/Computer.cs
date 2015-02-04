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
        public int[,] playerMap {get; set;} //the players fleet known to the computer. A 0 indicates unknown space, a 1 indicates empty ocean, a 2 indicates a hit ship and a 3 indicates a destroyed ship.
        public Battleship[] ships;
        public int[] playerShipsRemaining;
        bool hunting;

        public Computer()
        {
            map = new int[10, 10];
            playerMap = new int[10, 10];
            ships = new Battleship[5];
            hunting = true;

            Array.Clear(map, 0, map.Length);
            Array.Clear(playerMap, 0, playerMap.Length);

            ships[0] = new Battleship("Aircraft Carrier", 5);
            ships[1] = new Battleship("Battleship", 4);
            ships[2] = new Battleship("Submarine", 3);
            ships[3] = new Battleship("Cruiser", 3);
            ships[4] = new Battleship("Patrol Boat", 2);

            playerShipsRemaining = new int[5] {1, 1, 1, 1, 1};
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
                    if (rand.Next(0, 2) == 1)
                    {
                        vertical = true;
                    }
                    else
                    {
                        vertical = false;
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
        /// This method checks to see if a hypothetical ship placement in the player grid would collide with a known ship. It returns the number of ship tiles the placement collides with.
        /// </summary>
        /// <param name="shipX"></param>
        /// <param name="shipY"></param>
        /// <param name="shipLength"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        private int PlayerShipCollision(int shipX, int shipY, int shipLength, bool vertical)
        {
            int numberOfCollisions = 0;

            for (int c = 0; c < shipLength; c++)
            {
                if (vertical)
                {
                    if (playerMap[shipX, shipY + c] == 2) //if the map square contains a ship
                    {
                        numberOfCollisions++;
                    }
                }
                else
                {
                    if (playerMap[shipX + c, shipY] == 2) //if the map square contains a ship
                    {
                        numberOfCollisions++;
                    }
                }
            }

            return numberOfCollisions;
        }

        /// <summary>
        /// This method checks to see if a hypothetical ship would collide with anything excpet unknown tiles.
        /// </summary>
        /// <param name="shipX"></param>
        /// <param name="shipY"></param>
        /// <param name="shipLength"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        private bool UnknownSpaceCollision(int shipX, int shipY, int shipLength, bool vertical)
        {
            bool collision = false;

            for (int c = 0; c < shipLength; c++)
            {
                if (vertical)
                {
                    if (playerMap[shipX, shipY + c] != 0) //if the map square is not an unknown tile
                    {
                        collision = true;
                    }
                }
                else
                {
                    if (playerMap[shipX + c, shipY] != 0) //if the map square is not an unknown tile
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

            hunting = true;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (playerMap[x, y] == 2) //if there is a hit ship that has not been fully destroyed...
                    {
                        hunting = false; //go into target mode
                    }
                }
            }

            if (hunting)
            {
                possibilityMap = CalculatepossiblePlacements(player);
            }
            else
            {
                possibilityMap = CalculateTargetedPlacements();
            }

            //clear all previously fired on tiles just to be safe

            for (int x = 0; x < 10; x++)
            {
                for(int y = 0; y < 10; y++)
                {
                    if (playerMap[x, y] != 0)
                    {
                        possibilityMap[x, y] = 0;
                    }
                }
            }

            //choose most likely location of the ship

                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
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
        private int[,] CalculatepossiblePlacements(Player player)
        {
            int[,] possibilityMap = new int[10, 10];
            int smallestRemainingShip = 4;
            Array.Clear(possibilityMap, 0, possibilityMap.Length);

            playerShipsRemaining = player.shipStatus();

            if (playerShipsRemaining[0] == 1)
            {
                smallestRemainingShip = 1;
            }
            else if (playerShipsRemaining[1] == 1 || playerShipsRemaining[2] == 1)
            {
                smallestRemainingShip = 2;
            }
            else if (playerShipsRemaining[3] == 1)
            {
                smallestRemainingShip = 3;
            }
            //Vertical placement possibilities

            for (int currentShipSize = smallestRemainingShip; currentShipSize < 5; currentShipSize++)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10 - currentShipSize; y++)
                    {

                        for (int c = 0; c < currentShipSize; c++)
                        {
                            if (UnknownSpaceCollision(x, y, currentShipSize, true) == false)
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
                            if (UnknownSpaceCollision(x, y, currentShipSize, false) == false)
                            {
                                possibilityMap[x + c, y]++; //increment the value in the possibility map, indicating a ship could be here.
                            }
                        }

                    }
                }
            }

            return possibilityMap;

        }

        /// <summary>
        /// This method is invoked when the AI knows the general location of a ship, to whittle down the remaining squares it occupies.
        /// </summary>
        /// <returns></returns>
        private int[,] CalculateTargetedPlacements()
        {
            int[,] possibilityMap = new int[10, 10];
            Array.Clear(possibilityMap, 0, possibilityMap.Length);

            //Vertical placement possibilities

            for (int currentShipSize = 0; currentShipSize < 5; currentShipSize++)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10 - currentShipSize; y++)
                    {

                        for (int c = 0; c < currentShipSize; c++)
                        {
                            if (PlayerShipCollision(x, y, currentShipSize, true) > 0)
                            {
                                possibilityMap[x, y + c] += PlayerShipCollision(x, y, currentShipSize, true); //increment the value in the possibility map, indicating a ship could be here.
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
                            if (PlayerShipCollision(x, y, currentShipSize, false) > 0)
                            {
                                possibilityMap[x + c, y] += PlayerShipCollision(x, y, currentShipSize, false); //increment the value in the possibility map, indicating a ship could be here.
                            }
                        }

                    }
                }
            }

            return possibilityMap;
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
