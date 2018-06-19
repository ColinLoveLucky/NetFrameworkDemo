using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDemo.SqlClient
{
    public class DataSetDemo
    {
        private const string _windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true;";
        public void DataSet()
        {
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Authors");
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from authors";
                adapter.SelectCommand = command;
                DataSet ds = new System.Data.DataSet("ds");
                adapter.Fill(ds, "Authors");
                foreach (DataRow row in ds.Tables["Authors"].Rows)
                {
                    Console.WriteLine("Name :{0}", row[1]);
                }
            }
        }
        public void DataSetFill()
        {
            DataSet ds = new System.Data.DataSet();
            DataTable dtAuthors = new DataTable("Authors");
            ds.Tables.Add(dtAuthors);
            dtAuthors.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Name",Type.GetType("System.String"))
            });
            var row = dtAuthors.NewRow();
            row[0] = "Hello Kitte";
            dtAuthors.Rows.Add(row);
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Authors");
                //adapter.InsertCommand = new SqlCommand("Insert into Authors(name) values(@Name)", conn);
                //adapter.InsertCommand.Parameters.Add("Name", SqlDbType.NVarChar);
                //adapter.InsertCommand.Parameters["Name"].SourceColumn = "Name";
                //adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                adapter.SelectCommand = new SqlCommand("Select * from Authors", conn);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(ds);
                adapter.Update(ds);
            }
        }
        public void DataSetUpdate()
        {
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from authors", conn);
                DataSet ds = new System.Data.DataSet("ds");
                adapter.Fill(ds, "authors");
                foreach (DataRow datarow in ds.Tables[0].Rows)
                {
                    datarow["name"] = "sc";//ai->sc
                }
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(ds, "authors");
            }
        }
        public void DataSetDelete()
        {
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from authors", conn);
                DataSet ds = new System.Data.DataSet("ds");
                adapter.Fill(ds, "authors");
                //adapter.DeleteCommand = new SqlCommand("delete from authors", conn);
                // adapter.DeleteCommand = new SqlCommand("delete from authors where name=@Name", conn);
                //  adapter.DeleteCommand.Parameters.Add("Name", SqlDbType.NVarChar);
                //  adapter.DeleteCommand.Parameters["Name"].SourceColumn = "name";
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                ds.Tables["authors"].Rows[ds.Tables["authors"].Rows.Count - 1].Delete();
                adapter.Update(ds, "authors");
            }
        }
        public void DataSetDataRelation()
        {
            DataTable dtAuthors = new DataTable("Authors");
            dtAuthors.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("AuthorId", Type.GetType("System.Int32")),
                new DataColumn("Name", Type.GetType("System.String"))
            });
            DataTable dtBooks = new DataTable("Books");
            dtBooks.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("BookId", Type.GetType("System.Int32")),
                new DataColumn("Title", Type.GetType("System.String")),
                 new DataColumn("Price", Type.GetType("System.Decimal")),
                new DataColumn("Author_AuthorId", Type.GetType("System.Int32"))
            });
            DataSet ds = new System.Data.DataSet("ds");
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Authors");
                adapter.TableMappings.Add("Table1", "Books");
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from authors select * from Books";
                adapter.SelectCommand = command;
                adapter.Fill(ds);

                //DataRelatin

                var childRelation = new DataRelation("FK", ds.Tables["Authors"].Columns["AuthorId"], ds.Tables["Books"].Columns["Author_AuthorId"]);
                ds.Relations.Add(childRelation);
                var fkF = childRelation.ChildKeyConstraint;
                fkF.DeleteRule = Rule.SetNull;
                ds.Tables["Authors"].Rows.RemoveAt(2);
                ds.AcceptChanges();
                foreach (DataRow row in ds.Tables["Authors"].Rows)
                {
                    row.GetChildRows(ds.Relations["FK"]).ToList().ForEach(x =>
                    {
                        Console.WriteLine(x["Title"]);
                    });
                }

                //Constraint->ForeignKey
                //var foreignKey = new ForeignKeyConstraint(ds.Tables["Authors"].Columns["AuthorId"], ds.Tables["Books"].Columns["Author_AuthorId"]);
                //foreignKey.DeleteRule = Rule.SetNull;
                //ds.Tables["Books"].Constraints.Add(foreignKey);
                //ds.EnforceConstraints = true;
                //ds.Tables["Authors"].PrimaryKey = new DataColumn[] { ds.Tables["Authors"].Columns["AuthorId"] };
                //ds.Tables["Authors"].Rows.Remove(ds.Tables["Authors"].Rows.Find(75));
                //ds.AcceptChanges();
                //foreach (DataRow row in ds.Tables["Books"].Rows)
                //{
                //    Console.WriteLine(row["Title"]);
                //}

            }
        }
        public void DataSetAcceptChanges()
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
                ///OrignalState
                Console.WriteLine("原始的RowState");
                foreach (DataRow row in ds.Tables["Books"].Rows)
                {
                    Console.WriteLine(row.RowState);
                }
                Console.WriteLine("更改之后的状态");
                ds.Tables["Books"].Rows[0]["Title"] = "I Wanna be Free";
                ds.Tables["Books"].Rows.Add(new object[] { 12, "KKK" });
                ds.Tables["Books"].Rows[1].Delete();
                foreach (DataRow row in ds.Tables["Books"].Rows)
                {
                    Console.WriteLine(row.RowState);
                }

                Console.WriteLine("RejectChanges之后的状态");
                ds.RejectChanges();
                foreach (DataRow row in ds.Tables["Books"].Rows)
                {
                    Console.WriteLine(row.RowState);
                }

                ds.Tables["Books"].Rows[0]["Title"] = "I Wanna be Free";
                ds.Tables["Books"].Rows.Add(new object[] { 12, "KKK" });
                ds.Tables["Books"].Rows[1].Delete();
                Console.WriteLine("调用AcceptChanges之后的状态");
                ds.AcceptChanges();
                foreach (DataRow row in ds.Tables["Books"].Rows)
                {
                    Console.WriteLine(row.RowState);
                }
            }
        }
        public void DataViewDemo()
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
                DataView dvBooks = new DataView(ds.Tables["Books"]);
                DataView dvAuthors = new DataView(ds.Tables["Authors"]);
                dvBooks.RowStateFilter = DataViewRowState.ModifiedCurrent;
                dvBooks.RowFilter = "Title like 'Hello%'";
                dvBooks.Sort = "Title desc";
                var dtBooksNew = dvBooks.ToTable(true, new string[] { "Title" });
                Console.WriteLine("Ds更改之前的数值");
                var dataRowViewCollection = dvBooks.FindRows(new object[] { "Hello Kitte" });
                foreach (DataRowView row in dataRowViewCollection)
                {
                    Console.WriteLine(row.Row[1]);
                }
                Console.WriteLine("Ds更改之后DV的数据");
                foreach (DataRow row in ds.Tables["Books"].Rows)
                {
                    row[1] = "HongKong";
                }
                dvBooks.RowFilter = "Title like 'Hong%'";
                dataRowViewCollection = dvBooks.FindRows(new object[] { "HongKong" });
                foreach (DataRowView row in dataRowViewCollection)
                {
                    Console.WriteLine(row.Row[1]);
                }
                Console.WriteLine("Dv更改之后的Ds结果");
                var enumerator = dvBooks.GetEnumerator();
                DataRowView rowView;
                while (enumerator.MoveNext())
                {
                    rowView = (DataRowView)enumerator.Current;
                    rowView.Row[1] = "I Wanna be free";
                    Console.WriteLine(rowView.Row[1]);
                }
                foreach(DataRow dr in ds.Tables["Books"].Rows)
                {
                    Console.WriteLine(dr["Title"]);
                }


                
            }
        }
    }
}
