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
                                if (Card.Characteristics.ActualToughness == 0)
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
                    EventsFired++;
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
                        foreach (CardBase c in p.BattlefieldCards)
                        {
                            if (c.HasSupertype("Legendary"))
                            {
                                Legendaries.Add(c);
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
                    EventsFired++;
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
                    EventsFired++;
                }));

            SBE14_PlaneswalkerLoyaltyCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
                }));

            SBE15_PlaneswalkerUniquenessCheck = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    EventsFired++;
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
