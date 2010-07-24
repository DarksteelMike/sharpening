using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    public class CardActivationEventArgs : EventArgs
    {
        private int cardID;
        public int CardID
        {
            get { return cardID; }
        }

        public CardActivationEventArgs(int cID)
        {
            cardID = cID;
        }
    }
}
