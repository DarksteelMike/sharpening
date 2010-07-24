using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal delegate void Effect(params object[] param);
    internal delegate bool Condition(params object[] param);

    internal class ReplacableEvent
    {
        private Effect StandardEffect;
        private List<ReplacingEvent> ReplacingEvents;

        internal void Run(params object[] param)
        {
            if (ReplacingEvents.Count == 0)
            {
                StandardEffect(param);
            }
            else
            {
                int MostRecentTimestamp = -1;
                int MRTAt = -1;

                for (int i = 0; i < ReplacingEvents.Count; i++)
                {
                    if (ReplacingEvents[i].CardSrc.Timestamp > MostRecentTimestamp)
                    {
                        MostRecentTimestamp = ReplacingEvents[i].CardSrc.Timestamp;
                        MRTAt = i;
                    }
                }

                ReplacingEvents[MRTAt].Run(param);
            }
        }

        internal void RunStandard(params object[] param)
        {
            StandardEffect(param);
        }

        internal ReplacableEvent(Effect MyEffect)
        {
            StandardEffect = MyEffect;
            ReplacingEvents = new List<ReplacingEvent>();
        }
    }
}
