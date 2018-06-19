using CQRSUnit.Domain;
using CQRSUnit.Domain.Impl;
using CQRSUnit.Infranstruture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CQRSUnit.Services.Impl
{
	public class DbService<TEntity> : IService<TEntity> where TEntity : class, new()
	{
		private IUnitOfWork _unitOfWork;
		public IUnitOfWork CurrentUnitOfWork
		{
			get
			{
				return _unitOfWork;
			}
			set
			{
				_unitOfWork = value;
			}
		}

		public EventHandlerImp<TEntity> Handler
		{
			get
			{
				return new EventHandlerImp<TEntity>();
			}
		}

		public DbService()
		{
			_unitOfWork = new UnitOfWork();
		}
		public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> express)
		{
			return CurrentUnitOfWork.CurrentDbContext.Set<TEntity>().Where(express);
		}
		public void Add(TEntity entity)
		{
			CurrentUnitOfWork.CurrentDbContext.Set<TEntity>().Add(entity);
		}
		public void Commit()
		{
			CurrentUnitOfWork.Commit();
		}
	}
}