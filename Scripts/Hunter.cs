using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Hunter
    {

        Player player = new Player();
        Mouse mouse = new Mouse();
        Interfaces interfaces = new Interfaces();

        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public void startScript()
        {
            mouse.initMouse();
            mouse.mouseClickLoop();
            waitForOrange();
        }

        public async void waitForOrange()
        {
            if(orange())
            {
                await Task.Delay(100);
                waitForGreen();
            } else
            {
                await Task.Delay(100);
                waitForOrange();
            }
        }

        public async void waitForGreen()
        {
            if(!orange())
            {
                if(green())
                {
                    mouse.addMouseClick(gameScreenCoords[0] + 280, gameScreenCoords[1] + 31);
                    while(green()) {
                        await Task.Delay(100);
                    }
                    mouse.addMouseClick(gameScreenCoords[0] + 280, gameScreenCoords[1] + 31);
                    waitForOrange();
                } else
                {
                    mouse.addMouseClick(gameScreenCoords[0] + 280, gameScreenCoords[1] + 31);
                    waitForOrange();
                }
            } else
            {
                await Task.Delay(100);
                waitForGreen();
            }
        }


        public bool orange()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 372, gameScreenCoords[1] + 92, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 240 && bitmap.GetPixel(i, j).G >= 190 && bitmap.GetPixel(i, j).G <= 210 && bitmap.GetPixel(i, j).B <= 10)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool green()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 372, gameScreenCoords[1] + 92, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 15 && bitmap.GetPixel(i, j).G >= 220 && bitmap.GetPixel(i, j).B <= 10)
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
        }
    }
}
