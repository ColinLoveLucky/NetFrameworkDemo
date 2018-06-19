using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDDemo
{
    class Program
    {
        static void Main(string[] args)
        {

			double sqrt5 = Math.Sqrt(5);
			double phi = (1 + sqrt5) / 2.0;
			double fn = (Math.Pow(phi, 3) - Math.Pow(1 - phi, 3)) / sqrt5;
		}
    }
}
