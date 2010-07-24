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
    }
}
