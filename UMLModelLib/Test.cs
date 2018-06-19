using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLModelLib
{
    public class Test
    {
        public void GetMultiStore()
        {
            using (var db = new LinqToEntityDbEntities3())
            {
                var results = db.GetAllBologsandCourse();

                foreach (var result in results)
                {
                    Console.WriteLine("Blog: " + result.Name);
                }

                var posts = results.GetNextResult<course>();

                foreach (var result in posts)
                {
                    Console.WriteLine("Course: " + result.Name);
                }

                Console.ReadLine();
            }
        }

        public void InsertBlogUseStore()
        {
            using (var dbContext = new LinqToEntityDbEntities3())
            {
                //dbContext.blogs.Add(new blog()
                //{
                //    Name = "Hello World"
                //});

                //dbContext.SaveChanges();

                //  var blog = dbContext.blogs.First();

                //  blog.posts.Add(new post() { Title = "Post second" });

                // dbContext.SaveChanges();

                var singleBlog = dbContext.blogs.First();

                dbContext.Entry<blog>(singleBlog).Reload();
            }
        }

        public void InsertPost()
        {
            using (var dbContext = new LinqToEntityDbEntities3())
            {
                dbContext.posts.Add(new post()
                {

                    Title = "Hello World"
                });

                dbContext.SaveChanges();
            }
        }

        public void InsertOneEntityTwoTablePeople()
        {
            using (var dbContext = new LinqToEntityDbEntities3())
            {
                dbContext.ChinaPersons.Add(new ChinaPerson()
                {
                    FirstName = "HelloKitte",
                    LastName = "HelloMoMo",
                    Email = "hell",
                    Phone = "233"

                });

                dbContext.SaveChanges();
            }
        }

        public void ManyETInOneTable()
        {
            //using (var dbContext = new LinqToEntityDbEntities3())
            //{
            //    dbContext.YuanGongTable1.Add(new YuanGongTable1()
            //    {
            //        Name = "zhansan"

            //    });

            //    dbContext.SaveChanges();
            //}
        }

        public void GetFun()
        {
            using (var dbContext = new LinqToEntityDbEntities3())
            {
                var q = (from a in dbContext.GetCourse(1)
                         select a).ToList();
                foreach (var item in q)
                {
                    Console.WriteLine("Name:{0}", item.Name);
                }
            }
        }

        public void GetMetaEdmItem()
        {
            using (var db = new LinqToEntityDbEntities3())
            {
                var metadata = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace;

                var storeItemCollection = metadata.GetItemCollection(DataSpace.OSpace);

                foreach (var item in storeItemCollection)
                {
                    //if (BuiltInTypeKind.EntityContainer == item.BuiltInTypeKind)
                    //{
                    //    var entityContainer = (EntityContainer)item;
                    //    foreach (var ass in entityContainer.AssociationSets)
                    //    {

                    //    }
                    //}
                    //if(BuiltInTypeKind.EdmType==item.BuiltInTypeKind)
                    //{
                    //    var a = item;
                    //}
                    //if (item.GetType().Name == EdmType.GetBuiltInType(BuiltInTypeKind.ComplexType).Name)
                    //{
                    //    var a = item;
                    //}

                    //Console.WriteLine(EdmType.GetBuiltInType(BuiltInTypeKind.ComplexType).FullName);

                    //Console.WriteLine(EdmType.GetBuiltInType(BuiltInTypeKind.ComplexType).Name);

                    //     var a = item as EntityContainerMapping;

                }
                // string tableName = entitySetBase.MetadataProperties["Schema"].Value + "." + entitySetBase.MetadataProperties["Table"].Value;
            }

        }
    }
}
