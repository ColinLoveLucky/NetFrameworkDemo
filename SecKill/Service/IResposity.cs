using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SecKill.Service
{
	public interface IResposity<T> where T : class, new()
	{
		IUnitOfWork CurrentUnitOfWork
		{
			get;set;
		}
		IQueryable<T> Find(Expression<Func<T, bool>> express);

		IQueryable<T> FindAll();
		
		void Add(T entity);

	}
}