using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrypDemo.HMACAlg
{
	/// <summary>
	/// HMAC 是多添加了加密key在hash算法的基础上
	/// 生成的hash签名的位数跟具体的hash算法一致
	/// 1. 客户端发出登录请求（假设是浏览器的GET请求）
　　//2. 服务器返回一个随机值，并在会话中记录这个随机值
　　//3. 客户端将该随机值作为密钥，用户密码进行hmac运算，然后提交给服务器
　　//4. 服务器读取用户数据库中的用户密码和步骤2中发送的随机值做与客户端一样的hmac运算，然后与用户发送的结果比较，如果结果一致则验证用户合法
	/// </summary>
	public class HMacAlog
	{
		public static string GetHashMacMd5(string input, string key)
		{
			HMAC hmacMd5 = new HMACMD5();
			hmacMd5.Key = Encoding.Default.GetBytes(key);
			var bytes = hmacMd5.ComputeHash(Encoding.Default.GetBytes(input));
			Console.WriteLine("Hash Value:{0}", BitConverter.ToString(bytes.Take(hmacMd5.HashSize >> 3).ToArray()));
			Console.WriteLine("data Value:{0}", BitConverter.ToString(bytes.Skip(hmacMd5.HashSize >> 3).ToArray()));
			StringBuilder sb = new StringBuilder();
			foreach (var item in bytes)
			{
				sb.Append(item.ToString("x2"));
			}
			return sb.ToString();
		}
		public static string GetSha256Mac(string input, string key)
		{
			HMAC hmacMd5 = new HMACSHA256();
			hmacMd5.Key = Encoding.Default.GetBytes(key);
			var bytes = hmacMd5.ComputeHash(Encoding.Default.GetBytes(input));
			StringBuilder sb = new StringBuilder();
			foreach (var item in bytes)
			{
				sb.Append(item.ToString("x2"));
			}
			return sb.ToString();
		}
		public static byte[] SignData(byte[] Key, byte[] data, HMAC alg)
		{
			alg.Key = Key;
			var hash = alg.ComputeHash(data);
			var result= hash.Concat(data).ToArray();
			return result;
		}
		public static bool VerityData(byte[] key, byte[] data, HMAC alg)
		{
			var receiveHash = data.Take(alg.HashSize >> 3);
			var dataContent = data.Skip(alg.HashSize >> 3).ToArray();
			alg.Key = key;
			var computeHash = alg.ComputeHash(dataContent);
			return receiveHash.SequenceEqual(computeHash);
		}
		public static void PrintData(byte[] data, HMAC alg)
		{
			Console.WriteLine("哈希值: {0}\n文件值: {1}",
				BitConverter.ToString(data.Take(alg.HashSize >> 3).ToArray()),
				BitConverter.ToString(data.Skip(alg.HashSize >> 3).ToArray()));
		}
		public static void VerityHMacMd5()
		{
			HMAC hmac = new HMACMD5();
			var data = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
			var key = new byte[100];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(key);
			}	
			var signedData = SignData(key, data, hmac);
			PrintData(signedData, hmac);
			Console.WriteLine(VerityData(key, signedData, hmac) ? "数据正确" : "数据已被修改");
			//故意修改数据（将源数据的5修改成4）
			signedData[(hmac.HashSize >> 3) + 4] = 4;
			//输出数据
			PrintData(signedData, hmac);
			//认证
			Console.WriteLine(VerityData(key, signedData, hmac) ? "数据正确" : "数据已被修改");
		}
	}
}
