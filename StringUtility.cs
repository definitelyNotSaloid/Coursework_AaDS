using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_AaDS
{
    public static class StringUtility
    {
        public static string GetFirstWord(this string str, int startingIndex=0, char[] letters = null, bool excludingLettersMode = true)
        {
            if (excludingLettersMode && letters == null)
                letters = new char[] { ' ' };

            int firstNonSeparatorIndex = -1;
            for (int i=startingIndex;i<str.Length;i++)
            {
                bool isSeparator = false;
                foreach(var letter in letters)
                {
                    if (str[i]==letter)
                    {
                        isSeparator = excludingLettersMode;
                        break;
                    }    
                }

                if (!isSeparator)
                {
                    if (firstNonSeparatorIndex == -1)
                        firstNonSeparatorIndex = i;
                }
                else
                {
                    if (firstNonSeparatorIndex != -1)
                        return str.Substring(firstNonSeparatorIndex, i - firstNonSeparatorIndex);

                }
            }

            if (firstNonSeparatorIndex != -1)
                return str.Substring(firstNonSeparatorIndex, str.Length - firstNonSeparatorIndex);

            return "";
        }


        public static int GetFirstWordIndex(this string str, int startingIndex = 0, char[] letters = null, bool excludingLettersMode = true)
        {
            if (excludingLettersMode && letters == null)
                letters = new char[] { ' ' };

            for (int i=startingIndex;i<str.Length;i++)
            {
                foreach (var letter in letters)
                {
                    if (str[i] == letter ^ excludingLettersMode)            //return if excludingMode-and-thisIsNotASeparator or if includingMode-and-thisIsALetter
                        return i;
                }    
            }

            return -1;
        }

        public static int FirstCharIndexAfterWord(this string str, int startinIndex=0, char[] letters=null, bool excludingLettersMode = true)
        {
            if (excludingLettersMode && letters == null)
                letters = new char[] { ' ' };

            bool nonSeparatorMet = false;
            for (int i = startinIndex; i < str.Length; i++)
            {
                foreach (var letter in letters)
                {
                    if (str[i] != letter ^ excludingLettersMode)            //return excludingMode-and-thisIsNotASeparator or if includingMode-and-thisIsALetter
                    {
                        if (nonSeparatorMet)
                            return i;

                        break;
                    }
                    else
                        nonSeparatorMet = true;
                }
            }

            return -1;

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

    }

    
}
