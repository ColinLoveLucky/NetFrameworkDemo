using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public class RepositoryBaseSql : IRepositoryBaseSql      
    {
        private IUnitOfWork unitOfWork;

        public RepositoryBaseSql(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
     
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return unitOfWork;
            }
        }
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            //this.Model.Database.ExecuteSqlCommand(sql, parameters);
            return ((IObjectContextAdapter)UnitOfWork.GetContext()).ObjectContext.ExecuteStoreCommand(sql, parameters);
            
        }
        public IEnumerable<R> SqlQuery<R>(string sql, params object[] parameters) where R : class
        {
            //return this.Model.Database.SqlQuery<R>(sql, parameters);
            return ((IObjectContextAdapter)UnitOfWork.GetContext()).ObjectContext.ExecuteStoreQuery<R>(sql, parameters);
        }

        /// <summary>
        /// 执行函数或者存储过程
        /// </summary>
        /// <typeparam name="R">类型</typeparam>
        /// <param name="functionName">函数名称或者存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public R ExcuteFunction<R>(string functionName, params ObjectParameter[] parameters)
        {
            var ret = ((IObjectContextAdapter)UnitOfWork.GetContext()).ObjectContext.ExecuteFunction<R>(functionName, parameters).FirstOrDefault();
            return ret;
        }

        /// <summary>
        /// 执行函数或存储过程(不返回结果集，只返回影响行数)
        /// </summary>
        /// <param name="functionName">存储过程名或函数名</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteFunction(string functionName, params ObjectParameter[] parameters)
        {
            var ret = ((IObjectContextAdapter)UnitOfWork.GetContext()).ObjectContext.ExecuteFunction(functionName, parameters);
            return ret;
        }
    }
}
