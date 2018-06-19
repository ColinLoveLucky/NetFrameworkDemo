using MVCDemo.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCDemo.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base("name=demo")
        {
            Database.SetInitializer<DataBaseContext>(null);
            //Database.SetInitializer<DataBaseContext>(new MigrateDatabaseToLatestVersion<DataBaseContext, Configuration>());
        }
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public System.Data.Entity.DbSet<MVCDemo.Models.Instructor> Instructors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Instructor>().MapToStoredProcedures();
        }
    }
}