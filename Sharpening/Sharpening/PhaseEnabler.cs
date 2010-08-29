using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpening
{
    class PhaseEnabler
    {
        private Dictionary<Phase, bool> Switchboard;

        internal PhaseEnabler()
        {
            Switchboard = new Dictionary<Phase, bool>();
            Switchboard.Add(Phase.AssignDamage, true);
            Switchboard.Add(Phase.AssignFirstStrikeDamage, true);
            Switchboard.Add(Phase.Cleanup, true);
            Switchboard.Add(Phase.CombatBeginning, true);
            Switchboard.Add(Phase.CombatEnd, true);
            Switchboard.Add(Phase.DeclareAttackers, true);
            Switchboard.Add(Phase.DeclareBlockers, true);
            Switchboard.Add(Phase.Draw, true);
            Switchboard.Add(Phase.EndOfTurn, true);
            Switchboard.Add(Phase.Main1, true);
            Switchboard.Add(Phase.Main2, true);
            Switchboard.Add(Phase.Untap, true);
            Switchboard.Add(Phase.Upkeep, true);
        }

        internal bool this[Phase p]
        {
            get { return Switchboard[p]; }
            set { Switchboard[p] = value; }
        }
    }
}
