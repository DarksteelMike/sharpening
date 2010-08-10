using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal static class CostFactory
    {
        internal static Cost Create(Game g, string CostType)
        {
            string NewDescription = "";
            Effect NewDoPayment = null;
            bool NewIsManaCost = false;

            if (CostType.StartsWith("Discard("))
            {
                string Param = CostType.Substring(CostType.IndexOf("("));
                string[] ParamArr = Param.Substring(0, Param.IndexOf(")")).Split(',');
                int AmountToDiscard = Int32.Parse(ParamArr[0]);
                string Type = ParamArr[1];
                string Color = ParamArr[2];

                //Construct description
                NewDescription = "Discard " + AmountToDiscard.ToString();
                if (Color != "Any")
                {
                    NewDescription += " " + Color;
                }
                if (Type != "Any")
                {
                    NewDescription += " " + Type;
                }
                NewDescription += " card";
                if (AmountToDiscard > 1)
                {
                    NewDescription += "s";
                }
                NewDescription += ".";

                NewDoPayment = delegate(object[] param)
                {

                };

                NewIsManaCost = false;
            }
            else if (CostType.StartsWith("Sacrifice("))
            {

                NewIsManaCost = false;
            }
            else if (CostType.StartsWith("Reveal("))
            {

                NewIsManaCost = false;
            }
            else if (CostType.StartsWith("Mana(")) //Mana cost
            {
                NewIsManaCost = true;
            }

            return new Cost(NewDescription, NewDoPayment, NewIsManaCost);
        }
    }
}
