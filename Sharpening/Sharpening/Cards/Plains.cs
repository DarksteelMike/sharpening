using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening.Cards
{
    class Plains : CardBase
    {
        public Plains(Sharpening.Game g)
            : base(g)
        {
            activatables.Add(ActivatableFactory.Create(g, this, "PlayLand"));

            activatables.Add(ActivatableFactory.Create(g, this, "Manatap:-:W"));
        }
    }
}
