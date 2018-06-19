using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo
{
    public class DoubleQuesionUnit
    {
        public void DoubleQ()
        {
            string d = "I Love The World!";

            var result = d ?? "Hello World";

            Console.WriteLine("The Final Result is {0}", result);
        }
    }
}
