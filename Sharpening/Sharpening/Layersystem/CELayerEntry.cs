using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace Sharpening
{
    internal class CELayerEntry
    {
        private Effect myEffect;
        internal Effect MyEffect
        {
            get { return myEffect; }
        }
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }
        private bool isCDA;
        internal bool IsCDA
        {
            get { return isCDA; }
        }

        //Dependency
        private List<FieldInfo> dependsOnFields;
        internal List<FieldInfo> DependsOnFields
        {
            get { return dependsOnFields; }
        }

        private List<FieldInfo> changingFields;
        internal List<FieldInfo> ChangingFields
        {
            get { return changingFields; }
        }

        internal CELayerEntry(Effect e, CardBase c, bool i)
        {
            myEffect = e;
            cardSrc = c;
            isCDA = i;
        }
    }
}
