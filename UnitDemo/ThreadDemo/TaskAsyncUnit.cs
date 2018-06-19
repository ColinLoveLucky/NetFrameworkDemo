using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	//Task
	//TaskFactory
	//Parallel
	//Tasks
	//async await
	//TaskWait
	public class TaskAsyncUnit
	{
		//Nito.AsyncEx
		public void TestTaskRunAction()
		{
			var task = Task.Run(() =>
			  {
				  Console.WriteLine("Excuting....");
			  });
			task.Wait();
			Console.WriteLine("Excuted");
		}
		public void TestTaskRunActionCancelToken()
		{
			var cs = new CancellationTokenSource();
			CancellationTokenSource.CreateLinkedTokenSource(cs.Token);
			cs.Token.Register(() =>
			{
				Console.WriteLine("I'M Cancelled");
			});
			Task.Run(() =>
			{
				Console.WriteLine("I'M Ready to Excute");
				Thread.Sleep(4000);
				if (cs.Token.IsCancellationRequested)
				{

					Console.WriteLine("sorrry beacuse of timeout,I'm break off");
					try
					{
						cs.Token.ThrowIfCancellationRequested();
					}
					catch (OperationCanceledException ex)
					{
						throw ex;
					}

				}
			}, cs.Token);
			Task.Delay(3000).Wait();
			cs.Cancel();
			Console.ReadKey();
		}
		public void TestTaskRunActionObjectState()
		{
			var task = new Task((state) =>
			  {
				  Console.WriteLine("state is {0}", state.ToString());
  
			  }, "Hello");
			task.Start();
			task.Wait();
			Console.WriteLine("Excuted");
		}
		public void TestTaskRunFunction()
		{
			var task = Task.Run<string>(() =>
			  {
				  return "Hello";
			  });
			task.Wait();
			Console.WriteLine(task.Result);
		}
		public void TestTaskRunFunctionCanclToken()
		{
			var cs = new CancellationTokenSource();
			cs.Token.Register(() =>
			{
				Console.WriteLine("Cancelled");
			});
			var task = Task.Run<string>(() =>
			  {
				  return "Hello";
			  }, cs.Token);

			Thread.Sleep(2000);
			cs.Cancel();
			Console.ReadKey();
		}
		public void TestTaskRunFunctionTask()
		{
			var task = Task.Run(() =>
			  {
				  return Task.Run(() =>
				  {
					  Console.WriteLine("Hi Task");
				  });
			  });

			task.Wait();
			Console.WriteLine("Excuted");
		}
		public void TestTaskRunFunResult()
		{
			var task = Task.Run(() =>
			   {
				   return "Hi";
			   });

			task.Wait();
			Console.WriteLine("task result:{0}", task.Result);
		}
		public void TestTaskRunFunTaskResult()
		{
			var task = Task.Run(() =>
			   {
				   return Task.Run(() =>
				   {
					   return "Hi";
				   });
			   });
			task.Wait();

			Console.WriteLine(task.Result);
		}
		public void TestTaskDelay()
		{
			Console.WriteLine("I'M Delaying");
			Task.Delay(1000).Wait();
			Console.WriteLine("Delay Excuted");
		}
		public void TestTaskDelayCancel()
		{
			Console.WriteLine("I'M Delaying Cancelled");
			var cs = new CancellationTokenSource();
			var task=Task.Run(() =>
			{
				Thread.Sleep(3000);
				Console.WriteLine("I'M Invoke Cancel to finish");
			});
			Task.Delay(10000, cs.Token).Wait();
			//cs.Cancel();
			Console.WriteLine("before finish before timeout");
		}
		public void TestTaskStatus()
		{
			var task = Task.Run(() =>
			  {
				  Thread.Sleep(3000);
				  Console.WriteLine("Running...");
			  });
			while (true)
			{
				if (task.Status == TaskStatus.Created)
				{
					Console.WriteLine("Have be Initinal");
				}
				if (task.Status == TaskStatus.WaitingForActivation)
				{
					Console.WriteLine("Waiting Excuted activator");
				}
				if (task.Status == TaskStatus.WaitingToRun)
				{
					Console.WriteLine("Have Excuted Wating to Run");
				}
				if (task.Status == TaskStatus.Running)
				{
					Console.WriteLine("Task is Running");
				}
				if (task.Status == TaskStatus.WaitingForChildrenToComplete)
				{
					Console.WriteLine("Wating child to Complete");
				}
				if (task.Status == TaskStatus.Faulted)
				{
					Console.WriteLine("Task Faulted");
				}
				if (task.Status == TaskStatus.RanToCompletion)
				{
					Console.WriteLine("Success finished");
					return;
				}
			}
			Console.ReadKey();
		}
		public void TestTaskCreationOption()
		{
			var task = Task.Run(() =>
			 {
				 Console.WriteLine("I'M Long time Run");
				 Thread.Sleep(140000);
			 });
			Console.WriteLine(task.CreationOptions);
			task.Wait();
			Console.WriteLine("Finshed");
			//task.CreationOptions = TaskCreationOptions.LongRunning;
		}
		public void TestTaskResult()
		{
			var task = Task.FromResult<string>("Hello");
			task.Wait();
			Console.WriteLine(task.Result);
		}
		public void TestTaskThrowException()
		{
			try
			{
				var task = Task.Run(() =>
				{
					Console.WriteLine("Be Ready to throw exception");
					Thread.Sleep(2000);
					throw new Exception("Mistake Message");
				});
				task.Wait();
			}
			catch (AggregateException ex)
			{
				//var exception = Task.FromException<Exception>(ex);

				//exception.Wait();

				//var exceptionResult = exception.Result;
				foreach (var item in ex.InnerExceptions)
					Console.WriteLine(item.Message);
			}
			Console.ReadKey();
		}
		public void TestTaskWaitAll()
		{
			var taskArray = new Task[]{
				Task.Run(()=>{ Thread.Sleep(1000); Console.WriteLine("Task1"); }),
				Task.Run(()=>{ Thread.Sleep(3000); Console.WriteLine("Task2"); }),
				Task.Run(()=>{ Thread.Sleep(2000); Console.WriteLine("Task3"); })
				};
			Task.WaitAll(taskArray);
			Console.WriteLine("Full Excuted");
		}
		public void TestTaskWaitAllTimeOut()
		{
			var taskArray = new Task[]{
				Task.Run(()=>{ Thread.Sleep(1000); Console.WriteLine("Task1"); }),
				Task.Run(()=>{ Thread.Sleep(3000); Console.WriteLine("Task2"); }),
				Task.Run(()=>{ Thread.Sleep(2000); Console.WriteLine("Task3"); })
				};

			var result=Task.WaitAll(taskArray,1000);
			Console.WriteLine("Is All of Finished {0}",result);
		}
		public void TestTaskWatiAllCancel()
		{
			var cs = new CancellationTokenSource();
			var taskArray = new Task[]{
				Task.Run(()=>{
					Thread.Sleep(2000);
					Console.WriteLine("Task1");
				}),
				Task.Run(()=>{ Thread.Sleep(1000); Console.WriteLine("Task2"); }),
				Task.Run(()=>{ Thread.Sleep(3000); Console.WriteLine("Task3"); })
				};
			cs.CancelAfter(2000);
			try
			{
				Task.WaitAll(taskArray, cs.Token);
			}
			catch (OperationCanceledException ex)
			{
				Console.WriteLine("Exception");
			}
			Console.WriteLine("Cancel Excuted");
			Console.ReadKey();
		}
		public void TestTaskWaitAny()
		{
			var taskArray = new Task[]{
				Task.Run(()=>{ Thread.Sleep(1000); Console.WriteLine("Task1"); }),
				Task.Run(()=>{ Thread.Sleep(3000); Console.WriteLine("Task2"); }),
				Task.Run(()=>{ Thread.Sleep(2000); Console.WriteLine("Task3"); })
				};
			var taskIndex = Task.WaitAny(taskArray);
			Console.WriteLine("Excuted Index {0}", taskIndex);
		}
		public void TestTaskWhenAll()
		{
			var taskArray = new Task[]{
				Task.Run(()=>{ Thread.Sleep(1000); Console.WriteLine("Task1"); }),
				Task.Run(()=>{ Thread.Sleep(3000); Console.WriteLine("Task2"); }),
				Task.Run(()=>{ Thread.Sleep(2000); Console.WriteLine("Task3"); })
				};
			var task=Task.WhenAll(taskArray);
			Console.WriteLine("task Id {0}", task.Id);
			task.ContinueWith((t) =>
			{
				Console.WriteLine("t Id {0}", t.Id);
			}).Wait();
			Console.WriteLine("Finshed Excuted");
		}
		public void TestTaskWhenAllTaskResult()
		{
			var taskResultArray = new Task<string>[]{
				Task.FromResult<string>("Hi"),
				Task.FromResult<string>("Hello"),
				Task.FromResult<string>("Glad"),
			};
			var taskWhenAll = Task.WhenAll<string>(taskResultArray);
			foreach (var item in taskWhenAll.Result)
			{
				Console.WriteLine(item);
			}
		}
		public void TestTaskContiuneWith()
		{
			Task.Run(() =>
			 {
				 Console.WriteLine("I'M the first Task");
			 }).ContinueWith((t) =>
			 {
				 Console.WriteLine("I'M the Sencod Task");
			 }).ContinueWith((t) =>
			 {
				 Console.WriteLine("I'M the third Task");
			 }).ContinueWith((t, o) =>
			 {
				 Console.WriteLine("hi {0}", o.ToString());
			 }, "zhangsan").ContinueWith((t, o) =>
			  {
				  Console.WriteLine("I'm Fault {0}", o.ToString());
			  }, "exception", TaskContinuationOptions.PreferFairness).Wait();
		}
		public void TestTaskContiunewithOption()
		{
			try
			{
				Task.Run(() =>
				{
					Console.WriteLine("Hi");
					throw new Exception("Exception");
				}).ContinueWith((t, state) =>
				{
					Console.WriteLine("Exception");
				}, "hi", TaskContinuationOptions.OnlyOnFaulted).Wait();
			}
			catch(AggregateException ex)
			{
				throw ex;
			}

		}
		public void TestTaskFactory()
		{
			Task.Factory.StartNew(() =>
			{
				Console.WriteLine("TaskFactory Excuting");
			}).Wait();
		}
		public void TestTaskFactoryCancel()
		{
			var cs = new CancellationTokenSource();
			try
			{
				Task.Factory.StartNew((state) =>
				{
					Console.WriteLine("hi {0}", state);
					Thread.Sleep(2000);

				}, "zhangsan", cs.Token);

				cs.CancelAfter(1000);

				Console.ReadKey();
			}
			catch (AggregateException ex)
			{
				throw ex;
			}
		}
		public void TestTaskFacoryResult()
		{
			Console.WriteLine(Task.Factory.StartNew<string>(() => "Hi").Result);
		}
		public void TestTaskFactoryFromAsync()
		{
			Action action = () => { Thread.Sleep(1000); Console.WriteLine("Async Method"); };
			var asynResult = action.BeginInvoke(null, null);
			Task.Factory.FromAsync(asynResult, (asyncResult) =>
			 {
				 Thread.Sleep(1000);
				 Console.WriteLine("I'M Async Excuted");
			 });
			Console.ReadKey();
		}
		public void TestTaskFacotoryContiunWhenAll()
		{
			var taskArray = new Task[]{
				Task.Run(()=>{ Thread.Sleep(1000); Console.WriteLine("Task1"); }),
				Task.Run(()=>{ Thread.Sleep(3000); Console.WriteLine("Task2"); }),
				Task.Run(()=>{ Thread.Sleep(2000); Console.WriteLine("Task3"); })
				};

			Task.Factory.ContinueWhenAll(taskArray, (t) =>
			 {
				 Console.WriteLine("When All Excuted");
			 });

			Console.ReadKey();
		}
	}
}
