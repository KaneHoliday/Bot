using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Herblore
    {

        Inventory inv = new Inventory();
        public Player player = new Player();
        public Peripherals mouse = new Peripherals();
        Interfaces interfaces = new Interfaces();

        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public async void startScript()
        {
            mouse.busy = true;
            mouse.leftClick(616, 333);
            while(mouse.busy)
            {
                await Task.Delay(10);
            }
            mouse.busy = true;
            mouse.leftClick(661, 332);
            while (mouse.busy)
            {
                await Task.Delay(10);
            }
            while(!inter())
            {
                await Task.Delay(10);
            }
            Peripherals.PressKey((byte)Keys.Space, 1);
            waitForBank();
        }

        public async void waitForBank()
        {
            if(!hasSec())
            {
                mouse.busy = true;
                mouse.leftClick(160, 98);
                while (mouse.busy)
                {
                    await Task.Delay(10);
                }
                doBanking();
            } else
            {
                await Task.Delay(100);
                waitForBank();
            }
        }

        public async void doBanking()
        {
            if(bankOpen())
            {
                mouse.busy = true;
                mouse.leftClick(440, 309);
                while (mouse.busy)
                {
                    await Task.Delay(10);
                }
                mouse.busy = true;
                mouse.leftClick(422, 273);
                while (mouse.busy)
                {
                    await Task.Delay(10);
                }
                mouse.busy = true;
                mouse.leftClick(419, 240);
                while (mouse.busy)
                {
                    await Task.Delay(10);
                } 
                Peripherals.PressKey((byte)Keys.Escape, 1);
                await Task.Delay(600);
                startScript();
            } else
            {
                await Task.Delay(100);
                doBanking();
            }
        }

        public bool hasSec()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(170, 250))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 556, gameScreenCoords[1] + 208, 0, 0, new Size(170, 250));
                }

                for (int i = 0; i < 170; i++)
                {
                    for (int j = 0; j < 250; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 65 && bitmap.GetPixel(i, j).R == 9 && bitmap.GetPixel(i, j).B == 23)
                        {
                            return true;
                        }
                        if (bitmap.GetPixel(i, j).G == 88 && bitmap.GetPixel(i, j).R == 19 && bitmap.GetPixel(i, j).B == 39)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool inter()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(280, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 20, gameScreenCoords[1] + 355, 0, 0, new Size(280, 20));
                }

                for (int i = 0; i < 280; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 64 && bitmap.GetPixel(i, j).G == 48 && bitmap.GetPixel(i, j).B == 32)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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


        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            player.setCoords(x, y);
            interfaces.setCoords(x, y);
            mouse.setCoords(x, y);
        }

    }
}
