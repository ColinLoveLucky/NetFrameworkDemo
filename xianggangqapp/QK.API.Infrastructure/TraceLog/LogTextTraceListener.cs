using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.API.Infrastructure.TraceLog
{
    public class LogTextTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            WriteMsg(message, "");
        }

        public override void WriteLine(string message)
        {
            WriteMsg(message, "");
        }

        public override void Write(object o, string category)
        {
            WriteMsg(o, category);
        }

        private void WriteMsg(object o, string category)
        {
            //仅仅记录请求和响应报文的信息
            //if (!o.ToString().Contains("CFCA:")) { return; }
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(category) == false)
            {
                msg = category + " : " + o.ToString();
            }
            else
            {
                msg = o.ToString();
            }
            Logger.Info(msg);

        }
    }
}
