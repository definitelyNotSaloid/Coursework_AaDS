using NUnit.Framework;
using System.Collections.Generic;
using Coursework_AaDS;
using System;

namespace TestProject
{

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            GlobalData.Init();
        }

        [Test]
        public void Test_Basics()
        {
            List<string> formulas = new List<string>()
            {
                "2 + 2",
                "1 + 2 + 3 + 4 + 5 + 6",
                "5 - 3",
                "2 * 2",
                "6 / 3",
                "2 * 2 * 2 * 2"
            };
            List<double> answers = new List<double>()
            { 
                4,
                21,
                2,
                4,
                2,
                16
            };
            
            for (int i=0;i<formulas.Count;i++)
            {
                Assert.AreEqual(answers[i], FormulaParser.GetRootValueProvider(formulas[i]).GetValue());
            }
        }

        [Test]
        public void Test_Priority()
        {
            List<string> formulas = new List<string>()
            {
                "2 + 2 * 2",
                "1 + 2 * 2 / 4 + 5 * 6",
                "2 * 4 / 2 + 20 / 10 * 2",
                "333 / 333 - 0 * 10000000 + 1"
            };
            List<double> answers = new List<double>()
            {
                6,
                32,
                8,
                2
            };

            for (int i = 0; i < formulas.Count; i++)
            {
                Assert.AreEqual(answers[i], FormulaParser.GetRootValueProvider(formulas[i]).GetValue());
            }
        }

        [Test]
        public void Test_Brackets()
        {
            List<string> formulas = new List<string>()
            {
                "( 2 + 2 ) * 2",
                "( 2 + 6 ) * 2 / 4 + 5 * 6",
                "2 * ( 4 / 4 ) + 20 / ( 10 * 2 )",
                "( 222 / ( 222 - 111 ) * 10000000 ) + 1",
                "1 - (2 - (3 - (4 - (5 - (6 + 7)))))"
            };
            List<double> answers = new List<double>()
            {
                8,
                34,
                3,
                20000001,
                -10
            };

            for (int i = 0; i < formulas.Count; i++)
            {
                Assert.AreEqual(answers[i], FormulaParser.GetRootValueProvider(formulas[i]).GetValue());
            }
        }

        [Test]
        public void Test_UnaryOperations()
        {
            List<string> formulas = new List<string>()
            {
                "cos 0",
                "tg 0",
                "5 !",
                "cos 0 + 5",
                "cos 0 * 5",
                "ln e ^ 3",                 //any unary op has higher priority then binary. ans is 1 ^ 3 = 1
                "sin 7 ^ 2 + cos 7 ^ 2",    //might cause troubles with result like 0.999999999999998. upd yep, it does cause em.
                "ln cos 0",
                "6 * sin 0 + 7 * cos 0"
            };
            List<double> answers = new List<double>()
            {
                1,
                0,
                120,
                6,
                5,
                1,
                1,
                0,
                7
            };

            for (int i = 0; i < formulas.Count; i++)
            {
                Assert.AreEqual(answers[i], Math.Round(FormulaParser.GetRootValueProvider(formulas[i]).GetValue(), 8));
            }
        }

        [Test]
        public void Test_Exceptions()
        {
            List<string> formulas = new List<string>()
            {
                "5",                    //nothing wrong

                // Format exceptions
                "5+5",                  //too lazy to upgrade parser. 
                "1 + (2 + 3",           //no enclosing bracket
                "1 + 2 + 3 )",          //no opening bracket
                "sin ^ 2 + cos ^ 2",    //no sin and cos args
                "5 5",                  //no operations
                "sin sin sin sin sin sin sin",  //no arg
                "- 5",                  //no unary minuses as ops!

                // Arg exceptions
                "ln -1",                
                "lg cos pi",


                //div by 0
                "1 / 0"                 //div by 0

            };

            Assert.DoesNotThrow(() => FormulaParser.GetRootValueProvider("5").GetValue());

            for (int i = 1; i < 8; i++)
                Assert.Throws<FormatException>(() => FormulaParser.GetRootValueProvider(formulas[i]).GetValue());

            Assert.Throws<ArgumentException>(() => FormulaParser.GetRootValueProvider(formulas[8]).GetValue());
            Assert.Throws<ArgumentException>(() => FormulaParser.GetRootValueProvider(formulas[9]).GetValue());

            Assert.Throws<DivideByZeroException>(() => FormulaParser.GetRootValueProvider(formulas[10]).GetValue());
        }
    }
}