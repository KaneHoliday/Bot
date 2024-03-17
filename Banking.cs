using Corp.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp
{
    internal class Banking
    {
        Interfaces interfaces = new Interfaces();
        Peripherals peripherals = new Peripherals();
        public Mouse mouse;

        public int posx;
        public int posy;
        public int[] gameScreenCoords = new int[2] { 0, 0 };

        public bool busy = false;

        public async void depositAllInventory()
        {
            peripherals.leftClick(441, 310);
        }

        public async void depositAllEquipment()
        {
            peripherals.leftClick(476, 310);
        }

        public async void switchTab(int tab)
        {
            switch (tab)
            {
                case 0: //main tab
                    peripherals.leftClick(76, 57);
                    break;
                case 1:
                    peripherals.leftClick(116, 57);
                    break;
                case 2:
                    peripherals.leftClick(159, 56);
                    break;
                case 3:
                    peripherals.leftClick(198, 57);
                    break;
                case 4:
                    peripherals.leftClick(238, 56);
                    break;
                case 5:
                    peripherals.leftClick(278, 58);
                    break;
                case 6:
                    peripherals.leftClick(318, 59);
                    break;
                case 7:
                    peripherals.leftClick(356, 56);
                    break;
                case 8:
                    peripherals.leftClick(339, 57);
                    break;
                case 9:
                    peripherals.leftClick(440, 597);
                    break;
                default:
                    MessageBox.Show("Incorrect input for switch tab.");
                    break;
            }
        }

        public async void withdrawAs(string with)
        {
            switch (with)
            {
                case "Item":
                    peripherals.leftClick(142, 318);
                    break;
                case "Note":
                    peripherals.leftClick(195, 320);
                    break;
                default:
                    MessageBox.Show("Incorrect input for withdraw as.");
                    break;
            }
        }

        public async void withdrawItem(string item, int amount)
        {
            if (item == "Book of the dead")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(94, 76, 136))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Dragon dagger")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(21, 195, 26))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Rune pouch")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(106, 131, 135))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Dragon claws")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(56, 8, 0))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Super restore")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(182, 63, 111))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Ranging potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(86, 176, 211))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Eternal boots")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(222, 194, 24))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Saradomin brew")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(203, 202, 96))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Ahrims robeskirt")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(67, 67, 53))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Imbued zamorak cape")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(26, 0, 0))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Pegasian boots")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(55, 72, 8))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Toxic blowpipe")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(24, 228, 179))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Virtus robe top")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(149, 149, 137))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Occult nec")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(59, 6, 62))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Assembler")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(132, 24, 12))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Masori legs")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(0, 35, 26))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Virtus mask")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(176, 176, 164))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Shadow of tumken")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(135, 26, 15))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Anguish")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(230, 119, 24) || bitmap.GetPixel(i, j) == Color.FromArgb(97, 109, 86))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Masori body")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(414, 209))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 58, gameScreenCoords[1] + 77, 0, 0, new Size(414, 209));
                    }

                    for (int i = 0; i < 414; i++)
                    {
                        for (int j = 0; j < 209; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(47, 53, 61))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    mouse.addMouseClick(gameScreenCoords[0] + i + 61, gameScreenCoords[1] + j + 79);
                                }
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Panic tab")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(24, 31, 220) || bitmap.GetPixel(i, j) == Color.FromArgb(119, 159, 161) || bitmap.GetPixel(i, j) == Color.FromArgb(135, 170, 171))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    peripherals.busy = true;
                                    peripherals.leftClick(i + 61, j + 79);
                                    while(peripherals.busy)
                                    {
                                        await Task.Delay(10);
                                    }
                                }
                                busy = false;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Prayer Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(26, 162, 110) || bitmap.GetPixel(i, j) == Color.FromArgb(29, 190, 129) || bitmap.GetPixel(i, j) == Color.FromArgb(48, 203, 151) || bitmap.GetPixel(i, j) == Color.FromArgb(50, 210, 155) || bitmap.GetPixel(i, j) == Color.FromArgb(48, 201, 149) || bitmap.GetPixel(i, j) == Color.FromArgb(43, 179, 133))
                            {
                                    for (int y = 0; y < amount; y++)
                                    {
                                        peripherals.busy = true;
                                        peripherals.leftClick(i + 61, j + 79);
                                        while (peripherals.busy)
                                        {
                                            await Task.Delay(10);
                                        }
                                    }   
                                busy = false;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Dragon Warhammer")
            { 
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(105, 19, 12) || bitmap.GetPixel(i, j) == Color.FromArgb(73, 9, 3) || bitmap.GetPixel(i, j) == Color.FromArgb(77, 15, 8) || bitmap.GetPixel(i, j) == Color.FromArgb(105, 19, 12) || bitmap.GetPixel(i, j) == Color.FromArgb(108, 21, 12))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    peripherals.busy = true;
                                    peripherals.leftClick(i + 61, j + 79);
                                    while (peripherals.busy)
                                    {
                                        await Task.Delay(10);
                                    }
                                }
                                busy = false;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Games nec")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(143, 119, 9) || bitmap.GetPixel(i, j) == Color.FromArgb(143, 199, 9) || bitmap.GetPixel(i, j) == Color.FromArgb(162, 141, 19))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    peripherals.busy = true;
                                    peripherals.leftClick(i + 61, j + 79);
                                    while (peripherals.busy)
                                    {
                                        await Task.Delay(10);
                                    }
                                }
                                busy = false;
                                return;
                            }
                        }
                    }
                }
            }

            if (item == "Shark")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(174, 142, 115) || bitmap.GetPixel(i, j) == Color.FromArgb(161, 124, 95) || bitmap.GetPixel(i, j) == Color.FromArgb(164, 134, 109) || bitmap.GetPixel(i, j) == Color.FromArgb(103, 78, 56) || bitmap.GetPixel(i, j) == Color.FromArgb(139, 113, 92) || bitmap.GetPixel(i, j) == Color.FromArgb(165, 135, 110) || bitmap.GetPixel(i, j) == Color.FromArgb(64, 48, 35))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    peripherals.busy = true;
                                    peripherals.leftClick(i + 61, j + 79);
                                    while (peripherals.busy)
                                    {
                                        await Task.Delay(10);
                                    }
                                }
                                busy = false;
                                return;
                            }
                        }
                    }
                }
            }
            if (item == "Super Attack Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(45, 48, 188) || bitmap.GetPixel(i, j) == Color.FromArgb(26, 29, 162) || bitmap.GetPixel(i, j) == Color.FromArgb(70, 71, 209) || bitmap.GetPixel(i, j) == Color.FromArgb(27, 30, 174))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    peripherals.busy = true;
                                    peripherals.leftClick(i + 61, j + 79);
                                    while (peripherals.busy)
                                    {
                                        await Task.Delay(10);
                                    }
                                }
                                busy = false;
                                return;
                            }
                        }
                    }
                }
            }
            if (item == "Super Combat Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(13, 88, 4) || bitmap.GetPixel(i, j) == Color.FromArgb(12, 77, 4) || bitmap.GetPixel(i, j) == Color.FromArgb(10, 69, 3) || bitmap.GetPixel(i, j) == Color.FromArgb(12, 77, 4) || bitmap.GetPixel(i, j) == Color.FromArgb(21, 92, 8) || bitmap.GetPixel(i, j) == Color.FromArgb(25, 105, 10) || bitmap.GetPixel(i, j) == Color.FromArgb(18, 80, 7))
                            {
                                for (int y = 0; y < amount; y++)
                                {
                                    peripherals.busy = true;
                                    peripherals.leftClick(i + 61, j + 79);
                                    while (peripherals.busy)
                                    {
                                        await Task.Delay(10);
                                    }
                                }
                                busy = false;
                                return;
                            }
                        }
                    }
                }
            }
        }

        public bool isAtTop()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(800, 800))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0], gameScreenCoords[1], 0, 0, new Size(800, 800));
                }
                    if (bitmap.GetPixel(480, 94).R > 0 && bitmap.GetPixel(480, 94).G > 0 && bitmap.GetPixel(480, 94).B > 1)
                        {
                            return false;
                        }
                return true;

            }
        }

        public bool hasItem(string item)
        {
            if (item == "Prayer potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(26, 162, 110) || bitmap.GetPixel(i, j) == Color.FromArgb(29, 190, 129) || bitmap.GetPixel(i, j) == Color.FromArgb(48, 203, 151) || bitmap.GetPixel(i, j) == Color.FromArgb(50, 210, 155) || bitmap.GetPixel(i, j) == Color.FromArgb(48, 201, 149) || bitmap.GetPixel(i, j) == Color.FromArgb(43, 179, 133))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                }
            }
            if (item == "Shark")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(174, 142, 115) || bitmap.GetPixel(i, j) == Color.FromArgb(161, 124, 95) || bitmap.GetPixel(i, j) == Color.FromArgb(164, 134, 109) || bitmap.GetPixel(i, j) == Color.FromArgb(103, 78, 56) || bitmap.GetPixel(i, j) == Color.FromArgb(139, 113, 92) || bitmap.GetPixel(i, j) == Color.FromArgb(165, 135, 110) || bitmap.GetPixel(i, j) == Color.FromArgb(64, 48, 35))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            if (item == "Super Attack Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(90, 91, 211) || bitmap.GetPixel(i, j) == Color.FromArgb(26, 29, 162) || bitmap.GetPixel(i, j) == Color.FromArgb(70, 71, 209) || bitmap.GetPixel(i, j) == Color.FromArgb(27, 30, 174))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            if (item == "Super Combat Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(386, 210))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gameScreenCoords[0] + 61, gameScreenCoords[1] + 79, 0, 0, new Size(386, 210));
                    }

                    for (int i = 0; i < 386; i++)
                    {
                        for (int j = 0; j < 210; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(12, 77, 4) | bitmap.GetPixel(i, j) == Color.FromArgb(10, 69, 3) | bitmap.GetPixel(i, j) == Color.FromArgb(12, 77, 4) || bitmap.GetPixel(i, j) == Color.FromArgb(21, 92, 8) || bitmap.GetPixel(i, j) == Color.FromArgb(25, 105, 10) || bitmap.GetPixel(i, j) == Color.FromArgb(18, 80, 7))
                            {
                                posx = i;
                                posy = j;
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            return false;
        }
        public bool isOpen()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(23, 23))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(gameScreenCoords[0] + 49, gameScreenCoords[1] + 9, 0, 0, new Size(23, 23));
                }

                for (int i = 0; i < 23; i++)
                {
                    for (int j = 0; j < 23; j++)
                    {
                        if (bitmap.GetPixel(i, j).R >= 255 && bitmap.GetPixel(i, j).G == 152 && bitmap.GetPixel(i, j).B == 31)
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
            interfaces.setCoords(x, y);
            peripherals.setCoords(x, y);
        }
    }
}
