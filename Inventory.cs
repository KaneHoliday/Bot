using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp
{
    internal class Inventory
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public int invx = 554;
        public int invy = 203;
        public int[] gamescreenCoords = new int[2];
        Peripherals mouse = new Peripherals();
        public int counter = 0;

        public int prevX;
        public int prevY;

        //Peripherals peripherals = new Peripherals();


        public void clickInventory(string name)
        {
            if (name == "Energy Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(155, 73, 85) || bitmap.GetPixel(i, j) == Color.FromArgb(95, 43, 52) || bitmap.GetPixel(i, j) == Color.FromArgb(107, 49, 59) || bitmap.GetPixel(i, j) == Color.FromArgb(147, 69, 80) || bitmap.GetPixel(i, j) == Color.FromArgb(152, 70, 83) || bitmap.GetPixel(i, j) == Color.FromArgb(162, 76, 89) || bitmap.GetPixel(i, j) == Color.FromArgb(152, 70, 83) || bitmap.GetPixel(i, j) == Color.FromArgb(149, 70, 82) || bitmap.GetPixel(i, j) == Color.FromArgb(152, 70, 83) || bitmap.GetPixel(i, j) == Color.FromArgb(147, 69, 80) || bitmap.GetPixel(i, j) == Color.FromArgb(112, 53, 61) || bitmap.GetPixel(i, j) == Color.FromArgb(178, 63, 137) || bitmap.GetPixel(i, j) == Color.FromArgb(191, 89, 156) || bitmap.GetPixel(i, j) == Color.FromArgb(187, 84, 152) || bitmap.GetPixel(i, j) == Color.FromArgb(168, 60, 129) || bitmap.GetPixel(i, j) == Color.FromArgb(171, 61, 131) || bitmap.GetPixel(i, j) == Color.FromArgb(178, 63, 137) || bitmap.GetPixel(i, j) == Color.FromArgb(168, 60, 129) || bitmap.GetPixel(i, j) == Color.FromArgb(184, 74, 145) || bitmap.GetPixel(i, j) == Color.FromArgb(178, 63, 137))
                            {
                                mouse.leftClick(invx + i, invy+ j);
                                prevX = invx + i;
                                prevY = invy + j;
                                return;
                            }
                        }
                    }

                }
            }
            if (name == "Stamina potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(140, 98, 49) || bitmap.GetPixel(i, j) == Color.FromArgb(137, 97, 49) || bitmap.GetPixel(i, j) == Color.FromArgb(160, 121, 71) || bitmap.GetPixel(i, j) == Color.FromArgb(148, 113, 66) || bitmap.GetPixel(i, j) == Color.FromArgb(124, 94, 55) || bitmap.GetPixel(i, j) == Color.FromArgb(144, 110, 63) || bitmap.GetPixel(i, j) == Color.FromArgb(107, 49, 59) || bitmap.GetPixel(i, j) == Color.FromArgb(160, 121, 71))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                return;
                            }
                        }
                    }

                }
            }
            if (name == "Panic tab")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(119, 159, 161) || bitmap.GetPixel(i, j) == Color.FromArgb(135, 170, 171))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                return;
                            }
                        }
                    }

                }
            }


            if (name == "Attack Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(70, 71, 209) || bitmap.GetPixel(i, j) == Color.FromArgb(59, 63, 210) || bitmap.GetPixel(i, j) == Color.FromArgb(43, 47, 179))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                return;
                            }
                        }
                    }

                }
            }
            if (name == "Super Attack Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(90, 91, 211) || bitmap.GetPixel(i, j) == Color.FromArgb(26, 29, 162) || bitmap.GetPixel(i, j) == Color.FromArgb(70, 71, 209) || bitmap.GetPixel(i, j) == Color.FromArgb(27, 30, 174))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                return;
                            }
                        }
                    }

                }
            }
            if (name == "Super Combat Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(13, 88, 4) || bitmap.GetPixel(i, j) == Color.FromArgb(12, 77, 4) || bitmap.GetPixel(i, j) == Color.FromArgb(10, 69, 3) || bitmap.GetPixel(i, j) == Color.FromArgb(12, 77, 4) || bitmap.GetPixel(i, j) == Color.FromArgb(21, 92, 8) || bitmap.GetPixel(i, j) == Color.FromArgb(25, 105, 10) || bitmap.GetPixel(i, j) == Color.FromArgb(18, 80, 7))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                return;
                            }
                        }
                    }

                }
            }

            if (name == "Shark")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(174, 142, 115) || bitmap.GetPixel(i, j) == Color.FromArgb(161, 124, 95) || bitmap.GetPixel(i, j) == Color.FromArgb(164, 134, 109) || bitmap.GetPixel(i, j) == Color.FromArgb(103, 78, 56) || bitmap.GetPixel(i, j) == Color.FromArgb(139, 113, 92) || bitmap.GetPixel(i, j) == Color.FromArgb(165, 135, 110) || bitmap.GetPixel(i, j) == Color.FromArgb(64, 48, 35))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                return;
                            }
                        }
                    }

                }
            }
            if (name == "Prayer Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(26, 162, 110) || bitmap.GetPixel(i, j) == Color.FromArgb(29, 190, 129) || bitmap.GetPixel(i, j) == Color.FromArgb(48, 203, 151) || bitmap.GetPixel(i, j) == Color.FromArgb(50, 210, 155) || bitmap.GetPixel(i, j) == Color.FromArgb(48, 201, 149) || bitmap.GetPixel(i, j) == Color.FromArgb(43, 179, 133))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                return;
                            }
                        }
                    }

                }
            }

        }

        public async void useItem(string item)
        {

        }
        public async void useItemHalfInv(string item)
        {
            if (item == "jug")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 170))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 170; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(85, 117, 127))
                            {
                                mouse.rightClick(invx + i, invy + j);
                                await Task.Delay(500);
                                mouse.leftClick(invx + i, invy + j + 50);
                                return;
                            }
                        }
                    }
                }
            }
        }


        public bool hasItemHalfInv(string item)
        {

            if (item == "jug")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 170))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 170; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(85, 117, 127))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                }
            } 
            return false;
        }

        public bool hasItem(string item)
        {
            if (item == "Dragon Warhammer")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(105, 19, 12) || bitmap.GetPixel(i, j) == Color.FromArgb(73, 9, 3) || bitmap.GetPixel(i, j) == Color.FromArgb(77, 15, 8) || bitmap.GetPixel(i, j) == Color.FromArgb(105, 19, 12) || bitmap.GetPixel(i, j) == Color.FromArgb(108, 21, 12))
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                }
            }
            if (item == "Panic tab")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(119, 159, 161) || bitmap.GetPixel(i, j) == Color.FromArgb(135, 170, 171))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            if (item == "Prayer Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
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
            if (item == "jug")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(85, 117, 127))
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
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
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
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(12, 77, 4) | bitmap.GetPixel(i, j) == Color.FromArgb(10, 69, 3) | bitmap.GetPixel(i, j) == Color.FromArgb(12, 77, 4) || bitmap.GetPixel(i, j) == Color.FromArgb(21, 92, 8) || bitmap.GetPixel(i, j) == Color.FromArgb(25, 105, 10) || bitmap.GetPixel(i, j) == Color.FromArgb(18, 80, 7))
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
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
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
            if (item == "Prayer Potion")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(43, 179, 133) || bitmap.GetPixel(i, j) == Color.FromArgb(29, 184, 125) || bitmap.GetPixel(i, j) == Color.FromArgb(29, 184, 125) || bitmap.GetPixel(i, j) == Color.FromArgb(48, 203, 151) || bitmap.GetPixel(i, j) == Color.FromArgb(50, 210, 155) || bitmap.GetPixel(i, j) == Color.FromArgb(48, 201, 149) || bitmap.GetPixel(i, j) == Color.FromArgb(43, 179, 133) || bitmap.GetPixel(i, j) == Color.FromArgb(30, 198, 135) || bitmap.GetPixel(i, j) == Color.FromArgb(32, 203, 138) || bitmap.GetPixel(i, j) == Color.FromArgb(52, 207, 147) || bitmap.GetPixel(i, j) == Color.FromArgb(26, 162, 110))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        public async void rubAmulet(string item)
        {
            if (item == "Games nec")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(143, 119, 9) || bitmap.GetPixel(i, j) == Color.FromArgb(143, 199, 9) || bitmap.GetPixel(i, j) == Color.FromArgb(162, 141, 19))
                            {
                                mouse.rightClick(invx + i, invy + j);
                                await Task.Delay(500);
                                mouse.leftClick(invx + i, invy + j + 60);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public async void equipItem(string item)
        {
            if (item == "Blowpipe")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(110, 16, 158))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                prevX = invx + i;
                                prevY = invy + j;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Bowfa")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(173, 191, 192))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                prevX = invx + i;
                                prevY = invy + j;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Zaryte crossbow")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(65, 61, 65))
                            {
                                mouse.leftClick(invx + gamescreenCoords[0] + i, invy + gamescreenCoords[1] + j);
                                prevX = invx + i;
                                prevY = invy + j;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Dragon Warhammer")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(105, 19, 12) || bitmap.GetPixel(i, j) == Color.FromArgb(73, 9, 3) || bitmap.GetPixel(i, j) == Color.FromArgb(77, 15, 8) || bitmap.GetPixel(i, j) == Color.FromArgb(105, 19, 12) || bitmap.GetPixel(i, j) == Color.FromArgb(108, 21, 12))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                prevX = invx + i;
                                prevY = invy + j;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Dragon crossbow")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(105, 19, 12))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                prevX = invx + i;
                                prevY = invy + j;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "fang")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(52, 56, 52))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                prevX = invx + i;
                                prevY = invy + j;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Fang")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(110, 93, 37) || bitmap.GetPixel(i, j) == Color.FromArgb(64, 68, 64) || bitmap.GetPixel(i, j) == Color.FromArgb(96, 104, 96) || bitmap.GetPixel(i, j) == Color.FromArgb(87, 70, 22) || bitmap.GetPixel(i, j) == Color.FromArgb(49, 55, 49))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                prevX = invx + i;
                                prevY = invy + j;
                                return;
                            }
                        }
                    }

                }
            }
            if (item == "Dragon defender")
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(170, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(gamescreenCoords[0] + invx, gamescreenCoords[1] + invy, 0, 0, new Size(170, 250));
                    }

                    for (int i = 0; i < 170; i++)
                    {
                        for (int j = 0; j < 250; j++)
                        {
                            if (bitmap.GetPixel(i, j) == Color.FromArgb(124, 15, 7) || bitmap.GetPixel(i, j) == Color.FromArgb(121, 14, 6))
                            {
                                mouse.leftClick(invx + i, invy + j);
                                return;
                            }
                        }
                    }

                }
            }
        }

        public void setCoords(int x, int y)
        {
            mouse.setCoords(x, y);
            gamescreenCoords[0] = x;
            gamescreenCoords[1] = y;
        }

        public int getPreviousClick(string s)
        {
            if (s == "x")
            {
                return prevX;
            }
            if (s == "y")
            {
                return prevY;
            }
            return 0;
        }


    }
}
