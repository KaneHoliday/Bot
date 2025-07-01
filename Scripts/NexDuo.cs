using Bot.Core;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Scripts
{
    internal class NexDuo
    {
        public Interfaces interfaces;
        public Player player;
        public Inventory inventory;
        public XpDrops xpDrops;
        public CoreProcessor processor;
        public Equipment equipment;
        public Prayer prayer;

        public int[] clientCoords = new int[2];

        public bool playCen = false;
        public bool nexCen = false;
        public bool fumusMin = false;
        public int attackCount = 15;

        public int reaverX = 0;
        public int reaverY = 0;

        public async void update()
        {
            nexCen = nexCenter();
            playCen = playerCenter();
            fumusMin = fumusMinion();
            await Task.Delay(10);
            update();
        }

        public async void startScript()
        {
            setupInventory();
            update();
            await Task.Delay(1000);
            while(!nexCen)
            {
                await Task.Delay(100);
            }
            inventory.clickItem2("ranging potion (4)", 98);
            //set inventory up.
            //wait for nex spawn, drink range pot, and right after the first attack, summon a thrall.
            a();
        }

        public void setupInventory()
        {
            inventory.inventory[0] = "Ranging potion (4)";
            inventory.inventory[1] = "Prayer potion (4)";
            inventory.inventory[2] = "Prayer potion (4)";
            inventory.inventory[3] = "Prayer potion (4)";
            inventory.inventory[4] = "Ranging potion (4)";
            inventory.inventory[5] = "Prayer potion (4)";
            inventory.inventory[6] = "Prayer potion (4)";
            inventory.inventory[7] = "Prayer potion (4)"; 
        }

        public void setupGear()
        {

        }
        public void summonThrall()
        {
            processor.addMouseClick(639, 319, "attack");
        }

        public async void a()
        {
            Console.WriteLine("Starting nex script inside it");
            while (playCen)
            {
                Console.WriteLine("player center");
                await Task.Delay(10);
            }
            getReadyToAttack();
        }

        public async void waitForMinDead()
        {
            while(!minionHealth())
            {
                await Task.Delay(100);
            }
            Console.WriteLine("Found minion health");
            summonThrall();
            while (minionHealth())
            {
                await Task.Delay(100);
            }
            Console.WriteLine("Minion dead");
            processor.addMouseClick(179, 179, "movement"); //move back to spot.
            waitForUmbra();
        }

        public async void getReadyToAttack()
        {
            while (!(playCen || fumusMin))
            {
                await Task.Delay(10);
                Console.WriteLine("player not center");

            }
            if (playCen)
            {
                Console.WriteLine("attack");
                processor.addMouseClick(257, 45, "attack");
                attackCount++;
                await Task.Delay(100);
                processor.addMouseClick(258, 190, "attack");
                if(attackCount > 14)
                {
                    summonThrall();
                }
                await Task.Delay(1500);
            }
            else if (fumusMin)
            {
                Console.WriteLine("attack");
                processor.addMouseClick(410, 157, "attack"); //attack fumus
                waitForMinDead();
                return;
            }
            await Task.Delay(600);
            a();
        }

        public async void waitForUmbra()
        {
            while(!umbraMinion())
            {
                Console.WriteLine("Waiting for umbra");
                await Task.Delay(100);
            }
            processor.addMouseClick(104, 152, "attack"); //attack umbra
            while(!minionHealth())
            {
                await Task.Delay(100);
            }
            summonThrall();
            await Task.Delay(100);
            while (minionHealth())
            {
                await Task.Delay(100);
            }
            processor.addMouseClick(338, 179, "movement"); //walk back to safespot
            bloodPhase();
        }

        public async void bloodPhase()
        {
            Console.WriteLine("Starting blood phase");
            while(!(nexCen && playCen))
            {
                await Task.Delay(100);
            }
        }

        public bool nexCenter()
        {
            return checkColor(3, 6, 637, 25, 252, 252, 2);
        }
        public bool playerCenter()
        {
            return checkColor(40, 3, 644, 28, 0, 253, 0);
        }
        public bool fumusMinion()
        {
            return checkColor(3, 3, 464, 308, 200, 0, 0);
        }
        public bool umbraMinion()
        {
            return checkColor(3, 3, 427, 308, 200, 0, 0);
        }
        public bool shadowPhase()
        {
            return checkColor(3, 3, 644, 28, 0, 253, 0);
        }
        public bool reaver()
        {
            return checkColor(293, 127, 124, 46, 0, 252, 0);
        }

        public bool minionHealth()
        {
            using (Bitmap bitmap = new Bitmap(3, 3))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + 9, clientCoords[1] + 166, 0, 0, new Size(3, 3));
                }

                BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, 3, 3),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0;
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            byte b = ptr[0];
                            byte g = ptr[1];
                            byte r = ptr[2];

                            if (r <= 10 && g < 140 && b <= 60)
                            {
                                bitmap.UnlockBits(bmpData);
                                return true;
                            }
                            ptr += 3;
                        }
                        ptr += bmpData.Stride - (3 * 3);
                    }
                }
                bitmap.UnlockBits(bmpData);
            }
            return false;
        }

        public bool checkColor(int a, int b, int posX, int posY, int red, int green, int blue, int npc = 0)
        {
            using (Bitmap bitmap = new Bitmap(a, b))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(clientCoords[0] + posX, clientCoords[1] + posY, 0, 0, new Size(a, b));
                }

                BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, a, b),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0;
                    for (int y = 0; y < b; y++)
                    {
                        for (int x = 0; x < a; x++)
                        {
                            if (ptr[2] == red && ptr[1] == green && ptr[0] == blue)
                            {
                                if (npc == 1)
                                {
                                    reaverX = posX + x;
                                    reaverY = posY + y;
                                }
                                bitmap.UnlockBits(bmpData);
                                return true;
                            }
                            ptr += 3;
                        }
                        ptr += bmpData.Stride - (a * 3);
                    }
                }
                bitmap.UnlockBits(bmpData);
            }
            return false;
        }

        public void equipRange()
        {

        }
        public void equipMagic()
        {

        }
        public void equipRecoil()
        {

        }
        public void drinkVenom()
        {

        }
        public void drinkRangePot()
        {

        }
        public void drinkMagicPot()
        {

        }
        public void drinkPrayerPot()
        {

        }

    }
}
