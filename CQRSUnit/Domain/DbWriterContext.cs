using CQRSUnit.Infranstruture;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CQRSUnit.Domain
{
	public class DbWriterContext:DbContext, IDbContext
	{

		public DbWriterContext() : base("DbWriter")
		{
		}
		public DbSet<DataItemEntity> DataItems { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DataItemEntity>().Property(x => x.Title).HasMaxLength(100);
			modelBuilder.Entity<DataItemEntity>().Property(x => x.Description).HasMaxLength(32);
		}
	}
}