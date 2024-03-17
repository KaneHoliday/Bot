using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using EasyHook;

namespace Corp
{
    public partial class Peripherals : Form
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        const int KEY_DOWN_EVENT = 0x0001; //Key down flag
        const int KEY_UP_EVENT = 0x0002; //Key up flag

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        public bool busy = false;

        public int[] mouseClicks = { };
        public int[] urgentMouseClicks = { };

        public async void mouseClickLoop()
        {
            while(mouseClicks == null && busy)
            {
                await Task.Delay(10);
            }
            busy = true;
            leftClick(mouseClicks[0], mouseClicks[1]);
        }

        public void addMouseClick(int x, int y)
        {
            mouseClicks.Append(x);
            mouseClicks.Append(y);
        }

        public void addUrgentMouseClick(int x, int y)
        {

        }

        public enum WMessages : int
        {
            WM_LBUTTONDOWN = 0x201, //Left mousebutton down
            WM_LBUTTONUP = 0x202,   //Left mousebutton up
            WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
            WM_RBUTTONDOWN = 0x204, //Right mousebutton down
            WM_RBUTTONUP = 0x205,   //Right mousebutton up
            WM_RBUTTONDBLCLK = 0x206, //Right mousebutton do
            WM_PARENTNOTIFY = 0x0210,
            WM_TEST = 0xC207,
            WM_MOVEMOUSE = 0x0200,
            WM_NCHITTEST = 0x0084,
            WM_SETCURSOR = 0x0020,
            WM_ACTIVATEAPP = 0x001C,
        }


        public static int[] gameScreenCoords = new int[2] { 0, 0 };

        public static void HoldKey(byte key, int duration)
        {
                keybd_event(key, 0, KEY_DOWN_EVENT, 0);
                System.Threading.Thread.Sleep(100*duration);
                keybd_event(key, 0, KEY_UP_EVENT, 0);
        }

        public static void PressKey(byte key, int duration)
        {
            int totalDuration = 0;
            while (totalDuration < duration)
            {
                keybd_event(key, 0, KEY_DOWN_EVENT, 0);
                System.Threading.Thread.Sleep(100);
                keybd_event(key, 0, KEY_UP_EVENT, 0);
                totalDuration += 1;
            }
        }

        public async void leftClick(int x, int y)
        {
            Cursor.Position = new Point(gameScreenCoords[0] +  x, gameScreenCoords[1] + y);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0], gameScreenCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_LEFTUP, gameScreenCoords[0], gameScreenCoords[1], 0, 0);
            await Task.Delay(100);
            busy = false;
        }

        public async void moveMouse(int x, int y)
        {
            Cursor.Position = new Point(x, y);
            await Task.Delay(30);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, gameScreenCoords[0], gameScreenCoords[1], 0, 0);
            await Task.Delay(30);
            mouse_event(MOUSEEVENTF_RIGHTUP, gameScreenCoords[0], gameScreenCoords[1], 0, 0);
            await Task.Delay(30);
        }
        public async void fastLeftClick(int x, int y)
        {
            Cursor.Position = new Point(gameScreenCoords[0] + x, gameScreenCoords[1] + y);
            await Task.Delay(30);
            mouse_event(MOUSEEVENTF_LEFTDOWN, gameScreenCoords[0], gameScreenCoords[1], 0, 0);
            await Task.Delay(30);
            mouse_event(MOUSEEVENTF_LEFTUP, gameScreenCoords[0], gameScreenCoords[1], 0, 0);
            await Task.Delay(30);
            busy = false;
        }


        public async void rightClick(int x, int y)
        {
            Cursor.Position = new Point(gameScreenCoords[0] + x, gameScreenCoords[1] + y);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, gameScreenCoords[0], gameScreenCoords[1], 0, 0);
            await Task.Delay(10);
            mouse_event(MOUSEEVENTF_RIGHTUP, gameScreenCoords[0], gameScreenCoords[1], 0, 0);
            await Task.Delay(100);
            busy = false;
        }

        public void setCoords(int x, int y)
        {
            gameScreenCoords[0] = x;
            gameScreenCoords[1] = y;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Peripherals
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Peripherals";
            this.Load += new System.EventHandler(this.Peripherals_Load);
            this.ResumeLayout(false);

        }

        private void Peripherals_Load(object sender, EventArgs e)
        {

        }
    }
}
