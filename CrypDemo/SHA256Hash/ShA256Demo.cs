using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrypDemo
{
	public class ShA256Demo
	{
		/// <summary>
		/// 生成的是256位hash，32字节
		/// 输入的报文不超过 2^64 bit
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string SHA256Encrypt(string input)
		{
			SHA256 sha256 = new SHA256Managed();
			byte[] bytes = sha256.ComputeHash(Encoding.Default.GetBytes(input));
			sha256.Clear();
			StringBuilder sb = new StringBuilder();
			foreach (var item in bytes)
			{
				sb.Append(item.ToString("x2"));
			}
			return sb.ToString();
		}
	}
}
