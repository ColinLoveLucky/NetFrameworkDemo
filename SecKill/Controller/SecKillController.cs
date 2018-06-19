using SecKill.domain;
using SecKill.Model;
using SecKill.Models;
using SecKill.Service.Impl;
using SecKill.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SecKill
{
	public class SecKillController : Controller
	{
		private RedisHelper _redisHelpher;
		public SecKillController()
		{
			_redisHelpher = new RedisHelper();
		}
		// GET: SecKill
		public ActionResult Index()
		{
			var resposity = new Resposity<Stock>(new UnitOfWork());
			var stocks = resposity.FindAll().ToList();
			ViewBag.Stock = stocks;
			return View();
		}
		public string GenerateUrl()
		{
			return "/SecKill/StartKill";
		}
		public JsonResult StartKill()
		{
			var result = GenerateUsr();
			if (result == "Error")
			{
				MessageOutPut message = new MessageOutPut()
				{
					Code = "200",
					Data = "Error"
				};
				return Json(message);
			}
			else
			{
				MessageOutPut message = new MessageOutPut()
				{
					Code = "200",
					Data = "Success"
				};
				return Json(message);
			}
		}
		private void GetStartKillUsr()
		{
			//for (var i = 0; i < 10000; i++)
			//{
			Task[] taskArray= new Task[10];
			for (var i=0;i<taskArray.Length;i++)
			{
				taskArray[i] = new Task(() => GenerateUsr());
				taskArray[i].Start();
			}
			Task.WaitAll(taskArray);
			var result = "Hello";
		}
		private string GenerateUsr()
		{
			string key = "XPhone";
			var result = _redisHelpher.DeQueue<string>(key);
			
			if (result != null)
			{
				string phoneNum = "158214019" +
					new Random().Next(0, 100000).ToString() + new Random().Next(0, 100000).ToString();
				PhoneToken token = new PhoneToken()
				{
					Toke = result,
					Phone = phoneNum
				};
				_redisHelpher.Set(phoneNum, token, 5000);
				return "Success";
			}
			else
			{
				return "Error";
			}
		}
		public ActionResult Pay()
		{
			return View();
		}
	}
}