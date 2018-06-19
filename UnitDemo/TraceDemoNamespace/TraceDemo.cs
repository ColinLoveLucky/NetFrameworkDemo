using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.TraceDemoNamespace
{
    /// <summary>
    /////TextWriterTraceListener
    //ConsoleTraceListener
    //XmlWriterTraceListener
    //EventLogTraceListener 
    //BooleanSwitch 
    //TraceSwitch 
    //SourceSwitch 
    //Debug.Listeners
    //TraceSource
    //Trace
    /// </summary>
    public class TraceDemo
    {
        /// <summary>
        /// 既然是作为一个跟着的类库
        /// 跟踪输出的媒介不统一的
        /// 输出的日志类型是不统一
        /// 对于是否监听也是可以关闭的
        /// 对于监听的类型也是可以关闭的
        /// 对于输出的格式是自己可以定制的
        /// 必须是可以配置的
        /// 必须是可以向后扩展兼容的
        /// </summary>
        public void TraceConsole()
        {

            TextWriterTraceListener writer = new ConsoleTraceListener();

            writer.WriteLine("Hello");
        }
        /// <summary>
        /// 这个跟Debug环境变量一样，是区别开发生产环境，是可以在工程文件属性里面设置的
        /// </summary>
        public void TraceEviro()
        {
            TraceListener trace = new TextWriterTraceListener("D:\\TraceLog.txt");
            Trace.Listeners.Add(trace);
            Trace.WriteLine("Debug");
            Trace.WriteLine("TraceTextFile");
            Trace.AutoFlush = true;
            Trace.Close();
        }
        /// <summary>
        /// AutoFlush 不设置不会写入到本地文件
        /// </summary>
        public void TraceTextFile()
        {
            TraceListener trace = new TextWriterTraceListener("D:\\TraceLog.txt");
            trace.IndentLevel = 1;
            trace.IndentSize = 4;
            trace.TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime;
            trace.WriteLine("Debug");
            trace.Flush();
            trace.Close();
        }
        public void TraceXmlFile()
        {
            TraceListener trace = new XmlWriterTraceListener("D:\\TraceXmlLog.xml");

            trace.TraceOutputOptions = TraceOptions.DateTime;

            trace.IndentLevel = 2;

            trace.WriteLine("HelloXml");

            trace.WriteLine("HelloXmlSeconde");

            trace.Flush();

            trace.Close();
        }
        /// <summary>
        /// 注意点在使用的时候除了缩进是可以使用通过Write之外，
        /// 其它设置都是没有用，因为实现的的源码里面只是默认实现了缩进操作，其他的都没有调用
        /// 要想设置用TraceData
        /// 这个地方的设计就很好体现了，abstract的使用原理，virtual的使用原理，实体类实现的使用原理
        /// 继承类应该实现什么的原理
        /// </summary>
        public void TraceDemiline()
        {
            var trace = new DelimitedListTraceListener("D:\\DelimitTraceLog.txt");
            trace.TraceOutputOptions = TraceOptions.DateTime;
            trace.IndentLevel = 2;
            trace.Delimiter = ";";
            trace.Write("Trace", "T");
            trace.Write("HelloXmlSeconde");
            trace.Flush();
            trace.Close();
        }
        public void TraceDataByConfig()
        {
            //var traceSource = new TraceSource("source",SourceLevels.All);
            //TraceListener trace = new TextWriterTraceListener("D:\\TraceLog.txt");
            //traceSource.Listeners.Add(trace);
            //traceSource.TraceData(TraceEventType.Error, 1, "Hello");
            //traceSource.Flush();
            //traceSource.Close();
            //TraceSource trace = new TraceSource("tt");
            //trace.Switch.Level = SourceLevels.All;
            //trace.Listeners.Add(new TextWriterTraceListener("C:\\Text.txt"));
            //trace.TraceEvent(TraceEventType.Start, 0, "逻辑操作的开始");
            ////写入普通消息
            //trace.TraceInformation("写入消息");
            ////写入错误消息，根据TraceEventType枚举不同的消息级别
            //trace.TraceEvent(TraceEventType.Error, 0, "错误消息");
            ////逻辑操作的结束
            //trace.TraceEvent(TraceEventType.Stop, 0, "逻辑操作的结束");
            ////写入普通消息
            //trace.TraceInformation("写入消息");
            //trace.Flush();
            // var traceSource = new TraceSource("TraceSourceName");
            //  traceSource.TraceEvent(TraceEventType.Error, 2, "Hello World !");
            //  traceSource.Close();
            //  CLog.LogCritical("Video Play()");
            //  CLog.LogError("Audio Play()");
            // CLog.LogWarning("Picture Play()");
            //CLog.LogInformation("SubTitle Play()");
            // CLog.LogData("LogData Play()");
            CLog.LogTraceStart("Hello");
        }
        public void StaticTraceDemo()
        {
            TextWriterTraceListener writer = new ConsoleTraceListener();
            Trace.Listeners.Add(writer);
            writer.Filter = new EventTypeFilter(SourceLevels.All);
            Trace.TraceError("Hello");
            Trace.TraceInformation("Hello Information");
            Trace.AutoFlush = true;
        }
    }
    public class CLog
    {
        public string A { get; set; }
        public static void LogCritical(string strMessage)
        {
            _ts.TraceEvent(TraceEventType.Critical,
                           0,
                           "   [{0}{1}]: {2}",
                           DateTime.Now.ToLongDateString(),
                           DateTime.Now.ToLongTimeString(),
                           strMessage);
        }
        public static void LogError(string strMessage)
        {
            _ts.TraceEvent(TraceEventType.Error,
                           0,
                           "      [{0}{1}]: {2}",
                           DateTime.Now.ToLongDateString(),
                           DateTime.Now.ToLongTimeString(),
                           strMessage);
        }
        public static void LogWarning(string strMessage)
        {
            _ts.TraceEvent(TraceEventType.Warning,
                           0,
                           "    [{0}{1}]: {2}",
                           DateTime.Now.ToLongDateString(),
                           DateTime.Now.ToLongTimeString(),
                           strMessage);
        }
        public static void LogInformation(string strMessage)
        {
            _ts.TraceEvent(TraceEventType.Information,
                            0,
                           "[{0}{1}]: {2}",
                           DateTime.Now.ToLongDateString(),
                           DateTime.Now.ToLongTimeString(),
                           strMessage);
        }
        public static void LogData(string strMessage)
        {
            _ts.TraceData(TraceEventType.Information,
                            0,
                           "[{0}{1}]: {2}",
                           DateTime.Now.ToLongDateString(),
                           DateTime.Now.ToLongTimeString(),
                           strMessage);


        }
        /// <summary>
        /// C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\Bin
        /// 输出成XML可以用 Trace View 查看
        /// </summary>
        /// <param name="strMessage"></param>
        public static void LogTraceStart(string strMessage)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            //逻辑操作的开始
            _ts.TraceEvent(TraceEventType.Start, 0, "逻辑操作的开始");
            //启用线程上的逻辑操作
            Trace.CorrelationManager.StartLogicalOperation("Main Operation");
            //写入普通消息
            _ts.TraceInformation("写入一段提示消息");
            //写入错误消息，根据TraceEventType枚举不同的消息级别
            _ts.TraceEvent(TraceEventType.Error, 0, "写入一段错误消息");
            //开启一个线程
            Thread th = new Thread(new ThreadStart(Simple));
            th.Start();
            //停止线程上的逻辑操作
            Trace.CorrelationManager.StopLogicalOperation();
            //逻辑操作的结束
            _ts.TraceEvent(TraceEventType.Stop, 0, "逻辑操作的结束");
        }
        private static void Simple()
        {
            //设置一个活动ID
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            //启用线程上的逻辑操作
            Trace.CorrelationManager.StartLogicalOperation("Simple Operation");
            //写入警告消息
            _ts.TraceEvent(TraceEventType.Warning, 0, "写入一段警告消息");
            //停止线程上的逻辑操作
            Trace.CorrelationManager.StopLogicalOperation();

        }

        private static TraceSource _ts = new TraceSource("LogTrace");
    }
}
