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

namespace Bot.Core
{
    internal class CoreProcessor
    {

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

        public int[] prayerClicks = new int[100000];
        public int[] movementClicks = new int[100000];
        public int[] inventoryClicks = new int[100000];
        public int[] spellbookClicks = new int[100000];
        public int[] gamescreenClicks = new int[100000];

        public int[] tempPClicks = new int[100000];
        public int[] tempMClicks = new int[100000];
        public int[] tempIClicks = new int[100000];
        public int[] tempSClicks = new int[100000];
        public int[] tempGClicks = new int[100000];

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
            Console.WriteLine($"Setting next prayer: {tick}: {prayer.ToString()}");
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
            Console.WriteLine($"Doing tick counter: {tick}");
            while(!tick1())
            {
                await Task.Delay(10);
            }
            currentTick = tick;
            prayer.checkPrayer();
            while (!tick2())
            {
                await Task.Delay(10);
            }
            currentTick = tick;
            tickCounter();
            prayer.checkPrayer();
            await Task.Delay(50);
        }

        public void checkPrayers()
        {
            for(int i = 0; i < prayerArray.Length; i++)
            {
                Console.Write(prayerArray[i]);
            }
            Console.WriteLine("Checked prayers");
            if(tick > 8)
            {
                //Console.WriteLine($"Tick: {tick.ToString()}, Have prayer: {prayerActive.ToString()}, want prayer: {prayerArray[0].ToString()}");
                if (prayerArray[0] == prayerActive)
                {
                    Console.WriteLine("No change needed");
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
                    Console.WriteLine("No change needed");
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
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            if (currentTick == tick)
                            {
                                tick++;
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
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 0 && bitmap.GetPixel(x, y).B == 0)
                        {
                            if (currentTick == tick)
                            {
                                tick++;
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

            if (script == "gotr")
            {
                gotr.interfaces = interfaces;
                gotr.inventory = inventory;
                gotr.player = player;
                gotr.xpDrops = xpDrops;
                gotr.processor = this;
                gotr.enterPortal();
            }
            if (script == "baba")
            {
                baba.interfaces = interfaces;
                baba.inventory = inventory;
                baba.player = player;
                baba.xpDrops = xpDrops;
                baba.processor = this;
                baba.startScript();
            }
            if (script == "natures")
            {
                natures.interfaces = interfaces;
                natures.inventory = inventory;
                natures.player = player;
                natures.xpDrops = xpDrops;
                natures.processor = this;
                natures.startScript();
            }
            if (script == "ladder")
            {
                natures.interfaces = interfaces;
                natures.inventory = inventory;
                natures.player = player;
                natures.xpDrops = xpDrops;
                natures.processor = this;
                natures.ladPos();
            }
            if (script == "anglers")
            {
                anglers.interfaces = interfaces;
                anglers.inventory = inventory;
                anglers.player = player;
                anglers.xpDrops = xpDrops;
                anglers.processor = this;
                anglers.startScript();
            }
            if (script == "fletch")
            {
                fletch.interfaces = interfaces;
                fletch.inventory = inventory;
                fletch.player = player;
                fletch.xpDrops = xpDrops;
                fletch.processor = this;
                fletch.startScript();
            }
            if (script == "corp")
            {
                corpTank.interfaces = interfaces;
                corpTank.inventory = inventory;
                corpTank.player = player;
                corpTank.xpDrops = xpDrops;
                corpTank.processor = this;
                corpTank.inventory.processor = this;
                corpTank.startScript();
            }
            if (script == "colo")
            {
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
                colo.startScript();
                initPrayer();
                tickCounter();
                await Task.Delay(1000);
            }
            if (script == "wave2")
            {
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
                wave2.killSingleRanger1();
            }
            if (script == "prayer test")
            {
                initPrayer();
                tickCounter();
                colo.interfaces = interfaces;
                colo.inventory = inventory;
                colo.player = player;
                colo.xpDrops = xpDrops;
                colo.processor = this;
                colo.inventory.processor = this;
                colo.setFremPrayers();
            }
            if (script == "hunter")
            {
                hunter.interfaces = interfaces;
                hunter.inventory = inventory;
                hunter.player = player;
                hunter.xpDrops = xpDrops;
                hunter.processor = this;
                hunter.inventory.processor = this;
                hunter.startScript();
            }
            busy = false;
        }
        public void initMouse()
        {
            for (int i = 0; i < 10000; i++)
            {
                prayerClicks[i] = 0;
                movementClicks[i] = 0;
                inventoryClicks[i] = 0;
                spellbookClicks[i] = 0;
                gamescreenClicks[i] = 0;
            }
        }

        public bool prayerChanged = false;

        public async void click1()
        {
            if (tempPClicks[0] != 0)
            {
                PressKey((byte)Keys.F3, 1);
                tabOpen = 3;
                await Task.Delay(60);
                Cursor.Position = new Point(clientCoords[0] + tempPClicks[0], clientCoords[1] + tempPClicks[1]);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, tempPClicks[0], tempPClicks[1], 0, 0);
                await Task.Delay(50);
                mouse_event(MOUSEEVENTF_LEFTUP, tempPClicks[0], tempPClicks[1], 0, 0);
                await Task.Delay(150);
                prayerClicks = prayerClicks.Skip(2).ToArray();
                prayerClicks.Append(0);
                prayerClicks.Append(0);
                tempPClicks = tempPClicks.Skip(2).ToArray();
                tempPClicks.Append(0);
                tempPClicks.Append(0);
                tickTime -= (30 + 100 + 50 + 150 + 10);
            }
            else
            {
                if (tempIClicks[0] != 0)
                {
                    Console.WriteLine("Has inventory click");
                    if (tabOpen == 1)
                    {
                        Cursor.Position = new Point(clientCoords[0] + tempIClicks[0], clientCoords[1] + tempIClicks[1]);
                        await Task.Delay(50);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, tempIClicks[0], tempIClicks[1], 0, 0);
                        await Task.Delay(30);
                        mouse_event(MOUSEEVENTF_LEFTUP, tempIClicks[0], tempIClicks[1], 0, 0);
                        await Task.Delay(30);
                        inventoryClicks = inventoryClicks.Skip(2).ToArray();
                        inventoryClicks.Append(0);
                        inventoryClicks.Append(0);
                        tempIClicks = tempIClicks.Skip(2).ToArray();
                        tempIClicks.Append(0);
                        tempIClicks.Append(0);
                        tickTime -= (30 + 10 + 30);
                    }
                    else
                    {
                        PressKey((byte)Keys.F1, 1);
                        tabOpen = 1;
                        await Task.Delay(30);
                        Cursor.Position = new Point(clientCoords[0] + tempIClicks[0], clientCoords[1] + tempIClicks[1]);
                        await Task.Delay(50);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, tempIClicks[0], tempIClicks[1], 0, 0);
                        await Task.Delay(30);
                        mouse_event(MOUSEEVENTF_LEFTUP, tempIClicks[0], tempIClicks[1], 0, 0);
                        await Task.Delay(30);
                        inventoryClicks = inventoryClicks.Skip(2).ToArray();
                        inventoryClicks.Append(0);
                        inventoryClicks.Append(0);
                        tempIClicks.Append(0);
                        tempIClicks.Append(0);
                        tempIClicks = tempIClicks.Skip(2).ToArray();
                        tickTime -= (30 + 30 + 10 + 30);
                    }
                }
                else if (tempGClicks[0] != 0)
                {
                    Cursor.Position = new Point(clientCoords[0] + tempGClicks[0], clientCoords[1] + tempGClicks[1]);
                    await Task.Delay(200);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, tempGClicks[0], tempGClicks[1], 0, 0);
                    await Task.Delay(40);
                    mouse_event(MOUSEEVENTF_LEFTUP, tempGClicks[0], tempGClicks[1], 0, 0);
                    await Task.Delay(30);
                    gamescreenClicks = gamescreenClicks.Skip(2).ToArray();
                    gamescreenClicks.Append(0);
                    gamescreenClicks.Append(0);
                    tempGClicks.Append(0);
                    tempGClicks.Append(0);
                    tempGClicks = tempGClicks.Skip(2).ToArray();
                    tickTime -= (150 + 10 + 30);
                }
            }
            click3();
        }

        public async void click2()
        {
            for (int i = 0; i < 9; i++)
            {
                tempGClicks[i] = 0;
                tempIClicks[i] = 0;
                tempMClicks[i] = 0;
                tempPClicks[i] = 0;
                tempSClicks[i] = 0;
            }
            for (int i = 0; i < 9; i++)
            {
                tempGClicks[i] = gamescreenClicks[i];
                tempIClicks[i] = inventoryClicks[i];
                tempMClicks[i] = movementClicks[i];
                tempPClicks[i] = prayerClicks[i];
            }
            busy = true;
            tickTime = 580;
            if (tempPClicks[0] != 0)
            {
                PressKey((byte)Keys.F3, 1);
                tabOpen = 3;
                await Task.Delay(60);
                Cursor.Position = new Point(clientCoords[0] + tempPClicks[0], clientCoords[1] + tempPClicks[1]);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, tempPClicks[0], tempPClicks[1], 0, 0);
                await Task.Delay(50);
                mouse_event(MOUSEEVENTF_LEFTUP, tempPClicks[0], tempPClicks[1], 0, 0);
                await Task.Delay(150);
                prayerClicks = prayerClicks.Skip(2).ToArray();
                prayerClicks.Append(0);
                prayerClicks.Append(0);
                tempPClicks = tempPClicks.Skip(2).ToArray();
                tempPClicks.Append(0);
                tempPClicks.Append(0);
                tickTime -= (30 + 100 + 50 + 150 + 10);
            }
            else
            {
                if (tempMClicks[0] != 0)
                {
                    Cursor.Position = new Point(clientCoords[0] + tempMClicks[0], clientCoords[1] + tempMClicks[1]);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, tempMClicks[0], tempMClicks[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, tempMClicks[0], tempMClicks[1], 0, 0);
                    await Task.Delay(100);
                    movementClicks = movementClicks.Skip(2).ToArray();
                    movementClicks.Append(0);
                    movementClicks.Append(0);
                    tempMClicks.Append(0);
                    tempMClicks.Append(0);
                    tempMClicks = tempMClicks.Skip(2).ToArray();
                    tickTime -= (50 + 10 + 100);
                }
                else
                {
                    if (tempIClicks[0] != 0)
                    {
                        skip = true;
                        if (tabOpen == 1)
                        {
                            Cursor.Position = new Point(clientCoords[0] + tempIClicks[0], clientCoords[1] + tempIClicks[1]);
                            await Task.Delay(50);
                            mouse_event(MOUSEEVENTF_LEFTDOWN, tempIClicks[0], tempIClicks[1], 0, 0);
                            await Task.Delay(30);
                            mouse_event(MOUSEEVENTF_LEFTUP, tempIClicks[0], tempIClicks[1], 0, 0);
                            await Task.Delay(30);
                            inventoryClicks = inventoryClicks.Skip(2).ToArray();
                            inventoryClicks.Append(0);
                            inventoryClicks.Append(0);
                            tempIClicks.Append(0);
                            tempIClicks.Append(0);
                            tempIClicks = tempIClicks.Skip(2).ToArray();
                            tickTime -= (30 + 10 + 30);
                        }
                        else
                        {
                            PressKey((byte)Keys.F1, 1);
                            tabOpen = 1;
                            await Task.Delay(30);
                            Cursor.Position = new Point(clientCoords[0] + tempIClicks[0], clientCoords[1] + tempIClicks[1]);
                            await Task.Delay(50);
                            mouse_event(MOUSEEVENTF_LEFTDOWN, tempIClicks[0], tempIClicks[1], 0, 0);
                            await Task.Delay(30);
                            mouse_event(MOUSEEVENTF_LEFTUP, tempIClicks[0], tempIClicks[1], 0, 0);
                            await Task.Delay(30);
                            inventoryClicks = inventoryClicks.Skip(2).ToArray();
                            inventoryClicks.Append(0);
                            inventoryClicks.Append(0);
                            tempIClicks.Append(0);
                            tempIClicks.Append(0);
                            tempIClicks = tempIClicks.Skip(2).ToArray();
                            tickTime -= (30 + 30 + 10 + 30);
                        }
                    }
                    else if (tempGClicks[0] != 0)
                    {
                        Cursor.Position = new Point(clientCoords[0] + tempGClicks[0], clientCoords[1] + tempGClicks[1]);
                        await Task.Delay(200);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, tempGClicks[0], tempGClicks[1], 0, 0);
                        await Task.Delay(10);
                        mouse_event(MOUSEEVENTF_LEFTUP, tempGClicks[0], tempGClicks[1], 0, 0);
                        await Task.Delay(30);
                        gamescreenClicks = gamescreenClicks.Skip(2).ToArray();
                        gamescreenClicks.Append(0);
                        gamescreenClicks.Append(0);
                        tempGClicks.Append(0);
                        tempGClicks.Append(0);
                        tempGClicks = tempGClicks.Skip(2).ToArray();
                        tickTime -= (150 + 10 + 30);
                    }
                }
            }
            click1();
        }

        public async void click3()
        {
            if (tempIClicks[0] != 0)
            {
                skip = true;
                if (tabOpen == 1)
                {
                    Cursor.Position = new Point(clientCoords[0] + tempIClicks[0], clientCoords[1] + tempIClicks[1]);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, tempIClicks[0], tempIClicks[1], 0, 0);
                    await Task.Delay(30);
                    mouse_event(MOUSEEVENTF_LEFTUP, tempIClicks[0], tempIClicks[1], 0, 0);
                    await Task.Delay(30);
                    inventoryClicks = inventoryClicks.Skip(2).ToArray();
                    inventoryClicks.Append(0);
                    inventoryClicks.Append(0);
                    tempIClicks.Append(0);
                    tempIClicks.Append(0);
                    tempIClicks = tempIClicks.Skip(2).ToArray();
                    tickTime -= (30 + 10 + 30);
                }
                else
                {
                    PressKey((byte)Keys.F1, 1);
                    tabOpen = 1;
                    await Task.Delay(30);
                    tickTime -= 30;
                    Cursor.Position = new Point(clientCoords[0] + tempIClicks[0], clientCoords[1] + tempIClicks[1]);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, tempIClicks[0], tempIClicks[1], 0, 0);
                    await Task.Delay(30);
                    mouse_event(MOUSEEVENTF_LEFTUP, tempIClicks[0], tempIClicks[1], 0, 0);
                    await Task.Delay(30);
                    inventoryClicks = inventoryClicks.Skip(2).ToArray();
                    inventoryClicks.Append(0);
                    inventoryClicks.Append(0);
                    tempIClicks.Append(0);
                    tempIClicks.Append(0);
                    tempIClicks = tempIClicks.Skip(2).ToArray();
                    tickTime -= (30 + 10 + 30);
                }
            }
            else if (tempGClicks[0] != 0)
            {
                Cursor.Position = new Point(clientCoords[0] + tempGClicks[0], clientCoords[1] + tempGClicks[1]);
                await Task.Delay(200);
                mouse_event(MOUSEEVENTF_LEFTDOWN, tempGClicks[0], tempGClicks[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, tempGClicks[0], tempGClicks[1], 0, 0);
                await Task.Delay(30);
                gamescreenClicks = gamescreenClicks.Skip(2).ToArray();
                gamescreenClicks.Append(0);
                gamescreenClicks.Append(0);
                tempGClicks.Append(0);
                tempGClicks.Append(0);
                tempGClicks = tempGClicks.Skip(2).ToArray();
                tickTime -= (150 + 10 + 30);
            }
            busy = false;
        }

        public int tickTime = 560;

        public async void mouseClickLoop()
        {
            click2();
            if (tick1())
            {
                //Console.WriteLine("Waiting for tick2 now");
                while (tick1())
                {
                    await Task.Delay(10);
                }
            }
            else if (tick2())
            {
                //Console.WriteLine("Waiting for tick1 now");
                while (tick2())
                {
                    await Task.Delay(10);
                }
            }
            mouseClickLoop();
        }

        public void PressKey(byte key, int duration)
        {
            int totalDuration = 0;
            while (totalDuration < duration)
            {
                keybd_event(key, 0, KEY_DOWN_EVENT, 0);
                System.Threading.Thread.Sleep(10);
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
                Console.WriteLine("Full inventory");
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
            Console.Clear();
        }

        public async void printStats()
        {
            Console.Clear();
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
                Console.WriteLine("Cannot find client");

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