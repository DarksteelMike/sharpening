using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    class Cost
    {
        private string description;
        internal string Description
        {
            get { return description; }
        }

        private Effect doPayment;
        internal Effect DoPayment
        {
            get { return doPayment; }
        }

        private bool isPaid;
        internal bool IsPaid
        {
            get { return isPaid; }
        }

        private bool isManaCost;
        internal bool IsManaCost
        {
            get { return isManaCost; }
        }

        public Cost(string d, Effect p, bool i)
        {
            description = d;
            doPayment = p;
            isManaCost = i;
        }
    }
}
