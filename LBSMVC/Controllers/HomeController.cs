using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;
using System.Text;
using Newtonsoft.Json;

namespace LBSMVC.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			RedisHelper redis = new RedisHelper();
			ViewBag.Message = "Your application description page. name" + redis.Get<string>("token");

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(FormCollection contex)
		{
			RedisHelper redis = new RedisHelper();
			//redis.Set("name", "111", 1000);
			redis.Set("token", contex[0], 5000);
			return View();
		}
	}

	public class RedisHelper
	{
		/// <summary>
		/// 连接字符串
		/// </summary>
		private static readonly string ConnectionString= "127.0.0.1:6990,allowAdmin=true";
		/// <summary>
		/// 锁
		/// </summary>
		private readonly object _lock = new object();
		/// <summary>
		/// 连接对象
		/// </summary>
		private volatile IConnectionMultiplexer _connection;
		/// <summary>
		/// 数据库
		/// </summary>
		private IDatabase _db;
		public RedisHelper()
		{
			_connection = ConnectionMultiplexer.Connect(ConnectionString);
			
			_db = GetDatabase();
		}
		/// <summary>
		/// 获取连接
		/// </summary>
		/// <returns></returns>
		protected IConnectionMultiplexer GetConnection()
		{
			if (_connection != null && _connection.IsConnected)
			{
				return _connection;
			}
			lock (_lock)
			{
				if (_connection != null && _connection.IsConnected)
				{
					return _connection;
				}

				if (_connection != null)
				{
					_connection.Dispose();
				}
				_connection = ConnectionMultiplexer.Connect(ConnectionString);
			}

			return _connection;
		}
		/// <summary>
		/// 获取数据库
		/// </summary>
		/// <param name="db"></param>
		/// <returns></returns>
		public IDatabase GetDatabase(int? db = null)
		{
			return GetConnection().GetDatabase(db ?? -1);
		}
		/// <summary>
		/// 设置
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="data">值</param>
		/// <param name="cacheTime">时间</param>
		public virtual void Set(string key, object data, int cacheTime)
		{
			if (data == null)
			{
				return;
			}
			var entryBytes = Serialize(data);
			var expiresIn = TimeSpan.FromMinutes(cacheTime);

			_db.StringSet(key, entryBytes, expiresIn);
		}
		/// <summary>
		/// 根据键获取值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual T Get<T>(string key)
		{

			var rValue = _db.StringGet(key);
			if (!rValue.HasValue)
			{
				return default(T);
			}

			var result = Deserialize<T>(rValue);

			return result;
		}
		/// <summary>
		/// 反序列化
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializedObject"></param>
		/// <returns></returns>
		protected virtual T Deserialize<T>(byte[] serializedObject)
		{
			if (serializedObject == null)
			{
				return default(T);
			}
			var json = Encoding.UTF8.GetString(serializedObject);
			return JsonConvert.DeserializeObject<T>(json);
		}
		/// <summary>
		/// 判断是否已经设置
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual bool IsSet(string key)
		{
			return _db.KeyExists(key);
		}
		/// <summary>
		/// 序列化
		/// </summary>
		/// <param name="data"></param>
		/// <returns>byte[]</returns>
		private byte[] Serialize(object data)
		{
			var json = JsonConvert.SerializeObject(data);
			return Encoding.UTF8.GetBytes(json);
		}
	}
}