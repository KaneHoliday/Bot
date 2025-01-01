using Bot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts
{
    internal class CookingGuild
    {

        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;

        public int[] clientCoords = new int[2];
        public int profit = 0;
        public int anglersCooked = 0;
        public int xpGained = 0;
        public int seconds;
        public int minutes;
        public int hours;

        public void startScript()
        {
            withdrawAnglers();
            updateConsole();
            startTime();
        }

        public async void updateConsole()
        {
            Console.Clear();
            if (seconds < 10)
            {
                Console.WriteLine("Time running: " + hours.ToString() + ":" + minutes.ToString() + ":0" + seconds.ToString());
            }
            if (seconds > 9)
            {
                Console.WriteLine("Time running: " + hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString());
            }
            Console.WriteLine("Anglers cooked: " + anglersCooked.ToString());
            Console.WriteLine("Xp gained: " + xpGained.ToString());
            Console.WriteLine("Profit made: " + profit.ToString());
            await Task.Delay(1000);
            updateConsole();
        }

        public async void withdrawAnglers()
        {
            if (bankOpen())
            {
                processor.addMouseClick(442, 309);
                await Task.Delay(100);
                processor.addMouseClick(229, 132);
                await Task.Delay(600);
                processor.PressKey((byte)Keys.Escape, 1);
                await Task.Delay(100);
                processor.addMouseClick(308, 314);
                waitForChatbox();
            }
            else
            {
                await Task.Delay(100);
                withdrawAnglers();
            }
        }

        public async void waitForChatbox()
        {
            if(chatBox())
            {
                processor.PressKey((byte)Keys.Space, 1);
                waitForFinish();
            } else
            {
                await Task.Delay(100);
                waitForChatbox();
            }
        }

        public async void waitForFinish()
        {
            if (!hasAnglers())
            {
                processor.addMouseClick(212, 43);
                withdrawAnglers();
                profit += 8000;
                anglersCooked += 28;
                xpGained += 6440;
            } else
            {
                await Task.Delay(1000);
                waitForFinish();
            }
        }

        public async void startTime()
        {
            await Task.Delay(1000);
            seconds++;
            if (seconds > 59)
            {
                seconds = 0;
                minutes++;
            }
            if (minutes > 59)
            {
                minutes = 0;
                hours++;
            }
            startTime();
        }

        public bool bankOpen()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 53, clientCoords[1] + 19, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 152 && bitmap.GetPixel(x, y).B == 31)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool hasAnglers()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(170, 250))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 556, clientCoords[1] + 209, 0, 0, new Size(170, 250));
                }
                for (int x = 0; x < 170; x++)
                {
                    for (int y = 0; y < 250; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 39 && bitmap.GetPixel(x, y).G == 153 && bitmap.GetPixel(x, y).B == 87)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool chatBox()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 483, clientCoords[1] + 359, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == 255 && bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
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
