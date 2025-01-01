using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core
{
    internal class XpDrops
    {
        public int[] clientCoords = new int[2];

        public bool gettingXp()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(18, 160))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 494, clientCoords[1] + 39, 0, 0, new Size(18, 160));
                }
                for (int x = 0; x < 18; x++)
                {
                    for (int y = 0; y < 160; y++)
                    {
                        if (bitmap.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
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
