using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts.ToA
{
    internal class Scarab
    {

        public int[] gameScreenCoords = new int[2] { 0, 0 };
        public int[] tiles = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0};
        public int[] numTiles = new int[25] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] simonSaysTiles = new int[5] { 0, 0, 0, 0, 0 };
        public int[] pillarOrder = new int[6] { -1, -1, -1, -1, -1, -1 };

        public int[] pillarsDone = new int[6] { -1, -1, -1, -1, -1, -1 };

        Player player = new Player();
        Inventory inv = new Inventory();
        Interfaces interfaces = new Interfaces();
        Peripherals peripherals = new Peripherals();
        public int barrierx;
        public int barriery;

        public int numberToSolve = -1;

        public int pillarNums = 0;

        public int playerPos = 0;

        public int num;

        public async void startScript()
        {
            if(inRoom())
            {
                interfaces.rotateCamera("east");
                await Task.Delay(1800);
                clickBarrier();
            } else
            {
                await Task.Delay(600);
                startScript();
            }
        }

        public async void clickBarrier()
        {
            getBarrierCoords();
            await Task.Delay(200);
            peripherals.leftClick(barrierx, barriery);
            waitForPuzzleStart();
        }

        public async void waitForPuzzleStart()
        {
            if(throughBarrier())
            {
                identifyPuzzle();
            } else
            {
                await Task.Delay(600);
                waitForPuzzleStart();
            }
        }

        public async void identifyPuzzle()
        {
            if (numberPuzzle())
            {
                //MessageBox.Show("Number puzzle. num: " + num.ToString());
                await Task.Delay(1200);
                doNumberPuzzle();
                return;
            }
            if (simonPuzzle() && !stepPuzzle())
            {
                //MessageBox.Show("Simon says puzzle. num: " + num.ToString());
                await Task.Delay(1200);
                doSimonSaysPuzzle();
                return;
            }
            if (stepPuzzle())
            {
                //MessageBox.Show("Step puzzle. num: " + num.ToString());
                await Task.Delay(1200);
                doStepPuzzle();
                return;
            }
            if (pillarPuzzle())
            {
                //MessageBox.Show("pillar puzzle. num: " + num.ToString());
                await Task.Delay(1200);
                doPillarPuzzle();
                return;
            }
        }

        public async void identifyPuzzle2()
        {
            if (pillarPuzzle2())
            {
                MessageBox.Show("pillar puzzle. num: " + num.ToString());
                await Task.Delay(1200);
                doPillarPuzzle2();
                return;
            }
            if (numberPuzzle2())
            {
                MessageBox.Show("Number puzzle. num: " + num.ToString());
                await Task.Delay(1200);
                doNumberPuzzle();
                return;
            }
            if (simonPuzzle() && !stepPuzzle())
            {
                MessageBox.Show("Simon says puzzle. num: " + num.ToString());
                await Task.Delay(1200);
                doSimonSaysPuzzle();
                return;
            }
            if (stepPuzzle())
            {
                MessageBox.Show("Step puzzle. num: " + num.ToString());
                await Task.Delay(1200);
                doStepPuzzle2();
                return;
            }
        }

        public async void doPillarPuzzle()
        {
            peripherals.leftClick(252, 88);
            pillarPuz();
        }
        public async void doPillarPuzzle2()
        {
            peripherals.leftClick(259, 92);
            pillarPuz2();
        }

        public async void pillarPuz()
        {
            if(pillarPuzStart())
            {
                checkPillars();
                await Task.Delay(2000);
                pillars();
            } else
            {
                await Task.Delay(300);
                pillarPuz();
            }
        }

        public async void pillarPuz2()
        {
            if (pillarPuzStart2())
            {
                checkPillars();
                await Task.Delay(2000);
                pillars10();
            }
            else
            {
                await Task.Delay(300);
                pillarPuz2();
            }
        }

        public async void doNumberPuzzle()
        {
            peripherals.leftClick(233, 121);
            numberPuz();
        }

        public async void numberPuz()
        {
            if(numPuzzleActive())
            {
                await Task.Delay(600);
                checkNumTiles();
            } else
            {
                await Task.Delay(600);
                numberPuz();
            }
        }

        public async void doSimonSaysPuzzle()
        {
            peripherals.leftClick(231, 122);
            simonPuz();
        }

        public async void simonPuz()
        {
            if (puzzleActive())
            {
                checkSimonTiles();
            }
            else
            {
                await Task.Delay(600);
                simonPuz();
            }
        }



        public async void doStepPuzzle()
        {
            checkTiles();
        }

        public async void doStepPuzzle2()
        {
            checkTiles2();
        }

        public async void walkToCentre1()
        {
            peripherals.leftClick(246, 107);
            walkToCentre2();
        }
        public async void walkToCentre2()
        {
            if(atBottomTile())
            {
               await Task.Delay(600);
               peripherals.leftClick(267, 144);
               solveTiles();
            } else
            {
                await Task.Delay(600);
                walkToCentre2();
            }
        }


        public async void solveTiles()
        {
            playerPos = -1;
            getPlayerPosition();
            await Task.Delay(300);
            if (playerPos == 6)
            {
                if (tiles[0] == 1)
                {
                    peripherals.leftClick(232, 146);
                    await Task.Delay(2200);
                    peripherals.leftClick(284, 193);
                    tiles[0] = 0;
                    solveTiles();
                    return;
                }
                if (tiles[1] == 1)
                {
                    peripherals.leftClick(259, 144);
                    await Task.Delay(2200);
                    peripherals.leftClick(258, 194);
                    tiles[1] = 0;
                    solveTiles();
                    return;
                }
                if (tiles[2] == 1)
                {
                    peripherals.leftClick(283, 145);
                    await Task.Delay(2200);
                    peripherals.leftClick(231, 193);
                    tiles[2] = 0;
                    solveTiles();
                    return;
                }
                if (tiles[3] == 1)
                {
                    peripherals.leftClick(232, 168);
                    await Task.Delay(2200);
                    peripherals.leftClick(283, 168);
                    tiles[3] = 0;
                    solveTiles();
                    return;
                }
                if (tiles[4] == 1)
                {
                    peripherals.leftClick(285, 166);
                    await Task.Delay(2200);
                    peripherals.leftClick(230, 165);
                    tiles[4] = 0;
                    solveTiles();
                    return;
                }
                if (tiles[5] == 1)
                {
                    peripherals.leftClick(231, 193);
                    await Task.Delay(2200);
                    peripherals.leftClick(280, 146);
                    tiles[5] = 0;
                    solveTiles();
                    return;
                }
                if (tiles[6] == 1)
                {
                    peripherals.leftClick(258, 193);
                    await Task.Delay(2200);
                    peripherals.leftClick(254, 143);
                    tiles[6] = 0;
                    solveTiles();
                    return;
                }
                if (tiles[7] == 1)
                {
                    peripherals.leftClick(287, 192);
                    await Task.Delay(2200);
                    peripherals.leftClick(230, 145);
                    tiles[7] = 0;
                    solveTiles();
                    return;
                }
                peripherals.leftClick(290, 98);
                toStart2nd();
            } else
            {
                await Task.Delay(300);
                solveTiles();
            }
        }

        public async void tile(int x)
        {
            switch(x)
            {
                case 0:
                    peripherals.leftClick(268, 147);
                    await Task.Delay(2000);
                    peripherals.leftClick(245, 187);
                    break;
                case 1:
                    peripherals.leftClick(255, 158);
                    await Task.Delay(1500);
                    peripherals.leftClick(255, 177);
                    break;
                case 2:
                    peripherals.leftClick(281, 157);
                    await Task.Delay(2000);
                    peripherals.leftClick(231, 178);
                    break;
                case 3:
                    peripherals.leftClick(242, 170);
                    await Task.Delay(1500);
                    peripherals.leftClick(268, 165);
                    break;
                case 4:
                    peripherals.leftClick(268, 169);
                    await Task.Delay(1500);
                    peripherals.leftClick(245, 166);
                    break;
                case 5:
                    peripherals.leftClick(294, 170);
                    await Task.Delay(600);
                    peripherals.leftClick(225, 165);
                    await Task.Delay(3000);
                    break;
                case 6:
                    peripherals.leftClick(255, 182);
                    await Task.Delay(2000);
                    peripherals.leftClick(255, 154);
                    break;
                case 7:
                    peripherals.leftClick(283, 178);
                    await Task.Delay(2000);
                    peripherals.leftClick(232, 155);
                    break;
                case 8:
                    peripherals.leftClick(269, 192);
                    await Task.Delay(2000);
                    peripherals.leftClick(246, 146);
                    break;
            }
        }

        public async void lastPuzzle()
        {
            peripherals.leftClick(178, 159);
        }

        public async void toStart2nd()
        {
            if(throughHole())
            {
                peripherals.leftClick(282, 147);
                await Task.Delay(3000);
                identifyPuzzle2();
            } else
            {
                await Task.Delay(300);
                toStart2nd();
            }
        }

        public async void solveSimonSays1(int x)
        {
            if (x != 5)
            {
                if (simonSaysTiles[x] == 0)
                {
                    tile(0);
                    await Task.Delay(4000);
                    solveSimonSays1(x + 1);
                }
                if (simonSaysTiles[x] == 1)
                {
                    tile(1);
                    await Task.Delay(4000);
                    solveSimonSays1(x + 1);
                }
                if (simonSaysTiles[x] == 2)
                {
                    tile(2);
                    await Task.Delay(4000);
                    solveSimonSays1(x + 1);
                }
                if (simonSaysTiles[x] == 3)
                {
                    tile(3);
                    await Task.Delay(4000);
                    solveSimonSays1(x + 1);
                }
                if (simonSaysTiles[x] == 4)
                {
                    tile(4);
                    await Task.Delay(4000);
                    solveSimonSays1(x + 1);
                }
                if (simonSaysTiles[x] == 5)
                {
                    tile(5);
                    await Task.Delay(4000);
                }
                if (simonSaysTiles[x] == 6)
                {
                    tile(6);
                    await Task.Delay(4000);
                    solveSimonSays1(x + 1);
                }
                if (simonSaysTiles[x] == 7)
                {
                    tile(7);
                    await Task.Delay(4000);
                    solveSimonSays1(x + 1);
                }
                if (simonSaysTiles[x] == 8)
                {
                    tile(8);
                    await Task.Delay(4000);
                    solveSimonSays1(x + 1);
                }
            }
            else
            {
                peripherals.leftClick(287, 94);
                toStart2nd();
            }
        }

        public async void checkA(int x)
        {
            getPlayerPosition();
            await Task.Delay(300);
            if (playerPos == 10)
            {
                peripherals.leftClick(284, 159);
                checkB(x);
            } else
            {
                await Task.Delay(100);
                checkA(x);
            }
        }
        public async void checkB(int x)
        {
            getPlayerPosition();
            await Task.Delay(300);
            if (playerPos == 8)
            {
                peripherals.leftClick(231, 178);
                checkC(x);
            }
            else
            {
                await Task.Delay(100);
                checkB(x);
            }
        }
        public async void checkC(int x)
        {
            getPlayerPosition();
            await Task.Delay(300);
            if (playerPos == 10)
            {
                peripherals.leftClick(243, 157);
                solveSimonSays1(x);
            }
            else
            {
                await Task.Delay(100);
                checkC(x);
            }
        }

        public async void aWalk()
        {
            getPlayerPosition();
            await Task.Delay(300);
            if (playerPos == 15)
            {
                peripherals.leftClick(267, 144);
                bWalk();
            }  else
            {
                await Task.Delay(50);
                aWalk();
            }
        }
        public async void bWalk()
        {
            getPlayerPosition();
            await Task.Delay(300);
            if (playerPos == 5)
            {
                solveSimonSays1(0);
            }
            else
            {
                await Task.Delay(50);
                bWalk();
            }
        }

        public async void cWalk()
        {
            getPlayerPosition();
            await Task.Delay(300);
            if (playerPos == 16)
            {
                peripherals.leftClick(258, 145);
            }
            else
            {
                await Task.Delay(50);
                cWalk();
            }
        }

        public void checkTiles2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(300, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(300, 200));
                }

                for (int i = 0; i < 300; i++)
                {
                    for (int j = 0; j < 200; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255)
                        {
                            if (i >= 230 && j >= 75)
                            {
                                if (i <= 245 && j <= 85)
                                {
                                    tiles[0] = 1;
                                }
                            }
                            if (i >= 250 && j >= 75)
                            {
                                if (i <= 262 && j <= 85)
                                {
                                    tiles[1] = 1;
                                }
                            }
                            if (i >= 269 && j >= 75)
                            {
                                if (i <= 280 && j <= 83)
                                {
                                    tiles[2] = 1;
                                }
                            }
                            if (i >= 231 && j >= 88)
                            {
                                if (i <= 243 && j <= 99)
                                {
                                    tiles[3] = 1;
                                }
                            }
                            if (i >= 269 && j >= 89)
                            {
                                if (i <= 283 && j <= 100)
                                {
                                    tiles[4] = 1;
                                }
                            }
                            if (i >= 229 && j >= 104)
                            {
                                if (i <= 243 && j <= 115)
                                {
                                    tiles[5] = 1;
                                }
                            }
                            if (i >= 249 && j >= 103)
                            {
                                if (i <= 262 && j <= 116)
                                {
                                    tiles[6] = 1;
                                }
                            }
                            if (i >= 269 && j >= 104)
                            {
                                if (i <= 283 && j <= 115)
                                {
                                    tiles[7] = 1;
                                }
                            }
                        }
                    }
                }


            }

            walkToCentre1();

            //MessageBox.Show(tiles[0] + " " + tiles[1] + " " + tiles[2]);
            //MessageBox.Show(tiles[3] + " " + "x" + " " + tiles[4]);
            //MessageBox.Show(tiles[5] + " " + tiles[6] + " " + tiles[7]);
        }

        public void checkTiles()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(300, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(300, 200));
                }

                for (int i = 0; i < 300; i++)
                {
                    for (int j = 0; j < 200; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255)
                        {
                            if (i >= 230 && j >= 75)
                            {
                                if (i <= 245 && j <= 85)
                                {
                                    tiles[0] = 1;
                                }
                            }
                            if (i >= 250 && j >= 75)
                            {
                                if (i <= 262 && j <= 85)
                                {
                                    tiles[1] = 1;
                                }
                            }
                            if (i >= 269 && j >= 75)
                            {
                                if (i <= 280 && j <= 83)
                                {
                                    tiles[2] = 1;
                                }
                            }
                            if (i >= 231 && j >= 88)
                            {
                                if (i <= 243 && j <= 99)
                                {
                                    tiles[3] = 1;
                                }
                            }
                            if (i >= 269 && j >= 89)
                            {
                                if (i <= 283 && j <= 100)
                                {
                                    tiles[4] = 1;
                                }
                            }
                            if (i >= 229 && j >= 104)
                            {
                                if (i <= 243 && j <= 115)
                                {
                                    tiles[5] = 1;
                                }
                            }
                            if (i >= 249 && j >= 103)
                            {
                                if (i <= 262 && j <= 116)
                                {
                                    tiles[6] = 1;
                                }
                            }
                            if (i >= 269 && j >= 104)
                            {
                                if (i <= 283 && j <= 115)
                                {
                                    tiles[7] = 1;
                                }
                            }
                        }
                    }
                }


            }

            walkToCentre1();

            //MessageBox.Show(tiles[0] + " " + tiles[1] + " " + tiles[2]);
            //MessageBox.Show(tiles[3] + " " + "x" + " " + tiles[4]);
            //MessageBox.Show(tiles[5] + " " + tiles[6] + " " + tiles[7]);
        }

        public void checkNumTiles()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(75, 440).R >= 220 && bitmap.GetPixel(75, 440).G <= 20 && bitmap.GetPixel(79, 440).R >= 220 && bitmap.GetPixel(79, 440).G <= 20 && bitmap.GetPixel(87, 440).R >= 220 && bitmap.GetPixel(87, 440).G <= 20)
                {
                    numberToSolve = 27;
                }
                if (bitmap.GetPixel(75, 440).R >= 220 && bitmap.GetPixel(75, 440).G <= 20 && bitmap.GetPixel(79, 440).R >= 220 && bitmap.GetPixel(79, 440).G <= 20 && bitmap.GetPixel(83, 440).R >= 220 && bitmap.GetPixel(83, 440).G <= 20 && bitmap.GetPixel(86, 440).R >= 220 && bitmap.GetPixel(86, 440).G <= 20)
                {
                    numberToSolve = 30;
                }
            }

            //MessageBox.Show("Number to solve: " + numberToSolve.ToString());
        }

        public void grabTileCoords()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(309, 144))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(309, 144));
                }

                for (int i = 238; i < 309; i++)
                {
                    for (int j = 84; j < 144; j++)
                    {
                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 255)
                        {
                            //MessageBox.Show("i: " + i + "j: " + j);
                        }
                    }
                }
            }
        }

        public void checkPillars()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(331, 210))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(331, 210));
                }

                for (int i = 202; i < 331; i++)
                {
                    for (int j = 101; j < 210; j++)
                    {
                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 255)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarOrder[0] = 0;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarOrder[0] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarOrder[0] = 2;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarOrder[0] = 3;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarOrder[0] = 4;
                                }
                            }

                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 204)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarOrder[1] = 0;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarOrder[1] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarOrder[1] = 2;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarOrder[1] = 3;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarOrder[1] = 4;
                                }
                            }

                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 153)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarOrder[2] = 0;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarOrder[2] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarOrder[2] = 2;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarOrder[2] = 3;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarOrder[2] = 4;
                                }
                            }

                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 102)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarOrder[3] = 0;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarOrder[3] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarOrder[3] = 2;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarOrder[3] = 3;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarOrder[3] = 4;
                                }
                            }

                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 51)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarOrder[4] = 0;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarOrder[4] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarOrder[4] = 2;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarOrder[4] = 3;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarOrder[4] = 4;
                                }
                            }

                        }
                    }
                }
            }

            //MessageBox.Show(pillarOrder[0] + " " + pillarOrder[1] + " " + pillarOrder[2] + " " + pillarOrder[3] + " " + pillarOrder[4]);


        }

        public bool pillarsComplete()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(331, 210))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(331, 210));
                }

                for (int i = 202; i < 331; i++)
                {
                    for (int j = 101; j < 210; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 128 && bitmap.GetPixel(i, j).G == 128 && bitmap.GetPixel(i, j).B == 128)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarsDone[0] = 1;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarsDone[1] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarsDone[2] = 1;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarsDone[3] = 1;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarsDone[4] = 1;
                                }
                            }

                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 204)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarsDone[0] = 1;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarsDone[1] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarsDone[2] = 1;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarsDone[3] = 1;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarsDone[4] = 1;
                                }
                            }

                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 153)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarsDone[0] = 1;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarsDone[1] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarsDone[2] = 1;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarsDone[3] = 1;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarsDone[4] = 1;
                                }
                            }

                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 102)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarsDone[0] = 1;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarsDone[1] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarsDone[2] = 1;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarsDone[3] = 1;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarsDone[4] = 1;
                                }
                            }

                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 51)
                        {
                            if (i >= 219 && j >= 137)
                            {
                                if (i <= 224 && j <= 146)
                                {
                                    pillarsDone[0] = 1;
                                }
                            }
                            if (i >= 208 && j >= 163)
                            {
                                if (i <= 223 && j <= 177)
                                {
                                    pillarsDone[1] = 1;
                                }
                            }

                            if (i >= 288 && j >= 162)
                            {
                                if (i <= 303 && j <= 173)
                                {
                                    pillarsDone[2] = 1;
                                }
                            }
                            if (i >= 211 && j >= 187)
                            {
                                if (i <= 223 && j <= 203)
                                {
                                    pillarsDone[3] = 1;
                                }
                            }
                            if (i >= 293 && j >= 189)
                            {
                                if (i <= 305 && j <= 204)
                                {
                                    pillarsDone[4] = 1;
                                }
                            }
                        }
                    }
                }
            }

            if(pillarsDone[0] == 1 && pillarsDone[1] == 1 && pillarsDone[2] == 1 && pillarsDone[3] == 1 && pillarsDone[4] == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }


        public void findTileToFlip()
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
                        if (bitmap.GetPixel(i, j) == Color.FromArgb(107, 101, 88) || bitmap.GetPixel(i, j) == Color.FromArgb(102, 96, 84) || bitmap.GetPixel(i, j) == Color.FromArgb(106, 100, 87))
                        {
                            if(i >= 161 && j >= 114)
                            {
                                if (i <= 165 && j <= 118)
                                {
                                    flipTilesLeft[0] = 1;
                                    break;
                                }
                            }
                            if (i >= 184 && j >= 115)
                            {
                                if (i <= 189 && j <= 119)
                                {
                                    flipTilesLeft[1] = 1;
                                    break;
                                }
                            }
                            if (i >= 206 && j >= 114)
                            {
                                if (i <= 210 && j <= 118)
                                {
                                    flipTilesLeft[2] = 1;
                                    break;
                                }
                            }
                            if (i >= 154 && j >= 135)
                            {
                                if (i <= 158 && j <= 139)
                                {
                                    flipTilesLeft[3] = 1;
                                    break;
                                }
                            }
                            if (i >= 179 && j >= 134)
                            {
                                if (i <= 183 && j <= 138)
                                {
                                    flipTilesLeft[4] = 1;
                                    break;
                                }
                            }
                            if (i >= 204 && j >= 134)
                            {
                                if (i <= 208 && j <= 138)
                                {
                                    flipTilesLeft[5] = 1;
                                    break;
                                }
                            }
                            if (i >= 150 && j >= 156)
                            {
                                if (i <= 154 && j <= 160)
                                {
                                    flipTilesLeft[6] = 1;
                                    break;
                                }
                            }
                            if (i >= 176 && j >= 156)
                            {
                                if (i <= 180 && j <= 160)
                                {
                                    flipTilesLeft[7] = 1;
                                    break;
                                }
                            }
                            if (i >= 202 && j >= 156)
                            {
                                if (i <= 206 && j <= 160)
                                {
                                    flipTilesLeft[2] = 1;
                                    break;
                                }
                            }




                            if (i >= 298 && j >= 114)
                            {
                                if (i <= 302 && j <= 118)
                                {
                                    flipTilesRight[0] = 1;
                                    break;
                                }
                            }
                            if (i >= 321 && j >= 115)
                            {
                                if (i <= 325 && j <= 119)
                                {
                                    flipTilesRight[1] = 1;
                                    break;
                                }
                            }
                            if (i >= 344 && j >= 114)
                            {
                                if (i <= 348 && j <= 118)
                                {
                                    flipTilesRight[2] = 1;
                                    break;
                                }
                            }
                            if (i >= 300 && j >= 135)
                            {
                                if (i <= 304 && j <= 139)
                                {
                                    flipTilesRight[3] = 1;
                                    break;
                                }
                            }
                            if (i >= 324 && j >= 134)
                            {
                                if (i <= 328 && j <= 138)
                                {
                                    flipTilesRight[4] = 1;
                                    break;
                                }
                            }
                            if (i >= 349 && j >= 134)
                            {
                                if (i <= 353 && j <= 138)
                                {
                                    flipTilesRight[5] = 1;
                                    break;
                                }
                            }
                            if (i >= 301 && j >= 156)
                            {
                                if (i <= 305 && j <= 160)
                                {
                                    flipTilesRight[6] = 1;
                                    break;
                                }
                            }
                            if (i >= 328 && j >= 156)
                            {
                                if (i <= 332 && j <= 160)
                                {
                                    flipTilesRight[7] = 1;
                                    break;
                                }
                            }
                            if (i >= 354 && j >= 156)
                            {
                                if (i <= 358 && j <= 160)
                                {
                                    flipTilesRight[2] = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            peripherals.leftClick(181, 147);
            startReveal1();


            //MessageBox.Show(flipTilesLeft[0].ToString() + flipTilesLeft[1].ToString() + flipTilesLeft[2].ToString());
            //MessageBox.Show(flipTilesLeft[3].ToString() + flipTilesLeft[4].ToString() + flipTilesLeft[5].ToString());
            //MessageBox.Show(flipTilesLeft[6].ToString() + flipTilesLeft[7].ToString() + flipTilesLeft[8].ToString());

            //MessageBox.Show(flipTilesRight[0].ToString() + flipTilesRight[1].ToString() + flipTilesRight[2].ToString());
            //MessageBox.Show(flipTilesRight[3].ToString() + flipTilesRight[4].ToString() + flipTilesRight[5].ToString());
            //MessageBox.Show(flipTilesRight[6].ToString() + flipTilesRight[7].ToString() + flipTilesRight[8].ToString());
        }

        public int[] flipTilesLeft = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] flipTilesRight = new int[9] { 0, 0, 1, 0, 1, 1, 0, 0, 0 };

        public async void startReveal1()
        {
            if(tilesLeft())
            {
                await Task.Delay(300);
                for(int i = 0; i < 9; i++)
                {
                    if (flipTilesLeft[i] == 1)
                    {
                        revealTiles(i, "left");
                        return;
                    }
                }
                revealTiles(9, "left");
            } else
            {
                await Task.Delay(100);
                startReveal1();
            }
        }

        public async void startReveal2()
        {
            if (tilesRight())
            {
                await Task.Delay(300);
                for (int i = 0; i < 9; i++)
                {
                    if (flipTilesRight[i] == 1)
                    {
                        revealTiles(i, "right");
                        return;
                    }
                }
                revealTiles(9, "left");
            }
            else
            {
                await Task.Delay(100);
                startReveal2();
            }
        }


        public async void revealTiles(int x, string side) {

            if (x <= 8)
            {
                if(side == "left")
                {
                    flipTilesLeft[x] = 0;
                }
                if (side == "right")
                {
                    flipTilesRight[x] = 0;
                }
            }

            switch (x)
            {
                case 0:
                    peripherals.leftClick(229, 155);
                    await Task.Delay(2000);
                    peripherals.leftClick(267, 179);
                    await Task.Delay(1000);
                    break;
                case 1:
                    peripherals.leftClick(255, 155);
                    await Task.Delay(1000);
                    break;
                case 2:
                    peripherals.leftClick(281, 155);
                    await Task.Delay(2000);
                    peripherals.leftClick(244, 180);
                    await Task.Delay(1000);
                    break;
                case 3:
                    peripherals.leftClick(227, 180);
                    await Task.Delay(2000);
                    peripherals.leftClick(268, 157);
                    await Task.Delay(1000);
                    break;
                case 4:
                    peripherals.leftClick(253, 181);
                    await Task.Delay(1000);
                    break;
                case 5:
                    peripherals.leftClick(281, 180);
                    await Task.Delay(2000);
                    peripherals.leftClick(245, 157);
                    await Task.Delay(1000);
                    break;
                case 6:
                    peripherals.leftClick(227, 208);
                    await Task.Delay(2000);
                    peripherals.leftClick(279, 148);
                    await Task.Delay(1000);
                    break;
                case 7:
                    peripherals.leftClick(255, 208);
                    await Task.Delay(2000);
                    peripherals.leftClick(258, 148);
                    await Task.Delay(1000);
                    break;
                case 8:
                    peripherals.leftClick(283, 208);
                    await Task.Delay(2000);
                    peripherals.leftClick(232, 148);
                    await Task.Delay(1000);
                    break;
                case 9:
                    peripherals.leftClick(414, 166);
                    await Task.Delay(1000);
                    break;
            }
            if(flipTilesLeft.Contains(1))
            {
                startReveal1();
                return;
            }
            if(flipTilesRight.Contains(1))
            {
                startReveal2();
                return;
            } else
            {
                doMatching();
            }
        }

        public async void doMatching()
        {

        }

        public async void pillars()
        {
            if (!pillarsComplete())
            {

                for (int i = 0; i < 5; i++)
                {
                    if (pillarOrder[i] != -1)
                    {
                        clickPillar(pillarOrder[i]);
                        await Task.Delay(600);
                    }
                }

                if(!pillarOrder.Contains(0))
                {
                    clickPillar(0);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(1))
                {
                    clickPillar(1);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(2))
                {
                    clickPillar(2);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(3))
                {
                    clickPillar(3);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(4))
                {
                    clickPillar(4);
                    await Task.Delay(600);
                }

                checkPillars();
                peripherals.leftClick(254, 124);
                await Task.Delay(3000);
                peripherals.leftClick(255, 212);
                await Task.Delay(3000);
                pillars2();

            } else
            {
                peripherals.leftClick(287, 103);
                toStart2nd();
            }

        }

        public async void pillars10()
        {
            if (!pillarsComplete())
            {

                for (int i = 0; i < 5; i++)
                {
                    if (pillarOrder[i] != -1)
                    {
                        clickPillar(pillarOrder[i]);
                        await Task.Delay(600);
                    }
                }

                if (!pillarOrder.Contains(0))
                {
                    clickPillar(0);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(1))
                {
                    clickPillar(1);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(2))
                {
                    clickPillar(2);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(3))
                {
                    clickPillar(3);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(4))
                {
                    clickPillar(4);
                    await Task.Delay(600);
                }

                checkPillars();
                peripherals.leftClick(254, 124);
                await Task.Delay(3000);
                peripherals.leftClick(255, 212);
                await Task.Delay(3000);
                pillars20();

            }
            else
            {
                peripherals.leftClick(287, 103);
                lastPuzzle();
            }

        }

        public async void pillars2()
        {
            if (!pillarsComplete())
            {

                for (int i = 0; i < 5; i++)
                {
                    if (pillarOrder[i] != -1)
                    {
                        clickPillar(pillarOrder[i]);
                        await Task.Delay(600);
                    }
                }

                if (!pillarOrder.Contains(4))
                {
                    clickPillar(4);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(3))
                {
                    clickPillar(3);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(2))
                {
                    clickPillar(2);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(1))
                {
                    clickPillar(1);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(0))
                {
                    clickPillar(0);
                    await Task.Delay(600);
                }

                checkPillars();
                peripherals.leftClick(254, 124);
                await Task.Delay(3000);
                peripherals.leftClick(255, 212);
                await Task.Delay(3000);
                pillars3();

            }
            else
            {
                peripherals.leftClick(287, 103);
                toStart2nd();
            }
        }
        public async void pillars20()
        {
            if (!pillarsComplete())
            {

                for (int i = 0; i < 5; i++)
                {
                    if (pillarOrder[i] != -1)
                    {
                        clickPillar(pillarOrder[i]);
                        await Task.Delay(600);
                    }
                }

                if (!pillarOrder.Contains(4))
                {
                    clickPillar(4);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(3))
                {
                    clickPillar(3);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(2))
                {
                    clickPillar(2);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(1))
                {
                    clickPillar(1);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(0))
                {
                    clickPillar(0);
                    await Task.Delay(600);
                }

                checkPillars();
                peripherals.leftClick(254, 124);
                await Task.Delay(3000);
                peripherals.leftClick(255, 212);
                await Task.Delay(3000);
                pillars30();

            }
            else
            {
                peripherals.leftClick(287, 103);
                lastPuzzle();
            }
        }

        public async void pillars3()
        {
            if (!pillarsComplete())
            {

                for (int i = 0; i < 5; i++)
                {
                    if (pillarOrder[i] != -1)
                    {
                        clickPillar(pillarOrder[i]);
                        await Task.Delay(600);
                    }
                }

                if (!pillarOrder.Contains(3))
                {
                    clickPillar(3);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(0))
                {
                    clickPillar(0);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(2))
                {
                    clickPillar(2);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(1))
                {
                    clickPillar(1);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(4))
                {
                    clickPillar(4);
                    await Task.Delay(600);
                }


                checkPillars();
                peripherals.leftClick(254, 124);
                await Task.Delay(3000);
                peripherals.leftClick(255, 212);
                await Task.Delay(3000);
                pillars4();

            }
            else
            {
                peripherals.leftClick(287, 103);
                toStart2nd();
            }
        }

        public async void pillars30()
        {
            if (!pillarsComplete())
            {

                for (int i = 0; i < 5; i++)
                {
                    if (pillarOrder[i] != -1)
                    {
                        clickPillar(pillarOrder[i]);
                        await Task.Delay(600);
                    }
                }

                if (!pillarOrder.Contains(3))
                {
                    clickPillar(3);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(0))
                {
                    clickPillar(0);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(2))
                {
                    clickPillar(2);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(1))
                {
                    clickPillar(1);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(4))
                {
                    clickPillar(4);
                    await Task.Delay(600);
                }


                checkPillars();
                peripherals.leftClick(254, 124);
                await Task.Delay(3000);
                peripherals.leftClick(255, 212);
                await Task.Delay(3000);
                pillars40();

            }
            else
            {
                peripherals.leftClick(287, 103);
                lastPuzzle();
            }
        }

        public async void pillars4()
        {
            if (!pillarsComplete())
            {

                for (int i = 0; i < 5; i++)
                {
                    if (pillarOrder[i] != -1)
                    {
                        clickPillar(pillarOrder[i]);
                        await Task.Delay(600);
                    }
                }

                if (!pillarOrder.Contains(2))
                {
                    clickPillar(2);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(4))
                {
                    clickPillar(4);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(0))
                {
                    clickPillar(0);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(3))
                {
                    clickPillar(3);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(1))
                {
                    clickPillar(1);
                    await Task.Delay(600);
                }

                checkPillars();
                peripherals.leftClick(254, 124);
                await Task.Delay(3000);
                peripherals.leftClick(255, 212);
                await Task.Delay(3000);
                pillars();
            }
            else
            {
                peripherals.leftClick(287, 103);
                toStart2nd();
            }
        }

        public async void pillars40()
        {
            if (!pillarsComplete())
            {

                for (int i = 0; i < 5; i++)
                {
                    if (pillarOrder[i] != -1)
                    {
                        clickPillar(pillarOrder[i]);
                        await Task.Delay(600);
                    }
                }

                if (!pillarOrder.Contains(2))
                {
                    clickPillar(2);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(4))
                {
                    clickPillar(4);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(0))
                {
                    clickPillar(0);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(3))
                {
                    clickPillar(3);
                    await Task.Delay(600);
                }

                if (!pillarOrder.Contains(1))
                {
                    clickPillar(1);
                    await Task.Delay(600);
                }

                checkPillars();
                peripherals.leftClick(254, 124);
                await Task.Delay(3000);
                peripherals.leftClick(255, 212);
                await Task.Delay(3000);
                pillars10();
            }
            else
            {
                peripherals.leftClick(287, 103);
                lastPuzzle();
            }
        }

        public void clickPillar(int pillar)
        {
            if(pillar == 0)
            {
                peripherals.leftClick(211, 127);
            }
            if (pillar == 1)
            {
                peripherals.leftClick(209, 153);
            }
            if (pillar == 2)
            {
                peripherals.leftClick(303, 152);
            }
            if (pillar == 3)
            {
                peripherals.leftClick(206, 182);
            }
            if (pillar == 4)
            {
                peripherals.leftClick(305, 183);
            }
        }        

        public async void checkSimonTiles()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(309, 144))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(309, 144));
                }

                for (int i = 238; i < 309; i++)
                {
                    for (int j = 84; j < 144; j++)
                    {
                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 255)
                        {
                            if (i >= 277 && j >= 87)
                            {
                                if (i <= 283 && j <= 92)
                                {
                                    simonSaysTiles[0] = 0;
                                }
                            }
                            if (i >= 266 && j >= 96)
                            {
                                if (i <= 270 && j <= 101)
                                {
                                    simonSaysTiles[0] = 1;
                                }
                            }
                            if (i >= 286 && j >= 94)
                            {
                                if (i <= 294 && j <= 101)
                                {
                                    simonSaysTiles[0] = 2;
                                }
                            }
                            if (i >= 254 && j >= 104)
                            {
                                if (i <= 260 && j <= 109)
                                {
                                    simonSaysTiles[0] = 3;
                                }
                            }
                            if (i >= 277 && j >= 105)
                            {
                                if (i <= 282 && j <= 110)
                                {
                                    simonSaysTiles[0] = 4;
                                }
                            }
                            if (i >= 298 && j >= 103)
                            {
                                if (i <= 306 && j <= 109)
                                {
                                    simonSaysTiles[0] = 5;
                                }
                            }
                            if (i >= 266 && j >= 113)
                            {
                                if (i <= 270 && j <= 119)
                                {
                                    simonSaysTiles[0] = 6;
                                }
                            }
                            if (i >= 291 && j >= 114)
                            {
                                if (i <= 294 && j <= 121)
                                {
                                    simonSaysTiles[0] = 7;
                                }
                            }
                            if (i >= 279 && j >= 124)
                            {
                                if (i <= 283 && j <= 129)
                                {
                                    simonSaysTiles[0] = 8;
                                }
                            }
                        }

                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 204)
                        {
                            if (i >= 277 && j >= 87)
                            {
                                if (i <= 283 && j <= 92)
                                {
                                    simonSaysTiles[1] = 0;
                                }
                            }
                            if (i >= 266 && j >= 96)
                            {
                                if (i <= 270 && j <= 101)
                                {
                                    simonSaysTiles[1] = 1;
                                }
                            }
                            if (i >= 286 && j >= 94)
                            {
                                if (i <= 294 && j <= 101)
                                {
                                    simonSaysTiles[1] = 2;
                                }
                            }
                            if (i >= 254 && j >= 104)
                            {
                                if (i <= 260 && j <= 109)
                                {
                                    simonSaysTiles[1] = 3;
                                }
                            }
                            if (i >= 277 && j >= 105)
                            {
                                if (i <= 282 && j <= 110)
                                {
                                    simonSaysTiles[1] = 4;
                                }
                            }
                            if (i >= 298 && j >= 103)
                            {
                                if (i <= 306 && j <= 109)
                                {
                                    simonSaysTiles[1] = 5;
                                }
                            }
                            if (i >= 266 && j >= 113)
                            {
                                if (i <= 270 && j <= 119)
                                {
                                    simonSaysTiles[1] = 6;
                                }
                            }
                            if (i >= 291 && j >= 114)
                            {
                                if (i <= 294 && j <= 121)
                                {
                                    simonSaysTiles[1] = 7;
                                }
                            }
                            if (i >= 279 && j >= 124)
                            {
                                if (i <= 283 && j <= 129)
                                {
                                    simonSaysTiles[1] = 8;
                                }
                            }
                        }
                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 153)
                        {
                            if (i >= 277 && j >= 87)
                            {
                                if (i <= 283 && j <= 92)
                                {
                                    simonSaysTiles[2] = 0;
                                }
                            }
                            if (i >= 266 && j >= 96)
                            {
                                if (i <= 270 && j <= 101)
                                {
                                    simonSaysTiles[2] = 1;
                                }
                            }
                            if (i >= 286 && j >= 94)
                            {
                                if (i <= 294 && j <= 101)
                                {
                                    simonSaysTiles[2] = 2;
                                }
                            }
                            if (i >= 254 && j >= 104)
                            {
                                if (i <= 260 && j <= 109)
                                {
                                    simonSaysTiles[2] = 3;
                                }
                            }
                            if (i >= 277 && j >= 105)
                            {
                                if (i <= 282 && j <= 110)
                                {
                                    simonSaysTiles[2] = 4;
                                }
                            }
                            if (i >= 298 && j >= 103)
                            {
                                if (i <= 306 && j <= 109)
                                {
                                    simonSaysTiles[2] = 5;
                                }
                            }
                            if (i >= 266 && j >= 113)
                            {
                                if (i <= 270 && j <= 119)
                                {
                                    simonSaysTiles[2] = 6;
                                }
                            }
                            if (i >= 291 && j >= 114)
                            {
                                if (i <= 294 && j <= 118)
                                {
                                    simonSaysTiles[2] = 7;
                                }
                            }
                            if (i >= 279 && j >= 124)
                            {
                                if (i <= 283 && j <= 129)
                                {
                                    simonSaysTiles[2] = 8;
                                }
                            }
                        }
                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 102)
                        {
                            if (i >= 277 && j >= 87)
                            {
                                if (i <= 283 && j <= 92)
                                {
                                    simonSaysTiles[3] = 0;
                                }
                            }
                            if (i >= 266 && j >= 96)
                            {
                                if (i <= 270 && j <= 101)
                                {
                                    simonSaysTiles[3] = 1;
                                }
                            }
                            if (i >= 286 && j >= 94)
                            {
                                if (i <= 294 && j <= 101)
                                {
                                    simonSaysTiles[3] = 2;
                                }
                            }
                            if (i >= 254 && j >= 104)
                            {
                                if (i <= 260 && j <= 109)
                                {
                                    simonSaysTiles[3] = 3;
                                }
                            }
                            if (i >= 277 && j >= 105)
                            {
                                if (i <= 282 && j <= 110)
                                {
                                    simonSaysTiles[3] = 4;
                                }
                            }
                            if (i >= 298 && j >= 103)
                            {
                                if (i <= 306 && j <= 109)
                                {
                                    simonSaysTiles[3] = 5;
                                }
                            }
                            if (i >= 266 && j >= 113)
                            {
                                if (i <= 270 && j <= 119)
                                {
                                    simonSaysTiles[3] = 6;
                                }
                            }
                            if (i >= 291 && j >= 114)
                            {
                                if (i <= 294 && j <= 121)
                                {
                                    simonSaysTiles[3] = 7;
                                }
                            }
                            if (i >= 279 && j >= 124)
                            {
                                if (i <= 283 && j <= 129)
                                {
                                    simonSaysTiles[3] = 8;
                                }
                            }
                        }
                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G == 51)
                        {
                            if (i >= 277 && j >= 87)
                            {
                                if (i <= 283 && j <= 92)
                                {
                                    simonSaysTiles[4] = 0;
                                }
                            }
                            if (i >= 266 && j >= 96)
                            {
                                if (i <= 270 && j <= 101)
                                {
                                    simonSaysTiles[4] = 1;
                                }
                            }
                            if (i >= 286 && j >= 94)
                            {
                                if (i <= 294 && j <= 101)
                                {
                                    simonSaysTiles[4] = 2;
                                }
                            }
                            if (i >= 254 && j >= 104)
                            {
                                if (i <= 260 && j <= 109)
                                {
                                    simonSaysTiles[4] = 3;
                                }
                            }
                            if (i >= 277 && j >= 105)
                            {
                                if (i <= 282 && j <= 110)
                                {
                                    simonSaysTiles[4] = 4;
                                }
                            }
                            if (i >= 298 && j >= 103)
                            {
                                if (i <= 306 && j <= 109)
                                {
                                    simonSaysTiles[4] = 5;
                                }
                            }
                            if (i >= 266 && j >= 113)
                            {
                                if (i <= 270 && j <= 119)
                                {
                                    simonSaysTiles[4] = 6;
                                }
                            }
                            if (i >= 291 && j >= 114)
                            {
                                if (i <= 294 && j <= 121)
                                {
                                    simonSaysTiles[4] = 7;
                                }
                            }
                            if (i >= 279 && j >= 124)
                            {
                                if (i <= 283 && j <= 129)
                                {
                                    simonSaysTiles[4] = 8;
                                }
                            }
                        }



                    }

                }
                //MessageBox.Show(simonSaysTiles[0] + " " + simonSaysTiles[1] + " " + simonSaysTiles[2] + " " + simonSaysTiles[3] + " " + simonSaysTiles[4]);
                peripherals.leftClick(255, 135);
                aWalk();
            }
        }


        public async void leaveRoom()
        {

        }

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            player.setCoords(x, y);
            //bank.setCoords(x, y);
            interfaces.setCoords(x, y);
            peripherals.setCoords(x, y);
        }

        public bool puzzleActive()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(350, 150))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(350, 150));
                }

                for (int i = 0; i < 350; i++)
                {
                    for (int j = 0; j < 150; j++)
                    {
                        if (bitmap.GetPixel(i, j).B == 255 && bitmap.GetPixel(i, j).G <= 60)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public async void findRedSquareSelf()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 185, gameScreenCoords[1] + 135, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0)
                        {
                            peripherals.leftClick(i + 185, j + 135);
                        }
                    }
                }
            }
        }
        public async void findBlueSquareSelf()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 185, gameScreenCoords[1] + 135, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 0 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 255)
                        {
                            peripherals.leftClick(i + 185, j + 135);
                        }
                    }
                }
            }
        }

        public async void findYellowSquareSelf()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 185, gameScreenCoords[1] + 135, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 255 && bitmap.GetPixel(i, j).B == 0)
                        {
                            peripherals.leftClick(i + 185, j + 135);
                        }
                    }
                }
            }
        }


        public async void findRedSquareLeft()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 65, gameScreenCoords[1] + 147, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0)
                        {
                            peripherals.leftClick(i, j);
                        }
                    }
                }
            }
        }
        public async void findBlueSquareLeft()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 65, gameScreenCoords[1] + 147, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 0 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 255)
                        {
                            peripherals.leftClick(i + 185, j + 135);
                        }
                    }
                }
            }
        }

        public async void findYellowSquareLeft()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 65, gameScreenCoords[1] + 147, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 255 && bitmap.GetPixel(i, j).B == 0)
                        {
                            peripherals.leftClick(i + 185, j + 135);
                        }
                    }
                }
            }
        }

        public async void findRedSquareRight()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 359, gameScreenCoords[1] + 136, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0)
                        {
                            peripherals.leftClick(i, j);
                        }
                    }
                }
            }
        }
        public async void findBlueSquareRight()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 359, gameScreenCoords[1] + 136, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 0 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 255)
                        {
                            peripherals.leftClick(i + 185, j + 135);
                        }
                    }
                }
            }
        }

        public async void findYellowSquareRight()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(133, 98))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 359, gameScreenCoords[1] + 136, 0, 0, new Size(133, 98));
                }

                for (int i = 0; i < 133; i++)
                {
                    for (int j = 0; j < 98; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 255 && bitmap.GetPixel(i, j).B == 0)
                        {
                            peripherals.leftClick(i + 185, j + 135);
                        }
                    }
                }
            }
        }

        public bool numPuzzleActive()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(350, 150))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(350, 150));
                }

                for (int i = 0; i < 350; i++)
                {
                    for (int j = 0; j < 150; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255 && bitmap.GetPixel(i, j).G == 0 && bitmap.GetPixel(i, j).B == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool inRoom()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(25, 25))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 19, gameScreenCoords[1] + 100, 0, 0, new Size(25, 25));
                }

                for (int i = 0; i < 24; i++)
                {
                    for (int j = 0; j < 24; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 250)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool atBottomTile()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 641, gameScreenCoords[1] + 108, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 225 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool atMiddleTile()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 639, gameScreenCoords[1] + 117, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 225 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool throughBarrier()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 657, gameScreenCoords[1] + 125, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool throughHole()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 646, gameScreenCoords[1] + 141, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool tilesLeft()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(661, 40).R >= 220 && bitmap.GetPixel(661, 40).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool tilesRight()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                if (bitmap.GetPixel(613, 41).R >= 220 && bitmap.GetPixel(613, 41).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool pillarPuzStart()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 640, gameScreenCoords[1] + 117, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool pillarPuzStart2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(5, 5))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 613, gameScreenCoords[1] + 70, 0, 0, new Size(5, 5));
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 191 && bitmap.GetPixel(i, j).G == 191)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool stepPuzzle()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(70, 50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 222, gameScreenCoords[1] + 67, 0, 0, new Size(70, 50));
                }

                for (int i = 0; i < 69; i++)
                {
                    for (int j = 0; j < 49; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 255)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public void getBarrierCoords()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(617, 38).R >= 220 && bitmap.GetPixel(617, 38).G == 0)
                {
                    barrierx = 201;
                    barriery = 70;
                }
                if (bitmap.GetPixel(617, 42).R >= 220 && bitmap.GetPixel(617, 42).G == 0)
                {
                    barrierx = 200;
                    barriery = 76;
                }
                if (bitmap.GetPixel(613, 38).R >= 220 && bitmap.GetPixel(613, 38).G == 0)
                {
                    barrierx = 193;
                    barriery = 71;
                }
                if (bitmap.GetPixel(613, 42).R >= 220 && bitmap.GetPixel(613, 42).G == 0)
                {
                    barrierx = 193;
                    barriery = 76;
                }
                if (bitmap.GetPixel(609, 38).R >= 220 && bitmap.GetPixel(609, 38).G == 0)
                {
                    barrierx = 182;
                    barriery = 70;
                }
                if (bitmap.GetPixel(609, 42).R >= 220 && bitmap.GetPixel(609, 42).G == 0)
                {
                    barrierx = 182;
                    barriery = 78;
                }
            }
        }

        public void getPlayerPosition()
        {
            playerPos = -1;
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(640, 125).R >= 220 && bitmap.GetPixel(640, 125).G == 0)
                {
                    playerPos = 0;
                }
                if (bitmap.GetPixel(642, 121).R >= 220 && bitmap.GetPixel(642, 121).G == 0)
                {
                    playerPos = 1;
                }
                if (bitmap.GetPixel(640, 121).R >= 220 && bitmap.GetPixel(640, 121).G == 0)
                {
                    playerPos = 2;
                }
                if (bitmap.GetPixel(634, 122).R >= 220 && bitmap.GetPixel(634, 122).G == 0)
                {
                    playerPos = 3;
                }
                if (bitmap.GetPixel(646, 117).R >= 220 && bitmap.GetPixel(646, 117).G == 0)
                {
                    playerPos = 4;
                }
                if (bitmap.GetPixel(642, 117).R >= 220 && bitmap.GetPixel(642, 117).G == 0)
                {
                    playerPos = 5;
                }
                if (bitmap.GetPixel(640, 117).R >= 220 && bitmap.GetPixel(640, 117).G == 0)
                {
                    playerPos = 6;
                }
                if (bitmap.GetPixel(634, 118).R >= 220 && bitmap.GetPixel(634, 118).G == 0)
                {
                    playerPos = 7;
                }
                if (bitmap.GetPixel(630, 118).R >= 220 && bitmap.GetPixel(630, 118).G == 0)
                {
                    playerPos = 8;
                }
                if (bitmap.GetPixel(642, 113).R >= 220 && bitmap.GetPixel(642, 113).G == 0)
                {
                    playerPos = 9;
                }
                if (bitmap.GetPixel(640, 113).R >= 220 && bitmap.GetPixel(640, 113).G == 0)
                {
                    playerPos = 10;
                }
                if (bitmap.GetPixel(634, 114).R >= 220 && bitmap.GetPixel(634, 114).G == 0)
                {
                    playerPos = 11;
                }
                if (bitmap.GetPixel(640, 109).R >= 220 && bitmap.GetPixel(640, 109).G == 0)
                {
                    playerPos = 12;
                }
                if (bitmap.GetPixel(646, 125).R >= 220 && bitmap.GetPixel(646, 125).G == 0)
                {
                    playerPos = 13;
                }
                if (bitmap.GetPixel(630, 126).R >= 220 && bitmap.GetPixel(630, 126).G == 0)
                {
                    playerPos = 14;
                }
                if (bitmap.GetPixel(646, 109).R >= 220 && bitmap.GetPixel(646, 109).G == 0)
                {
                    playerPos = 15;
                }
                if (bitmap.GetPixel(630, 110).R >= 220 && bitmap.GetPixel(630, 110).G == 0)
                {
                    playerPos = 16;
                }
            }
        }

        public void getWalkTilePosition()
        {
            playerPos = -1;
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(646, 125).R >= 220 && bitmap.GetPixel(646, 125).G == 0)
                {
                    playerPos = 0;
                }
                if (bitmap.GetPixel(640, 125).R >= 220 && bitmap.GetPixel(640, 125).G == 0)
                {
                    playerPos = 1;
                }
                if (bitmap.GetPixel(630, 126).R >= 220 && bitmap.GetPixel(630, 126).G == 0)
                {
                    playerPos = 2;
                }
                if (bitmap.GetPixel(646, 117).R >= 220 && bitmap.GetPixel(646, 117).G == 0)
                {
                    playerPos = 3;
                }
                if (bitmap.GetPixel(630, 118).R >= 220 && bitmap.GetPixel(630, 118).G == 0)
                {
                    playerPos = 4;
                }
                if (bitmap.GetPixel(646, 109).R >= 220 && bitmap.GetPixel(646, 109).G == 0)
                {
                    playerPos = 5;
                }
                if (bitmap.GetPixel(640, 109).R >= 220 && bitmap.GetPixel(640, 109).G == 0)
                {
                    playerPos = 6;
                }
                if (bitmap.GetPixel(630, 110).R >= 220 && bitmap.GetPixel(630, 110).G == 0)
                {
                    playerPos = 7;
                }
            }
        }

        public bool simonPuzzle()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(70, 50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 222, gameScreenCoords[1] + 67, 0, 0, new Size(70, 50));
                }

                for (int i = 0; i < 69; i++)
                {
                    for (int j = 0; j < 49; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 165 && bitmap.GetPixel(i, j).G == 134 && bitmap.GetPixel(i, j).B == 19)
                        {
                            num++;
                        }
                    }
                }
                if (num > 40 && num < 180)
                {
                    return true;
                }
                return false;

            }
        }

        public bool pillarPuzzle()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(70, 50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 229, gameScreenCoords[1] + 70, 0, 0, new Size(70, 50));
                }

                for (int i = 0; i < 69; i++)
                {
                    for (int j = 0; j < 49; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 214 && bitmap.GetPixel(i, j).G == 173 && bitmap.GetPixel(i, j).B == 25)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool pillarPuzzle2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(70, 45))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 223, gameScreenCoords[1] + 70, 0, 0, new Size(70, 45));
                }

                for (int i = 0; i < 70; i++)
                {
                    for (int j = 0; j < 45; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 209 && bitmap.GetPixel(i, j).G == 169 && bitmap.GetPixel(i, j).B == 24)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        public bool numberPuzzle()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(70, 50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 222, gameScreenCoords[1] + 67, 0, 0, new Size(70, 50));
                }

                for (int i = 0; i < 69; i++)
                {
                    for (int j = 0; j < 49; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 165 && bitmap.GetPixel(i, j).G == 134 && bitmap.GetPixel(i, j).B == 19)
                        {
                            num++;
                        }
                    }
                }
                if (num > 150)
                {
                    return true;
                }
                return false;

            }
        }

        public bool numberPuzzle2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(50, 40))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 236, gameScreenCoords[1] + 77, 0, 0, new Size(50, 40));
                }

                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 40; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 165 && bitmap.GetPixel(i, j).G == 134 && bitmap.GetPixel(i, j).B == 19)
                        {
                            num++;
                        }
                    }
                }
                if (num > 120)
                {
                    return true;
                }
                return false;

            }
        }

    }
}
