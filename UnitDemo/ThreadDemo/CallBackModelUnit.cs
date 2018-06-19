using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class CallBackModelUnit
	{
		public void TestCallBackFun()
		{
			var callBackClass = new CallBackFunctionClass<string>();
			new Thread(() =>
			{
				Thread.Sleep(3000);
				callBackClass.Result = "Hello";
			});
		}
	}
	public class CallBackFunctionClass<T>
	{
		private T _result;
		public T Result
		{
			get;set;
		}
		public T GetResult(T a)
		{
			T temp = default(T);
			temp = a;
			_result = temp;
			return _result;
		}
	}
}
