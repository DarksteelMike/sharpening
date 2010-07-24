using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    class Spell
    {
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }

        private Effect resolve;
        internal Effect Resolve
        {
            get { return resolve; }
        }

        private string description;
        internal string Description
        {
            get { return description; }
        }

        public Spell(CardBase c, Effect r,string d)
        {
            cardSrc = c;
            resolve = r;
            description = d;
        }
    }
}
