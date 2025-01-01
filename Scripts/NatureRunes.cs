using Bot.Core;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts
{

    internal class NatureRunes
    {

        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;

        public int[] clientCoords = new int[2];

        public int naturesCrafted = 0;
        public int xpGained = 0;
        public int profit = 0;

        public bool repairPouch = false;

        public int bankPos = 0;
        public int ladderPos = 0;

        public int ladX;
        public int ladY;

        public bool stam = true;

        public int seconds = 0;
        public int minutes = 0;
        public int hours = 0;

        public void startScript()
        {
            updateConsole();
            startTime();
            if (bankOpen())
            {
                doBanking();
            }
        }

        public async void startTime()
        {
            await Task.Delay(1000);
            seconds++;
            if(seconds > 59)
            {
                seconds = 0;
                minutes++;
            }
            if(minutes > 59)
            {
                minutes = 0;
                hours++;
            }
            startTime();
        }

        public async void updateConsole()
        {
            Console.Clear();
            if(seconds < 10)
            {
                Console.WriteLine("Time running: " + hours.ToString() + ":" + minutes.ToString() + ":0" + seconds.ToString());
            }
            if(seconds > 9)
            {
                Console.WriteLine("Time running: " + hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString());
            }
            Console.WriteLine("Natures crafted: " + naturesCrafted.ToString());
            Console.WriteLine("Xp gained: " + xpGained.ToString());
            Console.WriteLine("Profit made: " + profit.ToString());
            await Task.Delay(1000);
            updateConsole();
        }

        public bool pouchDeg()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(37, 32))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 559, clientCoords[1] + 211, 0, 0, new Size(37, 32));
                }

                for (int i = 0; i < 37; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 34 && bitmap.GetPixel(i, j).G == 31 && bitmap.GetPixel(i, j).B == 30)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async void repairPouches()
        {
                processor.PressKey((byte)Keys.F3, 1);
                await Task.Delay(100);
                processor.rightClick(558, 239);
                await Task.Delay(150);
                processor.addMouseClick(552, 281);
                await Task.Delay(150);
                waitForChat();
        }


        public async void waitForChat()
        {
            if (npcTalk())
            {
                processor.PressKey((byte)Keys.F1, 1);
                await Task.Delay(100);
                processor.PressKey((byte)Keys.Space, 1);
                await Task.Delay(1200);
                processor.PressKey((byte)Keys.NumPad2, 1);
                await Task.Delay(1200);
                processor.PressKey((byte)Keys.Space, 1);
                await Task.Delay(1200);
                seeBank();
            }
            else
            {
                await Task.Delay(200);
                waitForChat();
            }
        }

        public bool npcTalk()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(58, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 262, clientCoords[1] + 356, 0, 0, new Size(58, 20));
                }

                for (int i = 0; i < 58; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).R == 128 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async void  seeBank()
        {
            bool clicked = false;
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(200, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 351, clientCoords[1] + 156, 0, 0, new Size(200, 200));
                }

                for (int i = 0; i < 200; i++)
                {
                    for (int j = 0; j < 200; j++)
                    {
                        if (bitmap.GetPixel(i, j).R > 253 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B > 253)
                        {
                            clicked = true;
                            processor.addMouseClick(351 + i + 3, 156 + j);
                            doBanking();
                            return;
                        }
                    }
                }
            }

            if(!clicked)
            {
                await Task.Delay(600);
                seeBank();
            }
            
        }

        public async void doBanking()
        {
            if (bankOpen())
            {
                processor.addMouseClick(618, 225, "gamescreen");
                await Task.Delay(200);
                if (stam)
                {
                    processor.addMouseClick(132, 207, "gamescreen");
                    await Task.Delay(200);
                }
                processor.addMouseClick(228, 238, "gamescreen");
                await Task.Delay(200);
                processor.addMouseClick(575, 226, "gamescreen");
                await Task.Delay(200);
                processor.addMouseClick(228, 238, "gamescreen");
                await Task.Delay(200);
                if (stam)
                {
                    await Task.Delay(600);
                    processor.addMouseClick(611, 224, "gamescreen");
                    stam = false;
                }
                processor.addMouseClick(575, 226, "gamescreen");
                await Task.Delay(200);
                processor.addMouseClick(228, 238, "gamescreen");
                await Task.Delay(2500);
                processor.PressKey((byte)Keys.Escape, 1);
                await Task.Delay(300);
                processor.PressKey((byte)Keys.F5, 1);
                await Task.Delay(100);
                processor.addMouseClick(597, 263, "gamescreen");
                waitForCapeInterface();
            } else
            {
                await Task.Delay(100);
                doBanking();
            }
        }

        public async void waitForCapeInterface()
        {
            if (capeInterface())
            {
                processor.PressKey((byte)Keys.NumPad7, 1);
                await Task.Delay(3000);
                ladPos();
            }
            else
            {
                await Task.Delay(100);
                waitForCapeInterface();
            }
        }

        public void clickChest()
        {
            switch(bankPos)
            {
                case 1:
                    processor.addMouseClick(407, 201, "gamescreen");
                    break;
                case 2:
                    processor.addMouseClick(392, 201, "gamescreen");
                    break;
                case 3:
                    processor.addMouseClick(403, 188, "gamescreen");
                    break;
                case 4:
                    processor.addMouseClick(387, 188, "gamescreen");
                    break;
                case 5:
                    processor.addMouseClick(375, 188, "gamescreen");
                    break;
                case 6:
                    processor.addMouseClick(386, 174, "gamescreen");
                    break;
                case 7:
                    processor.addMouseClick(371, 174, "gamescreen");
                    break;
                default:
                    break;

            }
            doBanking();
        }

        public async void agilityShortcut()
        {
            while (!leftOfLadder() && !rightOfLadder())
            {
                await Task.Delay(100);
            }
            if (leftOfLadder())
            {
                processor.addMouseClick(385, 88, "gamescreen");
                while (!throughShortcut())
                {
                    await Task.Delay(100);
                }
            }
            else if (rightOfLadder())
            {
                processor.addMouseClick(363, 88, "gamescreen");
                while (!throughShortcut())
                {
                    await Task.Delay(100);
                }
            }
            while (!greenShortcut() && !greenShortcut2())
            {
                await Task.Delay(100);
            }
            await Task.Delay(3000);
            processor.addMouseClick(236, 59, "gamescreen");
            processor.PressKey((byte)Keys.F1, 1);
            waitForInside();
        }

        public async void waitForInside()
        {
            if(naturesAltar())
            {
                if(pouchDeg())
                {
                    repairPouch = true;
                    stam = true;
                }
                processor.addMouseClick(262, 105, "gamescreen");
                while (!xpDrop())
                {
                    await Task.Delay(100);
                }
                processor.addMouseClick(575, 226, "gamescreen");
                processor.addMouseClick(256, 126, "gamescreen");
                await Task.Delay(1000);
                processor.addMouseClick(575, 226, "gamescreen");
                processor.addMouseClick(256, 126, "gamescreen");
                await Task.Delay(1000);
                processor.PressKey((byte)Keys.F5, 1);
                await Task.Delay(300);
                processor.addMouseClick(597, 263, "gamescreen");
                while (!capeInterface())
                {
                    await Task.Delay(10);
                }
                naturesCrafted += 100;
                profit += 11265;
                xpGained += 594;
                processor.PressKey((byte)Keys.NumPad2, 1);
                bankPos = 0;
                if (repairPouch)
                {
                    await Task.Delay(4000);
                    repairPouches();
                    repairPouch = false;
                }
                else
                {
                    await Task.Delay(2000);
                    seeBank();
                };
            } else
            {
                await Task.Delay(100);
                waitForInside();
            }
        }

        public bool capeInterface()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 310, clientCoords[1] + 52, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R < 55 && bitmap.GetPixel(x, y).G < 45 && bitmap.GetPixel(x, y).B < 10)
                        {
                            if (bitmap.GetPixel(x, y).R > 45 && bitmap.GetPixel(x, y).G > 35 && bitmap.GetPixel(x, y).B > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool greenShortcut()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(25, 25))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 250, clientCoords[1] + 176, 0, 0, new Size(15, 15));
                }
                for (int x = 0; x < 25; x++)
                {
                    for (int y = 0; y < 25; y++)
                    {
                        if (bitmap.GetPixel(x, y).G == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool xpDrop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(19, 37))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 492, clientCoords[1] + 123, 0, 0, new Size(19, 37));
                }
                for (int x = 0; x < 19; x++)
                {
                    for (int y = 0; y < 37; y++)
                    {
                        if (bitmap.GetPixel(x, y).G == 255 && bitmap.GetPixel(x, y).B == 255)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool greenShortcut2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(15, 15))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 246, clientCoords[1] + 207, 0, 0, new Size(15, 15));
                }
                for (int x = 0; x < 15; x++)
                {
                    for (int y = 0; y < 15; y++)
                    {
                        if (bitmap.GetPixel(x, y).G > 250)
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
        public bool rightOfLadder()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 631, clientCoords[1] + 41, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 200 && bitmap.GetPixel(x, y).G > 200 && bitmap.GetPixel(x, y).B > 200)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool leftOfLadder()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 639, clientCoords[1] + 41, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 200 && bitmap.GetPixel(x, y).G > 200 && bitmap.GetPixel(x, y).B > 200)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool throughShortcut()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 636, clientCoords[1] + 73, 0, 0, new Size(5, 5));
                }
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        if (bitmap.GetPixel(x, y).R < 5 && bitmap.GetPixel(x, y).G < 5 && bitmap.GetPixel(x, y).B < 5)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool naturesAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 248, clientCoords[1] + 176, 0, 0, new Size(20, 10));
                }
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 220 && bitmap.GetPixel(x, y).G > 225 && bitmap.GetPixel(x, y).B < 10)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async void ladPos()
        {
            bool clicked = false;
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(120, 90))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 230, clientCoords[1] + 109, 0, 0, new Size(120, 90));
                }
                for (int x = 0; x < 120; x++)
                {
                    for (int y = 0; y < 90; y++)
                    {
                        if (bitmap.GetPixel(x, y).R > 190 && bitmap.GetPixel(x, y).G > 180 && bitmap.GetPixel(x, y).B == 0)
                        {
                            processor.rightClick(230 + x + 8, 109 + y + 10);
                            await Task.Delay(300);
                            processor.addMouseClick(230 + x, 123 + y + 40);
                            clicked = true;
                            agilityShortcut();
                            return;
                        }
                    }
                }
                if(!clicked)
                {
                    await Task.Delay(100);
                    ladPos();
                }
            }
        }
    }

}
