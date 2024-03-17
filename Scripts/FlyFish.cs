using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class FlyFish
    {


        Inventory inv = new Inventory();
        public Player player = new Player();
        Peripherals peripherals = new Peripherals();
        Interfaces interfaces = new Interfaces();

        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public int fish = 0;
        public bool spec;


        public async void startScript()
        {
            doFish();
        }

        public async void doFish()
        {
            if(fish == 5)
            {
                drop();
                await Task.Delay(700);
                fish = 0;
            }
            tick();
            await Task.Delay(300);
            peripherals.busy = true;
            peripherals.leftClick(369, 197);
            while(peripherals.busy)
            {
                await Task.Delay(10);
            }
            waitForXp();
        }

        public async void waitForXp()
        {
            if(xpDrop())
            {
                fish++;
                doFish();
            } else
            {
                await Task.Delay(100);
                waitForXp();
            }
        }


        public async void tick()
        {
            peripherals.busy = true;
            peripherals.fastLeftClick(574, 229);
            while (peripherals.busy)
            {
                await Task.Delay(10);
            }
            peripherals.busy = true;
            peripherals.fastLeftClick(575, 261);
            while (peripherals.busy)
            {
                await Task.Delay(10);
            }
        }


        public async void drop()
        {
            peripherals.busy = true;
            peripherals.fastLeftClick(616, 258);
            while (peripherals.busy)
            {
                await Task.Delay(10);
            }
            peripherals.busy = true;
            peripherals.fastLeftClick(660, 223);
            while (peripherals.busy)
            {
                await Task.Delay(10);
            }
            peripherals.busy = true;
            peripherals.fastLeftClick(706, 224);
            while (peripherals.busy)
            {
                await Task.Delay(10);
            }
            peripherals.busy = true;
            peripherals.fastLeftClick(672, 258);
            while (peripherals.busy)
            {
                await Task.Delay(10);
            }
            peripherals.busy = true;
            peripherals.fastLeftClick(709, 262);
            while (peripherals.busy)
            {
                await Task.Delay(10);
            }
            if (checkSpec() && spec)
            {
                peripherals.busy = true;
                peripherals.fastLeftClick(585, 144);
                while (peripherals.busy)
                {
                    await Task.Delay(10);
                }
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

        public bool xpDrop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(27, 67))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 485, gameScreenCoords[1] + 85, 0, 0, new Size(27, 67));
                }

                for (int i = 0; i < 27; i++)
                {
                    for (int j = 0; j < 67; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 255 && bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).B == 255)
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
            peripherals.setCoords(x, y);
        }

    }

}
