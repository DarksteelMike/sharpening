using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal static class ActivatableFactory
    {
        internal static Activatable Create(Game g, CardBase TargetCard, string ActivatableType)
        {
            bool NewIsManaAbility = false;
            bool NewIsCastingAbility = false;
            Condition NewCondition = null;
            CompoundCost NewCost = null;
            CompoundEffect NewEffect = null;
            string NewDescription = null;

            if(ActivatableType.StartsWith("CastPermanent"))
            {
            	Effect SpellEffect = new Effect(delegate(object[] param)
            	{
            		TargetCard.Move(CardLocation.Battlefield);
            	});
            	Spell StackEntry = new Spell(TargetCard,SpellEffect,TargetCard.Name);
            	
            	NewCondition = new Condition(delegate(object[] param)
                {
                    bool Result;

                    //Is it our controllers turn?
                    Result = g.Players[g.WhoseTurn] == TargetCard.CurrentCharacteristics.Controller;

                    //Are we in the hand?
                    Result = Result && (TargetCard.CurrentCharacteristics.Location == CardLocation.Hand);

                    return Result;

                });
            	
            	NewEffect = new CompoundEffect(new Effect(delegate(object[] target)
                {
            		g.PushSpell(StackEntry);
                }));

            	List<Cost> Costs = new List<Cost>();
            	foreach(string indivcost in ActivatableType.Split(':')[1].Split(','))
            	{
            		Costs.Add(CostFactory.Create(g,indivcost));
            	}
            	NewCost = new CompoundCost(Costs.ToArray());

                NewDescription = "";

                NewIsManaAbility = false;
                NewIsCastingAbility = true;
            }
            else if (ActivatableType.StartsWith("PlayLand"))
            {
                //Create a casting Activatable
                NewCondition = new Condition(delegate(object[] param)
                {
                    bool Result;

                    //Is it our controllers turn?
                    Result = g.Players[g.WhoseTurn] == TargetCard.CurrentCharacteristics.Controller;

                    //Are we in the hand?
                    Result = Result && (TargetCard.CurrentCharacteristics.Location == CardLocation.Hand);

                    //Has our controller played less than the maximum amount of lands this turn?
                    Result = Result && (TargetCard.CurrentCharacteristics.Controller.LandsPlayedThisTurn < TargetCard.CurrentCharacteristics.Controller.MaxLandPerTurn);

                    return Result;

                });

                NewEffect = new CompoundEffect(new Effect(delegate(object[] target)
                {
                    TargetCard.Move(CardLocation.Battlefield);
                    TargetCard.CurrentCharacteristics.Controller.LandsPlayedThisTurn++;
                }));

                NewCost = new CompoundCost(CostFactory.Create(g, "-"));

                NewDescription = "";

                NewIsManaAbility = false;
                NewIsCastingAbility = true;
            }
            else if (ActivatableType.StartsWith("Flashback:"))
            {
                //Flashback is not a mana ability
                NewIsManaAbility = false;
                
                NewIsCastingAbility = true;

                //Can only be activated when in the graveyard
                NewCondition = delegate(object[] param)
                {
                    return TargetCard.CurrentCharacteristics.Location == CardLocation.Graveyard;
                };


                //Describe the activatable
                NewDescription = TargetCard.Name + " Flashback - " + TargetCard.Activatables[0].Description + " Then exile " + TargetCard.Name + ".";

                //Construct the cost
                string FullCost = ActivatableType.Split(':')[1];
                string[] Costs = FullCost.Split('+');

                List<Cost> SingleCosts = new List<Cost>();
                foreach (string s in Costs)
                {
                    SingleCosts.Add(CostFactory.Create(g, s));
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
            else if (ActivatableType.StartsWith("Suspend"))
            {
            	NewIsManaAbility = false;
            	
            	NewCondition = delegate(object[] param)
                {
                    return TargetCard.CurrentCharacteristics.Location == CardLocation.Hand;
                };
            	
            	//Construct the cost
                string FullCost = ActivatableType.Split(':')[1];
                string[] Costs = FullCost.Split('+');

                List<Cost> SingleCosts = new List<Cost>();
                foreach (string s in Costs)
                {
                    SingleCosts.Add(CostFactory.Create(g, s));
                }
                NewCost = new CompoundCost(SingleCosts.ToArray());
                
                NewEffect = new CompoundEffect(new Effect(delegate(object[] param) {
                                                          	TargetCard.Move(CardLocation.Exile);
                                                          	for(int i=0;i<int.Parse(ActivatableType.Split(':')[2]);i++)
                                                          	{
                                                          		TargetCard.CurrentCharacteristics.Counters.Add("Time");
                                                          	}
                                                          }));
                 
            }
            else if (ActivatableType.StartsWith("Manatap"))
            {
                //It's a mana ability
                NewIsManaAbility = true;

                //Can only be activated on the battlefield
                NewCondition = delegate(object[] param)
                {
                    return (TargetCard.CurrentCharacteristics.Location == CardLocation.Battlefield) && !TargetCard.CurrentCharacteristics.IsTapped;
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

                NewEffect = new CompoundEffect(delegate(object[] param)
                {
                    //Tap the card
                    TargetCard.CurrentCharacteristics.IsTapped = true;

                    //Add the mana

                });

                NewDescription += "tap: Add " + ManaMade + " to your manapool.";
            }

            return new Activatable(TargetCard, NewIsManaAbility, NewIsCastingAbility, NewCost, NewCondition, NewEffect, NewDescription);
        }
    }
}
