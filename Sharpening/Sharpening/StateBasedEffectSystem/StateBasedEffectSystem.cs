using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal class StateBasedEffectSystem
    {
        private Game InvolvedGame;
        private ReplacableEvent SBE01_ZeroLifeCheck;
        private ReplacableEvent SBE02_ZeroToughnessCheck;
        private ReplacableEvent SBE03_LethalDamageCheck;
        private ReplacableEvent SBE04_AuraLegalAttachmentCheck;
        private ReplacableEvent SBE05_LegendRuleCheck;
        private ReplacableEvent SBE06_MisplacedTokenCheck;
        private ReplacableEvent SBE07_MillingLossCheck;
        private ReplacableEvent SBE08_PoisonCounterCheck;
        private ReplacableEvent SBE09_WorldRuleCheck;
        private ReplacableEvent SBE10_MisplacedCopyCheck;
        private ReplacableEvent SBE11_IllegalEquipFortifCheck;
        private ReplacableEvent SBE12_ShouldntAttachCheck;
        private ReplacableEvent SBE13_CounterBalanceCheck;
        private ReplacableEvent SBE14_PlaneswalkerLoyaltyCheck;
        private ReplacableEvent SBE15_PlaneswalkerUniquenessCheck;

        private int EventsFired;

        internal StateBasedEffectSystem(Game g)
        {
            InvolvedGame = g;
            EventsFired = 0;
            SBE01_ZeroLifeCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE02_ZeroToughnessCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    foreach (Player p in InvolvedGame.Players)
                    {
                        foreach (CardBase Card in p.BattlefieldCards)
                        {
                            if (Card.HasType("Creature"))
                            {
                                if (Card.CurrentCharacteristics.Toughness == 0)
                                {
                                    Card.Move(CardLocation.Graveyard);
                                    EventsFired++;
                                }
                            }                            
                        }
                    }
                }));

            SBE03_LethalDamageCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    foreach(Player p in InvolvedGame.Players)
                    {
                    	foreach(CardBase Card in p.BattlefieldCards)
                        {
                        	if(Card.CurrentCharacteristics.AssignedDamage >= Card.CurrentCharacteristics.Toughness)
                            {
                            	Card.Move(CardLocation.Graveyard);
                            	EventsFired++;
                            }
                        }
                    }                    
                }));

            SBE04_AuraLegalAttachmentCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE05_LegendRuleCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    List<CardBase> Legendaries = new List<CardBase>();
                    foreach (Player p in InvolvedGame.Players)
                    {
                        foreach (CardBase Card in p.BattlefieldCards)
                        {
                            if (Card.HasSupertype("Legendary"))
                            {
                                Legendaries.Add(Card);
                            }
                        }
                    }

                    for (int i = 0; i < Legendaries.Count;i++ )
                    {
                        for (int j = 0; j < Legendaries.Count; j++)
                        {
                            if (Legendaries[i].Name == Legendaries[j].Name && i != j)
                            {
                                Legendaries[i].Move(CardLocation.Graveyard);
                                Legendaries[j].Move(CardLocation.Graveyard);
                                Legendaries.RemoveAt(i);
                                Legendaries.RemoveAt(j);
                                EventsFired++;
                            }
                        }
                    }
                }));

            SBE06_MisplacedTokenCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE07_MillingLossCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE08_PoisonCounterCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE09_WorldRuleCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                   List<CardBase> Worlds = new List<CardBase>();
                   foreach(Player p in InvolvedGame.Players)
                   {
                    	foreach(CardBase Card in p.BattlefieldCards)
                    	{
                         	if(Card.CurrentCharacteristics.Supertypes.Contains("World"))
                        	{
                             	Worlds.Add(Card);
                        	}
                    	}
                    }
                                                                  	
                    int max = int.MinValue;
                    int maxat = -1;
                    for(int i=0;i<Worlds.Count;i++)
                    {
                    	if(Worlds[i].Timestamp > max)
                    	{
                        	max = Worlds[i].Timestamp;
                        	maxat = i;
                    	}
                    }
                                                                  	
                    Worlds.RemoveAt(maxat);
                                                                  	
                    foreach(CardBase Card in Worlds)
                    {
                    	Card.Move(CardLocation.Graveyard);
                    	EventsFired++;
                	}
 				}));
            
            SBE10_MisplacedCopyCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE11_IllegalEquipFortifCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE12_ShouldntAttachCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE13_CounterBalanceCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                   	foreach(Player p in InvolvedGame.Players)
                    {
                    	foreach(CardBase Card in p.BattlefieldCards)
                    	{
                    		while(Utility.CountInList<string>(Card.CurrentCharacteristics.Counters,"+1/+1") > 0 && Utility.CountInList<string>(Card.CurrentCharacteristics.Counters,"-1/-1") > 0)
                    		{
                    			Card.CurrentCharacteristics.Counters.Remove("+1/+1");
                    			Card.CurrentCharacteristics.Counters.Remove("-1/-1");
                    			EventsFired++;
                    		}
                    	}
                    }
                }));

            SBE14_PlaneswalkerLoyaltyCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                	foreach(Player p in InvolvedGame.Players)
                    {
                    	foreach(CardBase Card in p.BattlefieldCards)
                        {
                        	if(Card.CurrentCharacteristics.Types.Contains("Planeswalker"))
                            {
                            	if(Utility.CountInList<string>(Card.CurrentCharacteristics.Counters,"Loyalty") == 0)
                                {
                                	Card.Move(CardLocation.Graveyard);
                                	EventsFired++;
                                }
                            }
                       	}
                 	}
                }));

            SBE15_PlaneswalkerUniquenessCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                	List<CardBase> Planeswalkers = new List<CardBase>();
                    foreach(Player p in InvolvedGame.Players)
                    {
                    	foreach(CardBase Card in p.BattlefieldCards)
                        {
                        	if(Card.CurrentCharacteristics.Types.Contains("Planeswalker"))
                            {
                            	Planeswalkers.Add(Card);
                            }
                        }
                    }
                    
                    List<CardBase> HasDupes = new List<CardBase>();
                    for(int i = 0;i<Planeswalkers.Count;i++)
                    {
                    	for(int j=0;j<Planeswalkers.Count;j++)
                        {
                        	if(Planeswalkers[i].CurrentCharacteristics.Subtypes[0] == Planeswalkers[j].CurrentCharacteristics.Subtypes[0] && i != j)
                            {
                            	HasDupes.Add(Planeswalkers[i]);
                            	HasDupes.Add(Planeswalkers[j]);
                            }
                        }
                    }
                                                                               	
                    foreach(CardBase Card in HasDupes)
                    {
                    	Card.Move(CardLocation.Graveyard);
                    	EventsFired++;
                    }
                }));
        }

        internal void Run()
        {
            EventsFired = 0;

            do
            {
                EventsFired = 0;

                SBE01_ZeroLifeCheck.Run();
                SBE02_ZeroToughnessCheck.Run();
                SBE03_LethalDamageCheck.Run();
                SBE04_AuraLegalAttachmentCheck.Run();
                SBE05_LegendRuleCheck.Run();
                SBE06_MisplacedTokenCheck.Run();
                SBE07_MillingLossCheck.Run();
                SBE08_PoisonCounterCheck.Run();
                SBE09_WorldRuleCheck.Run();
                SBE10_MisplacedCopyCheck.Run();
                SBE11_IllegalEquipFortifCheck.Run();
                SBE12_ShouldntAttachCheck.Run();
                SBE13_CounterBalanceCheck.Run();
                SBE14_PlaneswalkerLoyaltyCheck.Run();
                SBE15_PlaneswalkerUniquenessCheck.Run();
            } while (EventsFired > 0);
        }
    }
}
