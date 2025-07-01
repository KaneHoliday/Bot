                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               using Bot.Core;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Scripts.Colo
{
    internal class Wave1
    {

        //camera zoom = 244/896

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

        public int chestPosX = 0;
        public int chestPosY = 0;

        public int bankPosX = 0;
        public int bankPosY = 0;

        public int[] waveTimes = new int[1000];

        public void loadTimes()
        {
            string filePath = "output.txt";

            string content = File.ReadAllText(filePath);

            string[] numberStrings = content.Split(',');

            waveTimes = numberStrings.Select(int.Parse).ToArray();
        }

        private DateTime startTime;
        private CancellationTokenSource cancellationTokenSource;

        public async Task StartTimerAsync()
        {
            startTime = DateTime.Now;
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            while (!token.IsCancellationRequested)
            {
                await Task.Delay(1000, token);

                var elapsed = DateTime.Now - startTime;
                hours = (int)elapsed.TotalHours;
                minutes = elapsed.Minutes;
                seconds = elapsed.Seconds;
            }
        }

        public void StopTimer()
        {
            cancellationTokenSource?.Cancel();
        }

        public async void dataLoop()
        {
            while(!atMiddlePilar())
            {
                await Task.Delay(100);
            }
            Console.WriteLine("At middle pillar.");
            //captureScreen();
            claims++;
            while(!leaveInterface())
            {
                await Task.Delay(100);
            }
            Console.WriteLine("At leave interface.");
            dataLoop();
        }

        private CancellationTokenSource updateConsoleCts;

        public async Task StartConsoleUpdateAsync()
        {
            updateConsoleCts = new CancellationTokenSource();
            var token = updateConsoleCts.Token;

            while (!token.IsCancellationRequested)
            {
                UpdateConsole();

                // Wait for processor ticks to complete
                await WaitForProcessorTicks();

                await Task.Delay(1200, token);
            }
        }

        private void UpdateConsole()
        {
            // Use string formatting instead of complex if statements
            var timeString = $"{hours:D1}:{minutes:D2}:{seconds:D2}";

            // Build the entire output first, then write it once
            var output = new StringBuilder();
            output.AppendLine($"Time running: {timeString}");
            output.AppendLine($"Waves claimed: {claims}");
            output.AppendLine($"Profit Made: {claims * 40000:N0}"); // Format with thousands separator
            output.AppendLine($"Melee skips: {meleeSkips}");

            // Clear and write in one go
            Console.Clear();
            Console.Write(output.ToString());
        }

        private async Task WaitForProcessorTicks()
        {
            // Combine tick1 and tick2 logic to avoid code duplication
            while (processor.tick1() || processor.tick2())
            {
                await Task.Delay(100);
            }
        }

        public void StopConsoleUpdate()
        {
            updateConsoleCts?.Cancel();
        }

        public async void startScript()
        {
            prayer.boostPrayer = 0;
            prayer.activePrayer = 0;
            Console.WriteLine("Starting script");
            if (firstRun)
            {
                loadTimes();
                CheckLoopAsync();
                magePosLoop();
                StartTimerAsync();
                UpdateConsole();
                firstRun = false;
            }
            if (atMiddleTile())
            {
                await Task.Delay(600);
                processor.addMouseClick(220, 164, "movement");
                while (!popup)
                {
                    await Task.Delay(100);
                }
                await Task.Delay(200);
                processor.addMouseClick(339, 281, "prayer"); //click on the invocation
                await Task.Delay(200);
                processor.addMouseClick(435, 294, "prayer"); //accept the wave
                await Task.Delay(200);
                processor.addMouseClick(599, 113, "prayer");
                await Task.Delay(200);
                //inventory.clickItem2("saturated heart", 99);
                //equipMagicFrem();
                waitForStartLocation();
            }
            else if (atMiddleTile())
            {
                waitForStartLocation();
            }
        }

        public async void waitForStartLocation()
        {
            while(atMiddleTile())
            {
                await Task.Delay(100);
            }
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
            prayer.solidMagic();
            if (waveTicks > 100)
            {
                saveTimes();
                waveTimes[claims - 1] = waveTicks;
                waveTicks = 0;
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
        public bool mageFrem = false;
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

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public async Task CheckLoopAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                meleeFremCheck();
                mageFremCheck();
                xpDropCheck();
                fremDeadCheck();
                popupCheck();
                await Task.Delay(20, _cancellationTokenSource.Token);
            }
        }

        public bool moveSkip = false;

        public async void killFrems()
        {
            xpDropCount = 0;
            while (!meleeFrem)
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(238, 167, "attack");
            while (xpDropCount < 2)
            {
                setFremPrayers();
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
            while (xpDrop)
            {
                await Task.Delay(10);
            }
            xpDropCount = 0;
            fremsCleared++;
            equipMeleeFrem();
            prayer.prayPiety(); //turn on piety
            while(!canSpec())
            {
                await Task.Delay(10);
            }
            processor.addMouseClick(584, 145, "gamescreen"); //special on
            await Task.Delay(10);
            processor.addMouseClick(257, 186, "attack"); //attack ranger
            while (!xpDrop)
            {
                await Task.Delay(10);
            }
            //summonThrall(); only enable if doing wave 2 too.
            xpDropCount = 0;
            fremsCleared++;
            while (xpDrop)
            {
                await Task.Delay(10);
            }
            if (magePos == 1 || magePos == 6)
            {
                await Task.Delay(10);
                processor.addMouseClick(225, 151, "movement");
                await Task.Delay(10);
                equipRangeFrem(); //has ven bow as weapon
                while (magePos != 7)
                {
                    await Task.Delay(10);
                }
                processor.addMouseClick(293, 184, "movement");
                while (magePos != 4)
                {
                    await Task.Delay(10);
                }
                await Task.Delay(300);
                processor.addMouseClick(298, 167, "attack");
                await Task.Delay(10);
                prayer.prayRigour();
                while (xpDropCount < 3 || mageFrem) //minimum 3 attacks to kill mager frem
                {
                    setFremPrayers();
                    int check = 0;
                    while (!xpDrop || mageFrem)
                    {
                        await Task.Delay(100);
                        check++;
                        if (check > 20)
                        {
                            break;
                        }
                    }
                    if ((xpDropCount + 1) >= 3)
                    {
                        xpDropCount++;
                        break;
                    }
                    while (xpDrop || mageFrem)
                    {
                        await Task.Delay(10);
                    }
                    xpDropCount++;
                }
                equipLongRangeWeapon();
                await Task.Delay(10);
                xpDropCount = 0;
                solveWave(4);
            }
            else if (magePos == 2)
            {
                processor.addMouseClick(290, 140, "movement");
                await Task.Delay(10);
                equipRangeFrem(); //has ven bow as weapon
                while (magePos != 9)
                {
                    await Task.Delay(10);
                }
                await Task.Delay(100);
                processor.addMouseClick(256, 121, "attack");
                await Task.Delay(10);
                prayer.prayRigour();
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
                        await Task.Delay(10);
                    }
                    xpDropCount++;
                }
                equipLongRangeWeapon();
                await Task.Delay(10);
                xpDropCount = 0;
                solveWave(9);
            }
            else if (magePos == 3)
            {
                processor.addMouseClick(225, 151, "movement");
                await Task.Delay(10);
                equipRangeFrem(); //has ven bow as weapon
                while (magePos != 7)
                {
                    await Task.Delay(10);
                }
                processor.addMouseClick(293, 184, "movement");
                while (magePos != 4)
                {
                    await Task.Delay(10);
                }
                processor.addMouseClick(298, 167, "attack");
                await Task.Delay(10);
                prayer.prayRigour();
                while (xpDropCount < 3 || mageFrem) //minimum 3 attacks to kill mager frem
                {
                    setFremPrayers();
                    while (!xpDrop || mageFrem)
                    {
                        await Task.Delay(100);
                    }
                    if ((xpDropCount + 1) >= 3)
                    {
                        xpDropCount++;
                        break;
                    }
                    while (xpDrop || mageFrem)
                    {
                        await Task.Delay(10);
                    }
                    xpDropCount++;
                }
                xpDropCount = 0;
                equipLongRangeWeapon();
                await Task.Delay(10);
                solveWave(4);
            }
            else if (magePos == 4)
            {
                equipRangeFrem(); //has ven bow as weapon
                await Task.Delay(10);
                processor.addMouseClick(298, 167, "attack");
                await Task.Delay(10);
                prayer.prayRigour();
                while (xpDropCount < 3 || mageFrem) //minimum 3 attacks to kill mager frem
                {
                    setFremPrayers();
                    while (!xpDrop || mageFrem)
                    {
                        await Task.Delay(100);
                    }
                    if ((xpDropCount + 1) >= 3)
                    {
                        xpDropCount++;
                        break;
                    }
                    while (xpDrop || mageFrem)
                    {
                        await Task.Delay(10);
                    }
                    xpDropCount++;
                }
                xpDropCount = 0;
                equipLongRangeWeapon();
                await Task.Delay(10);
                solveWave(4);
            }
            else if (magePos == 10)
            {
                processor.addMouseClick(225, 151, "movement");
                await Task.Delay(10);
                equipRangeFrem(); //has ven bow as weapon
                while (magePos != 7)
                {
                    await Task.Delay(10);
                }
                processor.addMouseClick(293, 184, "movement");
                while (magePos != 4)
                {
                    await Task.Delay(10);
                }
                processor.addMouseClick(298, 167, "attack");
                prayer.prayRigour();
                while (xpDropCount < 3 || mageFrem) //minimum 3 attacks to kill mager frem
                {
                    setFremPrayers();
                    while (!xpDrop || mageFrem)
                    {
                        await Task.Delay(10);
                    }
                    if ((xpDropCount + 1) >= 3)
                    {
                        xpDropCount++;
                        break;
                    }
                    while (xpDrop || mageFrem)
                    {
                        await Task.Delay(10);
                    }
                    xpDropCount++;
                }
                xpDropCount = 0;
                equipLongRangeWeapon();
                await Task.Delay(10);
                solveWave(4);
            }
            else if (magePos == 11)
            {
                equipRangeFrem();
                processor.addMouseClick(276, 169, "attack");
                prayer.prayRigour();
                while (xpDropCount < 2) //minimum 3 attacks to kill mager frem
                {
                    setFremPrayers();
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
                await Task.Delay(10);
                xpDropCount = 0;
                equipLongRangeWeapon();
                await Task.Delay(10);
                solveWave(6);
            }
            else if (magePos == 8)
            {
                equipRangeFrem();
                processor.addMouseClick(276, 169, "attack");
                prayer.prayRigour();
                while (xpDropCount < 2) //minimum 2 attacks to kill mager frem
                {
                    setFremPrayers();
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
                await Task.Delay(10);
                xpDropCount = 0;
                equipLongRangeWeapon();
                await Task.Delay(10);
                solveWave(8);
            }
            else
            {
                equipRangeFrem();
                processor.addMouseClick(350, 153, "attack");  //fix this
                prayer.prayRigour();
                while (xpDropCount < 2) //minimum 2 attacks to kill mager frem
                {
                    setFremPrayers();
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
                await Task.Delay(10);
                xpDropCount = 0;
                equipLongRangeWeapon();
                await Task.Delay(10);
                solveWave(5);
            }
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
            await Task.Delay(100);
            magePosLoop();
        }

        public async void solveWave(int mageposition)
        {
            if(mageposition == 0)
            {
                mageposition = magePos;
            }
            switch (mageposition)
            {
                case 1:
                    Console.WriteLine("Mage pos 1");
                    processor.addMouseClick(244, 167, "movement");
                    while (magePos == 1)
                    {
                        await Task.Delay(10);
                    }
                    while (magePos != 6)
                    {
                        await Task.Delay(10);
                    }
                    processor.addMouseClick(349, 151, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(10);
                    }
                    processor.addMouseClick(268, 168, "movement");
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
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    m1++;
                    break;
                case 2:
                    Console.WriteLine("Mage pos 2");
                    processor.addMouseClick(257, 128, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    prayer.activePrayer = 0;
                    while (!atCornerTile())
                    {
                        await Task.Delay(100);
                    }
                    await Task.Delay(600);
                    //Console.WriteLine("At corner tile");
                    while (!meleeNorth() && !meleeWest() && !popup)
                    {
                        await Task.Delay(100);
                    }
                    //Console.WriteLine("See melee north, west, or popup.");
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
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    m2++;
                    break;
                case 3:
                    Console.WriteLine("Mage pos 3");
                    //Console.WriteLine("mager pos 3");
                    processor.addMouseClick(232, 157, "movement");
                    while (magePos != 7)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(282, 180, "movement");
                    while (magePos != 4)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(290, 165, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(646, 80, "movement");
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
                        //Console.WriteLine("mager dead, melee skipped");
                    }
                    else
                    {
                        while (!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                        prayer.boostPrayer = 0;
                    }
                    Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    //Console.Beep();
                    break;
                    m3++;
                case 4:
                    Console.WriteLine("Mage pos 4");
                    processor.addMouseClick(298, 167, "attack");
                    while (magePos == 4)
                    {
                        await Task.Delay(100);
                    }
                    await Task.Delay(300);
                    processor.addMouseClick(298, 167, "movement");
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
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    Console.WriteLine("Melee dead, wave done.");
                    //Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    m4++;
                    break;
                case 5:
                    Console.WriteLine("Mage pos 8");
                    processor.addMouseClick(320, 208, "attack");
                    while(magerOnMap())
                    {
                        await Task.Delay(10);
                    }
                    await Task.Delay(300);
                    processor.addMouseClick(298, 167, "movement");
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
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    m5++;
                    break;
                case 6:
                    Console.WriteLine("Mage pos 6");
                    processor.addMouseClick(316, 156, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(10);
                    }
                    prayer.activePrayer = 0;
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
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    m5++;
                    break;
                case 8:
                    Console.WriteLine("Mage pos 8");
                    processor.addMouseClick(339, 218, "attack");
                    while (magePos == 8)
                    {
                        await Task.Delay(100);
                    }
                    await Task.Delay(300);
                    processor.addMouseClick(298, 167, "movement");
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
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    Console.WriteLine("Melee dead, wave done.");
                    waveComplete = true;
                    m8++;
                    break;
                case 9:
                    Console.WriteLine("Mage pos 9");
                    await Task.Delay(100);
                    processor.addMouseClick(256, 121, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(256, 203, "movement");
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
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    break;
                case 10:
                    processor.addMouseClick(290, 165, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(646, 80, "movement");
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
                        //Console.WriteLine("Melee dead, wave done.");
                    }
                    else
                    {
                        while (!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    m10++;
                    break;
                default:
                    await Task.Delay(400);
                    processor.addMouseClick(232, 157, "movement");
                    while (magePos != 7)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(282, 180, "movement");
                    while (magePos != 4)
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(284, 165, "attack");
                    while (magerOnMap())
                    {
                        await Task.Delay(100);
                    }
                    processor.addMouseClick(646, 80, "movement");
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
                        //Console.WriteLine("Melee dead, wave done.");
                    }
                    else
                    {
                        while (!(meleeNorth() || meleeWest()))
                        {
                            await Task.Delay(100);
                        }
                        if (meleeNorth())
                        {
                            processor.addMouseClick(248, 92, "attack");
                            while (meleeNorth())
                            {
                                await Task.Delay(100);
                            }
                        }
                        else if (meleeWest())
                        {
                            processor.addMouseClick(178, 157, "attack");
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
                    m11++;
                    break;
            }
            //Console.WriteLine("Melee dead, wave done.");
            waveComplete = true;
            finishWave();
            return;
        }

        public void summonThrall()
        {
            processor.addMouseClick(639, 319, "attack");
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
                //wave2.startScript();
                return;
            }
            await Task.Delay(300);
            processor.addMouseClick(79, 293, "gamescreen");
            await Task.Delay(300);
            processor.addMouseClick(328, 244, "gamescreen");
            await Task.Delay(300);
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
            processor.addMouseClick(326, 134, "gamescreen");
            await Task.Delay(300);
            while (!leaveInterface())
            {
                await Task.Delay(600);
            }
            processor.PressKey((byte)Keys.NumPad1, 1);
            await Task.Delay(600);
            processor.addMouseClick(531, 148, "prayer");
            await Task.Delay(3000);
            while (!colorDoor())
            {
                await Task.Delay(600);
            }
            Console.WriteLine($"bank pos: {bankPosX.ToString()} + {bankPosY.ToString()}");
            await Task.Delay(600);
            //if (inventory.hasItem2("ranging potion (1)") || inventory.hasItem2("ranging potion (2)") || inventory.hasItem2("ranging potion (3)") || inventory.hasItem2("ranging potion (4)"))
            //{
                Console.WriteLine("Has potion");
                processor.addMouseClick(doorPosX + 15, doorPosY, "gamescreen");
                while (!atMiddleTile())
                {
                    await Task.Delay(100);
                }
                startScript();
            //} else
            //{
            //    Console.WriteLine("Has no potion");
            //    processor.addMouseClick(bankPosX + 7, bankPosY, "gamescreen");
            //    while(!bankOpen())
            //    {
            //        await Task.Delay(100);
            //    }
            //    doBanking();
            //}
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
            while(!atMiddleTile())
            {
                await Task.Delay(100);
            }
            startScript();
        }

        public async void equipMagicFrem()
        {
            inventory.clickItem2("kodai wand", 4);
            inventory.clickItem2("tome of fire", 6);
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
            inventory.clickItem2("venator bow", 4, true);
            inventory.clickItem2("amethyst arrows", 3);
        }
        public async void equipMeleeFrem()
        {
            inventory.clickItem2("saradomin godsword", 4, true);
            inventory.clickItem2("infernal cape", 1);
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
            if (checkColor(3, 3, 673, 91, 0, 0, 255))
            {
                return 11;
            }
            return 0;
        }
        public bool checkColor(int width, int height, int posX, int posY, int red, int green, int blue)
        {
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + posX, clientCoords[1] + posY, 0, 0, new Size(width, height));
                }

                // Use unsafe code for much faster pixel access
                unsafe
                {
                    BitmapData bitmapData = bitmap.LockBits(
                        new Rectangle(0, 0, width, height),
                        ImageLockMode.ReadOnly,
                        PixelFormat.Format24bppRgb);

                    try
                    {
                        byte* scan0 = (byte*)bitmapData.Scan0.ToPointer();
                        int stride = bitmapData.Stride;

                        for (int y = 0; y < height; y++)
                        {
                            byte* row = scan0 + (y * stride);
                            for (int x = 0; x < width; x++)
                            {
                                int bIndex = x * 3;
                                byte b = row[bIndex];
                                byte g = row[bIndex + 1];
                                byte r = row[bIndex + 2];

                                if (r == red && g == green && b == blue)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                }
            }
            return false;
        }

        public void StopCheckLoop()
        {
            _cancellationTokenSource.Cancel();
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
            // Removed duplicate call - was calling checkColor twice
            meleeFrem = checkColor(5, 5, 633, 79, 255, 255, 0);
            return meleeFrem;
        }
        public bool mageFremCheck()
        {
            // Removed duplicate call - was calling checkColor twice
            mageFrem = checkColor(5, 5, 640, 79, 255, 255, 0);
            return mageFrem;
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
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                