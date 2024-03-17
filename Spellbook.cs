using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp
{
    internal class Spellbook
    {

        Peripherals peripherals = new Peripherals();

        public async void castSpell(string spell, string Npc = "")
        {
            openMageBook();
            await Task.Delay(600);

            switch (spell)
            {
                case "Home teleport":
                    peripherals.leftClick(561, 214);
                    break;
                case "Varrock teleport":
                    peripherals.leftClick(588, 263);
                    break;
                case "Lumbridge teleport":
                    peripherals.leftClick(665, 262);
                    break;
                case "Falador teleport":
                    peripherals.leftClick(562, 286);
                    break;
                case "House teleport":
                    peripherals.leftClick(613, 287);
                    break;
                case "Camelot teleport":
                    peripherals.leftClick(691, 288);
                    break;
                case "Ardougne teleport":
                    peripherals.leftClick(666, 311);
                    break;
                case "Watchtower teleport":
                    peripherals.leftClick(613, 335);
                    break;
                case "Trollheim teleport":
                    peripherals.leftClick(614, 358);
                    break;
                case "Ape atoll teleport":
                    peripherals.leftClick(691, 357);
                    break;
                case "Kourend castle teleport":
                    peripherals.leftClick(638, 381);
                    break;
                default:
                    break;
            }
        }

        public void openMageBook()
        {
            peripherals.leftClick(741, 182);
        }


    }
}
