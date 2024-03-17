using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts.ToA
{
    internal class BabaBoss
    {

        public int[] gameScreenCoords = new int[2] { 0, 0 };
        Player player = new Player();
        Prayer prayer = new Prayer();
        Inventory inv = new Inventory();
        Interfaces interfaces = new Interfaces();
        Peripherals peripherals = new Peripherals();

        public int playerPos = 0;
        public int playerPosInside = 0;
        public int pos = 2;

        public int boulderPos = -1;

        public bool afterBoulders = false;


        public async void startScript()
        {
            interfaces.rotateCamera("east");
            await Task.Delay(1200);
            //getPlayerPosition();
            await Task.Delay(600);
            switch(playerPos)
            {
                case 0:
                    peripherals.leftClick(258, 145); //click on portal
                    break;
            }

            waitTillEntered();

        }

        public async void waitTillEntered()
        {
            if (entered())
            {
                peripherals.leftClick(284, 196); //lure spot
                waitForNpc();
            } else
            {
                await Task.Delay(300);
                waitTillEntered();
            }
        }

        public async void waitForNpc()
        {
            if (!afterBoulders)
            {
                if (npcLured())
                {
                    switch (pos)
                    {
                        case 0:
                            peripherals.leftClick(243, 171);
                            await Task.Delay(800);
                            peripherals.leftClick(277, 144);//baba
                            await Task.Delay(300);
                            startRedX(359, 224);
                            break;
                        case 1:
                            peripherals.leftClick(243, 171);
                            await Task.Delay(1200);
                            peripherals.leftClick(301, 143);//baba
                            await Task.Delay(300);
                            startRedX(314, 225);
                            break;
                        case 2:
                            if (tempNum == 7)
                            {
                                peripherals.leftClick(284, 171);
                                await Task.Delay(1800);
                                startRedX(220, 224);
                            }
                            else
                            {
                                peripherals.leftClick(271, 171);
                                await Task.Delay(1200);
                                peripherals.leftClick(222, 147);//baba
                                await Task.Delay(300);
                                startRedX(220, 224);
                            }
                            break;
                        case 3:
                            peripherals.leftClick(271, 171);
                            await Task.Delay(1200);
                            peripherals.leftClick(222, 147);//baba
                            await Task.Delay(300);
                            startRedX(199, 224);
                            break;
                        case 4:
                            peripherals.leftClick(271, 171);
                            await Task.Delay(1200);
                            peripherals.leftClick(222, 147);//baba
                            await Task.Delay(300);
                            startRedX(140, 225);
                            break;
                    }
                }
                else
                {
                    await Task.Delay(300);
                    waitForNpc();
                }
            } else
            {
                if (hitSplat() && babaLured())
                {
                    switch (pos)
                    {
                        case 0:
                            peripherals.leftClick(243, 171);
                            await Task.Delay(800);
                            peripherals.leftClick(277, 144);//baba
                            await Task.Delay(300);
                            startRedX(359, 224);
                            break;
                        case 1:
                            peripherals.leftClick(243, 171);
                            await Task.Delay(1200);
                            peripherals.leftClick(301, 143);//baba
                            await Task.Delay(300);
                            startRedX(314, 225);
                            break;
                        case 2:
                            if (tempNum == 7)
                            {
                                peripherals.leftClick(284, 171);
                                await Task.Delay(1800);
                                startRedX(220, 224);
                            }
                            else
                            {
                                peripherals.leftClick(271, 171);
                                await Task.Delay(1200);
                                peripherals.leftClick(222, 147);//baba
                                await Task.Delay(300);
                                startRedX(220, 224);
                            }
                            break;
                        case 3:
                            peripherals.leftClick(271, 171);
                            await Task.Delay(1200);
                            peripherals.leftClick(222, 147);//baba
                            await Task.Delay(300);
                            startRedX(199, 224);
                            break;
                        case 4:
                            peripherals.leftClick(271, 171);
                            await Task.Delay(1200);
                            peripherals.leftClick(222, 147);//baba
                            await Task.Delay(300);
                            startRedX(140, 225);
                            break;
                    }
                }
                else
                {
                    await Task.Delay(300);
                    waitForNpc();
                }
            }
        }
        public void startRedX(int x, int y)
        { 
            peripherals.leftClick(x, y); //click teleport
            waitForUnderBaba();
        }

        public async void waitForUnderBaba()
        {
            if(pos == 0)
            {
                if (!underBaba0())
                {
                    await Task.Delay(20);
                    waitForUnderBaba();
                    return;
                }
            } else
            if (pos == 1)
            {
                if (!underBaba1())
                {
                    await Task.Delay(20);
                    waitForUnderBaba();
                    return;
                }
            } else
            if (pos == 2)
            {
                if(!underBaba2())
                {
                    await Task.Delay(20);
                    waitForUnderBaba();
                    return;
                }
            }else
            if (pos == 3)
            {
                if (!underBaba3())
                {
                    await Task.Delay(20);
                    waitForUnderBaba();
                    return;
                }
            } else
            if (pos == 4)
            {
                if (!underBaba4())
                {
                    await Task.Delay(20);
                    waitForUnderBaba();
                    return;
                }
            }

            switch (pos)
                {
                    case 0:
                        peripherals.leftClick(291, 137); //baba
                        break;
                    case 1:
                        peripherals.leftClick(291, 137); //baba
                        break;
                    case 2:
                        peripherals.leftClick(240, 140); //baba
                        break;
                    case 3:
                        peripherals.leftClick(240, 140); //baba
                        break;
                    case 4:
                        peripherals.leftClick(240, 140); //baba
                        break;
                }
                waitForSpec();
        }

        public async void continueRedX(int w, int x)
        {
                peripherals.leftClick(w, x); //click teleport
                waitForUnderBaba();
        }

        public async void waitForSpec()
        {
            if (pos == 2 || pos == 3 || pos == 4)
            {
                if (npcBoulderHealth())
                {
                    await Task.Delay(600);
                    inv.equipItem("Blowpipe");
                    await Task.Delay(200);
                    prayer.boost("Rigour");
                    await Task.Delay(1000);
                    peripherals.leftClick(561, 122);
                    await Task.Delay(200);
                    for (int i = 0; i < 8; i++)
                    {
                        peripherals.leftClick(256, 64);
                        await Task.Delay(400);
                    }
                    tempNum = 0;
                    getPlayerPositionInside();
                    await Task.Delay(300);
                    doBoulders();
                } else
                    if (doneSpec())
                {
                    await Task.Delay(100);
                    switch (pos)
                    {
                        case 0:
                            continueRedX(359, 224);
                            break;
                        case 1:
                            continueRedX(314, 225);
                            break;
                        case 2:
                            continueRedX(220, 224);
                            break;
                        case 3:
                            continueRedX(199, 225);
                            break;
                        case 4:
                            continueRedX(140, 225);
                            break;
                    }
                }
                else
                {
                    await Task.Delay(100);
                    waitForSpec();
                }
            }
        }

        public void setPosition()
        {
            switch (playerPosInside)
            {
                case 0:
                    playerPos = 0;
                    peripherals.leftClick(268, 171);
                    break;
                case 1:
                    break;
                case 2:
                    playerPos = 0;
                    peripherals.leftClick(245, 169);
                    break;
                case 3:
                    playerPos = 1;
                    peripherals.leftClick(268, 171);
                    break;
                case 4:
                    break;
                case 5:
                    playerPos = 1;
                    peripherals.leftClick(245, 169);
                    break;
                case 6:
                    playerPos = 2;
                    peripherals.leftClick(268, 171);
                    break;
                case 7:
                    break;
                case 8:
                    playerPos = 2;
                    peripherals.leftClick(245, 169);
                    break;
                case 9:
                    playerPos = 3;
                    peripherals.leftClick(268, 171);
                    break;
                case 10:
                    break;
                case 11:
                    playerPos = 3;
                    peripherals.leftClick(245, 169);
                    break;
                case 12:
                    playerPos = 3;
                    peripherals.leftClick(268, 171);
                    break;
                case 13:
                    break;
                case 14:
                    playerPos = 4;
                    peripherals.leftClick(245, 169);
                    break;
            }
            boulderPos = -1;
            doBoulders();
        }

        public int tempNum;
        public bool xpDrop;

        public async void doBoulders()
        {
            getBoulderPosition();
            await Task.Delay(10);
            switch (boulderPos)
            {
                case -1:
                    await Task.Delay(10);
                    doBoulders();
                    break;
                case 1:
                    peripherals.leftClick(217, 65);
                    await Task.Delay(600);
                    peripherals.leftClick(230, 142);
                    await Task.Delay(600);
                    peripherals.leftClick(639, 9);
                    await Task.Delay(1500);
                    break;
                case 2:
                    peripherals.leftClick(257, 65);
                    await Task.Delay(600);
                    peripherals.leftClick(639, 9);
                    await Task.Delay(1500);
                    boulderPos = -1;
                    break;
                case 3:
                    peripherals.leftClick(293, 65);
                    await Task.Delay(600);
                    peripherals.leftClick(230, 142);
                    await Task.Delay(600);
                    peripherals.leftClick(639, 9);
                    await Task.Delay(1500);
                    break;
                default:
                    await Task.Delay(10);
                    boulderPos = -1;
                    doBoulders();
                    break;
            }
        }
        public async void dropPotion(int x)
        {
            switch(pos)
            {
                case 0:
                    if (boulderPos == 2)
                    {
                        peripherals.leftClick(590, 33);
                    }
                    if (boulderPos == 3)
                    {
                        peripherals.leftClick(590, 33);
                    }
                    break;
                case 1:
                    if (boulderPos == 1)
                    {
                        peripherals.leftClick(590, 33);
                    }
                    if (boulderPos == 2)
                    {
                        peripherals.leftClick(639, 26);
                    }
                    if (boulderPos == 3)
                    {
                        peripherals.leftClick(639, 26);
                    }
                    break;
                case 2:
                    if (boulderPos == 1)
                    {
                        peripherals.leftClick(590, 33);
                    }
                    if (boulderPos == 2)
                    {
                        peripherals.leftClick(639, 26);
                    }
                    if (boulderPos == 3)
                    {
                        peripherals.leftClick(684, 26);
                    }
                    break;
                case 3:
                    if (boulderPos == 1)
                    {
                        peripherals.leftClick(639, 26);
                    }
                    if (boulderPos == 2)
                    {
                        peripherals.leftClick(684, 26);
                    }
                    if (boulderPos == 3)
                    {
                        peripherals.leftClick(684, 26);
                    }
                    break;
                case 4:
                    if (boulderPos == 1)
                    {
                        peripherals.leftClick(684, 26);
                    }
                    if (boulderPos == 2)
                    {
                        peripherals.leftClick(684, 26);
                    }
                    break;
                default:
                    break;
            }
        }

        public async void dhiawd()
        {
            if (successSkip())
            {
                MessageBox.Show("Drop potion now.");
            }
            else
            {
                await Task.Delay(100);
                dhiawd();
            }
        }

        public void getPlayerPositionInside()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                for (int i = 0; i < 800; i++)
                {
                    for (int j = 0; j < 800; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0)
                        {
                            if (i == 673 && j == 113)
                            {
                                playerPosInside = 0;
                                break;
                            }
                            if (i == 669 && j == 113)
                            {
                                playerPosInside = 1;
                                break;
                            }
                            if (i == 665 && j == 113)
                            {
                                playerPosInside = 2;
                                break;
                            }
                            if (i == 661 && j == 113)
                            {
                                playerPosInside = 3;
                                break;
                            }
                            if (i == 657 && j == 113)
                            {
                                playerPosInside = 4;
                                break;
                            }
                            if (i == 653 && j == 113)
                            {
                                playerPosInside = 5;
                                break;
                            }
                            if (i == 649 && j == 113)
                            {
                                playerPosInside = 6;
                                break;
                            }
                            if (i == 645 && j == 113)
                            {
                                playerPosInside = 7;
                                break;
                            }
                            if (i == 641 && j == 113)
                            {
                                playerPosInside = 8;
                                break;
                            }
                            if (i == 637 && j == 114)
                            {
                                playerPosInside = 9;
                                break;
                            }
                            if (i == 633 && j == 114)
                            {
                                playerPosInside = 10;
                                break;
                            }
                            if (i == 629 && j == 114)
                            {
                                playerPosInside = 11;
                                break;
                            }
                            if (i == 625 && j == 114)
                            {
                                playerPosInside = 12;
                                break;
                            }
                            if (i == 621 && j == 114)
                            {
                                playerPosInside = 13;
                                break;
                            }
                            if (i == 617 && j == 114)
                            {
                                playerPosInside = 14;
                                break;
                            }
                        }
                    }
                }
            }

            //MessageBox.Show("Player pos: " + playerPosInside);
        }

        public void getBoulderPosition()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                for (int i = 157; i < 360; i++)
                {
                    for (int j = 12; j < 100; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 10 && bitmap.GetPixel(i, j).G <= 10 && bitmap.GetPixel(i, j).B <= 35 && bitmap.GetPixel(i + 1, j).R <= 10 && bitmap.GetPixel(i + 1, j).G <= 10 && bitmap.GetPixel(i + 1, j).B <= 35)
                        {
                            if(i >= 168 && j >= 67)
                            {
                                if (i <= 201 && j <= 75)
                                {
                                    boulderPos = 0;
                                }
                            }
                            if (i >= 202 && j >= 67)
                            {
                                if (i <= 235 && j <= 75)
                                {
                                    boulderPos = 1;
                                }
                            }
                            if (i >= 240 && j >= 67)
                            {
                                if (i <= 271 && j <= 75)
                                {
                                    boulderPos = 2;
                                }
                            }
                            if (i >= 280 && j >= 67)
                            {
                                if (i <= 306 && j <= 75)
                                {
                                    boulderPos = 3;
                                }
                            }
                            if (i >= 316 && j >= 67)
                            {
                                if (i <= 343 && j <= 75)
                                {
                                    boulderPos = 4;
                                }
                            }
                        }
                    }
                }

            }
            
        }

        public bool doneSpec() //left
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(11, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 229, gameScreenCoords[1] + 166, 0, 0, new Size(11, 6));
                }
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 60 && bitmap.GetPixel(i, j).G <= 60 && bitmap.GetPixel(i, j).B <= 50)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }
        public bool doneSpec2() //right
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(11, 6))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 275, gameScreenCoords[1] + 166, 0, 0, new Size(11, 6));
                }
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (bitmap.GetPixel(i, j).R <= 60 && bitmap.GetPixel(i, j).G <= 60 && bitmap.GetPixel(i, j).B <= 50)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
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
                    if (bitmap.GetPixel(640, 121).R >= 220 && bitmap.GetPixel(640, 121).G == 0)
                        {
                            return true;
                        }
            }
            return false;
        }

        public bool underBaba0()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(658, 113).R >= 220 && bitmap.GetPixel(658, 113).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool underBaba1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(647, 113).R >= 220 && bitmap.GetPixel(647, 113).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool underBaba2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(636, 114).R >= 220 && bitmap.GetPixel(636, 114).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool underBaba3()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(633, 114).R >= 220 && bitmap.GetPixel(633, 114).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool underBaba4()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(620, 114).R >= 220 && bitmap.GetPixel(620, 114).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool npcLured()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                for (int i = 628; i < 670; i++)
                {
                    for (int j = 66; j < 80; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 245 && bitmap.GetPixel(i, j).G >= 245 && bitmap.GetPixel(i, j).B <= 100)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool hitSplat()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                for (int i = 258; i < 270; i++)
                {
                    for (int j = 161; j < 175; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 0 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool babaLured()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                for (int i = 627; i < 670; i++)
                {
                    for (int j = 66; j < 70; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 245 && bitmap.GetPixel(i, j).G >= 245)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool skipBoulders()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                for (int i = 574; i < 708; i++)
                {
                    for (int j = 43; j < 78; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 245 && bitmap.GetPixel(i, j).G >= 245)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool successSkip()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                for (int i = 578; i < 647; i++)
                {
                    for (int j = 81; j < 87; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 245 && bitmap.GetPixel(i, j).G >= 245)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public bool npcLured2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(626, 70).R >= 245 && bitmap.GetPixel(626, 70).G >= 245)
                {
                    return true;
                }
            }
            return false;
        }

        public bool npcBoulderHealth()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(286, 48).R == 0 && bitmap.GetPixel(286, 48).G == 144 && bitmap.GetPixel(286, 48).B == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool boulders()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(60, 30))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 600, gameScreenCoords[1] + 51, 0, 0, new Size(60, 30));
                }
                for (int i = 0; i < 60; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 245 && bitmap.GetPixel(i, j).G >= 245 && bitmap.GetPixel(i, j).B <= 30)
                        {
                            return false;
                        }
                    }
                }

            }
            return true;
        }
        public void setCoords(int x, int y)
        {
            inv.setCoords(x, y);
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
