using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal class ReplacingEvent
    {
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }

        private Effect NewEvent;

        internal void Run(params object[] param)
        {
            NewEvent(param);
        }

        internal ReplacingEvent(CardBase Src, Effect Event)
        {
            cardSrc = Src;
            NewEvent = Event;
        }
    }
}
