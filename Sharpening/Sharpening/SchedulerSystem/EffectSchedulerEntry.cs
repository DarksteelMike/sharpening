using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal class EffectSchedulerEntry
    {
        private Effect myEffect;
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }
        private Player waitingFor_Player;
        internal Player WaitingFor_Player
        {
            get { return waitingFor_Player; }
        }
        private Phase waitingFor_Phase;
        internal Phase WaitingFor_Phase
        {
            get { return waitingFor_Phase; }
        }
        private bool removeAfterRun;
        internal bool RemoveAfterRun
        {
            get { return removeAfterRun; }
        }

        internal void Run(params object[] param)
        {
            myEffect(param);
        }

        internal EffectSchedulerEntry(Effect e, CardBase c, Player p, Phase ph, bool r)
        {
            myEffect = e;
            cardSrc = c;
            waitingFor_Player = p;
            waitingFor_Phase = ph;
            removeAfterRun = r;
        }
    }
}
