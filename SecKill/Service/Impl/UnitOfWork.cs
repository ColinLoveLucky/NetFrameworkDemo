using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;

namespace SecKill.Service.Impl
{
	public class UnitOfWork:IUnitOfWork
	{
		private DbContext _dbContext;
		public UnitOfWork()
		{
			_dbContext = new SecKillContext();
		}
		public UnitOfWork(DbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public DbContext CurrentDbContext {
			get
			{
				return _dbContext;
			}
			set
			{
				_dbContext = value;
			}
		}

		public void Commit()
		{
			using (TransactionScope scope = new TransactionScope())
			{
				_dbContext.SaveChanges();
				scope.Complete();
			}
		}
	}
}