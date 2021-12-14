using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Lab1_AaDS;

namespace Coursework_AaDS
{
    
    public struct Pair<T1,T2>
    {
        public T1 first;
        public T2 second;
    }

    //Hello from 2nd lab
    public static class TimeMeasurementUtility
    {
        public static TimeSpan MeasureExecutionTime(Action action, int nTests = 1, bool useApproxValue = false)
        {
            if (nTests < 1)
                throw new ArgumentException("Number of tests must be positive");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < nTests; i++)
            {
                action();
            }
            stopwatch.Stop();
            return useApproxValue ? stopwatch.Elapsed / nTests : stopwatch.Elapsed;
        }

    }


}


