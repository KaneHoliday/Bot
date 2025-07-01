using Bot.Core;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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

        public int tick = 0;
        public bool waveComplete = false;

        public int currentCycle;

        public int doorPosX = 0;
        public int doorPosY = 0;
        public bool firstRun = true;

        public int minutes;
        public int seconds;
        public int hours;

        public int claims;

        public int health = 99;

        public int fremsCleared = 0;
        public bool wave3tests = false;

        public int waveTicks = 0;
        public int meleeSkips = 0;

        public int chestPosX = 0;
        public int chestPosY = 0;

        public int bankPosX = 0;
        public int bankPosY = 0;

        public int[] waveTimes = new int[1000];

        public string npcAttacking = "None";

        public void loadTimes()
        {
            string filePath = "output.txt";

            string content = File.ReadAllText(filePath);

            string[] numberStrings = content.Split(',');

            waveTimes = numberStrings.Select(int.Parse).ToArray();
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

        public async void dataLoop()
        {
            while (!atMiddlePilar())
            {
                await Task.Delay(100);
            }
            Console.WriteLine("At middle pillar.");
            captureScreen();
            claims++;
            while (!leaveInterface())
            {
                await Task.Delay(100);
            }
            Console.WriteLine("At leave interface.");
            dataLoop();
        }

        public async void updateConsole()
        {
            //Console.Clear();
            if (seconds < 10)
            {
                if (minutes < 10)
                {
                    Console.WriteLine("Time running: " + hours.ToString() + ":0" + minutes.ToString() + ":0" + seconds.ToString());
                }
                else
                {
                    Console.WriteLine("Time running: " + hours.ToString() + ":" + minutes.ToString() + ":0" + seconds.ToString());
                }
            }
            if (seconds > 9)
            {
                if (minutes < 10)
                {
                    Console.WriteLine("Time running: " + hours.ToString() + ":0" + minutes.ToString() + ":" + seconds.ToString());
                }
                else
                {
                    Console.WriteLine("Time running: " + hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString());
                }
            }
            Console.WriteLine("Waves claimed: " + claims.ToString());
            //Console.WriteLine("Xp gained: " + xpGained.ToString());
            Console.WriteLine("Profit Made: " + (claims * 40000).ToString());
            Console.WriteLine("Melee skips: " + meleeSkips.ToString());
            Console.WriteLine($"Cons: M1: ${m1.ToString()}, m2: ${m2.ToString()}, m3: ${m3.ToString()}, m4: ${m4.ToString()}, m5: ${m5.ToString()}, m8: ${m8.ToString()}, m9: ${m9.ToString()}, m10: ${m10.ToString()}, m11: ${m11.ToString()}");
            if (processor.tick1())
            {
                while (processor.tick1())
                {
                    await Task.Delay(100);
                }
            }
            else if (processor.tick2())
            {
                while (processor.tick2())
                {
                    await Task.Delay(100);
                }
            }
            await Task.Delay(1200);
            updateConsole();
        }

        public async void startScript()
        {
            prayer.boostPrayer = 0;
            prayer.activePrayer = 0;
            Console.WriteLine("Starting script");
            updateConsole();
            //processor.addMouseClick(175, 281, "prayer"); //click on the invocation
            await Task.Delay(200);
            processor.addMouseClick(435, 294, "prayer"); //accept the wave
            await Task.Delay(200);
            processor.addMouseClick(287, 182, "prayer"); //walk to spawn tile
            await Task.Delay(200);
            processor.addMouseClick(531, 148, "prayer");
            await Task.Delay(200);
            processor.addMouseClick(531, 148, "prayer");
            await Task.Delay(200);
            processor.addMouseClick(531, 148, "prayer");
            await Task.Delay(200);
            processor.addMouseClick(206, 155, "prayer"); //move to the safespot
            await Task.Delay(200);
            prayer.solidRange();
            findNPC();
            await Task.Delay(600);
            while (npcAttacking == "None")
            {
                await Task.Delay(50);
                findNPC();
            }
            killFrems();
            if(npcAttacking == "Mager")
            {
                prayer.solidMagic();
            }
        }



        public int rangePos = 0;

        public async void waitForStartLocation()
        {
            prayer.solidMagic();
            if (waveTicks > 100)
            {
                //saveTimes();
                //waveTimes[claims - 1] = waveTicks;
                //waveTicks = 0;
            }
            await Task.Delay(600);
            equipMagicFrem();
            while (!atMiddlePilar())
            {
                await Task.Delay(100);
            }
            //rightClickMeleeFrem();
            //captureScreen();
            killFrems();
        }

        public void captureScreen()
        {
            Console.WriteLine("Screenshotting the client.");
            using (Bitmap bitmap = new Bitmap(770, 510))
            {
                // Create a graphics object to draw the screenshot
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Capture the screen area starting at (x, y)
                    graphics.CopyFromScreen(clientCoords[0], clientCoords[1], 0, 0, new Size(770, 510));
                }
                // Get the path to the Pictures folder
                string picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                // Define the claims folder path
                string claimsFolder = Path.Combine(picturesPath, "claims");
                // Ensure the claims folder exists
                if (!Directory.Exists(claimsFolder))
                {
                    Directory.CreateDirectory(claimsFolder);
                }
                // Define the file path inside the claims folder
                string filePath = Path.Combine(claimsFolder, $"claim {(claims + 1).ToString()}.png");
                // Save the screenshot as a PNG file in the claims folder
                bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        public void saveTimes()
        {
            string filePath = "output.txt";

            string content = string.Join(",", waveTimes);

            File.WriteAllText(filePath, content);

        }

        public async void setFremPrayers()
        {
            if (fremsCleared < 1)
            {
                prayer.solidMelee();
            }
            if (fremsCleared < 2)
            {
                prayer.solidRange();
            }
            if (fremsCleared < 3)
            {
                prayer.solidMagic();
            }
        }

        public async void rightClickMeleeFrem()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(326, 112))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 138, clientCoords[1] + 39, 0, 0, new Size(326, 112));
                }

                for (int x = 0; x < 325; x++)
                {
                    for (int y = 0; y < 111; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            processor.rightClick(x + 4 + 138, y - 4 + 39);
                            await Task.Delay(100);
                            leftClickAttack(x + 4 + 138, y - 4 + 39);
                            return;
                        }
                    }
                }
            }
        }

        public bool leftClickAttack(int i, int j)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(24, 63))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + i + 146, clientCoords[1] + j, 0, 0, new Size(24, 63));
                }

                for (int x = 0; x < 23; x++)
                {
                    for (int y = 0; y < 62; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G >= 250 && bitmap.GetPixel(x, y).B == 0)
                        {
                            clickMeleeFrem(i, j + y);
                            return true;
                        }
                    }
                }
            }
            processor.addMouseClick(i, j); //did not right click melee frem
            return false;
        }

        public void clickMeleeFrem(int i, int j)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(17, 16))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 630, clientCoords[1] + 71, 0, 0, new Size(17, 16));
                }

                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 15; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            processor.addMouseClick(i, j);
                            return;
                        }
                    }
                }
            }
        }

        public int xpDropCount = 0;

        public bool meleeFrem = false;
        public bool xpDrop = false;
        public bool fremDead = false;
        public bool popup = false;

        public int m1 = 0;
        public int m2 = 0;
        public int m3 = 0;
        public int m4 = 0;
        public int m5 = 0;
        public int m6 = 0;
        public int m7 = 0;
        public int m8 = 0;
        public int m9 = 10;
        public int m10 = 0;
        public int m11 = 0;

        public async void checkLoop()
        {
            meleeFremCheck();
            xpDropCheck();
            fremDeadCheck();
            popupCheck();
            await Task.Delay(20);
            checkLoop();
        }

        public bool moveSkip = false;

        public async void killFrems()
        {
            xpDropCount = 0;
            equipMagicFrem();
            while (!meleeFrem)
            {
                await Task.Delay(10);
            }
            if (magePos == 4)
            {
                return;
            }
            processor.addMouseClick(240, 166, "attack");
            while (xpDropCount < 2)
            {
                while (!xpDrop)
                {
                    await Task.Delay(10);
                }
                if ((xpDropCount + 1) >= 2)
                {
                    xpDropCount++;
                    break;
                }
                while (xpDrop)
                {
                    await Task.Delay(10);
                }
                xpDropCount++;
            }
            for (int i = 0; i < processor.inventory.inventory.Length; i++)
            {
                if (processor.inventory.inventory[i] == "super combat potion (1)")
                {
                    inventory.clickItem2("super combat potion (1)", 98);
                    break;
                }
                else if (processor.inventory.inventory[i] == "super combat potion (2)")
                {
                    inventory.clickItem2("super combat potion (2)", 98);
                    break;
                }
                else if (processor.inventory.inventory[i] == "super combat potion (3)")
                {
                    inventory.clickItem2("super combat potion (3)", 98);
                    break;
                }
                else if (processor.inventory.inventory[i] == "super combat potion (4)")
                {
                    inventory.clickItem2("super combat potion (4)", 98);
                    break;
                }
            }
            xpDropCount = 0;
            fremsCleared++;
            equipMeleeFrem();
            while (xpDrop)
            {
                await Task.Delay(10);
            }
            prayer.prayPiety(); //turn on piety
            while (!canSpec())
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(584, 145, "gamescreen"); //special on
            await Task.Delay(10);
            processor.addMouseClick(256, 178, "attack"); //attack ranger
            while (!xpDrop)
            {
                await Task.Delay(10);
            }
            //summonThrall(); only enable if doing wave 2 too.
            xpDropCount = 0;
            fremsCleared++;
            if (rangerPos1() || rangerPos4() || rangerPos5())
            {
                processor.addMouseClick(233, 144, "gamescreen"); //move around pillar
                await Task.Delay(200);
                equipDPSRangeWeapon();
                Console.WriteLine("Danger");
                solveWave(1);
            } else
            {
                if (magePos == 4)
                {
                    equipRangeFrem(); //has ven bow as weapon
                    while(xpDrop)
                    {
                        await Task.Delay(100);
                    }
                    await Task.Delay(10);
                    processor.addMouseClick(284, 168, "attack");
                    await Task.Delay(10);
                    prayer.prayRigour();
                    xpDropCount = 0;
                    while (xpDropCount < 3) //minimum 3 attacks to kill mager frem
                    {
                        while (!xpDrop)
                        {
                            await Task.Delay(10);
                        }
                        if ((xpDropCount + 1) >= 3)
                        {
                            xpDropCount++;
                            break;
                        }
                        while (xpDrop)
                        {
                            await Task.Delay(10);
                        }
                        xpDropCount++;
                    }
                    xpDropCount = 0;
                    equipLongRangeWeapon();
                    await Task.Delay(20);
                    processor.addMouseClick(284, 168, "attack");
                } else
                {
                    equipRangeFrem();
                    processor.addMouseClick(272, 168, "attack");
                    prayer.prayRigour();
                    while (xpDropCount < 3) //minimum 3 attacks to kill mager frem
                    {
                        setFremPrayers();
                        while (!xpDrop)
                        {
                            await Task.Delay(10);
                        }
                        if ((xpDropCount + 1) >= 3)
                        {
                            xpDropCount++;
                            break;
                        }
                        while (xpDrop)
                        {
                            await Task.Delay(10);
                        }
                        xpDropCount++;
                    }
                    await Task.Delay(10);
                    xpDropCount = 0;
                    equipLongRangeWeapon();
                    await Task.Delay(10);
                    //solvewave(2) ranger than mager
                    //solvewave(3) mager than ranger
                    //solvewave(4) single ranger
                    //solvewave(5) single mager
                    //solvewave(6) single mager cannot see ranger
                    //solvewave(7) must lure mager
                }
            }
            if(magePos == 3 && rangePos == 2)
            {
                Console.WriteLine("Solving 7");
                solveWave(7);
            } else if (magePos == 12 && rangePos == 2)
            {
                Console.WriteLine("Solving 2");
                solveWave(2);
            } else if (magePos == 10 && rangePos == 2)
            {
                Console.WriteLine("Solving 7");
                solveWave(7);
            }
            Console.WriteLine("Unknown solve.");
        }
        public bool rangerPos1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(15, 15))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 669, clientCoords[1] + 63, 0, 0, new Size(15, 15));
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

        public bool xpDropCheck()
        {
            if (checkColor(20, 40, 492, 111, 255, 255, 255))
            {
                xpDrop = true;
                return true;
            }
            else if (checkColor(20, 40, 492, 111, 21, 128, 173))
            {
                xpDrop = true;
                return true;
            }
            else
            {
                xpDrop = false;
                return false;
            }
        }

        public int magePos = 0;

        public async void magePosLoop()
        {
            magePos = checkMagerPos();
            rangePos = checkRangerPos();
            await Task.Delay(100);
            magePosLoop();
        }

        public async void solveWave(int mageposition)
        {
            //if mage above me and ranger cannot be seen, step out and kill mager and melee.
            if (mageposition == 0)
            {
                mageposition = magePos;
            }
            switch (mageposition)
            {
                case 1:
                    while (atMiddlePilar())
                    {
                        await Task.Delay(10);
                    }
                    prayer.prayMage();
                    while(!atMiddleNorth())
                    {
                        await Task.Delay(100);
                    }
                    processor.rightClick(558,17);
                    await Task.Delay(400);
                    processor.addMouseClick(552, 73, "inventory");
                    await Task.Delay(400);
                    xpDropCount = 0;
                    while(!fremNorthSide())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(242, 165, "inventory");
                    while (xpDropCount < 2) //minimum 3 attacks to kill mager frem
                    {
                        while (!xpDrop)
                        {
                            await Task.Delay(10);
                        }
                        if ((xpDropCount + 1) >= 2)
                        {
                            xpDropCount++;
                            break;
                        }
                        while (xpDrop)
                        {
                            await Task.Delay(10);
                        }
                        xpDropCount++;
                    }
                    equipLongRangeWeapon();
                    if(magePos == 11)
                    {
                        processor.addMouseClick(231, 167, "gamescreen");
                    } else
                    {
                        prayer.prayMelee();
                    }
                    break;
                case 2:
                    //ranger and mager
                    equipRangeFrem();
                    await Task.Delay(1000);
                    processor.addMouseClick(281, 168, "prayer"); //walk out
                    prayerFlick();
                    while (magerOnMap() && rangerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    Console.WriteLine("One npc died");
                    break;
                case 3:
                    equipLongRangeWeapon();
                    processor.addMouseClick(281, 168, "movement");
                    prayer.solidMagic();
                    while(magePos != 2 && atCornerTile())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(256, 121, "attack");
                    while(magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(229, 169, "movement");
                    await Task.Delay(5000);
                    solveWave(4);
                    break;
                case 4:
                    equipMelee();
                    await Task.Delay(1000);
                    //single ranger
                    prayer.solidRange();
                    Console.WriteLine("Single ranger");
                    processor.addMouseClick(276, 168, "movement");
                    await Task.Delay(5000);
                    while (rangePos != 2 && !atCornerTile())
                    {
                        await Task.Delay(100);
                    }
                    Console.WriteLine("See ranger. Attacking now");
                    await Task.Delay(1000);
                    processor.addMouseClick(243, 111, "attack");
                    await Task.Delay(300);
                    killSingleRanger1();
                    break;
                case 5:
                    Console.WriteLine("Mage pos 8");
                    processor.addMouseClick(320, 208, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(10);
                    }
                    processor.addMouseClick(281, 168, "movement");
                    prayer.activePrayer = 0;
                    while (!atCornerTile())
                    {
                        await Task.Delay(100);
                    }
                    await Task.Delay(600);
                    while (!meleeNorth() && !meleeWest() && !popup)
                    {
                        await Task.Delay(100);
                    }
                    if (!meleeOnMap() || popup)
                    {
                        meleeSkips++;
                    }
                    else
                    {
                        while (!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(254, 116, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(194, 156, "attack");
                            while (meleeWest())
                            {
                                await Task.Delay(100);
                            }
                        }
                        await Task.Delay(2000);
                        while (meleeNorth() || meleeWest())
                        {
                            await Task.Delay(100);
                        }
                        
                        processor.addMouseClick(653, 38, "movement");
                    }
                    prayer.boostPrayer = 0;
                    Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    m5++;
                    break;
                case 7:
                    processor.addMouseClick(232, 157, "movement");
                    if (rangePos == 2)
                    {
                        prayer.solidRange();
                    }
                    while (magePos != 7)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(282, 180, "movement");
                    while (magePos != 4)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(288, 165, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    //Console.Beep();
                    break;
                    break;
            }
            Console.WriteLine("Melee dead, wave done.");
            waveComplete = true;
            finishWave();
            return;
        }

        public int flinchPos = 0;

        public async void prayerFlick()
        {
            prayer.prayMage();
            await Task.Delay(10);
            prayer.solidRange();
            await Task.Delay(10);
            prayer.prayRange();
            await Task.Delay(10);
            processor.addMouseClick(531, 152, "prayer");
            await Task.Delay(10);
            attackMagerRanger();
            await Task.Delay(10);
            processor.addMouseClick(230, 165, "prayer");
            await Task.Delay(10);
            prayer.prayMage();
            prayer.turnOff();
            await Task.Delay(5000);
            if (magePos == 12 && rangePos == 2)
            {
                processor.addMouseClick(281, 168, "prayer"); //walk out
                prayerFlick();
            } else
            {
                if (rangerOnMap())
                {
                    solveWave(2);
                } else if (magerOnMap())
                {

                } else
                {
                    processor.addMouseClick(282, 169, "movement"); //walk out
                }
            }
        }

        public async void attackMagerRanger()
        {
            switch (flinchPos)
            {
                case 0:
                    processor.addMouseClick(235, 99, "prayer"); //attack
                    break;
            }
        }

        public async void move()
        {
            switch (flinchPos)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public async void killSingleRanger1()
        {
            await Task.Delay(1800);
            processor.addMouseClick(270, 156, "prayer");
            while (rangePos != 4 || !rangerOnMap()) //north?
            {
                await Task.Delay(10);
            }
            if(!rangerOnMap())
            {
                processor.addMouseClick(256, 207, "movement");
                //kill melee
                return;
            }
            processor.addMouseClick(234, 149, "attack");
            await Task.Delay(1800);
            processor.addMouseClick(242, 183, "movement");
            while (rangePos != 3 || !rangerOnMap()) //east?
            {
                await Task.Delay(10);
            }
            if (!rangerOnMap())
            {
                processor.addMouseClick(212, 222, "movement");
                //kill melee
                return;
            }
            processor.addMouseClick(240, 130, "attack");
            killSingleRanger1();
        }
        public void summonThrall()
        {
            processor.addMouseClick(639, 319, "attack");
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

        public async void finishWave()
        {
            while (!popup)
            {
                await Task.Delay(600);
                //if (player.health < 70)
                //{
                //    inventory.clickItem2("shark", 99);
                //}
            }
            if (wave3tests)
            {
                //wave3.startScript();
                return;
            }
            await Task.Delay(300);
            processor.addMouseClick(79, 293, "gamescreen");
            await Task.Delay(300);
            processor.addMouseClick(328, 244, "gamescreen");
            while (popup)
            {
                await Task.Delay(100);
            }
            await Task.Delay(300);
            while (!chestHitbox())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(chestPosX + 3, chestPosY + 5, "gamescreen");
            await Task.Delay(100);
            while (!chestGuy())
            {
                await Task.Delay(600);
            }
            await Task.Delay(300);
            processor.addMouseClick(337, 289, "gamescreen");
            claims++;
            fremsCleared = 0;
            await Task.Delay(600);
            processor.addMouseClick(371, 28, "gamescreen");
            await Task.Delay(600);
            processor.addMouseClick(308, 141, "gamescreen");
            await Task.Delay(300);
            while (!leaveInterface())
            {
                await Task.Delay(600);
            }
            processor.PressKey((byte)Keys.NumPad1, 1);
            await Task.Delay(3000);
            while (!colorDoor() && !bankHitbox())
            {
                await Task.Delay(600);
            }
            Console.WriteLine($"bank pos: {bankPosX.ToString()} + {bankPosY.ToString()}");
            await Task.Delay(600);
            if (inventory.hasItem2("ranging potion (1)") || inventory.hasItem2("ranging potion (2)") || inventory.hasItem2("ranging potion (3)") || inventory.hasItem2("ranging potion (4)"))
            {
                Console.WriteLine("Has potion");
                processor.addMouseClick(doorPosX + 15, doorPosY + 25, "gamescreen");
                while (!atMiddleTile())
                {
                    await Task.Delay(100);
                }
                startScript();
            }
            else
            {
                Console.WriteLine("Has no potion");
                processor.addMouseClick(bankPosX + 7, bankPosY, "gamescreen");
                while (!bankOpen())
                {
                    await Task.Delay(100);
                }
                doBanking();
            }
        }

        private bool bankOpen()
        {
            return checkColor(5, 5, 53, 19, 255, 152, 31);
        }

        public async void doBanking()
        {
            processor.addMouseClick(276, 171, "gamescreen");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)");
            inventory.addItem("ranging potion (4)"); //change this later to add amount
            await Task.Delay(300);
            processor.addMouseClick(482, 20, "gamescreen");
            await Task.Delay(300);
            processor.addMouseClick(191, 96, "gamescreen");
            while (!atMiddleTile())
            {
                await Task.Delay(100);
            }
            startScript();
        }

        public async void equipMagicFrem()
        {
            inventory.clickItem2("kodai wand", 4);
            inventory.clickItem2("tome of fire", 6);
            inventory.clickItem2("occult necklace", 2);
            inventory.clickItem2("virtus robetop", 5);
            inventory.clickItem2("imbued zamorak cape", 1);
        }
        public async void equipMagic()
        {
            inventory.clickItem2("kodai wand", 4);
            inventory.clickItem2("tome of fire", 6);
            inventory.clickItem2("occult necklace", 2);
            inventory.clickItem2("virtus robetop", 5);
            inventory.clickItem2("imbued zamorak cape", 1);
        }
        public async void equipRangeFrem()
        {
            inventory.clickItem2("masori assembler", 1);
            inventory.clickItem2("necklace of anguish", 2);
            inventory.clickItem2("armadyl dhide top", 5);
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrows", 3);
        }
        public async void equipRange()
        {
            inventory.clickItem2("masori assembler", 1);
            inventory.clickItem2("necklace of anguish", 2);
            inventory.clickItem2("armadyl dhide top", 5);
            inventory.clickItem2("zaryte crossbow", 4, true);
        }
        public async void equipMeleeFrem()
        {
            inventory.clickItem2("saradomin godsword", 4, true);
            inventory.clickItem2("amulet of fury", 2);
        }
        public async void equipSgs()
        {
            inventory.clickItem2("infernal cape", 1);
            inventory.clickItem2("saradomin godsword", 4, true);
            inventory.clickItem2("amulet of fury", 2);
            inventory.clickItem2("fighters torso", 5);
        }

        public async void equipLongRangeWeapon()
        {
            inventory.clickItem2("zaryte crossbow", 4, true);
        }
        public async void equipDPSRangeWeapon()
        {
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrows", 3);
        }
        public async void equipMelee()
        {
            inventory.clickItem2("amulet of fury", 2);
            inventory.clickItem2("fighters torso", 5);
            inventory.clickItem2("infernal cape", 1);
            inventory.clickItem2("avernic defender", 6);
            inventory.clickItem2("osmumtens fang", 4);
        }
        public bool fremDeadCheck()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 8, clientCoords[1] + 48, 0, 0, new Size(5, 5));
                }

                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (bitmap.GetPixel(x, y).R >= 90 && bitmap.GetPixel(x, y).R <= 110 && bitmap.GetPixel(x, y).G < 30 && bitmap.GetPixel(x, y).B < 30)
                        {
                            fremDead = true;
                            return true;
                        }
                    }
                }
            }
            fremDead = false;
            return false;
        }
        public bool meleeNextToMager()
        {
            return checkColor(5, 5, 631, 61, 255, 0, 0);
        }
        public bool meleeNorth()
        {
            return checkColor(5, 5, 635, 61, 255, 0, 0);
        }
        public bool meleeWest()
        {
            return checkColor(5, 5, 619, 77, 255, 0, 0);
        }
        public bool meleeWest2()
        {
            return checkColor(5, 5, 627, 77, 255, 0, 0);
        }
        public bool magerOnTop()
        {
            if (checkColor(100, 50, 589, 25, 0, 0, 255))
            {
                return true;
            }
            else if (checkColor(99, 24, 589, 25, 0, 0, 255))
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public bool magerOnMap()
        {
            return checkColor(106, 86, 588, 30, 0, 0, 255);
        }
        public bool canSpec()
        {
            return checkColor(3, 3, 575, 146, 53, 155, 181);
        }
        public bool meleeOnMap()
        {
            return checkColor(106, 86, 588, 30, 255, 0, 0);
        }

        public int checkMagerPos()
        {
            if (checkColor(3, 3, 673, 63, 0, 0, 255))
            {
                return 1;
            }
            if (checkColor(3, 3, 677, 63, 0, 0, 255))
            {
                return 3;
            }
            if (checkColor(3, 3, 637, 63, 0, 0, 255))
            {
                return 2;
            }
            if (checkColor(3, 3, 645, 79, 0, 0, 255))
            {
                return 4;
            }
            if (checkColor(13, 13, 665, 94, 0, 0, 255))
            {
                return 5;
            }
            if (checkColor(3, 3, 665, 75, 0, 0, 255))
            {
                return 6;
            }
            if (checkColor(3, 3, 653, 79, 0, 0, 255))
            {
                return 7;
            }
            if (checkColor(3, 3, 653, 91, 0, 0, 255))
            {
                return 8;
            }
            if (checkColor(3, 3, 637, 71, 0, 0, 255))
            {
                return 9;
            }
            if (checkColor(3, 3, 673, 91, 0, 0, 255))
            {
                return 10;
            }
            if (checkColor(3, 3, 629, 79, 0, 0, 255))
            {
                return 11;
            }
            if (checkColor(3, 3, 637, 51, 0, 0, 255))
            {
                return 12;
            }
            return 0;
        }

        public int checkRangerPos()
        {
            if (checkColor(3, 3, 669, 63, 0, 255, 0))
            {
                return 1;
            }
            if (checkColor(3, 3, 633, 59, 0, 255, 0))
            {
                return 2;
            }
            if (checkColor(3, 3, 633, 71, 0, 255, 0))
            {
                return 3; //east
            }
            if (checkColor(3, 3, 629, 75, 0, 255, 0))
            {
                return 4; //north
            }
            if (checkColor(3, 3, 629, 59, 0, 255, 0))
            {
                return 5; //north
            }
            if (checkColor(3, 3, 629, 63, 0, 255, 0))
            {
                return 6; //north
            }
            return 0;
        }
        public bool checkColor(int a, int b, int posX, int posY, int red, int green, int blue)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(a, b))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + posX, clientCoords[1] + posY, 0, 0, new Size(a, b));
                }

                for (int x = 0; x < a; x++)
                {
                    for (int y = 0; y < b; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == red && bitmap.GetPixel(x, y).G == green && bitmap.GetPixel(x, y).B == blue)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool checkDoorPixel(int a, int b, int posX, int posY)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(a, b))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + posX, clientCoords[1] + posY, 0, 0, new Size(a, b));
                }

                for (int x = 0; x < a; x++)
                {
                    for (int y = 0; y < b; y++)
                    {
                        if (bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).R >= 220 && bitmap.GetPixel(x, y).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerrFrem()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 641, clientCoords[1] + 79, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool rangerFrem()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 637, clientCoords[1] + 83, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool chestGuy()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 652, clientCoords[1] + 72, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
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
        public bool leaveInterface()
        {
            return checkColor(10, 10, 359, 360, 128, 0, 0);
        }
        public bool lootInterface()
        {
            return checkColor(5, 5, 200, 22, 254, 152, 31);
        }

        public bool meleeFremCheck()
        {
            meleeFrem = checkColor(5, 5, 633, 79, 255, 255, 0);
            return checkColor(5, 5, 633, 79, 255, 255, 0);
        }

        public bool fremNorthSide()
        {
            return checkColor(5, 5, 633, 79, 255, 255, 0);
        }
        public bool colorDoor()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(401, 208))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 78, clientCoords[1] + 43, 0, 0, new Size(401, 208));
                }

                for (int i = 0; i < 400; i++)
                {
                    for (int j = 0; j < 207; j++)
                    {
                        if (bitmap.GetPixel(i, j).R > 220 && bitmap.GetPixel(i, j).G > 220 && bitmap.GetPixel(i, j).B == 0)
                        {
                            doorPosX = 78 + i;
                            doorPosY = 43 + j;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool chestHitbox()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(95, 160))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 272, clientCoords[1] + 2, 0, 0, new Size(95, 160));
                }

                for (int i = 0; i < 95; i++)
                {
                    for (int j = 0; j < 160; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 0 && bitmap.GetPixel(i, j).G > 200 && bitmap.GetPixel(i, j).B == 0)
                        {
                            chestPosX = i + 272;
                            chestPosY = j + 2;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool bankHitbox()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(75, 75))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 294, clientCoords[1] + 115, 0, 0, new Size(75, 75));
                }

                for (int i = 0; i < 74; i++)
                {
                    for (int j = 0; j < 74; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 10 && bitmap.GetPixel(i, j).G <= 10 && bitmap.GetPixel(i, j).B >= 210)
                        {
                            bankPosX = i + 318;
                            bankPosY = j + 124;
                            Console.WriteLine($"bank pos: {bankPosX.ToString()} + {bankPosY.ToString()}");
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool popupCheck()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 200, clientCoords[1] + 22, 0, 0, new Size(20, 20));
                }

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255)
                        {
                            popup = true;
                            return true;
                        }
                    }
                }
            }
            popup = false;
            return false;
        }
        public bool atMiddleTile()
        {
            return checkDoorPixel(5, 5, 633, 143);
        }
        public bool atMiddlePilar()
        {
            return checkDoorPixel(5, 5, 665, 104);
        }
        public bool atMiddleNorth()
        {
            return checkDoorPixel(5, 5, 675, 114);
        }
        public bool atCornerTile()
        {
            return checkDoorPixel(3, 3, 658, 106);
        }

        public bool atStartTile()
        {
            return checkDoorPixel(5, 5, 639, 102);
        }
    }
}
