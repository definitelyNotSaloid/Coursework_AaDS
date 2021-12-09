using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_AaDS
{
    public static class GlobalData
    {
        public static List<OperationWorker> operationWorkers;
        public static List<ConstValue> constValues;

        public static void Init()
        {
            operationWorkers = new List<OperationWorker>();
            constValues = new List<ConstValue>();
            foreach (var op in XmlParsingUtility.GetOperations())
                operationWorkers.Add(op);
        }
    }
}
