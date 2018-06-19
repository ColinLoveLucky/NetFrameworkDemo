using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDemo.SqlClient
{
    //Aceess数据库：providerName="System.Data.OleDb"
    //Oracle 数据库：providerName="System.Data.OracleClient"或者providerName="Oracle.DataAccess.Client"
    //SQLite数据库：providerName="System.Data.SQLite"
    //SQL SERVER数据库：providerName="System.Data.SqlClient"
    //MYSQL数据库：providerName="MySql.Data.MySqlClient"
    //ODBC连接数据库:providerName="System.Data.Odbc"
    public class SqlAsyncDemo
    {
        public async Task SqlConnectionStringBilderMethod()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "172.16.37.67";
            builder.IntegratedSecurity = false;
            builder.Password = "Password@1";
            builder.UserID = "sa";
            builder.InitialCatalog = "DbBooks";
            string provider = "System.Data.SqlClient";
         
            DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = builder.ConnectionString;
                await connection.OpenAsync();
                DbCommand command = connection.CreateCommand();
                command.CommandText = "Select * from books";
                using (DbDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object obj = await reader.GetFieldValueAsync<object>(i);

                            Console.WriteLine(obj);
                        }
                    }
                }
            }
        }
    }
}
