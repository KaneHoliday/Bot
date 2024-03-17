using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Fireblore
    {

        Inventory inv = new Inventory();
        public Player player = new Player();
        public Peripherals mouse = new Peripherals();
        Interfaces interfaces = new Interfaces();

        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public int potion = 0;
        public int log = 0;

        public int pos = 1;

        public void startScript()
        {
            mouse.busy = true;
            mouse.leftClick(662, 333); //click sec
            makePotion();
        }

        public async void makePotion()
        {
            while(mouse.busy)
            {
                await Task.Delay(10);
            }
            mouse.busy = true;
            switch(potion)
            {
                case 0:
                    mouse.leftClick(574, 228);
                    break;
                case 1:
                    mouse.leftClick(616, 229);
                    break;
                case 2:
                    mouse.leftClick(658, 228);
                    break;
                case 3:
                    mouse.leftClick(700, 228);
                    break;
                case 4:
                    mouse.leftClick(574, 264);
                    break;
                case 5:
                    mouse.leftClick(616, 264);
                    break;
                case 6:
                    mouse.leftClick(658, 264);
                    break;
                case 7:
                    mouse.leftClick(700, 264);
                    break;
                case 8:
                    mouse.leftClick(574, 305);
                    break;
                case 9:
                    mouse.leftClick(616, 305);
                    break;
                case 10:
                    mouse.leftClick(658, 305);
                    break;
                case 11:
                    mouse.leftClick(700, 305);
                    break;
                case 12:
                    mouse.leftClick(574, 337);
                    break;
                case 13:
                    mouse.leftClick(10, 10);
                    MessageBox.Show("Out of pots");
                    break;
                default:
                    mouse.leftClick(574, 337);
                    break;
            }
            while (mouse.busy)
            {
                await Task.Delay(10);
            }
            await Task.Delay(50);
            mouse.busy = true;
            if (potion != 12)
            {
                mouse.leftClick(618, 334);
            }
            lightFire();
            potion++;
        }

        public async void lightFire()
        {
            if (potion == 12)
            {
                await Task.Delay(600);
                if(pos == 1)
                {
                    mouse.leftClick(130, 218);
                }
                if (pos == 2)
                {
                    mouse.leftClick(137, 174);
                }
                if (pos == 3)
                {
                    mouse.leftClick(137, 174);
                }
                if (pos == 4)
                {
                    mouse.leftClick(138, 115);
                }
                doBanking();
                return;
            }
            else
            {

                while (!inter())
                {
                    await Task.Delay(10);
                }
                Peripherals.PressKey((byte)Keys.Space, 1);
                while (mouse.busy)
                {
                    await Task.Delay(10);
                }
                mouse.busy = true;
                switch (log)
                {
                    case 0:
                        mouse.leftClick(574, 371);
                        break;
                    case 1:
                        mouse.leftClick(616, 371);
                        break;
                    case 2:
                        mouse.leftClick(658, 371);
                        break;
                    case 3:
                        mouse.leftClick(700, 371);
                        break;
                    case 4:
                        mouse.leftClick(574, 409);
                        break;
                    case 5:
                        mouse.leftClick(616, 409);
                        break;
                    case 6:
                        mouse.leftClick(658, 409);
                        break;
                    case 7:
                        mouse.leftClick(700, 409);
                        break;
                    case 8:
                        mouse.leftClick(574, 443);
                        break;
                    case 9:
                        mouse.leftClick(616, 443);
                        break;
                    case 10:
                        mouse.leftClick(658, 443);
                        break;
                    case 11:
                        mouse.leftClick(700, 443);
                        break;
                    default:
                        mouse.leftClick(10, 10);
                        MessageBox.Show("Out of logss");
                        break;
                }
                log++;
                while (mouse.busy)
                {
                    await Task.Delay(10);
                }
                mouse.busy = true;
                mouse.leftClick(662, 333);
                waitForS();
            }
        }

        public async void waitForS()
        {
            if(xpDrop())
            {
                await Task.Delay(50);
                makePotion();
            }
            else
            {
                await Task.Delay(10);
                waitForS();
            }
        }

        public async void doBanking()
        {
            while (!bankOpen())
            {
                await Task.Delay(10);
            }
            mouse.busy = true;
            mouse.leftClick(577, 336);
            while(mouse.busy)
            {
                await Task.Delay(10);
            }
            mouse.busy = true;
            mouse.leftClick(704, 337);
            while (mouse.busy)
            {
                await Task.Delay(10);
            }
            mouse.busy = true;
            mouse.leftClick(425, 263);
            while (mouse.busy)
            {
                await Task.Delay(10);
            }
            mouse.busy = true;
            mouse.leftClick(422, 224);
            while (mouse.busy)
            {
                await Task.Delay(10);
            }
            if(pos != 4)
            {
                log = 0;
                potion = 0;
                mouse.leftClick(687, 83);
                pos++;
                await Task.Delay(5000);
                startScript();
            } else
            {
                log = 0;
                potion = 0;
                mouse.leftClick(685, 69);
                pos = 1;
                await Task.Delay(5000);
                startScript();
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

        public bool xpDrop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 465, gameScreenCoords[1] + 127, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 79 && bitmap.GetPixel(i, j).R == 221 && bitmap.GetPixel(i, j).B == 1)
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
