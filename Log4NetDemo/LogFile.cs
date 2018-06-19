using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetDemo
{
    public class LogFile
    {
        public void AppendFile()
        {
           // var ddd = AppDomain.CurrentDomain.FriendlyName;
            ILog log = LogManager.GetLogger("testApp.Logging");
            log.Info(DateTime.Now.ToString() + ": login success");
            Console.Read();
        }
    }
}
