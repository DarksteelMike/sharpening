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

        internal Activatable(CardBase cardsrc,bool manaability,CompoundCost cost,Condition canbeactivated,CompoundEffect effect,string desc)
        {
            cardSrc = cardsrc;
            isManaAbility = manaability;
            myCost = cost;
            canBeActivated = canbeactivated;
            activatedEffect = effect;
            description = desc;
        }
    }
}
