using System;
using System.Collections.Generic;
using System.Text;
using Lab1_AaDS;

namespace Coursework_AaDS
{
    public static class GlobalData
    {
        public static NotAList<OperationWorker> operationWorkers;
        public static NotAList<ConstValue> constValues;

        public static void Init()
        {
            operationWorkers = new NotAList<OperationWorker>();
            constValues = new NotAList<ConstValue>();
            foreach (var op in XmlParsingUtility.GetOperations())
                operationWorkers.Add(op);

            foreach (var c in XmlParsingUtility.GetConstValues())
                constValues.Add(c);
        }
    }
}
