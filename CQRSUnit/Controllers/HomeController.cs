using CQRSUnit.CommandHandler.Impl;
using CQRSUnit.Commands;
using CQRSUnit.Domain;
using CQRSUnit.Message;
using CQRSUnit.Message.Impl;
using CQRSUnit.Services;
using CQRSUnit.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQRSUnit.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";
			return View();
		}
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";
			return View();
		}
		//public ActionResult InsertUser()
		//{
		//	UserEntity entity = new UserEntity()
		//	{
		//		Id = Guid.NewGuid().ToString(),
		//		Name = "lisi",
		//		Password = "8975456689754"
		//	};
		//	IUserService<UserEntity> service = new UserService<UserEntity>();
		//	service.Handler.Events += Handler_Events;
		//	service.Add(entity);
		//	service.Commit();
		//	return View();
		//}
		private void Handler_Events(UserEntity entity)
		{
			throw new NotImplementedException();
		}
		public ActionResult FindUser()
		{
			IQuerableService<UserEntity> querableService = new QurableService<UserEntity>();
			var list = querableService.Find(x => x.Name == "zhangsan").ToList();
			ViewBag.Name = list[0].Name;
			return View();
		}
		public ActionResult AddDataItem()
		{
			ICommandBus commandBus = new CommandBus(new DefaultCommandHadlerFactory());
			commandBus.Send<CreateItemCommand>(new CreateItemCommand(aggregateId:new Guid(),title:"Hi",
				version:1,description:"hiiii",from:DateTime.Now,to:DateTime.Now)
			{
			});
			return View();
		}
	}
}