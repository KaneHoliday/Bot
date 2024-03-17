using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp
{
    internal class Player
    {

        public float hpPercent;
        public float prayerPercent;
        public int[] gameScreenCoords = new int[2];


        public void setHp()
        {
            int tempx;
            int tempy;
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(14, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 521, gameScreenCoords[1] + 54, 0, 0, new Size(14, 10));
                }

                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bitmap.GetPixel(i, j).B == 0 && bitmap.GetPixel(i, j).G != 0)
                        {
                            tempx = bitmap.GetPixel(i, j).G;
                            tempy = 255 - bitmap.GetPixel(i, j).R;

                            hpPercent = (tempx + (float)tempy) / 510 * 100;
                        }
                    }
                }

            }
        }

        public void setPrayer()
        {
            int tempx;
            int tempy;
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(14, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 521, gameScreenCoords[1] + 88, 0, 0, new Size(14, 10));
                }

                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bitmap.GetPixel(i, j).B == 0 && bitmap.GetPixel(i, j).G != 0)
                        {
                            tempx = bitmap.GetPixel(i, j).G;
                            tempy = 255 - bitmap.GetPixel(i, j).R;

                            prayerPercent = (tempx + (float)tempy) / 510 * 100;
                        }
                    }
                }

            }
        }

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
        }


        public float getHp()
        {
            return hpPercent;
        }

        public float getPrayer()
        {
            return prayerPercent;
        }
    }
}
