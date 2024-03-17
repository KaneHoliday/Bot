using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts.ToA
{
    internal class CrocBoss
    {

        public int[] gameScreenCoords = new int[2] { 0, 0 };
        Player player = new Player();
        Prayer prayer = new Prayer();
        Inventory inv = new Inventory();
        Interfaces interfaces = new Interfaces();
        Peripherals peripherals = new Peripherals();
        public int playerPos;
        public string attack;

        public string realPrayer;
        public string prayerActive;

        public async void startScript()
        {
            peripherals.leftClick(255, 142); //click crystal, make dynamic
            await Task.Delay(300);
            startKill();
        }

        public async void startKill()
        {
            if(entered())
            {
                peripherals.leftClick(255, 8);
                waitForAttack();
            } else
            {
                await Task.Delay(300);
                startKill();
            }
        }

        public async void waitForAttack()
        {
            if(rockSpot())
            {
                checkAttack();
                doPrayers();
            } else
            {
                await Task.Delay(100);
                waitForAttack();
            }
        }

        public async void checkAttack()
        {
            if(magicAttack())
            {
                attack = "Magic";
                await Task.Delay(100);
                checkAttack();
                return;
            }
            if(rangeAttack())
            {
                attack = "Ranged";
                await Task.Delay(100);
                checkAttack();
                return;
            }
            if(poisonUnder())
            {
                if(!poisonUnderSouth())
                {
                    peripherals.leftClick(257, 180);
                    await Task.Delay(600);
                    peripherals.leftClick(258, 25);
                    checkAttack();
                    return;
                }
                if (!poisonUnderNorth())
                {
                    peripherals.leftClick(257, 153);
                    await Task.Delay(600);
                    peripherals.leftClick(258, 25);
                    checkAttack();
                    return;
                }
                if (!poisonUnderEast())
                {
                    peripherals.leftClick(270, 167);
                    await Task.Delay(600);
                    peripherals.leftClick(258, 25);
                    checkAttack();
                    return;
                }
                if (!poisonUnderWest())
                {
                    peripherals.leftClick(243, 167);
                    await Task.Delay(600);
                    peripherals.leftClick(258, 25);
                    checkAttack();
                    return;
                }
            }
            //if (acidAttack())
            //{
            //    attack = "Acid";
            //    mapAcid(); //if acid under player, waterfalls? - rng dependent, might land on player if not waterfall?
            //    return; //waterfall also leaves 2 jugs out, look for jugs?
            //}
            if (waterfallAttackSouth())
            {
                peripherals.leftClick(204, 170);
                runBack();
                checkAttack();
                return;
            }
            //if (waterfallAttackNorth())
            //{
            //    attack = "Waterfall North";
            ////    return;
            //}
            //if (bloodAttack())
            //{
                //attack = "Blood";
                //await Task.Delay(100);
                //checkAttack();
                //return;
            //}
            attack = "";
            await Task.Delay(10);
            checkAttack();
            return;
        }

        public async void runBack()
        {
            await Task.Delay(2000);
            peripherals.leftClick(302, 170);
        }

        public async void mapAcid()
        {

        }

        public async void doPrayers()
        {
            if(attack == "Magic") {
                if (prayerActive != "Magic")
                {
                    prayer.prayMage();
                    prayerActive = "Magic";
                }
            }

            if (attack == "Ranged")
            {
                if (prayerActive != "Ranged")
                {
                    prayer.prayRange();
                    prayerActive = "Ranged";
                }
            }
            await Task.Delay(10);
            doPrayers();
        }
        public bool entered()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(633, 86).R == 0 && bitmap.GetPixel(633, 86).G == 0 && bitmap.GetPixel(633, 86).B == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool rockSpot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(613, 30).R == 0 && bitmap.GetPixel(613, 30).G == 0 && bitmap.GetPixel(613, 30).B == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool poisonUnder()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(300, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(300, 200));
                }
                for (int i = 254; i < 260; i++)
                {
                    for (int j = 164; j < 170; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 120 && bitmap.GetPixel(i, j).B >= 70 && bitmap.GetPixel(i, j).R >= 120)
                        {
                            if (bitmap.GetPixel(i, j).G <= 130 && bitmap.GetPixel(i, j).B <= 75 && bitmap.GetPixel(i, j).R <= 130)
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }

        public bool poisonUnderNorth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(300, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(300, 200));
                }
                for (int i = 254; i < 260; i++)
                {
                    for (int j = 149; j < 156; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 120 && bitmap.GetPixel(i, j).B >= 70 && bitmap.GetPixel(i, j).R >= 120)
                        {
                            if (bitmap.GetPixel(i, j).G <= 130 && bitmap.GetPixel(i, j).B <= 75 && bitmap.GetPixel(i, j).R <= 130)
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }

        public bool poisonUnderSouth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(300, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(300, 200));
                }
                for (int i = 254; i < 260; i++)
                {
                    for (int j = 177; j < 185; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 120 && bitmap.GetPixel(i, j).B >= 70 && bitmap.GetPixel(i, j).R >= 120)
                        {
                            if (bitmap.GetPixel(i, j).G <= 130 && bitmap.GetPixel(i, j).B <= 75 && bitmap.GetPixel(i, j).R <= 130)
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }

        public bool poisonUnderEast()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(300, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(300, 200));
                }
                for (int i = 265; i < 273; i++)
                {
                    for (int j = 164; j < 170; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 120 && bitmap.GetPixel(i, j).B >= 70 && bitmap.GetPixel(i, j).R >= 120)
                        {
                            if (bitmap.GetPixel(i, j).G <= 130 && bitmap.GetPixel(i, j).B <= 75 && bitmap.GetPixel(i, j).R <= 130)
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }

        public bool poisonUnderWest()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(300, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(300, 200));
                }
                for (int i = 239; i < 248; i++)
                {
                    for (int j = 164; j < 170; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 120 && bitmap.GetPixel(i, j).B >= 70 && bitmap.GetPixel(i, j).R >= 120)
                        {
                            if (bitmap.GetPixel(i, j).G <= 130 && bitmap.GetPixel(i, j).B <= 75 && bitmap.GetPixel(i, j).R <= 130)
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }

        public bool magicAttack()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 246, gameScreenCoords[1] + 131, 0, 0, new Size(20, 30));
                }
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 200 && bitmap.GetPixel(i, j).G <= 100 && bitmap.GetPixel(i, j).G >= 50 && bitmap.GetPixel(i, j).B >= 40 && bitmap.GetPixel(i, j).B <= 50)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        public bool rangeAttack()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 246, gameScreenCoords[1] + 131, 0, 0, new Size(20, 30));
                }
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 40 && bitmap.GetPixel(i, j).G <= 50 && bitmap.GetPixel(i, j).B <= 50)
                        {
                            if (bitmap.GetPixel(i, j).R >= 30 && bitmap.GetPixel(i, j).G >= 30 && bitmap.GetPixel(i, j).B >= 30)
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }

        public bool tail()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(11, 26))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 273, gameScreenCoords[1] + 1, 0, 0, new Size(11, 26));
                }
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 26; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 100 && bitmap.GetPixel(i, j).G >= 100 && bitmap.GetPixel(i, j).B >= 100)
                        {
                                return true;
                        }
                    }
                }

            }
            return false;
        }

        public void playerSafe()
        {

        }

        public bool acidAttack()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(131, 107))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 196, gameScreenCoords[1] + 135, 0, 0, new Size(131, 107));
                }
                for (int i = 0; i < 131; i++)
                {
                    for (int j = 0; j < 107; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 160 && bitmap.GetPixel(i, j).G >= 150 && bitmap.GetPixel(i, j).B >= 85)
                        {
                            if (bitmap.GetPixel(i, j).R <= 170 && bitmap.GetPixel(i, j).G <= 160 && bitmap.GetPixel(i, j).B <= 100)
                            {
                                return true;
                            }

                        }
                    }

                }
                return false;
            }
        }

        public bool waterfallAttackSouth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(200, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(200, 200));
                }
                for (int i = 186; i < 196; i++)
                {
                    for (int j = 165; j < 175; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 200 && bitmap.GetPixel(i, j).G >= 200 && bitmap.GetPixel(i, j).B >= 200)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        public bool waterfallAttackNorth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(55, 161))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 313, gameScreenCoords[1] + 108, 0, 0, new Size(55, 161));
                }
                for (int i = 0; i < 55; i++)
                {
                    for (int j = 0; j < 161; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 200 && bitmap.GetPixel(i, j).G >= 200 && bitmap.GetPixel(i, j).B >= 200)
                        {
                            return true;
                        }
                        if (bitmap.GetPixel(i, j).R <= 150 && bitmap.GetPixel(i, j).R >= 130 && bitmap.GetPixel(i, j).G <= 70 && bitmap.GetPixel(i, j).G >= 60 && bitmap.GetPixel(i, j).B <= 70 && bitmap.GetPixel(i, j).B >= 58)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        public bool bloodAttack()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(34, 32))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 237, gameScreenCoords[1] + 110, 0, 0, new Size(34, 32));
                }
                for (int i = 0; i < 34; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 25 && bitmap.GetPixel(i, j).G <= 25 && bitmap.GetPixel(i, j).G >= 22)
                        {
                            return true;
                        }
                        if (bitmap.GetPixel(i, j).R <= 150 && bitmap.GetPixel(i, j).R >= 130 && bitmap.GetPixel(i, j).G <= 70 && bitmap.GetPixel(i, j).G >= 60 && bitmap.GetPixel(i, j).B <= 70 && bitmap.GetPixel(i, j).B >= 58)
                        {
                            return true;
                        }
                    }
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
            prayer.setCoords(x, y);
        }
    }
}
