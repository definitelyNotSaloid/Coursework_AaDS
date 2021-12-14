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
                try
                {
                    IValueProvider res=null;
                    res = FormulaParser.GetRootValueProvider(formula);


                    Console.WriteLine(res.GetValue());

                    if (res is Operation operation)
                        foreach (var op in FormulaParser.ToPrefixFormat(operation))
                            Console.Write(op);

                    else
                        Console.Write(res.GetValue());



                    Console.WriteLine();
                    /*Console.WriteLine("Parse time = " + timeParse.ToString());
                    Console.WriteLine("Calculation time = " + timeVal.ToString());
                    Console.WriteLine("translating to prefix form time = " + timePrefix.ToString());*/
                }
                catch(FormatException ex)
                {
                    Console.WriteLine("Formula format error: " + ex.Message);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("runtime error: " + ex.Message);
                }
            }
            
        }
    }
}

