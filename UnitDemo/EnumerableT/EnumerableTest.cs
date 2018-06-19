using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.EnumerableT
{
    public class EnumerableTest : IEnumerable
    {
        private int _capcity = 100;

        private int[] _array = null;

        public int[] ArrayItem
        {
            get
            {
                return _array;
            }
        }

        public int Capcity
        {
            get
            {
                return _capcity;
            }
            set
            {
                _capcity = value;
            }
        }

        public EnumerableTest()
        {
            _array = new int[_capcity];

            for (int i = 0; i < _capcity; i++)
            {
                _array[i] = i;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return new EmumeratorTest(this);
        }
    }

    public class EmumeratorTest : IEnumerator
    {
        private EnumerableTest _obj;

        private int _position = -1;

        public EmumeratorTest(EnumerableTest obj)
        {
            _obj = obj;
        }

        public object Current
        {
            get
            {
                return _obj.ArrayItem[_position];
            }
        }

        public bool MoveNext()
        {
            if (_position < _obj.ArrayItem.Length - 1)
            {
                _position++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _position = -1;
        }
    }

    public class EmT
    {
        private int[] _array;

        private int _position = 0;

        public int Count
        {
            get
            {
                return _array.Length;
            }
        }

        public int Current
        {
            get
            {
                return _array[_position];
            }
        }

        public bool MoveNext()
        {
            return true;
        }
    }

    public class EMT : IEnumerable
    {
        private int[] _array;

        private int _inialCaptical = 100;

        private int[,] _doubleArray = null;

        public int Capcity
        {
            get
            {
                return _array.Length;
            }

        }

        public EMT()
        {
            _array = new int[_inialCaptical];

            _doubleArray = new int[_inialCaptical, _inialCaptical];
        }

        public int this[int index]
        {
            get
            {
                if (index > Capcity)
                    return 0;
                else
                    return _array[index];
            }
            set
            {
                _array[index] = value;
            }
        }

        public int this[int index, int doubleIndex]
        {
            get
            {
                if (index > Capcity && doubleIndex > Capcity)
                    return 0;
                return _doubleArray[index, doubleIndex];
            }
            set
            {
                _doubleArray[index, doubleIndex] = value;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _doubleArray.GetEnumerator();

            // return _array.GetEnumerator();
        }
    }

    public class YieldTest : IEnumerable
    {
        private int[] _array = new int[10];

        public YieldTest()
        {
            for (int i = 0; i < 10; i++)
            {
                _array[i] = i;
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i < 8)
                    yield return _array[i];
                else
                    yield break;
            }
        }


    }

    public class YieldT
    {
        public static bool onOff = true;
        public static IEnumerable GetEnumerable()
        {
            yield return "1";

            yield return "2";

            if (onOff)
            {
                yield break;
            }

            yield return "3";

            yield return "4";

        }

        public IEnumerator<String> GetEnumerator()
        {

            yield return "Hello";

            yield return "World";

            yield break;
        }
    }



   
}
