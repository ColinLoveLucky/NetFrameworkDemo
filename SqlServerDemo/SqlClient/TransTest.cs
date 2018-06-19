using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDemo.SqlClient
{
    public class TransTest
    {
        private const string _windowsConnection = "Server=172.16.37.10;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true;min pool size = 1;max pool size=100;Pooling=true;";
        public void TestMultiTrans()
        {
            CreateTrans();

        }

        private void CreateTrans()
        {
            for (var i = 0; i < 100; i++)
            {
                //using (SqlConnection conn = new SqlConnection(_windowsConnection))
                // {
                SqlConnection conn = new SqlConnection(_windowsConnection);
                    conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction trans = conn.BeginTransaction();
                command.CommandType = System.Data.CommandType.Text;
                var strBuilder = new StringBuilder();
                var commandText = "Insert into authors(Name) values(@Name)";
                command.CommandText = commandText;
                var parsName = new SqlParameter("@Name", System.Data.SqlDbType.NVarChar);
                parsName.Value = i;
                command.Parameters.Add(parsName);
                try
                {
                    command.Transaction = trans;
                    command.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    //trans.Dispose();
                    //conn.Close();
                }
                // }
            }
        }
    }
}
