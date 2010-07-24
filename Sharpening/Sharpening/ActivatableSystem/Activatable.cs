using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal class Activatable
    {
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }
        private bool isManaAbility;
        internal bool IsManaAbility
        {
            get { return isManaAbility; }
        }

        private CompoundCost myCost;
        internal CompoundCost MyCost
        {
            get { return myCost; }
        }

        private Condition canBeActivated;
        internal Condition CanBeActivated
        {
            get { return canBeActivated; }
        }

        private CompoundEffect activatedEffect;
        internal CompoundEffect ActivatedEffect
        {
            get { return activatedEffect; }
        }

        private string description;
        internal string Description
        {
            get { return description; }
        }

        internal Activatable(CardBase c,bool m,CompoundCost co,Condition can,CompoundEffect doit,string d)
        {
            cardSrc = c;
            isManaAbility = m;
            myCost = co;
            canBeActivated = can;
            activatedEffect = doit;
            description = d;
        }
    }
}
