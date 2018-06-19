using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{

	public class AsyncDelegateUnit
	{
		private delegate void _deleInvoke(object value);
		public void TestDele()
		{
			_deleInvoke d = RunDeleInvoke;
			d("Hi");
		}
		public void TestAsyncDele()
		{
			Console.WriteLine("请求异步返回结果");
			_deleInvoke d = RunDeleInvoke;
			var asyncResult = d.BeginInvoke("hi", AsyncCallBackFuntion, "zhangsan");
			d.EndInvoke(asyncResult);
			Console.ReadKey();
		}
		private void AsyncCallBackFuntion(IAsyncResult async)
		{
			if (async.IsCompleted)
			{
				Console.WriteLine("I'M Finished Excuted");
			}
		}
		public void RunDeleInvoke(object value)
		{
			Console.WriteLine("Delegate Invoke:{0}", value.ToString());
			Thread.Sleep(10000);
		}
	}
}
