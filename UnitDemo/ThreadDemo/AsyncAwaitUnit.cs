using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class AsyncAwaitUnit
	{
		public async Task GetAsyncTask()
		{
			await Task.Run(() =>
		   {
			   Thread.Sleep(3000);
			   Console.WriteLine("Hi");
		   });
			Console.WriteLine("Hi 111");
		}
		public async Task<string> GetAsynTaskResult()
		{
			return await Task.FromResult<string>("hi");
		}
	}
}
