using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_AaDS
{
    public class FixedSizeQueue<T>
    {
        public readonly int MaxSize;
        protected int size;
        protected int oldestValIndex;
        protected int newestValIndex;
        protected T[] innerArray;

        public int Count => size;

        public FixedSizeQueue(int maxSize)
        {
            MaxSize = maxSize;
            innerArray = new T[MaxSize];
            size = 0;
            oldestValIndex = 0;
            newestValIndex = 0;
        }

        public void Add(T item)
        {
            if (size >= MaxSize)
                throw new ArgumentOutOfRangeException("Queue overflow");

            newestValIndex++;
            if (newestValIndex == MaxSize - 1)
                newestValIndex = 0;                        // Looping index

            innerArray[newestValIndex] = item;
            size++;
            
        }

        public T Peek() => innerArray[oldestValIndex];

        public T Take()
        {
            if (size == 0)
                throw new Exception("Empty queue");

            T returnValue = innerArray[oldestValIndex];

            innerArray[oldestValIndex] = default;           // nullifying contained value to ensure GC will do its work properly

            oldestValIndex++;
            if (oldestValIndex == MaxSize - 1)
                oldestValIndex = 0;

            size--;
            return returnValue;
        }
    }

}
