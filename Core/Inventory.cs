using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace Bot.Core
{
    internal class Inventory
    {
        public int invx = 554;
        public int invy = 203;

        public int itemx;
        public int itemy;

        public CoreProcessor processor;

        public int[] clientCoords = new int[2];

        public string[] inventory = new string[28];

        public void inventorySetup()
        {
            inventory[0] = "venator bow";
            inventory[1] = "masori assembler";
            inventory[2] = "infernal cape";
            inventory[3] = "fighters torso";
            inventory[4] = "necklace of anguish";
            inventory[5] = "armadyl dhide top";
            inventory[6] = "amulet of fury";
            inventory[7] = "saradomin godsword";
            inventory[8] = "rune pouch";
            inventory[9] = "osmumtens fang";
            inventory[10] = "avernic defender";
            inventory[11] = "ranging potion (4)";
            inventory[12] = "ranging potion (4)";
            inventory[13] = "ranging potion (4)";
            inventory[14] = "ranging potion (4)";
            inventory[15] = "ranging potion (4)";
            inventory[16] = "ranging potion (4)";
            inventory[17] = "ranging potion (4)";
            inventory[18] = "ranging potion (4)";
            inventory[19] = "ranging potion (4)";
            inventory[20] = "ranging potion (4)";
            inventory[21] = "";
            inventory[22] = "";
            inventory[23] = "";
            inventory[24] = "";
            inventory[25] = "zaryte crossbow";
            inventory[26] = "diamond bolts";
            inventory[27] = "";
        }

        public void addItem(string item, int amount = 0) // add amount later
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == "")
                {
                    inventory[i] = item;
                    break;
                }
            }
        }

        public bool hasItem2(string item)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == item)
                {
                    return true;
                }
            }
            return false;
        }

        public void clickInventorySlot(int x)
        {
            switch(x)
            {
                case 0:
                    processor.addMouseClick(575, 225, "inventory");
                    break;
                case 1:
                    processor.addMouseClick(575 + 45, 225, "inventory");
                    break;
                case 2:
                    processor.addMouseClick(575 + 45*2, 225, "inventory");
                    break;
                case 3:
                    processor.addMouseClick(575 + 45*3, 225, "inventory");
                    break;
                case 4:
                    processor.addMouseClick(575, 225 + 35, "inventory");
                    break;
                case 5:
                    processor.addMouseClick(575 + 45, 225 + 35, "inventory");
                    break;
                case 6:
                    processor.addMouseClick(575 + 45 * 2, 225 + 35, "inventory");
                    break;
                case 7:
                    processor.addMouseClick(575 + 45 * 3, 225 + 35, "inventory");
                    break;
                case 8:
                    processor.addMouseClick(575, 225 + 35*2, "inventory");
                    break;
                case 9:
                    processor.addMouseClick(575 + 45, 225 + 35 * 2, "inventory");
                    break;
                case 10:
                    processor.addMouseClick(575 + 45 * 2, 225 + 35 * 2, "inventory");
                    break;
                case 11:
                    processor.addMouseClick(575 + 45 * 3, 225 + 35 * 2, "inventory");
                    break;
                case 12:
                    processor.addMouseClick(575, 225 + 35 * 3, "inventory");
                    break;
                case 13:
                    processor.addMouseClick(575 + 45, 225 + 35 * 3, "inventory");
                    break;
                case 14:
                    processor.addMouseClick(575 + 45 * 2, 225 + 35 * 3, "inventory");
                    break;
                case 15:
                    processor.addMouseClick(575 + 45 * 3, 225 + 35 * 3, "inventory");
                    break;
                case 16:
                    processor.addMouseClick(575, 225 + 35 * 4, "inventory");
                    break;
                case 17:
                    processor.addMouseClick(575 + 45, 225 + 35 * 4, "inventory");
                    break;
                case 18:
                    processor.addMouseClick(575 + 45 * 2, 225 + 35 * 4, "inventory");
                    break;
                case 19:
                    processor.addMouseClick(575 + 45 * 3, 225 + 35 * 4, "inventory");
                    break;
                case 20:
                    processor.addMouseClick(575, 225 + 35 * 5, "inventory");
                    break;
                case 21:
                    processor.addMouseClick(575 + 45, 225 + 35 * 5, "inventory");
                    break;
                case 22:
                    processor.addMouseClick(575 + 45 * 2, 225 + 35 * 5, "inventory");
                    break;
                case 23:
                    processor.addMouseClick(575 + 45 * 3, 225 + 35 * 5, "inventory");
                    break;
                case 24:
                    processor.addMouseClick(575, 225 + 35 * 6, "inventory");
                    break;
                case 25:
                    processor.addMouseClick(575 + 45, 225 + 35 * 6, "inventory");
                    break;
                case 26:
                    processor.addMouseClick(575 + 45 * 2, 225 + 35 * 6, "inventory");
                    break;
                case 27:
                    processor.addMouseClick(575 + 45 * 3, 225 + 35 * 6, "inventory");
                    break;
            }
        }

        public string tempItem = "";

        public void clickItem2(string item, int slot, bool twohanded = false)
        {
            for(int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == item)
                {
                    clickInventorySlot(i);
                    if (slot < 90)
                    {
                        if (twohanded)
                        {
                            // Store the current item in the slot
                            tempItem = inventory[i];

                            // Replace the current item with the new two-handed weapon from slot 4
                            inventory[i] = processor.equipment.equipment[4]; // Assuming slot 4 is for the main weapon

                            // If there's an offhand in slot 6, find an empty slot for it
                            if (processor.equipment.equipment[6] != "")
                            {
                                bool slotFound = false;
                                for (int y = 0; y < inventory.Length; y++)
                                {
                                    if (inventory[y] == "")
                                    {
                                        inventory[y] = processor.equipment.equipment[6];
                                        processor.equipment.equipment[6] = ""; // Clear the offhand slot
                                        slotFound = true;
                                        break;
                                    }
                                }
                                if (!slotFound)
                                {
                                    Console.WriteLine("No empty slot found for offhand.");
                                    // You might want to handle this situation, like not equipping the two-handed weapon or throwing an error
                                }
                            }

                            // Update the equipment slot with the item that was in the inventory
                            processor.equipment.equipment[4] = tempItem; // Place the old item in the main weapon slot

                            // Clear tempItem
                            tempItem = "";
                        } else
                        {
                            if (slot == 6)
                            {
                                tempItem = inventory[i];
                                inventory[i] = processor.equipment.equipment[6];
                                processor.equipment.equipment[6] = tempItem;
                                tempItem = "";
                            }
                            else
                            {
                                tempItem = inventory[i];
                                inventory[i] = processor.equipment.equipment[slot];
                                processor.equipment.equipment[slot] = tempItem;
                                tempItem = "";
                            }
                        }

                        // Note: The 'break;' should be outside of this if block if you want to exit the outer loop after handling the two-handed weapon
                        break;
                    } else if(slot == 98)
                    {
                        configurePotion(i);
                        break;
                    }
                    break;
                }
                else if (slot == 95)
                {
                    for (int u = 0; u < inventory.Length; u++)
                    {
                        if (inventory[u] == (item + " (4)"))
                        {
                            clickInventorySlot(u);
                            break;
                        }
                        if (inventory[u] == (item + " (3)"))
                        {
                            clickInventorySlot(u);
                            break;
                        }
                        if (inventory[u] == (item + " (2)"))
                        {
                            clickInventorySlot(u);
                            break;
                        }
                        if (inventory[u] == (item + " (1)"))
                        {
                            clickInventorySlot(u);
                            break;
                        }
                    }
                }
            } 
        }

        public void configurePotion(int i)
        {
            if (inventory[i] == "ranging potion (4)")
            {
                inventory[i] = "ranging potion (3)";
            } else if (inventory[i] == "ranging potion (3)")
            {
                inventory[i] = "ranging potion (2)";
            } else if (inventory[i] == "ranging potion (2)")
            {
                inventory[i] = "ranging potion (1)";
            } else if (inventory[i] == "ranging potion (1)")
            {
                inventory[i] = "";
            }
        }
        public async void clickItem(string item)
        {
            while(processor.busy)
            {
                await Task.Delay(10);
            }
            processor.busy = true;
            processor.PressKey((byte)Keys.F1, 1);
            await Task.Delay(60);

            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(180, 260))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + invx, clientCoords[1] + invy, 0, 0, new Size(180, 260));
                }
                processor.busy = false;
                if (item == "mystic top")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 12 && bitmap.GetPixel(x, y).G == 29 && bitmap.GetPixel(x, y).B == 26)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 34 && bitmap.GetPixel(x + 1, y + 1).G == 73 && bitmap.GetPixel(x + 1, y + 1).B == 68)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "mystic bottoms")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 54 && bitmap.GetPixel(x, y).G == 114 && bitmap.GetPixel(x, y).B == 106)
                            {
                                if (bitmap.GetPixel(x+1, y+1).R == 54 && bitmap.GetPixel(x + 1, y + 1).G == 114 && bitmap.GetPixel(x + 1, y + 1).B == 106)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "master wand")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 99 && bitmap.GetPixel(x, y).G == 85 && bitmap.GetPixel(x, y).B == 15)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 75 && bitmap.GetPixel(x + 1, y + 1).G == 48 && bitmap.GetPixel(x + 1, y + 1).B == 26)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "smoke battlestaff")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 148 && bitmap.GetPixel(x, y).G == 16 && bitmap.GetPixel(x, y).B == 16)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R < 2 && bitmap.GetPixel(x + 1, y + 1).G < 2 && bitmap.GetPixel(x + 1, y + 1).B < 2)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "staff of light")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 124 && bitmap.GetPixel(x, y).G == 112 && bitmap.GetPixel(x, y).B == 111)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 60 && bitmap.GetPixel(x + 1, y + 1).G == 94 && bitmap.GetPixel(x + 1, y + 1).B == 129)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "karils top")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 96 && bitmap.GetPixel(x, y).G == 85 && bitmap.GetPixel(x, y).B == 56)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 23 && bitmap.GetPixel(x + 1, y + 1).G == 23 && bitmap.GetPixel(x + 1, y + 1).B == 12)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "torags legs")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 68 && bitmap.GetPixel(x, y).G == 68 && bitmap.GetPixel(x, y).B == 50)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 63 && bitmap.GetPixel(x + 1, y + 1).G == 62 && bitmap.GetPixel(x + 1, y + 1).B == 46)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "ahrims top")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 34 && bitmap.GetPixel(x, y).G == 33 && bitmap.GetPixel(x, y).B == 20)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 55 && bitmap.GetPixel(x + 1, y + 1).G == 54 && bitmap.GetPixel(x + 1, y + 1).B == 41)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "ahrims skirt")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 60 && bitmap.GetPixel(x, y).G == 60 && bitmap.GetPixel(x, y).B == 44)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 33 && bitmap.GetPixel(x + 1, y + 1).G == 32 && bitmap.GetPixel(x + 1, y + 1).B == 19)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "tome of fire")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 111 && bitmap.GetPixel(x, y).G == 33 && bitmap.GetPixel(x, y).B == 27)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 164 && bitmap.GetPixel(x + 1, y + 1).G == 84 && bitmap.GetPixel(x + 1, y + 1).B == 9)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "mixed hide top")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 81 && bitmap.GetPixel(x, y).G == 81 && bitmap.GetPixel(x, y).B == 73)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 53 && bitmap.GetPixel(x + 1, y + 1).G == 53 && bitmap.GetPixel(x + 1, y + 1).B == 47)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "mixed hide bottoms")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 112 && bitmap.GetPixel(x, y).G == 68 && bitmap.GetPixel(x, y).B == 53)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 134 && bitmap.GetPixel(x + 1, y + 1).G == 130 && bitmap.GetPixel(x + 1, y + 1).B == 121)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "dragon defender")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 191 && bitmap.GetPixel(x, y).G == 23 && bitmap.GetPixel(x, y).B == 10)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 9 && bitmap.GetPixel(x + 1, y + 1).G == 7 && bitmap.GetPixel(x + 1, y + 1).B == 7)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "abyssal whip")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 43 && bitmap.GetPixel(x, y).G == 15 && bitmap.GetPixel(x, y).B == 14)
                            {
                                if (bitmap.GetPixel(x+1, y+1).R == 54 && bitmap.GetPixel(x + 1, y + 1).G == 20 && bitmap.GetPixel(x + 1, y + 1).B == 19)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "karils cbow")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 37 && bitmap.GetPixel(x, y).G == 37 && bitmap.GetPixel(x, y).B == 27)
                            {
                                if (bitmap.GetPixel(x+1, y+1).R == 34 && bitmap.GetPixel(x+1, y+1).G == 34 && bitmap.GetPixel(x+1, y+1).B == 25)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "imbued heart")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 123 && bitmap.GetPixel(x, y).G == 57 && bitmap.GetPixel(x, y).B == 122)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 110 && bitmap.GetPixel(x + 1, y + 1).G == 39 && bitmap.GetPixel(x + 1, y + 1).B == 109)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "ahrims staff")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 17 && bitmap.GetPixel(x, y).G == 17 && bitmap.GetPixel(x, y).B == 5)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 0 && bitmap.GetPixel(x + 1, y + 1).G == 0 && bitmap.GetPixel(x + 1, y + 1).B == 1)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "occult necklace")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 24 && bitmap.GetPixel(x, y).G == 1 && bitmap.GetPixel(x, y).B == 25)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 42 && bitmap.GetPixel(x + 1, y + 1).G == 2 && bitmap.GetPixel(x + 1, y + 1).B == 44)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "amulet of fury")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 60 && bitmap.GetPixel(x, y).G == 7 && bitmap.GetPixel(x, y).B == 3)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 34 && bitmap.GetPixel(x + 1, y + 1).G == 34 && bitmap.GetPixel(x + 1, y + 1).B == 46)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "tome of earth")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 82 && bitmap.GetPixel(x, y).G == 48 && bitmap.GetPixel(x, y).B == 5)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 48 && bitmap.GetPixel(x + 1, y + 1).G == 103 && bitmap.GetPixel(x + 1, y + 1).B == 65)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "venator bow")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 27 && bitmap.GetPixel(x, y).G == 22 && bitmap.GetPixel(x, y).B == 37)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 34 && bitmap.GetPixel(x + 1, y + 1).G == 27 && bitmap.GetPixel(x + 1, y + 1).B == 47)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "eclipse atlatl")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 205 && bitmap.GetPixel(x, y).G == 146 && bitmap.GetPixel(x, y).B == 33)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 94 && bitmap.GetPixel(x + 1, y + 1).G == 69 && bitmap.GetPixel(x + 1, y + 1).B == 23)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "regen bracelet")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 12 && bitmap.GetPixel(x, y).G == 12 && bitmap.GetPixel(x, y).B == 34)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 10 && bitmap.GetPixel(x + 1, y + 1).G == 10 && bitmap.GetPixel(x + 1, y + 1).B == 30)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "atlatl dart")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 14 && bitmap.GetPixel(x, y).G == 57 && bitmap.GetPixel(x, y).B == 41)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 10 && bitmap.GetPixel(x + 1, y + 1).G == 46 && bitmap.GetPixel(x + 1, y + 1).B == 31)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "amethyst arrow")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 82 && bitmap.GetPixel(x, y).G == 9 && bitmap.GetPixel(x, y).B == 5)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 82 && bitmap.GetPixel(x + 1, y + 1).G == 9 && bitmap.GetPixel(x + 1, y + 1).B == 5)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "tormented bracelet")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 181 && bitmap.GetPixel(x, y).G == 93 && bitmap.GetPixel(x, y).B == 10)
                            {
                                if (bitmap.GetPixel(x + 1, y + 1).R == 189 && bitmap.GetPixel(x + 1, y + 1).G == 80 && bitmap.GetPixel(x + 1, y + 1).B == 10)
                                {
                                    itemx = x + invx;
                                    itemy = y + invy;
                                    processor.addMouseClick(itemx, itemy, "inventory");
                                    return;
                                }
                            }
                        }
                    }
                }
                if (item == "ranging potion")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y) == Color.FromArgb(52, 161, 207) || bitmap.GetPixel(x, y) == Color.FromArgb(70, 167, 209))
                            {
                                itemx = x + invx;
                                itemy = y + invy;
                                processor.addMouseClick(itemx, itemy, "inventory");
                                return;
                            }
                        }
                    }

                }
                if (item == "Monkey tail")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y) == Color.FromArgb(26, 73, 3))
                            {
                                itemx = x + invx;
                                itemy = y + invy;
                                processor.addMouseClick(itemx, itemy, "inventory");
                                return;
                            }
                        }
                    }

                }
                if (item == "Banana basket")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y) == Color.FromArgb(122, 103, 72))
                            {
                                itemx = x + invx;
                                itemy = y + invy;
                                processor.addMouseClick(itemx, itemy, "inventory");
                                return;
                            }
                        }
                    }

                }
            }
        }
        public async void rubAmulet(string item)
        {
            if (item == "Games nec")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(clientCoords[0] + invx, clientCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(143, 119, 9) || bitmap.GetPixel(i, j) == Color.FromArgb(143, 199, 9) || bitmap.GetPixel(i, j) == Color.FromArgb(162, 141, 19))
                            {
                                processor.rightClick(invx + i, invy + j);
                                await Task.Delay(200);
                                processor.addMouseClick(invx + i, invy + j + 60);
                                return;
                            }
                        }
                    }
                }
            }
        }
        public bool hasItem(string item)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(173, 254))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + invx, clientCoords[1] + invy, 0, 0, new Size(173, 254));
                }

                if (item == "Full ess")
                {
                    for (int i = 132; i < 173; i++)
                    {
                        for (int j = 225; j < 254; j++)
                        {
                            if (bitmap.GetPixel(i, j).R == 35 && bitmap.GetPixel(i, j).G == 59 && bitmap.GetPixel(i, j).B == 60)
                            {
                                return true;
                            }
                        }
                    }
                }
                if (item == "Panic tab")
                {
                    for (int i = 0; i < 173; i++)
                    {
                        for (int j = 0; j < 254; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(119, 159, 161) || bitmap.GetPixel(i, j) == Color.FromArgb(135, 170, 171))
                            {
                                itemx = i + invx;
                                itemy = j + invy;
                                return true;
                            }
                        }
                    }
                }
                if (item == "Monkey tail")
                {
                    for (int i = 0; i < 173; i++)
                    {
                        for (int j = 0; j < 254; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(26, 73, 3))
                            {
                                itemx = i + invx;
                                itemy = j + invy;
                                return true;
                            }
                        }
                    }
                }
                if (item == "Banana")
                {
                    for (int i = 0; i < 173; i++)
                    {
                        for (int j = 0; j < 254; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(69, 68, 17))
                            {
                                itemx = i + invx;
                                itemy = j + invy;
                                return true;
                            }
                        }
                    }
                }
                if (item == "Banana basket")
                {
                    for (int i = 0; i < 173; i++)
                    {
                        for (int j = 0; j < 254; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(122, 103, 72))
                            {
                                itemx = i + invx;
                                itemy = j + invy;
                                return true;
                            }
                        }
                    }
                }
                if (item == "Super Attack Potion")
                {
                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(52, 55, 207) || bitmap.GetPixel(i, j) == Color.FromArgb(90, 91, 211) || bitmap.GetPixel(i, j) == Color.FromArgb(67, 70, 210) || bitmap.GetPixel(i, j) == Color.FromArgb(26, 29, 162) || bitmap.GetPixel(i, j) == Color.FromArgb(70, 71, 209) || bitmap.GetPixel(i, j) == Color.FromArgb(27, 30, 174))
                            {
                                itemx = i + invx;
                                itemy = j + invy;
                                return true;
                            }
                        }
                    }

                }
                if (item == "Shark")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 80 && bitmap.GetPixel(x, y).G == 56 && bitmap.GetPixel(x, y).B == 37)
                            {
                                itemx = x + invx;
                                itemy = y + invy;
                                return true;
                            }
                        }
                    }
                }

                if (item == "Construction cape")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 53 && bitmap.GetPixel(x, y).G == 43 && bitmap.GetPixel(x, y).B == 31)
                            {
                                itemx = x + invx;
                                itemy = y + invy;
                                return true;
                            }
                        }
                    }
                }
                if (item == "Crafting cape")
                {
                    for (int x = 0; x < 173; x++)
                    {
                        for (int y = 0; y < 254; y++)
                        {
                            if (bitmap.GetPixel(x, y).R == 113 && bitmap.GetPixel(x, y).G == 76 && bitmap.GetPixel(x, y).B == 7)
                            {
                                itemx = x + invx;
                                itemy = y + invy;
                                return true;
                            }
                        }
                    }
                }

                if (item == "Pouch ess")
                {
                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 80; j++)
                        {
                            if (bitmap.GetPixel(i, j).R == 35 && bitmap.GetPixel(i, j).G == 59 && bitmap.GetPixel(i, j).B == 60)
                            {
                                return true;
                            }
                        }
                    }
                }

                for (int i = 0; i < 173; i++)
                {
                    for (int j = 0; j < 254; j++)
                    {
                        if (item == "Damaged pouch")
                        {
                            if (bitmap.GetPixel(i, j).R == 41 && bitmap.GetPixel(i, j).G == 31 && bitmap.GetPixel(i, j).B == 31)
                            {
                                return true;
                            }
                        }
                        if (item == "Colo pouch")
                        {
                            if (bitmap.GetPixel(i, j).R == 177 && bitmap.GetPixel(i, j).G == 169 && bitmap.GetPixel(i, j).B == 166)
                            {
                                itemx = i + invx;
                                itemy = j + invy;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
