using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public class EFRepositoryTransaction : Disposable, IRepositoryTransaction, IDisposable
    {


        Dictionary<Type, object> RepositoryCollection;

        IUnitOfWork _unitOfWork;

        public EFRepositoryTransaction(string contectStr)
        {
            RepositoryCollection = new Dictionary<Type, object>();  
            _unitOfWork = new UnitOfWork(new DbContextFactory(contectStr));
        }


        public EFRepositoryTransaction(IUnitOfWork unitOfWork)
        {
            RepositoryCollection = new Dictionary<Type, object>();
            _unitOfWork = unitOfWork;
        }


        public IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : Entity.BasicEntity
        {

            if (RepositoryCollection.ContainsKey(typeof(TEntity)))
            {

                return (IRepositoryBase<TEntity>)RepositoryCollection[typeof(TEntity)];

            }

            IRepositoryBase<TEntity> repository = new RepositoryBase<TEntity>(_unitOfWork);

            RepositoryCollection.Add(typeof(TEntity), repository);

            return repository;


        }

        public void BeginTransaction()
        {
            //EF SaveChange 本身就是一个事务处理无需要BeginTransaction
        }

        public void Commit()
        {
            _unitOfWork.SaveChanges();
        }

        public void Rollback()
        {
            //EF SaveChange 本身就是一个事务处理无需要Rollback
        }


        protected override void DisposeCore()
        {

            RepositoryCollection.Clear();
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
                _unitOfWork = null;
            }

        }
    }
}
