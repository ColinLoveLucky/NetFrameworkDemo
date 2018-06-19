using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypDemo.base64
{
	/// <summary>
	/// Base64 每24位 转换为32 位
	/// 不够补充为0
	/// 然后在转为字符串展示
	/// </summary>
	public class Base64Demo
	{
		public static void strBase64()
		{
			byte[] bytes = Encoding.Default.GetBytes("才");
			string base64Code = Convert.ToBase64String(bytes);
			Console.WriteLine(base64Code);
		}

		public void Base64Url()
		{
			byte[] bytes = Encoding.Default.GetBytes("A%2fA");
			string base64Code = Convert.ToBase64String(bytes);
			Console.WriteLine(base64Code);
		}

		public void DesBase64()
		{
			byte[] output = Convert.FromBase64String("ssU=");
			string originalStr = Encoding.Default.GetString(output);
			Console.WriteLine(originalStr);
		}

		public void EncImg()
		{
			string path = @"../../base64/1.png";
			FileInfo file = new FileInfo(path);
			var filePath = file.FullName;
			MemoryStream m = new MemoryStream();
			Bitmap bp = new Bitmap(filePath);
			bp.Save(m, ImageFormat.Png);
			byte[] bytes = m.GetBuffer();
			string base64String = Convert.ToBase64String(bytes);
		}
	}
}
