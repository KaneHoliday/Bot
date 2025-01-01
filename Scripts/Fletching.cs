using Bot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts
{
    internal class Fletching
    {

        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;

        public int[] clientCoords = new int[2];
        public int profit = 0;
        public int logsFletched = 0;
        public int xpGained = 0;
        public int seconds;
        public int minutes;
        public int hours;

        public void startScript()
        {
            withdrawLogs();
            updateConsole();
            startTime();
        }
        public async void withdrawLogs()
        {
            if (bankOpen())
            {
                processor.addMouseClick(617, 225);
                await Task.Delay(100);
                processor.addMouseClick(325, 131);
                await Task.Delay(600);
                processor.PressKey((byte)Keys.Escape, 1);
                while (!hasYewLogs())
                {
                    await Task.Delay(100);
                }
                await Task.Delay(100);
                processor.addMouseClick(574, 225);
                await Task.Delay(100);
                processor.addMouseClick(619, 226);
                waitForChatbox();
            }
            else
            {
                await Task.Delay(100);
                withdrawLogs();
            }
        }

        public async void waitForChatbox()
        {
            if (chatBox())
            {
                processor.PressKey((byte)Keys.Space, 1);
                waitForFinish();
            }
            else
            {
                await Task.Delay(100);
                waitForChatbox();
            }
        }

        public async void waitForFinish()
        {
            if (!hasYewLogs())
            {
                processor.addMouseClick(305, 121);
                withdrawLogs();
                profit += 1300;
                logsFletched += 27;
                xpGained += 2025;
            }
            else
            {
                await Task.Delay(1000);
                waitForFinish();
            }
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

        public bool hasYewLogs()
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
                        if (bitmap.GetPixel(x, y).R == 50 && bitmap.GetPixel(x, y).G == 34 && bitmap.GetPixel(x, y).B == 3)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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
            Console.WriteLine("Logs fletched: " + logsFletched.ToString());
            Console.WriteLine("Xp gained: " + xpGained.ToString());
            Console.WriteLine("Profit made: " + profit.ToString());
            await Task.Delay(1000);
            updateConsole();
        }
    }
}
