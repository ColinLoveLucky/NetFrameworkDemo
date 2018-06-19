using Clutch.Diagnostics.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    public class EFTracingListener : IDbTracingListener
    {
        public void CommandExecuted(DbTracingContext context)
        {
            StringBuilder parameters = new StringBuilder();
            foreach (DbParameter item in context.Command.Parameters)
            {
                parameters.AppendLine("参数:"+item.ParameterName + ",值:" + item.Value);
            }
            StringBuilder strInfo = new StringBuilder();
            strInfo.AppendLine(string.Format("开始时间:{0}",context.StartTime));
            strInfo.AppendLine(string.Format("结束时间:{0}",context.FinishTime));
            strInfo.AppendLine(string.Format("执行时间(ms)：{0}",context.Duration));
            strInfo.AppendLine(string.Format("执行方式:{0}",context.Type.ToString()));
            strInfo.AppendLine(string.Format("CommonType:{0}",context.Command.CommandType));
            strInfo.AppendLine(string.Format("执行语句:{0}",context.Command.CommandText));
            strInfo.AppendLine(string.Format("参数:{0}", parameters.ToString()));
            QK.QAPP.Infrastructure.Log4Net.LogWriter.Info(strInfo.ToString());
        }

        public void CommandExecuting(DbTracingContext context)
        {
            //throw new NotImplementedException();
        }

        public void CommandFailed(DbTracingContext context)
        {
            //throw new NotImplementedException();
        }

        public void CommandFinished(DbTracingContext context)
        {
            //throw new NotImplementedException();
        }

        public void ReaderFinished(DbTracingContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
