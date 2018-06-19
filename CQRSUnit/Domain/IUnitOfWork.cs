using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CQRSUnit.Domain
{
	public interface IUnitOfWork
	{
		DbContext CurrentDbContext { get; set; }
		void Commit();
	}
}