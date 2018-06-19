using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.EnumDemo
{
    public class EnumTestDemo
    {
        public void Test()
        {
            Console.WriteLine(string.Format("{0:G}", (int)Status.Failure));
            Console.WriteLine(Status.Success);
            Console.WriteLine(Status.Success.GetHashCode().ToString());
            Console.ReadKey();
        }
    }

    public enum Status:int
    {
        Success=200,
        Failure=500
    }
}
