using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts.ToA
{
    internal class Croc
    {

        public int[] gameScreenCoords = new int[2] { 0, 0 };
        Player player = new Player();
        Inventory inv = new Inventory();
        Interfaces interfaces = new Interfaces();
        Peripherals peripherals = new Peripherals();
        public int playerPos;

        public bool green1;
        public bool green2;
        public bool green3;

        public bool green1b;
        public bool green2b;
        public bool green3b;

        public int treePosx;
        public int treePosy;

        public async void startScript()
        {
            greenLoop();
            interfaces.rotateCamera("west");
            await Task.Delay(1200);
            getPlayerPosition();
            await Task.Delay(300);
            MessageBox.Show(playerPos.ToString());
            enterRoom(playerPos);
        }

        public async void greenLoop()
        {
            green();
            greenb();
            await Task.Delay(100);
            greenLoop();
        }

        public async void enterRoom(int x)
        {
            switch(x)
            {
                case 0:
                    peripherals.leftClick(296, 130);
                    waitForEnterRoom();
                    break;
                case 1:
                    peripherals.leftClick(281, 130);
                    waitForEnterRoom();
                    break;
                case 2:
                    peripherals.leftClick(271, 123);
                    waitForEnterRoom();
                    break;
                case 3:
                    peripherals.leftClick(272, 131);
                    waitForEnterRoom();
                    break;
                case 4:
                    peripherals.leftClick(259, 123);
                    waitForEnterRoom();
                    break;
                case 5:
                    peripherals.leftClick(258, 131);
                    waitForEnterRoom();
                    break;
            }
        }

        public async void waitForEnterRoom()
        {
            if(entered())
            {
                peripherals.leftClick(318, 64); //jug
                waitForPickup();
            } else
            {
                await Task.Delay(5);
                waitForEnterRoom();
            }
        }

        public async void waitForPickup()
        {
            if(inv.hasItemHalfInv("jug"))
            {
                peripherals.leftClick(305, 198); //tile
                await Task.Delay(200);
                inv.useItemHalfInv("jug");
                getReadyToRun();
            } else
            {
                await Task.Delay(5);
                waitForPickup();
            }
        }

        public async void getReadyToRun()
        {
            if(atTile1())
            {
                peripherals.leftClick(503, 175); //waterfall
                waitToFill1();
            } else
            {
                await Task.Delay(5);
                getReadyToRun();
            }
        }

        public async void waitToFill1()
        {
            if(atWaterfall())
            {
                runback1();
            } else
            {
                await Task.Delay(5);
                waitToFill1();
            }
        }

        public async void runback1()
        {
            if (green1 || green2 || green3)
            {
                if (!green1)
                {
                    peripherals.leftClick(246, 169);
                    await Task.Delay(600);
                    peripherals.leftClick(24, 173);
                    await Task.Delay(300);
                    inv.useItemHalfInv("jug");
                    waitForTree();
                }
                else if (!green2)
                {
                    peripherals.leftClick(243, 181);
                    await Task.Delay(600);
                    peripherals.leftClick(24, 173);
                    await Task.Delay(300);
                    inv.useItemHalfInv("jug");
                    waitForTree();
                }
                else if (!green3)
                {
                    peripherals.leftClick(246, 169);
                    await Task.Delay(600);
                    peripherals.leftClick(224, 192);
                    await Task.Delay(600);
                    peripherals.leftClick(24, 173);
                    await Task.Delay(300);
                    inv.useItemHalfInv("jug");
                    waitForTree();
                }
            }
            else
            {
                await Task.Delay(50);
                runback1();
            }
        }

        public async void waitForTree()
        {
            if(treeInRange())
            {
                peripherals.leftClick(treePosx, treePosy); //click on tree
                waitToTree();
            } else
            {
                await Task.Delay(5);
                waitForTree();
            }
        }
        public async void waitToTree()
        {
            if(atTree())
            {
                peripherals.leftClick(489, 94);
                await Task.Delay(200);
                inv.useItemHalfInv("jug");
                await Task.Delay(300);
                peripherals.leftClick(500, 88); //waterfall 2
                await Task.Delay(4000);
                waitToFill2();
            } else
            {
                await Task.Delay(5);
                waitToTree();
            }
        }

        public async void waitToFill2()
        {
            if (atWaterfall())
            {
                runBack2();
            } else
            {
                await Task.Delay(20);
                waitToFill2();
            }
        }
        public async void runBack2()
        {
            if (!green1b && !green2b && !green3b)
            {
                peripherals.leftClick(34, 183);
                await Task.Delay(300);
                inv.useItemHalfInv("jug");
                waitForTree2();
            } else if (green1b || green2b || green3b)
            {
                if (!green2b)
                {
                    peripherals.leftClick(258, 155);
                    await Task.Delay(1000);
                    peripherals.leftClick(34, 183);
                    await Task.Delay(300);
                    inv.useItemHalfInv("jug");
                    waitForTree2();
                }
                else if (!green3b)
                {
                    peripherals.leftClick(34, 183);
                    await Task.Delay(300);
                    inv.useItemHalfInv("jug");
                    waitForTree2();
                }
            } else
            {
                await Task.Delay(10);
                runBack2();
            }
        }

        public async void waitForTree2()
        {
            if(seeTree())
            {
                peripherals.leftClick(62, 247); //tree
                waitToComplete();
            } else
            {
                await Task.Delay(20);
                waitForTree2();
            }
        }

        public async void waitToComplete()
        {
            if(roomCompleted())
            {
                peripherals.leftClick(229, 32); //exit room
                exitRaid();
            } else { 
                await Task.Delay(20);
                waitToComplete();
            }
        }

        public async void exitRaid()
        {

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

                if (bitmap.GetPixel(630, 102).R >= 220 && bitmap.GetPixel(630, 102).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool roomCompleted()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(633, 142).R >= 220 && bitmap.GetPixel(633, 142).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool atTile1()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(600, 114).R >= 220 && bitmap.GetPixel(600, 114).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool atWaterfall()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(300, 200))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(300, 200));
                }
                for (int i = 260; i < 270; i++)
                {
                    for (int j = 150; j < 160; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 200 && bitmap.GetPixel(i, j).B == 0 && bitmap.GetPixel(i, j).R >= 200)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        public bool atWaterfall2()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(264, 159).G >= 224 && bitmap.GetPixel(264, 159).B == 0 && bitmap.GetPixel(264, 159).R >= 224)
                {
                    return true;
                }
            }
            return false;
        }

        public void green()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(25, 39))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 228, gameScreenCoords[1] + 164, 0, 0, new Size(25, 39));
                }

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 150)
                        {
                            green1 = true;
                            break;
                        } else
                        {
                            green1 = false;
                        }
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 12; j < 20; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 150)
                        {
                            green2 = true;
                            break;
                        } else
                        {
                            green2 = false;
                        }
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 26; j < 34; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 150)
                        {
                            green3 = true;
                            break;
                        } else
                        {
                            green3 = false;
                        }
                    }
                }
            }
        }

        public void greenb()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(25, 39))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 230, gameScreenCoords[1] + 142, 0, 0, new Size(25, 39));
                }

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 150)
                        {
                            green1b = true;
                            break;
                        }
                        else
                        {
                            green1b = false;
                        }
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 11; j < 18; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 150)
                        {
                            green2b = true;
                            break;
                        }
                        else
                        {
                            green2b = false;
                        }
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 23; j < 30; j++)
                    {
                        if (bitmap.GetPixel(i, j).G >= 150)
                        {
                            green3b = true;
                            break;
                        }
                        else
                        {
                            green3b = false;
                        }
                    }
                }
            }
        }

        public bool treeInRange()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(605, 114).R >= 220 && bitmap.GetPixel(605, 114).G == 0)
                {
                    treePosx = 115;
                    treePosy = 98;
                    return true;
                }
                if (bitmap.GetPixel(605, 110).R >= 220 && bitmap.GetPixel(605, 110).G == 0)
                {
                    treePosx = 122;
                    treePosy = 91;
                    return true;
                }
                if (bitmap.GetPixel(605, 106).R >= 220 && bitmap.GetPixel(605, 106).G == 0)
                {
                    treePosx = 125;
                    treePosy = 80;
                    return true;
                }
            }
            return false;
        }

        public bool seeTree()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }

                if (bitmap.GetPixel(600, 29).R >= 220 && bitmap.GetPixel(600, 29).G == 0)
                {
                    return true;
                }
                if (bitmap.GetPixel(600, 25).R >= 220 && bitmap.GetPixel(600, 25).G == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool atTree()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                for (int i = 625; i < 631; i++)
                {
                    for (int j = 10; j < 17; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 220 && bitmap.GetPixel(i, j).G == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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

                if (bitmap.GetPixel(649, 81).R >= 220 && bitmap.GetPixel(649, 81).G == 0)
                {
                    playerPos = 0;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(649, 85).R >= 220 && bitmap.GetPixel(649, 85).G == 0)
                {
                    playerPos = 1;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(645, 81).R >= 220 && bitmap.GetPixel(645, 81).G == 0)
                {
                    playerPos = 2;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(645, 85).R >= 220 && bitmap.GetPixel(645, 85).G == 0)
                {
                    playerPos = 3;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(641, 81).R >= 220 && bitmap.GetPixel(641, 81).G == 0)
                {
                    playerPos = 4;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(641, 85).R >= 220 && bitmap.GetPixel(641, 85).G == 0)
                {
                    playerPos = 5;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
            }

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
        }
    }
}
