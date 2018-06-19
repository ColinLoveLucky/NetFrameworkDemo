using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Gereneric
{
	public interface GenricInterface<out T, in V> {
		T GetValue();

		string InputValue(V data);
	}
	public class GenericChangeUnit<T, V> : GenricInterface<T, V>
	{
		public T Data
		{
			get;set;
		}
		public V Input
		{
			get;set;
		}
		public T GetValue()
		{
			throw new NotImplementedException();
		}
		public string InputValue(V data)
		{
			throw new NotImplementedException();
		}
	}
	public interface AnimalT
	{

	}
	public class PigT : AnimalT
	{
	}
	public class TestGenericChangeClass
	{
		public void TestMethod()
		{
			AnimalT animal = new PigT();
			GenricInterface<PigT, AnimalT> _tchild = new GenericChangeUnit<PigT, AnimalT>();
			GenricInterface<AnimalT, PigT> _t = _tchild;
		}
	}
}
