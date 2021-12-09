using System;
using System.Collections.Generic;
using System.Text;
using Lab1_AaDS;

namespace Coursework_AaDS
{
    public static class FormulaParsingUtility
    {
        public static IValueProvider GetRootValueProvider(string formula)
        {
            NotAList<Operation> formulaOperations = new NotAList<Operation>();
            NotAList<IValueProvider> formulaInitedValues = new NotAList<IValueProvider>();
            IValueProvider[] formulaWordsArr=null;

            int index1 = 0;

            while (index1!=-1 && index1<formula.Length)
            {
                string word = formula.GetFirstWord(startingIndex: index1);

                if (word == "")
                    break;

                if (word=="(")
                {
                    int bracketIndex = formula.GetFirstWordIndex(startingIndex: index1);
                    int enclosingBracketIndex = formula.FindEnclosingBracketIndex(bracketIndex);
                    IValueProvider underbracketValue = GetRootValueProvider(
                        formula.Substring(bracketIndex + 1, enclosingBracketIndex- (bracketIndex +1)));

                    formulaInitedValues.PushBack(underbracketValue);
                    index1 = enclosingBracketIndex+1;
                }

                else
                {
                    IValueProvider value = word.AsConstValue();         //can throw exeptions

                    if (value == null)
                    {
                        var worker = word.AsOperationWorker();
                        if (worker == null)
                            throw new Exception(word + " is not a const value, named const value or operation");

                        formulaOperations.PushBack(new Operation(worker));
                    }

                    else
                        formulaInitedValues.PushBack(value);

                    index1 = formula.FirstCharIndexAfterWord(index1);
                }
            }

            while (!formulaOperations.Empty)
            {
                var highestPriorityOp = formulaOperations[0];
                int highestPriorityOpIndex = 0;
                int firstArgIndex = 0;
                int highestPriorityOpFirstArgIndex = 0;
                int index2 = 0;

                foreach(var op in formulaOperations)
                {
                    

                    if (op.Priority > highestPriorityOp.Priority)
                    {
                        highestPriorityOp = op;
                        highestPriorityOpIndex = index2;
                        highestPriorityOpFirstArgIndex = firstArgIndex;
                    }

                    if (op.OperationType == OperationType.Binary)
                        firstArgIndex++;

                    index2++;
                }
                //TODO avoid too much indexation calls
                highestPriorityOp.Arg1 = formulaInitedValues[highestPriorityOpFirstArgIndex];
                if (highestPriorityOp.OperationType == OperationType.Binary)
                {
                    highestPriorityOp.Arg2 = formulaInitedValues[highestPriorityOpFirstArgIndex + 1];
                    formulaInitedValues.RemoveAt(highestPriorityOpFirstArgIndex + 1);
                }
                formulaInitedValues[highestPriorityOpFirstArgIndex] = highestPriorityOp;
                formulaOperations.RemoveAt(highestPriorityOpIndex);

            }

            if (formulaInitedValues.Count != 1)
                throw new Exception("some args missed");

            return formulaInitedValues[0];
        }
    }
}
