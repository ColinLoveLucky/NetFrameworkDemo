using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class ThreadContextUnit
	{
		private static Thread _thread;
		private static Thread _thread2;
		private static Context _contex;

		static ThreadContextUnit()
		{
			_thread = new Thread(() =>
			{
				_contex = Thread.CurrentContext;
				Thread.Sleep(1000);
				Console.WriteLine("thread1 managerId:{0}", _thread.ManagedThreadId);
				Console.WriteLine("current thread contextId:{0}", Thread.CurrentContext.ToString());
			});

			_thread2 = new Thread(() =>
			{
				Thread.Sleep(2000);
				if (_contex ==Thread.CurrentContext)
				{
					Console.WriteLine("....................");
				}
				Console.WriteLine("_thread2 managerId:{0}", _thread2.ManagedThreadId);
				Console.WriteLine("current thread2 contextId:{0}", Thread.CurrentContext.ToString());
			});
		}

		public static void TestThreadContext()
		{
			_thread.Start();
			_thread2.Start();
			//Console.WriteLine("thread managerId:{0}", Thread.CurrentThread.ManagedThreadId);
			//Console.WriteLine("thread ContextId:{0}", Thread.CurrentContext.ContextID);
		}
	}
}
