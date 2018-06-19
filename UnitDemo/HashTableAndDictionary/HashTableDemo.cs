using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.HashTableAndDictionary
{
    public class HashTableDemo
    {
        private static Hashtable _hashTable;
        private static Dictionary<string, object> _dictionary;

        public Hashtable hash = Hashtable.Synchronized(new Hashtable());

        public static void Compare(int dataCount)
        {
            Console.WriteLine("----------------------\n");

            _hashTable = new Hashtable();

            _dictionary = new Dictionary<string, object>();

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            for (int i = 0; i < dataCount; i++)
            {
                _hashTable.Add("Str" + i, i.ToString());
            }

            stopWatch.Stop();

            Console.WriteLine("Hastable Insert {0} Data Elsapsed Time {1}", dataCount, stopWatch.Elapsed);

            stopWatch.Reset();

            stopWatch.Start();

            for (int i = 0; i < dataCount; i++)
            {
                _dictionary.Add("Str" + i, i.ToString());


            }
            stopWatch.Stop();

            Console.WriteLine("Dictionary Insert {0} Data Elaspsed Time {1}", dataCount, stopWatch.Elapsed);
            stopWatch.Reset();

            int si = 0;

            stopWatch.Start();

            for (int i = 0; i < _hashTable.Count; i++)
            {
                si++;
            }

            stopWatch.Stop();

            Console.WriteLine("Hastable For Time {0}", stopWatch.Elapsed);

            stopWatch.Reset();

            si = 0;

            stopWatch.Start();

            foreach (var s in _hashTable)
            {
                si++;
            }

            stopWatch.Stop();

            Console.WriteLine("Hashtable Foreach Time {0}", stopWatch.Elapsed);

            stopWatch.Reset();

            si = 0;

            stopWatch.Start();

            var _hashEnum = _hashTable.GetEnumerator();

            while (_hashEnum.MoveNext())
            {
                si++;
            }

            stopWatch.Stop();
            Console.WriteLine("Hashtable Enumerator Time {0}", stopWatch.Elapsed);

            stopWatch.Reset();

            stopWatch.Start();

            si = 0;

            for (int i = 0; i < _dictionary.Count; i++)
            {
                si++;
            }

            stopWatch.Stop();

            Console.WriteLine("Dictionary For Time {0}", stopWatch.Elapsed);

            stopWatch.Reset();

            si = 0;

            stopWatch.Start();

            foreach (var s in _dictionary)
            {
                si++;
            }

            Console.WriteLine("Dictionary Foreach Time {0}", stopWatch.Elapsed);

            stopWatch.Reset();

            si = 0;

            stopWatch.Start();

            _hashEnum = _dictionary.GetEnumerator();

            while (_hashEnum.MoveNext())
            {
                si++;
            }

            stopWatch.Stop();

            Console.WriteLine("Dictionary Enumerator Time {0}", stopWatch.Elapsed);

            Console.WriteLine("---------------------------\n");
        }

        public void InsertSynchornized(string key, string value)
        {

            hash.Add(key, value);

        }
    }
}
