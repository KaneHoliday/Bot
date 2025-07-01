using Bot.Core;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Bot.Core.CoreProcessor;
using static System.Windows.Forms.Design.AxImporter;

namespace Bot.Scripts
{
    internal class CorpTank
    {

        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;
        public Prayer prayer;

        public bool third = false;

        public bool hit = false;

        public int[] clientCoords = new int[2];
        public int health;
        public int attacks = 10;
        public int tAttacks = 0;
        public bool wild = false;

        public int profit = 0;

        public bool cLured = false;
        public bool bPos = false;

        public int tick = 0;
        public int attacksSpecial;
        public int resets;

        public bool firstStomp = false;
        public bool firstHit = true;

        public string spellBook = "Lunars";

        //settings
        public bool thrallsEnabled = false;
        public bool vengEnabled = true;
        public bool spellbookSwap = true;
        public string potion = "Super Attack Potion";
        public bool spec = false;
        public string specWep = "None";
        public bool tDrop = false;
        public int corpKills = 0;

        public int seconds = 0;
        public int minutes = 0;
        public int hours = 0;
        
        public void putItemsOnTest()
        {
            processor.addMouseClick(575, 224, "inventory");
            processor.addMouseClick(619, 224, "inventory");
            processor.addMouseClick(135, 113, "gamescreen");
            processor.addMouseClick(658, 224, "inventory");
        }

        public async void update()
        {
            if(blockerPos())
            {
                bPos = true;
            } else
            {
                bPos = false;
            }
            if(corpLured())
            {
                cLured = true;
            } else
            {
                cLured = false;
            }
            await Task.Delay(200);
            tenthTick();
            playerHit();
            //inside();
            update();
        }

        public async void startTime()
        {
            await Task.Delay(1000);
            seconds++;
            if (seconds > 59)
            {
                seconds = 0;
                minutes++;
            }
            if (minutes > 59)
            {
                minutes = 0;
                hours++;
            }
            startTime();
        }

        public async void updateConsole()
        {
            Console.Clear();
            if (seconds < 10)
            {
                Console.WriteLine("Time running: " + hours.ToString() + ":" + minutes.ToString() + ":0" + seconds.ToString());
            }
            if (seconds > 9)
            {
                Console.WriteLine("Time running: " + hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString());
            }
            Console.WriteLine("Corps killed: " + corpKills.ToString());
            //Console.WriteLine("Xp gained: " + xpGained.ToString());
            Console.WriteLine("Estimated Profit Made: " + profit.ToString());
            await Task.Delay(1000);
            updateConsole();
        }

        public void startScript()
        {
            //updateConsole();
            startTime();
            //drinkpotion and remove waitforsetup,
            attack();
            update();
            Console.WriteLine("Starting corp bot");
        }

        public async void drinkPotion()
        {
            if (inventory.hasItem("Super Attack Potion"))
            {
                processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
            } else
            {
                Console.WriteLine("No attack potion");
            }
            await Task.Delay(600);
            inventory.clientCoords[0] = inventory.clientCoords[0] + 769;
            if (inventory.hasItem("Super Attack Potion"))
            {
                processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
            }
            else
            {
                Console.WriteLine("Alt has no attack potion");
            }
            await Task.Delay(100);
            inventory.clientCoords[0] = inventory.clientCoords[0] - 769;
            await Task.Delay(100);
            if (third)
            {
                inventory.clientCoords[1] = inventory.clientCoords[1] + 530;
                await Task.Delay(100);
                if (inventory.hasItem("Super Attack Potion"))
                {
                    processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                }
                else
                {
                    Console.WriteLine("Alt has no attack potion");
                }
                await Task.Delay(100);
                inventory.clientCoords[1] = inventory.clientCoords[1] - 530;
            }
            await Task.Delay(100);
            while (green())
            {
                await Task.Delay(10);
            }
            waitForSetup();
        }

        public async void testHealth()
        {
            Console.WriteLine("Current health: " + player.playerHealth().ToString());
            player.updateHealth();
            await Task.Delay(200);
            Console.WriteLine("New health: " + player.playerHealth().ToString());
        }

        public async void teleportToBank()
        {
            if (inventory.hasItem("Crafting cape"))
            {
                processor.addMouseClick(inventory.itemx, inventory.itemy);
            }
            await Task.Delay(3000);
            processor.addMouseClick(421, 45, "gamescreen");
            doBanking();
        }

        public async void emergencyTab()
        {
            hit = false;
            inventory.clientCoords[0] += 774;
            if(inventory.hasItem("Panic tab"))
            {
                processor.addMouseClick(inventory.itemx, inventory.itemy);
            }
            await Task.Delay(3000);
            inventory.rubAmulet("Games nec");
            await Task.Delay(1800);
            processor.PressKey((byte)Keys.NumPad3, 1);
            await Task.Delay(3000);
            while (!corpPortal2())
            {
                await Task.Delay(10);
            }
            Console.WriteLine("Clicked corp portal");
            inventory.clientCoords[0] -= 774;
            while (!instanceMessage2())
            {
                await Task.Delay(10);
            }
            await Task.Delay(300);
            processor.addMouseClick(135 + 774, 614);
            await Task.Delay(600);
            lureWild();
        }

        public async void doBanking()
        {
            if(bankOpen())
            {
                processor.addMouseClick(398, 57, "gamescreen");
                await Task.Delay(300);
                firstHit = true;
                attacks = 10;
                if (!inventory.hasItem("Super Attack Potion"))
                {
                    processor.addMouseClick(371, 207, "gamescreen");
                    await Task.Delay(300);
                }
                //if has attack pot, dont withdraw
                processor.addMouseClick(85, 165, "gamescreen"); //withdraw shark
                await Task.Delay(600);
                processor.PressKey((byte)Keys.Escape, 1);
                await Task.Delay(300);
                if (inventory.hasItem("Construction cape"))
                {
                    processor.addMouseClick(inventory.itemx, inventory.itemy);
                }
                while(!welcomeHome())
                {
                    await Task.Delay(10);
                }
                teleportToCorp();
            } else
            {
                await Task.Delay(10);
                doBanking();
            }
        }

        public async void teleportToCorp()
        {
            while (welcomeHome())
            {
                await Task.Delay(10);
            }
            await Task.Delay(1200);
            processor.addMouseClick(129, 250, "gamescreen");
            await Task.Delay(300);
            processor.rightClick(485, 224);
            await Task.Delay(300);
            player.updateHealth();
            await Task.Delay(600);
            Console.WriteLine("New health: " + player.playerHealth());
            while (player.playerHealth() < 90) {
                Console.WriteLine("Havnt recharged yet.");
                await Task.Delay(600);
                player.updateHealth();
                Console.WriteLine("New health: " + player.playerHealth());
            }
            await Task.Delay(600);
            processor.addMouseClick(488, 270);
            await Task.Delay(400);
            if (!tDrop && !wild)
            {
                processor.addMouseClick(1082, 172);
                tDrop = false;
            }
            await Task.Delay(4700);
            while (!corpPortal())
            {
                await Task.Delay(10);
            }
            Console.WriteLine("Clicked corp portal");
            while(!instanceMessage())
            {
                await Task.Delay(10);
            }
            await Task.Delay(300);
            processor.addMouseClick(120, 74);
            while(!atLureSpot())
            {
                await Task.Delay(10);
            }
            Console.WriteLine("Time to lure corp.");
            lureCorp();
        }

        public async void lureCorp()
        {
            player.updateHealth();
            await Task.Delay(100);
            if (inventory.hasItem("Shark") && player.health < 65)
            {
                processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                player.health += 20;
                await Task.Delay(1200);
                lureCorp();
            }
            else
            {
                int checks = 0;
                if (corpMap())
                {
                    processor.addMouseClick(553, 90, "gamescreen");
                    await Task.Delay(100);
                    processor.addMouseClick(256, 91, "gamescreen");
                    await Task.Delay(100);
                    while (!insideCave)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(258, 213, "gamescreen");
                    await Task.Delay(600);
                    processor.addMouseClick(553, 90, "gamescreen");
                    while (!corpLured())
                    {
                        await Task.Delay(600);
                        checks++;
                        Console.WriteLine("Checked " + checks.ToString() + " times");
                        if (checks > 20)
                        {
                            break;
                        }
                    }
                    if (checks > 20)
                    {
                        lureWild();
                        return;
                    }
                    processor.addMouseClick(1030, 106, "gamescreen");
                    await Task.Delay(600);
                    if (third)
                    {
                        processor.addMouseClick(248, 547, "gamescreen");
                    }
                    await Task.Delay(600);
                    processor.addMouseClick(1031, 127, "gamescreen");
                    if (hit)
                    {
                        emergencyTab();
                        return;
                    }
                    await Task.Delay(200);
                    if (hit)
                    {
                        emergencyTab();
                        return;
                    }
                    await Task.Delay(200);
                    if (hit)
                    {
                        emergencyTab();
                        return;
                    }
                    await Task.Delay(200);
                    if (hit)
                    {
                        emergencyTab();
                        return;
                    }
                    if (hit)
                    {
                        emergencyTab();
                        return;
                    }
                    drinkPotion();
                }
                else
                {
                    processor.addMouseClick(553, 90, "gamescreen"); // turns prayer on
                    await Task.Delay(800);
                    processor.addMouseClick(262, 89, "gamescreen"); //enters room
                    while (!insideCave)
                    {
                        await Task.Delay(10);
                    }
                    await Task.Delay(100);
                    if (corpMap())
                    {
                        processor.addMouseClick(260, 203);
                        await Task.Delay(350);
                    }
                    else
                    {
                        processor.addMouseClick(639, 48, "gamescreen"); // run north
                        await Task.Delay(350);
                        processor.rightClick(260, 203);
                        while (!corpMap())
                        {
                            await Task.Delay(10);
                        }
                        await Task.Delay(300);
                        processor.addMouseClick(260, 233); //click enterance 
                    }
                    while (!atLureSpot())
                    {
                        await Task.Delay(20);
                    }
                    processor.addMouseClick(553, 90, "gamescreen"); //turns prayer off
                    await Task.Delay(1000);
                    while (!corpLured())
                    {
                        await Task.Delay(600);
                        checks++;
                        Console.WriteLine("Checked " + checks.ToString() + " times");
                        if (checks > 20)
                        {
                            break;
                        }
                    }
                    if (checks > 20)
                    {
                        lureWild();
                        return;
                    }
                    processor.addMouseClick(1030, 106, "gamescreen");
                    await Task.Delay(600);
                    if (third)
                    {
                        processor.addMouseClick(248, 547, "gamescreen");
                    }
                    await Task.Delay(600);
                    if (hit)
                    {
                        emergencyTab();
                        return;
                    }
                    await Task.Delay(600);
                    processor.addMouseClick(1031, 127, "gamescreen");
                    await Task.Delay(600);
                    if (hit)
                    {
                        emergencyTab();
                        return;
                    }
                    drinkPotion();
                }
            }
        }

        public async void lureWild()
        {
            player.updateHealth();
            await Task.Delay(100);
            if (inventory.hasItem("Shark") && player.health < 65)
            {
                processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                player.health += 20;
                await Task.Delay(1200);
                lureWild();
            }
            else {
            int checks = 0;
            wild = true;
            if (corpMap())
            {
                processor.addMouseClick(553, 90, "gamescreen");
                await Task.Delay(100);
                processor.addMouseClick(256, 123, "gamescreen");
                await Task.Delay(100);
                while (!insideCave)
                {
                    await Task.Delay(100);
                }
                processor.addMouseClick(258, 234, "gamescreen");
                await Task.Delay(600);
                processor.addMouseClick(553, 90, "gamescreen");
                await Task.Delay(100);
                while (!corpLured())
                {
                    await Task.Delay(600);
                    checks++;
                    Console.WriteLine("Checked " + checks.ToString() + " times");
                    if (checks > 20)
                    {
                        lureWild();
                        break;
                    }
                }
                if (inventory.hasItem("Construction cape"))
                {
                    processor.addMouseClick(inventory.itemx, inventory.itemy);
                }
                while (!welcomeHome())
                {
                    await Task.Delay(10);
                }
                teleportToCorp();
            }
            else
            {
                    processor.addMouseClick(553, 90, "gamescreen"); // turns prayer on
                    await Task.Delay(800);
                    processor.addMouseClick(262, 89, "gamescreen"); //enters room
                    while (!insideCave)
                    {
                        await Task.Delay(10);
                    }
                    await Task.Delay(100);
                    if (corpMap())
                    {
                        processor.addMouseClick(260, 203);
                        await Task.Delay(350);
                    }
                    else
                    {
                        processor.addMouseClick(636, 48, "gamescreen"); // run north
                        await Task.Delay(350);
                        processor.rightClick(260, 203);
                        while (!corpMap())
                        {
                            await Task.Delay(10);
                        }
                        await Task.Delay(300);
                        processor.addMouseClick(260, 233); //click enterance 
                    }
                    while (!atLureSpot())
                    {
                        await Task.Delay(20);
                    }
                    processor.addMouseClick(553, 90, "gamescreen"); //turns prayer off
                    await Task.Delay(100);
                    while (!corpLured())
                    {
                        await Task.Delay(600);
                        checks++;
                        Console.WriteLine("Checked " + checks.ToString() + " times");
                        if (checks > 20)
                        {
                            lureWild();
                            break;
                        }
                    }
                    if (inventory.hasItem("Construction cape"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy);
                    }
                    while (!welcomeHome())
                    {
                        await Task.Delay(10);
                    }
                    teleportToCorp();
                }
        }
        }

        public void scriptDialogue()
        {
            Console.WriteLine("Enter function:");
            string answer = Console.ReadLine();
            switch(answer)
            {
                case "start":
                    startScript();
                    break;
                case "settings":
                    break;
                case "load":
                    break;
                case "save":
                    break;
                default:
                    Console.WriteLine("Valid functions: start, settings, load, save.");
                    scriptDialogue();
                    break;
            }
        }

        public void settings()
        {
            Console.WriteLine("Enter setting: ");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "thralls":
                    thrallsEnabled = !thrallsEnabled;
                    settings();
                    break;
                case "veng":
                    vengEnabled = !vengEnabled;
                    settings();
                    break;
                case "food":
                    settings();
                    break;
                case "special":
                    settings();
                    spec = !spec;
                    break;
                case "special weapon":
                    Console.WriteLine("Special weapon name: ");
                    specWep = Console.ReadLine();
                    Console.WriteLine("Special weapon set to: " + specWep.ToString());
                    settings();
                    break;
                case "main":
                    scriptDialogue();
                    break;
                default:
                    Console.WriteLine("Valid functions: thralls, veng, food, special, special weapon");
                    scriptDialogue();
                    break;
            }
        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        const uint KEY_DOWN_EVENT = 0x0000; // Key down
        const uint KEY_UP_EVENT = 0x0002;   // Key up

        public void PressShift()
        {
            // Simulate right Shift key down (VK_RSHIFT = 0xA1, scan code = 0x36)
            keybd_event((byte)Keys.RShiftKey, 0x36, KEY_DOWN_EVENT, 0);
        }

        public void ReleaseShift()
        {
            // Simulate right Shift key up
            keybd_event((byte)Keys.RShiftKey, 0x36, KEY_UP_EVENT, 0);
        }


        public async void waitForSetup()
        {
            int tick = processor.tick;
            while(tick != 0)
            {
                Console.WriteLine($"{tick.ToString()} : {processor.tick.ToString()}");
                await Task.Delay(10);
                tick = processor.tick;
            }
            Console.WriteLine("On tick 0.");
            await Task.Delay(100);
            processor.moveMouse(256, 93);
            await Task.Delay(100);
            PressShift();         // Press right Shift
            await Task.Delay(50);
            tick = processor.tick;
            processor.instaClick(256, 93);
            while (tick == processor.tick)
            {
                await Task.Delay(10);
            }
            ReleaseShift();
            tick = processor.tick;
            processor.addMouseClick(256, 204);
            while (tick == processor.tick)
            {
                await Task.Delay(10);
            }
            tick = processor.tick;
            processor.addMouseClick(256, 204);
            while (tick == processor.tick)
            {
                await Task.Delay(10);
            }
            tick = processor.tick;
            processor.addMouseClick(256, 204);
        }

        int tempX = 0;

        public async void attack()
        {
            while(!tenthT)
            {
                await Task.Delay(10);
            }
            Console.WriteLine("Clicking door");
            processor.addMouseClick(245, 98, "gamescreen"); //click door
            while(!insideCave)
            {
                await Task.Delay(10);
            }
            Console.WriteLine("inside corp cave");
            processor.addMouseClick(245, 98, "gamescreen"); //attack corp
            attacks++;
            tAttacks++;
            if (attacks > 5 && !firstHit && spellBook == "Lunars")
            {
                veng();
                attacks = 0;
            }
            else
            {
                Console.WriteLine($"Cannot veng or thrall. Attacks:{attacks.ToString()} + firsthit: {firstHit.ToString()} + Spellbook: {spellBook.ToString()}");
            }
            //blockerAttack();
            processor.addMouseClick(309, 198, "gamescreen"); //go into door
            while (!outside() && !atLureSpot()) 
            {
                await Task.Delay(10);
            }
            if (atLureSpot() && !corpLured())
            {
                corpKills++;
                profit += 434000;
                wild = false;
                Console.WriteLine("Total corp kills: " + corpKills.ToString());
                pickUpLoot();
                return;
            }
            processor.addMouseClick(226, 176, "attack");
            await Task.Delay(100);
            if(tAttacks > 7 && attacks < 4) //thrall ready but veng not. Veng > thralls.
            {
                //spellBookSwap();
                //spellBook = "Arceuus";
            }
            player.updateHealth();
            await Task.Delay(10);
            attacksSpecial++;
            if (player.playerHealth() < 75)
            {
                firstStomp = true;
                if (inventory.hasItem("Shark"))
                {
                    processor.addMouseClick(inventory.itemx, inventory.itemy);
                    player.health += 20;
                }
            }
            attack();
        }

        public async void blockerAttack()
        {
            if (firstHit)
            {
                if (third)
                {
                    processor.addMouseClick(935, 162, "gamescreen");
                    await Task.Delay(100);
                }
                else
                {
                    processor.addMouseClick(935, 162, "gamescreen");
                    firstHit = false;
                }
            }
        }

        public async void attackerAttack()
        {
            processor.addMouseClick(98, 604, "attack");
        }

        public async void summonThralls()
        {
            processor.PressKey((byte)Keys.F4, 1);
            await Task.Delay(100);
            processor.addMouseClick(639, 375, "gamescreen");
            await Task.Delay(100);
            processor.PressKey((byte)Keys.F1, 1);
            spellBook = "Lunars";
        }

        public async void spellBookSwap()
        {
            processor.PressKey((byte)Keys.F4, 1);
            await Task.Delay(100);
            processor.addMouseClick(717, 429, "gamescreen");
            await Task.Delay(500);
            processor.PressKey((byte)Keys.F1, 1);
        }

        public async void veng()
        {
            processor.PressKey((byte)Keys.F4, 1);
            await Task.Delay(100);
            processor.addMouseClick(639, 429, "attack");
            await Task.Delay(300);
            processor.PressKey((byte)Keys.F1, 1);
   
        }

        public async void pickUpLoot()
        {
            Console.Beep();
            Console.Write("Corp is dead. Saving instance.");
            //check whos drop it is
            if (third)
            {
                processor.addMouseClick(249, 803, "gamescreen"); //attacker inside
            }
            await Task.Delay(300);
            if (blockerDrop())
            {
                tDrop = false;
                Console.WriteLine("Blocker drop!");
                processor.addMouseClick(962, 206, "gamescreen");
                await Task.Delay(150);
                processor.instaClick(962, 206);
                await Task.Delay(3000);
                processor.addMouseClick(1028, 228, "gamescreen");
                await Task.Delay(150);
                processor.instaClick(1028, 228);
                await Task.Delay(300);
                teleportToBank();
            } else if (tankDrop())
            {
                tDrop = true;
                Console.WriteLine("Tank drop!");
                processor.addMouseClick(257, 107, "gamescreen");
                await Task.Delay(100);
                processor.addMouseClick(1029, 266, "gamescreen");
                while (!insideCave)
                {
                    await Task.Delay(100);
                }
                await Task.Delay(100);
                processor.addMouseClick(226, 145, "gamescreen");
                await Task.Delay(3000);
                processor.addMouseClick(254, 233, "gamescreen");
                await Task.Delay(300);
                teleportToBank();
            } else
            {
                await Task.Delay(100);
                pickUpLoot();
            }

        }

        public bool seeBankChest()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(40, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 394, clientCoords[1] + 38, 0, 0, new Size(40, 30));
                }
                for (int x = 0; x < 40; x++)
                {
                    for (int y = 0; y < 30; y++)
                    {
                        if (bitmap.GetPixel(x, y).R >= 200 && bitmap.GetPixel(x, y).G >= 200 && bitmap.GetPixel(x, y).B == 0)
                        {
                            processor.addMouseClick(x + 394 + 5, y + 38);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool blockerPos()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 640, clientCoords[1] + 56, 0, 0, new Size(10, 10));
                }
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 150 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B > 170)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool corpMap()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(110, 63))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 590, clientCoords[1] + 4, 0, 0, new Size(110, 63));
                }
                for (int x = 0; x < 110; x++)
                {
                    for (int y = 0; y < 63; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 215 && bitmap.GetPixel(x, y).G > 230 && bitmap.GetPixel(x, y).B < 40)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool playerHit()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(15, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 1025, clientCoords[1] + 142, 0, 0, new Size(15, 20));
                }
                for (int x = 0; x < 15; x++)
                {
                    for (int y = 0; y < 20; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 250 && bitmap.GetPixel(x, y).G > 250 && bitmap.GetPixel(x, y).B > 250)
                        {
                            hit = true;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool corpPortal()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(512, 334))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0], clientCoords[1], 0, 0, new Size(512, 334));
                }
                for (int x = 0; x < 512; x++)
                {
                    for (int y = 0; y < 334; y++)
                    {
                        if (bitmap.GetPixel(x, y).R >= 200 && bitmap.GetPixel(x, y).G >= 200 && bitmap.GetPixel(x, y).B == 0)
                        {
                            processor.addMouseClick(x + 10, y + 10);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool corpPortal2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(512, 334))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 774, clientCoords[1], 0, 0, new Size(512, 334));
                }
                for (int x = 0; x < 512; x++)
                {
                    for (int y = 0; y < 334; y++)
                    {
                        if (bitmap.GetPixel(x, y).R >= 200 && bitmap.GetPixel(x, y).G >= 200 && bitmap.GetPixel(x, y).B == 0)
                        {
                            processor.addMouseClick(x + 10 + 774, y + 10);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool tankDrop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 633, clientCoords[1] + 60, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R >= 250 && bitmap.GetPixel(x, y).G < 10 && bitmap.GetPixel(x, y).B < 10)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool tenthT = false;

        public bool tenthTick()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 249, clientCoords[1] + 11, 0, 0, new Size(3, 3));
                }
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 107 && bitmap.GetPixel(x, y).G == 237 && bitmap.GetPixel(x, y).B == 124)
                        {
                            tenthT = true;
                            return true;
                        }
                    }
                }
            }
            tenthT = false;
            return false;
        }

        public bool welcomeHome()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(200, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 161, clientCoords[1] + 266, 0, 0, new Size(200, 20));
                }
                for (int x = 0; x < 200; x++)
                {
                    for (int y = 0; y < 20; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool blockerDrop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 1402, clientCoords[1] + 83, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R >= 250 && bitmap.GetPixel(x, y).G < 10 && bitmap.GetPixel(x, y).B < 10)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool green()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 250, clientCoords[1] + 10, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool instanceMessage()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(150, 15))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 182, clientCoords[1] + 439, 0, 0, new Size(150, 15));
                }
                for (int x = 0; x < 150; x++)
                {
                    for (int y = 0; y < 15; y++)
                    {
                        if (bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool instanceMessage2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(150, 15))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 182 + 774, clientCoords[1] + 439, 0, 0, new Size(150, 15));
                }
                for (int x = 0; x < 150; x++)
                {
                    for (int y = 0; y < 15; y++)
                    {
                        if (bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool atLureSpot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 651, clientCoords[1] + 105, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 225 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool corpLured()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 627, clientCoords[1] + 54, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 215 && bitmap.GetPixel(x, y).G > 230 && bitmap.GetPixel(x, y).B < 40)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool insideCave = false;
        public bool inside()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 650, clientCoords[1] + 121, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 225 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 0)
                        {
                            insideCave = true;
                            return true;
                        }
                    }
                }
            }
            insideCave = false;
            return false;
        }

        public bool outside()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 647, clientCoords[1] + 105, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 225 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool bankOpen()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 53, clientCoords[1] + 19, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 152 && bitmap.GetPixel(x, y).B == 31)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
