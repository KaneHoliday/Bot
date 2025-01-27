using Bot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Console.WriteLine("starting angs");
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
            Console.WriteLine("Withdraw anglers");
            if (bankOpen())
            {
                Console.WriteLine("Bank open");
                processor.addMouseClick(442, 309, "gamescreen");
                await Task.Delay(600);
                processor.addMouseClick(229, 132, "gamescreen");
                await Task.Delay(600);
                processor.addMouseClick(484, 19, "gamescreen");
                await Task.Delay(600);
                processor.addMouseClick(308, 314);
                waitForChatbox();
            }
            else
            {
                Console.WriteLine("Bank not open");
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
            return checkColor(5, 5, 53, 19, 255, 152, 31);
        }

        public bool hasAnglers()
        {
            return checkColor(170, 250, 556, 209, 39, 153, 87);
        }

        public bool chatBox()
        {
            return checkColor(5, 5, 483, 359, 255, 255, 255);
        }
        public bool checkColor(int a, int b, int posX, int posY, int red, int green, int blue)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(a, b))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + posX, clientCoords[1] + posY, 0, 0, new Size(a, b));
                }

                for (int x = 0; x < a; x++)
                {
                    for (int y = 0; y < b; y++)
                    {
                        if (bitmap.GetPixel(x, y).R == red && bitmap.GetPixel(x, y).G == green && bitmap.GetPixel(x, y).B == blue)
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
