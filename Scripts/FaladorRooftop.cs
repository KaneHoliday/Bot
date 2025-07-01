using Bot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts
{
    internal class FaladorRooftop
    {
        // Dependencies injected or initialized elsewhere.
        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;

        // Constants for game screen positions
        private const int courseXp = 6440;

        public int lapsCompleted = 0;
        public int xpGained = 0;

        public int[] clientCoords = new int[2];

        // Timer variables
        private int seconds;
        private int minutes;
        private int hours;

        // Task cancellation token
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Initiates the script execution.
        /// </summary>
        public async void startScript()
        {
            Console.WriteLine("Starting falador rooftop...");
            StartTimer();
            await StartLap();
            await UpdateConsoleAsync(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Updates console output periodically.
        /// </summary>
        private async Task UpdateConsoleAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Clear();
                Console.WriteLine($"Time running: {hours:D2}:{minutes:D2}:{seconds:D2}");
                Console.WriteLine($"Laps completed: {lapsCompleted}");
                Console.WriteLine($"XP gained: {xpGained}");
                await Task.Delay(1000, cancellationToken);
            }
        }

        /// <summary>
        /// Withdraws anglers from the bank if the bank is open.
        /// </summary>
        private async Task StartLap()
        {
            await Task.Delay(600);
            processor.addMouseClick(336, 78, "gamescreen"); // climb onto roof
            await climb1();
        }

        /// <summary>
        /// Waits for the chatbox to appear and starts cooking.
        /// </summary>
        private async Task climb1()
        {
            while (!pos1())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(308, 155, "gamescreen"); //cross tightrope
            await climb2();
        }

        private async Task climb2()
        {
            while (!pos2())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(291, 104, "gamescreen"); //cross handlebars
            await climb3();
        }

        private async Task climb3()
        {
            while (!pos3() && !pos14())
            {
                await Task.Delay(100);
            }
            if (pos3())
            {
                await Task.Delay(600);
                processor.addMouseClick(234, 143, "gamescreen");
                await climb4();
            } else if(pos14())
            {
                await Task.Delay(1200);
                processor.addMouseClick(580, 119, "gamescreen");
                await climb14();
            }
        }
        private async Task climb4()
        {
            while (!pos4())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(203, 163, "gamescreen");
            await climb5();
        }
        private async Task climb5()
        {
            while (!pos5())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(166, 159, "gamescreen");
            await climb6();
        }
        private async Task climb6()
        {
            while (!pos6())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(229, 181, "gamescreen");
            await climb7();
        }
        private async Task climb7()
        {
            while (!pos7())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(229, 169, "gamescreen");
            await climb8();
        }
        private async Task climb8()
        {
            while (!pos8())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(217, 215, "gamescreen");
            await climb9();
        }
        private async Task climb9()
        {
            while (!pos9())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(216, 202, "gamescreen");
            await climb10();
        }
        private async Task climb10()
        {
            while (!pos10())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(246, 289, "gamescreen");
            await climb11();
        }
        private async Task climb11()
        {
            while (!pos11())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(328, 159, "gamescreen");
            await climb12();
        }
        private async Task climb12()
        {
            while (!pos12())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(335, 165, "gamescreen");
            await climb13();
        }
        private async Task climb13()
        {
            while (!pos13())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(336, 78, "gamescreen");
            await climb1();
        }
        private async Task climb14()
        {
            while (!pos15())
            {
                await Task.Delay(100);
            }
            await Task.Delay(600);
            processor.addMouseClick(256, 160, "gamescreen");
            await StartLap();
        }

        /// <summary>
        /// Waits for cooking to finish and then repeats the process.
        /// </summary>

        /// <summary>
        /// Starts asynchronous timer to track elapsed time.
        /// </summary>
        private void StartTimer()
        {
            Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
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
                }
            }, cancellationTokenSource.Token);
        }

        /// <summary>
        /// Checks if the bank is open by verifying its color on screen.
        /// </summary>
        private bool pos1()
        {
            return CheckColor(5, 5, 660, 76, 96, 74, 14);
        }
        private bool pos2()
        {
            return CheckColor(5, 5, 613, 84, 96, 74, 14);
        }
        private bool pos3()
        {
            return CheckHigherColor(5, 5, 628, 118, 225, 0, 0);
        }
        private bool pos4()
        {
            return CheckHigherColor(5, 5, 636, 134, 225, 0, 0);
        }
        private bool pos5()
        {
            return CheckHigherColor(5, 5, 662, 131, 225, 0, 0);
        }
        private bool pos6()
        {
            return CheckHigherColor(5, 5, 592, 118, 225, 0, 0);
        }
        private bool pos7()
        {
            return CheckHigherColor(5, 5, 625, 114, 225, 0, 0);
        }
        private bool pos8()
        {
            return CheckHigherColor(5, 5, 635, 98, 225, 0, 0);
        }
        private bool pos9()
        {
            return CheckHigherColor(5, 5, 651, 86, 225, 0, 0);
        }
        private bool pos10()
        {
            return CheckHigherColor(5, 5, 655, 70, 225, 0, 0);
        }
        private bool pos11()
        {
            return CheckHigherColor(5, 5, 655, 34, 225, 0, 0);
        }
        private bool pos12()
        {
            return CheckHigherColor(5, 5, 628, 34, 225, 0, 0);
        }
        private bool pos13()
        {
            return CheckHigherColor(5, 5, 583, 82, 225, 0, 0);
        }
        private bool pos14()
        {
            return CheckHigherColor(5, 5, 627, 55, 225, 0, 0);
        }
        private bool pos15()
        {
            return CheckHigherColor(5, 5, 600, 66, 225, 0, 0);
        }

        /// <summary>
        /// Checks color of specified screen area for matching RGB values.
        /// </summary>
        private bool CheckColor(int width, int height, int posX, int posY, int red, int green, int blue)
        {
            using Bitmap bitmap = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(clientCoords[0] + posX, clientCoords[1] + posY, 0, 0, new Size(width, height));

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (pixel.R == red && pixel.G == green && pixel.B == blue)
                        return true;
                }
            }
            return false;
        }

        private bool CheckHigherColor(int width, int height, int posX, int posY, int red, int green, int blue)
        {
            using Bitmap bitmap = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(clientCoords[0] + posX, clientCoords[1] + posY, 0, 0, new Size(width, height));

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (pixel.R >= red && pixel.G == green && pixel.B == blue)
                        return true;
                }
            }
            return false;
        }
    }
}