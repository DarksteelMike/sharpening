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
        private bool isCastingAbility;
        internal bool IsCastingAbility
        {
        	get { return isCastingAbility; }
        }

        private CompoundCost baseCost;
        internal CompoundCost BaseCost
        {
            get { return baseCost; }
        }
        
        private CompoundCost currentCost;
        internal CompoundCost CurrentCost
        {
        	get 
        	{
        			if(currentCost == null)
        			{
        				return baseCost;
        			}
        			return currentCost;
        	}
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

        internal Activatable(CardBase cardsrc,bool manaability,bool castingability,CompoundCost cost,Condition canbeactivated,CompoundEffect effect,string desc)
        {
            cardSrc = cardsrc;
            isManaAbility = manaability;
            isCastingAbility = castingability;
            baseCost = cost;
            canBeActivated = canbeactivated;
            activatedEffect = effect;
            description = desc;
        }
    }
}
