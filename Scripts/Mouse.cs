using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Mouse
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        public int[] mouseClicks = new int[10000];
        public int[] urgentMouseClicks = {};
        public bool busy = false;

        public void initMouse()
        {
            for (int i = 0; i < 10000; i++)
            {
                mouseClicks[i] = 0;
            }
        }
        public async void mouseClickLoop()
        {
            while (mouseClicks[0] == 0 || mouseClicks[1] == 0 || busy)
            {
                await Task.Delay(20);
            }
            busy = true;
            leftClick(mouseClicks[0], mouseClicks[1]);
            mouseClicks = mouseClicks.Skip(2).ToArray();
            mouseClicks.Append(0);
            mouseClicks.Append(0);
            mouseClickLoop();
        }

        public void addMouseClick(int x, int y)
        {
            for(int i = 0; i < mouseClicks.Length; i++)
            {
                if (mouseClicks[i] == 0)
                {
                    mouseClicks[i] = x;
                    mouseClicks[i + 1] = y;
                    return;
                }
            }
        }

        private async void leftClick(int x, int y)
        {
            Cursor.Position = new Point(x, y);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            await Task.Delay(50);
            busy = false;
        }

    }
}
