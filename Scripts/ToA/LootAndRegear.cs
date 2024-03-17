using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts.ToA
{
    internal class LootAndRegear
    {
        public int playerPos;
        public bool completed = false;
        public Banking bank = new Banking();
        public Mouse mouse;
        public bool ready = false;

        //check player pos
        //click on warden to exit
        //click to walk up to chests
        //if white, click on personal chest
        //if purple, loot main chest
        //leave
        //click bank
        //deposit all
        //re-gear
        //completed = true;
        public async void regear()
        {
            bank.mouse = mouse;
            //bank.depositAllInventory();
            await Task.Delay(100);
            bank.withdrawItem("Masori body", 1);
            await Task.Delay(100);
            bank.withdrawItem("Anguish", 1);
            await Task.Delay(100);
            bank.withdrawItem("Shadow of tumken", 1);
            await Task.Delay(100);
            bank.withdrawItem("Virtus mask", 1);
            await Task.Delay(100);
            bank.withdrawItem("Masori legs", 1);
            await Task.Delay(100);
            bank.withdrawItem("Assembler", 1);
            await Task.Delay(100);
            bank.withdrawItem("Occult nec", 1);
            await Task.Delay(100);
            bank.withdrawItem("Virtus robe top", 1);
            await Task.Delay(100);
            bank.withdrawItem("Toxic blowpipe", 1);
            await Task.Delay(100);
            bank.withdrawItem("Pegasian boots", 1);
            await Task.Delay(100);
            bank.withdrawItem("Imbued zamorak cape", 1);
            await Task.Delay(100);
            bank.withdrawItem("Ahrims robeskirt", 1);
            await Task.Delay(3000);
            bank.withdrawItem("Super Combat Potion", 1);
            await Task.Delay(100);
            bank.withdrawItem("Saradomin brew", 1);
            await Task.Delay(100);
            bank.withdrawItem("Saradomin brew", 1);
            await Task.Delay(100);
            bank.withdrawItem("Eternal boots", 1);
            await Task.Delay(100);
            bank.withdrawItem("Ranging potion", 1);
            await Task.Delay(100);
            bank.withdrawItem("Saradomin brew", 1);
            await Task.Delay(100);
            bank.withdrawItem("Super restore", 1);
            await Task.Delay(100);
            bank.withdrawItem("Super restore", 1);
            await Task.Delay(100);
            bank.withdrawItem("Dragon claws", 1);
            await Task.Delay(100);
            bank.withdrawItem("Super restore", 1);
            await Task.Delay(100);
            bank.withdrawItem("Super restore", 1);
            await Task.Delay(100);
            bank.withdrawItem("Rune pouch", 1);
            await Task.Delay(100);
            bank.withdrawItem("Dragon dagger", 1);
            await Task.Delay(100);
            bank.withdrawItem("Super restore", 1);
            await Task.Delay(100);
            bank.withdrawItem("Super restore", 1);
            await Task.Delay(100);
            bank.withdrawItem("Book of the dead", 1);
            await Task.Delay(100);
            while(!(mouse.mouseClicks[0] == 0))
            {
                await Task.Delay(100);
            }
            await Task.Delay(2000);
            Peripherals.PressKey((byte)Keys.Escape, 1);
            await Task.Delay(2000);
            ready = true;
        }

        public void setCoords(int x, int y)
        {

        }

    }
}
