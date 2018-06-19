using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ImplictAndExplictApp
{
    public class ImplictAndExplict
    {
        public ImplictAndExplict(double x)
        {
            this.X = x;
        }

        public double X { get; set; }

        public static implicit operator ImplictAndExplict(double x)
        {
            return new ImplictAndExplict(x);
        }

        static public explicit operator double(ImplictAndExplict x)
        {
            return x.X + 10;
        }

    }
}
