using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetDemo
{
    public class LogDb
    {
        public void InsertDbLog()
        {
            ILog log = LogManager.GetLogger("testApp.Logging");
            try
            {
                log.Info(new LogContent("127.0.0.1", "111111", "登陆系统", "登陆成功"));
              //  var ss = 1 - int.Parse("sss");
            }
            catch (Exception ex)
            {
                log.Error(new LogContent("127.0.0.1", "111111", "登陆系统", ex.Message + ":" + ex.StackTrace));
            }
        }
    }
}
