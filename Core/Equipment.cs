using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Bot.Core
{
    internal class Equipment
    {
        public string[] equipment = new string[11];

        public void setEquipment()
        {
            equipment[0] = "eclipse moon helm";
            equipment[1] = "bandos cloak";
            equipment[2] = "occult necklace";
            equipment[3] = "atlatl dart";
            equipment[4] = "staff of light";
            equipment[5] = "ahrims robetop";
            equipment[6] = "tome of fire";
            equipment[7] = "torag platelegs";
            equipment[8] = "tormented bracelet";
            equipment[9] = "dragon boots";
            equipment[10] = "lightbearer";
        }
    }
}
