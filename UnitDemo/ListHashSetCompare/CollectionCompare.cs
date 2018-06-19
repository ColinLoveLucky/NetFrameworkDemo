using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ListHashSetCompare
{
    public class CollectionCompare
    {
        public void TestArray(int dataCount)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var dataArray = new int[dataCount];

            for (int i = 0; i < dataCount; i++)
            {
                dataArray[i] = i;
            }

            stopwatch.Stop();

            Console.WriteLine("Array Insert {0} 条 数据 共花费 {1}", dataCount, stopwatch.Elapsed);

            var sum = 0;

            stopwatch.Reset();

            stopwatch.Start();

            for (int i = 0; i < dataCount; i++)
            {
                sum += dataArray[i];
            }

            stopwatch.Stop();
            Console.WriteLine("Array Read {0} 条数据，共耗时{1}", dataCount, stopwatch.Elapsed);

            stopwatch.Reset();

            stopwatch.Start();

            var findResult = 0;

            findResult = dataArray[1000];

            stopwatch.Stop();
            Console.WriteLine("Array Find {0} 条 数据 结果为 {2},共耗时{1}", 1000, stopwatch.Elapsed, findResult);

            stopwatch.Reset();

            stopwatch.Start();

           var s= dataArray.Select(x => x == 1000).First();

           stopwatch.Stop();

           Console.WriteLine("Array Find Value is {0} 耗时 {1}", 1000, stopwatch.Elapsed);
        }

        public void TestArrayList(int dataCount)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var dataArray = new ArrayList();

            for (int i = 0; i < dataCount; i++)
            {
                dataArray.Add(i);
            }

            stopwatch.Stop();

            Console.WriteLine("ArrayList Insert {0} 条 数据 共花费 {1}", dataCount, stopwatch.Elapsed);

            var sum = 0;

            stopwatch.Reset();

            stopwatch.Start();

            for (int i = 0; i < dataCount; i++)
            {
                sum += Convert.ToInt32(dataArray[i]);
            }

            stopwatch.Stop();
            Console.WriteLine("ArrayList Read {0} 条数据，共耗时{1}", dataCount, stopwatch.Elapsed);

            stopwatch.Reset();

            stopwatch.Start();

            var findResult = 0;

            findResult = Convert.ToInt32(dataArray[1000]);
            stopwatch.Stop();
            Console.WriteLine("ArrayList Find {0} 条 数据 结果为 {2},共耗时{1}", 1000, stopwatch.Elapsed, findResult);

        }

        public void TestList(int dataCount)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var dataArray = new List<string>();

            for (int i = 0; i < dataCount; i++)
            {
                dataArray.Add(i.ToString());
            }

            stopwatch.Stop();

            Console.WriteLine("List Insert {0} 条 数据 共花费 {1}", dataCount, stopwatch.Elapsed);

            var sum = 0;

            stopwatch.Reset();

            stopwatch.Start();

            for (int i = 0; i < dataCount; i++)
            {
                sum +=Convert.ToInt32(dataArray[i]);
            }

            stopwatch.Stop();
            Console.WriteLine("List Read {0} 条数据，共耗时{1}", dataCount, stopwatch.Elapsed);

            stopwatch.Reset();

            stopwatch.Start();

            var findResult = 0;

            findResult =Convert.ToInt32(dataArray[1000]);
            stopwatch.Stop();
            Console.WriteLine("List Find {0} 条 数据 结果为 {2},共耗时{1}", 1000, stopwatch.Elapsed, findResult);

            dataArray.Add("1");

            stopwatch.Reset();

            stopwatch.Start();

            var s = dataArray.Select(x => x == "1000").First();

           bool istur= dataArray.Contains("1000");

            stopwatch.Stop();

           // dataArray.Contains(1000);

            Console.WriteLine("List Find Value is {0} 耗时 {1}", istur, stopwatch.Elapsed);

            stopwatch.Reset();

            stopwatch.Start();

            dataArray.Reverse();

            dataArray.Sort((x, y) => x.CompareTo(y));

            Console.WriteLine("List Sort 耗时{0}", stopwatch.Elapsed);

            //dataArray.Sort(new ListSort());

            stopwatch.Reset();

            stopwatch.Start();

            ListSort sort=new ListSort();

            dataArray.Sort(sort);

            var value = dataArray.Take(100).ToList()[99];

            stopwatch.Stop();

            Console.WriteLine("List Sort Value {0} 耗时 {1}", value, stopwatch.Elapsed);

        }

        public void TestDictionary(int dataCount)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var dataArray = new Dictionary<string, int>();

            for (int i = 0; i < dataCount; i++)
            {
                dataArray.Add(i.ToString(),i);
            }

            stopwatch.Stop();

            Console.WriteLine("Dictionary Insert {0} 条 数据 共耗时 {1}", dataCount, stopwatch.Elapsed);

            var sum = 0;

            stopwatch.Reset();

            stopwatch.Start();

            for (int i = 0; i < dataCount; i++)
            {
                sum += dataArray[i.ToString()];
            }

            stopwatch.Stop();
            Console.WriteLine("Dictionary Read {0} 条数据，共耗时{1}", dataCount, stopwatch.Elapsed);

            stopwatch.Reset();

            stopwatch.Start();

            var findResult = 0;

            findResult = dataArray["1000"];
            stopwatch.Stop();
            Console.WriteLine("Dictionary Find {0} 条 数据 结果为 {2},共耗时{1}", 1000, stopwatch.Elapsed, findResult);

            stopwatch.Reset();

            stopwatch.Start();

            var s = dataArray.ContainsKey("10000");

            //bool istur = dataArray.Contains(1000);

            stopwatch.Stop();

            // dataArray.Contains(1000);

            Console.WriteLine("Dictionary Find Value is {0} 耗时 {1}", s, stopwatch.Elapsed);

           
        }

        public void TestSortDictionary(int dataCount)
        {
        //    SortedDictioanry
           // SortedList
            //SortList
        }
    }

    public class ListSort : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (Convert.ToInt32(x)>Convert.ToInt32(y))
                return -1;
            else if (Convert.ToInt32(x) == Convert.ToInt32(y))
                return 0;
            else
                return 1;
        }
    }

}
