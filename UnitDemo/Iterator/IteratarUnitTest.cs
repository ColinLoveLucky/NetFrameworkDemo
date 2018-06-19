using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Iterator
{
    public class IteratarUnitTest
    {
        public void IteratorUnitDemo()
        {
            int initLenght = 10;

            var iterator = new NumList<string>(initLenght);

            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                iterator[i] = random.Next(0, 10).ToString();
            }

            for (int i = 0; i < initLenght; i++)
            {
                Console.WriteLine("The NumList index is {0} ,The data is {1}", i, iterator[i]);
            }

            for (int i = 0; i < initLenght; i++)
            {
                Console.WriteLine("The NumList Data is:{0}", iterator.Next());
            }


        }
    }
}
