using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ASISDemo
{
    public class IsUnit
    {
        public void TestIsUnit()
        {
            var baseA = new BaseA();

            var childB = new ChildB();

            //bool isBaseAToChildB = baseA is ChildB;

            //bool isChildBToBaseA = childB is BaseA;

            var testb = baseA as ChildB;

         //   var testa = childB as BaseA;

            //Console.WriteLine("BaseA Is ChildB  ?{0}", isBaseAToChildB);

            //Console.WriteLine("ChildB Is BaseA ?{0}", isChildBToBaseA);

            //Console.WriteLine("BaseA AS ChildB  {0}", testb);

            //Console.WriteLine("ChildB AS BaseA {0}", testa);

        }
    }

    public class BaseA
    {
        public string Name
        {
            get;
            set;
        }
    }

    public class ChildB : BaseA
    {
        public string Age
        {
            get;
            set;
        }
    }

}
