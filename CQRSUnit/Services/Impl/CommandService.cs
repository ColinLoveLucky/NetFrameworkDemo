using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRSUnit.Domain;

namespace CQRSUnit.Services.Impl
{
	public class CommandService<TEntity> : ICommandService<TEntity> where TEntity : class, new()
	{
		private IUnitOfWork _unitOfWork;
		public IUnitOfWork CurrentUnitOfWork
		{
			get
			{
				if (_unitOfWork == null)
				{
					_unitOfWork = new UnitOfManager().CurrentUnitOfWork;
					_unitOfWork.CurrentDbContext = new DbWriterContext();
				}
				return _unitOfWork;
			}
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