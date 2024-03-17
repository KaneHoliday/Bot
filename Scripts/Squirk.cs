using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Squirk
    {
        Inventory inv = new Inventory();
        public Player player = new Player();
        Peripherals mouse = new Peripherals();
        Interfaces interfaces = new Interfaces();

        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public async void startScript()
        {
            if(gotCaught())
            {
                await Task.Delay(600);
                mouse.leftClick(255, 144);
                waitForCollect();
            } else
            if(atStart())
            {
                if (checkStam())
                {
                    inv.clickInventory("Stamina potion");
                    await Task.Delay(600);
                }
                await Task.Delay(1000);
                mouse.leftClick(246, 66);
                waitForCollect();
            } else
            {
                await Task.Delay(100);
                startScript();
            }
        }

        public async void waitForCollect()
        {
            if(inGate())
            {
                mouse.leftClick(313, 65);
                startScript();
            } else
            {
                await Task.Delay(100);
                waitForCollect();
            }
        }

        public bool atStart()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(726, 158))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(726, 158));
                }

                if (bitmap.GetPixel(605, 77).R <= 254 && bitmap.GetPixel(605, 77).R >= 225 && bitmap.GetPixel(605, 77).G == 0 && bitmap.GetPixel(605, 77).B == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool inGate()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(726, 158))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(726, 158));
                }

                if (bitmap.GetPixel(680, 109).R <= 254 && bitmap.GetPixel(680, 109).R >= 225 && bitmap.GetPixel(680, 109).G == 0 && bitmap.GetPixel(680, 109).B == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool gotCaught()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(726, 158))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(726, 158));
                }

                if (bitmap.GetPixel(649, 140).R <= 254 && bitmap.GetPixel(649, 140).R >= 225 && bitmap.GetPixel(649, 140).G == 0 && bitmap.GetPixel(649, 140).B == 0)
                {
                    if (bitmap.GetPixel(609, 109).R <= 254 && bitmap.GetPixel(609, 109).R >= 225 && bitmap.GetPixel(609, 109).G == 0 && bitmap.GetPixel(609, 109).B == 0)
                    {
                        if (bitmap.GetPixel(680, 97).R <= 254 && bitmap.GetPixel(680, 97).R >= 225 && bitmap.GetPixel(680, 97).G == 0 && bitmap.GetPixel(680, 97).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public bool checkStam()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 531, gameScreenCoords[1] + 120, 0, 0, new Size(10, 10));
                }

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 100)
                        {
                            if (bitmap.GetPixel(i, j).R == 255)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;

            }
        }

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            player.setCoords(x, y);
            interfaces.setCoords(x, y);
            mouse.setCoords(x, y);
            inv.setCoords(x, y);
        }
    }
}
