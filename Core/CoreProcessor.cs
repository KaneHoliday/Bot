using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.Permissions;
using Bot.Scripts;
using System.Numerics;
using Bot.Scripts.Colo;
using System.Diagnostics;
using static System.Windows.Forms.Design.AxImporter;
using System.Xml.Schema;
using System.Security.Claims;

namespace Bot.Core
{
    internal class CoreProcessor
    {
        public int totalTicks = 0;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        const int KEY_DOWN_EVENT = 0x0001; //Key down flag
        const int KEY_UP_EVENT = 0x0002; //Key up flag

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        public int[] clientCoords = new int[2];
        public bool clientFound;

        public Player player = new Player();
        public Inventory inventory = new Inventory();
        public Interfaces interfaces = new Interfaces();
        public XpDrops xpDrops = new XpDrops();
        public Equipment equipment = new Equipment();
        public Prayer prayer = new Prayer();

        public GOTR gotr = new GOTR();
        public Baba baba = new Baba();
        public NatureRunes natures = new NatureRunes();
        public CookingGuild anglers = new CookingGuild();
        public Fletching fletch = new Fletching();
        public CorpTank corpTank = new CorpTank();
        public Wave1 colo = new Wave1();
        public Hunter hunter = new Hunter();
        public Wave2 wave2 = new Wave2();
        public FaladorRooftop fally = new FaladorRooftop();
        public NexDuo nex = new NexDuo();

        public int[] prayerClicks = new int[1000000];
        public int[] movementClicks = new int[1000000];
        public int[] inventoryClicks = new int[1000000];
        public int[] spellbookClicks = new int[1000000];
        public int[] gamescreenClicks = new int[1000000];
        private int[] attackClicks = new int[1000000];

        public int[] tempPClicks = new int[1000000];
        public int[] tempMClicks = new int[1000000];
        public int[] tempIClicks = new int[1000000];
        public int[] tempSClicks = new int[1000000];
        public int[] tempGClicks = new int[1000000];

        public int[] prayerArray = new int[10];
        public int prayerActive = 0; //0 nothing, 1 mage, 2 range, 3 melee

        public int tabOpen = 1;

        public int red;
        public int green;
        public int blue;
        public Image screen;

        public bool busy = false;
        public bool skip = false;

        public int tick = 0;

        //prayer array 
        public void captureScreen()
        {
            using (Bitmap bitmap = new Bitmap(510, 333))
            {
                // Create a graphics object to draw the screenshot
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Capture the screen area starting at (x, y)
                    graphics.CopyFromScreen(clientCoords[0], clientCoords[1], 0, 0, new Size(510, 333));
                }
                // Define the target directory
                string picturesPath = @"C:\Users\corp bot\Documents";
                // Ensure the directory exists
                if (!Directory.Exists(picturesPath))
                {
                    Directory.CreateDirectory(picturesPath);
                }
                // Define the file path directly in D:\Botwatch\pics
                string filePath = Path.Combine(picturesPath, $"claim {totalTicks.ToString()}.png");
                // Save the screenshot as a PNG file
                bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        public void initPrayer()
        {
            for (int i = 0; i < prayerArray.Length; i++)
            {
                if(i%2 == 0)
                {
                    prayerArray[i] = 0;
                } else
                {
                    prayerArray[i] = 0;
                }
            }
        }

        public void setNextPrayer(int prayer)
        {
            //Console.WriteLine($"Setting next prayer: {tick}: {prayer.ToString()}");
            if (tick <= 8)
            {
                prayerArray[tick + 2] = prayer;
            }
            else if(tick == 9)
            {
                prayerArray[0] = prayer;
            }
            else if (tick == 10)
            {
                prayerArray[1] = prayer;
            }
        }
        public int currentTick = 0;
        public async void tickCounter()
        {
            //Console.WriteLine($"Doing tick counter: {tick}");
            while(!tick1())
            {
                await Task.Delay(10);
            }
            currentTick = tick;
            totalTicks++;
            //captureScreen();
            prayer.checkPrayer();
            while (!tick2())
            {
                await Task.Delay(10);
            }
            currentTick = tick;
            totalTicks++;
            //captureScreen();
            tickCounter();
            prayer.checkPrayer();
        }

        public void checkPrayers()
        {
            for(int i = 0; i < prayerArray.Length; i++)
            {
                //Console.Write(prayerArray[i]);
            }
            //Console.WriteLine("Checked prayers");
            if(tick > 8)
            {
                //Console.WriteLine($"Tick: {tick.ToString()}, Have prayer: {prayerActive.ToString()}, want prayer: {prayerArray[0].ToString()}");
                if (prayerArray[0] == prayerActive)
                {
                    //Console.WriteLine("No change needed");
                    return;
                }
                if (prayerArray[0] == 0)
                {
                    if (prayerActive == 1)
                    {
                        addMouseClick(601, 340, "prayer");
                    }
                    if (prayerActive == 2)
                    {
                        addMouseClick(639, 339, "prayer");
                    }
                    if (prayerActive == 3)
                    {
                        addMouseClick(675, 341, "prayer");
                    }
                    prayerActive = 0;
                    return;
                }
                if (prayerArray[0] == 1)
                {
                    addMouseClick(601, 340, "prayer");
                    prayerActive = 1;
                    return;
                }
                if (prayerArray[0] == 2)
                {
                    addMouseClick(639, 339, "prayer");
                    prayerActive = 2;
                    return;
                }
                if (prayerArray[0] == 3)
                {
                    addMouseClick(675, 341, "prayer");
                    prayerActive = 3;
                    return;
                }
            } else
            {
                //Console.WriteLine($"Tick: {tick.ToString()}, Have prayer: {prayerActive.ToString()}, want prayer: {prayerArray[tick + 1].ToString()}");
                if (prayerArray[tick + 1] == prayerActive)
                {
                    //Console.WriteLine("No change needed");
                    return; //do nothing
                }
                if (prayerArray[tick + 1] == 0)
                {
                    if (prayerActive == 1)
                    {
                        addMouseClick(601, 340, "prayer");
                    }
                    if (prayerActive == 2)
                    {
                        addMouseClick(639, 339, "prayer");
                    }
                    if (prayerActive == 3)
                    {
                        addMouseClick(675, 341, "prayer");
                    }
                    prayerActive = 0;
                    return;
                }
                if (prayerArray[tick + 1] == 1)
                {
                    addMouseClick(601, 340, "prayer");
                    prayerActive = 1;
                    return;
                }
                if (prayerArray[tick + 1] == 2)
                {
                    addMouseClick(639, 339, "prayer");
                    prayerActive = 2;
                    return;
                }
                if (prayerArray[tick + 1] == 3)
                {
                    addMouseClick(675, 341, "prayer");
                    prayerActive = 3;
                    return;
                }
            }
        }

        public bool tick1()
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
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G >= 252 && bitmap.GetPixel(x, y).B == 0)
                        {
                            if (currentTick == tick)
                            {
                                tick++;
                                colo.waveTicks++;
                                if (tick >= 9)
                                {
                                    tick = 0;
                                }
                                return true;
                            }
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool tick2()
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
                        if (bitmap.GetPixel(x, y).R >= 252 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 0)
                        {
                            if (currentTick == tick)
                            {
                                tick++;
                                colo.waveTicks++;
                                if (tick >= 9)
                                {
                                    tick = 0;
                                }
                                return true;
                            }
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        //end prayer array


        public async void startProcessor(string script)
        {
            busy = true;
            initMouse();
            player.processor = this;
            findClient();
            while (!clientFound)
            {
                await Task.Delay(10);
            }
            Console.WriteLine("Starting script: " + script);
            //startTicks();
            mouseClickLoop();
            inventory.clientCoords = clientCoords;
            interfaces.clientCoords = clientCoords;
            xpDrops.clientCoords = clientCoords;
            gotr.clientCoords = clientCoords;
            natures.clientCoords = clientCoords;
            anglers.clientCoords = clientCoords;
            fletch.clientCoords = clientCoords;
            corpTank.clientCoords = clientCoords;
            player.clientCoords = clientCoords;
            colo.clientCoords = clientCoords;
            hunter.clientCoords = clientCoords;
            wave2.clientCoords = clientCoords;
            fally.clientCoords = clientCoords;
            nex.clientCoords = clientCoords;
            Console.WriteLine("Above the switch");
            switch (script)
            {
                case "gotr":
                    gotr.interfaces = interfaces;
                    gotr.inventory = inventory;
                    gotr.player = player;
                    gotr.xpDrops = xpDrops;
                    gotr.processor = this;
                    gotr.enterPortal();
                    break;
                case "baba":
                    baba.prayer = prayer;
                    prayer.setPrayerArray();
                    prayer.processor = this;
                    baba.interfaces = interfaces;
                    baba.inventory = inventory;
                    baba.player = player;
                    baba.xpDrops = xpDrops;
                    baba.processor = this;
                    baba.inventory.processor = this;
                    baba.equipment = equipment;
                    busy = false;
                    baba.inventory.inventorySetup();
                    baba.equipment.setEquipment();
                    await Task.Delay(1000);
                    baba.startScript();
                    initPrayer();
                    tickCounter();
                    break;
                case "natures":
                    natures.interfaces = interfaces;
                    natures.inventory = inventory;
                    natures.player = player;
                    natures.xpDrops = xpDrops;
                    natures.processor = this;
                    natures.startScript();
                    break;
                case "anglers":
                    //Console.WriteLine("wadawdawd2");
                    anglers.interfaces = interfaces;
                    anglers.inventory = inventory;
                    anglers.player = player;
                    anglers.xpDrops = xpDrops;
                    anglers.processor = this;
                    anglers.startScript();
                    break;
                case "feltch":
                    fletch.interfaces = interfaces;
                    fletch.inventory = inventory;
                    fletch.player = player;
                    fletch.xpDrops = xpDrops;
                    fletch.processor = this;
                    fletch.startScript();
                    break;
                case "corp":
                    corpTank.prayer = prayer;
                    prayer.setPrayerArray();
                    prayer.processor = this;
                    corpTank.processor = this;
                    corpTank.interfaces = interfaces;
                    corpTank.inventory = inventory;
                    corpTank.player = player;
                    corpTank.xpDrops = xpDrops;
                    corpTank.inventory.processor = this;
                    initPrayer();
                    tickCounter();
                    corpTank.startScript();
                    break;
                case "colo":
                    Console.WriteLine("Im in the colo case");
                    colo.prayer = prayer;
                    prayer.setPrayerArray();
                    prayer.processor = this;
                    colo.interfaces = interfaces;
                    colo.inventory = inventory;
                    colo.player = player;
                    colo.xpDrops = xpDrops;
                    colo.processor = this;
                    colo.inventory.processor = this;
                    colo.equipment = equipment;
                    busy = false;
                    colo.inventory.inventorySetup();
                    colo.equipment.setEquipment();
                    wave2.equipment = equipment;
                    wave2.player = player;
                    wave2.processor = this;
                    wave2.inventory = inventory;
                    wave2.xpDrops = xpDrops;
                    wave2.interfaces = interfaces;
                    wave2.clientCoords = clientCoords;
                    wave2.prayer = prayer;
                    colo.wave2 = wave2;
                    await Task.Delay(1000);
                    Console.WriteLine("About to start the script");
                    colo.startScript();
                    initPrayer();
                    tickCounter();
                    await Task.Delay(1000);
                    break;
                case "data":
                    colo.prayer = prayer;
                    prayer.setPrayerArray();
                    prayer.processor = this;
                    colo.interfaces = interfaces;
                    colo.inventory = inventory;
                    colo.player = player;
                    colo.xpDrops = xpDrops;
                    colo.processor = this;
                    colo.inventory.processor = this;
                    colo.equipment = equipment;
                    busy = false;
                    colo.inventory.inventorySetup();
                    colo.equipment.setEquipment();
                    wave2.equipment = equipment;
                    wave2.player = player;
                    wave2.processor = this;
                    wave2.inventory = inventory;
                    wave2.xpDrops = xpDrops;
                    wave2.interfaces = interfaces;
                    wave2.clientCoords = clientCoords;
                    wave2.prayer = prayer;
                    colo.wave2 = wave2;
                    await Task.Delay(1000);
                    //colo.dataLoop();
                    initPrayer();
                    tickCounter();
                    await Task.Delay(1000);
                    break;
                case "wave2":
                    wave2.prayer = prayer;
                    prayer.setPrayerArray();
                    prayer.processor = this;
                    wave2.equipment = equipment;
                    wave2.player = player;
                    wave2.processor = this;
                    wave2.inventory = inventory;
                    wave2.xpDrops = xpDrops;
                    wave2.interfaces = interfaces;
                    wave2.clientCoords = clientCoords;
                    wave2.inventory.processor = this;
                    busy = false;
                    wave2.inventory.inventorySetup();
                    wave2.equipment.setEquipment();
                    wave2.checkLoop();
                    await Task.Delay(1000);
                    initPrayer();
                    tickCounter();
                    wave2.startScript();
                    break;
                case "hunter":
                    hunter.interfaces = interfaces;
                    hunter.inventory = inventory;
                    hunter.player = player;
                    hunter.xpDrops = xpDrops;
                    hunter.processor = this;
                    hunter.inventory.processor = this;
                    hunter.startScript();
                    break;
                case "test":
                    wave2.prayer = prayer;
                    prayer.setPrayerArray();
                    prayer.processor = this;
                    wave2.equipment = equipment;
                    wave2.player = player;
                    wave2.processor = this;
                    wave2.inventory = inventory;
                    wave2.xpDrops = xpDrops;
                    wave2.interfaces = interfaces;
                    wave2.clientCoords = clientCoords;
                    wave2.inventory.processor = this;
                    busy = false;
                    wave2.inventory.inventorySetup();
                    wave2.equipment.setEquipment();
                    wave2.checkLoop();
                    await Task.Delay(1000);
                    initPrayer();
                    tickCounter();
                    wave2.magePosLoop();
                    Console.WriteLine($"{wave2.rangePos.ToString()}");
                    wave2.solveWave(4);
                    break;
                case "nex":
                    nex.prayer = prayer;
                    prayer.setPrayerArray();
                    prayer.processor = this;
                    nex.equipment = equipment;
                    nex.player = player;
                    nex.processor = this;
                    nex.inventory = inventory;
                    nex.xpDrops = xpDrops;
                    nex.interfaces = interfaces;
                    nex.clientCoords = clientCoords;
                    nex.inventory.processor = this;
                    busy = false;
                    nex.inventory.inventorySetup();
                    nex.equipment.setEquipment();
                    await Task.Delay(1000);
                    initPrayer();
                    tickCounter();
                    nex.startScript();
                    break;
                case "fally":
                    fally.processor = this;
                    fally.player = player;
                    fally.xpDrops = xpDrops;
                    fally.interfaces = interfaces;
                    busy = false;
                    fally.startScript();
                    break;
                default:
                    //Console.WriteLine("Unkown script");
                    break;
            }
            busy = false;
        }
        public void initMouse()
        {
            for (int i = 0; i < 1000000; i++)
            {
                prayerClicks[i] = 0;
                movementClicks[i] = 0;
                inventoryClicks[i] = 0;
                spellbookClicks[i] = 0;
                gamescreenClicks[i] = 0;
            }
        }

        public bool prayerChanged = false;

        public async Task click()
        {
            busy = true;
            try
            {
                int tickTime = 550; // Budget for clicks in this cycle
                bool anyClickPerformed = false;

                // Priority clicks
                if (tickTime >= 160 && canExecuteClick(prayerClicks))
                {
                    prayerClicks = await executeClick(prayerClicks, 0, () => PressKey((byte)Keys.F3, 1), tabOpen: 3);
                    tickTime -= 160;
                    anyClickPerformed = true;
                }
                if (tickTime >= 160 && canExecuteClick(movementClicks))
                {
                    movementClicks = await executeClick(movementClicks, 0, null, tabOpen: 0);
                    tickTime -= 100;
                    anyClickPerformed = true;
                }

                // Loop for inventory and game screen clicks
                while (tickTime >= 100)
                {
                    bool clickedThisIteration = false;

                    if (canExecuteClick(inventoryClicks))
                    {
                        inventoryClicks = await executeClick(inventoryClicks, 0, () => PressKey((byte)Keys.F1, 1), tabOpen: 1);
                        tickTime -= 100;
                        clickedThisIteration = true;
                        anyClickPerformed = true;
                    }
                    else if (canExecuteClick(gamescreenClicks))
                    {
                        gamescreenClicks = await executeClick(gamescreenClicks, 0, null, tabOpen: 0);
                        tickTime -= 100;
                        clickedThisIteration = true;
                        anyClickPerformed = true;
                    }

                    if (!clickedThisIteration)
                    {
                        break;
                    }
                }

                // Fallback to attack clicks if nothing else was clicked
                if (!anyClickPerformed && tickTime >= 160 && canExecuteClick(attackClicks))
                {
                    attackClicks = await executeClick(attackClicks, 0, () => PressKey((byte)Keys.F4, 1), tabOpen: 4);
                }
            }
            finally
            {
                busy = false;
            }
        }

        private async Task<int[]> executeClick(int[] clicks, int startIndex, Action tabAction = null, int tabOpen = 0)
        {
            if (startIndex >= clicks.Length || clicks.Length < 2 || clicks[startIndex] == 0)
                return clicks;

            try
            {
                int x = clicks[startIndex];
                int y = clicks[startIndex + 1];

                Console.WriteLine($"Clicking: {x}, {y}");
                if (tabAction != null)
                {
                    tabAction();
                    await Task.Delay(100); // Delay after tab switch
                }

                Cursor.Position = new Point(clientCoords[0] + x, clientCoords[1] + y);
                if(tabOpen == 1 || tabOpen == 3)
                {
                    await Task.Delay(150); // Delay before click
                } else
                {
                    await Task.Delay(150); // Delay before click
                }
                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                if (tabOpen == 1 || tabOpen == 3)
                {
                    await Task.Delay(50); // Delay before click
                }
                else
                {
                    await Task.Delay(50); // Delay before click
                }
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);

                return clicks.Skip(2).Concat(new int[] { 0, 0 }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Click execution failed: {ex.Message}");
                return clicks;
            }
        }

        private bool canExecuteClick(int[] clicks)
        {
            // Check if there are any meaningful clicks left in the array
            return clicks != null && clicks.Length >= 2 && clicks[0] != 0;
        }

        public int tickTime = 560;

        private async void mouseClickLoop()
        {
            while (true)
            {
                await click();
                if (tick1())
                {
                    while (tick1())
                    {
                        await Task.Delay(10);
                    }
                }
                else if (tick2())
                {
                    while (tick2())
                    {
                        await Task.Delay(10);
                    }
                }
            }
        }

        public bool holdingShift = false;
        public class ShiftHolder : IDisposable
        {
            public ShiftHolder()
            {
                keybd_event((byte)Keys.ShiftKey, 0, KEY_DOWN_EVENT, 0);
            }

            public void Dispose()
            {
                keybd_event((byte)Keys.ShiftKey, 0, KEY_UP_EVENT, 0);
            }
        }

        public async void PressKey(byte key, int duration)
        {
            int totalDuration = 0;
            while (totalDuration < duration)
            {
                keybd_event(key, 0, KEY_DOWN_EVENT, 0);
                await Task.Delay(10);
                keybd_event(key, 0, KEY_UP_EVENT, 0);
                totalDuration += 1;
            }
        }

        public void addMouseClick(int x, int y, string cat = "gamescreen")
        {
            switch (cat)
            {
                case "gamescreen":
                    for (int i = 0; i < gamescreenClicks.Length; i++)
                    {
                        if (gamescreenClicks[i] == 0)
                        {
                            gamescreenClicks[i] = x;
                            gamescreenClicks[i + 1] = y;
                            return;
                        }
                    }
                    break;
                case "inventory":
                    for (int i = 0; i < inventoryClicks.Length; i++)
                    {
                        if (inventoryClicks[i] == 0)
                        {
                            inventoryClicks[i] = x;
                            inventoryClicks[i + 1] = y;
                            return;
                        }
                    }
                    break;
                case "prayer":
                    for (int i = 0; i < prayerClicks.Length; i++)
                    {
                        if (prayerClicks[i] == 0)
                        {
                            prayerClicks[i] = x;
                            prayerClicks[i + 1] = y;
                            return;
                        }
                    }
                    break;
                case "movement":
                    for (int i = 0; i < movementClicks.Length; i++)
                    {
                        if (movementClicks[i] == 0)
                        {
                            movementClicks[i] = x;
                            movementClicks[i + 1] = y;
                            return;
                        }
                    }
                    break;
                case "attack":
                    for (int i = 0; i < attackClicks.Length; i++)
                    {
                        if (attackClicks[i] == 0)
                        {
                            attackClicks[i] = x;
                            attackClicks[i + 1] = y;
                            return;
                        }
                    }
                    break;

            }
        }

        private async void leftClick(int x, int y, int a = 150, int b = 30, int c = 100)
        {
            busy = true;
            Cursor.Position = new Point(clientCoords[0] + x, clientCoords[1] +y);
            await Task.Delay(a);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            await Task.Delay(b);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            await Task.Delay(c);
            busy = false;
        }

        public void moveMouse(int x, int y)
        {
            Cursor.Position = new Point(clientCoords[0] + x, clientCoords[1] + y);
        }

        public async void instaClick(int x, int y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        public async void repeat()
        {
            if (inventory.hasItem("Full ess"))
            {
                //Console.WriteLine("Full inventory");
            }
            await Task.Delay(100);
            repeat();
        }

        public async void startTicks()
        {
            if(findTick1())
            {
                while(findTick1())
                {
                    await Task.Delay(10);
                }
                busy = false;
            }
            //do stuff
            if (findTick2())
            {
                while (findTick2())
                {
                    await Task.Delay(10);
                }
                busy = false;
            }
            startTicks();
        }

        public async void printMap()
        {
            //Console.Clear();
        }

        public async void printStats()
        {
            //Console.Clear();
        }

        public async void rightClick(int x, int y)
        {
            Cursor.Position = new Point(clientCoords[0] + x, clientCoords[1] + y);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, clientCoords[0], clientCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_RIGHTUP, clientCoords[0], clientCoords[1], 0, 0);
            await Task.Delay(100);
            busy = false;
        }

        public bool white()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0], clientCoords[1], 0, 0, new Size(20, 20));
                }
                for (int x = 0; x < 19; x++)
                {
                    for (int y = 0; y < 19; y++)
                    {
                        if (bitmap.GetPixel(x, y).R >= 200 && bitmap.GetPixel(x, y).G >= 200 && bitmap.GetPixel(x, y).B >= 200)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool findTick1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 500))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 245, clientCoords[1] + 6, 0, 0, new Size(1, 1));
                }
                if (bitmap.GetPixel(0, 0) == Color.FromArgb(255, 0, 0))
                {
                    return true;
                }
            }
            return false;
        }

        public bool findTick2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 500))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 245, clientCoords[1] + 6, 0, 0, new Size(1, 1));
                }
                if (bitmap.GetPixel(0, 0) == Color.FromArgb(0, 255, 0))
                {
                    return true;
                }
            }
            return false;
        }

        public async void findClient()
        {
            await Task.Delay(100);
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                screen = bitmap;

                for (int x = 0; x < screen.Width; x++)
                {
                    for (int y = 0; y < screen.Height; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(25, 23, 17))
                        {
                            Console.WriteLine("Client found");
                            clientFound = true;
                            clientCoords[0] = x + 3;
                            clientCoords[1] = y + 3;
                            return;
                        }
                    }
                }
                //Console.WriteLine("Cannot find client");

            }
        }

        public float getPlayerHealth()
        {
            return player.playerHealth();
        }

        public float getPlayerPrayer()
        {
            return 0;
        }
    }
}