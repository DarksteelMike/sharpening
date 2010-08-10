using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpening
{
    class CompoundEffect
    {
        private List<Effect> myEffects;
        internal List<Effect> MyEffects
        {
            get { return myEffects; }
        }

        internal CompoundEffect(params Effect[] effects)
        {
            myEffects = new List<Effect>();
            myEffects.AddRange(effects);
        }

        internal void Run(params object[] param)
        {
            foreach (Effect e in myEffects)
            {
                e(param);
            }
        }

        internal void Append(CompoundEffect Effects)
        {
            Append(Effects.myEffects.ToArray());
        }

        internal void Append(params Effect[] Effects)
        {
            myEffects.AddRange(Effects);
        }
    }
}
