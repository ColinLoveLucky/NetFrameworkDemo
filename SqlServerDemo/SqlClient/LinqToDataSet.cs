using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDemo.SqlClient
{
    public class LinqToDataSet
    {
        private const string _windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true;";
        // DataRowExtensions
        // DataTableExtensions
        public void LinqDt()
        {
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                DataSet ds = new System.Data.DataSet("ds");
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Authors");
                adapter.TableMappings.Add("Table1", "Books");
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from authors select * from Books";
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                DataTable dtBooks = ds.Tables["Books"];
                var query = from books in dtBooks.AsEnumerable()
                            where books.Field<string>("Title").Contains("Hello") == true
                            select new
                            {
                                Id = books.Field<int>("bookId"),
                                bookTitle = books.Field<string>("Title"),
                                Genre = books.Field<string>("Genre"),
                                PublishDate = books.Field<DateTime>("publishDate"),
                                Price = books.Field<decimal>("Price")
                            };
                foreach (var book in query)
                {
                    Console.WriteLine("BookId:{0},Title:{1}，Genre:{2},PublishDate:{3},Price:{4}",
                        book.Id, book.bookTitle, book.Genre, book.PublishDate, book.Price);
                }
            }
        }
        public void LinqJoinDt()
        {
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                DataSet ds = new System.Data.DataSet("ds");
                ds.Locale = CultureInfo.InvariantCulture;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Authors");
                adapter.TableMappings.Add("Table1", "Books");
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from authors select * from Books";
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                DataTable dtBooks = ds.Tables["Books"];
                DataTable dtAuthors = ds.Tables["Authors"];
                var query = from books in dtBooks.AsEnumerable()
                            join authors in dtAuthors.AsEnumerable()
                            on books.Field<int>("Author_AuthorId") equals
                            authors.Field<int>("AuthorId")
                            select new
                            {
                                Id = books.Field<int>("BookId"),
                                bookTitle = books.Field<string>("Title"),
                                Genre = books.Field<string>("Genre"),
                                PublishDate = books.Field<DateTime>("publishDate"),
                                Price = books.Field<decimal>("Price"),
                                AuthorName = authors.Field<string>("name")
                            };
                foreach (var book in query)
                {
                    Console.WriteLine("BookId:{0},Title:{1}，Genre:{2},PublishDate:{3},Price:{4},authorName:{5}",
                        book.Id, book.bookTitle, book.Genre, book.PublishDate, book.Price,book.AuthorName);
                }
            }
        }
    }
}
