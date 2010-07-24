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

        public Cost(string d, Effect p)
        {
            description = d;
            doPayment = p;
        }
    }
}
