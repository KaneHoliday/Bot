using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class CorpBlocker
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        public Inventory inv = new Inventory();
        public Banking banking = new Banking();
        public Peripherals mouse = new Peripherals();
        public DropAnal dropanal = new DropAnal();

        public int[] gameScreenCoords = new int[2] { 0, 0 };
        public int[] attackCoords = new int[2];
        public int[] safespot = new int[2];
        public int[] chatbox = new int[2];
        public Color checkMovement;
        public bool attack;
        public int kills;
        public int levelUps;
        public bool spec;
        public string specialWeapon;
        public bool att = true;
        public bool wait = false;
        public int bankWalkx;
        public int bankWalky;

        public bool firstKill = true;

        public int portalx;
        public int portaly;

        public bool alch;
        public bool fletch;

        public bool pass1;
        public bool pass2;
        public bool pass3;

        public bool specd;

        public int cnt = 6;

        public bool contFletch = false;

        public float storedHp = 0;

        public Player player = new Player();

        public bool sixHourTimer = false;

        public string potion = "";

        public bool enablePanic = false;
        public bool screeny = false;

        public void switchPotion()
        {
            switch(potion)
            {
                case "Super Combat Potion":
                    potion = "Super Attack Potion";
                    break;
                case "Super Attack Potion":
                    potion = "Nothing";
                    break;
                case "Nothing":
                    potion = "Nothing";
                    break;
                default:
                    MessageBox.Show("Unknown potion");
                    break;
            }
        }

        public async void panicTele()
        {
            if (storedHp > player.getHp())
            {
                inv.clickInventory("Panic tab");
                firstKill = true;
            }
            else
            {
                await Task.Delay(100);
                panicTele();
            }
        }

        public async void doAlch()
        {
            if (!checkSpec() || !corpHealthOverHalf())
            {
                if (alch)
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 739, gameScreenCoords[1] + 180);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(50);
                    Cursor.Position = new Point(gameScreenCoords[0] + 719, gameScreenCoords[1] + 318);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(50);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(50);
                    att = true;
                }
            }
            return;
        }

        public async void doFletch()
        {
            if (fletch)
            {
                if (contFletch)
                {
                    if (cnt == 6)
                    {
                        Cursor.Position = new Point(gameScreenCoords[0] + 617, gameScreenCoords[1] + 335);
                        await Task.Delay(50);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                        await Task.Delay(50);
                        mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                        await Task.Delay(50);
                        Cursor.Position = new Point(gameScreenCoords[0] + 661, gameScreenCoords[1] + 337);
                        await Task.Delay(50);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                        await Task.Delay(50);
                        mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                        await Task.Delay(50);
                        Peripherals.HoldKey((byte)Keys.Space, 5);
                        cnt = 0;
                        return;
                    }
                    else
                    {
                        cnt++;
                    }
                }
            }
        }

        public async void startScript()
        {
            attack = true;
            if (relogNow())
            {
                sixHourTimer = true;
            }

            if (storedHp > player.getHp())
            {
                await Task.Delay(10000);
                teleToCorp();
                return;
            }
            await Task.Delay(100);
            if (!checkNPC())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 641, gameScreenCoords[1] + 183);
                await Task.Delay(50);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(50);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(50);
                if (gotDrop())
                {
                    if (screeny)
                    {
                        screenshotDrop();
                    }
                    pickUpDrop();
                    return;
                }
                else
                {
                    if (screeny)
                    {
                        screenshotDrop();
                    }
                    if (inv.hasItem(potion) && potion != "Nothing")
                    {
                        if (sixHourTimer)
                        {
                            logout();
                            return;
                        }
                        await Task.Delay(300);
                        headToDoor();
                        return;
                    } else
                    {
                        bank();
                        return;
                    }
                }
            }
            if (!flincher())
            {
                if (storedHp > player.getHp())
                {
                    await Task.Delay(10000);
                    teleToCorp();
                    return;
                }
                if (att)
                {
                    if (storedHp > player.getHp())
                    {
                        await Task.Delay(10000);
                        teleToCorp();
                        return;
                    }
                    await Task.Delay(200);
                    Cursor.Position = new Point(attackCoords[0], attackCoords[1]);
                    await Task.Delay(200);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(3600);
                    if(!corpAlmostDead())
                    {
                        doAlch();
                    }
                    att = false;
                }
                if (spec)
                {
                    if (storedHp > player.getHp())
                    {
                        await Task.Delay(10000);
                        teleToCorp();
                        return;
                    }
                    if (checkSpec())
                    {
                        if (storedHp > player.getHp())
                        {
                            await Task.Delay(10000);
                            teleToCorp();
                            return;
                        }
                        if (corpHealthOverHalf())
                        {
                            if (storedHp > player.getHp())
                            {
                                await Task.Delay(10000);
                                teleToCorp();
                                return;
                            }
                            if (!specd)
                            {
                                if (storedHp > player.getHp())
                                {
                                    await Task.Delay(10000);
                                    teleToCorp();
                                    return;
                                }
                                Cursor.Position = new Point(gameScreenCoords[0] + 641, gameScreenCoords[1] + 183);
                                await Task.Delay(50);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                                await Task.Delay(50);
                                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                                await Task.Delay(50);
                                inv.equipItem(specialWeapon);
                                await Task.Delay(300);
                                doSpecs();
                                return;
                            }
                        }
                    }
                    attack = true;
                }
            }
            startScript();
        }

        public void ham()
        {
            inv.equipItem("Dragon Warhammer");
        }

        public bool relogNow()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(194, 111))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 298, gameScreenCoords[1] + 342, 0, 0, new Size(194, 111));
                }

                for (int i = 0; i < 193; i++)
                {
                    for (int j = 0; j < 110; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 127 && bitmap.GetPixel(i, j).B == 127)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void cameraPos()
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 558, gameScreenCoords[1] + 16);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_RIGHTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(100);
            Cursor.Position = new Point(gameScreenCoords[0] + 563, gameScreenCoords[1] + 93);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Peripherals.HoldKey((byte)Keys.Up, 10);
            Cursor.Position = new Point(gameScreenCoords[0] + 673, gameScreenCoords[1] + 482);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 702, gameScreenCoords[1] + 220);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 700, gameScreenCoords[1] + 314);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 632, gameScreenCoords[1] + 314);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 640, gameScreenCoords[1] + 183);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(600);
            setSpecWep();
            setPotion();
            enterDoor();
        }

        public void setSpecWep()
        {
            if (inv.hasItem("Dragon Warhammer"))
            {
                spec = true;
                specialWeapon = "Dragon Warhammer";
            } else
            {
                spec = false;
            }
        }

        public void setPotion()
        {
            if (inv.hasItem("Super Combat Potion"))
            {
                potion = "Super Combat Potion";
            } else if (inv.hasItem("Super Attack Potion"))
            {
                potion = "Super Attack Potion";
            } else
            {
                potion = "Nothing";
            }
        }

        public async void logout()
        {
            sixHourTimer = false;
            att = true;
            Cursor.Position = new Point(gameScreenCoords[0] + 638, gameScreenCoords[1] + 481);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 642, gameScreenCoords[1] + 429);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            login();
        }

        public async void login()
        {
            if (atLoginScreen())
            {
                if (pass1)
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 468, gameScreenCoords[1] + 293);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    Peripherals.PressKey((byte)Keys.B, 1);
                    Peripherals.PressKey((byte)Keys.E, 1);
                    Peripherals.PressKey((byte)Keys.S, 1);
                    Peripherals.PressKey((byte)Keys.T, 1);
                    Peripherals.PressKey((byte)Keys.P, 1);
                    Peripherals.PressKey((byte)Keys.K, 1);
                    Peripherals.PressKey((byte)Keys.E, 1);
                    Peripherals.PressKey((byte)Keys.R, 1);
                    await Task.Delay(1000);
                    Peripherals.PressKey((byte)Keys.Enter, 1);
                    await Task.Delay(1000);
                }
                if (pass2)
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 468, gameScreenCoords[1] + 293);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    Peripherals.PressKey((byte)Keys.P, 1);
                    Peripherals.PressKey((byte)Keys.R, 1);
                    Peripherals.PressKey((byte)Keys.NumPad4, 1);
                    Peripherals.PressKey((byte)Keys.S, 1);
                    Peripherals.PressKey((byte)Keys.T, 1);
                    Peripherals.PressKey((byte)Keys.A, 1);
                    Peripherals.PressKey((byte)Keys.S, 1);
                    Peripherals.PressKey((byte)Keys.H, 1);
                    Peripherals.PressKey((byte)Keys.U, 1);
                    Peripherals.PressKey((byte)Keys.N, 1);
                    Peripherals.PressKey((byte)Keys.NumPad4, 1);
                    Peripherals.PressKey((byte)Keys.NumPad5, 1);
                    await Task.Delay(1000);
                    Peripherals.PressKey((byte)Keys.Enter, 1);
                    await Task.Delay(1000);
                }
                if (pass3)
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 468, gameScreenCoords[1] + 293);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    Peripherals.PressKey((byte)Keys.D, 1);
                    Peripherals.PressKey((byte)Keys.I, 1);
                    Peripherals.PressKey((byte)Keys.C, 1);
                    Peripherals.PressKey((byte)Keys.K, 1);
                    Peripherals.PressKey((byte)Keys.B, 1);
                    Peripherals.PressKey((byte)Keys.U, 1);
                    Peripherals.PressKey((byte)Keys.T, 1);
                    Peripherals.PressKey((byte)Keys.T, 1);
                    await Task.Delay(1000);
                    Peripherals.PressKey((byte)Keys.Enter, 1);
                    await Task.Delay(1000);
                }
                clickPlay();
            }
            else
            {
                await Task.Delay(600);
                login();
            }
        }

        public async void clickPlay()
        {
            if (!atLoginScreen())
            {
                await Task.Delay(600);
                Cursor.Position = new Point(gameScreenCoords[0] + 411, gameScreenCoords[1] + 336);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                lurerEnterPortal();
            }
            else
            {
                await Task.Delay(600);
                clickPlay();
            }
        }

        public bool atLoginScreen()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(162, 17))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 304, gameScreenCoords[1] + 238, 0, 0, new Size(162, 17));
                }

                for (int i = 0; i < 161; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 255)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void lurerEnterPortal()
        {
            await Task.Delay(2000);
            Cursor.Position = new Point(gameScreenCoords[0] + 558, gameScreenCoords[1] + 16);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_RIGHTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(100);
            Cursor.Position = new Point(gameScreenCoords[0] + 563, gameScreenCoords[1] + 93);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Peripherals.HoldKey((byte)Keys.Up, 10);
            Cursor.Position = new Point(gameScreenCoords[0] + 673, gameScreenCoords[1] + 482);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 702, gameScreenCoords[1] + 220);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 700, gameScreenCoords[1] + 314);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 632, gameScreenCoords[1] + 314);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            Cursor.Position = new Point(gameScreenCoords[0] + 640, gameScreenCoords[1] + 183);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(3000);
            if (pos1())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 236, gameScreenCoords[1] + 168);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                moveToFlinchStart();
            }
            else
            if (pos2())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 254, gameScreenCoords[1] + 151);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                moveToFlinchStart();
            }
        }

        public bool pos1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 644, gameScreenCoords[1] + 68, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }
        public bool pos2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 648, gameScreenCoords[1] + 64, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void moveToFlinchStart()
        {
            if (enterTextbox())
            {
                await Task.Delay(1000);
                Cursor.Position = new Point(gameScreenCoords[0] + 416, gameScreenCoords[1] + 288);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                cameraPos();
            }
            else
            {
                await Task.Delay(600);
                moveToFlinchStart();
            }
        }

        public bool enterTextbox()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 346, gameScreenCoords[1] + 390, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 29; i++)
                {
                    for (int j = 0; j < 29; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 120 && bitmap.GetPixel(i, j).R <= 130)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool corpAlmostDead()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 24, gameScreenCoords[1] + 47, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 99 && bitmap.GetPixel(i, j).G == 20)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool corpHealthOverHalf()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 68, gameScreenCoords[1] + 40, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 10 && bitmap.GetPixel(i, j).G >= 130)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool corpNoHealth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 7, gameScreenCoords[1] + 41, 0, 0, new Size(2, 20));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 19; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 90 && bitmap.GetPixel(i, j).G <= 30)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool gotDrop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 628, gameScreenCoords[1] + 63, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 250)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void pickUpDrop()
        {
            mouse.leftClick(213, 92);
            await Task.Delay(3000);
            if (!inv.hasItem(potion) && potion != "Nothing")
            {
                bank();
                return;
            }
            if (sixHourTimer)
            {
                logout();
                return;
            }
            mouse.leftClick(292, 91);
            exitRoom();
        }

        public async void doSpecs()
        {
            if (storedHp > player.getHp())
            {
                await Task.Delay(10000);
                teleToCorp();
                return;
            }
            att = true;
            Cursor.Position = new Point(gameScreenCoords[0] + 585, gameScreenCoords[1] + 146);
            await Task.Delay(200);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            attack = true;
            await Task.Delay(100);
            if (moved())
            {
                if (storedHp > player.getHp())
                {
                    await Task.Delay(10000);
                    teleToCorp();
                    return;
                }
                await Task.Delay(600);
                Cursor.Position = new Point(attackCoords[0], attackCoords[1]);
                await Task.Delay(200);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(500);
                Cursor.Position = new Point(safespot[0], safespot[1]);
                await Task.Delay(2000);
                mouse_event(MOUSEEVENTF_LEFTDOWN, safespot[0], safespot[1], 0, 0);
                if (checkSpec())
                {
                    doSpecs();
                    return;
                }
                attack = true;
            }
            mouse.leftClick(inv.getPreviousClick("x"), inv.getPreviousClick("y"));
            specd = true;
            startScript();
        }

        public bool checkForLevel()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(100, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(chatbox[0] + 108, chatbox[1] + 34, 0, 0, new Size(100, 30));
                }

                for (int i = 0; i < 99; i++)
                {
                    for (int j = 0; j < 29; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 0)
                        {
                            if (bitmap.GetPixel(i, j).B == 255)
                            {
                                if (bitmap.GetPixel(i, j).G == 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }

        public async void headToDoor()
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 264, gameScreenCoords[1] + 16);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            exitRoom();
        }

        public bool doorPos()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 636, gameScreenCoords[1] + 53, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0 && bitmap.GetPixel(i, j).R <= 250)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void screenshotDrop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(480, 12))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 6, gameScreenCoords[1] + 426, 0, 0, new Size(480, 12));
                }
                bitmap.Save("Z:/screenshots/kill" + kills + ".png", System.Drawing.Imaging.ImageFormat.Png);
                dropanal.analyse(bitmap);
            }
        }

        public bool lured()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 616, gameScreenCoords[1] + 105, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 254)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool lurer()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 608, gameScreenCoords[1] + 71, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 160)
                        {
                            if (bitmap.GetPixel(i, j).B >= 170 && bitmap.GetPixel(i, j).G == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;

            }
        }

        public async void exitRoom()
        {
            while(!doorPos())
            {
                await Task.Delay(10);
            }
            Cursor.Position = new Point(gameScreenCoords[0] + 345, gameScreenCoords[1] + 172);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            contFletch = true;
            enterDoor();
        }

        public bool lastCharge()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(180, 250))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 550, gameScreenCoords[1] + 208, 0, 0, new Size(180, 250));
                }

                for (int i = 0; i < 180; i++)
                {
                    for (int j = 0; j < 250; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void enterDoor()
        {
            if (lured() && lurer())
            {
                if(firstKill)
                {
                    if (!enablePanic)
                    {
                        panicTele();
                    }
                    storedHp = player.getHp();
                    Cursor.Position = new Point(gameScreenCoords[0] + 256, gameScreenCoords[1] + 213);
                    await Task.Delay(1600);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    att = true;
                    wait = false;
                    contFletch = false;
                    specd = false;
                    walkToSpot();
                } else if (flincherOut())
                {
                    storedHp = player.getHp();
                    Cursor.Position = new Point(gameScreenCoords[0] + 256, gameScreenCoords[1] + 213);
                    await Task.Delay(150);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    att = true;
                    wait = false;
                    contFletch = false;
                    specd = false;
                    walkToSpot();
                } else
                {
                    if (player.getHp() < 80)
                    {
                        if (inv.hasItem("Shark"))
                        {
                            inv.clickInventory("Shark");
                            await Task.Delay(600);
                            enterDoor();
                        }
                        else
                        {
                            bank();
                            return;
                        }
                    }
                    else
                    {
                        await Task.Delay(100);
                        doFletch();
                        enterDoor();
                    }
                }
            } else
            {
                if (player.getHp() < 80)
                {
                    if (inv.hasItem("Shark"))
                    {
                        inv.clickInventory("Shark");
                        await Task.Delay(1800);
                        enterDoor();
                    }
                    else
                    {
                        bank();
                        return;
                    }
                } else {
                    await Task.Delay(600);
                    doFletch();
                    enterDoor();
                }
            }
        }

        public bool flincherOut()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(6, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 628, gameScreenCoords[1] + 78, 0, 0, new Size(6, 6));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 160 && bitmap.GetPixel(i, j).B >= 180)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void walkToSpot()
        {
            if (storedHp > player.getHp())
            {
                await Task.Delay(10000);
                teleToCorp();
                return;
            }
            firstKill = true;
            if (inRoom())
            {
                if(inv.hasItem(potion))
                {
                    inv.clickInventory(potion);
                }
                await Task.Delay(1000);
                Cursor.Position = new Point(gameScreenCoords[0] + 146, gameScreenCoords[1] + 325);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                waitForFlincher();
            } else
            {
                await Task.Delay(600);
                walkToSpot();
            }
        }

        public bool inRoom()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 620, gameScreenCoords[1] + 37, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void waitForFlincher()
        {
            if (storedHp > player.getHp())
            {
                await Task.Delay(10000);
                teleToCorp();
                return;
            }
            if (flincher())
            {
                startScript();
            } else
            {
                await Task.Delay(600);
                waitForFlincher();
            }
        }

        public bool flincher()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 609, gameScreenCoords[1] + 77, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 160)
                        {
                            if (bitmap.GetPixel(i, j).B >= 170 && bitmap.GetPixel(i, j).G == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;

            }
        }

        public bool checkSpec()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 553, gameScreenCoords[1] + 145, 0, 0, new Size(20, 20));
               }
            
                for (int i = 0; i < 20; i++)
               {
                    for (int j = 0; j < 20; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 255)
                      {
                            if (bitmap.GetPixel(i, j).R <= 255)
                            {
                                return true;
                            }
                        }
                    }
               }
                return false;

            }
        }

        public bool checkNPC()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 632, gameScreenCoords[1] + 66, 0, 0, new Size(10, 10));
                }

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 254)
                        {
                            return true;
                        }
                    }
                }
                kills++;
                return false;

            }
        }

        public bool moved()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 614, gameScreenCoords[1] + 80, 0, 0, new Size(2, 2));
                }
                checkMovement = bitmap.GetPixel(1, 1);
            }

            while (attack)
            {

                using (Bitmap bitmap = new Bitmap(2, 2))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 614, gameScreenCoords[1] + 80, 0, 0, new Size(2, 2));
                    }
                    if (bitmap.GetPixel(1, 1) != checkMovement)
                    {
                        attack = false;
                        return true;
                    }
                }

            }
            return true;

        }



        public void setCoords(int x, int y)
        {
            inv.setCoords(x, y);
            banking.setCoords(x,y);
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            attackCoords[0] = x + 234;
            attackCoords[1] = y + 80; 
            safespot[1] = y + 175;
            chatbox[0] = x + 4;
            chatbox[1] = y + 341;
            player.setCoords(gameScreenCoords[0], gameScreenCoords[1]);
            mouse.setCoords(x, y);
        }

        public int getKills()
        {
            return kills;
        }

        public int getLevels()
        {
            return levelUps;
        }



        public async void bank()
        {
            inv.rubAmulet("Games nec");
                await Task.Delay(20);
                tele(2);
        }

        public async void tele(int option)
        {
            if (hasChatOptions())
            {
                if (option == 2)
                {
                    await Task.Delay(100);
                    Peripherals.PressKey((byte)Keys.NumPad2, 1);
                    await Task.Delay(5000);
                    walkToBank();
                }
                if (option == 3)
                {
                    await Task.Delay(100);
                    Peripherals.PressKey((byte)Keys.NumPad3, 1);
                    await Task.Delay(5000);
                    goToPortal();
                }
            }
            else
            {
                await Task.Delay(300);
                tele(option);
            }

        }

        public async void walkToBank()
        {
            if (atBarbAssault())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 608 + bankWalkx, gameScreenCoords[1] + 109 + bankWalky); ;
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 203, gameScreenCoords[1] + 96, 0, 0);
                await Task.Delay(20);
                mouse_event(MOUSEEVENTF_LEFTUP, gameScreenCoords[0] + 203, gameScreenCoords[1] + 96, 0, 0);
                await Task.Delay(20);
                clickBank();
            }
            else
            {
                await Task.Delay(300);
                walkToBank();
            }
        }

        public async void clickBank()
        {
                if (seeBank())
                {
                    await Task.Delay(600);
                    Cursor.Position = new Point(gameScreenCoords[0] + 284, gameScreenCoords[1] + 191);
                    await Task.Delay(600);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 203, gameScreenCoords[1] + 96, 0, 0);
                    await Task.Delay(20);
                    mouse_event(MOUSEEVENTF_LEFTUP, gameScreenCoords[0] + 203, gameScreenCoords[1] + 96, 0, 0);
                    await Task.Delay(20);
                    doBanking();
                }
                else
                {
                    await Task.Delay(300);
                    clickBank();
                }
        }

        public bool seeBank()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(8, 8))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 633, gameScreenCoords[1] + 138, 0, 0, new Size(8, 8));
                }

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 225  && bitmap.GetPixel(i, j).B == 0)
                        {
                            if (bitmap.GetPixel(i + 1, j).R >= 225 && bitmap.GetPixel(i + 1, j).G >= 225 && bitmap.GetPixel(i + 1, j).B >= 225)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public async void doBanking()
        {
            if (bankOpen())
            {
                banking.setCoords(gameScreenCoords[0], gameScreenCoords[1]);
                banking.depositAllInventory();
                await Task.Delay(300);
                if (potion != "Nothing")
                {
                    if (banking.hasItem(potion))
                    {
                        banking.busy = true;
                        banking.withdrawItem(potion, 5);
                    } 
                    else
                    {
                        switchPotion();
                        await Task.Delay(600);
                        if (potion != "Nothing")
                        {
                            banking.busy = true;
                            banking.withdrawItem(potion, 5);
                        }
                    }
                }
                while(banking.busy)
                {
                    await Task.Delay(10);
                }
                banking.busy = true;
                banking.withdrawItem("Shark", 5);
                while (banking.busy)
                {
                    await Task.Delay(10);
                }
                banking.busy = true;
                banking.withdrawItem("Games nec", 1);
                while (banking.busy)
                {
                    await Task.Delay(10);
                }
                if (specialWeapon == "Dragon Warhammer")
                {
                    banking.busy = true;
                    banking.withdrawItem("Dragon Warhammer", 1);
                    while (banking.busy)
                    {
                        await Task.Delay(10);
                    }
                }
                banking.busy = true;
                banking.withdrawItem("Panic tab", 5);
                while (banking.busy)
                {
                    await Task.Delay(10);
                }
                Peripherals.PressKey((byte)Keys.Escape, 1);
                await Task.Delay(600);
                teleToCorp();
            }
            else
            {
                await Task.Delay(600);
                doBanking();
            }
        }

        public async void teleToCorp()
        {
            if (!inv.hasItem("Panic tab"))
            {
                bank();
            }
            else
            {
                inv.rubAmulet("Games nec");
                await Task.Delay(30);
                tele(3);
            }
        }

        public bool bankOpen()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 50, gameScreenCoords[1] + 9, 0, 0, new Size(20, 20));
                }

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 152 && bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).B == 31)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async void goToPortal()
        {
            if (atCorpLair())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + portalx, gameScreenCoords[1] + portaly);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                walkToStartingSpot();
            }
            else
            {
                await Task.Delay(600);
                goToPortal();
            }
        }

        public async void walkToStartingSpot()
        {
                if (enterTextbox())
                {
                    await Task.Delay(2000);
                    Cursor.Position = new Point(gameScreenCoords[0] + 414, gameScreenCoords[1] + 271);
                    await Task.Delay(2000);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    enterDoor();
                }  
                else
                {
                    await Task.Delay(600);
                    walkToStartingSpot();
                }
        }
        public bool seeAccounts()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(67, 64))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 611, gameScreenCoords[1] + 79, 0, 0, new Size(67, 64));
                }

                for (int i = 0; i < 66; i++)
                {
                    for (int j = 0; j < 63; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 160 && bitmap.GetPixel(i, j).B >= 180)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool hasChatOptions()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(50, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 132, gameScreenCoords[1] + 359, 0, 0, new Size(50, 20));
                }

                for (int i = 0; i < 49; i++)
                {
                    for (int j = 0; j < 19; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 120 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool atPortal1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 648, gameScreenCoords[1] + 60, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool atPortal2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 648, gameScreenCoords[1] + 64, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool atBarbAssault()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(110, 100))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 608, gameScreenCoords[1] + 109, 0, 0, new Size(110, 100));
                }

                for (int i = 0; i < 93; i++)
                {
                    for (int j = 0; j < 65; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 203 && bitmap.GetPixel(i, j).G == 82 && bitmap.GetPixel(i, j).B == 81)
                        {
                            bankWalkx = i + 18;
                            bankWalky = j + 14;
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool atCorpLair()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(726, 158))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(726, 158));
                }

                if (bitmap.GetPixel(617, 61).R <= 254 && bitmap.GetPixel(617, 61).R >= 225 && bitmap.GetPixel(617, 61).G == 0 && bitmap.GetPixel(617, 61).B == 0)
                {
                    portalx = 88;
                    portaly = 114;
                    return true;
                }
                if (bitmap.GetPixel(617, 57).R <= 254 && bitmap.GetPixel(617, 57).R >= 225 && bitmap.GetPixel(617, 57).G == 0 && bitmap.GetPixel(617, 57).B == 0)
                {
                    portalx = 84;
                    portaly = 127;
                    return true;
                }
                if (bitmap.GetPixel(621, 53).R <= 254 && bitmap.GetPixel(621, 53).R >= 225 && bitmap.GetPixel(621, 53).G == 0 && bitmap.GetPixel(621, 53).B == 0)
                {
                    portalx = 107;
                    portaly = 94;
                    return true;
                }
                if (bitmap.GetPixel(621, 57).R <= 254 && bitmap.GetPixel(621, 57).R >= 225 && bitmap.GetPixel(621, 57).G == 0 && bitmap.GetPixel(621, 57).B == 0)
                {
                    portalx = 105;
                    portaly = 108;
                    return true;
                }
                if (bitmap.GetPixel(621, 61).R <= 254 && bitmap.GetPixel(621, 61).R >= 225 && bitmap.GetPixel(621, 61).G == 0 && bitmap.GetPixel(621, 61).B == 0)
                {
                    portalx = 101;
                    portaly = 128;
                    return true;
                }
                if (bitmap.GetPixel(621, 65).R <= 254 && bitmap.GetPixel(621, 65).R >= 225 && bitmap.GetPixel(621, 65).G == 0 && bitmap.GetPixel(621, 65).B == 0)
                {
                    portalx = 98;
                    portaly = 146;
                    return true;
                }
                if (bitmap.GetPixel(625, 53).R <= 254 && bitmap.GetPixel(625, 53).R >= 225 && bitmap.GetPixel(625, 53).G == 0 && bitmap.GetPixel(625, 53).B == 0)
                {
                    portalx = 129;
                    portaly = 95;
                    return true;
                }
                if (bitmap.GetPixel(625, 57).R <= 254 && bitmap.GetPixel(625, 57).R >= 225 && bitmap.GetPixel(625, 57).G == 0 && bitmap.GetPixel(625, 57).B == 0)
                {
                    portalx = 126;
                    portaly = 109;
                    return true;
                }
                if (bitmap.GetPixel(625, 61).R <= 254 && bitmap.GetPixel(625, 61).R >= 225 && bitmap.GetPixel(625, 61).G == 0 && bitmap.GetPixel(625, 61).B == 0)
                {
                    portalx = 123;
                    portaly = 127;
                    return true;
                }
                if (bitmap.GetPixel(625, 65).R <= 254 && bitmap.GetPixel(625, 65).R >= 225 && bitmap.GetPixel(625, 65).G == 0 && bitmap.GetPixel(625, 65).B == 0)
                {
                    portalx = 120;
                    portaly = 146;
                    return true;
                }
                if (bitmap.GetPixel(625, 69).R <= 254 && bitmap.GetPixel(625, 69).R >= 225 && bitmap.GetPixel(625, 69).G == 0 && bitmap.GetPixel(625, 69).B == 0)
                {
                    portalx = 115;
                    portaly = 168;
                    return true;
                }
                if (bitmap.GetPixel(629, 53).R <= 254 && bitmap.GetPixel(629, 53).R >= 225 && bitmap.GetPixel(629, 53).G == 0 && bitmap.GetPixel(629, 53).B == 0)
                {
                    portalx = 149;
                    portaly = 94;
                    return true;
                }
                if (bitmap.GetPixel(629, 57).R <= 254 && bitmap.GetPixel(629, 57).R >= 225 && bitmap.GetPixel(629, 57).G == 0 && bitmap.GetPixel(629, 57).B == 0)
                {
                    portalx = 149;
                    portaly = 109;
                    return true;
                }
                if (bitmap.GetPixel(629, 61).R <= 254 && bitmap.GetPixel(629, 61).R >= 225 && bitmap.GetPixel(629, 61).G == 0 && bitmap.GetPixel(629, 61).B == 0)
                {
                    portalx = 149;
                    portaly = 127;
                    return true;
                }
                if (bitmap.GetPixel(629, 65).R <= 254 && bitmap.GetPixel(629, 65).R >= 225 && bitmap.GetPixel(629, 65).G == 0 && bitmap.GetPixel(629, 65).B == 0)
                {
                    portalx = 149;
                    portaly = 146;
                    return true;
                }
                if (bitmap.GetPixel(629, 69).R <= 254 && bitmap.GetPixel(629, 69).R >= 225 && bitmap.GetPixel(629, 69).G == 0 && bitmap.GetPixel(629, 69).B == 0)
                {
                    portalx = 149;
                    portaly = 168;
                    return true;
                }
                if (bitmap.GetPixel(633, 53).R <= 254 && bitmap.GetPixel(633, 53).R >= 225 && bitmap.GetPixel(633, 53).G == 0 && bitmap.GetPixel(633, 53).B == 0)
                {
                    portalx = 170;
                    portaly = 97;
                    return true;
                }
                if (bitmap.GetPixel(633, 57).R <= 254 && bitmap.GetPixel(633, 57).R >= 225 && bitmap.GetPixel(633, 57).G == 0 && bitmap.GetPixel(633, 57).B == 0)
                {
                    portalx = 170;
                    portaly = 108;
                    return true;
                }
                if (bitmap.GetPixel(633, 61).R <= 254 && bitmap.GetPixel(633, 61).R >= 225 && bitmap.GetPixel(633, 61).G == 0 && bitmap.GetPixel(633, 61).B == 0)
                {
                    portalx = 168;
                    portaly = 129;
                    return true;
                }
                if (bitmap.GetPixel(633, 65).R <= 254 && bitmap.GetPixel(633, 65).R >= 225 && bitmap.GetPixel(633, 65).G == 0 && bitmap.GetPixel(633, 65).B == 0)
                {
                    portalx = 164;
                    portaly = 148;
                    return true;
                }
                if (bitmap.GetPixel(633, 69).R <= 254 && bitmap.GetPixel(633, 69).R >= 225 && bitmap.GetPixel(633, 69).G == 0 && bitmap.GetPixel(633, 69).B == 0)
                {
                    portalx = 162;
                    portaly = 168;
                    return true;
                }
                if (bitmap.GetPixel(633, 73).R <= 254 && bitmap.GetPixel(633, 73).R >= 225 && bitmap.GetPixel(633, 73).G == 0 && bitmap.GetPixel(633, 73).B == 0)
                {
                    portalx = 159;
                    portaly = 189;
                    return true;
                }
                if (bitmap.GetPixel(637, 53).R <= 254 && bitmap.GetPixel(637, 53).R >= 225 && bitmap.GetPixel(637, 53).G == 0 && bitmap.GetPixel(637, 53).B == 0)
                {
                    portalx = 192;
                    portaly = 94;
                    return true;
                }
                if (bitmap.GetPixel(637, 57).R <= 254 && bitmap.GetPixel(637, 57).R >= 225 && bitmap.GetPixel(637, 57).G == 0 && bitmap.GetPixel(637, 57).B == 0)
                {
                    portalx = 190;
                    portaly = 109;
                    return true;
                }
                if (bitmap.GetPixel(637, 61).R <= 254 && bitmap.GetPixel(637, 61).R >= 225 && bitmap.GetPixel(637, 61).G == 0 && bitmap.GetPixel(637, 61).B == 0)
                {
                    portalx = 190;
                    portaly = 127;
                    return true;
                }
                if (bitmap.GetPixel(637, 65).R <= 254 && bitmap.GetPixel(637, 65).R >= 225 && bitmap.GetPixel(637, 65).G == 0 && bitmap.GetPixel(637, 65).B == 0)
                {
                    portalx = 194;
                    portaly = 147;
                    return true;
                }
                if (bitmap.GetPixel(637, 69).R <= 254 && bitmap.GetPixel(637, 69).R >= 225 && bitmap.GetPixel(637, 69).G == 0 && bitmap.GetPixel(637, 69).B == 0)
                {
                    portalx = 190;
                    portaly = 168;
                    return true;
                }
                if (bitmap.GetPixel(637, 73).R <= 254 && bitmap.GetPixel(637, 73).R >= 225 && bitmap.GetPixel(637, 73).G == 0 && bitmap.GetPixel(637, 73).B == 0)
                {
                    portalx = 190;
                    portaly = 190;
                    return true;
                }
                if (bitmap.GetPixel(637, 77).R <= 254 && bitmap.GetPixel(637, 77).R >= 225 && bitmap.GetPixel(637, 77).G == 0 && bitmap.GetPixel(637, 77).B == 0)
                {
                    portalx = 185;
                    portaly = 213;
                    return true;
                }
                if (bitmap.GetPixel(641, 56).R <= 254 && bitmap.GetPixel(641, 56).R >= 225 && bitmap.GetPixel(641, 56).G == 0 && bitmap.GetPixel(641, 56).B == 0)
                {
                    portalx = 211;
                    portaly = 112;
                    return true;
                }
                if (bitmap.GetPixel(641, 60).R <= 254 && bitmap.GetPixel(641, 60).R >= 225 && bitmap.GetPixel(641, 60).G == 0 && bitmap.GetPixel(641, 60).B == 0)
                {
                    portalx = 210;
                    portaly = 128;
                    return true;
                }
                if (bitmap.GetPixel(641, 64).R <= 254 && bitmap.GetPixel(641, 64).R >= 225 && bitmap.GetPixel(641, 64).G == 0 && bitmap.GetPixel(641, 64).B == 0)
                {
                    portalx = 210;
                    portaly = 146;
                    return true;
                }
                if (bitmap.GetPixel(641, 68).R <= 254 && bitmap.GetPixel(641, 68).R >= 225 && bitmap.GetPixel(641, 68).G == 0 && bitmap.GetPixel(641, 68).B == 0)
                {
                    portalx = 209;
                    portaly = 167;
                    return true;
                }
                if (bitmap.GetPixel(641, 72).R <= 254 && bitmap.GetPixel(641, 72).R >= 225 && bitmap.GetPixel(641, 72).G == 0 && bitmap.GetPixel(641, 72).B == 0)
                {
                    portalx = 207;
                    portaly = 189;
                    return true;
                }
                if (bitmap.GetPixel(645, 60).R <= 254 && bitmap.GetPixel(645, 60).R >= 225 && bitmap.GetPixel(645, 60).G == 0 && bitmap.GetPixel(645, 60).B == 0)
                {
                    portalx = 232;
                    portaly = 132;
                    return true;
                }
                if (bitmap.GetPixel(645, 64).R <= 254 && bitmap.GetPixel(645, 64).R >= 225 && bitmap.GetPixel(645, 64).G == 0 && bitmap.GetPixel(645, 64).B == 0)
                {
                    portalx = 231;
                    portaly = 147;
                    return true;
                }
                if (bitmap.GetPixel(645, 68).R <= 254 && bitmap.GetPixel(645, 68).R >= 225 && bitmap.GetPixel(645, 68).G == 0 && bitmap.GetPixel(645, 68).B == 0)
                {
                    portalx = 231;
                    portaly = 167;
                    return true;
                }
                if (bitmap.GetPixel(645, 72).R <= 254 && bitmap.GetPixel(645, 72).R >= 225 && bitmap.GetPixel(645, 72).G == 0 && bitmap.GetPixel(645, 72).B == 0)
                {
                    portalx = 231;
                    portaly = 188;
                    return true;
                }
                if (bitmap.GetPixel(649, 60).R <= 254 && bitmap.GetPixel(649, 60).R >= 225 && bitmap.GetPixel(649, 60).G == 0 && bitmap.GetPixel(649, 60).B == 0)
                {
                    portalx = 254;
                    portaly = 133;
                    return true;
                }
                if (bitmap.GetPixel(649, 64).R <= 254 && bitmap.GetPixel(649, 64).R >= 225 && bitmap.GetPixel(649, 64).G == 0 && bitmap.GetPixel(649, 64).B == 0)
                {
                    portalx = 254;
                    portaly = 146;
                    return true;
                }

                return false;

            }
        }

    }
}
