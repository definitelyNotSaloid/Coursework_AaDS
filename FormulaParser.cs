using System;
using System.Collections.Generic;
using System.Text;
using Lab1_AaDS;

namespace Coursework_AaDS
{
    public static class FormulaParser
    {
        public static IValueProvider GetRootValueProvider(string formula)
        {
            NotAList<Operation> formulaOperations = new NotAList<Operation>();
            NotAList<IValueProvider> formulaInitedValues = new NotAList<IValueProvider>();

            int index1 = 0;
            int valuesCalculated = 0;                  //used to check if operations and args placed correctly
            string word="";

            while (index1!=-1 && index1<formula.Length)
            {
                word = formula.GetFirstWord(startingIndex: index1);

                if (word == "")
                    break;

                if (word == "(")
                {
                    int bracketIndex = formula.GetFirstWordIndex(startingIndex: index1);
                    int enclosingBracketIndex = formula.FindEnclosingBracketIndex(bracketIndex);
                    if (enclosingBracketIndex == -1)
                        throw new FormatException("Some brackets aren't closed");

                    IValueProvider underbracketValue = GetRootValueProvider(
                        formula.Substring(bracketIndex + 1, enclosingBracketIndex - (bracketIndex + 1)));

                    formulaInitedValues.PushBack(underbracketValue);
                    index1 = enclosingBracketIndex + 1;

                    valuesCalculated++;
                }
                else if (word == ")")                   //if an opening bracket met, indexator jumps past enclosing one. no enclosing brackets should be met while parsing
                    throw new FormatException("Found enclosing bracket without opening one");

                else
                {
                    IValueProvider value = word.AsConstValue();         //can throw exeptions

                    if (value == null)
                    {
                        var worker = word.AsOperationWorker();
                        if (worker == null)
                            throw new FormatException("<" + word + "> is not a const value, named const value or an operation");

                        formulaOperations.PushBack(new Operation(worker));
                        switch (worker.Type)
                        {
                            case OperationType.Binary:
                                if (valuesCalculated != 1)
                                    throw new FormatException("operator <" + word + "> must have =1 argument at the left");
                                valuesCalculated = 0;
                                break;
                            case OperationType.UnaryPostfix:
                                if (valuesCalculated != 1)
                                    throw new FormatException("operator <" + word + "> must have =1 argument at the left");
                                //valuesCalculated still =1
                                break;
                            case OperationType.UnaryPrefix:
                                if (valuesCalculated != 0)
                                    throw new FormatException("operator <" + word + "> can't receive any arguments at the left");
                                break;
                            default:
                                break;
                        }
                    }

                    else
                    {
                        valuesCalculated++;
                        formulaInitedValues.PushBack(value);
                    }

                    index1 = formula.FirstCharIndexAfterWord(index1);
                }
            }

            if (valuesCalculated > 1)
                throw new FormatException("too many operands after the end of the formula. Was <" + word + "> supposed to be an operation?");

            else if (valuesCalculated < 1)
                throw new FormatException("expected a value after <" + word + ">");



            // Following piece isnt optimized, TODO

            while (!formulaOperations.Empty)
            {
                var highestPriorityOp = formulaOperations[0];
                bool highestPriorityOpIsUnary = highestPriorityOp.OperationType != OperationType.Binary;
                int highestPriorityOpIndex = 0;
                int firstArgIndex = 0;
                int highestPriorityOpFirstArgIndex = 0;
                int index2 = 0;

                foreach(var op in formulaOperations)
                {
                    if (op.OperationType == OperationType.Binary)
                    {
                        if ((op.Priority > highestPriorityOp.Priority) && !highestPriorityOpIsUnary)
                        {
                            highestPriorityOp = op;
                            highestPriorityOpIndex = index2;
                            highestPriorityOpFirstArgIndex = firstArgIndex;
                        }
                    }
                    else
                    {
                        if (!highestPriorityOpIsUnary || (op.Priority>=highestPriorityOp.Priority))
                        {
                            highestPriorityOp = op;
                            highestPriorityOpIndex = index2;
                            highestPriorityOpFirstArgIndex = firstArgIndex;

                            highestPriorityOpIsUnary = true;
                        }
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
                throw new Exception("argument expected");

            return formulaInitedValues[0];
        }

        public static string ToPrefixFormat(Operation rootOperation)
        {
            string res = rootOperation.Syntax + " ";
            foreach (var val in rootOperation.GetSubValueProviders())
            {
                if (val is Operation operation)
                {
                    res += ToPrefixFormat(operation) ;
                }

                else
                    res += val.GetValue().ToString() + " ";
            }

            return res;
        }
    }
}
