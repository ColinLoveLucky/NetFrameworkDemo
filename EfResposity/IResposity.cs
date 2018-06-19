using EntityFramework.DynamicFilters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EfResposity
{
	public static class MemberInfoExtensions
	{
		/// <summary>
		/// Gets a single attribute for a member.
		/// </summary>
		/// <typeparam name="TAttribute">Type of the attribute</typeparam>
		/// <param name="memberInfo">The member that will be checked for the attribute</param>
		/// <param name="inherit">Include inherited attributes</param>
		/// <returns>Returns the attribute object if found. Returns null if not found.</returns>
		public static TAttribute GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
			where TAttribute : Attribute
		{
			if (memberInfo == null)
			{
				throw new ArgumentNullException(nameof(memberInfo));
			}

			var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
			if (attrs.Length > 0)
			{
				return (TAttribute)attrs[0];
			}

			return default(TAttribute);
		}


		public static TAttribute GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type type, bool inherit = true)
			where TAttribute : Attribute
		{
			var attr = type.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();
			if (attr != null)
			{
				return attr;
			}

			if (type.GetTypeInfo().BaseType == null)
			{
				return null;
			}

			return type.GetTypeInfo().BaseType.GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(inherit);
		}
	}
	public class AbpObjectParameter
	{
		public string Name { get; private set; }

		public Type ParameterType { get; private set; }

		public object Value { get; set; }

		public AbpObjectParameter(string name, Type type)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("name is Null or Empty");
			if (type == null)
				throw new ArgumentException("type is Null");
			this.Name = name;
			this.ParameterType = type;
		}

		public AbpObjectParameter(string name, object value)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("name is Null or Empty");
			if (value == null)
				throw new ArgumentException("value is Null");
			this.Name = name;
			this.ParameterType = value.GetType();
			this.Value = value;
		}
	}
	public interface IRepositorySql : IRepository
	{
		/// <summary>
		/// 对数据库执行给定的 DDL/DML 命令。
		/// </summary>
		/// <param name="sql">命令字符串。</param>
		/// <param name="parameters">要应用于命令字符串的参数。</param>
		void ExecuteSqlCommand(string sql, params object[] parameters);
		/// <summary>
		/// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。
		/// </summary>
		/// <typeparam name="TElement">查询所返回对象的类型</typeparam>
		/// <param name="sql">SQL 查询字符串</param>
		/// <param name="parameters"> 要应用于 SQL 查询字符串的参数。</param>
		/// <returns></returns>
		IEnumerable<R> SqlQuery<R>(string sql, params object[] parameters) where R : class;

		/// <summary>
		/// 执行函数或者存储过程
		/// </summary>
		/// <typeparam name="R">类型</typeparam>
		/// <param name="functionName">函数名称或者存储过程名称</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		R ExcuteFunction<R>(string functionName, params AbpObjectParameter[] parameters);


		/// <summary>
		/// 执行函数或存储过程(不返回结果集，只返回影响行数)
		/// </summary>
		/// <param name="functionName">存储过程名或函数名</param>
		/// <param name="parameters">参数</param>
		/// <returns>影响行数</returns>
		int ExecuteFunction(string functionName, params AbpObjectParameter[] parameters);
	}
	public class EfRepositorySqlBase<TDbContext> : IRepositorySql
	  where TDbContext : DbContext
	{
		public virtual TDbContext Context { get { return _dbContextProvider.GetDbContext(); } }

		private readonly IDbContextProvider<TDbContext> _dbContextProvider;

		public EfRepositorySqlBase(IDbContextProvider<TDbContext> dbContextProvider)
		{
			_dbContextProvider = dbContextProvider;
		}

		public void ExecuteSqlCommand(string sql, params object[] parameters)
		{
			((IObjectContextAdapter)Context).ObjectContext.ExecuteStoreCommand(sql, parameters);

		}
		public IEnumerable<R> SqlQuery<R>(string sql, params object[] parameters) where R : class
		{
			return ((IObjectContextAdapter)Context).ObjectContext.ExecuteStoreQuery<R>(sql, parameters);
		}
		/// <summary>
		/// 执行函数或者存储过程
		/// </summary>
		/// <typeparam name="R">类型</typeparam>
		/// <param name="functionName">函数名称或者存储过程名称</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		public R ExcuteFunction<R>(string functionName, params AbpObjectParameter[] parameters)
		{
			var @params = parameters.Select(p => new ObjectParameter(p.Name, p.ParameterType) { Value = p.Value }).ToArray();
			var ret = ((IObjectContextAdapter)Context).ObjectContext.ExecuteFunction<R>(functionName, @params).FirstOrDefault();
			for (int i = 0; i < @params.Count(); i++)
			{
				var param = @params[i];
				var abpParam = parameters[i];
				abpParam.Value = param.Value;
			}
			return ret;
		}

		/// <summary>
		/// 执行函数或存储过程(不返回结果集，只返回影响行数)
		/// </summary>
		/// <param name="functionName">存储过程名或函数名</param>
		/// <param name="parameters">参数</param>
		/// <returns>影响行数</returns>
		public int ExecuteFunction(string functionName, params AbpObjectParameter[] parameters)
		{
			var @params = parameters.Select(p => new ObjectParameter(p.Name, p.ParameterType) { Value = p.Value }).ToArray();
			var ret = ((IObjectContextAdapter)Context).ObjectContext.ExecuteFunction(functionName, @params);
			for (int i = 0; i < @params.Count(); i++)
			{
				var param = @params[i];
				var abpParam = parameters[i];
				abpParam.Value = param.Value;
			}
			return ret;
		}
	}
	public interface IRepository
	{
	}
	public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
	{
		#region Select/Get/Query

		/// <summary>
		/// Used to get a IQueryable that is used to retrieve entities from entire table.
		/// </summary>
		/// <returns>IQueryable to be used to select entities from database</returns>
		IQueryable<TEntity> GetAll();

		/// <summary>
		/// Used to get a IQueryable that is used to retrieve entities from entire table.
		/// One or more 
		/// </summary>
		/// <param name="propertySelectors">A list of include expressions.</param>
		/// <returns>IQueryable to be used to select entities from database</returns>
		IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

		/// <summary>
		/// Used to get all entities.
		/// </summary>
		/// <returns>List of all entities</returns>
		List<TEntity> GetAllList();

		/// <summary>
		/// Used to get all entities.
		/// </summary>
		/// <returns>List of all entities</returns>
		Task<List<TEntity>> GetAllListAsync();

		/// <summary>
		/// Used to get all entities based on given <paramref name="predicate"/>.
		/// </summary>
		/// <param name="predicate">A condition to filter entities</param>
		/// <returns>List of all entities</returns>
		List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Used to get all entities based on given <paramref name="predicate"/>.
		/// </summary>
		/// <param name="predicate">A condition to filter entities</param>
		/// <returns>List of all entities</returns>
		Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Used to run a query over entire entities.
		/// <see cref="UnitOfWorkAttribute"/> attribute is not always necessary (as opposite to <see cref="GetAll"/>)
		/// if <paramref name="queryMethod"/> finishes IQueryable with ToList, FirstOrDefault etc..
		/// </summary>
		/// <typeparam name="T">Type of return value of this method</typeparam>
		/// <param name="queryMethod">This method is used to query over entities</param>
		/// <returns>Query result</returns>
		T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

		/// <summary>
		/// Gets an entity with given primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity to get</param>
		/// <returns>Entity</returns>
		TEntity Get(TPrimaryKey id);

		/// <summary>
		/// Gets an entity with given primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity to get</param>
		/// <returns>Entity</returns>
		Task<TEntity> GetAsync(TPrimaryKey id);

		/// <summary>
		/// Gets exactly one entity with given predicate.
		/// Throws exception if no entity or more than one entity.
		/// </summary>
		/// <param name="predicate">Entity</param>
		TEntity Single(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets exactly one entity with given predicate.
		/// Throws exception if no entity or more than one entity.
		/// </summary>
		/// <param name="predicate">Entity</param>
		Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets an entity with given primary key or null if not found.
		/// </summary>
		/// <param name="id">Primary key of the entity to get</param>
		/// <returns>Entity or null</returns>
		TEntity FirstOrDefault(TPrimaryKey id);

		/// <summary>
		/// Gets an entity with given primary key or null if not found.
		/// </summary>
		/// <param name="id">Primary key of the entity to get</param>
		/// <returns>Entity or null</returns>
		Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

		/// <summary>
		/// Gets an entity with given given predicate or null if not found.
		/// </summary>
		/// <param name="predicate">Predicate to filter entities</param>
		TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets an entity with given given predicate or null if not found.
		/// </summary>
		/// <param name="predicate">Predicate to filter entities</param>
		Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Creates an entity with given primary key without database access.
		/// </summary>
		/// <param name="id">Primary key of the entity to load</param>
		/// <returns>Entity</returns>
		TEntity Load(TPrimaryKey id);

		#endregion

		#region Insert

		/// <summary>
		/// Inserts a new entity.
		/// </summary>
		/// <param name="entity">Inserted entity</param>
		TEntity Insert(TEntity entity);

		/// <summary>
		/// Inserts a new entity.
		/// </summary>
		/// <param name="entity">Inserted entity</param>
		Task<TEntity> InsertAsync(TEntity entity);

		/// <summary>
		/// Inserts a new entity and gets it's Id.
		/// It may require to save current unit of work
		/// to be able to retrieve id.
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>Id of the entity</returns>
		TPrimaryKey InsertAndGetId(TEntity entity);

		/// <summary>
		/// Inserts a new entity and gets it's Id.
		/// It may require to save current unit of work
		/// to be able to retrieve id.
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>Id of the entity</returns>
		Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// </summary>
		/// <param name="entity">Entity</param>
		TEntity InsertOrUpdate(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// </summary>
		/// <param name="entity">Entity</param>
		Task<TEntity> InsertOrUpdateAsync(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// Also returns Id of the entity.
		/// It may require to save current unit of work
		/// to be able to retrieve id.
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>Id of the entity</returns>
		TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// Also returns Id of the entity.
		/// It may require to save current unit of work
		/// to be able to retrieve id.
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <returns>Id of the entity</returns>
		Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

		#endregion

		#region Update

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="entity">Entity</param>
		TEntity Update(TEntity entity);

		/// <summary>
		/// Updates an existing entity. 
		/// </summary>
		/// <param name="entity">Entity</param>
		Task<TEntity> UpdateAsync(TEntity entity);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="id">Id of the entity</param>
		/// <param name="updateAction">Action that can be used to change values of the entity</param>
		/// <returns>Updated entity</returns>
		TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="id">Id of the entity</param>
		/// <param name="updateAction">Action that can be used to change values of the entity</param>
		/// <returns>Updated entity</returns>
		Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

		#endregion

		#region Delete

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">Entity to be deleted</param>
		void Delete(TEntity entity);

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">Entity to be deleted</param>
		Task DeleteAsync(TEntity entity);

		/// <summary>
		/// Deletes an entity by primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity</param>
		void Delete(TPrimaryKey id);

		/// <summary>
		/// Deletes an entity by primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity</param>
		Task DeleteAsync(TPrimaryKey id);

		/// <summary>
		/// Deletes many entities by function.
		/// Notice that: All entities fits to given predicate are retrieved and deleted.
		/// This may cause major performance problems if there are too many entities with
		/// given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities</param>
		void Delete(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Deletes many entities by function.
		/// Notice that: All entities fits to given predicate are retrieved and deleted.
		/// This may cause major performance problems if there are too many entities with
		/// given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities</param>
		Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

		#endregion

		#region Aggregates

		/// <summary>
		/// Gets count of all entities in this repository.
		/// </summary>
		/// <returns>Count of entities</returns>
		int Count();

		/// <summary>
		/// Gets count of all entities in this repository.
		/// </summary>
		/// <returns>Count of entities</returns>
		Task<int> CountAsync();

		/// <summary>
		/// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
		/// </summary>
		/// <param name="predicate">A method to filter count</param>
		/// <returns>Count of entities</returns>
		int Count(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
		/// </summary>
		/// <param name="predicate">A method to filter count</param>
		/// <returns>Count of entities</returns>
		Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets count of all entities in this repository (use if expected return value is greather than <see cref="int.MaxValue"/>.
		/// </summary>
		/// <returns>Count of entities</returns>
		long LongCount();

		/// <summary>
		/// Gets count of all entities in this repository (use if expected return value is greather than <see cref="int.MaxValue"/>.
		/// </summary>
		/// <returns>Count of entities</returns>
		Task<long> LongCountAsync();

		/// <summary>
		/// Gets count of all entities in this repository based on given <paramref name="predicate"/>
		/// (use this overload if expected return value is greather than <see cref="int.MaxValue"/>).
		/// </summary>
		/// <param name="predicate">A method to filter count</param>
		/// <returns>Count of entities</returns>
		long LongCount(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Gets count of all entities in this repository based on given <paramref name="predicate"/>
		/// (use this overload if expected return value is greather than <see cref="int.MaxValue"/>).
		/// </summary>
		/// <param name="predicate">A method to filter count</param>
		/// <returns>Count of entities</returns>
		Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

		#endregion
	}
	public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
	{
	}
	public interface ISoftDelete
	{
		/// <summary>
		/// Used to mark an Entity as 'Deleted'. 
		/// </summary>
		bool IsDeleted { get; set; }
	}
	public abstract class AbpResposityBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey>
	{
		public static MultiTenancySides? MultiTenancySide { get; private set; }
		public IUnitOfWorkManager UnitOfWorkManager { get; set; }
		public IIocResolver IocResolver { get; set; }
		public AbpResposityBase()
		{
			var attr = typeof(TEntity).GetSingleAttributeOfTypeOrBaseTypesOrNull<MultiTenancySideAttribute>();
			if (attr != null)
			{
				MultiTenancySide = attr.Side;
			}
		}
		public abstract IQueryable<TEntity> GetAll();
		public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
		{
			return GetAll();
		}
		public virtual List<TEntity> GetAllList()
		{
			return GetAll().ToList();
		}
		public virtual Task<List<TEntity>> GetAllListAsync()
		{
			return Task.FromResult(GetAllList());
		}
		public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
		{
			return GetAll().Where(predicate).ToList();
		}
		public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
		{
			throw new NotImplementedException();
		}
		public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
		{
			return queryMethod(GetAll());
		}
		public virtual TEntity Get(TPrimaryKey id)
		{
			var entity = FirstOrDefault(id);
			if (entity == null)
				throw new Exception($"{typeof(TEntity)}:id{id}");
			return entity;
		}
		public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
		{
			var entity = await FirstOrDefaultAsync(id);
			if (entity == null)
				throw new Exception($"{typeof(TEntity)}:id{id}");
			return entity;
		}
		public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
		{
			return GetAll().Single(predicate);
		}
		public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return Task.FromResult(Single(predicate));
		}
		public virtual TEntity FirstOrDefault(TPrimaryKey id)
		{
			return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
		}
		public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
		{
			return Task.FromResult(FirstOrDefault(id));
		}
		public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
		{
			return GetAll().FirstOrDefault(predicate);
		}
		public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return Task.FromResult(FirstOrDefault(predicate));
		}
		public TEntity Load(TPrimaryKey id)
		{
			return Get(id);
		}
		public abstract TEntity Insert(TEntity entity);
		public virtual Task<TEntity> InsertAsync(TEntity entity)
		{
			return Task.FromResult(Insert(entity));
		}
		public virtual TPrimaryKey InsertAndGetId(TEntity entity)
		{
			return Insert(entity).Id;
		}
		public virtual Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
		{
			return Task.FromResult(InsertAndGetId(entity));
		}
		public virtual TEntity InsertOrUpdate(TEntity entity)
		{
			return entity.IsTransient() ? Insert(entity) : Update(entity);
		}
		public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
		{
			return entity.IsTransient() ? await InsertAsync(entity) : await UpdateAsync(entity);
		}
		public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
		{
			return InsertOrUpdate(entity).Id;
		}
		public virtual Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
		{
			return Task.FromResult(InsertOrUpdateAndGetId(entity));
		}
		public abstract TEntity Update(TEntity entity);
		public virtual Task<TEntity> UpdateAsync(TEntity entity)
		{
			return Task.FromResult(Update(entity));
		}
		public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
		{
			var entity = Get(id);
			updateAction(entity);
			return entity;
		}
		public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
		{
			var entity = await GetAsync(id);
			await updateAction(entity);
			return entity;
		}
		public abstract void Delete(TEntity entity);
		public virtual Task DeleteAsync(TEntity entity)
		{
			Delete(entity);
			return Task.FromResult(0);
		}
		public abstract void Delete(TPrimaryKey id);
		public virtual Task DeleteAsync(TPrimaryKey id)
		{
			Delete(id);
			return Task.FromResult(0);
		}
		public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
		{
			foreach (var entity in GetAll().Where(predicate).ToList())
			{
				Delete(entity);
			}
		}
		public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
		{
			Delete(predicate);
			return Task.FromResult(0);
		}
		public virtual int Count()
		{
			return GetAll().Count();
		}
		public virtual Task<int> CountAsync()
		{
			return Task.FromResult(Count());
		}
		public virtual int Count(Expression<Func<TEntity, bool>> predicate)
		{
			return GetAll().Where(predicate).Count();
		}
		public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return Task.FromResult(Count(predicate));
		}
		public virtual long LongCount()
		{
			return GetAll().LongCount();
		}
		public virtual Task<long> LongCountAsync()
		{
			return Task.FromResult(LongCount());
		}
		public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
		{
			return GetAll().Where(predicate).LongCount();
		}
		public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return Task.FromResult(LongCount(predicate));
		}
		protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
		{
			var lambdaParam = Expression.Parameter(typeof(TEntity));

			var lambdaBody = Expression.Equal(
				Expression.PropertyOrField(lambdaParam, "Id"),
				Expression.Constant(id, typeof(TPrimaryKey))
				);

			return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
		}
		protected virtual IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query)
		{
			query = ApplyMultiTenancyFilter(query);
			query = ApplySoftDeleteFilter(query);
			return query;
		}
		protected virtual IQueryable<TEntity> ApplyMultiTenancyFilter(IQueryable<TEntity> query)
		{
			var tenantId = UnitOfWorkManager?.Current?.GetTenantId();
			if (typeof(IMustHaveTenant).GetTypeInfo().IsAssignableFrom(typeof(TEntity)))
			{
				if (UnitOfWorkManager?.Current == null || UnitOfWorkManager.Current.IsFilterEnabled(AbpDataFilters.MustHaveTenant))
				{
					query = query.Where(e => ((IMustHaveTenant)e).TenantId == tenantId);
				}
			}
			if (typeof(IMayHaveTenant).GetTypeInfo().IsAssignableFrom(typeof(TEntity)))
			{
				if (UnitOfWorkManager?.Current == null || UnitOfWorkManager.Current.IsFilterEnabled(AbpDataFilters.MayHaveTenant))
				{
					query = query.Where(e => ((IMayHaveTenant)e).TenantId == tenantId);
				}
			}
			return query;
		}
		private IQueryable<TEntity> ApplySoftDeleteFilter(IQueryable<TEntity> query)
		{
			if (typeof(ISoftDelete).GetTypeInfo().IsAssignableFrom(typeof(TEntity)))
			{
				if (UnitOfWorkManager?.Current == null || UnitOfWorkManager.Current.IsFilterEnabled(AbpDataFilters.SoftDelete))
				{
					query = query.Where(e => !((ISoftDelete)e).IsDeleted);
				}
			}
			return query;
		}
	}
	public interface IRepositoryWithDbContext
	{
		DbContext GetDbContext();
	}
	public interface IDbContextProvider<out TDbContext>
		where TDbContext : DbContext
	{
		TDbContext GetDbContext();
		TDbContext GetDbContext(MultiTenancySides? multiTenancySide);
	}
	public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
		where TDbContext : DbContext
	{
		public TDbContext DbContext { get; set; }
		public SimpleDbContextProvider(TDbContext dbContext)
		{
			DbContext = dbContext;
		}
		public TDbContext GetDbContext()
		{
			return DbContext;
		}

		public TDbContext GetDbContext(MultiTenancySides? multiTenancySide)
		{
			return DbContext;
		}
	}
	public class EfRepositoryBase<TDbContext, TEntity, TPrimaryKey> : AbpResposityBase<TEntity, TPrimaryKey>, IRepositoryWithDbContext
		where TEntity : class, IEntity<TPrimaryKey>
		where TDbContext : DbContext
	{
		public virtual TDbContext Context
		{
			get
			{
				return _dbContextProvider.GetDbContext(MultiTenancySide);
			}
		}
		private readonly IDbContextProvider<TDbContext> _dbContextProvider;
		public virtual DbSet<TEntity> Table { get { return Context.Set<TEntity>(); } }
		public EfRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
		{
			_dbContextProvider = dbContextProvider;
		}
		public override IQueryable<TEntity> GetAll()
		{
			return Table;
		}
		public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
		{
			if (propertySelectors.IsNullOrEmpty())
			{
				return GetAll();
			}
			var query = GetAll();
			foreach (var propertySelector in propertySelectors)
			{
				query = query.Include(propertySelector);
			}
			return query;
		}
		public override async Task<List<TEntity>> GetAllListAsync()
		{
			return await GetAll().ToListAsync();
		}
		public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await GetAll().Where(predicate).ToListAsync();
		}
		public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await GetAll().SingleAsync(predicate);
		}
		public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
		{
			return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
		}
		public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await GetAll().FirstOrDefaultAsync(predicate);
		}
		public override TEntity Insert(TEntity entity)
		{
			return Table.Add(entity);
		}
		public override Task<TEntity> InsertAsync(TEntity entity)
		{
			return Task.FromResult(Table.Add(entity));
		}
		public override TPrimaryKey InsertAndGetId(TEntity entity)
		{
			entity = Insert(entity);
			if (entity.IsTransient())
			{
				Context.SaveChanges();
			}
			return entity.Id;
		}
		public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
		{
			entity = await InsertAsync(entity);
			if (entity.IsTransient())
			{
				await Context.SaveChangesAsync();
			}
			return entity.Id;
		}
		public override TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
		{
			entity = InsertOrUpdate(entity);
			if (entity.IsTransient())
			{
				Context.SaveChanges();
			}
			return entity.Id;
		}
		public override async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
		{
			entity = await InsertOrUpdateAsync(entity);
			if (entity.IsTransient())
			{
				await Context.SaveChangesAsync();
			}
			return entity.Id;
		}
		public override TEntity Update(TEntity entity)
		{
			AttachIfNot(entity);
			Context.Entry(entity).State = EntityState.Modified;
			return entity;
		}
		public override Task<TEntity> UpdateAsync(TEntity entity)
		{
			AttachIfNot(entity);
			Context.Entry(entity).State = EntityState.Modified;
			return Task.FromResult(entity);
		}
		protected virtual void AttachIfNot(TEntity entity)
		{
			if (!Table.Local.Contains(entity))
			{
				Table.Attach(entity);
			}
		}
		public override void Delete(TEntity entity)
		{
			AttachIfNot(entity);
			Table.Remove(entity);
		}
		public override void Delete(TPrimaryKey id)
		{
			var entity = Table.Local.FirstOrDefault(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
			if (entity == null)
			{
				entity = FirstOrDefault(id);
				if (entity == null)
				{
					return;
				}
			}
			Delete(entity);
		}
		public override async Task<int> CountAsync()
		{
			return await GetAll().CountAsync();
		}
		public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await GetAll().Where(predicate).CountAsync();
		}
		public override async Task<long> LongCountAsync()
		{
			return await GetAll().LongCountAsync();
		}
		public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await GetAll().Where(predicate).LongCountAsync();
		}
		public DbContext GetDbContext()
		{
			return Context;
		}




	}
	public class EfRepositoryBase<TDbContext, TEntity> : EfRepositoryBase<TDbContext, TEntity, int>, IRepository<TEntity>
		where TEntity : class, IEntity<int>
		where TDbContext : DbContext
	{
		public EfRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
			: base(dbContextProvider)
		{
		}
	}
	public class ActiveTransactionProviderArgs : Dictionary<string, object>
	{
		public static ActiveTransactionProviderArgs Empty { get; } = new ActiveTransactionProviderArgs();
	}
	public interface IActiveTransactionProvider
	{
		IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args);
		IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args);
	}
	public class EfActiveTransactionProvider : IActiveTransactionProvider
	{
		private readonly IIocResolver _iocResolver;
		public EfActiveTransactionProvider(IIocResolver iocResolver)
		{
			_iocResolver = iocResolver;
		}
		public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
		{
			return GetDbContext(args).Database.Connection;
		}
		public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
		{
			return GetDbContext(args).Database.CurrentTransaction.UnderlyingTransaction;
		}
		private DbContext GetDbContext(ActiveTransactionProviderArgs args)
		{
			//var dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType((Type)args["ContextType"]);
			//using (var dbContextProviderWrapper = _iocResolver.ResolveAsDisposable(dbContextProviderType))
			//{
			//	var method = dbContextProviderWrapper.Object.GetType()
			//		.GetMethod(
			//			nameof(IDbContextProvider<AbpDbContext>.GetDbContext),
			//			new[] { typeof(MultiTenancySides) }
			//		);

			//	return (DbContext)method.Invoke(
			//		dbContextProviderWrapper.Object,
			//		new object[] { (MultiTenancySides?)args["MultiTenancySide"] }
			//	);
			//}
			return null;
		}
	}
	public enum EntityChangeType
	{
		Created,
		Updated,
		Deleted
	}
	public class EntityChangeEntry
	{
		public object Entity { get; set; }
		public EntityChangeType ChangeType { get; set; }
		public EntityChangeEntry(object entity, EntityChangeType changeType)
		{
			Entity = entity;
			ChangeType = changeType;
		}
	}
	public interface IEventData
	{
		DateTime EventTime { get; set; }
		object EventSource { get; set; }
	}
	public class DomainEventEntry
	{
		public object SourceEntity { get; }
		public IEventData EventData { get; }
		public DomainEventEntry(object sourceEntity, IEventData eventData)
		{
			SourceEntity = sourceEntity;
			EventData = eventData;
		}
	}
	public class EntityChangeReport
	{
		public List<EntityChangeEntry> ChangedEntities { get; }
		public List<DomainEventEntry> DomainEvents { get; }
		public EntityChangeReport()
		{
			ChangedEntities = new List<EntityChangeEntry>();
			DomainEvents = new List<DomainEventEntry>();
		}
		public bool IsEmpty()
		{
			return ChangedEntities.Count <= 0 && DomainEvents.Count <= 0;
		}
		public override string ToString()
		{
			return $"[EntityChangeReport] ChangedEntities: {ChangedEntities.Count}, DomainEvents: {DomainEvents.Count}";
		}
	}
	public interface IEntityChangeEventHelpher
	{
		void TriggerEvents(EntityChangeReport changeReport);
		Task TriggerEventsAsync(EntityChangeReport changeReport);
		void TriggerEntityCreatingEvent(object entity);
		void TriggerEntityCreatedEventOnUowCompleted(object entity);
		void TriggerEntityUpdatingEvent(object entity);
		void TriggerEntityUpdatedEventOnUowCompleted(object entity);
		void TriggerEntityDeletingEvent(object entity);
		void TriggerEntityDeletedEventOnUowCompleted(object entity);
	}
	public interface ILogger
	{
	}
	public class NullLogger : ILogger
	{
		public static readonly NullLogger Instance;
	}
	public class NullEntityChangeEventHelper : IEntityChangeEventHelpher
	{
		public static NullEntityChangeEventHelper Instance { get; } = new NullEntityChangeEventHelper();
		public void TriggerEntityCreatedEventOnUowCompleted(object entity)
		{
			throw new NotImplementedException();
		}

		public void TriggerEntityCreatingEvent(object entity)
		{
			throw new NotImplementedException();
		}

		public void TriggerEntityDeletedEventOnUowCompleted(object entity)
		{
			throw new NotImplementedException();
		}

		public void TriggerEntityDeletingEvent(object entity)
		{
			throw new NotImplementedException();
		}

		public void TriggerEntityUpdatedEventOnUowCompleted(object entity)
		{
			throw new NotImplementedException();
		}

		public void TriggerEntityUpdatingEvent(object entity)
		{
			throw new NotImplementedException();
		}

		public void TriggerEvents(EntityChangeReport changeReport)
		{
			throw new NotImplementedException();
		}

		public Task TriggerEventsAsync(EntityChangeReport changeReport)
		{
			throw new NotImplementedException();
		}
	}
	public interface IEventHandler
	{
	}
	public interface IEventHandler<in TEventData> : IEventHandler
	{
		void HandleEvent(IEventData eventData);
	}
	public interface IEventHandlerFactory
	{
		IEventHandler GetHandler();
		void ReleaseHandler(IEventHandler handler);
	}
	public interface IEventBus
	{
		#region Register

		/// <summary>
		/// Registers to an event.
		/// Given action is called for all event occurrences.
		/// </summary>
		/// <param name="action">Action to handle events</param>
		/// <typeparam name="TEventData">Event type</typeparam>
		IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData;

		/// <summary>
		/// Registers to an event. 
		/// Same (given) instance of the handler is used for all event occurrences.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="handler">Object to handle the event</param>
		IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

		/// <summary>
		/// Registers to an event.
		/// A new instance of <see cref="THandler"/> object is created for every event occurrence.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <typeparam name="THandler">Type of the event handler</typeparam>
		IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler<TEventData>, new();

		/// <summary>
		/// Registers to an event.
		/// Same (given) instance of the handler is used for all event occurrences.
		/// </summary>
		/// <param name="eventType">Event type</param>
		/// <param name="handler">Object to handle the event</param>
		IDisposable Register(Type eventType, IEventHandler handler);

		/// <summary>
		/// Registers to an event.
		/// Given factory is used to create/release handlers
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="handlerFactory">A factory to create/release handlers</param>
		IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData;

		/// <summary>
		/// Registers to an event.
		/// </summary>
		/// <param name="eventType">Event type</param>
		/// <param name="handlerFactory">A factory to create/release handlers</param>
		IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory);

		#endregion

		#region Unregister

		/// <summary>
		/// Unregisters from an event.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="action"></param>
		void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData;

		/// <summary>
		/// Unregisters from an event.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="handler">Handler object that is registered before</param>
		void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

		/// <summary>
		/// Unregisters from an event.
		/// </summary>
		/// <param name="eventType">Event type</param>
		/// <param name="handler">Handler object that is registered before</param>
		void Unregister(Type eventType, IEventHandler handler);

		/// <summary>
		/// Unregisters from an event.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="factory">Factory object that is registered before</param>
		void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData;

		/// <summary>
		/// Unregisters from an event.
		/// </summary>
		/// <param name="eventType">Event type</param>
		/// <param name="factory">Factory object that is registered before</param>
		void Unregister(Type eventType, IEventHandlerFactory factory);

		/// <summary>
		/// Unregisters all event handlers of given event type.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		void UnregisterAll<TEventData>() where TEventData : IEventData;

		/// <summary>
		/// Unregisters all event handlers of given event type.
		/// </summary>
		/// <param name="eventType">Event type</param>
		void UnregisterAll(Type eventType);

		#endregion

		#region Trigger

		/// <summary>
		/// Triggers an event.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="eventData">Related data for the event</param>
		void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData;

		/// <summary>
		/// Triggers an event.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="eventSource">The object which triggers the event</param>
		/// <param name="eventData">Related data for the event</param>
		void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

		/// <summary>
		/// Triggers an event.
		/// </summary>
		/// <param name="eventType">Event type</param>
		/// <param name="eventData">Related data for the event</param>
		void Trigger(Type eventType, IEventData eventData);

		/// <summary>
		/// Triggers an event.
		/// </summary>
		/// <param name="eventType">Event type</param>
		/// <param name="eventSource">The object which triggers the event</param>
		/// <param name="eventData">Related data for the event</param>
		void Trigger(Type eventType, object eventSource, IEventData eventData);

		/// <summary>
		/// Triggers an event asynchronously.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="eventData">Related data for the event</param>
		/// <returns>The task to handle async operation</returns>
		Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData;

		/// <summary>
		/// Triggers an event asynchronously.
		/// </summary>
		/// <typeparam name="TEventData">Event type</typeparam>
		/// <param name="eventSource">The object which triggers the event</param>
		/// <param name="eventData">Related data for the event</param>
		/// <returns>The task to handle async operation</returns>
		Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

		/// <summary>
		/// Triggers an event asynchronously.
		/// </summary>
		/// <param name="eventType">Event type</param>
		/// <param name="eventData">Related data for the event</param>
		/// <returns>The task to handle async operation</returns>
		Task TriggerAsync(Type eventType, IEventData eventData);

		/// <summary>
		/// Triggers an event asynchronously.
		/// </summary>
		/// <param name="eventType">Event type</param>
		/// <param name="eventSource">The object which triggers the event</param>
		/// <param name="eventData">Related data for the event</param>
		/// <returns>The task to handle async operation</returns>
		Task TriggerAsync(Type eventType, object eventSource, IEventData eventData);


		#endregion
	}
	public sealed class NullEventBus : IEventBus
	{
		/// <summary>
		/// Gets single instance of <see cref="NullEventBus"/> class.
		/// </summary>
		public static NullEventBus Instance { get { return SingletonInstance; } }
		private static readonly NullEventBus SingletonInstance = new NullEventBus();

		private NullEventBus()
		{
		}

		/// <inheritdoc/>
		public IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc/>
		public IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc/>
		public IDisposable Register<TEventData, THandler>()
			where TEventData : IEventData
			where THandler : IEventHandler<TEventData>, new()
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc/>
		public IDisposable Register(Type eventType, IEventHandler handler)
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc/>
		public IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc/>
		public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc/>
		public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
		{
		}

		/// <inheritdoc/>
		public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
		{
		}

		/// <inheritdoc/>
		public void Unregister(Type eventType, IEventHandler handler)
		{
		}

		/// <inheritdoc/>
		public void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData
		{
		}

		/// <inheritdoc/>
		public void Unregister(Type eventType, IEventHandlerFactory factory)
		{
		}

		/// <inheritdoc/>
		public void UnregisterAll<TEventData>() where TEventData : IEventData
		{
		}

		/// <inheritdoc/>
		public void UnregisterAll(Type eventType)
		{
		}

		/// <inheritdoc/>
		public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
		{
		}

		/// <inheritdoc/>
		public void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
		{
		}

		/// <inheritdoc/>
		public void Trigger(Type eventType, IEventData eventData)
		{
		}

		/// <inheritdoc/>
		public void Trigger(Type eventType, object eventSource, IEventData eventData)
		{
		}

		/// <inheritdoc/>
		public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
		{
			return new Task(() => { });
		}

		/// <inheritdoc/>
		public Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
		{
			return new Task(() => { });
		}

		/// <inheritdoc/>
		public Task TriggerAsync(Type eventType, IEventData eventData)
		{
			return new Task(() => { });
		}

		/// <inheritdoc/>
		public Task TriggerAsync(Type eventType, object eventSource, IEventData eventData)
		{
			return new Task(() => { });
		}
	}
	public interface IGuidGenerator
	{
		Guid Create();
	}
	public static class LockExtensions
	{
		/// <summary>
		/// Executes given <paramref name="action"/> by locking given <paramref name="source"/> object.
		/// </summary>
		/// <param name="source">Source object (to be locked)</param>
		/// <param name="action">Action (to be executed)</param>
		public static void Locking(this object source, Action action)
		{
			lock (source)
			{
				action();
			}
		}

		/// <summary>
		/// Executes given <paramref name="action"/> by locking given <paramref name="source"/> object.
		/// </summary>
		/// <typeparam name="T">Type of the object (to be locked)</typeparam>
		/// <param name="source">Source object (to be locked)</param>
		/// <param name="action">Action (to be executed)</param>
		public static void Locking<T>(this T source, Action<T> action) where T : class
		{
			lock (source)
			{
				action(source);
			}
		}

		/// <summary>
		/// Executes given <paramref name="func"/> and returns it's value by locking given <paramref name="source"/> object.
		/// </summary>
		/// <typeparam name="TResult">Return type</typeparam>
		/// <param name="source">Source object (to be locked)</param>
		/// <param name="func">Function (to be executed)</param>
		/// <returns>Return value of the <paramref name="func"/></returns>
		public static TResult Locking<TResult>(this object source, Func<TResult> func)
		{
			lock (source)
			{
				return func();
			}
		}

		/// <summary>
		/// Executes given <paramref name="func"/> and returns it's value by locking given <paramref name="source"/> object.
		/// </summary>
		/// <typeparam name="T">Type of the object (to be locked)</typeparam>
		/// <typeparam name="TResult">Return type</typeparam>
		/// <param name="source">Source object (to be locked)</param>
		/// <param name="func">Function (to be executed)</param>
		/// <returns>Return value of the <paramnref name="func"/></returns>
		public static TResult Locking<T, TResult>(this T source, Func<T, TResult> func) where T : class
		{
			lock (source)
			{
				return func(source);
			}
		}
	}
	public class SequentialGuidGenerator : IGuidGenerator
	{
		public static SequentialGuidGenerator Instance { get; } = new SequentialGuidGenerator();
		private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();
		public SequentialGuidDatabaseType DatabaseType { get; set; }
		private SequentialGuidGenerator()
		{
			DatabaseType = SequentialGuidDatabaseType.SqlServer;
		}

		public enum SequentialGuidDatabaseType
		{
			SqlServer,
			Oracle,
			MySql,
			PostgreSql,
		}
		public enum SequentialGuidType
		{
			/// <summary>
			/// The GUID should be sequential when formatted using the
			/// <see cref="Guid.ToString()" /> method.
			/// </summary>
			SequentialAsString,

			/// <summary>
			/// The GUID should be sequential when formatted using the
			/// <see cref="Guid.ToByteArray" /> method.
			/// </summary>
			SequentialAsBinary,

			/// <summary>
			/// The sequential portion of the GUID should be located at the end
			/// of the Data4 block.
			/// </summary>
			SequentialAtEnd
		}
		public Guid Create()
		{
			throw new NotImplementedException();
		}
		public Guid Create(SequentialGuidDatabaseType databaseType)
		{

			switch (databaseType)
			{
				case SequentialGuidDatabaseType.SqlServer:
					return Create(SequentialGuidType.SequentialAtEnd);
				case SequentialGuidDatabaseType.Oracle:
					return Create(SequentialGuidType.SequentialAsBinary);
				case SequentialGuidDatabaseType.MySql:
					return Create(SequentialGuidType.SequentialAsString);
				case SequentialGuidDatabaseType.PostgreSql:
					return Create(SequentialGuidType.SequentialAsString);
				default:
					throw new InvalidOperationException();
			}
		}
		public Guid Create(SequentialGuidType guidType)
		{
			var randomBytes = new byte[10];
			Rng.Locking(r => r.GetBytes(randomBytes));
			long timestamp = DateTime.UtcNow.Ticks / 10000L;
			byte[] timestampBytes = BitConverter.GetBytes(timestamp);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(timestampBytes);
			}
			byte[] guidBytes = new byte[16];
			switch (guidType)
			{
				case SequentialGuidType.SequentialAsString:
				case SequentialGuidType.SequentialAsBinary:
					Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
					Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
					if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
					{
						Array.Reverse(guidBytes, 0, 4);
						Array.Reverse(guidBytes, 4, 2);
					}

					break;
				case SequentialGuidType.SequentialAtEnd:

					// For sequential-at-the-end versions, we copy the random data first,
					// followed by the timestamp.
					Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
					Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
					break;
			}
			return new Guid(guidBytes);
		}
	}
	internal static class ReflectionHelper
	{
		/// <summary>
		/// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
		/// </summary>
		/// <param name="givenType">Type to check</param>
		/// <param name="genericType">Generic type</param>
		public static bool IsAssignableToGenericType(Type givenType, Type genericType)
		{
			var givenTypeInfo = givenType.GetTypeInfo();

			if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}

			foreach (var interfaceType in givenType.GetInterfaces())
			{
				if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
			}

			if (givenTypeInfo.BaseType == null)
			{
				return false;
			}

			return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
		}

		/// <summary>
		/// Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
		/// </summary>
		/// <param name="inherit">Inherit attribute from base classes</param>
		/// <param name="memberInfo">MemberInfo</param>
		public static List<object> GetAttributesOfMemberAndDeclaringType(MemberInfo memberInfo, bool inherit = true)
		{
			var attributeList = new List<object>();

			attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));

			if (memberInfo.DeclaringType != null)
			{
				attributeList.AddRange(memberInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(inherit));
			}

			return attributeList;
		}

		/// <summary>
		/// Gets a list of attributes defined for a class member and type including inherited attributes.
		/// </summary>
		/// <param name="memberInfo">MemberInfo</param>
		/// <param name="type">Type</param>
		/// <param name="inherit">Inherit attribute from base classes</param>
		public static List<object> GetAttributesOfMemberAndType(MemberInfo memberInfo, Type type, bool inherit = true)
		{
			var attributeList = new List<object>();
			attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));
			attributeList.AddRange(type.GetTypeInfo().GetCustomAttributes(inherit));
			return attributeList;
		}

		/// <summary>
		/// Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
		/// </summary>
		/// <typeparam name="TAttribute">Type of the attribute</typeparam>
		/// <param name="memberInfo">MemberInfo</param>
		/// <param name="inherit">Inherit attribute from base classes</param>
		public static List<TAttribute> GetAttributesOfMemberAndDeclaringType<TAttribute>(MemberInfo memberInfo, bool inherit = true)
			where TAttribute : Attribute
		{
			var attributeList = new List<TAttribute>();

			if (memberInfo.IsDefined(typeof(TAttribute), inherit))
			{
				attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
			}

			if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.GetTypeInfo().IsDefined(typeof(TAttribute), inherit))
			{
				attributeList.AddRange(memberInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
			}

			return attributeList;
		}

		/// <summary>
		/// Gets a list of attributes defined for a class member and type including inherited attributes.
		/// </summary>
		/// <typeparam name="TAttribute">Type of the attribute</typeparam>
		/// <param name="memberInfo">MemberInfo</param>
		/// <param name="type">Type</param>
		/// <param name="inherit">Inherit attribute from base classes</param>
		public static List<TAttribute> GetAttributesOfMemberAndType<TAttribute>(MemberInfo memberInfo, Type type, bool inherit = true)
			where TAttribute : Attribute
		{
			var attributeList = new List<TAttribute>();

			if (memberInfo.IsDefined(typeof(TAttribute), inherit))
			{
				attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
			}

			if (type.GetTypeInfo().IsDefined(typeof(TAttribute), inherit))
			{
				attributeList.AddRange(type.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
			}

			return attributeList;
		}

		/// <summary>
		/// Tries to gets an of attribute defined for a class member and it's declaring type including inherited attributes.
		/// Returns default value if it's not declared at all.
		/// </summary>
		/// <typeparam name="TAttribute">Type of the attribute</typeparam>
		/// <param name="memberInfo">MemberInfo</param>
		/// <param name="defaultValue">Default value (null as default)</param>
		/// <param name="inherit">Inherit attribute from base classes</param>
		public static TAttribute GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true)
			where TAttribute : class
		{
			return memberInfo.GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault()
				   ?? memberInfo.DeclaringType?.GetTypeInfo().GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault()
				   ?? defaultValue;
		}

		/// <summary>
		/// Tries to gets an of attribute defined for a class member and it's declaring type including inherited attributes.
		/// Returns default value if it's not declared at all.
		/// </summary>
		/// <typeparam name="TAttribute">Type of the attribute</typeparam>
		/// <param name="memberInfo">MemberInfo</param>
		/// <param name="defaultValue">Default value (null as default)</param>
		/// <param name="inherit">Inherit attribute from base classes</param>
		public static TAttribute GetSingleAttributeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true)
			where TAttribute : Attribute
		{
			//Get attribute on the member
			if (memberInfo.IsDefined(typeof(TAttribute), inherit))
			{
				return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First();
			}

			return defaultValue;
		}

		/// <summary>
		/// Gets value of a property by it's full path from given object
		/// </summary>
		/// <param name="obj">Object to get value from</param>
		/// <param name="objectType">Type of given object</param>
		/// <param name="propertyPath">Full path of property</param>
		/// <returns></returns>
		internal static object GetValueByPath(object obj, Type objectType, string propertyPath)
		{
			var value = obj;
			var currentType = objectType;
			var objectPath = currentType.FullName;
			var absolutePropertyPath = propertyPath;
			if (absolutePropertyPath.StartsWith(objectPath))
			{
				absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
			}

			foreach (var propertyName in absolutePropertyPath.Split('.'))
			{
				var property = currentType.GetProperty(propertyName);
				value = property.GetValue(value, null);
				currentType = property.PropertyType;
			}

			return value;
		}

		/// <summary>
		/// Sets value of a property by it's full path on given object
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="objectType"></param>
		/// <param name="propertyPath"></param>
		/// <param name="value"></param>
		internal static void SetValueByPath(object obj, Type objectType, string propertyPath, object value)
		{
			var currentType = objectType;
			PropertyInfo property;
			var objectPath = currentType.FullName;
			var absolutePropertyPath = propertyPath;
			if (absolutePropertyPath.StartsWith(objectPath))
			{
				absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
			}

			var properties = absolutePropertyPath.Split('.');

			if (properties.Length == 1)
			{
				property = objectType.GetProperty(properties.First());
				property.SetValue(obj, value);
				return;
			}

			for (int i = 0; i < properties.Length - 1; i++)
			{
				property = currentType.GetProperty(properties[i]);
				obj = property.GetValue(obj, null);
				currentType = property.PropertyType;
			}

			property = currentType.GetProperty(properties.Last());
			property.SetValue(obj, value);
		}
	}
	public interface IHasCreationTime
	{
		/// <summary>
		/// Creation time of this entity.
		/// </summary>
		DateTime CreationTime { get; set; }
	}
	public static class EntityAuditingHelper
	{
		public static void SetCreationAuditProperties(
			IMultiTenancyConfig multiTenancyConfig,
			object entityAsObj,
			int? tenantId,
			long? userId)
		{
			var entityWithCreationTime = entityAsObj as IHasCreationTime;
			if (entityWithCreationTime == null)
			{
				//Object does not implement IHasCreationTime
				return;
			}

			if (entityWithCreationTime.CreationTime == default(DateTime))
			{
				//entityWithCreationTime.CreationTime = Clock.Now;
			}

			//if (!(entityAsObj is ICreationAudited))
			//{
			//	//Object does not implement ICreationAudited
			//	return;
			//}

			if (!userId.HasValue)
			{
				//Unknown user
				return;
			}

			//var entity = entityAsObj as ICreationAudited;
			if (entity.CreatorUserId != null)
			{
				//CreatorUserId is already set
				return;
			}

			if (multiTenancyConfig?.IsEnabled == true)
			{
				//if (MultiTenancyHelper.IsMultiTenantEntity(entity) &&
				//	!MultiTenancyHelper.IsTenantEntity(entity, tenantId))
				//{
				//	//A tenant entitiy is created by host or a different tenant
				//	return;
				//}

				//if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
				//{
				//	//Tenant user created a host entity
				//	return;
				//}
			}

			//Finally, set CreatorUserId!
			entity.CreatorUserId = userId;
		}

		public static void SetModificationAuditProperties(
			IMultiTenancyConfig multiTenancyConfig,
			object entityAsObj,
			int? tenantId,
			long? userId)
		{
			//if (entityAsObj is IHasModificationTime)
			//{
			//	entityAsObj.As<IHasModificationTime>().LastModificationTime = Clock.Now;
			//}

			//if (!(entityAsObj is IModificationAudited))
			//{
			//	//Entity does not implement IModificationAudited
			//	return;
			//}

			//	var entity = entityAsObj.As<IModificationAudited>();

			if (userId == null)
			{
				//Unknown user
				//	entity.LastModifierUserId = null;
				return;
			}

			if (multiTenancyConfig?.IsEnabled == true)
			{
				//if (MultiTenancyHelper.IsMultiTenantEntity(entity) &&
				//	!MultiTenancyHelper.IsTenantEntity(entity, tenantId))
				//{
				//	//A tenant entitiy is modified by host or a different tenant
				//	entity.LastModifierUserId = null;
				//	return;
				//}

				//if (tenantId.HasValue && MultiTenancyHelper.IsHostEntity(entity))
				//{
				//	//Tenant user modified a host entity
				//	entity.LastModifierUserId = null;
				//	return;
				//}
			}

			//Finally, set LastModifierUserId!
			//entity.LastModifierUserId = userId;
		}
	}
	public interface IHasDeletionTime : ISoftDelete
	{
		/// <summary>
		/// Deletion time of this entity.
		/// </summary>
		DateTime? DeletionTime { get; set; }
	}
	public interface IDeletionAudited : IHasDeletionTime
	{
		/// <summary>
		/// Which user deleted this entity?
		/// </summary>
		long? DeleterUserId { get; set; }
	}

	/// <summary>
	/// Adds navigation properties to <see cref="IDeletionAudited"/> interface for user.
	/// </summary>
	/// <typeparam name="TUser">Type of the user</typeparam>
	public interface IDeletionAudited<TUser> : IDeletionAudited
		where TUser : IEntity<long>
	{
		/// <summary>
		/// Reference to the deleter user of this entity.
		/// </summary>
		TUser DeleterUser { get; set; }
	}
	public interface IAggregateRoot : IAggregateRoot<int>, IEntity
	{

	}
	public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>, IGeneratesDomainEvents
	{
	}
	public interface IGeneratesDomainEvents
	{
		ICollection<IEventData> DomainEvents { get; }
	}
	public abstract class AbpDbContext : DbContext
	{
		public IAbpSession AbpSession { get; set; }
		public IEntityChangeEventHelpher EntityChangeEventHelper { get; set; }
		public ILogger Logger { get; set; }
		public IEventBus EventBus { get; set; }
		public IGuidGenerator GuidGenerator { get; set; }
		public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }
		public IMultiTenancyConfig MultiTenancyConfig { get; set; }
		public bool SuppressAutoSetTenantId { get; set; }
		protected AbpDbContext()
		{
			InitializeDbContext();
		}
		protected AbpDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			InitializeDbContext();
		}
		protected AbpDbContext(DbCompiledModel model)
		 : base(model)
		{
			InitializeDbContext();
		}
		private void InitializeDbContext()
		{
			SetNullsForInjectedProperties();
			RegisterToChanges();
		}
		protected AbpDbContext(DbConnection existingConnection, bool contextOwnsConnection)
		   : base(existingConnection, contextOwnsConnection)
		{
			InitializeDbContext();
		}
		protected AbpDbContext(string nameOrConnectionString, DbCompiledModel model)
			: base(nameOrConnectionString, model)
		{
			InitializeDbContext();
		}
		protected AbpDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
		   : base(objectContext, dbContextOwnsObjectContext)
		{
			InitializeDbContext();
		}
		protected AbpDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
		   : base(existingConnection, model, contextOwnsConnection)
		{
			InitializeDbContext();
		}
		public virtual void Initialize()
		{
			Database.Initialize(false);
			this.SetFilterScopedParameterValue(AbpDataFilters.MustHaveTenant, AbpDataFilters.Parameters.TenantId, AbpSession.TenantId ?? 0);
			this.SetFilterScopedParameterValue(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, AbpSession.TenantId);
		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Filter(AbpDataFilters.SoftDelete, (ISoftDelete d) => d.IsDeleted, false);
			modelBuilder.Filter(AbpDataFilters.MustHaveTenant, (IMustHaveTenant t, int tenantId) => t.TenantId == tenantId || (int?)t.TenantId == null, 0); //While "(int?)t.TenantId == null" seems wrong, it's needed. See https://github.com/jcachat/EntityFramework.DynamicFilters/issues/62#issuecomment-208198058
			modelBuilder.Filter(AbpDataFilters.MayHaveTenant, (IMayHaveTenant t, int? tenantId) => t.TenantId == tenantId, 0);
		}
		private void RegisterToChanges()
		{
			((IObjectContextAdapter)this)
			 .ObjectContext
			 .ObjectStateManager
			 .ObjectStateManagerChanged += ObjectStateManager_ObjectStateManagerChanged;
		}
		public override int SaveChanges()
		{
			try
			{
				var changedEntities = ApplyAbpConcepts();
				var result = base.SaveChanges();
				EntityChangeEventHelper.TriggerEvents(changedEntities);
				return result;
			}
			catch (DbEntityValidationException ex)
			{
				LogDbEntityValidationException(ex);
				throw;
			}
		}
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			try
			{
				var changeReport = ApplyAbpConcepts();
				var result = await base.SaveChangesAsync(cancellationToken);
				await EntityChangeEventHelper.TriggerEventsAsync(changeReport);
				return result;
			}
			catch (DbEntityValidationException ex)
			{
				LogDbEntityValidationException(ex);
				throw;
			}
		}
		protected virtual EntityChangeReport ApplyAbpConcepts()
		{
			var changeReport = new EntityChangeReport();
			var userId = GetAuditUserId();
			var entries = ChangeTracker.Entries().ToList();
			foreach (var entry in entries)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						CheckAndSetId(entry.Entity);
						CheckAndSetMustHaveTenantIdProperty(entry.Entity);
						CheckAndSetMayHaveTenantIdProperty(entry.Entity);
						SetCreationAuditProperties(entry.Entity, userId);
						changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Created));
						break;
					case EntityState.Modified:
						SetModificationAuditProperties(entry.Entity, userId);
						if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
						{
							SetDeletionAuditProperties(entry.Entity, userId);
							changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
						}
						else
						{
							changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Updated));
						}
						break;
					case EntityState.Deleted:
						CancelDeletionForSoftDelete(entry);
						SetDeletionAuditProperties(entry.Entity, userId);
						changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
						break;
				}
				AddDomainEvents(changeReport.DomainEvents, entry.Entity);
			}
			return changeReport;
		}
		protected virtual void CheckAndSetMayHaveTenantIdProperty(object entityAsObj)
		{
			if (SuppressAutoSetTenantId)
			{
				return;
			}

			//Only set IMayHaveTenant entities
			if (!(entityAsObj is IMayHaveTenant))
			{
				return;
			}

			var entity = entityAsObj.As<IMayHaveTenant>();

			//Don't set if it's already set
			if (entity.TenantId != null)
			{
				return;
			}

			//Only works for single tenant applications
			if (MultiTenancyConfig?.IsEnabled ?? false)
			{
				return;
			}

			//Don't set if MayHaveTenant filter is disabled
			if (!this.IsFilterEnabled(AbpDataFilters.MayHaveTenant))
			{
				return;
			}

			entity.TenantId = GetCurrentTenantIdOrNull();
		}
		protected virtual void SetModificationAuditProperties(object entityAsObj, long? userId)
		{
			EntityAuditingHelper.SetModificationAuditProperties(MultiTenancyConfig, entityAsObj, AbpSession.TenantId, userId);
		}
		protected virtual void ObjectStateManager_ObjectStateManagerChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
		{
			var contextAdapter = (IObjectContextAdapter)this;
			if (e.Action != CollectionChangeAction.Add)
			{
				return;
			}

			var entry = contextAdapter.ObjectContext.ObjectStateManager.GetObjectStateEntry(e.Element);
			switch (entry.State)
			{
				case EntityState.Added:
					CheckAndSetId(entry.Entity);
					CheckAndSetMustHaveTenantIdProperty(entry.Entity);
					SetCreationAuditProperties(entry.Entity, GetAuditUserId());
					break;
					//case EntityState.Deleted: //It's not going here at all
					//    SetDeletionAuditProperties(entry.Entity, GetAuditUserId());
					//    break;
			}
		}
		protected virtual void CheckAndSetId(object entityAsObj)
		{
			//Set GUID Ids
			var entity = entityAsObj as IEntity<Guid>;
			if (entity != null && entity.Id == Guid.Empty)
			{
				var entityType = ObjectContext.GetObjectType(entityAsObj.GetType());
				var idProperty = entityType.GetProperty("Id");
				var dbGeneratedAttr = ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
				if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
				{
					entity.Id = GuidGenerator.Create();
				}
			}
		}
		protected virtual void CheckAndSetMustHaveTenantIdProperty(object entityAsObj)
		{
			if (SuppressAutoSetTenantId)
			{
				return;
			}

			//Only set IMustHaveTenant entities
			if (!(entityAsObj is IMustHaveTenant))
			{
				return;
			}

			var entity = entityAsObj.As<IMustHaveTenant>();

			//Don't set if it's already set
			if (entity.TenantId != 0)
			{
				return;
			}

			var currentTenantId = GetCurrentTenantIdOrNull();

			if (currentTenantId != null)
			{
				entity.TenantId = currentTenantId.Value;
			}
			else
			{
				throw new Exception("Can not set TenantId to 0 for IMustHaveTenant entities!");
			}
		}
		protected virtual void SetCreationAuditProperties(object entityAsObj, long? userId)
		{
			EntityAuditingHelper.SetCreationAuditProperties(MultiTenancyConfig, entityAsObj, AbpSession.TenantId, userId);
		}
		protected virtual void SetDeletionAuditProperties(object entityAsObj, long? userId)
		{
			if (entityAsObj is IHasDeletionTime)
			{
				var entity = entityAsObj.As<IHasDeletionTime>();

				if (entity.DeletionTime == null)
				{
					//	entity.DeletionTime = Clock.Now;
				}
			}

			if (entityAsObj is IDeletionAudited)
			{
				var entity = entityAsObj.As<IDeletionAudited>();

				if (entity.DeleterUserId != null)
				{
					return;
				}

				if (userId == null)
				{
					entity.DeleterUserId = null;
					return;
				}

				//Special check for multi-tenant entities
				if (entity is IMayHaveTenant || entity is IMustHaveTenant)
				{
					//Sets LastModifierUserId only if current user is in same tenant/host with the given entity
					if ((entity is IMayHaveTenant && entity.As<IMayHaveTenant>().TenantId == AbpSession.TenantId) ||
						(entity is IMustHaveTenant && entity.As<IMustHaveTenant>().TenantId == AbpSession.TenantId))
					{
						entity.DeleterUserId = userId;
					}
					else
					{
						entity.DeleterUserId = null;
					}
				}
				else
				{
					entity.DeleterUserId = userId;
				}
			}
		}
		protected virtual int? GetCurrentTenantIdOrNull()
		{
			if (CurrentUnitOfWorkProvider?.Current != null)
			{
				return CurrentUnitOfWorkProvider.Current.GetTenantId();
			}

			return AbpSession.TenantId;
		}
		protected virtual long? GetAuditUserId()
		{
			if (AbpSession.UserId.HasValue &&
				CurrentUnitOfWorkProvider != null &&
				CurrentUnitOfWorkProvider.Current != null &&
				CurrentUnitOfWorkProvider.Current.GetTenantId() == AbpSession.TenantId)
			{
				return AbpSession.UserId;
			}

			return null;
		}
		private void SetNullsForInjectedProperties()
		{
			Logger = NullLogger.Instance;
			AbpSession = NullAbpSession.Instance;
			EntityChangeEventHelpher = NullEntityChangeEventHelper.Instance;
			GuidGenerator = SequentialGuidGenerator.Instance;
			EventBus = NullEventBus.Instance;
		}
		protected virtual void CancelDeletionForSoftDelete(DbEntityEntry entry)
		{
			if (!(entry.Entity is ISoftDelete))
			{
				return;
			}

			var softDeleteEntry = entry.Cast<ISoftDelete>();
			softDeleteEntry.Reload();
			softDeleteEntry.State = EntityState.Modified;
			softDeleteEntry.Entity.IsDeleted = true;
		}
		protected virtual void AddDomainEvents(List<DomainEventEntry> domainEvents, object entityAsObj)
		{
			var generatesDomainEventsEntity = entityAsObj as IGeneratesDomainEvents;
			if (generatesDomainEventsEntity == null)
			{
				return;
			}

			if (generatesDomainEventsEntity.DomainEvents.IsNullOrEmpty())
			{
				return;
			}

			domainEvents.AddRange(generatesDomainEventsEntity.DomainEvents.Select(eventData => new DomainEventEntry(entityAsObj, eventData)));
			generatesDomainEventsEntity.DomainEvents.Clear();
		}
		protected virtual void LogDbEntityValidationException(DbEntityValidationException exception)
		{
			//Logger.Error("There are some validation errors while saving changes in EntityFramework:");
			foreach (var ve in exception.EntityValidationErrors.SelectMany(eve => eve.ValidationErrors))
			{
				//Logger.Error(" - " + ve.PropertyName + ": " + ve.ErrorMessage);
			}
		}
	}
}
