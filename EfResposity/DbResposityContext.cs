using EfResposity.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfResposity
{
	public class DbResposityContext : DbContext
	{
		public DbResposityContext() :base("DbResposity")
		{
		}

	 
		public DbSet<Ef_User> Ef_Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Ef_User>().Property(x => x.UserName).HasMaxLength(20);
			modelBuilder.Entity<Ef_User>().Property(x => x.Password).HasMaxLength(32);
		
		}
	}
}
