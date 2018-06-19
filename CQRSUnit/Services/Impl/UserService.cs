using CQRSUnit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using CQRSUnit.Domain.Impl;

namespace CQRSUnit.Services.Impl
{
	public class UserService<IEntity> : IUserService<IEntity> where IEntity : class, new()
	{
		private EventHandlerImp<IEntity> _handler;
		private DbService<IEntity> _dbService;
		public IUnitOfWork CurrentUnitOfWork { get
			{
				return _dbService.CurrentUnitOfWork;
			}
			set
			{
				_dbService.CurrentUnitOfWork = value;
			}
		}
		public EventHandlerImp<IEntity> Handler
		{
			get
			{
				return _dbService.Handler;
			}
		}
		public UserService()
		{
			_dbService = new DbService<IEntity>();
			_handler = new EventHandlerImp<IEntity>();
		}
		public IQueryable<IEntity> Find(Expression<Func<IEntity, bool>> express)
		{
			return _dbService.Find(express);
		}
		public void Add(IEntity entity)
		{
			_dbService.Add(entity);
			//return _dbService.Add(entity);
		}
		public void Commit()
		{
			_dbService.Commit();
		}
	}
}