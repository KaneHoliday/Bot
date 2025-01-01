using Bot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts
{
    internal class GOTR
    {
        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;
        public string currentAltar;
        public string tempAltar;

        public int[] clientCoords = new int[2];

        public int fillCount = 2;

        public async void startScript()
        {
            //ask ele, cat or both
            if (interfaces.gotrStarted())
            {
                Console.WriteLine(fillCount.ToString() + ": Clicking rock");
                processor.addMouseClick(242, 168, "gamescreen");
                while (!xpDrops.gettingXp())
                {
                    await Task.Delay(10);
                }
                Console.WriteLine(fillCount.ToString() + ": Got Xp Drop");
                while (xpDrops.gettingXp())
                {
                    await Task.Delay(10);
                }
                Console.WriteLine(fillCount.ToString() + ": Xp for mining capped");
                processor.addMouseClick(328, 177, "gamescreen");
                Console.WriteLine(fillCount.ToString() + ": Clicking on bench");
                craftEss();
            }
            else
            {
                await Task.Delay(100);
                startScript();
            }
        }

        public async void craftEss()
        {
            if (interfaces.inventoryFull())
            {
                if(inventory.hasItem("Colo pouch"))
                {
                    processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                    if(fillCount == 2)
                    {
                        enterPortal();
                        return;
                    }
                    Console.WriteLine(fillCount.ToString() + ": Filling pouch");
                    fillCount++;
                    await Task.Delay(300);
                    processor.addMouseClick(258, 178, "gamescreen");
                    await Task.Delay(1200);
                    craftEss();
                } else
                {
                    Console.WriteLine(fillCount.ToString() + ": No colo pouch");
                }
            } else
            {
                await Task.Delay(100);
                craftEss();
            }
        }

        public async void enterPortal()
        {
            Console.WriteLine(fillCount.ToString() + ": Ready to enter portal");
            checkCurrentAltars();
            while(currentAltar == "")
            {
                await Task.Delay(100);
            }
            Console.WriteLine(fillCount.ToString() + ": Current altar: " + currentAltar);
            if(tenSeconds())
            {
                checkCurrentAltars();
                checkAltars();
                return;
            }
            tempAltar = currentAltar;
            waitForChange();
        }

        public async void waitForChange()
        {
            checkCurrentAltars();
            if(currentAltar != tempAltar)
            {
                checkAltars();
            } else
            {
                await Task.Delay(400);
                waitForChange();
            }
        }
        public async void checkCurrentAltars()
        {
            if (airAltar())
            {
                currentAltar = "Air";
                return;
            }
            if (fireAltar())
            {
                currentAltar = "Fire";
                return;
            }
            if (waterAltar())
            {
                currentAltar = "Water";
                return;
            }
            if (deathAltar())
            {
                currentAltar = "Death";
                return;
            }
            if (natureAltar())
            {
                currentAltar = "Nature";
                return;
            }
            if (cosmicAltar())
            {
                currentAltar = "Cosmic";
                return;
            }
            if (chaosAltar())
            {
                currentAltar = "Chaos";
                return;
            }
            if (bodyAltar())
            {
                currentAltar = "Body";
                return;
            }
            if (mindAltar())
            {
                currentAltar = "Mind";
                return;
            }
            if (earthAltar())
            {
                currentAltar = "Earth";
                return;
            }
            if (lawAltar())
            {
                currentAltar = "Law";
                return;
            }
        }

        public async void checkAltars()
        {
            if (deathAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See death altar");
                processor.addMouseClick(214, 9, "gamescreen");
                craftDeathRunes();
                return;
            }
            if (natureAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See nature altar");
                processor.addMouseClick(342, 3, "gamescreen");
                craftNatureRunes();
                return;
            }
            if (lawAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See law altar");
                processor.addMouseClick(236, 5, "gamescreen");
                craftLawRunes();
                return;
            }
            if (chaosAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See chaos altar");
                processor.addMouseClick(207, 40, "gamescreen");

            }
            if (airAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See air altar");
                processor.addMouseClick(339, 84, "gamescreen");
                craftAirRunes();
                return;
            }
            if (mindAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See mind altar");
                processor.addMouseClick(276, 81, "gamescreen");
                craftMindRunes();
                return;
            }
            if (waterAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See water altar");
                processor.addMouseClick(396, 26, "gamescreen");
                craftWaterRunes();
                return;
            }
            //if (fireAltar())
            //{
            //    Console.WriteLine(fillCount.ToString() + ": See fire altar. Adding later");
            //}
            if (cosmicAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See cosmic altar");
                //processor.addMouseClick(379, 75, "gamescreen");
                //craftCosmicRunes(); add this later
            }
            if (bodyAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See body altar");
                processor.addMouseClick(228, 71, "gamescreen");
                craftBodyRunes();
                return;
            }
            if(earthAltar())
            {
                Console.WriteLine(fillCount.ToString() + ": See earth altar");
                processor.addMouseClick(368, 23, "gamescreen");
                craftEarthRunes();
                return;
            }
            await Task.Delay(600);
        }

        public async void deposit()
        {
            while(!atGuardian())
            {
                await Task.Delay(100);
            }
            await Task.Delay(1000);
            processor.addMouseClick(257, 43, "gamescreen");
            await Task.Delay(200);
            processor.rightClick(169, 229);
            while(!xpDrops.gettingXp())
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(137, 250, "gamescreen");
            while(!atPool())
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(294, 187, "gamescreen");
            fillCount = 0;
            craftEss();
        }

        public async void craftAirRunes()
        {
            if(insideAirAltar())
            {
                Console.WriteLine(fillCount.ToString());
                await Task.Delay(1000);
                processor.addMouseClick(292, 111, "gamescreen");
                for(int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(269, 143, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                processor.addMouseClick(225, 222, "gamescreen");
                deposit();
            } else
            {
                await Task.Delay(100);
                craftAirRunes();
            }
        }

        public async void craftChaosRunes()
        {
            if (insideChaosAltar())
            {
                Console.WriteLine(fillCount.ToString());
                await Task.Delay(1000);
                processor.addMouseClick(136, 112, "gamescreen");
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(227, 151, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                processor.addMouseClick(392, 221, "gamescreen");
                deposit();
            }
            else
            {
                await Task.Delay(100);
                craftChaosRunes();
            }
        }

        public async void craftLawRunes()
        {
            if (insideLawAltar())
            {
                Console.WriteLine(fillCount.ToString());
                await Task.Delay(1000);
                processor.addMouseClick(256, 51, "gamescreen");
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(257, 126, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                processor.rightClick(558, 17);
                await Task.Delay(200);
                processor.addMouseClick(564, 76, "gamescreen");
                await Task.Delay(200);
                processor.addMouseClick(256, 38, "gamescreen");
                await Task.Delay(200);
                processor.rightClick(558, 17);
                await Task.Delay(200);
                processor.addMouseClick(562, 47, "gamescreen");
                deposit();
                Console.WriteLine(fillCount.ToString() + ": Done runecrafting");
            }
            else
            {
                await Task.Delay(100);
                craftLawRunes();
            }
        }


        public async void craftCosmicRunes()
        {
            if (insideCosmicAltar())
            {
                await Task.Delay(1000);
                processor.addMouseClick(257, 13, "gamescreen");
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(258, 116, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                Console.WriteLine(fillCount.ToString() + ": Done runecrafting");

            }
            else
            {
                craftCosmicRunes();
            }
        }

        public async void craftWaterRunes()
        {
            if (insideWaterAltar())
            {
                await Task.Delay(1000);
                processor.addMouseClick(114, 98, "gamescreen");
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(212, 136, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                Console.WriteLine(fillCount.ToString() + ": Done runecrafting");
                processor.addMouseClick(370, 211, "gamescreen");
                deposit();
            }
            else
            {
                craftWaterRunes();
            }
        }

        public async void craftNatureRunes()
        {
            if (insideNatureAltar())
            {
                await Task.Delay(1000);
                processor.addMouseClick(255, 102, "gamescreen");
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(256, 236, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                Console.WriteLine(fillCount.ToString() + ": Done runecrafting");
                processor.addMouseClick(256, 237, "gamescreen");
                deposit();
            }
            else
            {
                craftNatureRunes();
            }
        }

        public async void craftBodyRunes()
        {
            if (insideBodyAltar())
            {
                await Task.Delay(1000);
                processor.addMouseClick(278, 98, "gamescreen");
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(275, 131, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                Console.WriteLine(fillCount.ToString() + ": Done runecrafting");
                processor.addMouseClick(241, 239, "gamescreen");
                deposit();
            }
            else
            {
                craftBodyRunes();
            }
        }

        public async void craftDeathRunes()
        {
            if (insideDeathAltar())
            {
                await Task.Delay(1000);
                processor.addMouseClick(226, 103, "gamescreen"); //craft runes
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(234, 143, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                Console.WriteLine(fillCount.ToString() + ": Done runecrafting");
                processor.addMouseClick(285, 237, "gamescreen");
                deposit();
            }
            else
            {
                craftDeathRunes();
            }
        }
        public async void craftMindRunes()
        {
            if (insideMindAltar())
            {
                await Task.Delay(1000);
                processor.addMouseClick(186, 46, "gamescreen");
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(243, 139, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                Console.WriteLine(fillCount.ToString() + ": Done runecrafting");
                processor.rightClick(558, 17);
                await Task.Delay(200);
                processor.addMouseClick(564, 76, "gamescreen");
                await Task.Delay(200);
                processor.addMouseClick(198, 56, "gamescreen");
                await Task.Delay(200);
                processor.rightClick(558, 17);
                await Task.Delay(200);
                processor.addMouseClick(562, 47, "gamescreen");
                deposit();
            }
            else
            {
                craftMindRunes();
            }
        }

        public async void craftEarthRunes()
        {
            if (insideEarthAltar())
            {
                await Task.Delay(1000);
                processor.addMouseClick(283, 17, "gamescreen");
                for (int i = 0; i < fillCount; i++)
                {
                    while (!xpDrops.gettingXp())
                    {
                        await Task.Delay(10);
                    }
                    if (inventory.hasItem("Colo pouch"))
                    {
                        processor.addMouseClick(inventory.itemx, inventory.itemy, "inventory");
                        while (!inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        processor.addMouseClick(282, 129, "gamescreen");
                        while (inventory.hasItem("Pouch ess"))
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine(fillCount.ToString() + ": Runecrafting more");
                    }
                    await Task.Delay(600);
                }
                Console.WriteLine(fillCount.ToString() + ": Done runecrafting");
                processor.addMouseClick(227, 314, "gamescreen");
                deposit();
            }
            else
            {
                craftEarthRunes();
            }
        }


        //if gotr started
        //click rock, mine until no further xp drops
        //make rune ess and wait for portal
        //click portal
        //craft runes
        //tele out
        //click guardian and right click deposit pool
        //click deposit pool
        //mine or craft again until end of game
        //repeat

        public bool atGuardian()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 637, clientCoords[1] + 31, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 240 && bitmap.GetPixel(x, y).G > 240 && bitmap.GetPixel(x, y).B < 10)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool tenSeconds()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(1,1))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 91, clientCoords[1] + 76, 0, 0, new Size(1,1));
                }
                for (int x = 0; x < 1; x++)
                {
                    for (int y = 0; y < 1; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 240 && bitmap.GetPixel(x, y).G > 240 && bitmap.GetPixel(x, y).B > 240)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool atPool()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 666, clientCoords[1] + 98, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 187 && bitmap.GetPixel(x, y).G == 13 && bitmap.GetPixel(x, y).B == 5)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool insideAirAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 603, clientCoords[1] + 55, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(100, 85, 47))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool insideLawAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 639, clientCoords[1] + 127, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(0, 0, 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool insideChaosAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 634, clientCoords[1] + 49, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(0, 0, 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool insideMindAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 635, clientCoords[1] + 128, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(113, 103, 102))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool insideDeathAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 637, clientCoords[1] + 139, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(0, 0, 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool insideBodyAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 662, clientCoords[1] + 118, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(0, 0, 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool insideNatureAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 597, clientCoords[1] + 42, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(100, 85, 47))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool insideEarthAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 588, clientCoords[1] + 74, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(90, 67, 23))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool insideCosmicAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 633, clientCoords[1] + 72, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(74, 71, 67))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool insideWaterAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 701, clientCoords[1] + 73, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(90, 109, 151))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool airAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 49, clientCoords[1] + 76, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool chaosAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 125, clientCoords[1] + 85, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(39, 42, 56))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool waterAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 48, clientCoords[1] + 68, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(4, 30, 80))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool bloodAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 129, clientCoords[1] + 81, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(190, 38, 51))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool mindAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 120, clientCoords[1] + 77, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(218, 118, 20))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool fireAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 48, clientCoords[1] + 80, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(233, 14, 29))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool earthAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 47, clientCoords[1] + 70, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(157, 100, 58))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool bodyAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 128, clientCoords[1] + 64, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(4, 30, 80))
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public bool cosmicAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 129, clientCoords[1] + 68, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(255, 255, 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool lawAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 126, clientCoords[1] + 68, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(30, 85, 232))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool natureAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 126, clientCoords[1] + 68, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(62, 206, 39))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool deathAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 129, clientCoords[1] + 67, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
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
