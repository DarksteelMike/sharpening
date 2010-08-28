using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Sharpening
{
    internal class CardCompiler
    {
        internal List<CardBase> CompileCards(string[] CardNames)
        {
            CSharpCodeProvider Compiler = new CSharpCodeProvider();

            //Setup the compiler
            CompilerParameters Params = new CompilerParameters();
            Params.GenerateExecutable = false;
            Params.GenerateInMemory = true;
            //Params.ReferencedAssemblies.Add("..\\Sharpening.dll");
            Params.ReferencedAssemblies.Add(System.Reflection.Assembly.GetExecutingAssembly().FullName); //?
            Params.ReferencedAssemblies.Add("System.dll");
            

            //Remove multiples and add the file extensions
            List<string> Filenames = new List<string>();
            Filenames.AddRange(CardNames);
            for (int i = 0; i < Filenames.Count; i++)
            {
                for (int j = 0; j < Filenames.Count; j++)
                {
                    if (Filenames[i] == Filenames[j] && i != j)
                    {
                        Filenames.RemoveAt(j);
                    }
                }
            }
            for (int i = 0; i < Filenames.Count; i++)
            {
                Filenames[i] = "Cards\\" + Filenames[i] + ".cs";
            }

            //Actually compile the cards
            CompilerResults Result = Compiler.CompileAssemblyFromFile(Params, Filenames.ToArray());

            //Handle compiler errors!
            if (Result.Errors.Count != 0)
            {
                string totalmessage = "Card Compilation Error(s)!\n";
                for (int i = 0; i < Result.Errors.Count; i++)
                {
                    if (!Result.Errors[i].IsWarning)
                    {
                        totalmessage += Result.Errors[i].ErrorNumber + " " + Result.Errors[i].FileName + ":" + Result.Errors[i].Line + " - " + Result.Errors[i].ErrorText + "\n";
                    }
                }
                if (totalmessage != "Card Compilation Error(s)!\n")
                {
                    throw new Exception(totalmessage);
                }
            }

            //Return a list of cards from the compiled ones.
            List<CardBase> CompiledCards = new List<CardBase>();
            foreach (string s in CardNames)
            {
                string CleanName = Utility.CreateValidClassName(s);
                CompiledCards.Add((CardBase)Result.CompiledAssembly.CreateInstance("Cards." + CleanName, false));
            }

            return CompiledCards;
        }

        internal List<CardBase> CompileCards(List<string> CardNames)
        {
            return CompileCards(CardNames.ToArray());
        }
    }
}
