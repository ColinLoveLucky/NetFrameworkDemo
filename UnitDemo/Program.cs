
//using EntityFrameWorkInfrans;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using UnitDemo.ASISDemo;
//using UnitDemo.Compare;
//using UnitDemo.EnumerableT;
//using UnitDemo.Event;
//using UnitDemo.Finalizer;
//using UnitDemo.Fomratter;
//using UnitDemo.Gereneric;
//using UnitDemo.HashAlgorithm;
//using UnitDemo.HashTableAndDictionary;
//using UnitDemo.HttpCache;
//using UnitDemo.ImplictAndExplictApp;
//using UnitDemo.inheritT;
//using UnitDemo.Iterator;
//using UnitDemo.LamdaExpression;
//using UnitDemo.ListHashSetCompare;
//using UnitDemo.MemBaseCopy;
//using UnitDemo.NodeList;
//using UnitDemo.OperateOverride;
//using UnitDemo.RefAndOut;
//using UnitDemo.staticT;
//using UnitDemo.Struct;
//using UnitDemo.TupleDemo;
//using System.Runtime.InteropServices;
//using UnitDemo.SerializeT;
//using System.IO;
//using UnitDemo.Lock;
//using UnitDemo.EMIT;

using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnitDemo.ClosePackage;
using UnitDemo.CodeDomT;
using UnitDemo.ContactProgram;
using UnitDemo.ConvertDemo;
using UnitDemo.DataTableExtensions;
using UnitDemo.Dynamic;
using UnitDemo.EnumDemo;
using UnitDemo.EnumerableT;
using UnitDemo.ExceptionDemo;
using UnitDemo.ExtendMethod;
using UnitDemo.Gereneric;
using UnitDemo.ImplictAndExplictApp;
using UnitDemo.LamdaExpression;
using UnitDemo.ReflectionT;
using UnitDemo.ThreadDemo;
using UnitDemo.TraceDemoNamespace;
namespace UnitDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			//笛卡尔积:设A,B为集合，用A中元素为第一元素，B中元素为第二元素构成有序对，所有这样的有序对组成的集合叫做A与B的笛卡尔积，记作AxB.  


			//---------------以下方法用来测试------------------------  

			//建立数据源 集合l1  
			List<String> l1 = new List<string>();
			for (int i = 1; i < 5; i++)
			{
				l1.Add("a" + i.ToString() + " ");
			}
			//建立数据源 集合l2  
			List<String> l2 = new List<string>();
			for (int i = 1; i < 4; i++)
			{
				l2.Add("b" + i.ToString() + " ");
			}
			//建立数据源 集合l3  
			List<String> l3 = new List<string>();
			for (int i = 1; i < 3; i++)
			{
				l3.Add("c" + i.ToString() + " ");
			}

			//把需要进行笛卡尔积计算的集合放入到 List<List<string>>对象中  
			List<List<string>> dimvalue = new List<List<string>>();
			dimvalue.Add(l1);
			dimvalue.Add(l2);
			dimvalue.Add(l3);

			//建立结果容器  List<string> result  
			List<string> result = new List<string>();

			//传递给计算方法中计算  
			Descartes.run(dimvalue, result);

			//遍历查询结果  
			foreach (string s in result)
			{
				Console.WriteLine(s);
			}
			Console.Read();

		}
		private static void Test_NewMail1(object sender, NewMailEventArgs e)
		{
			Console.WriteLine("SayHi {0}", e.Name);
		}
		private static void Test_NewMail(object sender, NewMailEventArgs e)
		{
			throw new NotImplementedException();
		}
		private static string AllProperties = "name,givenName,samaccountname,mail";
		private static void Search(string domainName, string loginName)
		{
			if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(domainName))
				return;

			string[] properties = AllProperties.Split(new char[] { '\r', '\n', ',' },
								StringSplitOptions.RemoveEmptyEntries);

			try
			{
				DirectoryEntry entry = new DirectoryEntry("LDAP://" + domainName);
				DirectorySearcher search = new DirectorySearcher(entry);
				search.Filter = "(samaccountname=" + loginName + ")";
				foreach (string p in properties)
					search.PropertiesToLoad.Add(p);
				SearchResult result = search.FindOne();
				foreach (string p in properties)
				{
					ResultPropertyValueCollection collection = result.Properties[p];
					for (int i = 0; i < collection.Count; i++)
						Console.WriteLine(p + ": " + collection[i]);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		public class NewUsr
		{
			public string Name { get; set; }
			//public IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
			//{
			//    return null;
			//}

			public string GetName(string name)
			{
				return name;
			}
		}

		List<int> list = new List<int>();

		//low=0，high=9
		//15,2,4,7,4,1,3,8,87,2,10

		//10,2,4,7,4,1,3,8,87,2,10

		//lower 10 

		//10,2,4,7,4,1,3,8,87,2,87

		//lower=7;

		//high=8

		//10,2,4,7,4,1,3,8,2,2,87


		//lower=8;

		//10,2,4,7,4,1,3,8,2,2,87

		//lower=key;v

		// 10,2,4,7,4,1,3,8,2,15,87

		//high=8;
		//high=7
		//10,2,4,7,4,1,3,8,2,15,87


		//8 , 10, 3, 5, 2, 11,29,19
		//5
		//index++=4;
		//Dictionary<string,string>
		//Queue
		//Stack
		//Enumerable
		//Expression<Func<string,string>> _c
	}
	public class TestTask
	{
		public async Task DoSthSync()
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
		}
		public async Task FirstThing()
		{
			Thread.Sleep(2000);
			await DoSthSync();
		}
		public async Task Test()
		{
			await FirstThing();
		}
	}
	public static class ExtSource
	{
		public static IEnumerable<TResult> SelectT<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			foreach (TSource item in source)
			{
				yield return selector(item);
			}
		}
		public static IEnumerable<TResult> SelectManyT<TSource, TResult>(this IEnumerable<TSource> source,
			Func<TSource, IEnumerable<TResult>> selector)
		{
			foreach (TSource item in source)
			{
				foreach (TResult resultItem in selector(item))
				{
					yield return resultItem;
				}
			}

		}
	}
}
