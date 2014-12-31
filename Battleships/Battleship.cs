using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Battleship
    {
        public string name;
        public int[,] gridPositions;
        public int length {get; set;}
        int health;
        public bool destroyed;

        public Battleship(string name, int length)
        {
            this.name = name;
            this.length = length;
            destroyed = false;
            health = length;
            gridPositions = new int[length, 2];
        }

        /// <summary>
        /// This method updates the necessary variables once a ship has been placed into the game world.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="vertical"></param>
        public void PlaceShip(int x, int y, bool vertical)
        {
            for(int c = 0; c < length; c++)
            {
                if (vertical)
                {
                    gridPositions[c, 0] = x;
                    gridPositions[c, 1] = y + c;
                }
                else
                {
                    gridPositions[c, 0] = x + c;
                    gridPositions[c, 1] = y;
                }
            }
        }

        /// <summary>
        /// This method will update the health of the ship. If the ship is still intact, it will return 0. If the ship is destroyed, it will return 1.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int ShipHit()
        {
            health--;
            if (health == 0)
            {
                destroyed = true;
                return 1;
            }
            return 0;
        }

    }
}