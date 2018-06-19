using CrypDemo.asymmetric_Encryption;
using CrypDemo.base64;
using CrypDemo.HMACAlg;
using CrypDemo.md5;
using CrypDemo.SymmetricEncrytion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrypDemo
{
	class Program
	{
		static void Main(string[] args)
		{
		}
	}
	public class Family
	{
		public List<Child> Childs
		{
			get; set;
		}
	}
	public class Child
	{
		public string Name { get; set; }
	}
}
