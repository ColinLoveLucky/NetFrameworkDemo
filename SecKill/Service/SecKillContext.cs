using SecKill.domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SecKill.Service
{
	public class SecKillContext : DbContext
	{
		public SecKillContext() : base("DbSecKill")
		{
		}
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Stock> Stocks { get; set; }
		public DbSet<SecKillToken> SecKillTokens { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>().Property(x => x.Phone).HasMaxLength(11);
			modelBuilder.Entity<Product>().Property(x => x.ProdcutName).HasMaxLength(100);
			modelBuilder.Entity<Product>().Property(x => x.ProductType).HasMaxLength(10);
			modelBuilder.Entity<Stock>().Property(x => x.ProductName).HasMaxLength(100);
			modelBuilder.Entity<SecKillToken>().Property(x => x.Token).HasMaxLength(100);
			modelBuilder.Entity<SecKillToken>().Property(x => x.ProductName).HasMaxLength(100);
		}
	}
}