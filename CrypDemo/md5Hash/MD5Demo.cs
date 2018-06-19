using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrypDemo.md5
{
	public class MD5Demo
	{
		public static string GetMd5HashFromFile(string fileName)
		{
			using (FileStream file = new FileStream(fileName, FileMode.Open))
			{
				MD5 md5 = new MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(file);
				StringBuilder sb = new StringBuilder();
				foreach (var item in retVal)
				{
					sb.Append(item.ToString("x2"));
				}
				return sb.ToString();
			}
		}

		/// <summary>
		/// MD5 生成16字节的摘要，128bit
		/// 转换为X（16进制）2保留2位每次 
		/// 所以生成为32 的字符串
		/// 如果想更加安全针对每个用户添加salt 加盐(md5(userName+salt))
		/// 解密的时候取出盐来比较哈，盐存放的位置，在数据库里面哈
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string GetMd5Hash(string input)
		{
			MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
			byte[] data = md5Hash.ComputeHash(Encoding.Default.GetBytes(input));
			StringBuilder sb = new StringBuilder();
			foreach (var item in data)
			{
				sb.Append(item.ToString("x2"));
			}
			return sb.ToString();
		}
	}
}
