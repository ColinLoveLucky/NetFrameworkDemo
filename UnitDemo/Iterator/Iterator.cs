using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Iterator
{
    public interface IIterator<T> where T : class
    {
        
        T Next();

        T Current();

        int? Count
        {
            get;
        }
    }

    public class NumList<T> : IIterator<T> where T : class
    {
        private T[] list = null;

        private int initLength = 0;

        private int currentIndex = 0;

        public NumList(int length)
        {
            initLength = length;

            list = new T[length];
        }

        [System.Runtime.CompilerServices.IndexerName("testIndex")]
        public T this[int index]
        {
            get
            {
                T result = default(T);
                if (list.Length < Count)
                    result = list[index];
                return list[index];
            }
            set
            {
                if (index >= initLength)
                    throw new Exception("The Length is Bigger Than InitLenght,Please Check Again");
                else
                    list[index] = value;
            }
        }

        public T Next()
        {
            if (currentIndex < Count)
                return list[currentIndex++];
            else
                throw new Exception("Over Length");
        }

 
        public T Current()
        {
            if (currentIndex < Count)
                return list[currentIndex];
            else
                throw new Exception("Not Data");
        }


        public int? Count
        {
            get
            {
                return initLength;
            }
        }
    }
}
