using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCDemo.Models
{
    public class ErrorInfo
    {
        public  string p1;
        public string p2;
        public object instance;

        public ErrorInfo(string p1, string p2, object instance)
        {
            // TODO: Complete member initialization
            this.p1 = p1;
            this.p2 = p2;
            this.instance = instance;
        }
    }
}
