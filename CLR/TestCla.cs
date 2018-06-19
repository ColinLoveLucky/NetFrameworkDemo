using Frame4._0Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLR
{
    public class TestCla
    {
        public void Show()
        {
            TestHelloAssembly test = new TestHelloAssembly();

            test.Show();

            Console.WriteLine("Hello World");
        }

        public void ShowHi()
        {
            Console.WriteLine("Show Hi");
        }
    }
}
