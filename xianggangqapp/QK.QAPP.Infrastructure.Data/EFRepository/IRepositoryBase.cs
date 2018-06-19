using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public interface IRepositoryBase<R> : IDisposable where R :class
    {
        /// <summary>
        /// 将实体添加进数据库
        /// </summary>
        /// <typeparam name="R">泛型类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Add(R entity);
        /// <summary>
        /// 将多个实体添加进数据库
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddMultiple(List<R> entities) ;
        /// <summary>
        /// 添加或更新数据库
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddOrUpdate(R entity) ;
        /// <summary>
        /// 添加或更新数据库
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddOrUpdateMultiple(List<R> entities) ;
        /// <summary>
        /// Count all entities of a specific type.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        Int32 Count() ;
        /// <summary>
        /// 销毁数据库连接
        /// </summary>
        new void Dispose();
        /// <summary>
        /// 根据实体删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(R entity) ;
        /// <summary>
        /// 根据实体列表删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool DeleteMultiple(List<R> entities) ;
        /// <summary>
        /// 移除对象 
        /// </summary>
        /// <param name="entity"></param>
        void Detach(object entity);
        /// <summary>
        /// 根据实体列表移除对象
        /// </summary>
        /// <param name="entities"></param>
        void Detach(List<object> entities);
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="where">Lamada查询语句</param>
        /// <param name="istacking">是否跟踪状态</param>
        /// <returns></returns>
        IQueryable<R> Find(Expression<Func<R, bool>> where, bool istacking = true);
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<R> Find(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) ;
        /// <summary>
        /// 查询出第一条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        R First(Expression<Func<R, bool>> where) ;
        /// <summary>
        /// 查询出第一数数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="isTracking">是否跟踪状态</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        R FirstOrDefault(Expression<Func<R, bool>> where, bool isTracking = true, params Expression<Func<R, object>>[] includes);
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<R> GetAll() ;
        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<R> GetAll(params Expression<Func<R, object>>[] includes) ;
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        DbConnection GetConnection();
        /// <summary>
        /// 设置命令
        /// </summary>
        void SetIdentityCommand();
        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <param name="connectionString"></param>
        void SetConnectionString(string connectionString);
        /// <summary>
        /// 设置是否抛出异常
        /// </summary>
        /// <param name="rehtrowExceptions"></param>
        void SetRethrowExceptions(bool rehtrowExceptions);
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        R Single(Expression<Func<R, bool>> where) ;
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        R SingleOrDefault(Expression<Func<R, bool>> where) ;
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        R SingleOrDefault(Expression<Func<R, bool>> where, Expression<Func<R, object>> include) ;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(R entity) ;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        bool UpdateProperty(R entity, params Expression<Func<R, object>>[] properties) ;
        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool UpdateMultiple(List<R> entities) ;
        /// <summary>
        /// 带分页和排序的查询数据
        /// </summary>
        /// <typeparam name="R">The type of the entity.</typeparam>
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
        /// <param name="orderBy">The order by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        IEnumerable<R> Get<TOrderBy>(Expression<Func<R,TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) ;
        /// <summary>
        /// 带分页和排序的查询数据
        /// </summary>
        /// <typeparam name="R">The type of the entity.</typeparam>
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        IEnumerable<R> Get<TOrderBy>(Expression<Func<R, bool>> criteria, Expression<Func<R, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) ;
        /// <summary>
        /// Gets entities which satifies a specification.
        /// </summary>
        /// <typeparam name="R">The type of the entity.</typeparam>
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        IEnumerable<R> Get<TOrderBy>(ISpecification<R> specification, Expression<Func<R, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) ;
        /// <summary>
        /// Counts entities with the specified criteria.
        /// </summary>
        /// <typeparam name="R">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        int Count(Expression<Func<R, bool>> criteria) ;
        /// <summary>
        /// Counts entities satifying specification.
        /// </summary>
        /// <typeparam name="R">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        int Count(ISpecification<R> criteria) ;
        /// <summary>
        /// 对数据库执行给定的 DDL/DML 命令。
        /// </summary>
        /// <param name="sql">命令字符串。</param>
        /// <param name="parameters">要应用于命令字符串的参数。</param>
        void ExecuteSqlCommand(string sql, params object[] parameters);
        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定类型的元素。类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。
        /// </summary>
        /// <param name="elementType">查询所返回对象的类型。</param>
        /// <param name="sql">SQL 查询字符串。</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数。</param>
        /// <returns></returns>
        IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters);
        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。
        /// </summary>
        /// <typeparam name="TElement">查询所返回对象的类型</typeparam>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters"> 要应用于 SQL 查询字符串的参数。</param>
        /// <returns></returns>
        IEnumerable<R> SqlQuery(string sql, params object[] parameters);
        
        /// <summary>
        /// UnitOfWork对象
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }

    /// <summary>
    /// 排序
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// 正序
        /// </summary>
        Ascending, 
        /// <summary>
        /// 倒序
        /// </summary>
        Descending
    }

}
