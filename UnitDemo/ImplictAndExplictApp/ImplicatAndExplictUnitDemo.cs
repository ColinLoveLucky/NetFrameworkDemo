using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ImplictAndExplictApp
{
    public class ImplicatAndExplictUnitDemo
    {
        public void ImplictUnitDemo()
        {
            ImplictAndExplict implictDemo = null;

            implictDemo = 20;

            Console.WriteLine("Implict The Value is {0}", implictDemo.X);

            Console.WriteLine("Explict The Value is {0}", (double)implictDemo);

            var list = new List<string>();

     

        }
    }
}
