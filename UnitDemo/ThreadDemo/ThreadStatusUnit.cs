using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class ThreadStatusUnit
	{
		private static Thread _thread;
		static ThreadStatusUnit()
		{
			_thread = new Thread(() =>
			{
				Console.WriteLine("thread mangerId:{0},{1}", Thread.CurrentThread.ManagedThreadId,_thread.ManagedThreadId);
				//Thread.Sleep(2000);
				Console.WriteLine("Hi");
			});
		}
		public static void ThreadStatusUnstarted()
		{
			Console.WriteLine("ThreadUnStarted status:{0}", _thread.ThreadState);
		}
		public static void ThreadStatusRunning()
		{
			//Exception Thread Start
			_thread.Start();
			Console.WriteLine("ThreadStatusRunning Status:{0}", _thread.ThreadState);
		}
		public static void ThreadStatusAbort()
		{
			_thread.Start();
			_thread.Abort();
			Console.WriteLine("ThreadStatusAbort Status:{0}", _thread.ThreadState);
		}
		public static void ThreadStatusStop()
		{
			//Thread is Stopped 
			//Cant't Regain
			_thread.Start();
			_thread.Join(1000);
			Console.WriteLine("ThreadStatusStop Status:{0}", _thread.ThreadState);
			//_thread.Start();
			//Thread.Sleep(1000);
			//Console.WriteLine("ThreadStatusWaitSleep:{0}", _thread.ThreadState);
		}
		public static void ThreadStatusWaitSleep()
		{
			_thread.Start();
			Thread.Sleep(1000);
			Console.WriteLine("ThreadStatusWaitSleep:{0}", _thread.ThreadState);
		}
		public static void ThreadStatusBackground()
		{
			_thread.Start();
			_thread.IsBackground = true;
			Thread.Sleep(1000);
			Console.WriteLine("ThreadStatusBackground:{0}", _thread.ThreadState);
		}
	}
}
