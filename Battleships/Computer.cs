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

            ships[0] = new Battleship("Aircraft Carrier", 5);
            ships[1] = new Battleship("Battleship", 4);
            ships[2] = new Battleship("Submarine", 3);
            ships[3] = new Battleship("Cruiser", 3);
            ships[4] = new Battleship("Patrol Boat", 2);
        }

    }
}
