using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal static class InputStateFactory
    {
        internal static InputState Create(Game g,string StateType,CardBase cardSrc)
        {
            CardActivationDelegate EmptyCard = new CardActivationDelegate(delegate(CardActivationEventArgs e)
                { });
            VoidNoParams EmptyVNP = new VoidNoParams(delegate() { });

            InputState ret = null;
            Condition NewCondition = null;
            string[] SplitType = StateType.Split(':');
            if (SplitType[0] == "Target")
            {
                if (SplitType[1] == "Card")
                {
                    if (SplitType.Length == 2)
                    {
                        //Any Card
                        NewCondition = new Condition(delegate(object[] param)
                            {
                                if (param.Length == 0)
                                {
                                	string err = "Condition Arguments empty when creating Input State \"" + StateType + "\"";
                                	
                                	if(cardSrc != null)
                                	{
                                		err += " for card " + cardSrc.Name + " (" + cardSrc.CardID + ")";
                                	}
                                	
                                	throw new ArgumentException(err);
                                }

                                if (param[0] != null)
                                {
                                    return true;
                                }

                                return false;
                            });
                    }
                    else
                    {
                        NewCondition = new Condition(delegate(object[] param)
                            {
                                if (param.Length == 0)
                                {
                                    throw new ArgumentException("Condition Arguments empty when creating Input State \"" + StateType + "\" for card " + cardSrc.Name + " (" + cardSrc.CardID + ")");
                                }
                                for (int i = 3; i < SplitType.Length; i++)
                                {

                                }
                                return false;
                            });
                    }
                }
                else if (SplitType[1] == "Player")
                {
                    if (SplitType[2] == "Opponent")
                    {
                        NewCondition = new Condition(delegate(object[] param)
                            {
                                if (param.Length == 0)
                                {
                                    throw new ArgumentException("Condition Arguments empty when creating Input State \"" + StateType + "\" for card " + cardSrc.Name + " (" + cardSrc.CardID + ")");
                                }
                                if ((int)param[0] != g.WhoseTurn)
                                {
                                    return true;
                                }
                                return false;
                            });
                    }
                    else if (SplitType[2] == "Self")
                    {
                        NewCondition = new Condition(delegate(object[] param)
                            {
                                if (param.Length == 0)
                                {
                                    throw new ArgumentException("Condition Arguments empty when creating Input State \"" + StateType + "\" for card " + cardSrc.Name + " (" + cardSrc.CardID + ")");
                                }
                                if ((int)param[1] == g.WhoseTurn)
                                {
                                    return true;
                                }
                                return false;
                            });
                    }
                    else if (SplitType[2] == "Any")
                    {
                        NewCondition = new Condition(delegate(object[] param)
                            {
                                if (param.Length == 0)
                                {
                                    throw new ArgumentException("Condition Arguments empty when creating Input State \"" + StateType + "\" for card " + cardSrc.Name + " (" + cardSrc.CardID + ")");
                                }
                                if (param[0] == null)
                                {
                                    return true;
                                }

                                return false;
                            });
                    }
                    ret = new InputState(g, NewCondition, "", EmptyCard, null, null, null, null);
                }
                else if (SplitType[1] == "CardOrPlayer")
                {
                    //Not sure how to handle arguments
                }
            }
            else if(SplitType[0] == "MainPhase")
            {
            	NewCondition = new Condition(delegate(object[] param) 
            	                             {
            	                             	return param[0] != null;
            	                             });
            	
            }
            return ret;
        }
    }
}
