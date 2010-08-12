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
        	this.baseCharacteristics.Supertypes.Add("Basic");
        	this.baseCharacteristics.Types.Add("Land");
        	this.baseCharacteristics.Subtypes.Add("Plains");
        	this.name = "Plains";
        	
        	//Play from hand
            activatables.Add(ActivatableFactory.Create(g, this, "PlayLand"));

            //Mana ability
            activatables.Add(ActivatableFactory.Create(g, this, "Manatap:-:W"));
        }
    }
}
