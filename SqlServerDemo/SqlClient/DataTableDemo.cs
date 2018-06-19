using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDemo.SqlClient
{
    public class DataTableDemo
    {
        private const string _windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true;";
        public void DtColumn()
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
                foreach (DataColumn item in dtBooks.Columns)
                {
                    if (item.ColumnName == "BookId")
                    {
                        item.AutoIncrement = true;
                        item.AutoIncrementStep = 1;
                        item.Unique = true;
                        item.ExtendedProperties.Add("BookId", "Don't Forget Orignal Heart");
                    }
                    if (item.ColumnName == "Price")
                    {
                        item.Expression = "Max(BookId)";
                    }

                }
                ds.ExtendedProperties.Add("expiration", DateTime.Now.AddDays(1));
                var newRow = dtBooks.NewRow();
                newRow["Title"] = "I Wanna be free";
                var newRow2 = dtBooks.NewRow();
                newRow2["Title"] = "I Wanna be free2";
                dtBooks.Rows.Add(newRow);
                dtBooks.Rows.Add(newRow2);
                Console.WriteLine(dtBooks.Columns["BookId"].ExtendedProperties["BookId"]);
            }
        }
        public void DtRow()
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
                foreach(DataRow row in dtBooks.Rows)
                {
                    Console.WriteLine("Row HasErrors:{0}",row.HasErrors);
                    row.SetAdded();
                    Console.WriteLine(row.RowState);
                }

               
            }
        }
    }
}
