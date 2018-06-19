using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrypDemo.SymmetricEncrytion
{
	/// <summary>
	/// 对称加密，AES，安全性高，效率高
	/// DES安全级别低，现在以可以破解
	/// Rc2与AES是可变密钥
	/// 为什么要添加向量呢，设计到CipherMode的四种模式
	/// 防止同样的数据，加密成同样的文本，以免被破解，
	/// 用了向量，可以不用随时变密钥也可以，针对一段文本中相同的数据，加密成不同的内容
	/// 对称加密，算法用到置换，移位，多次迭代，替换等算法
	/// </summary>
	public class DesCryption
	{
		private static string _desKey = "abcdefgh";
		private static string _desIv = "igklmnop";
		private static string _tripleKey = "abcdefghijklmnopqrstuvws";
		private static string _tripleIv = "12345678";
		//private static string _rc2Key = "abcdef1234561234";
		private static string _rc2Key = "abcdef1234561234";
		private static string _rc2Iv = "abcdef12";

		private static string _riKey = "abcdef1234561234";
		private static string _riIv = "abcdef12abcdef12";

		/// <summary>
		/// DES使用的密钥key为8字节，初始向量IV也是8字节。
		/// 以8字节一块进行加密
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static byte[] EncryptByDESCryptoServiceProvider(string val)
		{
			var base64Key = Encoding.Default.GetBytes(_desKey);
			var base64IV = Encoding.Default.GetBytes(_desIv);
			DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
			provider.Mode = CipherMode.ECB;
			provider.Key = base64Key;
			provider.IV = base64IV;
			byte[] buffer3 = Encoding.Default.GetBytes(val);
			MemoryStream mstream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mstream, provider.CreateEncryptor(provider.Key, provider.IV), CryptoStreamMode.Write);
			cStream.Write(buffer3, 0, buffer3.Length);
			cStream.FlushFinalBlock();
			cStream.Close();
			return mstream.ToArray();
		}
		public static string DecryptByDESCryptoServiceProvider(string encryData)
		{
			var data = EncryptByDESCryptoServiceProvider(encryData);
			var base64Key = Encoding.Default.GetBytes(_desKey);
			var base64IV = Encoding.Default.GetBytes(_desIv);
			DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
			provider.Mode = CipherMode.ECB;
			provider.Key = base64Key;
			provider.IV = base64IV;
			MemoryStream mstream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mstream, provider.CreateDecryptor(provider.Key, provider.IV), CryptoStreamMode.Write);
			cStream.Write(data, 0, data.Length);
			cStream.FlushFinalBlock();
			cStream.Close();
			return Encoding.Default.GetString(mstream.ToArray());
		}
		public static void EncryptAndDecryptByDESCryptoServiceProvider()
		{
			//加密
			string myID = "435-86-0985-021";
			DESCryptoServiceProvider key = new DESCryptoServiceProvider();
			MemoryStream ms = new MemoryStream();
			CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);
			StreamWriter sw = new StreamWriter(encStream);
			sw.WriteLine(myID);
			sw.Close();
			//获取加密后的字节
			byte[] buffer = ms.ToArray();
			//解密
			ms = new MemoryStream(buffer);
			encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);
			StreamReader sr = new StreamReader(encStream);
			//输出解密后的内容
			Console.WriteLine(sr.ReadLine());
			key.Clear();
			sr.Close();
			Console.ReadLine();
		}
		/// <summary>
		/// TripleDES使用24字节的key 192位，初始向量IV也是8字节。
		/// 以8字节一块进行加密
		/// </summary>
		/// <returns></returns>
		public static string EncryByTripleDESCryptoServiceProvider(string input)
		{
			var key = Encoding.Default.GetBytes(_tripleKey);
			var iv = Encoding.Default.GetBytes(_tripleIv);
			TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
			provider.Key = key;
			provider.IV = iv;
			provider.Mode = CipherMode.CBC;
			byte[] buffer = Encoding.Default.GetBytes(input);
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(provider.Key, provider.IV), CryptoStreamMode.Write);
			cStream.Write(buffer, 0, buffer.Length);
			cStream.FlushFinalBlock();
			cStream.Close();
			return Convert.ToBase64String(mStream.ToArray());
		}
		public static string DecrptByTripleDESCryptoServiceProvider(string input)
		{
			var data =Convert.FromBase64String( EncryByTripleDESCryptoServiceProvider(input));
			var key = Encoding.Default.GetBytes(_tripleKey);
			var iv = Encoding.Default.GetBytes(_tripleIv);
			TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
			provider.Mode = CipherMode.CBC;
			provider.Key = key;
			provider.IV = iv;
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(provider.Key, provider.IV), CryptoStreamMode.Write);
			cStream.Write(data, 0, data.Length);
			cStream.FlushFinalBlock();
			cStream.Close();
			return Encoding.Default.GetString(mStream.ToArray());
		}
		/// <summary>
		/// Rc2 与des算法类似
		/// 密钥的16位，128字节
		/// 向量位8字节
		/// 密钥是从48-128 位变长的，但是以8的倍数
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string EncryByRC2CryptoServiceProvider(string input)
		{
			RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
			//rc2.UseSalt = true;
			Console.WriteLine("Effective key size is {0} bits.", rc2.EffectiveKeySize);	
			rc2.Key = Encoding.Default.GetBytes(_rc2Key);
			rc2.IV =Encoding.Default.GetBytes( _rc2Iv);
			rc2.Mode = CipherMode.ECB;
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, rc2.CreateEncryptor(rc2.Key, rc2.IV), CryptoStreamMode.Write);
			byte[] bytes = Encoding.Default.GetBytes(input);
			cStream.Write(bytes, 0, bytes.Length);
			cStream.FlushFinalBlock();
			cStream.Close();
			return Convert.ToBase64String(mStream.ToArray());
		}
		public static string DecryptRc2(string input)
		{
			var data = Convert.FromBase64String(EncryByRC2CryptoServiceProvider(input));
			RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
			rc2.Key = Encoding.Default.GetBytes(_rc2Key); 
			rc2.IV = Encoding.Default.GetBytes(_rc2Iv);
			rc2.Mode = CipherMode.ECB;
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, rc2.CreateDecryptor(rc2.Key, rc2.IV), CryptoStreamMode.Write);
			cStream.Write(data, 0, data.Length);
			cStream.FlushFinalBlock();
			cStream.Close();
			return Encoding.Default.GetString(mStream.ToArray());
		}

		/// <summary>
		/// 密钥长度有3种选择：128位、192位及256位。 大于128 32 的倍数
		/// 向量16字节 128位
		/// AES（Advanced Encryption Standard）
		/// </summary>
		public static string EncryptByRijndaelManaged(string input)
		{
			var key = Encoding.Default.GetBytes(_riKey);
			var iv = Encoding.Default.GetBytes(_riIv);
			RijndaelManaged provider = new RijndaelManaged();
			provider.Key = key;
			provider.IV = iv;
			provider.Mode = CipherMode.CBC;
			byte[] buffer = Encoding.Default.GetBytes(input);
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(provider.Key, provider.IV), CryptoStreamMode.Write);
			cStream.Write(buffer, 0, buffer.Length);
			cStream.FlushFinalBlock();
			cStream.Close();
			return Convert.ToBase64String(mStream.ToArray());
		}
		public static string DecryptByRijndaelManaged(string input)
		{
			var data = Convert.FromBase64String(EncryptByRijndaelManaged(input));
			RijndaelManaged provider = new RijndaelManaged();
			provider.Key = Encoding.Default.GetBytes(_riKey);
			provider.IV = Encoding.Default.GetBytes(_riIv);
			provider.Mode = CipherMode.CBC;
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(provider.Key, provider.IV), CryptoStreamMode.Write);
			cStream.Write(data, 0, data.Length);
			cStream.FlushFinalBlock();
			cStream.Close();
			return Encoding.Default.GetString(mStream.ToArray());
		}
	}
}
