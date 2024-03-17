using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp
{
    internal class Hitpoints
    {
        public int hp;
        public int[] hpCoords = new int[2];
        public int[] hpSize = new int[2] {15, 11};

        public int counter = 0;



        public async void setCoords(int x, int y)
        {
            hpCoords[0] = x;
            hpCoords[1] = y;
            record();
        }

        public void captureHp()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(761, 498))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(hpCoords[0], hpCoords[1], 0, 0, new Size(761, 4980));
                }
                bitmap.Save("D:/Data/Gamescreen/screen" + counter + ".png", ImageFormat.Png);
                counter++;
            }
        }
        public async void record()
        {
            captureHp();
            await Task.Delay(600);
            record();
        }

        public int getHp()
        {
            return hp;
        }

    }
}