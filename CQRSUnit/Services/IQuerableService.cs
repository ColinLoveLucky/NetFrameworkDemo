using CQRSUnit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CQRSUnit.Services
{
	public interface IQuerableService<TEntity> where TEntity:class,new ()
	{
		IUnitOfWork CurrentUnitOfWork { get;}

		IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> express);
	}
}