using Bot.Core;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts.Colo
{
    internal class Wave1
    {
        public Wave2 wave2;

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
        public bool wave2tests = false;

        public int waveTicks = 0;
        public int meleeSkips = 0;

        public int[] waveTimes = new int[1000];

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
                if (minutes < 10)
                {
                    Console.WriteLine("Time running: " + hours.ToString() + ":0" + minutes.ToString() + ":0" + seconds.ToString());
                } else
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
            if(processor.tick1())
            {
                while(processor.tick1())
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
            updateConsole();
        }

        public async void startScript()
        {
            if (firstRun)
            {
                checkLoop();
                magePosLoop();
                startTime();
                updateConsole();
                firstRun = false;
            }
            if(atMiddleTile())
            {
                await Task.Delay(200);
                processor.addMouseClick(116, 312, "movement");
                await Task.Delay(200);
                for (int i = 0; i < processor.inventory.inventory.Length; i++)
                {
                    if (processor.inventory.inventory[i] == "ranging potion (1)")
                    {
                        inventory.clickItem2("ranging potion (1)", 98);
                    }
                    else if (processor.inventory.inventory[i] == "ranging potion (2)")
                    {
                        inventory.clickItem2("ranging potion (2)", 98);
                    }
                    else if (processor.inventory.inventory[i] == "ranging potion (3)")
                    {
                        inventory.clickItem2("ranging potion (3)", 98);
                    }
                    else if (processor.inventory.inventory[i] == "ranging potion (4)")
                    {
                        inventory.clickItem2("ranging potion (4)", 98);
                    }
                }
                inventory.clickItem2("saturated heart", 99);
                equipMagicFrem();
                waitForStartLocation();
            }
            else if(atStartTile())
            {
                waitForStartLocation();
            }
        }

        public async void check()
        {
            processor.addMouseClick(530, 350, "gamescreen");
            await Task.Delay(1000);
            equipMagic();
            await Task.Delay(3000);
            equipMelee();
            await Task.Delay(3000);
            equipRange();
        }

        public async void waitForStartLocation()
        {
            while (!atCornerTile())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            Console.WriteLine("At start position");
            processor.addMouseClick(322, 76, "gamescreen");
            await Task.Delay(600);
            while (!popup)
            {
                await Task.Delay(100);
            }
            Console.WriteLine("Popup past");
            await Task.Delay(200);
            processor.addMouseClick(339, 281, "gamescreen"); //click on the invocation
            await Task.Delay(200);
            processor.addMouseClick(435, 294, "gamescreen"); //accept the wave
            while(popup)
            {
                await Task.Delay(300);
                processor.addMouseClick(315, 181, "gamescreen"); //accept the wave
            }
            await Task.Delay(100);
            prayer.solidMagic();
            while(!atStartTile())
            {
                await Task.Delay(100);
            }
            processor.addMouseClick(172, 157, "gamescreen"); //move to the safespot
            if (waveTicks > 100)
            {
                saveTimes();
                waveTimes[claims - 1] = waveTicks;
                waveTicks = 0;
            }
            await Task.Delay(1200);
            //rightClickMeleeFrem();
            killFrems();
        }

        public void saveTimes()
        {
            string filePath = "output.txt";

            // Join array elements with a comma
            string content = string.Join(",", waveTimes);

            // Write the content to the file
            File.WriteAllText(filePath, content);

            Console.WriteLine("Array has been written to the file.");
        }

        public async void setFremPrayers()
        {
                if (fremsCleared < 1)
                {
                    prayer.solidMelee();
                    //processor.prayerArray[x] = 3;
                }
                if (fremsCleared < 2)
                {
                    prayer.solidRange();
                    //processor.prayerArray[y] = 1;
                }
                if (fremsCleared < 3)
                {
                    prayer.solidMagic();
                    //processor.prayerArray[z] = 2;
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
            while (!meleeFrem)
            {
                await Task.Delay(20);
            }
            processor.addMouseClick(240, 166, "gamescreen");
            bool pass = false;
            while (xpDropCount < 2)
            {
                setFremPrayers();
                while (!xpDrop)
                {
                    await Task.Delay(100);
                }
                if ((xpDropCount + 1) >= 2)
                {
                    xpDropCount++;
                    break;
                }
                while (xpDrop)
                {
                    await Task.Delay(100);
                }
                xpDropCount++;
            }
            xpDropCount = 0;
            fremsCleared++;
            equipMeleeFrem();
            await Task.Delay(100);
            while (xpDrop)
            {
                await Task.Delay(100);
            }
            processor.addMouseClick(256, 178, "gamescreen"); //attack ranger
            while(!xpDrop)
            {
                await Task.Delay(100);
            }
            inventory.clickItem2("saradomin godsword", 4, true); //2 handed weapon
            await Task.Delay(600);
            processor.addMouseClick(584, 145, "gamescreen"); //special on
            while (xpDrop)
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(256, 178, "gamescreen"); //attack ranger
            while (!xpDrop)
            {
                await Task.Delay(100);
            }
            xpDropCount = 0;
            fremsCleared++;
            player.updateHealth();
            await Task.Delay(100);
            prayer.solidMagic();
            await Task.Delay(500);
            equipRangeFrem(); //has ven bow as weapon
            await Task.Delay(100);
            while (xpDrop)
            {
                await Task.Delay(100);
            }
            if (magerPos4())
            {
                processor.addMouseClick(284, 165);
                while (xpDropCount < 4) //minimum 4 attacks to kill mager frem
                {
                    setFremPrayers();
                    while (!xpDrop)
                    {
                        await Task.Delay(100);
                    }
                    if ((xpDropCount + 1) >= 4)
                    {
                        xpDropCount++;
                        break;
                    }
                    while (xpDrop)
                    {
                        await Task.Delay(100);
                    }
                    xpDropCount++;
                }
                equipLongRangeWeapon();
                await Task.Delay(600);
            } else
            //} else if (magerPos2())
            //{
            //    processor.addMouseClick(283, 168);
            //    while(magerPos2())
            //    {
            //        await Task.Delay(100);
            //    }
            //    while(!magerPos2())
            //    {
            //        await Task.Delay(100);
            //    }
            //    moveSkip = true;
            //    processor.addMouseClick(256, 121, "gamescreen");
            //    while (xpDropCount < 4) //minimum 4 attacks to kill mager frem
            //    {
            //        while (!xpDrop)
            //        {
            //            await Task.Delay(100);
            //        }
            //        if ((xpDropCount + 1) >= 4)
            //        {
            //            xpDropCount++;
            //            break;
            //        }
            //        while (xpDrop)
            //        {
            //            await Task.Delay(100);
            //        }
            //        xpDropCount++;
            //    }
            //    equipLongRangeWeapon();
            //    await Task.Delay(600);
            //} else
            {
                processor.addMouseClick(270, 165, "gamescreen"); //attack mager
                while (xpDropCount < 3) //minimum 3 attacks to kill mager frem
                {
                    setFremPrayers();
                    while (!xpDrop)
                    {
                        await Task.Delay(100);
                    }
                    if ((xpDropCount + 1) >= 3)
                    {
                        xpDropCount++;
                        break;
                    }
                    while (xpDrop)
                    {
                        await Task.Delay(100);
                    }
                    xpDropCount++;
                }
                while (!fremDead)
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
        public bool xpDropCheck()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 40))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 492, clientCoords[1] + 111, 0, 0, new Size(20, 40));
                }
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 40; y++)
                    {
                        if (bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
                        {
                            xpDrop = true;
                            return true;
                        }
                    }
                }
            }
            xpDrop = false;
            return false;
        }

        public int magePos = 0;

        public async void magePosLoop()
        {
            magerPos1();
            magerPos2();
            magerPos3();
            magerPos4();
            magerPos5();
            magerPos6();
            magerPos7();
            await Task.Delay(100);
            magePosLoop();
        }

        public async void solveWave()
        {
            switch(magePos)
            {
                case 1:
                    processor.addMouseClick(244, 167, "gamescreen");
                    while (magerPos1())
                    {
                        await Task.Delay(100);
                    }
                    while (!magerPos6())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(349, 151, "gamescreen");
                    while (magerOnMap())
                    {
                        await Task.Delay(600);
                    }
                    await Task.Delay(1800);
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        processor.addMouseClick(335, 181, "gamescreen");
                        await Task.Delay(600);
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
                        processor.addMouseClick(268, 168, "gamescreen");
                        await Task.Delay(600);
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                        await Task.Delay(600);
                        while(!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(254, 116, "gamescreen");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(194, 156, "gamescreen");
                            while (meleeWest())
                            {
                                await Task.Delay(100);
                            }
                        }
                        while (meleeNorth() || meleeWest())
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine("Melee dead, wave done.");
                        processor.addMouseClick(325, 182);
                    }
                    waveComplete = true;
                    finishWave();
                    return;
                case 2:
                    if (!moveSkip)
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
                    }
                    moveSkip = false;
                    processor.addMouseClick(256, 121, "gamescreen");
                    while (magerOnMap())
                    {
                        await Task.Delay(600);
                        if (meleeNextToMager())
                        {
                            equipDPSRangeWeapon();
                            await Task.Delay(100);
                            processor.addMouseClick(256, 121, "gamescreen");
                            while (magerOnMap())
                            {
                                await Task.Delay(600);
                            }
                        }
                    }
                    await Task.Delay(1800);
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        processor.addMouseClick(324, 181, "gamescreen");
                        await Task.Delay(600);
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                        await Task.Delay(600);
                        while (!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(250, 109, "gamescreen");
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(204, 167, "gamescreen");
                        }
                        while (meleeNorth() || meleeWest())
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine("Melee dead, wave done.");
                        processor.addMouseClick(325, 182);
                    }
                    waveComplete = true;
                    finishWave();
                    return;
                case 3:
                    Console.WriteLine("mager pos 3");
                    processor.addMouseClick(232, 157, "gamescreen");
                    while (!magerPos7())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(282, 180, "gamescreen");
                    while (!magerPos4())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(284, 165);
                    while (magerOnMap())
                    {
                        await Task.Delay(600);
                    }
                    await Task.Delay(1800);
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        processor.addMouseClick(350, 182, "gamescreen");
                        Console.WriteLine("mager dead, melee skipped");
                        await Task.Delay(600);
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
                        processor.addMouseClick(646, 80, "gamescreen");
                        await Task.Delay(600);
                        Console.WriteLine("mager dead");
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                        await Task.Delay(600);
                        while (!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(250, 109, "gamescreen");
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(204, 167, "gamescreen");
                        }
                        await Task.Delay(600);
                        while (meleeNorth() || meleeWest())
                        {
                            await Task.Delay(100);
                        }
                        Console.WriteLine("Melee dead, wave done.");
                        processor.addMouseClick(325, 182);
                    }
                    waveComplete = true;
                    Console.Beep();
                    finishWave();
                    return;
                case 4:
                    processor.addMouseClick(284, 165);
                    while (magerPos4())
                    {
                        await Task.Delay(100);
                    }
                    await Task.Delay(1800);
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        await Task.Delay(600);
                        processor.addMouseClick(350, 182, "gamescreen");
                        await Task.Delay(600);
                        equipMagic();
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
                        processor.addMouseClick(281, 168, "gamescreen");
                        await Task.Delay(600);
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                        await Task.Delay(600);
                        while (!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(250, 109, "gamescreen");
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(204, 167, "gamescreen");
                        }
                        while (meleeNorth() || meleeWest())
                        {
                            await Task.Delay(100);
                        }
                    }
                    Console.WriteLine("Melee dead, wave done.");
                    processor.addMouseClick(325, 182);
                    waveComplete = true;
                    finishWave();
                    return;
                default:
                    processor.addMouseClick(232, 157, "gamescreen");
                    while (!magerPos7())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(282, 180, "gamescreen");
                    while (!magerPos4())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(284, 165);
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        Console.WriteLine("Melee dead, wave done.");
                        processor.addMouseClick(325, 182);
                        await Task.Delay(600);
                        Console.WriteLine("Skipped the melee!"); //log this?
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
                        processor.addMouseClick(646, 80, "gamescreen");
                        await Task.Delay(600);
                        Console.WriteLine("mager dead");
                        equipMagic();
                        await Task.Delay(600);
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                        await Task.Delay(600);
                        while (!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(250, 109, "gamescreen");
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(204, 167, "gamescreen");
                        }
                        while (meleeNorth() || meleeWest())
                        {
                            await Task.Delay(100);
                        }
                    }
                    Console.WriteLine("Melee dead, wave done.");
                    processor.addMouseClick(325, 182);
                    waveComplete = true;
                    finishWave();
                    return;
            }
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
            if (wave2tests)
            {
                wave2.startScript();
                return;
            }
            await Task.Delay(100);
            processor.addMouseClick(79, 293);
            await Task.Delay(100);
            processor.addMouseClick(328, 244);
            while (popup)
            {
                await Task.Delay(100);
            }
            await Task.Delay(100);
            processor.addMouseClick(267, 33);
            while (!chestGuy())
            {
                await Task.Delay(600);
            }
            await Task.Delay(100);
            processor.addMouseClick(337, 289);
            claims++;
            fremsCleared = 0;
            await Task.Delay(100);
            processor.addMouseClick(371, 28, "gamescreen");
            await Task.Delay(100);
            processor.addMouseClick(308, 141, "gamescreen");
            await Task.Delay(100);
            while (!leaveInterface())
            {
                await Task.Delay(100);
            }
            processor.PressKey((byte)Keys.NumPad1, 1);
            while (!colorDoor())
            {
                await Task.Delay(600);
            }
            await Task.Delay(600);
            processor.addMouseClick(doorPosX + 15, doorPosY + 25, "gamescreen"); //YELLOW CLICK?
            while (!atMiddleTile())
            {
                await Task.Delay(100);
            }
            startScript();
        }
      

        public async void test11()
        {
            inventory.clickItem2("eclipse atlatl", 4, true);
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
            inventory.clickItem2("amulet of anguish", 2);
            inventory.clickItem2("eclipse moon chestplate", 5);
            inventory.clickItem2("eclipse moon tassets", 7);
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrow", 3);
            inventory.clickItem2("regen bracelet", 8);
        }
        public async void equipRange()
        {
            inventory.clickItem2("amulet of anguish", 2);
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
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 631, clientCoords[1] + 61, 0, 0, new Size(5, 5));
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
        public bool meleeNorth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 635, clientCoords[1] + 61, 0, 0, new Size(5, 5));
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
        public bool meleeWest()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 619, clientCoords[1] + 77, 0, 0, new Size(5, 5));
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
        public bool meleeWest2()
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
        public bool meleeOnMap()
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
                            magePos = 1;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPos6()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 665, clientCoords[1] + 75, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            magePos = 6;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool magerPos7()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 653, clientCoords[1] + 79, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            magePos = 7;
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
            using (Bitmap bitmap = new Bitmap(13, 13))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 665, clientCoords[1] + 94, 0, 0, new Size(13, 13));
                }

                for (int x = 0; x < 12; x++)
                {
                    for (int y = 0; y < 12; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            magePos = 5;
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
                            magePos = 4;
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
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 677, clientCoords[1] + 63, 0, 0, new Size(3, 3));
                }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 255)
                        {
                            magePos = 3;
                            return true;
                        }
                    }
                }
            }
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
                            magePos = 2;
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
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 359, clientCoords[1] + 360, 0, 0, new Size(10, 10));
                }

                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 128 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool meleeFremCheck()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 633, clientCoords[1] + 79, 0, 0, new Size(5, 5));
                }

                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            meleeFrem = true;
                            return true;
                        }
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            meleeFrem = false;
            return false;
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
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 633, clientCoords[1] + 143, 0, 0, new Size(5, 5));
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
        public bool atCornerTile()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 665, clientCoords[1] + 106, 0, 0, new Size(5, 5));
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
        public bool atFremTile()
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

        public bool atStartTile()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 639, clientCoords[1] + 102, 0, 0, new Size(5, 5));
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

        public bool green()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 257, clientCoords[1] + 6, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            Console.WriteLine($"Found green at x:{x.ToString()}, y:{y.ToString()}");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
