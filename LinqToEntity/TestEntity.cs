using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinqToEntity
{
    public class TestEntity
    {
        private static LinqEntityCodeFirstDbContext dbContext = new LinqEntityCodeFirstDbContext();

        private static readonly Func<ObjectContext, string, IEnumerable<Blogs>> BlogssComplie =
       CompiledQuery.Compile<ObjectContext, string, IEnumerable<Blogs>>((x, y) => dbContext.Blog.Where(z => z.Name.Contains(y)));

        public void InsertBlogs()
        {


            Blogs Blogs = new Blogs()
            {
                BlogId = 1,
                Name = "Colin Blogs",
               // Posts = new List<Posts>()
               //{
               //    new Posts()
               //    {
               //        PostId=1,
               //        Title="First Blogs",
               //        Content="I Wanna be Free"
               //    }
               //}
            };
            dbContext.Blog.Add(Blogs);

            dbContext.SaveChanges();
        }

        public void FindBlogs()
        {
            dbContext.Configuration.AutoDetectChangesEnabled = false;

            var product = dbContext.Blog.Find(1);

            Console.WriteLine("Name:{0}", product.Name);

            dbContext.Configuration.AutoDetectChangesEnabled = true;
        }

        public void FindQueryBlogs()
        {
            var query = from a in dbContext.Blog
                        select a;
            ObjectQuery oQuery = query as ObjectQuery;
            oQuery.EnablePlanCaching = false;
            oQuery.MergeOption = MergeOption.NoTracking;
        }
        public void EntityCommand()
        {
            using (EntityConnection conn =
                    new EntityConnection("name = LinqToEntityDbEntities"))
            {
                conn.Open();
                string esqlQuery =
                    @"SELECT name from Blogs";
                using (EntityCommand cmd = new EntityCommand(esqlQuery, conn))
                {
                    //EntityParameter param1 = new EntityParameter();
                    //param1.ParameterName = "name";
                    //param1.Value = "colin";
                    //cmd.Parameters.Add(param1);
                    using (EntityDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        while (rdr.Read())
                        {
                            Console.WriteLine(rdr["name"]);
                        }
                    }
                }
                conn.Close();
            }
        }
        public void SqlQuery()
        {
            var q = dbContext.Database.SqlQuery<Blogs>("select * from Blogss");

            foreach (var item in q)
            {
                Console.WriteLine("Name:{0}", item.Name);
            }
        }
        public void ExcuteStoreQuery()
        {
            ObjectResult<Blogs> Blogss = ((IObjectContextAdapter)dbContext).ObjectContext.ExecuteStoreQuery<Blogs>("select * from Blogss");

            foreach (var item in Blogss)
            {
                Console.WriteLine("Name:{0}", item.Name);
            }
        }
        public void GetBlogssForName()
        {
            var name = "Colin";
            var Blogss = BlogssComplie.Invoke(((IObjectContextAdapter)dbContext).ObjectContext, name).ToList();
            foreach (var item in Blogss)
            {
                Console.WriteLine("Name:{0}", item.Name);
            }
        }
        public void IncludeLoad()
        {
            var q = dbContext.Blog.Include("Posts");
            foreach (var item in q)
            {
                foreach (var Posts in item.Posts)
                {
                    Console.WriteLine(Posts.Title);
                }
            }


        }
        public void AutoDetectChanges()
        {
            try
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                var p = dbContext.Blog.Find(1);
                Console.WriteLine("Name:{0}", p.Name);
            }
            finally
            {
                dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void UseDataBaseNull()
        {
            //默认是false，会添加很多判断为空的代码，true不会直接比较
            dbContext.Configuration.UseDatabaseNullSemantics = true;
            decimal? price = 1;
            DateTime? datetime = DateTime.Now;

            var q = from a in dbContext.Product
                    where a.Price == price
                    select a;
            var r = q.ToList();
            foreach (var item in r)
            {
                Console.WriteLine("Name:{0}", item.productDate);
            }
        }
        public void DatabaseLog()
        {
            //var sw = new StreamWriter(@"d:\Data.log") { AutoFlush = true };
            //dbContext.Database.Log = s => {
            //    sw.Write(s);
            //};
            var q = dbContext.Blog;
            q.ToList();
        }
        public void GenerateViews()
        {
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            mappingCollection.GenerateViews(new List<EdmSchemaError>());

            //DbMappingViewCache

            // EntityTypeConfiguration

            // IDbDependencyResolver 
        }
        public void ExplictLoading()
        {
            dbContext.Configuration.LazyLoadingEnabled = false;

            var blog = dbContext.Blog.Find(1);

            if (!dbContext.Entry(blog).Collection(x => x.Posts).IsLoaded)
            {
                dbContext.Entry(blog).Collection(x => x.Posts).Load();

                Console.WriteLine("Posts is Loading");
            }

            foreach (var item in blog.Posts)
            {
                Console.WriteLine("Title:{0}", item.Title);
            }

            blog.Posts.Clear();

            Console.WriteLine("清空了blog下关联的Post");

            Console.WriteLine("重新加载Post");

            dbContext.Entry(blog).Collection(x => x.Posts).Load();

            Console.WriteLine("Blog 关联的Post Num:{0}", blog.Posts.Count);

            Console.WriteLine("怎么解决这种问题呢OverwriteChanges");

            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var objectSet = objectContext.CreateObjectSet<Posts>();
            objectSet.MergeOption = MergeOption.OverwriteChanges;
            objectSet.Load();
            Console.WriteLine("Blog 关联的Post Num:{0}", blog.Posts.Count);
        }
        public void TPH()
        {
        //    dbContext.Lodging.Add(new Lodging()
        //    {
        //        LodgingId = 1,
        //        Name = "Tokyo",
        //        Owner = "BeiHaiDao"
        //    });

        //    dbContext.Lodging.Add(new Resort()
        //    {
        //        Activities="Spring",
        //        Entertainment="Sky",
        //        LodgingId=2,
        //        Name="test"
        //    });
           // dbContext.SaveChanges();
        }
        public void TPT()
        {
        //    dbContext.Lodging.Add(new Lodging()
        //    {
        //        LodgingId = 1,
        //        Name = "Tokyo",
        //        Owner = "BeiHaiDao"
        //    });
        //    dbContext.Lodging.Add(new Resort()
        //    {
        //        Activities = "Spring",
        //        Entertainment = "Sky",
        //        LodgingId = 1,
        //        Name = "test"
        //    });
        //    dbContext.SaveChanges();
        }
        public void TPC()
        {
            //var q = dbContext.Resort.First();

            //Console.WriteLine("Activities:{0}", q.Name);
         
        }
        public void TestGenerateComputeOnLine()
        {
            dbContext.OnLineCourse.Add(new OnLineCourse()
            {
                Name = "test",
                CourseDetails = new CourseDetails()
                {
                    Days = "1",
                    Location ="1111",
                    Time=DateTime.Now
                }
            });
            dbContext.SaveChanges();
        }
        public void InsertDepartment()
        {
            dbContext.Department.Add(new Department()
            {
                Budget = 1,
                Name = DepartmentNames.Economics
            });

            dbContext.SaveChanges();
        }
        public void GetDepartment()
        {
            var q = dbContext.Department.ToList();

            foreach(var item in q)
            {
                Console.WriteLine("Name:{0}",item.Name);
            }
        }
        public void AddUniversity()
        {
            var universities = new List<University>() {
                new University { Name = "Graphic Design Institute", Location = DbGeography.FromText("POINT(-122.336106 47.605049)") },
                new University { Name = "School of Fine Art", Location = DbGeography.FromText("POINT(-122.335197 47.646711)") }
            }; 
            dbContext.Universities.AddRange(universities);
            dbContext.SaveChanges();
        }
        public void QueryUniversity()
        {
            var dbGeography = DbGeography.FromText("POINT(-122.335197 47.646711)");
            var q = dbContext.Universities.Where(x => SqlSpatialFunctions.Filter(x.Location, dbGeography) == true).ToList();
            foreach(var item in q)
            {
                Console.WriteLine("Location:{0}", item.Location);
            }
        }
        public void StoreQuery()
        {
            var parmas = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@count",
                Value = 0,
                Direction = ParameterDirection.Output
            };
            
            var result=dbContext.Database.SqlQuery<Blogs>("ModelOne.SQuery @count out", parmas).ToList();

            foreach(var item in result)
            {
                Console.WriteLine("Name:{0}", item.Name);
            }

            Console.WriteLine("Count:{0}", parmas.Value);
        }

        public void GetMultiFromStore()
        {
            var cmd = dbContext.Database.Connection.CreateCommand();

            cmd.CommandText = "ModelOne.GetAllBlogsAndPosts";

            dbContext.Database.Connection.Open();

            var reader = cmd.ExecuteReader();

            var blogs = ((IObjectContextAdapter)dbContext).ObjectContext.Translate<Blogs>(reader);

            foreach(var item in blogs)
            {
                Console.WriteLine("First Name:{0}", item.Name);
            }

            reader.NextResult();

            var blogs2 = ((IObjectContextAdapter)dbContext).ObjectContext.Translate<Blogs>(reader);

            foreach (var item in blogs2)
            {
                Console.WriteLine("First Name:{0}", item.Name);
            }

        }

        public class SingleLineFormatter : DatabaseLogFormatter
        {
            public SingleLineFormatter(DbContext ctx, Action<string> action)
                : base(ctx, action)
            {
            }
            public override void LogCommand<TResult>(System.Data.Common.DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
            {
                //Write(
                //    string.Format("DbContext '{0}' is Executing Command '{1}' '{2}'",
                //    Context.GetType().Name,
                //    command.CommandText.Replace(Environment.NewLine, ""),
                //    Environment.NewLine));

                base.LogCommand<TResult>(command, interceptionContext);
            }
            public override void LogResult<TResult>(System.Data.Common.DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
            {
                base.LogResult<TResult>(command, interceptionContext);
            }
        }

        public class NLogCommandInterceptor : IDbCommandInterceptor
        {
            // private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
            public void NonQueryExecuting(
                DbCommand command, DbCommandInterceptionContext<int> interceptionContext) 
            {
                LogIfNonAsync(command, interceptionContext);
            }
            public void NonQueryExecuted(
                DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
            {
                LogIfError(command, interceptionContext);
            }
            public void ReaderExecuting(
                DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
            {
                LogIfNonAsync(command, interceptionContext);
            }
            public void ReaderExecuted(
                DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
            {
                LogIfError(command, interceptionContext);
            }
            public void ScalarExecuting(
                DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
            {
                LogIfNonAsync(command, interceptionContext);
            }
            public void ScalarExecuted(
                DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
            {
                LogIfError(command, interceptionContext);
            }
            private void LogIfNonAsync<TResult>(
                DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
            {
                if (!interceptionContext.IsAsync)
                {
                    // Logger.Warn("Non-async command used: {0}", command.CommandText);

                    Console.WriteLine("Non-async command used: {0}", command.CommandText);
                }
            }
            private void LogIfError<TResult>(
                DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
            {
                if (interceptionContext.Exception != null)
                {
                    //  Logger.Error("Command {0} failed with exception {1}",
                    //  command.CommandText, interceptionContext.Exception);
                    Console.WriteLine("Command {0} failed with exception {1}",
                     command.CommandText, interceptionContext.Exception);
                }
            }
        }
    }
}
