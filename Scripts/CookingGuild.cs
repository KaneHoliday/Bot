using Bot.Core;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot.Scripts
{
    /// <summary>
    /// Handles operations for cooking in the Cooking Guild.
    /// </summary>
    internal class CookingGuild
    {
        // Dependencies injected or initialized elsewhere.
        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;

        // Constants for game screen positions
        private const int BankClickX = 442;
        private const int BankClickY = 309;
        private const int CookClickX = 308;
        private const int CookClickY = 314;
        private const int AnglerValue = 8000;
        private const int AnglersCookedXp = 6440;

        public int profit = 0;
        public int anglersCooked = 0;
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
        public async Task startScript()
        {
            Console.WriteLine("Starting anglers...");
            StartTimer();
            await WithdrawAnglersAsync();
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
                Console.WriteLine($"Anglers cooked: {anglersCooked}");
                Console.WriteLine($"XP gained: {xpGained}");
                Console.WriteLine($"Profit made: {profit}");
                await Task.Delay(1000, cancellationToken);
            }
        }

        /// <summary>
        /// Withdraws anglers from the bank if the bank is open.
        /// </summary>
        private async Task WithdrawAnglersAsync()
        {
            processor.addMouseClick(207, 39, "gamescreen");
            while (!bankOpen())
            {
                Console.WriteLine("Bank not open. Retrying...");
                await Task.Delay(100);
            }

            Console.WriteLine("Bank open");
            processor.addMouseClick(442, 309, "gamescreen");
            await Task.Delay(600);
            processor.addMouseClick(229, 132);
            await Task.Delay(600);
            processor.addMouseClick(484, 19);
            await Task.Delay(600);
            processor.addMouseClick(308, 314);
            await WaitForChatboxAsync();
        }

        /// <summary>
        /// Waits for the chatbox to appear and starts cooking.
        /// </summary>
        private async Task WaitForChatboxAsync()
        {
            while (!chatBox())
            {
                await Task.Delay(100);
            }

            processor.PressKey((byte)Keys.Space, 1);
            await WaitForFinishAsync();
        }

        /// <summary>
        /// Waits for cooking to finish and then repeats the process.
        /// </summary>
        private async Task WaitForFinishAsync()
        {
            while (hasAnglers())
            {
                await Task.Delay(1000);
            }

            profit += AnglerValue;
            anglersCooked += 28;
            xpGained += AnglersCookedXp;
            await WithdrawAnglersAsync();
        }

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
        private bool bankOpen()
        {
            return CheckColor(5, 5, 53, 19, 255, 152, 31);
        }

        /// <summary>
        /// Checks if there are more anglers to cook.
        /// </summary>
        private bool hasAnglers()
        {
            return CheckColor(170, 250, 556, 209, 39, 153, 87);
        }

        /// <summary>
        /// Checks if the chatbox is active.
        /// </summary>
        private bool chatBox()
        {
            return CheckColor(5, 5, 483, 359, 255, 255, 255);
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
    }
}