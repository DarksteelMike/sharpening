using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal enum Phase { Untap, Upkeep, Draw, Main1, CombatBeginning, DeclareAttackers, DeclareBlockers, AssignFirstStrikeDamage, AssignDamage, CombatEnd, Main2, EndOfTurn, Cleanup };
    internal class EffectScheduler
    {
        private List<EffectSchedulerEntry> Entries;

        internal void EnteredPhase(Phase ThePhase, Player CurPlayer)
        {
            foreach (EffectSchedulerEntry Entry in Entries)
            {
                if (Entry.WaitingFor_Phase == ThePhase && (Entry.WaitingFor_Player == CurPlayer || Entry.WaitingFor_Player == null))
                {
                    Entry.Run();
                    if (Entry.RemoveAfterRun)
                    {
                        Entries.Remove(Entry);
                    }
                }
            }
        }

        internal void AddEntry(EffectSchedulerEntry Entry)
        {
            Entries.Add(Entry);
            SortEntries();
        }

        internal void RemoveEntriesFromCard(CardBase Card)
        {
            for(int i=0;i<Entries.Count;i++)
            {
                if (Entries[i].CardSrc.CardID == Card.CardID)
                {
                    Entries.RemoveAt(i);
                }
            }
        }

        private void SortEntries()
        {
            bool HasChanged = false;

            do
            {
                HasChanged = false;
                for (int i = 0; i < Entries.Count - 1; i++)
                {
                    if (Entries[i].CardSrc.Timestamp > Entries[i + 1].CardSrc.Timestamp)
                    {
                        HasChanged = true;
                        EffectSchedulerEntry tmp = Entries[i];
                        Entries[i] = Entries[i + 1];
                        Entries[i + 1] = tmp;
                    }
                }
            } while (HasChanged);
        }

        internal EffectScheduler()
        {
            Entries = new List<EffectSchedulerEntry>();
        }
    }
}
