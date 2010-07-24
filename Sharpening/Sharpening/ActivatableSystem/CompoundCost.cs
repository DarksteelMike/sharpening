using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    class CompoundCost
    {
        private List<Cost> costs;
        internal List<Cost> Costs
        {
            get { return costs; }
        }

        internal CompoundCost(params Cost[] c)
        {
            costs = new List<Cost>();
            costs.AddRange(c);
        }

        internal void DoPayment()
        {
            foreach (Cost c in Costs)
            {
                c.DoPayment();
            }
        }
    }
}
