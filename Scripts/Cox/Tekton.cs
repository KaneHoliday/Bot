using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts.Cox
{
    internal class Tekton
    {

        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public bool first = true;

        Player player = new Player();
        Inventory inv = new Inventory();
        Interfaces interfaces = new Interfaces();
        Peripherals peripherals = new Peripherals();


        public void startScript()
        {
            peripherals.leftClick(256, 105);
            waitForLure();
        }

        public async void waitForLure()
        {
            if (atLureSpot())
            {
                await Task.Delay(600);
                peripherals.leftClick(256, 255);
                waitForTekton();
            }
            else
            {
                await Task.Delay(100);
                waitForLure();
            }
        }

        public async void waitForTekton()
        {
            if(nextToTekton())
            {
                if (first)
                {
                    peripherals.leftClick(262, 190);
                    first = false;
                } else
                {
                    await Task.Delay(100);
                }
                waitForAttack();
            } else
            {
                await Task.Delay(100);
                waitForTekton();
            }
        }

        public async void waitForAttack()
        {
            if (awayFromTekton())
            {
                peripherals.leftClick(233, 117);
                await Task.Delay(600);
                peripherals.leftClick(258, 184);
                waitForTekton();
            }
            else
            {
                await Task.Delay(100);
                waitForAttack();
            }
        }

        public bool atLureSpot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(673, 65).R >= 240 && bitmap.GetPixel(673, 65).G == 0 && bitmap.GetPixel(673, 65).B == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool nextToTekton()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(632, 68).R >= 245 && bitmap.GetPixel(632, 68).G >= 245)
                {
                    return true;
                }
            }
            return false;
        }

        public bool awayFromTekton()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(632, 64).R >= 245 && bitmap.GetPixel(632, 64).G >= 245)
                {
                    return true;
                }
            }
            return false;
        }


        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            player.setCoords(x, y);
            //bank.setCoords(x, y);
            interfaces.setCoords(x, y);
            peripherals.setCoords(x, y);
        }

    }
}
