using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;

namespace CQRSUnit.Domain.Impl
{
	public class UnitOfWork : IUnitOfWork
	{
		private DbContext _dbContext;
		public UnitOfWork() {
			_dbContext = new DbReaderContext();
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
			try
			{
				using (TransactionScope transactionScope = new TransactionScope())
				{
					_dbContext.SaveChanges();

					transactionScope.Complete();
				}
			}
			catch
			{
			}
			finally
			{
			}
		}
	}
}