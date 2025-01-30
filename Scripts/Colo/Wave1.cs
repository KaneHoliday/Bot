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
                        break;
                    }
                    else if (processor.inventory.inventory[i] == "ranging potion (2)")
                    {
                        inventory.clickItem2("ranging potion (2)", 98);
                        break;
                    }
                    else if (processor.inventory.inventory[i] == "ranging potion (3)")
                    {
                        inventory.clickItem2("ranging potion (3)", 98);
                        break;
                    }
                    else if (processor.inventory.inventory[i] == "ranging potion (4)")
                    {
                        inventory.clickItem2("ranging potion (4)", 98);
                        break;
                    }
                }
                inventory.clickItem2("saturated heart", 99);
                //equipMagicFrem();
                waitForStartLocation();
            }
            else if(atMiddleTile())
            {
                waitForStartLocation();
            }
        }

        public async void waitForStartLocation()
        {
            Console.WriteLine("Waiting for start location");
            while (!atCornerTile())
            {
                Console.WriteLine("No corner tile");
                await Task.Delay(100);
            }
            Console.WriteLine("At corner tile");
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
            await Task.Delay(600);
            processor.addMouseClick(659, 85, "gamescreen"); //accept the wave
            while (popup)
            {
                await Task.Delay(10);
            }
            await Task.Delay(600);
            prayer.solidMagic();
            while(!atStartTile())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(175, 157, "gamescreen"); //move to the safespot
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

            string content = string.Join(",", waveTimes);

            File.WriteAllText(filePath, content);

            Console.WriteLine("Array has been written to the file.");
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
            prayer.prayPiety(); //turn on piety
            await Task.Delay(300);
            processor.addMouseClick(584, 145, "gamescreen"); //special on
            await Task.Delay(300);
            while (xpDrop)
            {
                await Task.Delay(100);
            }
            processor.addMouseClick(256, 178, "gamescreen"); //attack ranger
            while(!xpDrop)
            {
                await Task.Delay(100);
            }
            Console.Write("See xp drop");
            prayer.prayPiety(); //turn off piety
            xpDropCount = 0;
            fremsCleared++;
            player.updateHealth();
            await Task.Delay(100);
            prayer.solidMagic();
            await Task.Delay(500);
            equipRangeFrem(); //has ven bow as weapon
            await Task.Delay(300);
            while (xpDrop)
            {
                await Task.Delay(100);
            }
            if (magePos == 4)
            {
                prayer.prayRigour();
                await Task.Delay(300);
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
            }
            else
            {
                processor.addMouseClick(270, 165, "gamescreen"); //attack mager
                while (xpDropCount < 2) //minimum 3 attacks to kill mager frem
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
                while (!fremDead)
                {
                    await Task.Delay(100);
                }
                xpDropCount = 0;
                Console.WriteLine("Frems dead");
                if (magePos != 2)
                {
                    equipLongRangeWeapon();
                }
                await Task.Delay(500);
            }
            solveWave();
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
            } else
            {
                xpDrop = false;
                return false;
            }
        }

        public int magePos = 0;

        public async void magePosLoop()
        {
            magePos = checkMagerPos();
            await Task.Delay(100);
            magePosLoop();
        }

        public async void solveWave()
        {
            switch(magePos)
            {
                case 1:
                    prayer.prayRigour();
                    processor.addMouseClick(244, 167, "gamescreen");
                    while (magePos == 1)
                    {
                        await Task.Delay(100);
                    }
                    while (magePos != 6)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(349, 151, "gamescreen");
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(268, 168, "gamescreen");
                    await Task.Delay(600);
                    equipMagic();
                    if (prayer.activePrayer == 1)
                    {
                        prayer.turnOff();
                    }
                    await Task.Delay(600);
                    prayer.prayRigour(); // turn rigour off
                    while(!meleeNorth() && !meleeWest() && !popup) {
                        await Task.Delay(100);
                    }
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
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
                    }
                    waveComplete = true;
                    finishWave();
                    return;
                case 2:
                    prayer.prayRigour();
                    await Task.Delay(300);
                    equipDPSRangeWeapon();
                    if (!moveSkip)
                    {
                        processor.addMouseClick(646, 80, "gamescreen"); //move 2 squares right, north?
                        await Task.Delay(100);
                        while (magePos == 2)
                        {
                            await Task.Delay(100);
                        }
                        while (magePos != 2)
                        {
                            await Task.Delay(100);
                        }
                    }
                    moveSkip = false;
                    processor.addMouseClick(256, 121, "gamescreen");
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    equipMagic();
                    await Task.Delay(600);
                    if (prayer.activePrayer == 1)
                    {
                        prayer.turnOff();
                    }
                    await Task.Delay(600);
                    prayer.prayRigour(); //rigour off
                    while (!meleeNorth() && !meleeWest() && !popup)
                    {
                        await Task.Delay(100);
                    }
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
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
                    }
                    waveComplete = true;
                    finishWave();
                    return;
                case 3:
                    prayer.prayRigour();
                    Console.WriteLine("mager pos 3");
                    processor.addMouseClick(232, 157, "gamescreen");
                    while (magePos != 7)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(282, 180, "gamescreen");
                    while (magePos != 4)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(284, 165);
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(646, 80, "gamescreen");
                    await Task.Delay(600);
                    equipMagic();
                    if (prayer.activePrayer == 1)
                    {
                        prayer.turnOff();
                    }
                    await Task.Delay(600);
                    prayer.prayRigour(); //rigour off
                    while (!meleeNorth() && !meleeWest() && !popup)
                    {
                        await Task.Delay(100);
                    }
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        Console.WriteLine("mager dead, melee skipped");
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
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
                    }
                    waveComplete = true;
                    Console.Beep();
                    finishWave();
                    return;
                case 4:
                    processor.addMouseClick(284, 165);
                    while (magePos == 4)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(281, 168, "gamescreen");
                    await Task.Delay(600);
                    equipMagic();
                    if (prayer.activePrayer == 1)
                    {
                        prayer.turnOff();
                    }
                    await Task.Delay(600);
                    prayer.prayRigour();
                    while (!meleeNorth() && !meleeWest() && !popup)
                    {
                        await Task.Delay(100);
                    }
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
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
                    waveComplete = true;
                    finishWave();
                    return;
                default:
                    prayer.prayRigour();
                    processor.addMouseClick(232, 157, "gamescreen");
                    while (magePos != 7)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(282, 180, "gamescreen");
                    while (magePos != 4)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(284, 165);
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    equipMagic();
                    processor.addMouseClick(646, 80, "gamescreen");
                    await Task.Delay(600);
                    prayer.prayRigour();
                    if (prayer.activePrayer == 1)
                    {
                        prayer.turnOff();
                    }
                    await Task.Delay(600);
                    while (!meleeNorth() && !meleeWest() && !popup)
                    {
                        await Task.Delay(100);
                    }
                    if (!meleeOnMap())
                    {
                        meleeSkips++;
                        Console.WriteLine("Melee dead, wave done.");
                        if (prayer.activePrayer == 1)
                        {
                            prayer.turnOff();
                        }
                    }
                    else
                    {
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
            await Task.Delay(300);
            processor.addMouseClick(79, 293, "gamescreen");
            await Task.Delay(300);
            processor.addMouseClick(328, 244, "gamescreen");
            while (popup)
            {
                await Task.Delay(100);
            }
            await Task.Delay(300);
            processor.addMouseClick(313, 36, "gamescreen");
            while (!chestGuy())
            {
                await Task.Delay(600);
            }
            await Task.Delay(300);
            processor.addMouseClick(337, 289);
            claims++;
            fremsCleared = 0;
            await Task.Delay(300);
            processor.addMouseClick(371, 28, "gamescreen");
            await Task.Delay(300);
            processor.addMouseClick(308, 141, "gamescreen");
            await Task.Delay(300);
            while (!leaveInterface())
            {
                await Task.Delay(100);
            }
            processor.PressKey((byte)Keys.NumPad1, 1);
            await Task.Delay(3000);
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

        public async void equipMagicFrem()
        {
            inventory.clickItem2("kodai wand", 4);
            inventory.clickItem2("tome of fire", 6);
            inventory.clickItem2("occult necklace", 2);
            inventory.clickItem2("virtus robetop", 5);
            inventory.clickItem2("armadyl chaps", 7);
            inventory.clickItem2("imbued zamorak cape", 1);
        }
        public async void equipMagic()
        {
            inventory.clickItem2("kodai wand", 4);
            inventory.clickItem2("tome of fire", 6);
            inventory.clickItem2("occult necklace", 2);
            inventory.clickItem2("virtus robetop", 5);
            inventory.clickItem2("armadyl chaps", 7);
            inventory.clickItem2("imbued zamorak cape", 1);
        }
        public async void equipRangeFrem()
        {
            inventory.clickItem2("masori assembler", 1);
            inventory.clickItem2("necklace of anguish", 2);
            inventory.clickItem2("armadyl dhide top", 5);
            inventory.clickItem2("armadyl chaps", 7);
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrows", 3);
        }
        public async void equipRange()
        {
            inventory.clickItem2("masori assembler", 1);
            inventory.clickItem2("necklace of anguish", 2);
            inventory.clickItem2("armadyl dhide top", 5);
            inventory.clickItem2("armadyl chaps", 7);
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrows", 3);
        }
        public async void equipMeleeFrem()
        {
            inventory.clickItem2("infernal cape", 1);
            inventory.clickItem2("saradomin godsword", 4, true);
            inventory.clickItem2("amulet of fury", 2);
            inventory.clickItem2("fighters torso", 5);
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
            inventory.clickItem2("zaryte crossbow", 4);
            inventory.clickItem2("diamond bolts", 3);
        }
        public async void equipDPSRangeWeapon()
        {
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrows", 3);
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
        public bool magerOnMap()
        {
            return checkColor(106, 86, 588, 30, 0, 0, 255);
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

        public bool meleeFremCheck()
        {
            meleeFrem = checkColor(5, 5, 633, 79, 255, 255, 0);
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
        public bool atCornerTile()
        {
            return checkDoorPixel(5, 5, 665, 106);
        }

        public bool atStartTile()
        {
            return checkDoorPixel(5, 5, 639, 102);
        }
    }
}
