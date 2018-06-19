using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SecKill.Service
{
	public interface IUnitOfWork
	{
		DbContext CurrentDbContext
		{
			get; set;
		}

		void Commit();
	}
}