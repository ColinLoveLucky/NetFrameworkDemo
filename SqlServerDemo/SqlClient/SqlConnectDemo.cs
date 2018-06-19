using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlServerDemo.SqlClient
{
    public class SqlConnectDemo
    {
        private static string _connectionString = ConfigurationManager.AppSettings[""];

        public async Task Connect()
        {
            var windowsConnection = "Server=172.16.37.67;integrated security=true;database=DbBooks;user Id=sa;password=Password@1;pooling=true;connection lifetime=0;min pool size = 1;max pool size=40000";
            using (SqlConnection conn = new SqlConnection(windowsConnection))
            {
                await conn.OpenAsync();
                var command = conn.CreateCommand();
            }
            // SqlConnectionStringBuilder
            //DbProviderFactory
        }

      
    }
}
