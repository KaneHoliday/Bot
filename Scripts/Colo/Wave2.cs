using Bot.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts.Colo
{
    internal class Wave2
    {
        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;
        public Equipment equipment;
        public Prayer prayer;

        public int[] clientCoords = new int[2];
        public int[] prayerCycle = new int[10];

        public int minutes;
        public int seconds;
        public int hours;

        public string npcAttacking = "None";

        public int xpDropCount = 0;
        public int fremsCleared = 0;

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
            Console.WriteLine($"Npc seen: {npcAttacking}");
            await Task.Delay(600);
            updateConsole();
        }

        public async void startScript()
        {
            updateConsole();
            processor.addMouseClick(175, 281, "gamescreen"); //click on the invocation
            int currentTick = processor.tick;
            while (processor.tick == currentTick)
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(435, 294, "gamescreen"); //accept the wave
            processor.tick = 0;
            await Task.Delay(1000);
            while (processor.tick < 5)
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(167, 157, "movement"); //move to the safespot
            await Task.Delay(600);
            prayer.solidRange();
            findNPC();
            await Task.Delay(600);
            while (npcAttacking == "None")
            {
                await Task.Delay(50);
                findNPC();
            }
            killFrems();
        }

        public async void killFrems()
        {
            if (npcAttacking == "Ranger")
            {
                prayer.solidRange();
            }
            else if (npcAttacking == "Mager")
            {
                prayer.solidMagic();
            }
            while (!meleeFrem())
            {
                await Task.Delay(20);
            }
            processor.addMouseClick(240, 166, "gamescreen");
            while (xpDropCount < 2)
            {
                while (!xpDrop())
                {
                    await Task.Delay(100);
                }
                if ((xpDropCount + 1) >= 2)
                {
                    xpDropCount++;
                    break;
                }
                while (xpDrop())
                {
                    await Task.Delay(100);
                }
                xpDropCount++;
            }
            xpDropCount = 0;
            fremsCleared++;
            equipMeleeFrem();
            await Task.Delay(100);
            while (xpDrop())
            {
                await Task.Delay(100);
            }
            processor.addMouseClick(256, 178, "gamescreen"); //attack ranger
            while (!xpDrop())
            {
                await Task.Delay(100);
            }
            inventory.clickItem2("saradomin godsword", 4, true); //2 handed weapon
            await Task.Delay(600);
            processor.addMouseClick(584, 145, "gamescreen"); //special on
            while (xpDrop())
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(256, 178, "gamescreen"); //attack ranger
            while (!xpDrop())
            {
                await Task.Delay(100);
            }
            if (rangerPos4() || rangerPos5())
            {
                processor.addMouseClick(231, 157, "gamescreen"); //attack ranger
                await Task.Delay(1200);
                processor.addMouseClick(600, 339, "prayer"); //pray mage
                await Task.Delay(200);
                equipDPSRangeWeapon();
                solveWave();
                return;
            }
            xpDropCount = 0;
            fremsCleared++;
            await Task.Delay(500);
            equipRangeFrem();
            await Task.Delay(100);
            while (xpDrop())
            {
                await Task.Delay(100);
            }
            if (!magerPos4())
            {
                processor.addMouseClick(270, 165, "gamescreen"); //attack mager
                while (xpDropCount < 2) //minimum 2 attacks to kill mager frem
                {
                    while (!xpDrop())
                    {
                        await Task.Delay(100);
                    }
                    if ((xpDropCount + 1) >= 3)
                    {
                        xpDropCount++;
                        break;
                    }
                    while (xpDrop())
                    {
                        await Task.Delay(100);
                    }
                    xpDropCount++;
                }
                while (!fremDead())
                {
                    await Task.Delay(100);
                }
                xpDropCount = 0;
                Console.WriteLine("Frems dead");
                equipLongRangeWeapon();
                await Task.Delay(500);
            }
            solveWave();
        }

        public async void solveWave()
        {
            if (singleRangerPos1() && magerPos22())
            {
                equipRange();
                await Task.Delay(1000);
                flinchMager();
            } else if (magerPos5())
            {
                await Task.Delay(600);
                processor.addMouseClick(257, 135, "gamescreen");
                while (magerPos5())
                {
                    await Task.Delay(100);
                }
                processor.addMouseClick(674, 340, "prayer");
                await Task.Delay(1000);
                equipMagic();
                await Task.Delay(1000);
                processor.addMouseClick(257, 144, "gamescreen");
            }
            else if (rangerPos5())
            {
                equipMelee();
                await Task.Delay(1200);
                processor.addMouseClick(637, 339, "prayer");
                processor.addMouseClick(280, 105, "gamescreen");
                //killRanger1();
            }
            else if (magerPos5() && rangerMager()) {
                equipMelee();
                await Task.Delay(1000);
                killRangerNextToMager();
                while(!fremDead())
                {
                    await Task.Delay(100);
                }
                equipRange();
                while (!magerPos2())
                {
                    await Task.Delay(100);
                }
                processor.addMouseClick(269, 169, "gamescreen");
                processor.addMouseClick(602, 339, "prayer");
                while(magerPos2())
                {
                    await Task.Delay(100);
                }
                while(!magerPos2())
                {
                    await Task.Delay(100);
                }
                processor.addMouseClick(256, 121, "gamescreen");
                while (magerOnMap())
                {
                    await Task.Delay(600);
                }
                equipMagic();
                await Task.Delay(2400);
            } else if (magerPos1() && rangerPos3())
            {
                Console.WriteLine("mager pos 1");
                processor.addMouseClick(243, 169, "gamescreen");
                await Task.Delay(2400);
                processor.addMouseClick(269, 168, "gamescreen");
                await Task.Delay(600);
                processor.addMouseClick(338, 152, "gamescreen");
                await Task.Delay(600);
                while (magerOnMap())
                {
                    await Task.Delay(600);
                }
                Console.WriteLine("mager dead, getting ready to kill ranger.");
                equipMeleeFrem();
            }
            else if ((rangerPos1() && magerPos2()) || (rangerPos2() && magerPos2()))
            {
                processor.addMouseClick(230, 156, "gamescreen");
                await Task.Delay(600);
                prayer.solidMagic();
                await Task.Delay(600);
                equipRange();
                await Task.Delay(2400);
                processor.addMouseClick(257, 125, "gamescreen");
                //wait for meleer on screen
            }
            else if (magerPos4())
            {
                processor.addMouseClick(284, 165);
                while (magerPos4())
                {
                    await Task.Delay(300);
                }
            }
            else if (magerPos2())
            {
                processor.addMouseClick(646, 80, "gamescreen"); //move 2 squares right, north?
                await Task.Delay(100);
                while (magerPos2())
                {
                    await Task.Delay(100);
                }
                while (!magerPos2())
                {
                    await Task.Delay(100);
                }
                processor.addMouseClick(256, 121, "gamescreen");
                while (magerOnMap())
                {
                    await Task.Delay(300);
                }
                processor.addMouseClick(231, 167, "gamescreen");
                await Task.Delay(1200);
                if (meleeNorth())
                {
                    processor.addMouseClick(222, 156, "gamescreen");
                }
                while (meleeNorth())
                {
                    await Task.Delay(100);
                }
                equipMeleeFrem();
            }
        }
        public async void flinchMagerNorthPillar()
        {
            processor.addMouseClick(640, 339, "gamescreen"); //pray range
            await Task.Delay(600);
            if (!fremDead())
            {
                processor.addMouseClick(230, 165, "gamescreen");
            }
            while (magerPos2())
            {
                await Task.Delay(10);
            }
            await Task.Delay(100);
            processor.addMouseClick(600, 339, "gamescreen"); //pray mage
            await Task.Delay(600);
            processor.addMouseClick(256, 120, "gamescreen");
            processor.addMouseClick(600, 339, "gamescreen"); //pray mage
            await Task.Delay(1000);
            processor.addMouseClick(283, 167, "gamescreen");
            await Task.Delay(3000);
            flinchMagerNorthPillar();
        }

        public async void flinchMager()
        {
            await Task.Delay(300);
            while(processor.tick != 4)
            {
                await Task.Delay(10);
            }
            if (magerPos22())
            {
                processor.addMouseClick(267, 167, "gamescreen");
            } else
            {
                Console.WriteLine("Mager dead");
                return;
                //mager dead
            }
            prayer.solidMagic();
            while (processor.currentTick != 5)
            {
                await Task.Delay(10);
            }
            prayer.solidRange();
            while (processor.currentTick != 6)
            {
                await Task.Delay(10);
            }
            await Task.Delay(600);
            processor.addMouseClick(244, 167, "gamescreen");
            prayer.turnOff();
            await Task.Delay(3000);
            //flinchMager();
        }

        public async void killRangerSouth()
        {
            processor.addMouseClick(637, 339, "prayer");
            processor.addMouseClick(327, 147, "gamescreen");
            while (!northOfSouthRanger())
            {
                await Task.Delay(100);
            }
            loopAttack3();
            checkLoop();
        }

        public bool westR = false;
        public bool northR = false;

        public async void checkLoop()
        {
            northOfSouthRanger();
            westOfSouthRanger();
            rangerEastCheck();
            rangerNorthCheck();
            singleRangerPos1();
            await Task.Delay(100);
            checkLoop();
        }
        public async void loopAttack3()
        {
            processor.addMouseClick(270, 183, "gamescreen");
            while (!westR || fremDead())
            {
                await Task.Delay(100);
            }
            if (!fremDead())
            {
                processor.addMouseClick(271, 135, "gamescreen"); //attack
            }
            else
            {
                processor.addMouseClick(233, 155, "gamescreen"); //safespot
            }
            while(!westR)
            {
                await Task.Delay(10);
            }
            await Task.Delay(1000);
            processor.addMouseClick(244, 155, "gamescreen");
            while (!northR || fremDead())
            {
                await Task.Delay(100);
            }
            if (!fremDead())
            {
                processor.addMouseClick(284, 150, "gamescreen"); //attack
            }
            else
            {
                processor.addMouseClick(244, 167, "gamescreen"); //safespot
            }
            while(!northR)
            {
                await Task.Delay(10);
            }
            await Task.Delay(1000);
            loopAttack3();
        }

        public async void killRangerNextToMager()
        {
            processor.addMouseClick(637, 339, "prayer");
            processor.addMouseClick(280, 106, "gamescreen");
            while(!rangerMiddleEast())
            {
                await Task.Delay(100);
            }
            loopAttack2();
        }
        public async void loopAttack2()
        {
            processor.addMouseClick(268, 165, "gamescreen");
            while (!rangerEast)
            {
                await Task.Delay(100);
            }
            if (!fremDead())
            {
                processor.addMouseClick(243, 137, "gamescreen"); //attack
            } else
            {
                processor.addMouseClick(244, 209, "gamescreen"); //safespot
            }
            await Task.Delay(2000);
            processor.addMouseClick(242, 171, "gamescreen");
            while (!rangerMiddleEast())
            {
                await Task.Delay(100);
            }
            if (!fremDead())
            {
                processor.addMouseClick(255, 140, "gamescreen"); //attack
            } else
            {
                processor.addMouseClick(240, 207, "gamescreen"); //safespot
            }
            await Task.Delay(2000);
            loopAttack2();
        }

        public bool singleRangeP1 = false;
        public bool rangerEast = false;
        public bool rangerNorth = false;

        public async void killSingleRanger1()
        {
            processor.addMouseClick(283, 169, "gamescreen");
            prayer.solidRange();
            while (singleRangeP1)
            {
                await Task.Delay(10);
            }
            while (!singleRangeP1)
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(243, 105, "gamescreen");
            while(!rangerEast)
            {
                await Task.Delay(10);
            }
            await Task.Delay(1000);
            processor.addMouseClick(269, 158, "gamescreen");
            while(!rangerNorth)
            {
                await Task.Delay(10);
            }
            loopAttack();
        }

        public bool rangerOnMap()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(106, 86))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 588, clientCoords[1] + 30, 0, 0, new Size(106, 86));
                }

                for (int x = 0; x < 105; x++)
                {
                    for (int y = 0; y < 85; y++)
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

        public async void loopAttack()
        {
            Console.WriteLine("Loop");
            if (rangerOnMap())
            {
                processor.addMouseClick(229, 151, "gamescreen"); //attack
            }
            else
            {
                processor.addMouseClick(225, 221, "gamescreen"); //safespot
            }
            await Task.Delay(1000);
            processor.addMouseClick(241, 182, "gamescreen");
            while (!rangerEast)
            {
                await Task.Delay(10);
            }
            if (rangerOnMap())
            {
                processor.addMouseClick(243, 137, "gamescreen"); //attack
            } else
            {
                processor.addMouseClick(244, 209, "gamescreen"); //safespot
            }
            await Task.Delay(1000);
            processor.addMouseClick(274, 156, "gamescreen");
            while (!rangerNorth)
            {
                await Task.Delay(10);
            }
            loopAttack();
        }

        public async void testPrayers()
        {
            processor.tick = 0;
            prayer.prayerArray[1] = 1;
            prayer.prayerArray[2] = 2;
        }

        public async void flickRangerMager()
        {

        }
        public async void equipMagicFrem()
        {
            inventory.clickItem2("staff of light", 4);
            inventory.clickItem2("tome of fire", 6);
            inventory.clickItem2("occult necklace", 2);
            inventory.clickItem2("torags legs", 7);
            inventory.clickItem2("ahrims top", 5);
            inventory.clickItem2("tormented bracelet", 8);
        }
        public async void equipMagic()
        {
            inventory.clickItem2("staff of light", 4);
            inventory.clickItem2("ahrims robetop", 5);
            inventory.clickItem2("ahrims robeskirt", 7);
            inventory.clickItem2("tome of fire", 6);
            inventory.clickItem2("occult necklace", 2);
            inventory.clickItem2("tormented bracelet", 8);
        }
        public async void equipRangeFrem()
        {
            inventory.clickItem2("necklace of anguish", 2);
            inventory.clickItem2("eclipse moon chestplate", 5);
            inventory.clickItem2("eclipse moon tassets", 7);
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrow", 3);
            inventory.clickItem2("regen bracelet", 8);
        }
        public async void equipRange()
        {
            inventory.clickItem2("necklace of anguish", 2);
            inventory.clickItem2("eclipse moon chestplate", 5);
            inventory.clickItem2("eclipse moon tassets", 7);
            inventory.clickItem2("eclipse atlatl", 4, true);
            inventory.clickItem2("atlatl dart", 3);
            inventory.clickItem2("regen bracelet", 8);
        }
        public async void equipMeleeFrem()
        {
            inventory.clickItem2("abyssal whip", 4);
            inventory.clickItem2("amulet of fury", 2);
            inventory.clickItem2("torags legs", 7);
            inventory.clickItem2("regen bracelet", 8);
            inventory.clickItem2("dragon defender", 6);
        }

        public async void equipLongRangeWeapon()
        {
            inventory.clickItem2("eclipse atlatl", 4, true);
            inventory.clickItem2("atlatl dart", 3);
        }
        public async void equipDPSRangeWeapon()
        {
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrow", 3);
        }
        public async void equipMelee()
        {
            inventory.clickItem2("amulet of fury", 2);
            inventory.clickItem2("eclipse moon chestplate", 5);
            inventory.clickItem2("torags legs", 7);
            inventory.clickItem2("dragon defender", 6);
            inventory.clickItem2("abyssal whip", 4);
            inventory.clickItem2("regen bracelet", 8);
        }
        public bool meleeNorth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 627, clientCoords[1] + 77, 0, 0, new Size(5, 5));
                }

                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPos1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 673, clientCoords[1] + 63, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool atMiddleTile()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 673, clientCoords[1] + 106, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPos3()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(15, 15))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 664, clientCoords[1] + 67, 0, 0, new Size(15, 15));
                }

                for (int x = 0; x < 14; x++)
                {
                    for (int y = 0; y < 14; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool rangerMager()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 645, clientCoords[1] + 59, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool rangerMiddleEast()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 637, clientCoords[1] + 71, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool northOfSouthRanger()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 645, clientCoords[1] + 75, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            northR = true;
                            return true;
                        }
                    }
                }
            }
            northR = false;
            return false;
        }
        public bool hitSplat()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 239, clientCoords[1] + 127, 0, 0, new Size(10, 10));
                }

                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
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
        public bool hitSplat2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 226, clientCoords[1] + 143, 0, 0, new Size(10, 10));
                }

                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
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
        public bool westOfSouthRanger()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 641, clientCoords[1] + 71, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            westR = true;
                            return true;
                        }
                    }
                }
            }
            westR = false;
            return false;
        }
        public bool rangerEastCheck()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 633, clientCoords[1] + 71, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            rangerEast = true;
                            return true;
                        }
                    }
                }
            }
            rangerEast = false;
            return false;
        }
        public bool rangerNorthCheck()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 629, clientCoords[1] + 75, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            rangerNorth = true;
                            return true;
                        }
                    }
                }
            }
            rangerNorth = false;
            return false;
        }
        public bool rangerPos1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 673, clientCoords[1] + 63, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool rangerPos2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 661, clientCoords[1] + 67, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool magerOnMap()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(106, 86))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 588, clientCoords[1] + 30, 0, 0, new Size(106, 86));
                }

                for (int x = 0; x < 105; x++)
                {
                    for (int y = 0; y < 85; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPos4()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 645, clientCoords[1] + 79, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPos5()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(6, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 634, clientCoords[1] + 65, 0, 0, new Size(6, 6));
                }

                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPosDown1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 657, clientCoords[1] + 75, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPosUp1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 657, clientCoords[1] + 71, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerOnTop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(100, 50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 589, clientCoords[1] + 25, 0, 0, new Size(100, 50));
                }

                for (int x = 0; x < 48; x++)
                {
                    for (int y = 0; y < 48; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
                for (int x = 0; x < 99; x++)
                {
                    for (int y = 0; y < 24; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool fremDead()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 7, clientCoords[1] + 48, 0, 0, new Size(5, 5));
                }

                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (bitmap.GetPixel(x, y).R >= 90 && bitmap.GetPixel(x, y).R <= 110 && bitmap.GetPixel(x, y).G < 30 && bitmap.GetPixel(x, y).B < 30)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool xpDrop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(19, 60))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 492, clientCoords[1] + 80, 0, 0, new Size(19, 60));
                }
                for (int x = 0; x < 19; x++)
                {
                    for (int y = 0; y < 59; y++)
                    {
                        if (bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool meleeFrem()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 633, clientCoords[1] + 79, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            return true;
                        }
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool singleRangerPos1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 633, clientCoords[1] + 59, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            singleRangeP1 = true;
                            return true;
                        }
                    }
                }
            }
            singleRangeP1 = false;
            return false;
        }
        public bool magerPos2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 637, clientCoords[1] + 63, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPos22()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 637, clientCoords[1] + 51, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool rangerPos3()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 637, clientCoords[1] + 63, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool rangerPos4()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(15, 15))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 663, clientCoords[1] + 82, 0, 0, new Size(15, 15));
                }

                for (int x = 0; x < 14; x++)
                {
                    for (int y = 0; y < 14; y++)
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
        public bool rangerPos5()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 645, clientCoords[1] + 79, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool rangerPos6()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 669, clientCoords[1] + 63, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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

        public bool findNPC()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(100, 120))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 616, clientCoords[1] + 15, 0, 0, new Size(100, 120));
                }

                for (int x = 99; x > 2; x--)
                {
                    for (int y = 0; y < 119; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            npcAttacking = "Mager";
                            return true;
                        }
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            npcAttacking = "Ranger";
                            return true;
                        }
                    }
                }
            }
            Console.WriteLine("Couldnt find npc");
            return false;
        }

    }
}
