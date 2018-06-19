using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Gereneric
{
	public class GenericMethodUnit
	{	
		public GenericMethodUnit()
		{
		}
		public T GetValue<T>(T a)
		{
			return a;
		}
		public T GetLast<T>()
		{
			return default(T);
		}
	}
}
