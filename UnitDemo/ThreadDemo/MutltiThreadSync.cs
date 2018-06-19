using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class MutltiThreadSync
	{
		private static EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
		private  Mutex _mutex = new Mutex(false);
		//Kernal Model
		//WaitHandle
		public void TestWaitHandler()
		{
		}
		public static void TestEventWaitHandle()
		{
			Console.WriteLine("WaitHandle Waiting 5000");
			Thread t1 = new Thread(() =>
			{
				eventWaitHandle.WaitOne(5000,true);
				Console.WriteLine("Excute");
			});
			t1.Start();
			Thread t2 = new Thread(() =>
			  {
				  eventWaitHandle.Set();
			  });
			t2.Start();
			Console.WriteLine("WaitHandle Waiting 1000");
			Console.ReadKey();
		}
		public static void AutoResetWaitHandle()
		{
			AutoResetEvent autoResetEvent = new AutoResetEvent(false);
			Console.WriteLine("WaitHandle Waiting 5000");
			Thread t2 = new Thread(() =>
			{
				autoResetEvent.Set();
			});
			t2.Start();
			autoResetEvent.WaitOne();
			Console.WriteLine("WaitHandle Waiting 1000");
			autoResetEvent.WaitOne();
			Console.WriteLine("Is Reset single Status");
			Console.ReadKey();
		}
		public static void MenuResetHandle()
		{
			ManualResetEvent meunuReset = new ManualResetEvent(false);
			Console.WriteLine("WaitHandle Waiting 5000");
			Thread t2 = new Thread(() =>
			{
				meunuReset.Set();
			});
			t2.Start();
			meunuReset.WaitOne();
			Console.WriteLine("WaitHandle Waiting 1000");
			meunuReset.WaitOne();
			Console.WriteLine("Single Status Not Reset");
			Console.ReadKey();
		}
		public static void MenuResetHandleWaitAll()
		{
			ManualResetEvent meunuReset = new ManualResetEvent(false);
			ManualResetEvent menuReset2 = new ManualResetEvent(false);
			Console.WriteLine("Waiting all signled");
			Thread t2 = new Thread(() =>
			{
				Thread.Sleep(2000);
				meunuReset.Set();
			});
			t2.Start();
			Thread t3 = new Thread(() =>
			{
				Thread.Sleep(3000);
				menuReset2.Set();
			});
			t3.Start();
			WaitHandle.WaitAll(new WaitHandle[] { menuReset2, meunuReset });
			Console.WriteLine("Get all signled,finished current thread");
			Console.ReadKey();
		}
		public static void TestSemphore()
		{
			Semaphore sema = new Semaphore(5, 5);
			for (int i = 0; i < 10; i++)
			{
				Thread t = new Thread(() =>
				  {
					  sema.WaitOne();
					  Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "进洗手间：" + DateTime.Now.ToString());
					  Thread.Sleep(2000);
					  Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "出洗手间：" + DateTime.Now.ToString());
					  sema.Release();
				  });
				t.Start();
			}

			Console.ReadKey();
		}
		public  void TestMutex()
		{
			Thread t1 = new Thread((state)=> {
				RunMutex(state);
			});
			t1.Start("t1");
			Thread t2 = new Thread((state) =>
			{
				RunMutex(state);
			});
			t2.Start("t2");
		}
		public  void RunMutex(object state)
		{
			try
			{
				Console.WriteLine("Waiting {0} Mutex Realease....", state);
				_mutex.WaitOne();
			}
			catch (Exception e)
			{
			}
			finally
			{
				Thread.Sleep(1000);
				_mutex.ReleaseMutex();
				Console.WriteLine("Get {0} Sigled Excute Finshed", state);
			}
		}
	}
}
