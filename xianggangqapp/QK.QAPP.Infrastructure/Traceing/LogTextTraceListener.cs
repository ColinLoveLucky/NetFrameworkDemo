using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    public class LogTextTraceListener : TraceListener
    {
        public override void Fail(string message)
        {
            WriteMsg(message, "");
        }

        public override void Fail(string message, string detailMessage)
        {
            WriteMsg(message, "");
        }

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

        private void WriteMsg(object o,string category)
        {
            ////////////////// 刘云松 2015-11-04
            return;

            bool IsException = false;
            string msg = "";
            string GID = Guid.NewGuid().ToString();
            msg += "<<<<<<<< 互联ID:" + GID +":::"
                    + DateTime.Now.ToString()+Environment.NewLine;
            if (string.IsNullOrWhiteSpace(category) == false)
            {
                msg += category + " : ";
            }
            if (o is Exception || category=="error")
            {
                var ex = (Exception)o;
                msg += ex.Message + Environment.NewLine;
                msg += ex.StackTrace;
                if (ex.InnerException!=null)
                {
                    msg += Environment.NewLine + "InnerException:"+ex.InnerException.Message+Environment.NewLine;
                    msg += ex.InnerException.StackTrace;
                }
                IsException = true;
            }
            else if (o != null)
            {
                msg += o.ToString();
            }
            msg += Environment.NewLine + "end>>>>>>> " + GID + ":::" 
                    + DateTime.Now.ToString() + Environment.NewLine;

            if (IsException)
            {
                Logger.Error(msg);
            }
            else
            {
                Logger.Info(msg);
            }            
        }
    }
}
