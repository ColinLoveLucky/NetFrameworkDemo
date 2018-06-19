using SecKill.domain;
using SecKill.Service;
using SecKill.Service.Impl;
using SecKill.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecKill
{
	public class ProductController : Controller
	{
		// GET: Product
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public string Add([Bind(Include = "ProductType,ProdcutName,Price")] Product product, [Bind(Include = "SurplusNum,StartTime,EndTime")] Stock stock)
		{
			RedisHelper redisHelpher = new RedisHelper();
			IUnitOfWork unitOfWork = new UnitOfWork();
			var productResposity = new Resposity<Product>(unitOfWork);
			product.Id = Guid.NewGuid().ToString();
			product.CreateTime = DateTime.Now;
			product.UpdateTime = DateTime.Now;
			productResposity.Add(product);
			var stockResposity = new Resposity<Stock>(unitOfWork);
			stock.Id = Guid.NewGuid().ToString();
			stock.ProductName = product.ProdcutName;
			stock.SecKillPrice = product.Price;
			stock.CreateTime = DateTime.Now;
			stock.UpdateTime = DateTime.Now;
			stockResposity.Add(stock);
			var tokenResposity = new Resposity<SecKillToken>(unitOfWork);
			SecKillToken seckKillToken = null;
			string key = product.ProdcutName;
			for (var i = 0; i < stock.SurplusNum; i++)
			{
				seckKillToken = new SecKillToken();
				seckKillToken.Id = Guid.NewGuid().ToString();
				seckKillToken.Token = Guid.NewGuid().ToString();
				seckKillToken.ExpireStartTime = stock.StartTime;
				seckKillToken.ExpireEndTime = stock.EndTime;
				seckKillToken.ProductName = stock.ProductName;
				seckKillToken.CreateTime = DateTime.Now;
				seckKillToken.UpdateTime = DateTime.Now;
				seckKillToken.IsBuy = 0;
				tokenResposity.Add(seckKillToken);
				redisHelpher.Enqueue(key, seckKillToken.Token);
			}
			unitOfWork.Commit();
			string result = "Add Success";
			return result;
		}
	}
}