using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CQRSUnit.Domain;

namespace CQRSUnit.Services.Impl
{
	public class QurableService<TEntity> : IQuerableService<TEntity> where TEntity : class, new()
	{
		private IUnitOfWork _unitOfWork;
		public IUnitOfWork CurrentUnitOfWork
		{
			get
			{
				if (_unitOfWork == null)
					_unitOfWork = new UnitOfManager().CurrentUnitOfWork;
				return _unitOfWork;
			}
		}
		public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> express)
		{
			return CurrentUnitOfWork.CurrentDbContext.Set<TEntity>().Where(express.Compile()).AsQueryable();
		}
	}
}