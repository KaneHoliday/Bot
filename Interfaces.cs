using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp
{
    internal class Interfaces
    {

        public int[] gameScreenCoords = new int[2] { 0, 0 };

        Peripherals peripherals = new Peripherals();

        public bool toaPartyList()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(15, 15))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 161, gameScreenCoords[1] + 16, 0, 0, new Size(15, 15));
                }

                for (int i = 0; i < 14; i++)
                {
                    for (int j = 0; j < 14; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void rotateCamera(string direction)
        {
            peripherals.rightClick(558, 17);

            await Task.Delay(300);

            switch(direction)
            {
                case "north":
                    peripherals.leftClick(558, 44);
                    await Task.Delay(300);
                    Peripherals.HoldKey((byte)Keys.Up, 10);
                    break;
                case "south":
                    peripherals.leftClick(558, 75);
                    await Task.Delay(300);
                    Peripherals.HoldKey((byte)Keys.Up, 10);
                    break;
                case "east":
                    peripherals.leftClick(558, 60);
                    await Task.Delay(300);
                    Peripherals.HoldKey((byte)Keys.Up, 10);
                    break;
                case "west":
                    peripherals.leftClick(558, 88);
                    await Task.Delay(300);
                    Peripherals.HoldKey((byte)Keys.Up, 10);
                    break;
            }
        }

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            peripherals.setCoords(x, y);
        }
    }
}
