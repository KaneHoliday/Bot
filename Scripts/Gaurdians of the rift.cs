using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Gaurdians_of_the_rift
    {

        Player player = new Player();
        Peripherals peripherals = new Peripherals();
        Spellbook spellbook = new Spellbook();

        public int[] gameScreenCoords = new int[2] { 0, 0 };


        public async void startScript()
        {
            peripherals.leftClick(50,50);
        }

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            player.setCoords(x, y);
            peripherals.setCoords(x, y);

        }
    }
}
