using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal abstract class Utility
    {
        internal static string CreateValidClassName(string OriginalName)
        {
            string Ret = OriginalName;
            List<char> InvalidChars = new List<char>();
            InvalidChars.AddRange(System.IO.Path.GetInvalidFileNameChars());
            InvalidChars.Add(' ');
            InvalidChars.Add('*');
            InvalidChars.Add('!');
            InvalidChars.Add('+');
            InvalidChars.Add('-');
            InvalidChars.Add('\'');
            InvalidChars.Add('\"');
            InvalidChars.Add('û');
            InvalidChars.Add('ö');
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                Ret = Ret.Replace(c.ToString(), "");
            }

            return Ret;
        }

        internal static ManaColor StringToManaColor(string str)
        {
            ManaColor res = ManaColor.Colorless;
            switch (str)
            {
                case ("W"): res = ManaColor.White; break;
                case ("U"): res = ManaColor.Blue; break;
                case ("B"): res = ManaColor.Black; break;
                case ("R"): res = ManaColor.Red; break;
                case ("G"): res = ManaColor.Green; break;
            }

            return res;
        }

        internal static bool StringContainsAny(string Haystack, IEnumerable<string> Needles)
        {
            foreach (string n in Needles)
            {
                if (Haystack.Contains(n))
                {
                    return true;
                }
            }

            return false;
        }
        internal static bool StringContainsAny(string Haystack, IEnumerable<char> Needles)
        {
            List<string> NewNeedles = new List<string>();
            foreach (char c in Needles)
            {
                NewNeedles.Add(c.ToString());
            }

            return StringContainsAny(Haystack, NewNeedles);
        }
        
        internal static int CountInList<T>(List<T> Haystack,T Needle)
        {
        	int Result = 0;
        	foreach(T Straw in Haystack)
        	{
        		if(Straw.Equals(Needle))
        		{
        			Result++;
        		}
        	}
        	
        	return Result;
        }

        internal static readonly string Numbers = "0123456789";
    }
}
