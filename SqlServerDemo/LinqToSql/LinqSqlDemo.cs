using System;
using System.Collections.Generic;
using System.Data.Linq;

using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace SqlServerDemo.LinqToSql
{
    //1.一个持久对象进行CURD操作的API

    //2.一个语言或者API用来实现类和类属性相关 查询

    //3.一个规定mapping metadata的工具

    //4.一种技术可以让ORM的实现同事物对象一起dirty checking....

    public class LinqSqlDemo
    {
        public DbBookContextDataContext db = new DbBookContextDataContext();
        public void GetBooks()
        {
            Table<Books> books = db.GetTable<Books>();
            var q = from c in books
                    select c;
            foreach (var item in q)
            {
                Console.WriteLine("Title={0}", item.Title);
            }
        }
        public void GetBooksRefAuthors()
        {
            var q = from c in db.Books
                    where c.Authors.AuthorId == 75
                    select c;
            foreach (var item in q)
            {
                Console.WriteLine("Title={0}", item.Title);
            }
        }
        public void InsertBooks()
        {

            var author = db.Authors.Single(x => x.AuthorId == 72);

            author.Books.Add(new Books()
            {
                Title = "I'm Working Hard",
                Genre = "Commendy",
                PublishDate = DateTime.Now
            });


            db.SubmitChanges();

        }
        public void JoinQuery()
        {

            var q = from c in db.Books
                    join a in db.Authors
                    on c.Author_AuthorId equals a.AuthorId
                    select new
                    {
                        c.Title,
                        a.Name
                    };

            foreach (var item in q)
            {
                Console.WriteLine("Title:{0},Name:{1}", item.Title, item.Name);
            }


        }
        public void UnionQuery()
        {
            var q = from c in db.Authors
                    from o in db.Books
                    where o.Author_AuthorId == 75
                    select new { c, o };

            foreach (var item in q)
            {
                Console.WriteLine("Title:{0},Name:{1}", item.o.Title, item.c.Name);
            }
        }
        public void IntoQuery()
        {
            var q = from o in db.Books
                    where o.Author_AuthorId == 75
                    select new { Title = o.Title, Price = o.Price } into x
                    orderby x.Price
                    select x;

            foreach (var item in q)
            {
                Console.WriteLine("Title:{0},Price:{1}", item.Title, item.Price);
            }
        }
        public void ChangeBooksTitle()
        {
            ///RefreshMode的三种方式
            Books bb = new Books();
            try
            {
                var b = db.Books.Single(x => x.BookId == 4);
                b.Title = "Ba La Ba La ";
                bb = b;
                var c = db.Books.Single(x => x.BookId == 5);
                c.Title = "I Wanna Be Free";
                db.SubmitChanges();
            }
            catch (ChangeConflictException ex)
            {
                foreach (ObjectChangeConflict cc in db.ChangeConflicts)
                {
                    cc.Resolve(RefreshMode.KeepCurrentValues);
                }
                var title = bb.Title;
            }

        }
        public void RemoveBooks()
        {
            var author = db.Authors.Single(x => x.AuthorId == 75);

            author.Books.Remove(author.Books[0]);

            foreach (var item in author.Books)
            {
                Console.WriteLine("Title:{0}", item.Title);
            }
        }
        public void TransactionUse()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                var b = db.Books.Single(x => x.BookId == 2);

                b.Title = "SHit";

                db.SubmitChanges();

                ts.Complete();
            }
        }
        public void InheritVechcle()
        {
            // var p = db.Vehicles.Where(x => x is Truck);

            var p = db.Vehicles.OfType<Truck>();

            foreach (Truck item in p)
            {
                Console.WriteLine(item.Axles);
            }
        }
        public void ExcuteQuery()
        {
            IEnumerable<Vehicle> result = db.ExecuteQuery<Vehicle>("select * from Vehicle where MfgPlant={0}", 1);

            foreach (var item in result)
            {
                Console.WriteLine(item.MfgPlant);
            }
        }
        public void UseProMethod()
        {
            int result = db.GetBooksCount(1);
            Console.WriteLine(result);
        }
        public void QueryFun()
        {
            var query = (from a in db.GetQueryResult()
                         select new { a.Title }).ToList();
        }
        public void GetMulti()
        {
            var result = db.Pr_GetUserAndRole();

            foreach (var p in result.GetResult<Books>())
            {
                Console.WriteLine(p.Title);
            }

            foreach (var p in result.GetResult<Authors>())
            {
                Console.WriteLine(p.Name);
            }
        }
        public void LoadOptions()
        {

            ///这样会少执行几次数据库查询
            DataLoadOptions dloadOptions = new DataLoadOptions();
            dloadOptions.LoadWith<Authors>(x => x.Books);
            db.LoadOptions = dloadOptions;
            var result = db.Authors.ToList();
            foreach (var item in result)
            {
                //if (item.Books.Count > 0)
                //    Console.WriteLine("BooksName:{0}", item.Books[0].Title);
                foreach (var book in item.Books)
                {
                    Console.WriteLine("BooksName:{0}", item.Books[0].Title);
                }
            }


        }
        public void ObjectTrackingEnabledToFalse()
        {
            //在只读的时候可以用到提高效率
            //设置为false 会关闭懒加载机制
            db.ObjectTrackingEnabled = false;
            ///这样会少执行几次数据库查询
            db.Books.Attach(new Books());
            db.SubmitChanges();
        }
        public void ChangeSetUse()
        {
            //Attach 关联到 关联表 还有并发控制RowVersion
            var book = db.Books.Single(x => x.BookId == 4);
            book.Title = "quanliyouxi";
            book.Authors = null;
            var book2 = db.Books.Single(x => x.BookId == 13);
            db.Books.DeleteOnSubmit(book2);
            db.Books.InsertOnSubmit(new Books()
            {
                Title = "sanzhiyan",
                Price = 99,
                PublishDate = DateTime.Now,
            });
            var changeSet = db.GetChangeSet();
            foreach (var item in db.Books)
            {
                Console.WriteLine("Titel:{0}", item.Title);
            }
            var orginalBook = db.Books.GetOriginalEntityState(book);
            var books = db.Books.ToList();
            db.SubmitChanges();
        }
        public void ValidateDelete()
        {
            var book = db.Books.Single(x => x.BookId == 16);

            // author.Books.Remove(author.Books[0]);

            db.Books.DeleteOnSubmit(book);

            db.SubmitChanges(ConflictMode.FailOnFirstConflict);
          
        }
        public void Model()
        {
            var model = db.Mapping;
        }
        public void Dead()
        {
            int i = 0;
            do
            {
                try
                {
                    i++;
                    A();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("I'm Not Work at {0} index", i);
                }
               
            } while (i < 1);
        }
        public void A()
        {
            Console.Write("starting From A");
            B();
            Console.WriteLine("end from A");
           
        }
        public void B()
        {
            Console.Write("starting From B");
            A();
            Console.WriteLine("end from B");
        }
    }
}
