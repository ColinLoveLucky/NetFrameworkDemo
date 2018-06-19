using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SqlServerDemo.SqlClient
{
    public class SqlTransactionDemo
    {
        private const string _windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true;";
        private const string _windowsConnectionWangHua = "Server=172.16.37.36;integrated security=false;database=WangHua;user Id=sa;password=WangHua1986;Asynchronous Processing=true;";
        public void Trans()
        {
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction trans = conn.BeginTransaction();
                command.CommandType = System.Data.CommandType.Text;
                var strBuilder = new StringBuilder();
                var commandText = "select * from authors where name=@Name";
                command.CommandText = commandText;
                var parsName = new SqlParameter("@Name", System.Data.SqlDbType.NVarChar);
                parsName.Value = 12;
                command.Parameters.Add(parsName);
                try
                {
                    command.Transaction = trans;
                    var asyncResult = command.BeginExecuteReader();
                    var result = command.EndExecuteReader(asyncResult);
                    while (result.ReadAsync().GetAwaiter().GetResult())
                    {
                        Console.WriteLine(result.GetValue(1));
                    }
                    result.Close();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Dispose();
                    conn.Close();
                }
            }
        }
        public void CommitTrans()
        {
            using (SqlConnection conn = new SqlConnection(_windowsConnection))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                CommittableTransaction commitTrans = new CommittableTransaction();
                command.CommandType = System.Data.CommandType.Text;
                var strBuilder = new StringBuilder();
                var commandText = "select * from authors where name=@Name";
                command.CommandText = commandText;
                var parsName = new SqlParameter("@Name", System.Data.SqlDbType.NVarChar);
                parsName.Value = 12;
                command.Parameters.Add(parsName);
                try
                {
                    conn.EnlistTransaction(commitTrans);
                    var asyncResult = command.BeginExecuteReader();
                    var result = command.EndExecuteReader(asyncResult);
                    while (result.ReadAsync().GetAwaiter().GetResult())
                    {
                        Console.WriteLine(result.GetValue(1));
                    }
                    result.Close();
                    commitTrans.Commit();
                }
                catch (Exception ex)
                {
                    commitTrans.Rollback();
                    throw ex;
                }
                finally
                {
                    commitTrans.Dispose();
                    conn.Close();
                }
            }
        }
        public void DtcTrans()
        {
            ///利用分布式事务条件开启本地远程机上的DTC服务
            ///开启服务组件里面的安全允许进站，允许出站
            SqlConnection connXiangGang = new SqlConnection(_windowsConnection);
            SqlConnection connWangHua = new SqlConnection(_windowsConnectionWangHua);
            connXiangGang.Open();
            connWangHua.Open();
            CommittableTransaction commitTrans = new CommittableTransaction();
            connXiangGang.EnlistTransaction(commitTrans);
            connWangHua.EnlistTransaction(commitTrans);
            DisplayTransaction(commitTrans);
            try
            {
                SqlCommand command1 = new SqlCommand("insert into [Authors](name) values('xianggangtest')", connXiangGang);
                command1.ExecuteNonQuery();
                SqlCommand command2 = new SqlCommand("insert into CarA(name,Id) values('xianggangtest',111)", connWangHua);
                command2.ExecuteNonQuery();
                commitTrans.Commit();
            }
            catch(Exception ex)
            {
                commitTrans.Rollback();
            }
            finally
            {
                commitTrans.Dispose();
                connXiangGang.Close();
                connWangHua.Close();
            }
        }
        public  void DisplayTransaction(System.Transactions.Transaction tr)
        {
            if (tr != null)
            {
                Console.WriteLine("Createtime:" + tr.TransactionInformation.CreationTime);
                Console.WriteLine("Status:" + tr.TransactionInformation.Status);
                Console.WriteLine("Local ID:" + tr.TransactionInformation.LocalIdentifier);
                Console.WriteLine("Distributed ID:" + tr.TransactionInformation.DistributedIdentifier);
                Console.WriteLine();
            }
        }
        public void TransScope()
        {
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            // 设置事务超时时间为60秒
            transactionOption.Timeout = new TimeSpan(0, 0, 60);
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlConnection connXiangGang = new SqlConnection(_windowsConnection);
                SqlConnection connWangHua = new SqlConnection(_windowsConnectionWangHua);
                connXiangGang.Open();
                connWangHua.Open();
                try
                {
                    SqlCommand command1 = new SqlCommand("insert into [Authors](name) values('xianggangtest2')", connXiangGang);
                    command1.ExecuteNonQuery();
                    SqlCommand command2 = new SqlCommand("insert into CarA(name,Id) values('xianggangtest2',113)", connWangHua);
                    command2.ExecuteNonQuery();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    scope.Dispose();
                    connXiangGang.Close();
                    connWangHua.Close();
                }
            }
        }
    }
}
