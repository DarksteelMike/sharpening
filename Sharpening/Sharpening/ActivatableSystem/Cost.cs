using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    class Cost
    {
        private string description;
        internal string Description
        {
            get { return description; }
        }

        private Effect doPayment;
        internal Effect DoPayment
        {
            get { return doPayment; }
        }

        private bool isPaid;
        internal bool IsPaid
        {
            get { return isPaid; }
        }

        private bool isManaCost;
        internal bool IsManaCost
        {
            get { return isManaCost; }
        }

        internal Cost(string d, Effect p, bool i)
        {
            description = d;
            doPayment = p;
            isManaCost = i;
        }
        
        internal int Converted()
        {
        	if(!isManaCost)
        	{
        		return 0;
        	}
        	int Result = 0;
        	foreach(string part in description.Split(' '))
        	{
        		if(part.Length > 1)
        		{
        			if(part[0].ToString() == "2")
        			{
        				Result += 2;
        			}
        			else
        			{
        				Result += int.Parse(part);
        			}
        		}
        		else
        		{
        			if(Utility.Numbers.Contains(part))
        			{
        				Result += int.Parse(part);
        			}
        			else
        			{
        				Result += 1;
        			}
        		}
        	}
        	
        	return Result;
        }
    }
}
