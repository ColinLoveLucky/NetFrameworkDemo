using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class MutiMixThreadUnit
	{
		private static ManualResetEventSlim _manualResetEventSlim;
		private static SemaphoreSlim _semphoreSlim;
		private static readonly object _lockobj = new object();
		private static int _lockResult;
		private static ReaderWriterLock _readerWriterLock;
		private static List<int> _listdata;
		private static ReaderWriterLockSlim _readerWriterLockSlim;
		private static CountdownEvent _countDownEvent;
		static MutiMixThreadUnit()
		{
			_manualResetEventSlim = new ManualResetEventSlim(true);
			_semphoreSlim = new SemaphoreSlim(5, 5);
			_readerWriterLock = new ReaderWriterLock();
			_listdata = new List<int>();
			_readerWriterLockSlim = new ReaderWriterLockSlim();
			_countDownEvent = new CountdownEvent(2);
		}
		public static void TestManualResetEventSlim()
		{
			new Thread((state) =>
			{
				RunManualResetEventSlim(state.ToString());
			}).Start("t1");
			new Thread((state) =>
			{
				RunManualResetEventSlim(state.ToString());
			}).Start("t2");
			new Thread((state) =>
			{
				RunManualResetEventSlim(state.ToString());
			}).Start("t3");

		}
		private static void RunManualResetEventSlim(string threadName)
		{
			try
			{
				Thread.Sleep(1000);
				_manualResetEventSlim.Wait();
				Console.WriteLine("{0} Sleeping", threadName);
				Console.WriteLine("{0} Excuted", threadName);
			}
			catch
			{
			}
			finally
			{
				_manualResetEventSlim.Set();
			}

		}
		public static void TestSemphoreSlim()
		{

			for (int i = 0; i < 10; i++)
			{
				Thread t = new Thread(() =>
				{
					_semphoreSlim.Wait();
					Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "进洗手间：" + DateTime.Now.ToString());
					Thread.Sleep(2000);
					Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "出洗手间：" + DateTime.Now.ToString());
					_semphoreSlim.Release();
				});
				t.Start();
			}

			Console.ReadKey();
		}
		public static void TestLock()
		{
			Console.WriteLine("Begining Calculate.....");
			for (int i = 1; i <= 10; i++)
			{
				new Thread(() =>
				{
					lock (_lockobj)
					{
						for (int j = 1; j <= 10; j++)
							_lockResult += j;
					}
				}).Start();
			}
			Thread.Sleep(3000);
			Console.WriteLine("Calculate Result is {0}", _lockResult);
		}
		public static void TestMonitor()
		{	
			Console.WriteLine("Begining Calculate.....");
			for (int i = 1; i <= 10; i++)
			{
				new Thread(() =>
				{
					try
					{
						Monitor.Enter(_lockobj);
						
							for (int j = 1; j <= 10; j++)
								_lockResult += j;
					}
					catch {
					}
					finally {
						Monitor.Exit(_lockobj);
					}
					
				}).Start();
			}
			Thread.Sleep(3000);
			Console.WriteLine("Calculate Result is {0}", _lockResult);
		}
		public static void TestReaderWriterLock()
		{
			for (int i = 0; i < 10; i++)
			{
				new Thread((state) =>
				{
					Thread.Sleep(2000);
					RunWriterListData((int)state);
				}).Start(i);
			}
			for (int i = 0; i < 10; i++)
			{
				new Thread(() =>
				{
					Thread.Sleep(1000);
					RunReaderListData();
				}).Start();
			}
			Console.ReadKey();

		}
		private static void RunWriterListData(int data) {
			try
			{
				Console.WriteLine("Begining writer data is {0}", data);
				_readerWriterLock.AcquireWriterLock(Timeout.Infinite);
				_listdata.Add(data);
				Console.Write("data is {0} writer success", data);
			}
			catch
			{
			}
			finally
			{
				_readerWriterLock.ReleaseWriterLock();
			}
		}
		private static void RunReaderListData() {
			try
			{
				Console.WriteLine("Begining Reader List Data");
				_readerWriterLock.AcquireWriterLock(Timeout.Infinite);
				Console.WriteLine("The List Count is {0}", _listdata.Count);

			}
			catch
			{
			}
			finally
			{
				_readerWriterLock.ReleaseWriterLock();
			}
		}
		public static void TestReaderWriterLockSlim()
		{
			for (int i = 0; i < 10; i++)
			{
				new Thread((state) =>
				{
					Thread.Sleep(2000);
					RunWriterSlimListData((int)state);
				}).Start(i);
			}
			for (int i = 0; i < 10; i++)
			{
				new Thread(() =>
				{
					Thread.Sleep(1000);
					RunReaderSlimListData();
				}).Start();
			}
			Console.ReadKey();
		}
		private static void RunWriterSlimListData(int data)
		{
			try
			{
				Console.WriteLine("Begining writer data is {0}", data);
				_readerWriterLockSlim.EnterWriteLock();
				_listdata.Add(data);
				Console.Write("data is {0} writer success", data);
			}
			catch
			{
			}
			finally
			{
				_readerWriterLockSlim.ExitWriteLock();
			}
		}
		private static void RunReaderSlimListData()
		{
			try
			{
				Console.WriteLine("Begining Reader List Data");
				_readerWriterLockSlim.EnterReadLock();
				Console.WriteLine("The List Count is {0}", _listdata.Count);
			}
			catch
			{
			}
			finally
			{
				_readerWriterLockSlim.ExitReadLock();
			}
		}
		public  static void TestCountDownEvent()
		{
			Console.WriteLine("开始两个操作");
			var t1 = new Thread(() => PerformOperation("操作1完成", 4));
			var t2 = new Thread(() => PerformOperation("操作2完成", 8));
			t1.Start();
			t2.Start();
			// 阻塞当前线程，直到 CountdownEvent 的信号数量变为 0
			_countDownEvent.Wait();
			Console.WriteLine("两个操作都已完成.");
			_countDownEvent.Dispose();
		}
		private static void PerformOperation(string message, int seconds)
		{
			Thread.Sleep(TimeSpan.FromSeconds(seconds));
			Console.WriteLine(message);
			// 减少 1 个信号
			_countDownEvent.Signal();
		}
		public static void TestBarrier()
		{
			int count = 0;
			// Create a barrier with three participants
			// Provide a post-phase action that will print out certain information
			// And the third time through, it will throw an exception
			Barrier barrier = new Barrier(3, (b) =>
			{
				Console.WriteLine("Post-Phase action: count={0}, phase={1}", count, b.CurrentPhaseNumber);
				if (b.CurrentPhaseNumber == 2) throw new Exception("D'oh!");
			});

			// Nope -- changed my mind.  Let's make it five participants.
			barrier.AddParticipants(2);
			// Nope -- let's settle on four participants.
			barrier.RemoveParticipant();
			// This is the logic run by all participants
			Action action = () =>
			{
				Interlocked.Increment(ref count);
				barrier.SignalAndWait(); // during the post-phase action, count should be 4 and phase should be 0
				Interlocked.Increment(ref count);
				barrier.SignalAndWait(); // during the post-phase action, count should be 8 and phase should be 1

				// The third time, SignalAndWait() will throw an exception and all participants will see it
				Interlocked.Increment(ref count);
				try
				{
					barrier.SignalAndWait();
				}
				catch (BarrierPostPhaseException bppe)
				{
					Console.WriteLine("Caught BarrierPostPhaseException: {0}", bppe.Message);
				}

				// The fourth time should be hunky-dory
				Interlocked.Increment(ref count);
				barrier.SignalAndWait(); // during the post-phase action, count should be 16 and phase should be 3
			};

			// Now launch 4 parallel actions to serve as 4 participants
			Parallel.Invoke(action, action, action, action);

			// This (5 participants) would cause an exception:
			// Parallel.Invoke(action, action, action, action, action);
			//      "System.InvalidOperationException: The number of threads using the barrier
			//      exceeded the total number of registered participants."

			// It's good form to Dispose() a barrier when you're done with it.
			barrier.Dispose();
		}
	}
}
