using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal class CELayerEntry
    {
        private Effect myEffect;
        internal Effect MyEffect
        { 
            get { return myEffect; }
        }
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }
        private bool isCDA;
        internal bool IsCDA
        {
            get { return isCDA; }
        }

        internal CELayerEntry(Effect e, CardBase c, bool i)
        {
            myEffect = e;
            cardSrc = c;
            isCDA = i;
        }
    }
}
