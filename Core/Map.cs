using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core
{
    internal class Map
    {
        public int[] clientCoords = new int[2];

        public bool atBench()
        {
            return true;
        }

        public bool atTeleportspot()
        {
            return true;
        }

        public bool atGuardian()
        {
            return true;
        }
    }
}
