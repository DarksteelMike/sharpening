using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    class TextChangeOperation
    {
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }

        private string replaceFrom;
        internal string ReplaceFrom
        {
            get { return replaceFrom; }
        }

        private string replaceTo;
        internal string ReplaceTo
        {
            get { return replaceTo; }
        }

        internal TextChangeOperation(CardBase src, string From, string To)
        {
            cardSrc = src;
            replaceFrom = From;
            replaceTo = To;
        }
    }
}
