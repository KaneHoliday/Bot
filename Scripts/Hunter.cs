using Bot.Core;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts
{

    internal class Hunter
    {

        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;

        public int[] clientCoords = new int[2];

        public int xpGained;

        public int seconds = 0;
        public int minutes = 0;
        public int hours = 0;

        public int monkeysCaught = 0;
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
            Console.WriteLine($"Monkeys Caught: {monkeysCaught.ToString()}");
            Console.WriteLine("Xp gained: " + xpGained.ToString());
            await Task.Delay(1000);
            updateConsole();
        }

        public void startScript()
        {
            startTime();
            waitForOrange();
            updateConsole();
        }
        public async void waitForOrange()
        {
            while(orange() || yellow())
            {
                while (inventory.hasItem("Monkey tail"))
                {
                    inventory.clickItem("Monkey tail");
                    Console.WriteLine("Dropping monkey tail");
                    await Task.Delay(600);
                }
                if (!inventory.hasItem("Banana"))
                {
                    inventory.clickItem("Banana basket");
                }
                await Task.Delay(600);
            }
            if(green())
            {
                Console.WriteLine("Green");
                processor.addMouseClick(clientCoords[0] + 280, clientCoords[1] + 31);
                Console.WriteLine("Clicking green rock");
                monkeysCaught++;
                xpGained += 1000;
                while (green())
                {
                    await Task.Delay(100);
                }
                await Task.Delay(100);
                if (inventory.hasItem("Banana"))
                {
                    processor.addMouseClick(clientCoords[0] + 280, clientCoords[1] + 31);
                    Console.WriteLine("Setting up trap 1");
                    while (!yellow())
                    {
                        await Task.Delay(100);
                    }
                    waitForOrange();
                    return;
                } else
                {
                    Console.WriteLine("No banana basket");
                    //end script
                }
            } else
            {
                if (inventory.hasItem("Banana"))
                {
                    processor.addMouseClick(clientCoords[0] + 280, clientCoords[1] + 31);
                    Console.WriteLine("Setting up trap 1");
                    while (!yellow())
                    {
                        await Task.Delay(100);
                    }
                    waitForOrange();
                    return;
                }
                else
                {
                    Console.WriteLine("No banana basket");
                    //end script
                }
            }
        }

        public bool orange()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 340, clientCoords[1] + 105, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 210 && bitmap.GetPixel(i, j).G >= 170 && bitmap.GetPixel(i, j).G <= 210 && bitmap.GetPixel(i, j).B <= 20)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool yellow()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 340, clientCoords[1] + 105, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 240 && bitmap.GetPixel(i, j).G >= 170 && bitmap.GetPixel(i, j).G >= 240 && bitmap.GetPixel(i, j).B <= 10)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool green()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(30, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 340, clientCoords[1] + 105, 0, 0, new Size(30, 30));
                }

                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 15 && bitmap.GetPixel(i, j).G >= 220 && bitmap.GetPixel(i, j).B <= 10)
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
