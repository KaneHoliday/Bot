using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class ArdyAgility
    {
        Inventory inv = new Inventory();
        public Player player = new Player();
        Peripherals peripherals = new Peripherals();
        public Mouse mouse = new Mouse();
        Interfaces interfaces = new Interfaces();
        public int[] gameScreenCoords = new int[2] { 0, 0 };


        public void startScript()
        {
            mouse.initMouse();
            mouse.mouseClickLoop();
            firstObst();
        }

        public async void firstObst()
        {
            if (atStart())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 322, gameScreenCoords[1] + 153);
                waitForFirst();
            } else
            {
                await Task.Delay(100);
                firstObst();
            }
        }

        public async void waitForFirst()
        {
            if (first())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 252, gameScreenCoords[1] + 53);
                waitForSecond();
            } else
            {
                await Task.Delay(100);
                waitForFirst();
            }
        }

        public async void waitForSecond()
        {
            if(second())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 206, gameScreenCoords[1] + 167);
                waitForThird();
            } else
            {
                await Task.Delay(100);
                waitForSecond();
            }
        }

        public async void waitForThird()
        {
            if (third())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 212, gameScreenCoords[1] + 169);
                waitForFourth();
            }
            else
            {
                await Task.Delay(100);
                waitForThird();
            }
        }

        public async void waitForFourth()
        {
            if (fourth())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 256, gameScreenCoords[1] + 243);
                waitForFifth();
            }
            else
            {
                await Task.Delay(100);
                waitForFourth();
            }
        }

        public async void waitForFifth()
        {
            if (fifth())
            {
                await Task.Delay(1000);
                mouse.addMouseClick(gameScreenCoords[0] + 308, gameScreenCoords[1] + 312);
                waitForSixth();
            }
            else
            {
                await Task.Delay(100);
                waitForFifth();
            }
        }

        public async void waitForSixth()
        {
            if (sixth())
            {
                await Task.Delay(600);
                mouse.addMouseClick(gameScreenCoords[0] + 263, gameScreenCoords[1] + 180);
                firstObst();
            }
            else
            {
                await Task.Delay(100);
                waitForSixth();
            }
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

                if (bitmap.GetPixel(638, 45).R == 186 && bitmap.GetPixel(638, 45).G == 167 && bitmap.GetPixel(638, 45).B == 87)
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

                if (bitmap.GetPixel(615, 77).R == 96 && bitmap.GetPixel(615, 77).G == 74 && bitmap.GetPixel(615, 77).B == 14)
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

                if (bitmap.GetPixel(656, 78).R == 96 && bitmap.GetPixel(656, 78).G == 74 && bitmap.GetPixel(656, 78).B == 14)
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

                if (bitmap.GetPixel(665, 61).R == 96 && bitmap.GetPixel(665, 61).G == 74 && bitmap.GetPixel(665, 61).B == 14)
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

                if (bitmap.GetPixel(675, 42).R == 96 && bitmap.GetPixel(675, 42).G == 74 && bitmap.GetPixel(675, 42).B == 14)
                {
                    return true;
                }
            }
            return false;
        }

        public bool sixth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(642, 85).R >=240 && bitmap.GetPixel(642, 85).G >= 240 && bitmap.GetPixel(642, 85).B <= 30)
                {
                    return true;
                }
            }
            return false;
        }

        public bool atStart()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(665, 128).R >= 230 && bitmap.GetPixel(665, 128).G == 0 && bitmap.GetPixel(665, 128).B == 0)
                {
                    return true;
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
