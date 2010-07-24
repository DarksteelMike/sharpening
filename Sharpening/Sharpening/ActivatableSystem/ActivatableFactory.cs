using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening.UserInterfaceSystem
{
    internal static class ActivatableFactory
    {
        internal static Activatable CreateActivatable(Game g,CardBase TargetCard,string ActivatableType)
        {
            bool NewIsManaAbility;
            Condition NewCondition;
            CompoundCost NewCost;
            CompoundEffect NewEffect;
            string NewDescription;

            if (ActivatableType.StartsWith("Flashback:"))
            {
                //Flashback is not a mana ability
                NewIsManaAbility = false;

                //Can only be activated when in the graveyard
                NewCondition = delegate(object[] param)
                {
                    return TargetCard.Characteristics.Location == CardLocation.Graveyard;
                };


                //Describe the activatable
                NewDescription = TargetCard.Name + " - Flashback";

                //Construct the cost
                string FullCost = ActivatableType.Split(':')[1];
                string[] Costs = FullCost.Split('+');

                List<Cost> SingleCosts = new List<Cost>();
                foreach (string s in Costs)
                {
                    SingleCosts.Add(CostFactory.CreateCost(g,s));
                }
                NewCost = new CompoundCost(SingleCosts.ToArray());

                //Effect is to the same as hardcasting it, with the addition of exiling it afterwards.
                List<Effect> AllEffects = new List<Effect>();
                Effect AddedEffect = new Effect(delegate(object[] param)
                {
                    TargetCard.Move(CardLocation.Exile);
                });
                AllEffects.AddRange(TargetCard.Activatables[0].ActivatedEffect.MyEffects.ToArray());
                AllEffects.Add(AddedEffect);
                NewEffect = new CompoundEffect(AllEffects.ToArray());

                return new Activatable(TargetCard, false, NewCost, NewCondition, NewEffect, NewDescription);
            }

            return null;
        }
    }
}
