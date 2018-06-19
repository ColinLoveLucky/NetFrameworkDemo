using SqlServerDemo.LinqToSql;
using SqlServerDemo.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            TransTest sqlConnect = new TransTest();

            sqlConnect.TestMultiTrans();
        }
    }
}
