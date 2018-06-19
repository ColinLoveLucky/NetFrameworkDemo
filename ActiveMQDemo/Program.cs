using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQDemo
{
	/// <summary>
	/// http://blog.csdn.net/clj198606061111/article/details/38145597
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			SubHub test = new SubHub();
			test.Send();
		}
	}
}
