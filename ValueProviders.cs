using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;


namespace Coursework_AaDS
{
    public enum OperationType
    {
        None,
        UnaryPrefix,
        UnaryPostfix,
        Binary,
        Other
    }

    public interface IValueProvider
    {
        public IValueProvider Parent { get; set; }
        
        public double GetValue();
        public bool IsInited();           // does NOT guarantees value can be received. Only ensures args are inited
    }

    public class Operation : IValueProvider
    {
        protected OperationWorker worker;

       

       // public int ArgsBefore => worker.ArgsBefore;
       // public int ArgsAfter => worker.ArgsAfter;

        private IValueProvider[] _arguments = new IValueProvider[2];

        public IValueProvider Arg1
        {
            get => _arguments[0];
            set => _arguments[0] = value;
        }

        public IValueProvider Arg2
        {
            get => _arguments[1];
            set => _arguments[1] = value;
        }

        public OperationType OperationType => worker.Type;

        public string Syntax => worker.Syntax;
        public int Priority => worker.Priority;

        public IValueProvider parent;

        public IValueProvider Parent
        {
            get => parent;
            set => parent = value;
        }

        public double GetValue()
        {
            switch (worker.Type)
            {
                case OperationType.None:
                    throw new Exception("tried to get result of none-type operation");
                case OperationType.UnaryPrefix:
                case OperationType.UnaryPostfix:
                    return worker.Calculate(new double[] { Arg1.GetValue() });
                case OperationType.Binary:
                    return worker.Calculate(new double[] { Arg1.GetValue(), Arg2.GetValue() });
                default:
                    throw new Exception("no case for " + worker.Type.ToString() + "-type operation");
            }
        }

        public IEnumerable<IValueProvider> GetSubValueProviders()
        {
            yield return Arg1;
            if (worker.Type == OperationType.Binary)
                yield return Arg2;
        }

        public bool IsInited()
        {
            switch (worker.Type)
            {
                case OperationType.UnaryPostfix:
                case OperationType.UnaryPrefix:
                    return Arg1 != null;

                case OperationType.Binary:
                    return Arg1 != null && Arg2 != null;

                default:
                    return false;
            }
        }

        public Operation(OperationWorker worker)
        {
            parent = null;
            this.worker = worker;
        }
    }

    public class ConstValue : IValueProvider
    {
        private double _value;
        private string _name;
        public string Name => _name;
        public IValueProvider parent;
        public IValueProvider Parent
        {
            get => parent;
            set => parent = value;
        }

        public override string ToString()
        {
            if (_name != null && _name != "")
                return _name;

            return _value.ToString();
        }

        public ConstValue(double value, string name = "")
        {
            parent = null;   
            _value = value;
            _name = name;
        }

        public double GetValue() => _value;

        public bool IsInited() => true;
    }


}
