using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class CorpFlinching
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        Inventory inv = new Inventory();
        public Player player = new Player();
        Banking banking = new Banking();
        Peripherals mouse = new Peripherals();

        public int[] gameScreenCoords = new int[2];
        public int[] attackCoords = new int[2];
        public int[] safespot = new int[2];
        public int attackCount = 0;
        public Color checkMovement;
        public bool attack = true;
        public int kills;
        public int flinchTime;

        public bool firstKill = true;

        public int portalx;
        public int portaly;

        public bool pass1;
        public bool pass2;
        public bool pass3;

        public float storedHp = 0;

        public bool sixHourTimer = false;

        public int bankWalkx = 0;
        public int bankWalky = 0;

        public bool enablePanic = false;

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

        public async void panicTele()
        {
            if(storedHp > player.getHp())
            {
                inv.clickInventory("Panic tab");
                firstKill = true;
            } else
            {
                await Task.Delay(100);
                panicTele();
            }
        }

        public async void startScript()
        {
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

            if (!checkNPC())
            {
                if (!inv.hasItem(potion)|| !inv.hasItem("Shark"))
                {
                    bank();
                    return;
                }
                kills++;
                headToDoor();
                return;
            }
            else
            Cursor.Position = new Point(attackCoords[0], attackCoords[1]);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            Cursor.Position = new Point(safespot[0], safespot[1]);
            await Task.Delay(100);
            if (moved())
            {

                mouse_event(MOUSEEVENTF_LEFTDOWN, safespot[0], safespot[1], 0, 0);
                if (checkSpec() && attackCount > 3)
                {
                    Cursor.Position = new Point(gameScreenCoords[0] + 584, gameScreenCoords[1] + 146);
                    await Task.Delay(30);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                }
                if (storedHp > player.getHp())
                {
                    inv.clickInventory("Panic tab");
                    firstKill = false;
                    await Task.Delay(10000);
                    teleToCorp();
                    return;
                }
                await Task.Delay(flinchTime);
                attackCount++;
                attack = true;
                startScript();
            }
            else
            {
                if (storedHp > player.getHp())
                {
                    await Task.Delay(10000);
                    teleToCorp();
                    return;
                }
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

        public async void cameraPos()
        {
            setPotion();
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
            Cursor.Position = new Point(gameScreenCoords[0] + 652, gameScreenCoords[1] + 314);
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
            enterDoor();
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
            while(atLoginScreen())
            {
                await Task.Delay(10);
            }
            await Task.Delay(600);
            Cursor.Position = new Point(gameScreenCoords[0] + 411, gameScreenCoords[1] + 336);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
            await Task.Delay(10);
            lurerEnterPortal();
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
            Cursor.Position = new Point(gameScreenCoords[0] + 652, gameScreenCoords[1] + 314);
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
            if(pos1())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 216, gameScreenCoords[1] + 178);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                mouse_event(MOUSEEVENTF_LEFTUP, attackCoords[0], attackCoords[1], 0, 0);
                await Task.Delay(10);
                moveToFlinchStart();
            } else
            if(pos2())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 254, gameScreenCoords[1] + 138);
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
                Cursor.Position = new Point(gameScreenCoords[0] + 655, gameScreenCoords[1] + 106);
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

        public async void headToDoor()
        {
            Cursor.Position = new Point(gameScreenCoords[0] + 676, gameScreenCoords[1] + 49);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            attackCount = 0;
            exitRoom();
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

        public async void exitRoom()
        {
            if(doorPos())
            {
                if (sixHourTimer)
                {
                    logout();
                    return;
                }
                Cursor.Position = new Point(gameScreenCoords[0] + 258, gameScreenCoords[1] + 109);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                enterDoor();
            } else
            {
                await Task.Delay(600);
                exitRoom();
            }
        }

        public async void enterDoor()
        {
            if(checkStam())
            {
                inv.clickInventory("Stamina potion");
                inv.clickInventory("Energy Potion");
                await Task.Delay(600);
            } else
            {
                if (lured() && lurer())
                {
                    if (firstKill)
                    {
                        storedHp = player.getHp();
                        if (!enablePanic)
                        {
                            panicTele();
                        }
                        Cursor.Position = new Point(gameScreenCoords[0] + 254, gameScreenCoords[1] + 228);
                        await Task.Delay(2000);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                        walkToSpot1();
                    }
                    else if (blockerOut())
                    {
                        Cursor.Position = new Point(gameScreenCoords[0] + 254, gameScreenCoords[1] + 228);
                        await Task.Delay(2000);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                        storedHp = player.getHp();
                        walkToSpot1();
                        return;
                    }
                    else
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
                        }
                        else
                        {
                            await Task.Delay(200);
                            enterDoor();
                        }
                    }
                }
                else
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
                    }
                    else
                    {
                        await Task.Delay(600);
                        enterDoor();
                    }
                }
            }
        }

        public bool blockerOut()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(6, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 643, gameScreenCoords[1] + 78, 0, 0, new Size(6, 6));
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

        public bool lurer()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 617, gameScreenCoords[1] + 70, 0, 0, new Size(3, 3));
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

        public bool blocker()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
             {
               using (Graphics g = Graphics.FromImage(bitmap))
             {
                 g.CopyFromScreen(gameScreenCoords[0] + 663, gameScreenCoords[1] + 78, 0, 0, new Size(3, 3));
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

        public async void walkToSpot1()
        {
            if (storedHp > player.getHp())
            {
                await Task.Delay(10000);
                teleToCorp();
                return;
            }
            if (stoppedMoving())
            {
                Cursor.Position = new Point(gameScreenCoords[0] + 641, gameScreenCoords[1] + 107);
                await Task.Delay(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
                walkToSpot2();
            } else
            {
                await Task.Delay(600);
                walkToSpot1();
            }
        }

        public async void walkToSpot2()
        {
            while(!stoppedMoving2())
            {
                if (storedHp > player.getHp())
                {
                    await Task.Delay(10000);
                    teleToCorp();
                    return;
                }
                firstKill = true;
                await Task.Delay(10);
            }
            Cursor.Position = new Point(gameScreenCoords[0] + 597, gameScreenCoords[1] + 75);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, attackCoords[0], attackCoords[1], 0, 0);
            waitForStart();
        }

        public async void waitForStart()
        {
            if (storedHp > player.getHp())
            {
                await Task.Delay(10000);
                teleToCorp();
                return;
            }
            if (atLureSpot() && blocker())
            {
                if (inv.hasItem("Super Attack Potion"))
                {
                    inv.clickInventory("Super Attack Potion");
                } else if (inv.hasItem("Super Combat Potion"))
                {
                    inv.clickInventory("Super Combat Potion");
                }
                await Task.Delay(1000);
                storedHp = player.getHp();
                startScript();
            } else
            {
                await Task.Delay(600);
                waitForStart();
            }
        }


        public bool atLureSpot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 660, gameScreenCoords[1] + 68, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
               {
                  for (int j = 0; j < 3; j++)
                    {
                         {
                            if (bitmap.GetPixel(i, j).R >= 225 && bitmap.GetPixel(i, j).B == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            return false;

            }
        }

        public bool stoppedMoving()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 628, gameScreenCoords[1] + 37, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 225 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool stoppedMoving2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(4, 4))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 623, gameScreenCoords[1] + 9, 0, 0, new Size(4, 4));
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 225 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool lured()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 624, gameScreenCoords[1] + 106, 0, 0, new Size(3, 3));
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 253)
                        {
                                return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool checkStam()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(10,10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 531, gameScreenCoords[1] + 120, 0, 0, new Size(10,10));
                }
                
                for(int i = 0; i < 10; i++)
                {
                    for(int j = 0; j < 10; j++)
                    {
                        if (bitmap.GetPixel(i,j).G == 255)
                        {
                            if(bitmap.GetPixel(i,j).R >= 200)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;

            }
        }

        public bool doorPos()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 628, gameScreenCoords[1] + 37, 0, 0, new Size(4, 4));
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

        public bool checkNPC()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(21, 21))
            {
               using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 654, gameScreenCoords[1] + 61, 0, 0, new Size(20, 20));
                    g.CopyFromScreen(gameScreenCoords[0] + 657, gameScreenCoords[1] + 65, 0, 0, new Size(10, 10));
                }
            
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 254)
                        {
                            return true;
                        }
                    }
                }
            return false;

            }
        }



        public bool moved()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 230, gameScreenCoords[1] + 170, 0, 0, new Size(3, 3));
                }
                checkMovement = bitmap.GetPixel(1, 1);
            }

            while (attack)
            {

                using (Bitmap bitmap = new Bitmap(3, 3))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 230, gameScreenCoords[1] + 170, 0, 0, new Size(3, 3));
                    }
                    if(!checkNPC())
                    {
                        attack = false;
                        return true;
                    }
                    if(bitmap.GetPixel(1,1) != checkMovement)
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
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            attackCoords[0] = x + 452;
            attackCoords[1] = y + 56;
            safespot[0] = x + 225;
            safespot[1] = y + 176;
            player.setCoords(gameScreenCoords[0], gameScreenCoords[1]);
            mouse.setCoords(x, y);
        }

        public int getKills()
        {
            return kills;
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
                Cursor.Position = new Point(gameScreenCoords[0] + 289, gameScreenCoords[1] + 206);
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
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R >= 225 && bitmap.GetPixel(i, j).B == 0)
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
                        banking.withdrawItem(potion, 10);
                    }
                    else
                    {
                        switchPotion();
                        await Task.Delay(600);
                        if (potion != "Nothing")
                        {
                            banking.busy = true;
                            banking.withdrawItem(potion, 10);
                        }
                    }
                }
                while (banking.busy)
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
                banking.busy = true;
                banking.withdrawItem("Panic tab", 5);
                while (banking.busy)
                {
                    await Task.Delay(10);
                }
                Peripherals.HoldKey((byte)Keys.Escape, 1);
                await Task.Delay(600);
                teleToCorp();
            }
            else
            {
                await Task.Delay(600);
                doBanking();
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

        public async void teleToCorp()
        {
            if (!inv.hasItem("Panic tab"))
            {
                bank();
            }
            else
            {
                inv.rubAmulet("Games nec");
                tele(3);
            }
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
            if (seeAccounts())
            {
                await Task.Delay(2000);
                Cursor.Position = new Point(gameScreenCoords[0] + 428, gameScreenCoords[1] + 329);
                await Task.Delay(100);
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
                    portalx = 3;
                    portaly = 109;
                    return true;
                }
                if (bitmap.GetPixel(617, 57).R <= 254 && bitmap.GetPixel(617, 57).R >= 225 && bitmap.GetPixel(617, 57).G == 0 && bitmap.GetPixel(617, 57).B == 0)
                {
                    portalx = 4;
                    portaly = 86;
                    return true;
                }
                if (bitmap.GetPixel(621, 53).R <= 254 && bitmap.GetPixel(621, 53).R >= 225 && bitmap.GetPixel(621, 53).G == 0 && bitmap.GetPixel(621, 53).B == 0)
                {
                    portalx = 32;
                    portaly = 59;
                    return true;
                }
                if (bitmap.GetPixel(621, 57).R <= 254 && bitmap.GetPixel(621, 57).R >= 225 && bitmap.GetPixel(621, 57).G == 0 && bitmap.GetPixel(621, 57).B == 0)
                {
                    portalx = 28;
                    portaly = 81;
                    return true;
                }
                if (bitmap.GetPixel(621, 61).R <= 254 && bitmap.GetPixel(621, 61).R >= 225 && bitmap.GetPixel(621, 61).G == 0 && bitmap.GetPixel(621, 61).B == 0)
                {
                    portalx = 22;
                    portaly = 108;
                    return true;
                }
                if (bitmap.GetPixel(621, 65).R <= 254 && bitmap.GetPixel(621, 65).R >= 225 && bitmap.GetPixel(621, 65).G == 0 && bitmap.GetPixel(621, 65).B == 0)
                {
                    portalx = 15;
                    portaly = 138;
                    return true;
                }
                if (bitmap.GetPixel(625, 53).R <= 254 && bitmap.GetPixel(625, 53).R >= 225 && bitmap.GetPixel(625, 53).G == 0 && bitmap.GetPixel(625, 53).B == 0)
                {
                    portalx = 64;
                    portaly = 60;
                    return true;
                }
                if (bitmap.GetPixel(625, 57).R <= 254 && bitmap.GetPixel(625, 57).R >= 225 && bitmap.GetPixel(625, 57).G == 0 && bitmap.GetPixel(625, 57).B == 0)
                {
                    portalx = 58;
                    portaly = 81;
                    return true;
                }
                if (bitmap.GetPixel(625, 61).R <= 254 && bitmap.GetPixel(625, 61).R >= 225 && bitmap.GetPixel(625, 61).G == 0 && bitmap.GetPixel(625, 61).B == 0)
                {
                    portalx = 56;
                    portaly = 110;
                    return true;
                }
                if (bitmap.GetPixel(625, 65).R <= 254 && bitmap.GetPixel(625, 65).R >= 225 && bitmap.GetPixel(625, 65).G == 0 && bitmap.GetPixel(625, 65).B == 0)
                {
                    portalx = 49;
                    portaly = 139;
                    return true;
                }
                if (bitmap.GetPixel(625, 69).R <= 254 && bitmap.GetPixel(625, 69).R >= 225 && bitmap.GetPixel(625, 69).G == 0 && bitmap.GetPixel(625, 69).B == 0)
                {
                    portalx = 50;
                    portaly = 171;
                    return true;
                }
                if (bitmap.GetPixel(629, 53).R <= 254 && bitmap.GetPixel(629, 53).R >= 225 && bitmap.GetPixel(629, 53).G == 0 && bitmap.GetPixel(629, 53).B == 0)
                {
                    portalx = 94;
                    portaly = 60;
                    return true;
                }
                if (bitmap.GetPixel(629, 57).R <= 254 && bitmap.GetPixel(629, 57).R >= 225 && bitmap.GetPixel(629, 57).G == 0 && bitmap.GetPixel(629, 57).B == 0)
                {
                    portalx = 92;
                    portaly = 81;
                    return true;
                }
                if (bitmap.GetPixel(629, 61).R <= 254 && bitmap.GetPixel(629, 61).R >= 225 && bitmap.GetPixel(629, 61).G == 0 && bitmap.GetPixel(629, 61).B == 0)
                {
                    portalx = 88;
                    portaly = 109;
                    return true;
                }
                if (bitmap.GetPixel(629, 65).R <= 254 && bitmap.GetPixel(629, 65).R >= 225 && bitmap.GetPixel(629, 65).G == 0 && bitmap.GetPixel(629, 65).B == 0)
                {
                    portalx = 83;
                    portaly = 139;
                    return true;
                }
                if (bitmap.GetPixel(629, 69).R <= 254 && bitmap.GetPixel(629, 69).R >= 225 && bitmap.GetPixel(629, 69).G == 0 && bitmap.GetPixel(629, 69).B == 0)
                {
                    portalx = 79;
                    portaly = 170;
                    return true;
                }
                if (bitmap.GetPixel(633, 53).R <= 254 && bitmap.GetPixel(633, 53).R >= 225 && bitmap.GetPixel(633, 53).G == 0 && bitmap.GetPixel(633, 53).B == 0)
                {
                    portalx = 126;
                    portaly = 60;
                    return true;
                }
                if (bitmap.GetPixel(633, 57).R <= 254 && bitmap.GetPixel(633, 57).R >= 225 && bitmap.GetPixel(633, 57).G == 0 && bitmap.GetPixel(633, 57).B == 0)
                {
                    portalx = 125;
                    portaly = 82;
                    return true;
                }
                if (bitmap.GetPixel(633, 61).R <= 254 && bitmap.GetPixel(633, 61).R >= 225 && bitmap.GetPixel(633, 61).G == 0 && bitmap.GetPixel(633, 61).B == 0)
                {
                    portalx = 121;
                    portaly = 110;
                    return true;
                }
                if (bitmap.GetPixel(633, 65).R <= 254 && bitmap.GetPixel(633, 65).R >= 225 && bitmap.GetPixel(633, 65).G == 0 && bitmap.GetPixel(633, 65).B == 0)
                {
                    portalx = 117;
                    portaly = 139;
                    return true;
                }
                if (bitmap.GetPixel(633, 69).R <= 254 && bitmap.GetPixel(633, 69).R >= 225 && bitmap.GetPixel(633, 69).G == 0 && bitmap.GetPixel(633, 69).B == 0)
                {
                    portalx = 113;
                    portaly = 170;
                    return true;
                }
                if (bitmap.GetPixel(633, 73).R <= 254 && bitmap.GetPixel(633, 73).R >= 225 && bitmap.GetPixel(633, 73).G == 0 && bitmap.GetPixel(633, 73).B == 0)
                {
                    portalx = 112;
                    portaly = 201;
                    return true;
                }
                if (bitmap.GetPixel(637, 53).R <= 254 && bitmap.GetPixel(637, 53).R >= 225 && bitmap.GetPixel(637, 53).G == 0 && bitmap.GetPixel(637, 53).B == 0)
                {
                    portalx = 158;
                    portaly = 59;
                    return true;
                }
                if (bitmap.GetPixel(637, 57).R <= 254 && bitmap.GetPixel(637, 57).R >= 225 && bitmap.GetPixel(637, 57).G == 0 && bitmap.GetPixel(637, 57).B == 0)
                {
                    portalx = 157;
                    portaly = 81;
                    return true;
                }
                if (bitmap.GetPixel(637, 61).R <= 254 && bitmap.GetPixel(637, 61).R >= 225 && bitmap.GetPixel(637, 61).G == 0 && bitmap.GetPixel(637, 61).B == 0)
                {
                    portalx = 155;
                    portaly = 109;
                    return true;
                }
                if (bitmap.GetPixel(637, 65).R <= 254 && bitmap.GetPixel(637, 65).R >= 225 && bitmap.GetPixel(637, 65).G == 0 && bitmap.GetPixel(637, 65).B == 0)
                {
                    portalx = 151;
                    portaly = 139;
                    return true;
                }
                if (bitmap.GetPixel(637, 69).R <= 254 && bitmap.GetPixel(637, 69).R >= 225 && bitmap.GetPixel(637, 69).G == 0 && bitmap.GetPixel(637, 69).B == 0)
                {
                    portalx = 147;
                    portaly = 175;
                    return true;
                }
                if (bitmap.GetPixel(637, 73).R <= 254 && bitmap.GetPixel(637, 73).R >= 225 && bitmap.GetPixel(637, 73).G == 0 && bitmap.GetPixel(637, 73).B == 0)
                {
                    portalx = 147;
                    portaly = 201;
                    return true;
                }
                if (bitmap.GetPixel(637, 77).R <= 254 && bitmap.GetPixel(637, 77).R >= 225 && bitmap.GetPixel(637, 77).G == 0 && bitmap.GetPixel(637, 77).B == 0)
                {
                    portalx = 144;
                    portaly = 235;
                    return true;
                }
                if (bitmap.GetPixel(641, 56).R <= 254 && bitmap.GetPixel(641, 56).R >= 225 && bitmap.GetPixel(641, 56).G == 0 && bitmap.GetPixel(641, 56).B == 0)
                {
                    portalx = 189;
                    portaly = 86;
                    return true;
                }
                if (bitmap.GetPixel(641, 60).R <= 254 && bitmap.GetPixel(641, 60).R >= 225 && bitmap.GetPixel(641, 60).G == 0 && bitmap.GetPixel(641, 60).B == 0)
                {
                    portalx = 186;
                    portaly = 108;
                    return true;
                }
                if (bitmap.GetPixel(641, 64).R <= 254 && bitmap.GetPixel(641, 64).R >= 225 && bitmap.GetPixel(641, 64).G == 0 && bitmap.GetPixel(641, 64).B == 0)
                {
                    portalx = 186;
                    portaly = 138;
                    return true;
                }
                if (bitmap.GetPixel(641, 68).R <= 254 && bitmap.GetPixel(641, 68).R >= 225 && bitmap.GetPixel(641, 68).G == 0 && bitmap.GetPixel(641, 68).B == 0)
                {
                    portalx = 182;
                    portaly = 167;
                    return true;
                }
                if (bitmap.GetPixel(641, 72).R <= 254 && bitmap.GetPixel(641, 72).R >= 225 && bitmap.GetPixel(641, 72).G == 0 && bitmap.GetPixel(641, 72).B == 0)
                {
                    portalx = 184;
                    portaly = 201;
                    return true;
                }
                if (bitmap.GetPixel(645, 60).R <= 254 && bitmap.GetPixel(645, 60).R >= 225 && bitmap.GetPixel(645, 60).G == 0 && bitmap.GetPixel(645, 60).B == 0)
                {
                    portalx = 220;
                    portaly = 115;
                    return true;
                }
                if (bitmap.GetPixel(645, 64).R <= 254 && bitmap.GetPixel(645, 64).R >= 225 && bitmap.GetPixel(645, 64).G == 0 && bitmap.GetPixel(645, 64).B == 0)
                {
                    portalx = 218;
                    portaly = 138;
                    return true;
                }
                if (bitmap.GetPixel(645, 68).R <= 254 && bitmap.GetPixel(645, 68).R >= 225 && bitmap.GetPixel(645, 68).G == 0 && bitmap.GetPixel(645, 68).B == 0)
                {
                    portalx = 217;
                    portaly = 171;
                    return true;
                }
                if (bitmap.GetPixel(645, 72).R <= 254 && bitmap.GetPixel(645, 72).R >= 225 && bitmap.GetPixel(645, 72).G == 0 && bitmap.GetPixel(645, 72).B == 0)
                {
                    portalx = 217;
                    portaly = 204;
                    return true;
                }
                if (bitmap.GetPixel(649, 60).R <= 254 && bitmap.GetPixel(649, 60).R >= 225 && bitmap.GetPixel(649, 60).G == 0 && bitmap.GetPixel(649, 60).B == 0)
                {
                    portalx = 253;
                    portaly = 115;
                    return true;
                }
                if (bitmap.GetPixel(649, 64).R <= 254 && bitmap.GetPixel(649, 64).R >= 225 && bitmap.GetPixel(649, 64).G == 0 && bitmap.GetPixel(649, 64).B == 0)
                {
                    portalx = 252;
                    portaly = 139;
                    return true;
                }

                return false;

            }
        }


        }
}