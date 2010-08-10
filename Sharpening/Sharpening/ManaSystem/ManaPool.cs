/*
 * Created by SharpDevelop.
 * User: elev
 * Date: 2010-08-10
 * Time: 14:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Sharpening
{
    /// <summary>
    /// Description of ManaPool.
    /// </summary>
    internal class ManaPool
    {
        private List<ManaPoint> pool;
        internal List<ManaPoint> Pool
        {
            get { return pool; }
        }


        internal ManaPool()
        {
            pool = new List<ManaPoint>();
        }

        internal bool CanPay(string ManaCost, Activatable Target)
        {
            //Do not let tagged mana be used unless valid
            List<ManaPoint> UsableMana = new List<ManaPoint>();
            foreach (ManaPoint point in pool)
            {
                if (point.Tag(Target))
                {
                    UsableMana.Add(point);
                }
            }
            List<string> SplitMana = new List<string>();
            SplitMana.AddRange(ManaCost.Split(' '));
            SplitMana.Reverse(); //Check color reqs before colorless.			
            //Check most specific: single nonhybrid specific color
            foreach (string CostPoint in SplitMana)
            {
                if (CostPoint.Length == 1 && !Utility.Numbers.Contains(CostPoint))
                {
                    ManaColor Wanted = Utility.StringToManaColor(CostPoint);
                    bool found = false;
                    foreach (ManaPoint p in UsableMana)
                    {
                        if (p.Type == Wanted)
                        {
                            found = true;
                            SplitMana.Remove(CostPoint);
                            UsableMana.Remove(p);
                        }
                    }
                    if (!found)
                    {
                        return false;
                    }
                }
            }

            //Check second most specific: colored/colored hybrid mana
            foreach (string CostPoint in SplitMana)
            {
                if (CostPoint.Length == 2 && !Utility.StringContainsAny(CostPoint, Utility.Numbers.ToCharArray()))
                {

                }
            }

            //Check third most specific: colorless/colored hybrid mana

            //Check fourth most specific: colorless

            return true;
        }
    }
}
