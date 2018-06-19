using System;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsInTransaction { get; }

        void SaveChanges();

        void SaveChanges(SaveOptions saveOptions);

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void RollBackTransaction();

        void CommitTransaction();

        DbContext GetContext();
    }
}
