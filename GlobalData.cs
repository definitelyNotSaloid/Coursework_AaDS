using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_AaDS
{
    public static class GlobalData
    {
        public static List<OperationWorker> operationWorkers;
        public static List<ConstValue> constValues;
        public static char[] NumberAlphabet = new char[] {'0','1','2','3','4','5','6','7','8','9','.',',' };

        public static void Init()
        {
            operationWorkers = new List<OperationWorker>();
            constValues = new List<ConstValue>();
            foreach (var op in XmlParsingUtility.GetOperations())
                operationWorkers.Add(op);

            foreach (var c in XmlParsingUtility.GetConstValues())
                constValues.Add(c);
        }
    }
}
