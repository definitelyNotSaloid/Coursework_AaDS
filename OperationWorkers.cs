using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_AaDS
{
    [Serializable]
    public abstract class OperationWorker
    {
        protected OperationType type;
        protected string syntax;
        protected int priority;
        //protected int argsBefore;
        //protected int argsAfter;


        public OperationType Type { get => type; }
        public string Syntax { get => syntax; }
        public int Priority { get => priority; }
        //public int ArgsBefore { get => argsBefore; }
       // public int ArgsAfter { get => argsAfter; }

        public abstract double Calculate(double[] args);
        public OperationWorker(string syntax, int priority, OperationType type)
        {
            this.syntax = syntax;
            this.priority = priority;
            this.type = type;
        }
    }

    public class OperationWorker_Addition : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            double res = 0.0;
            foreach (double arg in arguments)
            {
                res += arg;
            }
            return res;
        }

        public OperationWorker_Addition(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    
    }

    public class OperationWorker_Substraction: OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 2)
                throw new ArgumentException("Can't substract more or less then two values");
            
            return arguments[0]-arguments[1];
        }

        public OperationWorker_Substraction(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_Multiplication : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            double res = 1;
            foreach (var val in arguments)
                res *= val;

            return res;
        }

        public OperationWorker_Multiplication(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_Division : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 2)
                throw new ArgumentException("Can't divide more or less then two values");

            return arguments[0] / arguments[1];
        }

        public OperationWorker_Division(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }
}
