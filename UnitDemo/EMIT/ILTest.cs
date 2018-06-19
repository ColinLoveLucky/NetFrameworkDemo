using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.EMIT
{
    public class ILTest
    {
       
        public void Calculate()
        {
            int i = 4;

            if(i<=6)
            {
                i = i + 9;
            }
        }


        public class A
        {
            public void Show()
            {

            }
        }

    }
}
