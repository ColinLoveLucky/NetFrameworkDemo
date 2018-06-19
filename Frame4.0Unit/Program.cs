using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Frame4._0Unit
{
    class Program
    {
        static void Main(string[] args)
        {

            DelegateTest test = new DelegateTest();

          //  test._calcuDelegate = test.AddA;

          //  test._calcuDelegate += test.AddB;

            // test._calcuEvent += test.AddA;

          //  test._calcuDelegate();


            int i = 0; int j = 0;

            //var m = i++;
            //var n = ++j;

            //var k = (i++);
           // var p = (++j);

            int q = 0;

            int t = (q = i++);

            Console.WriteLine("q:{0}", q);
            Console.WriteLine("t:{0}", t);
            Console.WriteLine("i:{0}", i);

         //   int t = (q=i++)++;

          //  int d = ++(++j);
        }
    }
}
