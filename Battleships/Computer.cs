using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Computer
    {
        int[,] map {get; set;}
        int[,] playerMap {get; set;} //the players fleet known to the computer.
        Battleship[] ships;

        public Computer()
        {
            map = new int[10, 10];
            playerMap = new int[10, 10];
            ships = new Battleship[5];

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
                                map[shipX, shipY + c] = 1;
                            }
                            else
                            {
                                map[shipX + c, shipY] = 1;
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
                    if (map[shipX, shipY + c] == 1)
                    {
                        collision = true;
                    }
                }
                else
                {
                    if (map[shipX + c, shipY] == 1)
                    {
                        collision = true;
                    }
                }
            }

            return collision;
        }

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

    }
}
