using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Agility
    {

        Inventory inv = new Inventory();
        public Player player = new Player();
        Peripherals peripherals = new Peripherals();
        Mouse mouse = new Mouse();
        Interfaces interfaces = new Interfaces();
        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public int goldCount = 0;

        public int startx;
        public int starty;

        public int pos;
        public bool goBank = false;


        public void startScript()
        {
            mouse.initMouse();
            mouse.mouseClickLoop();
            waitForStart();
        }

        public async void waitForStart()
        {
            if (startClick())
            {
                startClick();
                await Task.Delay(100);
                if (startx == 313 || starty == 138 || startx == 289 || starty == 128 || startx == 287 || starty == 121 || startx == 302 || starty == 127 || startx == 312 || starty == 117)
                {
                    mouse.addMouseClick(gameScreenCoords[0] + startx, gameScreenCoords[1] + starty - 10);
                }
                else
                {
                    mouse.addMouseClick(gameScreenCoords[0] + startx, gameScreenCoords[1] + starty);
                }
                waitForFirst();
            } else
            {
                await Task.Delay(50);
                waitForStart();
            }
        }

        public async void waitForFirst()
        {
            if(first())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 147, gameScreenCoords[1] + 139);
                waitForSecond();
            } else
            {
                await Task.Delay(50);
                waitForFirst();
            }
        }

        public async void waitForSecond()
        {
            if (second())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 212, gameScreenCoords[1] + 240);
                waitForThird();
            }
            else
            {
                await Task.Delay(50);
                waitForSecond();
            }
        }

        public async void waitForThird()
        {
            if (failed())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 691, gameScreenCoords[1] + 285);
                waitForStart();
                return;
            }
            if (third())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 267, gameScreenCoords[1] + 222);
                waitForFourth();
                return;
            }
            await Task.Delay(50);
            waitForThird();
        }

        public async void waitForFourth()
        {
            if (fourth())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 132, gameScreenCoords[1] + 208);
                waitForFifth();
            }
            else
            {
                await Task.Delay(50);
                waitForFourth();
            }
        }

        public async void waitForFifth()
        {
            if (fifth())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 270, gameScreenCoords[1] + 170);
                waitForFinal();
            }
            else
            {
                await Task.Delay(50);
                waitForFifth();
            }
        }

        public async void waitForFinal()
        {
            if (final())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 691, gameScreenCoords[1] + 285);
                waitForStart();
            }
            else
            {
                await Task.Delay(50);
                waitForFinal();
            }
        }

        public async void superHeat()
        {
            goldCount = 5;
            if (goldCount > 14)
            {
                goBank = true;
            }
            if (goldCount > 26)
            {
                goBank = true;
            } else if (goldCount < 8)
            {
                peripherals.fastLeftClick(668, 290);
                await Task.Delay(100);
                peripherals.fastLeftClick(668, 290);
                goldCount++;
            } else
            {
                peripherals.fastLeftClick(668, 290);
                await Task.Delay(100);
                peripherals.fastLeftClick(702, 441);
                goldCount++;
            }

        }

        public async void goToBank()
        {
            pos = checkPos();
            await Task.Delay(100);

            switch(pos)
            {
                case 0:
                    peripherals.leftClick(279, 58);
                    waitForBank();
                    break;
                case 1:
                    peripherals.leftClick(267, 61);
                    waitForBank();
                    break;
                case 2:
                    peripherals.leftClick(257, 62);
                    waitForBank();
                    break;
                case 3:
                    peripherals.leftClick(245, 68);
                    waitForBank();
                    break;
                default:
                    superHeat();
                    await Task.Delay(600);
                    startClick();
                    await Task.Delay(600);
                    peripherals.leftClick(startx, starty);
                    //MessageBox.Show("X: " + startx + "Y: " + starty);
                    waitForFirst();
                    break;
            }

        }

        public async void waitForBank()
        {
            goldCount = 0;
            if (seeBank())
            {
                peripherals.leftClick(575, 260);
                await Task.Delay(600);
                peripherals.leftClick(424, 265);
                await Task.Delay(600);
                Peripherals.HoldKey((byte)Keys.Escape, 1);
                goldCount = 0;
                await Task.Delay(200);
                peripherals.leftClick(287, 217);
                waitForFirst();
            } else
            {
                await Task.Delay(100);
                waitForBank();
            }
        }

        public bool seeBank()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(21, 21))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 49, gameScreenCoords[1] + 9, 0, 0, new Size(21, 21));
                }

                for (int i = 0; i < 21; i++)
                {
                    for (int j = 0; j < 21; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 255 && bitmap.GetPixel(i, j).G == 152 && bitmap.GetPixel(i, j).B == 31)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public int checkPos()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(573, 65).R >= 220 && bitmap.GetPixel(573, 65).G == 0)
                {
                    return 0;
                }
                if (bitmap.GetPixel(569, 65).R >= 220 && bitmap.GetPixel(569, 65).G == 0)
                {
                    return 1;
                }
                if (bitmap.GetPixel(572, 65).R >= 220 && bitmap.GetPixel(572, 65).G == 0)
                {
                    return 2;
                }
                if (bitmap.GetPixel(568, 69).R >= 220 && bitmap.GetPixel(568, 69).G == 0)
                {
                    return 3;
                }
            }
            return 5;
        }

        public bool first()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(625, 68).R >= 220 && bitmap.GetPixel(625, 68).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool second()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(644, 138).R >= 220 && bitmap.GetPixel(644, 138).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool third()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(656, 81).R >= 220 && bitmap.GetPixel(656, 81).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool failed()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(633, 33).R >= 220 && bitmap.GetPixel(633, 33).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool fourth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(600, 73).R >= 220 && bitmap.GetPixel(600, 73).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool fifth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(629, 93).R >= 220 && bitmap.GetPixel(629, 93).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool final()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(637, 88).R >= 220 && bitmap.GetPixel(637, 88).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool startClick()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(200, 150))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 218, gameScreenCoords[1] + 95, 0, 0, new Size(200, 150));
                }
                for (int i = 0; i < 200; i++)
                {
                    for (int j = 0; j < 150; j++)
                    {
                        if ((bitmap.GetPixel(i, j).R == 0 && bitmap.GetPixel(i, j).G == 255 && bitmap.GetPixel(i, j).B == 0) || (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0))
                        {
                            startx = i + 5 + 218; 
                            starty = j + 95;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            player.setCoords(x, y);
            interfaces.setCoords(x, y);
            peripherals.setCoords(x, y);
        }

    }
}
