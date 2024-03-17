using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{

    internal class CorpLurer
    {


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        public int kills;
        public int check;
        public Inventory inv = new Inventory();
        public Player player = new Player();
        Banking banking = new Banking();
        Peripherals mouse = new Peripherals();

        public int[] gameScreenCoords = new int[2] { 0, 0 };
        public int[] attackCoords = new int[2];
        public int[] safespot = new int[2];

        public bool att = false;
        public int wildLureAttempt = 0;
        public int bankx = 0;
        public int banky = 0;
        public int bankType;
        public bool attack;
        public bool spec;
        public int bankWalkx;
        public int bankWalky;
        public bool setBank = false;

        public bool pass1;
        public bool pass2;
        public bool pass3;

        public bool specd = false;


        public int portalx;
        public int portaly;

        public bool sixHourTimer = false;

        public int killState = 0; //0 wild, 1 lure, 2 lured, 3 idk

        public string specialWeapon;

        public float hpStored = 0;

        public string potion = "Super Combat Potion";

        public void switchPotion()
        {
            switch (potion)
            {
                case "Super Combat Potion":
                    potion = "Super Attack Potion";
                    break;
                case "Super Attack Potion":
                    potion = "Nothing";
                    break;
            }
        }

        public async void cameraPos()
        {
            att = true;
            attack = true;
            Cursor.Position = new Point(gameScreenCoords[0] + 558, gameScreenCoords[1] + 16);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_RIGHTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(100);
            Cursor.Position = new Point(gameScreenCoords[0] + 556, gameScreenCoords[1] + 59);
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
            Cursor.Position = new Point(gameScreenCoords[0] + 628, gameScreenCoords[1] + 314);
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
            waitForAccounts();
        }

        public void setSpecWep()
        {
            if (inv.hasItem("Dragon Warhammer"))
            {
                specialWeapon = "Dragon Warhammer";
                spec = true;
            }
            else
            {
                spec = false;
            }
        }

        public void setPotion()
        {
            if (inv.hasItem("Super Combat Potion"))
            {
                potion = "Super Combat Potion";
            }
            else if (inv.hasItem("Super Attack Potion"))
            {
                potion = "Super Attack Potion";
            }
            else
            {
                potion = "Nothing";
            }
        }

        public async void logout()
        {
            sixHourTimer = false;
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
            if(atLoginScreen())
            {
                if(pass1) {
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
            } else
            {
                await Task.Delay(600);
                login();
            }
        }

        public async void clickPlay()
        {
            if(!atLoginScreen())
            {
                await Task.Delay(600);
                Cursor.Position = new Point(gameScreenCoords[0] + 411, gameScreenCoords[1] + 336);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                lurerEnterPortal();
            } else
            {
                await Task.Delay(600);
                clickPlay();
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
            Cursor.Position = new Point(gameScreenCoords[0] + 556, gameScreenCoords[1] + 59);
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
            Cursor.Position = new Point(gameScreenCoords[0] + 628, gameScreenCoords[1] + 314);
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
            Cursor.Position = new Point(gameScreenCoords[0] + 280, gameScreenCoords[1] + 173);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            moveToBlockerStart();
        }

        public async void moveToBlockerStart()
        {
            if(enterTextbox())
            {
                await Task.Delay(2000);
                Cursor.Position = new Point(gameScreenCoords[0] + 280, gameScreenCoords[1] + 135);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                waitForAccounts();
            } else
            {
                await Task.Delay(600);
                moveToBlockerStart();
            }
        }

        public async void startScript()
        {
            if(player.getHp() < hpStored || !blockerIn() || !flincherIn())
            {
                if (checkNPC())
                {
                    quickExit();
                    return;
                }
            }
            if(relogNow())
            {
                sixHourTimer = true;
            }
            if (!checkNPC())
            {
                if (player.getHp() < 80)
                {
                    if (inv.hasItem("Shark") && inv.hasItem(potion))
                    {
                        inv.clickInventory("Shark");
                        await Task.Delay(1200);
                    } else
                    {
                        killState = 3;
                        bank();
                        return;
                    }
                }
                else
                {
                    if (player.getPrayer() <= 50)
                    {
                        inv.clickInventory("Prayer Potion");
                    }
                    await Task.Delay(600);
                    if (gotDrop())
                    {
                        pickUpDrop();
                        return;
                    }
                    else
                    {
                        headToLureSpot();
                        return;
                    }
                }
            }
            if (levelup())
            {
                att = true;
                attack = true;
            }
            if (corpHealthOverHalf())
            {
                if (spec)
                {
                    if (checkSpec())
                    {
                        if (!specd)
                        {
                            inv.equipItem(specialWeapon);
                            await Task.Delay(600);
                            doSpecs();
                            return;
                        }
                    }
                    attack = true;
                }
            }
            await Task.Delay(100);
            if (moved())
            {
                if (att)
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 283, gameScreenCoords[1] + 83);
                    await Task.Delay(1000);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    att = false;
                }
            }
            startScript();

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
        public async void doSpecs()
        {
            att = true;
            Cursor.Position = new Point(gameScreenCoords[0] + 584, gameScreenCoords[1] + 145);
            await Task.Delay(200);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            attack = true;
            await Task.Delay(100);
            waitForFlinch();
        }

        public async void waitForFlinch()
        {
            if (moved())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 277, gameScreenCoords[1] + 93);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                Cursor.Position = new Point(gameScreenCoords[0] + 638, gameScreenCoords[1] + 81);
                await Task.Delay(2000);
                mouse_event(MOUSEEVENTF_LEFTDOWN, safespot[0], safespot[1], 0, 0);
                if (checkSpec())
                {
                    doSpecs();
                    return;
                }
                await Task.Delay(200);
                mouse.leftClick(inv.getPreviousClick("x"), inv.getPreviousClick("y"));
                attack = true;
                specd = true;
                startScript();
            }
            else
            {
                waitForFlinch();
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

        public bool levelup()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(500, 130))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 4, gameScreenCoords[1] + 342, 0, 0, new Size(500, 130));
                }

                for (int i = 0; i < 499; i++)
                {
                    for (int j = 0; j < 129; j++)
                    {
                        if (bitmap.GetPixel(i, j).B == 225 && bitmap.GetPixel(i, j).G == 0)
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
                    g.CopyFromScreen(gameScreenCoords[0] + 643, gameScreenCoords[1] + 71, 0, 0, new Size(4, 4));
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

        public bool leaveTextbox()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 147, gameScreenCoords[1] + 359, 0, 0, new Size(10, 10));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
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

        public async void pickUpDrop()
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 299, gameScreenCoords[1] + 132);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 292, gameScreenCoords[1] + 137, 0, 0);
            await Task.Delay(5000);
            headToLureSpot2();
        }

        public bool NPConMap()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(102, 63))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 610, gameScreenCoords[1] + 6, 0, 0, new Size(102, 63));
                }

                for (int i = 0; i < 101; i++)
                {
                    for (int j = 0; j < 62; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 254 && bitmap.GetPixel(i, j).R >= 254)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }
        public bool NPClured()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 629, gameScreenCoords[1] + 43, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 253 && bitmap.GetPixel(i, j).R >= 253)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool NPCatEnterance()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4,4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 627, gameScreenCoords[1] + 54, 0, 0, new Size(4,4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 253 && bitmap.GetPixel(i, j).R >= 253)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool NPCspawned()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(111, 8))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 584, gameScreenCoords[1] + 5, 0, 0, new Size(111, 8));
                }

                for (int i = 0; i < 110; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 254 && bitmap.GetPixel(i, j).R >= 254)
                        {
                            return true;
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
                    g.CopyFromScreen(gameScreenCoords[0] + 639, gameScreenCoords[1] + 66, 0, 0, new Size(10, 10));
                }

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 254 && bitmap.GetPixel(i, j).R >= 254)
                        {
                            return true;
                        }
                    }
                }
                kills++;
                return false;

            }
        }

        public async void waitForSpawn()
        {
            if(NPConMap())
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                await Task.Delay(600);
                att = true;
                specd = false;
                walkToCorner();
            } else
            {
                await Task.Delay(10);
                waitForSpawn();
            }
        }

        public async void walkToCorner()
        {
            if(doorPos2())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 330, gameScreenCoords[1] + 207);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(100);
                Cursor.Position = new Point(gameScreenCoords[0] + 553, gameScreenCoords[1] + 89);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 55, gameScreenCoords[1] + 8, 0, 0);
                await Task.Delay(200);
                check = 0;
                wildLureAttempt = 0;
                waitForLure();
            } else
            {
                await Task.Delay(600);
                walkToCorner();
            }
        }

        public async void waitForLure()
        {
            if (check <= 10)
            {
                if (NPCspawned())
                    {
                       waitForLure3();
                    }
                    else
                    {
                        await Task.Delay(600);
                        check++;
                        waitForLure();
                    }
            } else
            {
                    lureWild(1);
            }
        }

        public async void waitForLure3()
        {
            if (check <= 25)
            {
                if (corpLured2())
                {
                    waitForAccounts();
                }
                else
                {
                    await Task.Delay(600);
                    check++;
                    waitForLure3();
                }
            }
            else
            {
                lureWild(1);
            }
        }

        public async void waitForLure2()
        {
            if (check <= 15)
            {
                if (NPClured())
                {
                    waitForAccounts();
                }
                else if (corpFailLure() && atLureSpot2())
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 259, gameScreenCoords[1] + 216);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(100);
                    relureExit();
                }
                else
                {
                    await Task.Delay(600);
                    check++;
                    waitForLure2();
                }
            }
            else
            {
                if (wildLureAttempt < 4)
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 196, gameScreenCoords[1] + 136);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(100);
                    waitForNonWildNpc();
                }
                else
                {
                    MessageBox.Show("Out of attempts. " + wildLureAttempt.ToString());
                    return;
                }
            }
        }

        public async void relureExit()
        {
            if(leaveTextbox())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 254, gameScreenCoords[1] + 399);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(3000); // change instance to normal room
                Cursor.Position = new Point(gameScreenCoords[0] + 279, gameScreenCoords[1] + 171);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(4000);
                Cursor.Position = new Point(gameScreenCoords[0] + 222, gameScreenCoords[1] + 102);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                waitForNonWildNpc();
            } else
            {
                await Task.Delay(500);
                relureExit();
            }
        }

        public async void lureWild(int x)
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 196, gameScreenCoords[1] + 136);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            if(x == 1)
            {
                await Task.Delay(3000);
                wildLureAttempt = 0;
                waitForWildNpc();
            }
            if(x == 2)
            {
                waitForNonWildNpc();
            }
        }

        public async void checkAtDoor()
        {
            if (atDoor())
            {
                waitForWildNpc();
            }
            else
            {
                await Task.Delay(600);
                checkAtDoor();
            }
        }

        public async void waitForWildNpc()
        {
            if (player.getHp() <= 80)
            {
                if (inv.hasItem("Shark"))
                {
                    inv.clickInventory("Shark");
                    await Task.Delay(1200);
                    waitForWildNpc();
                } else
                {
                    killState = 0;
                    bank();
                    return;
                }
            }
            else
            {
                if (NPConMap())
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 553, gameScreenCoords[1] + 89);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 55, gameScreenCoords[1] + 8, 0, 0);
                    await Task.Delay(200);
                    Cursor.Position = new Point(gameScreenCoords[0] + 260, gameScreenCoords[1] + 115);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    clickDoorWildIn();
                }
                else
                {
                    await Task.Delay(600);
                    waitForWildNpc();
                }
            }
        }

        public async void clickDoorWildIn()
        {
            if(moved2())
            {
                await Task.Delay(600);
                Cursor.Position = new Point(gameScreenCoords[0] + 259, gameScreenCoords[1] + 197);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 55, gameScreenCoords[1] + 8, 0, 0);
                await Task.Delay(1200);
                Cursor.Position = new Point(gameScreenCoords[0] + 553, gameScreenCoords[1] + 89);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 55, gameScreenCoords[1] + 8, 0, 0);
                await Task.Delay(200);
                check = 0;
                checkWildLure();
            } else
            {
                await Task.Delay(50);
                clickDoorWildIn();
            }
        }

        public async void checkWildLure()
        {
            if (NPCatEnterance())
            {
                exitRoom();
            } else
            {
                    if (check <= 15)
                    {
                        if (player.getHp() <= 80)
                        {
                            if (inv.hasItem("Shark"))
                            {
                                inv.clickInventory("Shark");
                            } else
                            {
                                killState = 0;
                                bank();
                                return;
                            }
                        }
                        check++;
                        await Task.Delay(600);
                        checkWildLure();
                    }
                    else
                    {
                        waitForWildNpc();
                    }
            }
        }

        public async void exitRoom()
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 324, gameScreenCoords[1] + 254);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(100);
            Cursor.Position = new Point(gameScreenCoords[0] + 256, gameScreenCoords[1] + 399);
            waitForOptions();
        }

        public async void waitForOptions()
        {
            if (leaveTextbox())
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(3000); // change instance to normal room
                Cursor.Position = new Point(gameScreenCoords[0] + 279, gameScreenCoords[1] + 171);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(4000);
                Cursor.Position = new Point(gameScreenCoords[0] + 222, gameScreenCoords[1] + 102);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                waitForNonWildNpc();
            } else
            {
                await Task.Delay(600);
                waitForOptions();
            }
        }
        public async void waitForNonWildNpc()
        {
            if(atDoor())
            {
                idk1();
            } else
            {
                await Task.Delay(600);
                waitForNonWildNpc();
            }
        }

        public async void idk1()
        {
            if (player.getHp() <= 80)
            {
                if (inv.hasItem("Shark"))
                {
                    inv.clickInventory("Shark");
                } else
                {
                    killState = 1;
                    bank();
                    return;
                }
                await Task.Delay(600);
                idk1();
            } else
            {
                if (corpLurable() || corpLurable2())
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 553, gameScreenCoords[1] + 89);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 55, gameScreenCoords[1] + 8, 0, 0);
                    await Task.Delay(200);
                    Cursor.Position = new Point(gameScreenCoords[0] + 260, gameScreenCoords[1] + 115);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    clickDoorWildIn2();
                }
                else
                {
                    await Task.Delay(600);
                    idk1();
                }
            }
        }

        public async void clickDoorWildIn2()
        {
            if (moved2())
            {
                await Task.Delay(100);
                Cursor.Position = new Point(gameScreenCoords[0] + 259, gameScreenCoords[1] + 197);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 55, gameScreenCoords[1] + 8, 0, 0);
                await Task.Delay(100);
                check = 0;
                wildLure();
            }
            else
            {
                if (check <= 22)
                {
                    await Task.Delay(100);
                    clickDoorWildIn2();
                }
                else
                {
                    lureWild(2);
                }
            }
        }

        public async void wildLure()
        {
            if(atDoor())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 332, gameScreenCoords[1] + 210);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                Cursor.Position = new Point(gameScreenCoords[0] + 553, gameScreenCoords[1] + 89);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 55, gameScreenCoords[1] + 8, 0, 0);
                await Task.Delay(200);
                check = 0;
                waitForLure2();
            } else
            {
                await Task.Delay(600);
                wildLure();
            }
        }

        public async void waitForAccounts()
        {
                if (!blockerOut() && !flincherOut())
                {
                    waitForAccounts1();
                }
                else
                {
                    await Task.Delay(600);
                    waitForAccounts();
                }
        }

        public async void waitForAccounts1()
        {
            if (blocker() && flincher())
            {
                    inv.clickInventory(potion);
                    hpStored = player.getHp();
                    waitForKillStart();
            }
            else if((blockerOut() || flincherOut()) && !somebodyInsde())
            {
                lureWild(1);
            } else
            {
                if (player.getHp() < 80)
                {
                    if (inv.hasItem("Shark"))
                    {
                        inv.clickInventory("Shark");
                    }
                }
                    check++;
                await Task.Delay(600);
                waitForAccounts1();
            }
        }

        public bool moved2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 638, gameScreenCoords[1] + 122, 0, 0, new Size(3, 3));
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

        public bool blockerOut()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(6, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 608, gameScreenCoords[1] + 71, 0, 0, new Size(6, 6));
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

        public bool blockerIn()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(16, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 632, gameScreenCoords[1] + 53, 0, 0, new Size(16, 10));
                }

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 10; j++)
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

        public bool flincherIn()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(16, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 657, gameScreenCoords[1] + 54, 0, 0, new Size(16, 10));
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 10; j++)
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

        public bool flincherOut()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(6, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 616, gameScreenCoords[1] + 71, 0, 0, new Size(6, 6));
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

        public bool blocker()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(6, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 622, gameScreenCoords[1] + 31, 0, 0, new Size(6, 6));
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

        public bool atDoor()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3,3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 638, gameScreenCoords[1] + 106, 0, 0, new Size(3, 3));
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

        public bool atPortal()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 625, gameScreenCoords[1] + 94, 0, 0, new Size(3, 3));
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

        public bool flincher()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(6, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 649, gameScreenCoords[1] + 31, 0, 0, new Size(6, 6));
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

        public async void waitForKillStart()
        {
                if (blocker() && !flincher())
                {
                    if (!inv.hasItem(potion))
                    {
                        killState = 2;
                        bank();
                        return;
                    }
                if (sixHourTimer)
                    {
                        logout();
                        return;
                    }
                    if (!inv.hasItem("Shark"))
                    {
                        killState = 2;
                        bank();
                        return;
                    }
                    Cursor.Position = new Point(gameScreenCoords[0] + 203, gameScreenCoords[1] + 96);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 203, gameScreenCoords[1] + 96, 0, 0);
                    checkStartingPos();
                }
                else
                {
                    await Task.Delay(10);
                    waitForKillStart();
                }
        }

        public async void checkStartingPos()
        {
            if(attackSpot())
            {
                startScript();
            } else
            {
                await Task.Delay(600);
                checkStartingPos();
            }
        }

        public bool attackSpot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 640, gameScreenCoords[1] + 121, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 220)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool corpLured()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 628, gameScreenCoords[1] + 43, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 254 && bitmap.GetPixel(i, j).R >= 254)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool corpFailLure()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 620, gameScreenCoords[1] + 47, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 254 && bitmap.GetPixel(i, j).R >= 254)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool corpLurable()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(88, 13))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 594, gameScreenCoords[1] + 5, 0, 0, new Size(88, 13));
                }

                for (int i = 0; i < 87; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 253 && bitmap.GetPixel(i, j).R >= 253)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool corpLurable2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(62, 37))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 623, gameScreenCoords[1] + 5, 0, 0, new Size(62, 37));
                }

                for (int i = 0; i < 61; i++)
                {
                    for (int j = 0; j < 36; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 253 && bitmap.GetPixel(i, j).R >= 253)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void bank()
        {
            att = true;
            attack = true;
            inv.rubAmulet("Games nec");
            await Task.Delay(20);
            tele(2);
        }

        public async void tele(int option)
        {
            if(hasChatOptions())
            {
                if(option == 2)
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
            } else
            {
                await Task.Delay(300);
                tele(option);
            }

        }

        public async void walkToBank()
        {
            if(atBarbAssault())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 598 + bankWalkx, gameScreenCoords[1] + 10 +bankWalky);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 203, gameScreenCoords[1] + 96, 0, 0);
                await Task.Delay(20);
                mouse_event(MOUSEEVENTF_LEFTUP, gameScreenCoords[0] + 203, gameScreenCoords[1] + 96, 0, 0);
                await Task.Delay(20);
                clickBank();
            } else
            {
                await Task.Delay(300);
                walkToBank();
            }
        }

        public async void clickBank()
        {
            if(atBankArea())
            {
                await Task.Delay(600);
                mouse.leftClick(296, 129);
                await Task.Delay(600);
                doBanking();
            } else
            {
                await Task.Delay(300);
                clickBank();
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
                while (banking.busy)
                {
                    await Task.Delay(10);
                }
                banking.busy = true;
                banking.withdrawItem("Shark", 8);
                while (banking.busy)
                {
                    await Task.Delay(10);
                }
                banking.busy = true;
                banking.withdrawItem("Prayer Potion", 2);
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
                if (specialWeapon == "Dragon Warhammer") {
                    banking.busy = true;
                    banking.withdrawItem("Dragon Warhammer", 1);
                    while (banking.busy)
                    {
                        await Task.Delay(10);
                    }
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
            inv.rubAmulet("Games nec");
            await Task.Delay(20);
            tele(3);
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

        public async void quickExit()
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 259, gameScreenCoords[1] + 197);
            await Task.Delay(200);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10000);
            waitForRestart();
        }

        public async void waitForRestart()
        {
            if(blockerOutside() || flincherOutside())
            {
                checkAtDoor();
            } else
            {
                await Task.Delay(600);
                waitForRestart();
            }
            
        }

        public bool blockerOutside()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 620, gameScreenCoords[1] + 78, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
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

        public bool flincherOutside()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 620, gameScreenCoords[1] + 78, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
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

        public async void walkToStartingSpot()
        {
            if(killState == 0)
            {
                if(enterTextbox())
                {
                    await Task.Delay(2000);
                    Cursor.Position = new Point(gameScreenCoords[0] + 217, gameScreenCoords[1] + 102);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    checkAtDoor();
                } else
                {
                    await Task.Delay(600);
                    walkToStartingSpot();
                }
            }
            if (killState == 1)
            {
                if (enterTextbox())
                {
                    await Task.Delay(2000);
                    Cursor.Position = new Point(gameScreenCoords[0] + 217, gameScreenCoords[1] + 102);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    idk1();
                }
                else
                {
                    await Task.Delay(600);
                    walkToStartingSpot();
                }
            }
            if (killState == 2)
            {
                if (enterTextbox())
                {
                    await Task.Delay(2000);
                    Cursor.Position = new Point(gameScreenCoords[0] + 206, gameScreenCoords[1] + 56);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    checkStartingPos();
                }
                else
                {
                    await Task.Delay(600);
                    walkToStartingSpot();
                }
            }
            if (killState == 3)
            {
                if (enterTextbox())
                {
                    await Task.Delay(2000);
                    Cursor.Position = new Point(gameScreenCoords[0] + 217, gameScreenCoords[1] + 102);
                    await Task.Delay(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                    await Task.Delay(10);
                    checkAtDoor();
                }
                else
                {
                    await Task.Delay(600);
                    walkToStartingSpot();
                }
            }
        }

        public bool seeBank()
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

        public bool seeCorpBeast()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 632, gameScreenCoords[1] + 35, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 254 && bitmap.GetPixel(i, j).R >= 254)
                        {
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

                if (bitmap.GetPixel(658, 97).R <= 254 && bitmap.GetPixel(658, 97).R >= 225 && bitmap.GetPixel(658, 97).G == 0 && bitmap.GetPixel(658, 97).B == 0)
                {
                    portalx = 439;
                    portaly = 232;
                    return true;
                }
                if (bitmap.GetPixel(658, 101).R <= 254 && bitmap.GetPixel(658, 101).R >= 225 && bitmap.GetPixel(658, 101).G == 0 && bitmap.GetPixel(658, 101).B == 0)
                {
                    portalx = 434;
                    portaly = 214;
                    return true;
                }
                if (bitmap.GetPixel(654, 105).R <= 254 && bitmap.GetPixel(654, 105).R >= 225 && bitmap.GetPixel(654, 105).G == 0 && bitmap.GetPixel(654, 105).B == 0) {
                    portalx = 421;
                    portaly = 256;
                    return true;
                }
                if (bitmap.GetPixel(654, 101).R <= 254 && bitmap.GetPixel(654, 101).R >= 225 && bitmap.GetPixel(654, 101).G == 0 && bitmap.GetPixel(654, 101).B == 0)
                {
                    portalx = 421;
                    portaly = 238;
                    return true;
                }
                if (bitmap.GetPixel(654, 97).R <= 254 && bitmap.GetPixel(654, 97).R >= 225 && bitmap.GetPixel(654, 97).G == 0 && bitmap.GetPixel(654, 97).B == 0)
                {
                    portalx = 410;
                    portaly = 216;
                    return true;
                }
                if (bitmap.GetPixel(654, 93).R <= 254 && bitmap.GetPixel(654, 93).R >= 225 && bitmap.GetPixel(654, 93).G == 0 && bitmap.GetPixel(654, 93).B == 0)
                {
                    portalx = 410;
                    portaly = 194;
                    return true;
                }
                if (bitmap.GetPixel(650, 105).R <= 254 && bitmap.GetPixel(650, 105).R >= 225 && bitmap.GetPixel(650, 105).G == 0 && bitmap.GetPixel(650, 105).B == 0)
                {
                    portalx = 396;
                    portaly = 259;
                    return true;
                }
                if (bitmap.GetPixel(650, 101).R <= 254 && bitmap.GetPixel(650, 101).R >= 225 && bitmap.GetPixel(650, 101).G == 0 && bitmap.GetPixel(650, 101).B == 0)
                {
                    portalx = 393;
                    portaly = 239;
                    return true;
                }
                if (bitmap.GetPixel(650, 97).R <= 254 && bitmap.GetPixel(650, 97).R >= 225 && bitmap.GetPixel(650, 97).G == 0 && bitmap.GetPixel(650, 97).B == 0)
                {
                    portalx = 390;
                    portaly = 215;
                    return true;
                }
                if (bitmap.GetPixel(650, 93).R <= 254 && bitmap.GetPixel(650, 93).R >= 225 && bitmap.GetPixel(650, 93).G == 0 && bitmap.GetPixel(650, 93).B == 0)
                {
                    portalx = 387;
                    portaly = 194;
                    return true;
                }
                if (bitmap.GetPixel(650, 89).R <= 254 && bitmap.GetPixel(650, 89).R >= 225 && bitmap.GetPixel(650, 89).G == 0 && bitmap.GetPixel(650, 89).B == 0)
                {
                    portalx = 384;
                    portaly = 172;
                    return true;
                }
                if (bitmap.GetPixel(646, 105).R <= 254 && bitmap.GetPixel(646, 105).R >= 225 && bitmap.GetPixel(646, 105).G == 0 && bitmap.GetPixel(646, 105).B == 0)
                {
                    portalx = 379;
                    portaly = 257;
                    return true;
                }
                if (bitmap.GetPixel(646, 101).R <= 254 && bitmap.GetPixel(646, 101).R >= 225 && bitmap.GetPixel(646, 101).G == 0 && bitmap.GetPixel(646, 101).B == 0)
                {
                    portalx = 378;
                    portaly = 238;
                    return true;
                }
                if (bitmap.GetPixel(646, 97).R <= 254 && bitmap.GetPixel(646, 97).R >= 225 && bitmap.GetPixel(646, 97).G == 0 && bitmap.GetPixel(646, 97).B == 0)
                {
                    portalx = 371;
                    portaly = 215;
                    return true;
                }
                if (bitmap.GetPixel(646, 93).R <= 254 && bitmap.GetPixel(646, 93).R >= 225 && bitmap.GetPixel(646, 93).G == 0 && bitmap.GetPixel(646, 93).B == 0)
                {
                    portalx = 368;
                    portaly = 192;
                    return true;
                }
                if (bitmap.GetPixel(646, 89).R <= 254 && bitmap.GetPixel(646, 89).R >= 225 && bitmap.GetPixel(646, 89).G == 0 && bitmap.GetPixel(646, 89).B == 0)
                {
                    portalx = 367;
                    portaly = 173;
                    return true;
                }
                if (bitmap.GetPixel(642, 105).R <= 254 && bitmap.GetPixel(642, 105).R >= 225 && bitmap.GetPixel(642, 105).G == 0 && bitmap.GetPixel(642, 105).B == 0)
                {
                    portalx = 352;
                    portaly = 262;
                    return true;
                }
                if (bitmap.GetPixel(642, 101).R <= 254 && bitmap.GetPixel(642, 101).R >= 225 && bitmap.GetPixel(642, 101).G == 0 && bitmap.GetPixel(642, 101).B == 0)
                {
                    portalx = 349;
                    portaly = 241;
                    return true;
                }
                if (bitmap.GetPixel(642, 97).R <= 254 && bitmap.GetPixel(642, 97).R >= 225 && bitmap.GetPixel(642, 97).G == 0 && bitmap.GetPixel(642, 97).B == 0)
                {
                    portalx = 348;
                    portaly = 215;
                    return true;
                }
                if (bitmap.GetPixel(642, 93).R <= 254 && bitmap.GetPixel(642, 93).R >= 225 && bitmap.GetPixel(642, 93).G == 0 && bitmap.GetPixel(642, 93).B == 0)
                {
                    portalx = 348;
                    portaly = 197;
                    return true;
                }
                if (bitmap.GetPixel(642, 89).R <= 254 && bitmap.GetPixel(642, 89).R >= 225 && bitmap.GetPixel(642, 89).G == 0 && bitmap.GetPixel(642, 89).B == 0)
                {
                    portalx = 348;
                    portaly = 173;
                    return true;
                }
                if (bitmap.GetPixel(642, 85).R <= 254 && bitmap.GetPixel(642, 85).R >= 225 && bitmap.GetPixel(642, 85).G == 0 && bitmap.GetPixel(642, 85).B == 0)
                {
                    portalx = 348;
                    portaly = 155;
                    return true;
                }
                if (bitmap.GetPixel(637, 105).R <= 254 && bitmap.GetPixel(637, 105).R >= 225 && bitmap.GetPixel(637, 105).G == 0 && bitmap.GetPixel(637, 105).B == 0)
                {
                    portalx = 331;
                    portaly = 256;
                    return true;
                }
                if (bitmap.GetPixel(638, 102).R <= 254 && bitmap.GetPixel(638, 102).R >= 225 && bitmap.GetPixel(638, 102).G == 0 && bitmap.GetPixel(638, 102).B == 0)
                {
                    portalx = 329;
                    portaly = 238;
                    return true;
                }
                if (bitmap.GetPixel(638, 98).R <= 254 && bitmap.GetPixel(638, 98).R >= 225 && bitmap.GetPixel(638, 98).G == 0 && bitmap.GetPixel(638, 98).B == 0)
                {
                    portalx = 328;
                    portaly = 215;
                    return true;
                }
                if (bitmap.GetPixel(638, 94).R <= 254 && bitmap.GetPixel(638, 94).R >= 225 && bitmap.GetPixel(638, 94).G == 0 && bitmap.GetPixel(638, 94).B == 0)
                {
                    portalx = 323;
                    portaly = 194;
                    return true;
                }
                if (bitmap.GetPixel(638, 90).R <= 254 && bitmap.GetPixel(638, 90).R >= 225 && bitmap.GetPixel(638, 90).G == 0 && bitmap.GetPixel(638, 90).B == 0)
                {
                    portalx = 323;
                    portaly = 173;
                    return true;
                }
                if (bitmap.GetPixel(638, 86).R <= 254 && bitmap.GetPixel(638, 86).R >= 225 && bitmap.GetPixel(638, 86).G == 0 && bitmap.GetPixel(638, 86).B == 0)
                {
                    portalx = 321;
                    portaly = 153;
                    return true;
                }
                if (bitmap.GetPixel(638, 82).R <= 254 && bitmap.GetPixel(638, 82).R >= 225 && bitmap.GetPixel(638, 82).G == 0 && bitmap.GetPixel(638, 82).B == 0)
                {
                    portalx = 320;
                    portaly = 135;
                    return true;
                }
                if (bitmap.GetPixel(634, 102).R <= 254 && bitmap.GetPixel(634, 102).R >= 225 && bitmap.GetPixel(634, 102).G == 0 && bitmap.GetPixel(634, 102).B == 0)
                {
                    portalx = 306;
                    portaly = 238;
                    return true;
                }
                if (bitmap.GetPixel(634, 98).R <= 254 && bitmap.GetPixel(634, 98).R >= 225 && bitmap.GetPixel(634, 98).G == 0 && bitmap.GetPixel(634, 98).B == 0)
                {
                    portalx = 303;
                    portaly = 214;
                    return true;
                }
                if (bitmap.GetPixel(634, 94).R <= 254 && bitmap.GetPixel(634, 94).R >= 225 && bitmap.GetPixel(634, 94).G == 0 && bitmap.GetPixel(634, 94).B == 0)
                {
                    portalx = 303;
                    portaly = 193;
                    return true;
                }
                if (bitmap.GetPixel(634, 90).R <= 254 && bitmap.GetPixel(634, 90).R >= 225 && bitmap.GetPixel(634, 90).G == 0 && bitmap.GetPixel(634, 90).B == 0)
                {
                    portalx = 301;
                    portaly = 173;
                    return true;
                }
                if (bitmap.GetPixel(634, 86).R <= 254 && bitmap.GetPixel(634, 86).R >= 225 && bitmap.GetPixel(634, 86).G == 0 && bitmap.GetPixel(634, 86).B == 0)
                {
                    portalx = 301;
                    portaly = 153;
                    return true;
                }
                if (bitmap.GetPixel(630, 98).R <= 254 && bitmap.GetPixel(630, 98).R >= 225 && bitmap.GetPixel(630, 98).G == 0 && bitmap.GetPixel(630, 98).B == 0)
                {
                    portalx = 281;
                    portaly = 211;
                    return true;
                }
                if (bitmap.GetPixel(630, 94).R <= 254 && bitmap.GetPixel(630, 94).R >= 225 && bitmap.GetPixel(630, 94).G == 0 && bitmap.GetPixel(630, 94).B == 0)
                {
                    portalx = 283;
                    portaly = 193;
                    return true;
                }
                if (bitmap.GetPixel(630, 90).R <= 254 && bitmap.GetPixel(630, 90).R >= 225 && bitmap.GetPixel(630, 90).G == 0 && bitmap.GetPixel(630, 90).B == 0)
                {
                    portalx = 280;
                    portaly = 174;
                    return true;
                }
                if (bitmap.GetPixel(630, 86).R <= 254 && bitmap.GetPixel(630, 86).R >= 225 && bitmap.GetPixel(630, 86).G == 0 && bitmap.GetPixel(630, 86).B == 0)
                {
                    portalx = 280;
                    portaly = 153;
                    return true;
                }
                if (bitmap.GetPixel(626, 98).R <= 254 && bitmap.GetPixel(626, 98).R >= 225 && bitmap.GetPixel(626, 98).G == 0 && bitmap.GetPixel(626, 98).B == 0)
                {
                    portalx = 262;
                    portaly = 208;
                    return true;
                }
                if (bitmap.GetPixel(626, 94).R <= 254 && bitmap.GetPixel(626, 94).R >= 225 && bitmap.GetPixel(626, 94).G == 0 && bitmap.GetPixel(626, 94).B == 0)
                {
                    portalx = 260;
                    portaly = 196;
                    return true;
                }

                return false;

            }
        }

        public bool corpLured2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(700, 100))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(700, 100));
                }

                if (bitmap.GetPixel(630, 42).R <= 254 && bitmap.GetPixel(630, 42).R >= 225 && bitmap.GetPixel(630, 42).B <= 30)
                {
                    return true;
                }
            }
            return false;
        }

        public bool seeAccounts()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(40, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 607, gameScreenCoords[1] + 47, 0, 0, new Size(40, 20));
                }

                for (int i = 0; i < 39; i++)
                {
                    for (int j = 0; j < 19; j++)
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

        public bool atBankArea()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 658, gameScreenCoords[1] + 16, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 225 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool atBarbAssault()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(110, 100))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 598, gameScreenCoords[1] + 10, 0, 0, new Size(110, 100));
                }

                for (int i = 0; i < 93; i++)
                {
                    for (int j = 0; j < 65; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 203 && bitmap.GetPixel(i, j).G == 82 && bitmap.GetPixel(i, j).B == 81)
                        {
                                    bankWalkx = i - 18;
                                    bankWalky = j - 14;
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

        public bool somebodyInsde()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(80, 60))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 582, gameScreenCoords[1] + 1, 0, 0, new Size(80, 60));
                }

                for (int i = 0; i < 80; i++)
                {
                    for (int j = 0; j < 60; j++)
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


        public bool doorPos2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 637, gameScreenCoords[1] + 106, 0, 0, new Size(4, 4));
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

        public bool atLureSpot2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                        if (bitmap.GetPixel(626, 98).R >= 210 && bitmap.GetPixel(626, 98).G == 0)
                        {
                            return true;
                        }
                return false;

            }
        }

        public bool moved()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 663, gameScreenCoords[1] + 55, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 160 && bitmap.GetPixel(i, j).B >= 180)
                        {
                            return false;
                        }
                    }
                }
                return true;

            }
        }

        public async void headToLureSpot()
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 256, gameScreenCoords[1] + 95);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            waitForNpc();
        }
        public async void headToLureSpot2()
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 218, gameScreenCoords[1] + 131);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            waitForNpc();
        }

        public async void waitForNpc()
        {
            if(atLureSpot())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 552, gameScreenCoords[1] + 89);
                await Task.Delay(18000);
                mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0] + 55, gameScreenCoords[1] + 8, 0, 0); //turn prayer on
                Cursor.Position = new Point(gameScreenCoords[0] + 249, gameScreenCoords[1] + 303);
                waitForSpawn();
            } else
            {
                await Task.Delay(10);
                waitForNpc();
            }
        }

        public bool atLureSpot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 637, gameScreenCoords[1] + 138, 0, 0, new Size(4, 4));
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

        public void setCoords(int x, int y)
        {
            inv.setCoords(x, y);
            mouse.setCoords(x, y);
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            attackCoords[0] = x + 253;
            attackCoords[1] = y + 54;
            safespot[0] = x + 638;
            safespot[1] = y + 80;
            player.setCoords(gameScreenCoords[0], gameScreenCoords[1]);
        }

        public int getKills()
        {
            return kills;
        }

    }
}
