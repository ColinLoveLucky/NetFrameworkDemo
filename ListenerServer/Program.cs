using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ListenerServer
{
	class Program
	{
		private static bool _isRunning = false;
		private static string _serverIp = "127.0.0.1";
		private static int _port = 9787;
		static void Main(string[] args)
		{
			if (_isRunning)
				return;
			//创建TcpListener
			var serverListener = new TcpListener(IPAddress.Parse(_serverIp), _port);
			//开始监听
			serverListener.Start(10);
			_isRunning = true;
			//输出服务器状态
			Console.WriteLine("Sever is running at http://{0}:{1}/.", _serverIp, _port);
			while (_isRunning)
			{
				//获取客户端连接
				TcpClient acceptClient = serverListener.AcceptTcpClient();
				//获取请求报文
				NetworkStream netstream = acceptClient.GetStream();
				//解析请求报文
				byte[] bytes = new byte[4000];
				int length = netstream.Read(bytes, 0, bytes.Length);
				string requestString = Encoding.UTF8.GetString(bytes, 0, length);
				Console.WriteLine("请求的字符串流\r\n{0}",requestString);
				//以下为响应报文(略)

				netstream.Close();
				acceptClient.Close();
				//netstream.Write(bytes, 0, bytes.Length);
			}
		}
	}
}
