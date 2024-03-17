using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts.ToA
{
    internal class ToAmain
    {

        public int[] gameScreenCoords = new int[2] { 0, 0 };
        Player player = new Player();
        Inventory inv = new Inventory();
        Interfaces interfaces = new Interfaces();
        Peripherals peripherals = new Peripherals();
        Mouse mouse = new Mouse();
        LootAndRegear lootgear = new LootAndRegear();

        Scarab scarab = new Scarab();
        Croc croc = new Croc();

        int playerPos = -1;

        public async void startScript()
        {
            mouse.initMouse();
            mouse.mouseClickLoop();
            lootgear.mouse = mouse;
            await Task.Delay(100);
            lootgear.regear();
            //while (!lootgear.ready)
            //{
            //    await Task.Delay(100);
            //}
            //if (inParty())
            //{
            //    getPlayerPosition();
            //    await Task.Delay(400);
            //    enterRaid(0);
            //} else
            //{
            //    getPlayerPosition();
            //   await Task.Delay(400);
            //    makeGroup(0);
            //}
        }

        public async void makeGroup(int state)
        {
            switch (state)
            {
                case 0: //from bank
                    if (playerPos == 0)
                    {
                        peripherals.leftClick(330, 144);
                    }
                    if (playerPos == 1)
                    {
                        peripherals.leftClick(321, 153);
                    }
                    if (playerPos == 2)
                    {
                        peripherals.leftClick(325, 164);
                    }
                    if (playerPos == 3)
                    {
                        peripherals.leftClick(350, 179);
                    }
                    if (playerPos == 4)
                    {
                        peripherals.leftClick(338, 178);
                    }
                    waitForPartyList();
                    break;
                case 1: //from door
                    break;
                default:
                    break;

            }
        }
        
        public async void waitForPartyList()
        {
            if(interfaces.toaPartyList())
            {
                peripherals.leftClick(257, 306);
                await Task.Delay(600);
                Peripherals.HoldKey((byte)Keys.Escape, 1);
                getPlayerPosition();
                await Task.Delay(600);
                enterRaid(1);
            } else
            {
                await Task.Delay(600);
                waitForPartyList();
            }
        }

        public async void enterRaid(int state)
        {
            switch (state)
            {
                case 0: //from bank
                    if(playerPos == 0)
                    {
                        peripherals.leftClick(355, 250);
                    }
                    if (playerPos == 1)
                    {
                        peripherals.leftClick(354, 279);
                    }
                    if (playerPos == 2)
                    {
                        peripherals.leftClick(360, 306);
                    }
                    if (playerPos == 3)
                    {
                        peripherals.leftClick(405, 328);
                    }
                    if (playerPos == 4)
                    {
                        peripherals.leftClick(370, 229);
                    }
                    waitForRaidStart();
                    break;
                case 1: //from Obelisk
                    if (playerPos == 5)
                    {
                        peripherals.leftClick(300, 326);
                    }
                    if (playerPos == 6)
                    {
                        peripherals.leftClick(303, 310);
                    }
                    if (playerPos == 7)
                    {
                        peripherals.leftClick(299, 287);
                    }
                    waitForRaidStart();
                    break;
                default:
                    break;

            }
        }

        public async void waitForRaidStart()
        {
            if(inRaid())
            {
                enterRoom(1);
            } else
            {
                await Task.Delay(600);
                waitForRaidStart();
            }
        }

        public async void enterRoom(int room)
        {
            switch(room)
            {
                case 0: //scarab
                    peripherals.leftClick(410, 242);
                    waitForRoomStart(room);
                    //scarab.startScript();
                    break;
                case 1: // croc
                    peripherals.leftClick(136, 240);
                    waitForRoomStart(room);
                    break;
                case 2: //baba
                    break;
                case 3: //niall
                    break;

            }
        }

        public async void waitForRoomStart(int x)
        {
            if(!inRaid())
            {
                await Task.Delay(3000);
                if(x == 0)
                {
                    scarab.startScript();
                }
                if(x == 1)
                {
                    croc.startScript();
                }
            } else
            {
                await Task.Delay(600);
                waitForRoomStart(x);
            }
        }

        public async void leaveRoom()
        {

        }

        public async void clickBank()
        {

        }

        public bool inParty()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(15, 15))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 41, gameScreenCoords[1] + 58, 0, 0, new Size(15, 15));
                }

                for (int i = 0; i < 14; i++)
                {
                    for (int j = 0; j < 14; j++)
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

                if (bitmap.GetPixel(652, 93).R >= 220 && bitmap.GetPixel(652, 93).G == 0)
                {
                    playerPos = 0;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(648, 97).R >= 220 && bitmap.GetPixel(648, 97).G == 0)
                {
                    playerPos = 1;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(648, 101).R >= 220 && bitmap.GetPixel(648, 101).G == 0)
                {
                    playerPos = 2;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(656, 105).R >= 220 && bitmap.GetPixel(656, 105).G == 0)
                {
                    playerPos = 3;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(652, 105).R >= 220 && bitmap.GetPixel(652, 105).G == 0)
                {
                    playerPos = 4;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(636, 105).R >= 220 && bitmap.GetPixel(636, 105).G == 0)
                {
                    playerPos = 5;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(636, 101).R >= 220 && bitmap.GetPixel(636, 101).G == 0)
                {
                    playerPos = 6;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
                if (bitmap.GetPixel(636, 97).R >= 220 && bitmap.GetPixel(636, 97).G == 0)
                {
                    playerPos = 7;
                    //MessageBox.Show("Player pos: " + playerPos);
                    return;
                }
            }

            //MessageBox.Show("player pos: " + playerPos);

        }

        public bool inRaid()
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

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
            player.setCoords(x, y);
            //bank.setCoords(x, y);
            interfaces.setCoords(x, y);
            peripherals.setCoords(x, y);
            scarab.setCoords(x, y);
            croc.setCoords(x, y);
            lootgear.bank.setCoords(x, y);
        }
    }
}
