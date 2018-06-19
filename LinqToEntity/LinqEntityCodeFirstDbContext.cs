namespace LinqToEntity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Infrastructure.Interception;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Text.RegularExpressions;
    using System.Data.Entity.Infrastructure.Pluralization;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Diagnostics.Contracts;

    public partial class LinqEntityCodeFirstDbContext : DbContext
    {

        public LinqEntityCodeFirstDbContext()
            : base("name=LinqEntityCodeFirstDbContext")
        {
            //DbUnexpectedValidationException
            // DbEntityValidationException
            //DbUpdateConcurrencyException
        }

        public virtual DbSet<Blogs> Blog { get; set; }
        public virtual DbSet<Posts> Post { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<Lodging> Lodging { get; set; }

        public virtual DbSet<Resort> Resort { get; set; }

        public virtual DbSet<OnLineCourse> OnLineCourse { get; set; }

        public virtual DbSet<ProductCategory> ProductCategory { get; set; }

        public virtual DbSet<Manager> Manager { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<Person> Person { get; set; }

        public virtual DbSet<Department> Department { get; set; }

        public DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blogs>().Property(u => u.BlogId).HasColumnName("Id");

            // modelBuilder.Entity<Blogs>().HasMany(x => x.Posts).WithOptional(x => x.Blogs).HasForeignKey(x => x.Blogs).WillCascadeOnDelete(true);
            modelBuilder.Entity<Blogs>().HasMany(x => x.Posts).WithOptional(x => x.Blogs).Map(x => x.MapKey("Blog_Id"));

            //modelBuilder.Conventions.Add(new KeyAttributeConvention());

            modelBuilder.Types().Where(x => x.Name != "CourseDetails" && x.Name != "Lodging" && x.Name != "Resort")
              .Configure(c => c.ToTable(GetTableName(c.ClrType)));

            modelBuilder.Conventions.Add(new StringConvertion());

            modelBuilder.Conventions.AddAfter<StringConvertion>(new StringNameConvertion());

            modelBuilder.Conventions.Remove<StringNameConvertion>();

            modelBuilder.HasDefaultSchema("ModelOne");

            modelBuilder.Entity<Blogs>().MapToStoredProcedures(s =>
            {
                s.Insert(u => u.HasName("InsertBlogs").Parameter(b => b.Name, "Name").Result(i => i.BlogId, "BlogId")).
                Update(u => u.HasName("updateBlogs").Parameter(b => b.BlogId, "blogId").Parameter(b => b.Name, "Name")).
                Delete(u => u.HasName("DeleteBlogs").Parameter(b => b.BlogId, "blogId"));

            });

            //modelBuilder
            //        .Entity<OnLineCourse>()
            //        .Property(t => t.Name)
            //        .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));


            //modelBuilder.Entity<OnLineCourse>().Map(m =>
            //{
            //    m.MapInheritedProperties();
            //    m.ToTable("OnlineCourse");

            //});

            modelBuilder.Entity<Instructor>()
                    .HasMany(t => t.Course)
                    .WithMany(t => t.Instructors)
                    .Map(m =>
                    {
                        m.MapLeftKey("CourseID");
                        m.MapRightKey("InstructorID");
                    });


            // modelBuilder.Types().Configure(c => c.ToTable(c.ClrType.Name));

            //TPT
            //modelBuilder.Entity<Lodging>().ToTable("Lodging");
            //modelBuilder.Entity<Resort>().ToTable("Resort");
            //TPC
            //modelBuilder.Entity<Lodging>()
            //    .Property(c => c.LodgingId)
            //     .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //modelBuilder.Entity<Resort>().Map(m =>
            //{
            //    m.MapInheritedProperties();
            //    m.ToTable("Resort");
            //});


        }
        private string GetTableName(Type type)
        {
            // var pluralizationService = DbConfiguration.DependencyResolver.GetService<IPluralizationService>();

            //var result = pluralizationService.Pluralize(type.Name);

            var result = Regex.Replace(type.Name, ".[A-Z]", m => m.Value[0] + "_" + m.Value[1]);

            return result.ToLower();
        }

        public class KeyConvention : Convention
        {
            public KeyConvention()
            {
                this.Properties<DateTime>().
                    Where(x => x.Name == "StartTime").Configure(p => p.HasColumnName("StartTime"));
            }
        }

        public class StringConvertion : Convention
        {
            public StringConvertion()
            {
                this.Properties<string>().Configure(c => c.HasMaxLength(250));
            }
        }

        public class StringNameConvertion : Convention
        {
            public StringNameConvertion()
            {
                this.Properties<string>().Where(x => x.Name == "Name").Configure(c => c.HasMaxLength(200));
            }
        }

    }
}
