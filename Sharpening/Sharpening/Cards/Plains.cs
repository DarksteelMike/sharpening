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
            //Create a casting Activatable
            Condition NewCondition = new Condition(delegate(object[] param)
            {
                bool Result;

                //Is it our controllers turn?
                Result = InvolvedGame.Players[InvolvedGame.WhoseTurn] == this.characteristics.Controller;

                //Are we in the hand?
                Result = Result && (this.characteristics.Location == CardLocation.Hand);

                //Has our controller played less than the maximum amount of lands this turn?
                Result = Result && (this.characteristics.Controller.LandsPlayedThisTurn < this.characteristics.Controller.MaxLandPerTurn);

                return true;

            });
            CompoundEffect NewEffect = new CompoundEffect(new Effect(delegate(object[] target) {
                this.Move(CardLocation.Battlefield);
            }));
            Activatable CastingActivatable = new Activatable(this, false, new CompoundCost(CostFactory.Create(g, "0")), NewCondition, NewEffect, "Play this land.");
            activatables.Add(CastingActivatable);

            activatables.Add(ActivatableFactory.Create(g,this,"Manatap:-:W"));
        }
    }
}
