using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Gereneric
{
	public class GenericClass<T>
	{
		private T _data;
		public T Data
		{
			get; set;
		}
		public T GetData()
		{
			T temp = default(T);
			if (Data != null)
			{
				temp = Data;
			}
			return temp;
		}
	}
	public class GenericClassRestrictionStruct<T> where T : struct
	{
		public T Data
		{
			get; set;
		}
	}
	public class GenericClassRestrictionClass<T> where T : class
	{
		public delegate T _delegate(T data);
		public T Data;
	}
	public class GenericClassRestrictionNew<T> where T : new()
	{
		public T Data
		{
			get; set;
		}
	}
	public class GenericClassRestrictionInterface<T> where T : ICollection<T>
	{
		public T Data
		{
			get;set;
		}
	}
	public class GenericClassRestrictionMuti<T, U> where T : new()
		where U : struct
	{
		public T Data
		{
			get;set;
		}
		public U DataValue
		{
			get;set;
		}
	}
	public class GenericClassRestrictSingile<T> where T : ICollection<T>, new()
	{
		public T Data
		{
			get;set;
		}
		public T this[int index]
		{
			get
			{
				return default(T);
			}
			set
			{
			}
		}
	}

}
