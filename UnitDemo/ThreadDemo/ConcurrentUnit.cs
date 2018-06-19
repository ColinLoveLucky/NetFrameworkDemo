using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public class ConcurrentUnit
	{
		private static ConcurrentQueue<int> _concurrentQueue;
		private static ConcurrentStack<int> _concurrentStack;
		private static ConcurrentBag<int> _concurrentBag;
		private static ConcurrentDictionary<int, int> _concurrentDictionary;
		static ConcurrentUnit()
		{
			_concurrentQueue = new ConcurrentQueue<int>();
			_concurrentStack = new ConcurrentStack<int>();
			_concurrentBag = new ConcurrentBag<int>();
			_concurrentDictionary = new ConcurrentDictionary<int, int>();
		}
		public static void TestConcurrentQueue()
		{
			for (int i = 0; i < 10; i++)
			{
				new Thread(() =>
				{
					RunConcurrentQueue();
				}).Start();
			}
			Thread.Sleep(2000);
			Console.WriteLine("ConcurrentQueue Count:{0}", _concurrentQueue.Count);
		}
		private static void RunConcurrentQueue()
		{
			for (int i = 0; i < 10; i++)
			{
				_concurrentQueue.Enqueue(i);
			}
		}
		public static void TestConcurrentStack()
		{
			for (int i = 0; i < 10; i++)
			{
				new Thread(() =>
				{
					RunConcurrentStatck();
				}).Start();
			}
			Thread.Sleep(2000);
			Console.WriteLine("ConcurrentStack Count:{0}", _concurrentStack.Count);
		}
		private static void RunConcurrentStatck()
		{
			for (int i = 0; i < 10; i++)
			{
				_concurrentStack.Push(i);
			}
		}
		public static void TestConcurrentBag()
		{
			for (int i = 0; i < 10; i++)
			{
				new Thread(() =>
				{
					RunConcurrentBag();
				}).Start();
			}
			Thread.Sleep(2000);
			Console.WriteLine("ConcurrentBag Count:{0}", _concurrentBag.Count);
		}
		private static void RunConcurrentBag()
		{
			for (int i = 0; i < 10; i++)
			{
				_concurrentBag.Add(i);
			}
		}
		public static void TestConcurrentDictionary()
		{
			for (int i = 0; i < 10; i++)
			{
				new Thread(() =>
				{
					RunConcurrentDictionary();
				}).Start();
			}
			Thread.Sleep(2000);
			Console.WriteLine("ConcurrentDictionary Count:{0}", _concurrentDictionary.Count);
		}
		private static void RunConcurrentDictionary()
		{
			for (int i = 0; i < 10; i++)
			{
				_concurrentDictionary.AddOrUpdate(i, i, (key, value) =>
				  {
					  return key + new Random().Next();
				  });
			}
		}
	}
}
