using Clutch.Diagnostics.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    /// <summary>
    ///EF性能相关
    /// </summary>
    public class TraceComponent
    {
        /// <summary>
        /// 初始化EF Query日志
        /// </summary>
        public static void Init()
        {
            if (QK.QAPP.Global.GlobalSetting.SqlTraceing == "OPEN")
            {
                //注册EntityFramwork日志监听实例
                DbTracing.Enable();
                DbTracing.AddListener(new EFTracingListener());

                //注册日志监听实例
                //Trace.Listeners.Clear();
                //Trace.Listeners.Add(new LogTextTraceListener());
            }
        }
    }
}
