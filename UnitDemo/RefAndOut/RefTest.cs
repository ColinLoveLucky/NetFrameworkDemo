using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.RefAndOut
{
    public class RefTest
    {
        private int y;

        public void Add(out int x)
        {
            x = 10;
        }

        public void Sub(ref int x)
        {
            x = x + 10;
        }
    }
}
