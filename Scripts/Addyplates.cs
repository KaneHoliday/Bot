using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Addyplates
    {
        public Player player = new Player();
        Peripherals peripherals = new Peripherals();
        Interfaces interfaces = new Interfaces();

        public int[] gameScreenCoords = new int[2] { 0, 0 };


        public void startScript()
        {
            walkToBank();
        }

        public async void walkToBank()
        {
            if(noBars())
            {
                peripherals.leftClick(354, 119);
                doBank();
            }
            else
            {
                await Task.Delay(100);
                walkToBank();
            }
        }

        public async void doBank()
        {
            if (bankOpen())
            {
                peripherals.leftClick(618, 226);
                await Task.Delay(300);
                peripherals.leftClick(421, 96);
                await Task.Delay(300);
                Peripherals.PressKey((byte)Keys.Escape, 1);
                await Task.Delay(1000);
                peripherals.leftClick(152, 238);
                waitForSmith();
            } else
            {
                await Task.Delay(100);
                doBank();
            }
        }

        public async void waitForSmith()
        {
            if (smithingInter())
            {
                Peripherals.PressKey((byte)Keys.Space, 1);
                walkToBank();
            }
            else
            {
                await Task.Delay(100);
                waitForSmith();
            }

        }
        public bool noBars()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 557, gameScreenCoords[1] + 428, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 39 && bitmap.GetPixel(i, j).G == 50 && bitmap.GetPixel(i, j).B == 39)
                        {
                            return false;
                        }
                        if(bitmap.GetPixel(i, j).R == 29 && bitmap.GetPixel(i, j).G == 31 && bitmap.GetPixel(i, j).B == 23)
                        {
                            return false;
                        }
                        if (bitmap.GetPixel(i, j).R == 23 && bitmap.GetPixel(i, j).G == 31 && bitmap.GetPixel(i, j).B == 23)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool bankOpen()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 173, gameScreenCoords[1] + 14, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 152 && bitmap.GetPixel(i, j).B == 31)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool smithingInter()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 159, gameScreenCoords[1] + 18, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 152 && bitmap.GetPixel(i, j).B == 31)
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
