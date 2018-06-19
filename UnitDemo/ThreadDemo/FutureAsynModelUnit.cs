using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public static class ShareWaitHandle
	{
		public static ManualResetEvent ManualResetEventHadle = new ManualResetEvent(false);
	}
	public interface IData
	{
		string GetResult();
	}
	public class FutureData : IData
	{
		private RealData realData;
		private bool isReady = false;
		public void SetRealData(RealData realData)
		{
			if (isReady)
				return;
			this.realData = realData;
			isReady = true;
			//Notify();
			ShareWaitHandle.ManualResetEventHadle.WaitOne();
		}
		public string GetResult()
		{
			while (!isReady)
			{
				try
				{
					Console.WriteLine("Waiting .....");
				}
				catch
				{
				}
			}
			//Console.WriteLine("Wating....");
			//ShareWaitHandle.ManualResetEventHadle.WaitOne();
			return this.realData.GetResult();
		}
	}
	public class RealData : IData
	{
		private string result;

		public RealData(string queryStr)
		{
			Console.WriteLine("根据 {0} 进行查询一个很耗时的操作", queryStr);
			try
			{
				Thread.Sleep(5000);
			}
			catch
			{
			}
			finally
			{
				ShareWaitHandle.ManualResetEventHadle.Set();
				Console.WriteLine("操作完毕,获取结果");
				result = "查询结果";
			}
		}
		public string GetResult()
		{
			return result;
		}
	}
	public class FutureAsynModelUnit
	{
		public void TestFutureClient()
		{
			FutureData futureData = new FutureData();
			new Thread(() =>
			{
				RealData realData = new RealData("http://www.baidu.com");
				futureData.SetRealData(realData);
			}).Start();
			var result=futureData.GetResult();
			Console.ReadKey();
		}
	}
}
