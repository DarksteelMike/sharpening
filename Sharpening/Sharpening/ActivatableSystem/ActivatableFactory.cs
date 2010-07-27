using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal static class ActivatableFactory
    {
        internal static Activatable Create(Game g,CardBase TargetCard,string ActivatableType)
        {
            bool NewIsManaAbility = false;
            Condition NewCondition = null;
            CompoundCost NewCost = null;
            CompoundEffect NewEffect = null;
            string NewDescription = null;

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
                    SingleCosts.Add(CostFactory.Create(g,s));
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
            }
            else if(ActivatableType.StartsWith("Manatap:"))
            {
                //It's a mana ability
                NewIsManaAbility = true;

                //Can only be activated on the battlefield
                NewCondition = delegate(object[] param) {
                    return (TargetCard.Characteristics.Location == CardLocation.Battlefield) && !TargetCard.Characteristics.IsTapped;
                };

                //Parse how much mana you get and any costs
                string FullCost = ActivatableType.Split(':')[1];
                string[] Costs = FullCost.Split('+');
                string ManaMade = ActivatableType.Split(':')[2];
                NewDescription = "";

                if (FullCost != "-")
                {
                    List<Cost> MyCosts = new List<Cost>();
                    foreach (string c in Costs)
                    {
                        MyCosts.Add(CostFactory.Create(g, c));
                    }

                    NewCost = new CompoundCost(MyCosts.ToArray());
                    NewDescription = NewCost.Description + ",";
                }

                NewEffect = new CompoundEffect(delegate(object[] param) {
                    //Tap the card

                    //Add the mana
                });

                NewDescription += "tap: Add " + ManaMade + " to your manapool.";
            }

            return new Activatable(TargetCard, NewIsManaAbility, NewCost, NewCondition, NewEffect, NewDescription);
        }
    }
}
