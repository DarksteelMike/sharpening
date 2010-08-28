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
        
        private List<ReplacingEvent> replacingEvents;
        internal List<ReplacingEvent> ReplacingEvents
        {
        	get { return replacingEvents; }
        }

        internal void Run(params object[] param)
        {
            if (replacingEvents.Count == 0)
            {
                StandardEffect(param);
            }
            else
            {
                int MostRecentTimestamp = -1;
                int MRTAt = -1;

                for (int i = 0; i < replacingEvents.Count; i++)
                {
                    if (replacingEvents[i].CardSrc.Timestamp > MostRecentTimestamp)
                    {
                        MostRecentTimestamp = replacingEvents[i].CardSrc.Timestamp;
                        MRTAt = i;
                    }
                }

                replacingEvents[MRTAt].Run(param);
            }
        }

        internal void RunStandard(params object[] param)
        {
            StandardEffect(param);
        }

        internal ReplacableEvent(Effect MyEffect)
        {
            StandardEffect = MyEffect;
            replacingEvents = new List<ReplacingEvent>();
        }
    }
}
