using Bot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts
{
    internal class Baba
    {
        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;

        public int[] clientCoords = new int[2];

        public async void startScript()
        {
            clickTeleportCrystal();
        }

        public async void clickTeleportCrystal()
        {
            processor.addMouseClick(256, 144, "gamescreen");
            await Task.Delay(100);
            while (!insideBabaLair())
            {
                await Task.Delay(100);
                Console.WriteLine("Waiting to be inside baba lair.");
            }
            processor.addMouseClick(283, 193, "gamescreen");
            waitForBaba();
        }

        public async void waitForBaba()
        {
            while(!babaEastSetupEntr())
            {
                await Task.Delay(100);
            }
            processor.addMouseClick(282, 172, "gamescreen");
            while (!babaNorthSetupEntr())
            {
                await Task.Delay(100);
            }
            waitForAttack();
        }

        public async void waitForAttack()
        {
            Console.WriteLine("I got here!");
        }

        public bool insideBabaLair()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 634, clientCoords[1] + 124, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 220 && bitmap.GetPixel(x, y).G == 0)
                        {
                            return true;
                        }
                        Console.WriteLine("R: " + bitmap.GetPixel(x, y).R.ToString() + " G: " + bitmap.GetPixel(x, y).G.ToString() + " B: " + bitmap.GetPixel(x, y).B.ToString());
                    }
                }
            }
            return false;
        }

        public bool babaEastSetupEntr()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 629, clientCoords[1] + 58, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool babaNorthSetupEntr()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(2, 2))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 625, clientCoords[1] + 71, 0, 0, new Size(2, 2));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 0 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
