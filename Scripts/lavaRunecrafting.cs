using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class lavaRunecrafting
    {

        Inventory inv = new Inventory();
        public Player player = new Player();
        Peripherals peripherals = new Peripherals();
        Mouse mouse = new Mouse();
        Interfaces interfaces = new Interfaces();

        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public int nec = 0;
        public bool repairPouch = false;
        public bool stam = false;
        public int playerPos;

        public int altarX;
        public int altarY;

        public int xpGained;

        public void startScript()
        {
            //repairPouches();
            doBanking();
        }

        public async void doBanking()
        {
            if (bankOpen())
            {
                if(nec > 5)
                {
                    peripherals.fastLeftClick(420, 159);
                    await Task.Delay(200);
                    peripherals.fastLeftClick(419, 233);
                    await Task.Delay(200);
                    peripherals.fastLeftClick(421, 274);
                    await Task.Delay(200);
                }
                if (nec != 5)
                {
                    peripherals.leftClick(421, 192);
                    await Task.Delay(600);
                    peripherals.fastLeftClick(573, 228);
                    await Task.Delay(100);
                    peripherals.leftClick(421, 192);
                    await Task.Delay(600);
                    peripherals.fastLeftClick(573, 228);
                    await Task.Delay(100);
                }
                //peripherals.fastLeftClick(659, 229);
                //await Task.Delay(100);
                if (nec > 5)
                {
                    peripherals.fastLeftClick(619, 224);
                    await Task.Delay(200);
                    peripherals.fastLeftClick(653, 222);
                    await Task.Delay(200);
                    peripherals.fastLeftClick(617, 260);
                    await Task.Delay(200);
                    nec = 0;
                }
                peripherals.leftClick(421, 192);
                await Task.Delay(600);
                Peripherals.PressKey((byte)Keys.Escape, 1);
                Peripherals.PressKey((byte)Keys.F2, 1);
                peripherals.fastLeftClick(694, 381);
                await Task.Delay(1800);
                enterAltar1();
            }
            else
            {
                await Task.Delay(300);
                doBanking();
            }
        }

        public async void enterAltar1()
        {
            if(leftEdge())
            {
                enterAltar2();
            } else
            {
                await Task.Delay(300);
                enterAltar1();
            }
        }

        public async void enterAltar2()
        {
            if (atDesert())
            {
                peripherals.leftClick(altarX, altarY);
                await Task.Delay(100);
                Peripherals.PressKey((byte)Keys.F4, 1);
                craftRunes1();
            } else
            {
                await Task.Delay(200);
                enterAltar2();
            }
        }

        public async void craftRunes1()
        {
            if(inAltar())
            {
                castImbue();
                await Task.Delay(400);
                peripherals.leftClick(575, 262);
                await Task.Delay(150);
                peripherals.leftClick(152, 41);
                xpGained = xpGained + 263;
                while (!noPureEss())
                {
                    await Task.Delay(100);
                }
                if (nec == 5)
                {
                    Peripherals.PressKey((byte)Keys.F2, 1);
                    await Task.Delay(150);
                    peripherals.leftClick(598, 262);
                    await Task.Delay(150);
                    Peripherals.PressKey((byte)Keys.F1, 1);
                    teleToBank();
                }
                else
                {
                    craftRunes2();
                }
            } else
            {
                await Task.Delay(300);
                craftRunes1();
            }
        }

        public async void craftRunes2()
        {
            if (noPureEss())
            {
                xpGained = xpGained + 252;
                peripherals.leftClick(577, 226);
                await Task.Delay(150);
                peripherals.leftClick(577, 261);
                await Task.Delay(150);
                peripherals.leftClick(235, 137);
                await Task.Delay(700);
                peripherals.leftClick(577, 226);
                await Task.Delay(150);
                peripherals.leftClick(577, 261);
                await Task.Delay(150);
                peripherals.leftClick(235, 137);
                await Task.Delay(150);
                while(!noPureEss())
                {
                    await Task.Delay(100);
                }
                xpGained = xpGained + 168;
                Peripherals.PressKey((byte)Keys.F2, 1);
                await Task.Delay(150);
                peripherals.leftClick(598, 262);
                await Task.Delay(150);
                Peripherals.PressKey((byte)Keys.F1, 1);
                teleToBank();
            }
            else
            {
                await Task.Delay(300);
                craftRunes2();
            }
        }

        public void teleToBank()
        {
                if(pouchDeg())
                {
                    repairPouches();
                } else
                {
                    clickBank();
                }
                nec++;
        }

        public async void clickBank()
        {
            if (atCraft())
            {
                peripherals.leftClick(620, 224);
                await Task.Delay(150);
                peripherals.leftClick(206, 112);
                doBanking();
            } else
            {
                await Task.Delay(200);
                clickBank();
            }
        }

        public async void repairPouches() {
            if (atCraft())
            {
                Peripherals.PressKey((byte)Keys.F4, 1);
                await Task.Delay(100);
                peripherals.rightClick(558, 239);
                await Task.Delay(150);
                peripherals.leftClick(552, 281);
                await Task.Delay(150);
                waitForChat();
            }
            else
            {
                await Task.Delay(100);
                repairPouches();
            }
        }

        public async void waitForChat()
        {
            if(npcTalk())
            {
                Peripherals.PressKey((byte)Keys.F1, 1);
                await Task.Delay(100);
                Peripherals.PressKey((byte)Keys.Space, 1);
                await Task.Delay(1200);
                Peripherals.PressKey((byte)Keys.NumPad2, 1);
                await Task.Delay(1200);
                Peripherals.PressKey((byte)Keys.Space, 1);
                await Task.Delay(1200);
                clickBank();
            } else
            {
                await Task.Delay(200);
                waitForChat();
            }
        }

        public async void castImbue()
        {
            peripherals.fastLeftClick(638, 348);
            await Task.Delay(150);
            Peripherals.PressKey((byte)Keys.F1, 1);
        }

        public bool leftEdge()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(726, 158))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(726, 158));
                }

                if (bitmap.GetPixel(653, 109).G == 0 && bitmap.GetPixel(653, 109).R >= 228 && bitmap.GetPixel(653, 109).B == 0)
                {
                        return false;
                }
            }
            return true;
        }

        public bool atDesert()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(726, 158))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(726, 158));
                }

                if (bitmap.GetPixel(665, 57).R <= 140 && bitmap.GetPixel(665, 57).G <= 140 && bitmap.GetPixel(665, 57).G >= 80)
                {
                    altarX = 257;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(669, 57).R <= 140 && bitmap.GetPixel(669, 57).G <= 140 && bitmap.GetPixel(669, 57).G >= 80)
                {
                    altarX = 264;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(673, 57).R <= 140 && bitmap.GetPixel(673, 57).G <= 140 && bitmap.GetPixel(673, 57).G >= 80)
                {
                    altarX = 270;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(677, 57).R <= 140 && bitmap.GetPixel(677, 57).G <= 140 && bitmap.GetPixel(677, 57).G >= 80)
                {
                    altarX = 276;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(681, 57).R <= 140 && bitmap.GetPixel(681, 57).G <= 140 && bitmap.GetPixel(681, 57).G >= 80)
                {
                    altarX = 284;
                    altarY = 2;
                    return true;
                }
                if (bitmap.GetPixel(665, 61).R <= 140 && bitmap.GetPixel(665, 61).G <= 140 && bitmap.GetPixel(665, 61).G >= 80)
                {
                    altarX = 257;
                    altarY = 2;
                    return true;
                }
                if (bitmap.GetPixel(669, 61).R <= 140 && bitmap.GetPixel(669, 61).G <= 140 && bitmap.GetPixel(669, 61).G >= 80)
                {
                    altarX = 263;
                    altarY = 3;
                    return true;
                }
                if (bitmap.GetPixel(673, 61).R <= 140 && bitmap.GetPixel(673, 61).G <= 140 && bitmap.GetPixel(673, 61).G >= 80)
                {
                    altarX = 270;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(677, 61).R <= 140 && bitmap.GetPixel(677, 61).G <= 140 && bitmap.GetPixel(677, 61).G >= 80)
                {
                    altarX = 276;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(681, 61).R <= 140 && bitmap.GetPixel(681, 61).G <= 140 && bitmap.GetPixel(681, 61).G >= 80)
                {
                    altarX = 284;
                    altarY = 2;
                    return true;
                }
                if (bitmap.GetPixel(665, 65).R <= 140 && bitmap.GetPixel(665, 65).G <= 140 && bitmap.GetPixel(665, 65).G >= 80)
                {
                    altarX = 257;
                    altarY = 2;
                    return true;
                }
                if (bitmap.GetPixel(669, 65).R <= 140 && bitmap.GetPixel(669, 65).G <= 140 && bitmap.GetPixel(669, 65).G >= 80)
                {
                    altarX = 263;
                    altarY = 3;
                    return true;
                }
                if (bitmap.GetPixel(673, 65).R <= 140 && bitmap.GetPixel(673, 65).G <= 140 && bitmap.GetPixel(673, 65).G >= 80)
                {
                    altarX = 270;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(677, 65).R <= 140 && bitmap.GetPixel(677, 65).G <= 140 && bitmap.GetPixel(677, 65).G >= 80)
                {
                    altarX = 276;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(681, 65).R <= 140 && bitmap.GetPixel(681, 65).G <= 140 && bitmap.GetPixel(681, 65).G >= 80)
                {
                    altarX = 284;
                    altarY = 2;
                    return true;
                }
                if (bitmap.GetPixel(665, 69).R <= 140 && bitmap.GetPixel(665, 69).G <= 140 && bitmap.GetPixel(665, 69).G >= 80)
                {
                    altarX = 257;
                    altarY = 2;
                    return true;
                }
                if (bitmap.GetPixel(669, 69).R <= 140 && bitmap.GetPixel(669, 69).G <= 140 && bitmap.GetPixel(669, 69).G >= 80)
                {
                    altarX = 263;
                    altarY = 3;
                    return true;
                }
                if (bitmap.GetPixel(673, 69).R <= 140 && bitmap.GetPixel(673, 69).G <= 140 && bitmap.GetPixel(673, 69).G >= 80)
                {
                    altarX = 270;
                    altarY = 1;
                    return true;
                }
                if (bitmap.GetPixel(677, 69).R <= 140 && bitmap.GetPixel(677, 69).G <= 140 && bitmap.GetPixel(677, 69).G >= 80)
                {
                    altarX = 276;
                    altarY = 3;
                    return true;
                }
                if (bitmap.GetPixel(665, 73).R <= 140 && bitmap.GetPixel(665, 73).G <= 140 && bitmap.GetPixel(665, 73).G >= 80)
                {
                    altarX = 257;
                    altarY = 12;
                    return true;
                }
                if (bitmap.GetPixel(669, 73).R <= 140 && bitmap.GetPixel(669, 73).G <= 140 && bitmap.GetPixel(669, 73).G >= 80)
                {
                    altarX = 263;
                    altarY = 13;
                    return true;
                }
                if (bitmap.GetPixel(673, 73).R <= 140 && bitmap.GetPixel(673, 73).G <= 140 && bitmap.GetPixel(673, 73).G >= 80)
                {
                    altarX = 270;
                    altarY = 11;
                    return true;
                }
                if (bitmap.GetPixel(677, 73).R <= 140 && bitmap.GetPixel(677, 73).G <= 140 && bitmap.GetPixel(677, 73).G >= 80)
                {
                    altarX = 276;
                    altarY = 13;
                    return true;
                }

                return false;

            }
        }
        public bool gloryOptions()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(58, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 129, gameScreenCoords[1] + 356, 0, 0, new Size(58, 20));
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

        public bool pouchDeg()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(37, 32))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 556, gameScreenCoords[1] + 209, 0, 0, new Size(37, 32));
                }

                for (int i = 0; i < 37; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 31 && bitmap.GetPixel(i, j).R == 41 && bitmap.GetPixel(i, j).B == 31)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool npcTalk()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(58, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 262, gameScreenCoords[1] + 356, 0, 0, new Size(58, 20));
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

        public bool noPureEss()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(38, 32))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 546, gameScreenCoords[1] + 309, 0, 0, new Size(38, 32));
                }

                for (int i = 0; i < 38; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 126 && bitmap.GetPixel(i, j).R == 135 && bitmap.GetPixel(i, j).B == 125)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool bankOpen()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(20, 20))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 50, gameScreenCoords[1] + 9, 0, 0, new Size(20, 20));
                }

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 152 && bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).B == 31)
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
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 465, gameScreenCoords[1] + 127, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 79 && bitmap.GetPixel(i, j).R == 221 && bitmap.GetPixel(i, j).B == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool inAltar()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 628, gameScreenCoords[1] + 104, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 151 && bitmap.GetPixel(i, j).R == 225 && bitmap.GetPixel(i, j).B == 24)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool atCraft()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(94, 86))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 591, gameScreenCoords[1] + 34, 0, 0, new Size(94, 86));
                }

                for (int i = 0; i < 94; i++)
                {
                    for (int j = 0; j < 86; j++)
                    {
                        if (bitmap.GetPixel(i, j).G == 61 && bitmap.GetPixel(i, j).R == 11 && bitmap.GetPixel(i, j).B == 221)
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
            interfaces.setCoords(x, y);
            peripherals.setCoords(x, y);
        }
    }
}
