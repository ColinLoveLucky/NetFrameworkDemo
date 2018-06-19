//using System.Data.Entity.Core.Metadata.Edm;

namespace QK.QAPP.Infrastructure.Data.EFRepository
{
    /// <summary>
    /// 5.0不支持异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public class AsyncRepositoryBase<T> : RepositoryBase<T>
    //    where T : DbContext
    //{
    //    public AsyncRepositoryBase(bool throwExceptions, string connectionString = "", bool lazyConnection = false, TransactionTypes transactionType = TransactionTypes.DbTransaction, IsolationLevel isolationLevel = IsolationLevel.Snapshot,
    //        bool useTransactions = true, bool proxyCreationEnabled = false, int commandTimeout = 300, bool saveLastExecutedMethodInfo = false)
    //        : base(
    //            throwExceptions, connectionString, lazyConnection, transactionType, isolationLevel, useTransactions, proxyCreationEnabled, commandTimeout, saveLastExecutedMethodInfo
    //        )
    //    {}

    //    public async Task<bool> AddAsync<R>(R entity) where R : class
    //    {
    //        bool result = false;

    //        await ProcessTransactionableMethodAsync(async () =>
    //        {
    //            try
    //            {
    //                base.SetEntity<R>()
    //                    .Add(entity);

    //                await this.SaveChangesAsync();

    //                result = true;
    //            }
    //            catch (Exception error)
    //            {
    //                var entry = Model.Entry(entity);
    //                entry.State = EntityState.Unchanged;

    //                base.RollBack();
    //                base.Detach(entity);

    //                if (base._rethrowExceptions)
    //                {
    //                    throw;
    //                }
    //                else
    //                {
    //                    base.OnRepositoryBaseExceptionRaised(error);
    //                }
    //            }
    //            finally
    //            { }

    //        });

    //        return result;
    //    }

    //    public async Task SaveChangesAsync()
    //    {
    //        await Model.SaveChangesAsync();
    //    }

    //    internal async Task ProcessTransactionableMethodAsync(Action action)
    //    {
    //        if (base._saveLastExcecutedMethodInfo)
    //            base._lastExecutedMethod = MethodInfo.GetCurrentMethod();

    //        await Task.Run(() =>
    //        {
    //            base.StartTransaction();
    //            action();
    //        });
    //    }
    //}
}
