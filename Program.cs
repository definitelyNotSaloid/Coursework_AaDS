using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;

namespace Coursework_AaDS
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalData.Init();
            
            while (true)
            { 
                var formula = Console.ReadLine();
                if (formula == "esc") 
                    break;
                var res = FormulaParsingUtility.GetRootValueProvider(formula);
                Console.WriteLine(res.GetValue());
            }
            
        }
    }
}
