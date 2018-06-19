using CQRSUnit.Domain;
using CQRSUnit.Domain.Impl;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CQRSUnit.Services
{
	public interface IService<TEntity>
	{

		EventHandlerImp<TEntity> Handler { get; }
		IUnitOfWork CurrentUnitOfWork { get; set; }
		IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> express);
		void Add(TEntity entity);
		void Commit();
	}
}