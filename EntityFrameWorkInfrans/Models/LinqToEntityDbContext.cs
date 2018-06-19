using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EntityFrameWorkInfrans.Models.Mapping;

namespace EntityFrameWorkInfrans.Models
{
    public partial class LinqToEntityDbContext : DbContext
    {
        static LinqToEntityDbContext()
        {
            Database.SetInitializer<LinqToEntityDbContext>(null);
        }

        public LinqToEntityDbContext()
            : base("Name=LinqToEntityDbContext")
        {
        }

        public DbSet<C__TransactionHistory> C__TransactionHistory { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new C__TransactionHistoryMap());
            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new ProductMap());
        }
    }
}
