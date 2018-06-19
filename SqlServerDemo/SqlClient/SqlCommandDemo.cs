using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlServerDemo.SqlClient
{
    public class SqlCommandDemo
    {
        public List<string> Names
        {
            get; set;
        } = new List<string> { "C# Via CLa", "Windows 核心编程" };

        public void Command()
        {
            var windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true";
            using (SqlConnection conn = new SqlConnection(windowsConnection))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                var strBuilder = new StringBuilder();
                Names.ForEach(item =>
                {
                    strBuilder.AppendLine("insert into authors(Name) values('" + item + "')");
                });
                strBuilder.AppendLine("inserttt   ddds");
                var commandText = strBuilder.ToString();
                command.CommandText = commandText;

                try
                {
                    var asyncResult = command.BeginExecuteNonQuery();

                    var result = command.EndExecuteNonQuery(asyncResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public async Task CommandExcuteNoQueryAsync()
        {
            var windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true";
            using (SqlConnection conn = new SqlConnection(windowsConnection))
            {
                await conn.OpenAsync();
                SqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 1000;
                command.CommandType = System.Data.CommandType.Text;
                var strBuilder = new StringBuilder();
                Names.ForEach(item =>
                {
                    strBuilder.AppendLine("insert into authors(Name) values('" + item + "')");
                });
                strBuilder.AppendLine("inserttt   ddds");
                var commandText = strBuilder.ToString();
                command.CommandText = commandText;
                try
                {
                    var task = command.ExecuteNonQueryAsync();
                    task.Wait();
                }
                catch (AggregateException ex)
                {
                    foreach (var item in ex.InnerExceptions)
                    {
                        throw item;
                    }
                }

            }
        }

        public void CommandParameters()
        {
            var windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true";
            using (SqlConnection conn = new SqlConnection(windowsConnection))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                var strBuilder = new StringBuilder();
                var commandText = "insert into authors(Name) values(@Name)";
                command.CommandText = commandText;
                var parsName = new SqlParameter("@Name", System.Data.SqlDbType.Int);
                parsName.Value = 12;
                command.Parameters.Add(parsName);
                try
                {
                    var asyncResult = command.BeginExecuteNonQuery();
                    var result = command.EndExecuteNonQuery(asyncResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void CommandRead()
        {
            var windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true";
            using (SqlConnection conn = new SqlConnection(windowsConnection))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                var strBuilder = new StringBuilder();
                var commandText = "select * from authors where name=@Name";
                command.CommandText = commandText;
                var parsName = new SqlParameter("@Name", System.Data.SqlDbType.NVarChar);
                parsName.Value = 12;
                command.Parameters.Add(parsName);
                try
                {
                    var asyncResult = command.BeginExecuteReader();
                    var result = command.EndExecuteReader(asyncResult);
                    while (result.ReadAsync().GetAwaiter().GetResult())
                    {
                        Console.WriteLine(result.GetValue(1));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void CommandPro()
        {
            var windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true";
            using (SqlConnection conn = new SqlConnection(windowsConnection))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var strBuilder = new StringBuilder();

                command.CommandText = "Pro_GetNames";
                var parsName = new SqlParameter("@Name", System.Data.SqlDbType.NVarChar);
                parsName.Direction = System.Data.ParameterDirection.Input;
                parsName.Value = 12;
                command.Parameters.Add(parsName);
                try
                {
                    var asyncResult = command.BeginExecuteReader();
                    var result = command.EndExecuteReader(asyncResult);
                    while (result.ReadAsync().GetAwaiter().GetResult())
                    {
                        Console.WriteLine(result.GetValue(1));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task StorePro()
        {
            var windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true";
            using (SqlConnection conn = new SqlConnection(windowsConnection))
            {
                await conn.OpenAsync();
                SqlCommand command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "GetMulti";
                SqlParameter outPar = new SqlParameter("@count", SqlDbType.Int);
                outPar.Direction = ParameterDirection.Output;
                command.Parameters.Add(outPar);
                SqlDataAdapter dapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                dapter.Fill(ds);
                int result = (int)command.Parameters["@count"].Value;
            }
        }

        public void TestRowState()
        {
            //DataRow 分为 4种状态 Detected，Add，Modified，UnChanged

            //AcceptChange(), 更改RowState状态，Add-》Unchanged，Modified=》Unchanged

            //Detected 状态:NeeRow 或者 Remove掉 DataRowState

            //RejectChange:要在AcceptChange之前调用，否则起不到作用
            //RejectChange:Add 数据去掉，状态更改为Detected，Modified =》Unchaged，Remove=》数据不会恢复，状态还是Detected

            DataTable dt = new DataTable();

            DataColumn dc = new DataColumn() { ColumnName = "Id", DataType = Type.GetType("System.String") };

            dt.Columns.Add(dc);

            DataRow dr = dt.NewRow();

            dr["Id"] = "1";

            dt.Rows.Add(dr);

            dt.RejectChanges();

            dt.AcceptChanges();

            DataRow dr2 = dt.NewRow();

            dr2["Id"] = "2";

            dt.Rows.Add(dr2);

            dr["Id"] = "3";

            dt.AcceptChanges();

            dt.Rows.Remove(dr);

            dt.RejectChanges();

            dt.AcceptChanges();

        }

        public void RowOrignalCurrentValue()
        {
            //只有在AcceptChange之后会存在2种状态

            //针对 Row的Origanl，Current ，修改之后，存在2种状态，Add没有AcceptChange提交时没有Orignl状态的，
            //没有确定改变之前是不存在Orignal数值的
            //Remove之后Dt里面是没有数据了，所以2种数值都不存在了
            //如果不存在调用Current，Orignal会报异常
            DataTable dt = new DataTable();

            DataColumn dc = new DataColumn() { ColumnName = "Id", DataType = Type.GetType("System.String") };

            dt.Columns.Add(dc);

            DataRow dr = dt.NewRow();

            dr["Id"] = "1";

            dt.Rows.Add(dr);

            var currentRow = dr["Id", DataRowVersion.Current];
            var orignalRow = dr["Id", DataRowVersion.Original];

            dt.AcceptChanges();

            dt.Rows.Remove(dr);

        //    var currentRow= dr["Id", DataRowVersion.Current];

       //   var orignalRow = dr["Id", DataRowVersion.Original];
        }

        public void TestAggreation()
        {
            IEnumerable<int> list = Enumerable.Range(2, 10);
            int all = list.Aggregate((sum, index) => sum + index);

            Console.WriteLine(all);

            double all_intial = list.Aggregate(10.1, (sum, index) => sum + index);

            Console.WriteLine(all_intial);

            int is75 = list.Aggregate(10, (sum, index) => sum + index,res=>res+10);

            Console.WriteLine(is75);
        }
    }
}
