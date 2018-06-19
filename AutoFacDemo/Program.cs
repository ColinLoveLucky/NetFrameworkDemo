using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoFacOwin component = new AutoFacOwin();
            component.Test();
        }
    }
}
