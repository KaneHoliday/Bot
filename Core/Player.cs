using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core
{
    internal class Player
    {
        public CoreProcessor processor;
        public float health = 50;
        public float prayer;
        public float run;
        public bool isRunning;
        public bool isPoisoned;
        public bool isVenomed;
        public int[] clientCoords = new int[2];

        public void updateHealth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(15, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 521, clientCoords[1] + 54, 0, 0, new Size(15, 10));
                }
                for (int x = 0; x < 15; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            Console.WriteLine("Setting health");

                            health = 99 - (bitmap.GetPixel(x, y).R) / 5;
                            return;
                        }
                        if (bitmap.GetPixel(x, y).G <= 255 && bitmap.GetPixel(x, y).B == 0)
                        {
                            Console.WriteLine("Setting health");
                            health = 50 - (255 - bitmap.GetPixel(x, y).G)/5;
                            return;
                        }
                    }
                }
            }
        }
        public float playerHealth()
        {
            return health;
        }
        public float playerPrayer()
        {
            return prayer;
        }
        public float playerRun()
        {
            return run;
        }
        public bool runStatus()
        {
            return isRunning;
        }
        public bool poisoned()
        {

            return isPoisoned;
        }
        public bool venomed()
        {
            return isVenomed;
        }
    }
}
