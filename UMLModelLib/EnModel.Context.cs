﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace UMLModelLib
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LinqToEntityDbEntities3 : DbContext
    {
        public LinqToEntityDbEntities3()
            : base("name=LinqToEntityDbEntities3")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<blog> blogs { get; set; }
        public virtual DbSet<course> courses { get; set; }
        public virtual DbSet<department> departments { get; set; }
        public virtual DbSet<employee> employees { get; set; }
        public virtual DbSet<instructor> instructors { get; set; }
        public virtual DbSet<Lodging> Lodgings { get; set; }
        public virtual DbSet<manager> managers { get; set; }
        public virtual DbSet<person> people { get; set; }
        public virtual DbSet<post> posts { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<product_category> product_category { get; set; }
        public virtual DbSet<university> universities { get; set; }
        public virtual DbSet<ChinaPerson> ChinaPersons { get; set; }
    
        public virtual int DeleteBlogs(Nullable<int> blogId)
        {
            var blogIdParameter = blogId.HasValue ?
                new ObjectParameter("blogId", blogId) :
                new ObjectParameter("blogId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteBlogs", blogIdParameter);
        }
    
        public virtual ObjectResult<GetAandB_Result> GetAandB()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAandB_Result>("GetAandB");
        }
    
        public virtual ObjectResult<GetAllBlogsAndPosts_Result> GetAllBlogsAndPosts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllBlogsAndPosts_Result>("GetAllBlogsAndPosts");
        }
    
        public virtual ObjectResult<GetAllBologsandCourse_Result> GetAllBologsandCourse()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllBologsandCourse_Result>("GetAllBologsandCourse");
        }
    
        public virtual ObjectResult<Nullable<int>> InsertBlogs(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("InsertBlogs", nameParameter);
        }
    
        public virtual ObjectResult<SQuery_Result> SQuery(ObjectParameter count)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SQuery_Result>("SQuery", count);
        }
    
        public virtual int updateBlogs(Nullable<int> blogId, string name)
        {
            var blogIdParameter = blogId.HasValue ?
                new ObjectParameter("blogId", blogId) :
                new ObjectParameter("blogId", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("updateBlogs", blogIdParameter, nameParameter);
        }
    
        [DbFunction("LinqToEntityDbEntities3", "GetCourse")]
        public virtual IQueryable<GetCourse_Result> GetCourse(Nullable<int> courseID)
        {
            var courseIDParameter = courseID.HasValue ?
                new ObjectParameter("CourseID", courseID) :
                new ObjectParameter("CourseID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<GetCourse_Result>("[LinqToEntityDbEntities3].[GetCourse](@CourseID)", courseIDParameter);
        }
    }
}
