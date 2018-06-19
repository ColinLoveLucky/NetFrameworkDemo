using EfResposity.Model;
using EfResposity.UnitOfWorkInfranstrure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EfResposity
{
	class Program
	{
		static void Main(string[] args)
		{
			//IUnitOfWork unitOfWork = new UnitOfWork()
			//{
			//	CurrentDbContext = new DbResposityContext()
			//};
			//UserService service = new UserService(new UserResposity(unitOfWork), unitOfWork);
			//var random = new Random();
			//var md5 = MD5.Create();
			//var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes("123456"));
			//StringBuilder sb = new StringBuilder();
			//foreach (var item in bytes)
			//{
			//	sb.Append(item.ToString("x2"));
			//}
			//var user = new Ef_User()
			//{
			//	UserName = "Colin" + random.Next(1, 100),
			//	Password = sb.ToString()
			//};
			//service.AddUser(user);

			//var resposity = new EfResposity<DbContext, Ef_User>(new DbResposityContext());
			//(resposity.GetAll().Where(x => x.UserName.Contains("x")) ?? null).ForEachAsync(x =>
			//{
			//	Console.WriteLine($"UserName:{x.UserName}");
			//}).Wait();

			//return fn;
		}
	}
}
