using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class ThreadPoolUnit
	{
		public static void TestThreadPool()
		{
			var isChange = ThreadPool.SetMaxThreads(10, 10);
			Console.WriteLine(isChange);
			ThreadPool.QueueUserWorkItem(InvokeMethod, "zhang san");
			ThreadPool.QueueUserWorkItem(InvokeMethod, "lisi");
			ThreadPool.QueueUserWorkItem(InvokeMethod, "wangwu");
			ThreadPool.QueueUserWorkItem(InvokeMethod, "zhuliu");
			ThreadPool.QueueUserWorkItem(InvokeMethod, "wangqi");
			Console.ReadKey();
		}
		private static void InvokeMethod(Object state)
		{
			Console.WriteLine("Hi:{0}", state);
		}
		public static void TestThreadPoolWaitHandler()
		{
			AutoResetEvent ar = new AutoResetEvent(false);
			ThreadPool.RegisterWaitForSingleObject(ar, RunWaitHandler, null,Timeout.Infinite, false);
			Console.WriteLine("时间:{0} 工作线程请注意，您需要等待5s才能执行。\n", DateTime.Now);
			Thread.Sleep(5000);
			ar.Set();
			Console.WriteLine("时间:{0} 工作线程已执行。\n", DateTime.Now);
			Console.ReadKey();
		}
		private static void RunWaitHandler(object obj, bool sign)
		{
			Console.WriteLine("当前时间:{0}  我是线程{1}\n", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
		}
	}
}
