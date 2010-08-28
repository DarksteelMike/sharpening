using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpening
{
    internal class CardMovedEventArgs
    {
        private CardLocation from;
        internal CardLocation From
        {
            get { return from; }
        }

        private CardLocation to;
        internal CardLocation To
        {
            get { return to; }
        }

        internal CardMovedEventArgs(CardLocation f, CardLocation t)
        {
            from = f;
            to = t;
        }
    }
}
