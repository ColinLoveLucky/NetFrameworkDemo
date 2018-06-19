using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILog
{
    public interface ILog
    {
        string Write(string content);
    }

    public class DbLog : ILog
    {
        public string Write(string content)
        {
            throw new NotImplementedException();
        }
    }
}
