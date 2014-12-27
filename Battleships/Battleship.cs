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
        int[,] gridPositions;
        public int length {get; set;}
        int[] status; //1 = intct, 0 = destroyed.
        bool destroyed;

        public Battleship(string name, int length)
        {
            this.name = name;
            this.length = length;
            destroyed = false;
            status = new int[length];
            gridPositions = new int[length, 2];

            Array.Clear(status, 0, status.Length);

        }

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
    }
}