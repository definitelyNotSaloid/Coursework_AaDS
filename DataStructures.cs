using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lab1_AaDS;

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

    public class NotAVector<T> : IEnumerable<T>
    {
        private T[] array;
        private int size;
        private int reservationSize;

        public NotAVector()
        {
            array = new T[1];
            size = 1;
            reservationSize = 1;
        }

        public int Count => size;

        public void Add(T item)
        {
            if (size == reservationSize)
                Reserve(reservationSize * 2);

            array[size] = item;
            size++;
        }

        public void Clear()
        {
            size = 0;
        }

        public IEnumerator<T> GetEnumerator() => new NotAVectorEnumerator<T>(this);

        public void Reserve(int reservationSize)
        {
            if (reservationSize < this.size)
                throw new ArgumentException("cant reserve less memory than required to containd data");

            this.reservationSize = reservationSize;
            T[] newArr = new T[reservationSize];
            for (int i = 0; i < size; i++)
                newArr[i] = array[i];
            array = newArr;
        }

        IEnumerator IEnumerable.GetEnumerator() => new NotAVectorEnumerator<T>(this);

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException();

            for (int i = index; i < size - 1; i++)
            {
                array[i] = array[i + 1];
            }
            size--;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= size)
                    throw new ArgumentOutOfRangeException();

                return array[index];
            }
            set
            {
                if (index < 0 || index >= size)
                    throw new ArgumentOutOfRangeException();

                array[index] = value;
            }
        }
    }

    public class NotAVectorEnumerator<T> : IEnumerator<T>
    {
        private int lastIndex = -1;
        private NotAVector<T> vector;

        public T Current => vector[lastIndex];

        object IEnumerator.Current => vector[lastIndex];

        public NotAVectorEnumerator(NotAVector<T> vector)
        {
            this.vector = vector;
            lastIndex = -1;
        }

        public void Dispose()
        {
            vector = null;
        }

        public bool MoveNext()
        {
            lastIndex++;
            if (lastIndex < vector.Count)
                return true;

            return false;
        }

        public void Reset()
        {
            lastIndex = -1;
        }
    }


}
