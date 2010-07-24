using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal enum ManaColor { White,Blue,Black,Red,Green,Colorless };
    class ManaPoint
    {
        private ManaColor type;
        internal ManaColor Type
        {
            get { return type; }
        }

        private Condition tag;
        internal Condition Tag
        {
            get { return tag; }
        }

        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }

        internal ManaPoint(ManaColor typ, Condition t, CardBase crd)
        {
            type = typ;
            tag = t;
            cardSrc = crd;
        }
    }
}
