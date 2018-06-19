using CQRSUnit.Infranstruture;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CQRSUnit.Domain
{
	public class DbReaderContext : DbContext, IDbContext
	{
		public DbReaderContext() : base("DbReader")
		{
		}
		public DbSet<UserEntity> Users { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserEntity>().Property(x => x.Name).HasMaxLength(100);
			modelBuilder.Entity<UserEntity>().Property(x => x.Password).HasMaxLength(32);
		}
	}
}