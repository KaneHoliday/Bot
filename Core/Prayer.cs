using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core
{
    internal class Prayer
    {
        public CoreProcessor processor;
        public int[] prayerArray = new int[10];
        public int activePrayer = 0;
        public int boostPrayer = 0;

        public void setPrayerArray()
        {
            for (int i = 0; i < prayerArray.Length; i++)
            {
                prayerArray[i] = 0;
            }
        }
        public void checkPrayer()
        {
            if (prayerArray[processor.tick] != activePrayer)
            {
                switch(prayerArray[processor.tick])
                {
                    case 0:
                        if(activePrayer == 1)
                        {
                            prayMage();
                        }
                        if (activePrayer == 2)
                        {
                            prayRange();
                        }
                        if (activePrayer == 3)
                        {
                            prayMelee();
                        }
                        break;
                    case 1:
                        prayMage();
                        break;
                    case 2:
                        prayRange();
                        break;
                    case 3:
                        prayMelee();
                        break;
                    default:
                        break;
                }
            }
        }

        public void solidMagic()
        {
            for (int i = 0; i < prayerArray.Length; i++)
            {
                prayerArray[i] = 1;
            }
        }
        public void solidRange()
        {
            for (int i = 0; i < prayerArray.Length; i++)
            {
                prayerArray[i] = 2;
            }
        }
        public void solidMelee()
        {
            for (int i = 0; i < prayerArray.Length; i++)
            {
                prayerArray[i] = 3;
            }
        }
        public void turnOff()
        {
            for (int i = 0; i < prayerArray.Length; i++)
            {
                prayerArray[i] = 0;
            }
        }

        public void prayRange()
        {
            processor.addMouseClick(640, 340,"prayer");
            if (activePrayer == 2) {
                activePrayer = 0;
            } else
            {
                activePrayer = 2;
            }
        }
        public void prayMelee()
        {
            processor.addMouseClick(680, 340, "prayer");
            if (activePrayer == 3)
            {
                activePrayer = 0;
            }
            else
            {
                activePrayer = 3;
            }
        }
        public void prayMage()
        {
            processor.addMouseClick(600, 340, "prayer");
            if (activePrayer == 1)
            {
                activePrayer = 0;
            }
            else
            {
                activePrayer = 1;
            }
        }

        public void prayPiety()
        {
            processor.addMouseClick(600, 412, "prayer");
            if (boostPrayer == 1)
            {
                boostPrayer = 0;
            }
            else
            {
                boostPrayer = 1;
            }
        }

        public void prayRigour()
        {
            processor.addMouseClick(640, 412, "prayer");
            if (boostPrayer == 2)
            {
                boostPrayer = 0;
            }
            else
            {
                boostPrayer = 2;
            }
        }

        public void prayAugury()
        {
            processor.addMouseClick(680, 412, "prayer");
            if (boostPrayer == 3)
            {
                boostPrayer = 0;
            }
            else
            {
                boostPrayer = 3;
            }
        }
    }
}
