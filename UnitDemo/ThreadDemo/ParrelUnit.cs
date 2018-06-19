using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class ParrelUnit
	{
		public void TestParrelInvoke()
		{
			Parallel.Invoke(() =>
			{
				Console.WriteLine("Hi");
			}, () => { Console.WriteLine("zhangsan"); });

			Console.WriteLine("Parell Excuted");
		}
		public void TestParellInvokeOption()
		{
			var cs = new CancellationTokenSource();
			var option = new ParallelOptions()
			{
				CancellationToken = cs.Token
			};
			Task.Factory.StartNew(() =>
			{
				Thread.Sleep(3000);
				cs.Cancel();
			});
			try
			{
				Parallel.Invoke(option, () =>
				{

					Console.WriteLine("Cancel");
				}, () =>
				{
					while (true)
					{
						Thread.Sleep(1000);
						if (option.CancellationToken.IsCancellationRequested)
							break;
						Console.WriteLine("zhangsan");
						throw new Exception("hi");
					}
				});
			}
			catch (AggregateException ex)
			{
				Console.WriteLine("Exception");
			}

		}
		public void TestParellFor()
		{
			try
			{
				var result = Parallel.For(1, 10, (i) =>
				{
					Console.WriteLine(i);
					if (i == 8)
						throw new Exception();
				});
				Console.WriteLine("parallel result {0} break {1}", result.IsCompleted, result.LowestBreakIteration);
			}
			catch (AggregateException ex)
			{
			}

		}
		public void TestParallForParallelLoopState()
		{

			Parallel.For(1, 100, (i, loopState) =>
			  {
				  Console.WriteLine("i is excute {0}", i);
				  if (i >= 8)
				  {
					  loopState.Stop();
				  }
			  });
			Console.WriteLine("Excuted");
		}
		public void TestParallForParallFun()
		{
			var random = new Random();
			Parallel.For(1, 2, () =>
			   {
				   var value = random.Next();
				   Console.WriteLine("local random initValue {0}", value);
				   return value;
			   }, (i, loopState, local) =>
			   {
				   Console.WriteLine("change i {0} local {1}", i, local + 10);
				   return (local + 10);
			   }, (local) =>
			  {
				  Console.WriteLine("the final excuted {0}", local + 10);
			  });
		}
		public void TestParallForEach()
		{
			List<int> list = new List<int>() { 1, 2, 3, 4, 5 };
			Parallel.ForEach<int>(list, (i) =>
			 {
				 Console.WriteLine(i + 10);
			 });
		}
		public void TestParallForEachSoureLocal()
		{
			List<int> list = new List<int>() { 1, 2 };
			var random = new Random();
			Parallel.ForEach<int, int>(list, () =>
			  {
				  var value = random.Next(10);
				  Console.WriteLine("local initValue {0}", value);
				  return value;
			  }, (Tsource, loopState, local) =>
			 {
				 Console.WriteLine("TSource {0} add local {1} equal {2}", Tsource, local, Tsource + local);
				 return local + Tsource;
			 }, (localfinally) =>
			{
				Console.WriteLine("localFinally {0}", localfinally + 10);
			});
		}
		public void TestParallForEachPartitioner()
		{
			IList<int> list = new List<int>() { 1, 2, 3,4,5,6,7,8 };
			Parallel.ForEach(Partitioner.Create<int>(list, true), (tsource) =>
			 {
				 Console.WriteLine("hi tSource {0}", tsource);
			 });
			Parallel.ForEach(Partitioner.Create(1, 5), (source) =>
			  {
				  Console.WriteLine("hi parti source {0} {1}", source.Item1, source.Item2);
			  });
		}
	}
}
