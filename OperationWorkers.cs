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


        public OperationType Type { get => type; }
        public string Syntax { get => syntax; }
        public int Priority { get => priority; }

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

            if (arguments[1] == 0)
                throw new DivideByZeroException();

            return arguments[0] / arguments[1];             
        }

        public OperationWorker_Division(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_NatLogarithm : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 1)
                throw new ArgumentException("Can't find logarithm of more or less than 1 value");

            if (arguments[0] <= 0)
                throw new ArgumentException("logarithm cant receive non-positive number as arg");

            return Math.Log(arguments[0]);
        }

        public OperationWorker_NatLogarithm(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_DecimalLogarithm : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 1)
                throw new ArgumentException("Can't find logarithm of more or less than 1 value");

            if (arguments[0] <= 0)
                throw new ArgumentException("logarithm cant receive non-positive number as arg");

            return Math.Log10(arguments[0]);
        }

        public OperationWorker_DecimalLogarithm(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_Power: OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 2)
                throw new ArgumentException("Power operation cant receive more or less than 2 args");

            double res = Math.Pow(arguments[0], arguments[1]);
            if (double.IsNaN(res))
                throw new ArgumentException("pow operation is not valid. Perhaps you entered negative number and non-int power");

            if (double.IsInfinity(res))
                throw new ArgumentException("pow operation returned inf");

            return res;
        }

        public OperationWorker_Power(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_Sin : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 1)
                throw new ArgumentException("sin operation cant receive more or less than 1 arg");

            return Math.Sin(arguments[0]);
        }

        public OperationWorker_Sin(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_Cos : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 1)
                throw new ArgumentException("cos operation cant receive more or less than 1 arg");

            return Math.Cos(arguments[0]);
        }

        public OperationWorker_Cos(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_Tangent : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 1)
                throw new ArgumentException("tangent operation cant receive more or less than 1 arg");

            double tan = Math.Tan(arguments[0]);
            if (double.IsInfinity(tan))
                throw new ArgumentException("tangent operation returned inf");

            return tan;
        }

        public OperationWorker_Tangent(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_Cotangent : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 1)
                throw new ArgumentException("cotangent operation cant receive more or less than 1 arg");

            double tan = Math.Tan(arguments[0]);
            if (tan == 0)
                throw new ArgumentException("cotangent operation returned inf");

            return 1/tan;
        }

        public OperationWorker_Cotangent(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_SquareRoot : OperationWorker
    {
        public override double Calculate(double[] arguments)
        {
            if (arguments.Length != 1)
                throw new ArgumentException("square root operation cant receive more or less than 1 arg");

            if (arguments[0] < 0)
                throw new ArgumentException("cant take square root of negative value");

            return Math.Sqrt(arguments[0]);
        }

        public OperationWorker_SquareRoot(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }

    public class OperationWorker_Factorial : OperationWorker            
    {
        public override double Calculate(double[] arguments)                        //for non-positive args[0] 1 is returned; args[0] is rounded through Convert.ToInt32(double); 
        {
            if (arguments.Length != 1)
                throw new ArgumentException("factorial operation cant receive more or less than 1 arg");

            double res = 1;
            for (int i = 1; i <= Convert.ToInt32(arguments[0]); i++)
                res *= i;

            return res;
        }

        public OperationWorker_Factorial(string syntax, int priority, OperationType type) : base(syntax, priority, type)
        { }
    }
}
