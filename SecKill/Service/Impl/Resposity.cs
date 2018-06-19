using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SecKill.Service.Impl
{
	public class Resposity<T> : IResposity<T> where T : class, new()
	{
		private IUnitOfWork _unitOfWork;
	
		public Resposity(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IUnitOfWork CurrentUnitOfWork {

			get
			{
				return _unitOfWork;
			}
			set
			{
				_unitOfWork = value;
			}
		}

		public void Add(T entity)
		{
			_unitOfWork.CurrentDbContext.Set<T>().Add(entity);
		}
	
		public IQueryable<T> Find(Expression<Func<T, bool>> express)
		{
			return _unitOfWork.CurrentDbContext.Set<T>().Where(express.Compile()).AsQueryable();
		}

		public IQueryable<T> FindAll()
		{
			return _unitOfWork.CurrentDbContext.Set<T>().AsQueryable();
		}
	}
}