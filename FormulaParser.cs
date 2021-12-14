using System;
using System.Collections.Generic;
using System.Text;
using Lab1_AaDS;
using System.Diagnostics;

namespace Coursework_AaDS
{
    public static class FormulaParser
    {
        public static IValueProvider GetRootValueProvider(string formula)
        {
            if (formula.IsEmptyOrSpacesOnly())
                throw new FormatException("Awaited formula, but empty string received");

            NotAList<Operation> formulaOperations = new NotAList<Operation>();
            NotAList<IValueProvider> formulaInitedValues = new NotAList<IValueProvider>();

            int index1 = 0;
            int valuesCalculated = 0;                  //used to check if operations and args placed correctly
            string word="";
            Pair<int, int> wordSubstr;

            while (index1!=-1 && index1<formula.Length)
            {
                wordSubstr = formula.GetFirstWordSubstr(index1);
                if (wordSubstr.first == -1)
                    break;

                word = formula.Substring(wordSubstr.first, wordSubstr.second);

                if (word == "(")
                {
                    int bracketIndex = wordSubstr.first;
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
                    IValueProvider value = word.AsConstValue();         

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

                    index1 = wordSubstr.first+wordSubstr.second;
                }
            }

            if (valuesCalculated > 1)
                throw new FormatException("too many operands after the end of the formula. Was <" + word + "> supposed to be an operation?");

            else if (valuesCalculated < 1)
                throw new FormatException("expected a value after <" + word + ">");

            

            while (!formulaOperations.Empty)
            {
                var initedValsEnumerator = formulaInitedValues.GetEnumerator() as NotAListEnumerator<IValueProvider>;           //used to avoid too much indexation

                var highestPriorityOp = formulaOperations[0];
                bool highestPriorityOpIsUnary = highestPriorityOp.OperationType != OperationType.Binary;
                int highestPriorityOpIndex = 0;
                int highestPriorityOpFirstArgIndex = 0;

                int firstArgIndex = 0;
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

                for (int i = 0; i <= highestPriorityOpFirstArgIndex; i++)
                    initedValsEnumerator.MoveNext();

                highestPriorityOp.Arg1 = (IValueProvider)initedValsEnumerator.Current;

                initedValsEnumerator.Current = highestPriorityOp;

                if (highestPriorityOp.OperationType == OperationType.Binary)
                {
                    initedValsEnumerator.MoveNext();
                    highestPriorityOp.Arg2 = (IValueProvider)initedValsEnumerator.Current;
                    formulaInitedValues.RemoveAt(initedValsEnumerator);
                }

                formulaOperations.RemoveAt(highestPriorityOpIndex);

            }

            if (formulaInitedValues.Count != 1)
                throw new Exception("RUNTIME more or less then 1 value left after calculations");

            return formulaInitedValues[0];
        }

        public static IEnumerable<string> ToPrefixFormat(Operation rootOperation)
        {
            yield return rootOperation.Syntax + " ";
            foreach (var val in rootOperation.GetSubValueProviders())
            {
                if (val is Operation operation)
                {
                    foreach (var val2 in ToPrefixFormat(operation))
                    {
                        yield return val2;
                    }
                }

                else
                    yield return val.ToString() + " ";
            }

        }
    }
}
