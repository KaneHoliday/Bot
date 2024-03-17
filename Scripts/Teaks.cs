using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Teaks
    {
        Inventory inv = new Inventory();
        public Player player = new Player();
        Mouse mouse = new Mouse();
        Interfaces interfaces = new Interfaces();

        public int[] gameScreenCoords = new int[2] { 0, 0 };
        public bool spec;
        
        public void startScript()
        {
            mouse.initMouse();
            mouse.mouseClickLoop();
            mouse.addMouseClick(gameScreenCoords[0] + 153, gameScreenCoords[1] + 190);
            waitForLeftDead();
        }

        public async void waitForLeftDead()
        {
            if(leftTreeDead())
            {
                mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 226);
                mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 226);
                mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 226);
                mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 226);
                mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 261);
                mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 261);
                mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 261);
                mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 261);
                mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 296);
                mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 296);
                mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 296);
                mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 296);
                if (checkSpec() && spec)
                {
                    mouse.addMouseClick(gameScreenCoords[0] + 585, gameScreenCoords[1] + 144);
                }
                mouse.addMouseClick(gameScreenCoords[0] + 378, gameScreenCoords[1] + 75);
                waitForRightDead();
            } else
            {
                await Task.Delay(100);
                waitForLeftDead();
            }
        }

        public async void waitForRightDead()
        {
            if (rightTreeDead())
            {
                mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 226);
                mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 226);
                mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 226);
                mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 226);
                mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 261);
                mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 261);
                mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 261);
                mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 261);
                mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 296);
                mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 296);
                mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 296);
                mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 296);
                if (checkSpec() && spec)
                {
                    mouse.addMouseClick(gameScreenCoords[0] + 585, gameScreenCoords[1] + 144);
                }
                mouse.addMouseClick(gameScreenCoords[0] + 133, gameScreenCoords[1] + 51);
                waitForLeftDead();
            }
            else
            {
                await Task.Delay(100);
                waitForRightDead();
            }
        }

        public void dropInventory()
        {
            mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 226);
            mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 226);
            mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 226);
            mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 226);
            mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 261);
            mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 261);
            mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 261);
            mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 261);
            mouse.addMouseClick(gameScreenCoords[0] + 576, gameScreenCoords[1] + 296);
            mouse.addMouseClick(gameScreenCoords[0] + 621, gameScreenCoords[1] + 296);
            mouse.addMouseClick(gameScreenCoords[0] + 666, gameScreenCoords[1] + 296);
            mouse.addMouseClick(gameScreenCoords[0] + 711, gameScreenCoords[1] + 296);
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
                            if (bitmap.GetPixel(i, j).R == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;

            }
        }

        public bool leftTreeDead()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 149, gameScreenCoords[1] + 189, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 255 && bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool rightTreeDead()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0]+351, gameScreenCoords[1] + 200, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 255 && bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).B == 0)
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
