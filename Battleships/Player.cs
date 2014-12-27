using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void PlaceShips()
        {
            int shipLength;
            int shipOldX;
            int shipOldY;
            int shipX;
            int shipY;
            int userInput;
            bool shipPlaced;
            bool vertical;
            Rendering rendering = new Rendering();

            for(int shipNumber = 0; shipNumber < 5; shipNumber++)
            {
                shipLength = ships[shipNumber].length;
                shipX = 0;
                shipY = 0;
                vertical = true;;

                shipPlaced = false;

                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(24, 2);
                Console.Write("Place:");
                Console.SetCursorPosition(24, 3);
                Console.Write("                ");
                Console.SetCursorPosition(24, 3);
                Console.Write(ships[shipNumber].name);

                //Broken code.
                //rendering.UpdateLog("Place " + ships[shipNumber].name);

                while(!shipPlaced)
                {
                    rendering.DrawGameScreens(this);
                    rendering.DrawShipPlacement(shipX, shipY, shipLength, vertical);

                    userInput = (int)Console.ReadKey(true).Key;

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

                        case 13: //Enter key
                            
                            if (ShipCollision(shipX, shipY, shipLength, vertical) == false)
                            {
                                for (int c = 0; c < shipLength; c++)
                                {
                                    if(vertical)
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
                            break;
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(24, 2);
            Console.Write("      ");
            Console.SetCursorPosition(24, 3);
            Console.Write("                ");
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
