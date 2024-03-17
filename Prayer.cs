using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp
{
    internal class Prayer
    {
        public int[] gameScreenCoords = new int[2] { 0, 0 };
        Peripherals peripherals = new Peripherals();

        public void prayMage()
        {
            peripherals.fastLeftClick(601, 339);
        }

        public async void boost(string pray)
        {
            switch(pray)
            {
                case "Rigour":
                    peripherals.fastLeftClick(705, 181);
                    await Task.Delay(200);
                    peripherals.fastLeftClick(640, 411);
                    await Task.Delay(200);
                    peripherals.fastLeftClick(640, 182);
                    break;
                default:
                    break;
            }
        }

        public async void prayRange()
        {
            peripherals.fastLeftClick(637, 339);
        }

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            peripherals.setCoords(x, y);
        }
    }
}
