using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class Logout_timer_extention
    {

        public async void startScript()
        {
            Peripherals.PressKey((byte)Keys.Space, 1);
            await Task.Delay(1000);
            Peripherals.PressKey((byte)Keys.Enter, 1);
            await Task.Delay(180000);
            startScript();
        }
    }
}
