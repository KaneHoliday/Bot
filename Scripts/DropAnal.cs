using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Scripts
{
    internal class DropAnal
    {
        public string item;

        public int totalGpMade;

        public int gold()
        {
            switch(item)
            {
                case "Spectral Sigil":
                    return 40000000;
                case "Arcane Sigil":
                    return 120000000;
                case "Elysian Sigil":
                    return 680000000;
                case "Mystic robe top":
                    return 71000;
                case "Mystic robe bottom":
                    return 48000;
                case "Mystic air staff":
                    return 25000;
                case "Mystic earth staff":
                    return 25000;
                case "Mystic water staff":
                    return 25000;
                case "Mystic fire staff":
                    return 25000;
                case "Spirit shield":
                    return 68000;
                case "Soul rune":
                    return 64000;
                case "Runite bolts":
                    return 26000;
                case "Death rune":
                    return 43000;
                case "Onyx bolts":
                    return 1500000;
                case "Cannonball":
                    return 340000;
                case "Adamant arrows":
                    return 9000;
                case "Law rune":
                    return 30000;
                case "cosmic rune":
                    return 40000;
                case "Raw shark":
                    return 22000;
                case "Pure essence":
                    return 5000;
                case "Adamantite bar":
                    return 65000;
                case "Green dragonhide":
                    return 138000;
                case "Runite ore":
                    return 222000;
                case "Teak plank":
                    return 80000;
                case "Mahogany logs":
                    return 62000;
                case "Magic logs":
                    return 83000;
                case "Adamantite ore":
                    return 121000;
                case "Tuna potato": //
                    return 1000;
                case "White berries":
                    return 33600;
                case "Desert goat horn":
                    return 5000;
                case "Watermelon seed":
                    return 48;
                case "Coins":
                    return 20000;
                case "Antidote++":
                    return 240000;
                case "Ranarr seed":
                    return 180000;
                case "Holy elixir":
                    return 620000;
                default:
                    return 0;
            }
        }

        public void analyse(Bitmap drop)
        {
            if (drop.GetPixel(153, 4).G == 95 && drop.GetPixel(153, 9).G == 95 && drop.GetPixel(160, 4).G == 95 && drop.GetPixel(161, 4).G == 95 && drop.GetPixel(162, 4).G == 95 && drop.GetPixel(163, 4).G == 95 && drop.GetPixel(167, 4).G == 95 && drop.GetPixel(170, 4).G == 95 && drop.GetPixel(172, 4).G == 95 && drop.GetPixel(178, 4).G == 95 && drop.GetPixel(182, 4).G == 95 && drop.GetPixel(189, 4).G == 95 && drop.GetPixel(193, 4).G == 95 && drop.GetPixel(197, 4).G == 95)
            {
                item = "Tuna potato";
                drop.Save("Z:/confirmed/" + item + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

    }
}
