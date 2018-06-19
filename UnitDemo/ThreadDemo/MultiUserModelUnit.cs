using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class MultiUserModelUnit
	{
		private static int _addResult = 0;
		private static int _incrementResult = 0;
		private static int _exchangeResult = 0;
		private static int _compareExchangeResult = 55;
		private static int _spinWaitResult = 0;
		public static void TestAddInterlocked()
		{
			Thread t1 = new Thread(() =>
			  {
				  RunAddInterLocked();
			  });
			t1.Start();
			Thread t2 = new Thread(() =>
			{
				RunAddInterLocked();
			});
			t2.Start();
			Thread.Sleep(2000);
			Console.WriteLine(_addResult);
			Console.ReadKey();
		}
		private static void RunAddInterLocked()
		{
			int sum = 0;
			for (int i = 1; i <= 10; i++)
			{
				sum = +i;
				Interlocked.Add(ref _addResult, sum);
			}
		}
		public static void TestIncrementInterlocked()
		{
			Thread t1 = new Thread(() =>
			{
				RunIncrementInterLocked();
			});
			t1.Start();
			Thread t2 = new Thread(() =>
			{
				RunIncrementInterLocked();
			});
			t2.Start();
			Thread.Sleep(2000);
			Console.WriteLine(_incrementResult);
			Console.ReadKey();
		}
		private static void RunIncrementInterLocked()
		{
			for (int i = 1; i <= 10; i++)
			{
				Interlocked.Increment(ref _incrementResult);
			}
		}
		public static void TestExchangeInterlocked()
		{
			Thread t1 = new Thread(() =>
			{
				RunExchangeInterLocked();
			});
			t1.Start();
			Thread t2 = new Thread(() =>
			{
				RunExchangeInterLocked();
			});
			t2.Start();
			Thread.Sleep(2000);
			Console.WriteLine(_exchangeResult);
			Console.ReadKey();
		}
		private static void RunExchangeInterLocked()
		{
			int sum = 0;
			for (int i = 1; i <= 10; i++)
			{
				sum += i;
				Interlocked.Exchange(ref _exchangeResult, sum);
			}
		}
		public static void TestCompareExchangeInterlocked()
		{
			Thread t1 = new Thread(() =>
			{
				RunCompareExchangeInterLocked();
			});
			t1.Start();
			Thread t2 = new Thread(() =>
			{
				RunCompareExchangeInterLocked();
			});
			t2.Start();
			Thread.Sleep(2000);
			Console.WriteLine(_compareExchangeResult);
			Console.ReadKey();
		}
		private static void RunCompareExchangeInterLocked()
		{
			int sum = 0;
			for (int i = 1; i <= 10; i++)
			{
				sum += i;
				Interlocked.CompareExchange(ref _compareExchangeResult, 101, sum);
			}
		}
		public static void TestSpinWait()
		{
			new Thread(() =>
				 {
					 RunSpinWait();
				 }).Start();
			new Thread(() =>
			{
				RunSpinWait();
			}).Start();
			new Thread(() =>
			{
				RunSpinWait();
			}).Start();
			Console.WriteLine("Beigin Calculate.....");
			SpinWait.SpinUntil(() =>
			{
				return _spinWaitResult == 30;
			});
			Console.WriteLine("End Calculate:{0}......", _spinWaitResult);
		}
		private static void RunSpinWait()
		{
			int sum = 0;
			for (int i = 1; i <= 10; i++)
			{
				Interlocked.Add(ref _spinWaitResult,1);
			}
		}
		public static void TestSpinLock()
		{
			var li = new List<int>();
			var s1 = new SpinLock();
			Parallel.For(0, 1000 * 1000, r =>
			{
				bool gotLock = false;
				s1.Enter(ref gotLock);
				li.Add(r);
				if (gotLock)
				{
					s1.Exit();
				}
			});
			Console.WriteLine("Li count::{0}", li.Count);
		}
	}
}
