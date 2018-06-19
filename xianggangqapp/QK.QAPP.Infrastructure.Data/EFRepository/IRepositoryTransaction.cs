using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public interface IRepositoryTransaction:IDisposable
    {

        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepositoryBase<TEntity> GetRepository<TEntity>()

            where TEntity : BasicEntity;

        /// <summary>
        /// 事件开始
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 事件提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 事件回滚
        /// </summary>
        void Rollback();




    }
}
