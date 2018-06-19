using CQRSUnit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.Services
{
	public interface ICommandService<TEntity> where TEntity : class, new()
	{
		IUnitOfWork CurrentUnitOfWork { get; }
		void Add(TEntity entity);
		void Commit();

	}
}