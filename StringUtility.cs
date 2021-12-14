using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_AaDS
{
    public static class StringUtility
    {
        public static Pair<int,int> GetFirstWordSubstr(this string str, int startingIndex=0)            //returns first pack of chars separated with spaces or just one bracket (ie string "do a (barrel roll)" contains words do,a,(,barrel,roll,) )
        {
            Pair<int, int> pair;

            int firstNonSeparatorIndex = -1;
            if (startingIndex < 0 || startingIndex >= str.Length)
                throw new ArgumentOutOfRangeException("statring index is out of range");

            
            for (int i=startingIndex;i<str.Length;i++)
            {
                if (str[i] != ' ')
                {
                    if (firstNonSeparatorIndex == -1)
                    {
                        if (str[i]=='(')        // brackets are separate words. noone is willing to write 5 + ( 5 + 5 ) instead of 5 + (5 + 5)
                        {
                            pair.first = i;
                            pair.second = 1;
                            return pair;    
                        }

                        firstNonSeparatorIndex = i;

                    }
                }
                else
                {

                    if (firstNonSeparatorIndex != -1)
                    {
                        pair.first = firstNonSeparatorIndex;
                        pair.second = i - firstNonSeparatorIndex;
                        return pair;
                    }

                }
            }

            if (firstNonSeparatorIndex != -1)
            {
                pair.first = firstNonSeparatorIndex;
                pair.second = str.Length - firstNonSeparatorIndex;
                return pair;
            }

            pair.first = -1;
            pair.second = 0;

            return pair;
        }

        public static int FindEnclosingBracketIndex(this string str, int openingBracketIndex=0)
        {
            if (str[openingBracketIndex] != '(')
                throw new ArgumentException("there is no bracket at index = " + openingBracketIndex);

            int nBracketsOpened = 1;
            for (int i=openingBracketIndex+1;i<str.Length;i++)
            {
                if (str[i] == '(')
                {
                    nBracketsOpened++;
                }

                else if (str[i] == ')')
                {
                    nBracketsOpened--;
                    if (nBracketsOpened == 0)
                        return i;
                }
            }

            return -1;
        }

        public static OperationWorker AsOperationWorker(this string str)
        {
            foreach (var worker in GlobalData.operationWorkers)
            {
                if (worker.Syntax == str)
                    return worker;
            }

            return null;
        }

        public static ConstValue AsConstValue(this string str)
        {
            try
            {
                return new ConstValue(Convert.ToDouble(str));
            }
            catch (FormatException)
            {
                foreach (var cnst in GlobalData.constValues)
                {
                    if (cnst.Name == str)
                        return cnst;
                }
                return null;
            }
        }

        public static bool IsEmptyOrSpacesOnly(this string str)
        {
            for (int i = 0; i < str.Length; i++)
                if (str[i] != ' ')
                    return false;

            return true;
        }
    }

    
}
