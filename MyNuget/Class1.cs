using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNuget
{
    public interface ILog
    {
        string Write();
    }

    public class DbLog : ILog
    {
        public string Write()
        {
            return "Hello World";
        }
    }

}
