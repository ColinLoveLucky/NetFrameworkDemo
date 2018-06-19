using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ReflectionT
{
	public class CustomerAttr : Attribute
	{
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		public CustomerAttr(string name)
		{
			_name = name;
		}
	}
	[Serializable]
	public class RelectionTypeUnit
	{
		public delegate void delegateMethod(object value);
		public event delegateMethod _event;
		protected int _filed;
		private int _privateFiled;
		private string _name;
		public RelectionTypeUnit()
		{
		}
		public RelectionTypeUnit(string name)
		{
			_name = name;
		}
		protected int Name
		{
			get;
			set;
		}
		[CustomerAttr("Zhangsan")]
		public void TestRelectType()
		{
			Console.WriteLine("Show the Type name toString:{0}", this.GetType().ToString());
			Console.WriteLine("Show the Type BaseType name toString:{0}", this.GetType().BaseType.ToString());
			Console.WriteLine("Show the Type fullName:{0}", this.GetType().FullName.ToString());
			Console.WriteLine("Show the Type isClass:{0}", this.GetType().IsClass);
			Console.WriteLine("Show the Type isAbstract {0}", this.GetType().IsAbstract);
			Console.WriteLine("Show the Type isPublic {0}", this.GetType().IsPublic);
			Console.WriteLine("Show the Type isNest {0}", this.GetType().IsNested);
			Console.WriteLine("Show the Type IsGenericType {0}", this.GetType().IsGenericType);
			Console.WriteLine("Show the Type IsInterface {0}", this.GetType().IsInterface);
			//Console.WriteLine("Show the Type Constructor {0}", this.GetType().GetConstructor(new Type[] { this.GetType() }).Invoke(null).ToString());
			foreach (MethodInfo item in this.GetType().GetMethods())
			{
				Console.WriteLine("MethodInfo {0}", item.ToString());
			}
			foreach (FieldInfo item in this.GetType().GetFields())
			{
				Console.WriteLine("FieldInfo {0}", item.ToString());
			}
			//Console.WriteLine("Private Filed {0}", this.GetType().GetField("_privateFiled", BindingFlags.NonPublic).ToString());
			foreach (MemberInfo item in this.GetType().GetMembers())
			{
				Console.WriteLine("MemberInfo {0}", item);
			}
			foreach (PropertyInfo item in this.GetType().GetProperties())
			{
				Console.WriteLine("PropertyInfo {0}", item.ToString());
			}
			//Console.WriteLine(this.GetType().GetProperty("Name",BindingFlags.NonPublic).ToString());
			foreach (EventInfo item in this.GetType().GetEvents())
			{
				Console.WriteLine("EventInfo {0}", item);
			}
			foreach (Attribute item in this.GetType().GetCustomAttributes())
			{
				Console.WriteLine("CustomerAttr {0}", item);
			}
		}
		public void TestCreateInstaceType()
		{
			var t = this.GetType();
			var obj = t.Assembly.CreateInstance(this.GetType().ToString(), true);
			if (obj is RelectionTypeUnit)
			{
				var tRe = obj as RelectionTypeUnit;
				tRe.TestRelectType();
			}
			var tConstrus = t.Assembly.CreateInstance(this.GetType().ToString(), true, BindingFlags.Default, null, new object[] { "Hello World" }, null, null);
			if (tConstrus is RelectionTypeUnit)
			{
				var tRe = tConstrus as RelectionTypeUnit;
				Console.WriteLine("RelectPrivate Name is {0}", tRe._name);
			}
			var tActivor = Activator.CreateInstance<RelectionTypeUnit>();
			tActivor.TestRelectType();
			var tActivorArg = Activator.CreateInstance(this.GetType(), new object[] { "Hello" }) as RelectionTypeUnit;
			Console.WriteLine("tActivor _name {0}", tActivorArg._name);
		}
		public void TestCreateMthodType()
		{
			var t = Activator.CreateInstance(this.GetType(), new object[] { "hi" });
			var method = this.GetType().GetMethod("TestRelectType", BindingFlags.Public | BindingFlags.Instance);
			method.Invoke(t, null);
		}
		public void TestCreateStaticType()
		{
			var t = Type.GetType("UnitDemo.ReflectionT.StaticRelectClass");
			var method = t.GetMethod("GetFiled", BindingFlags.Static | BindingFlags.Public);
			Console.WriteLine(method.ToString());
			var value = method.Invoke(null, null);
			Console.WriteLine(value.ToString());
			var staticFiled = t.GetField("_privateFiled", BindingFlags.Static | BindingFlags.NonPublic);
			var staticFieldValue = staticFiled.GetValue(null);
			Console.WriteLine("StaticFieldValue {0}", staticFieldValue);
			var tInstance = Activator.CreateInstance(this.GetType());
			var tSaticMethod = this.GetType().GetMethod("GetStaticValue", BindingFlags.Static | BindingFlags.Public);
			var tStaticValue = tSaticMethod.Invoke(null, null);
			Console.WriteLine(tStaticValue.ToString());
		}
		public void TestCreateCustomerAttr()
		{
			var t = this.GetType();
			var method = t.GetMethod("TestRelectType", BindingFlags.Public | BindingFlags.Instance);
			Console.WriteLine(method.ToString());
			var custormerAttr = method.GetCustomAttribute<CustomerAttr>();
			Console.WriteLine(custormerAttr.Name.ToString());
		}
		public static int GetStaticValue()
		{
			return 10;
		}
		public void RunRelectType()
		{
			Console.WriteLine("Hello World");
		}
		public void TestCreateGenericType()
		{
			var t = this.GetType();
			var obj = Activator.CreateInstance(t);
			var methodInfo = t.GetMethod("Method", BindingFlags.Instance | BindingFlags.Public);
			var tGeneric = methodInfo.MakeGenericMethod(typeof(string));
			var objValue = tGeneric.Invoke(obj, new object[] { "Hi" });
			Console.WriteLine("ObjValue {0}", objValue);
			var tGenericClassType = Type.GetType("UnitDemo.ReflectionT.GenericClass`1");
			var tGenercicClassMethodType = tGenericClassType.MakeGenericType(new Type[] { typeof(string) });
			var instanceMethod = tGenercicClassMethodType.GetMethod("GetName", BindingFlags.Public | BindingFlags.Instance);
			var objInstanceActivor = Activator.CreateInstance(tGenercicClassMethodType);
			var objGenericValue = instanceMethod.Invoke(objInstanceActivor, new object[] { "Hi" });
		}
		public T Method<T>(T a)
		{
			return a;
		}
	}
	public static class StaticRelectClass
	{
		private static int _privateFiled = 10;
		public static int GetFiled()
		{
			return _privateFiled;
		}
	}
	public class GenericClass<T>
	{
		public T GetName(T a)
		{
			return a;
		}
	}

}
