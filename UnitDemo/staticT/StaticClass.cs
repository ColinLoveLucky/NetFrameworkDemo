using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitDemo.Struct;

namespace UnitDemo.staticT
{
    public static class StaticClass
    {
     
    }

    public class TestStatic
    {
        private static int i;


        public TestStatic()
        {

        }
        static TestStatic()
        {

            Console.WriteLine("Hello World");
        }
        public static void TestB()
        {

        }

    }
}
