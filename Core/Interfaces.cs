using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot.Core
{
    internal class Interfaces
    {

        public int[] clientCoords = new int[2];

        public bool gotrStarted()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(3,3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 12, clientCoords[1] + 31, 0, 0, new Size(3,3));
                }
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 240 && bitmap.GetPixel(x, y).G > 220 && bitmap.GetPixel(x, y).B > 130)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool inventoryFull()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(149, 14))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 184, clientCoords[1] + 440, 0, 0, new Size(149, 14));
                }
                for (int x = 0; x < 149; x++)
                {
                    for (int y = 0; y < 14; y++)
                    {
                        if (bitmap.GetPixel(x, y).B == 255)
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
