using EfResposity.Model;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.DynamicFilters;
using System.Data.Common;

namespace EfResposity
{
	//public class EfResposity<TDbContext, TEntity, TPrimaryKey> : IResposity<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
	//	where TDbContext : DbContext
	//{
	//	public TDbContext DbContext { get; private set; }

	//	public EfResposity(TDbContext dbContext)
	//	{
	//		DbContext = dbContext;
	//	}
	//	public int Count()
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public int Count(Expression<Func<TEntity, bool>> condition)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<int> CountAsync()
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<int> CountAsync(Expression<Func<TEntity, bool>> condition)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public bool Delete(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public bool Delete(TPrimaryKey id)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public bool Delete(Predicate<TEntity> predicate)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<bool> DeleteAsync(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<bool> DeleteAsync(TPrimaryKey id)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<bool> DeleteAsync(Predicate<TEntity> predicate)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public TEntity FirstOfDefault(TPrimaryKey id)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public TEntity FirstOfDefault(Expression<Func<TEntity, bool>> condition)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<TEntity> FirstOfDefaultAsync(TPrimaryKey id)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<TEntity> FirstOfDefaultAsync(Expression<Func<TEntity, bool>> condition)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public TEntity Get(TPrimaryKey id)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public IQueryable<TEntity> GetAll()
	//	{
	//		return DbContext.Set<TEntity>().AsQueryable();
	//	}

	//	public IQueryable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> condition)
	//	{
	//		return DbContext.Set<TEntity>().Where(condition);
	//	}

	//	public List<TEntity> GetAllList()
	//	{
	//		return GetAll().ToList();
	//	}

	//	public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> condition)
	//	{
	//		return GetAll().Where(condition).ToList();
	//	}

	//	public Task<List<TEntity>> GetAllListAsync()
	//	{
	//		return Task.FromResult<List<TEntity>>(GetAll().ToList());
	//	}

	//	public Task<TEntity> GetAsync(TPrimaryKey id)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public bool Insert(TEntity entity)
	//	{
	//		try
	//		{
	//			DbContext.Set<TEntity>().Add(entity);
	//			DbContext.SaveChanges();
	//			return true;
	//		}
	//		catch (Exception ex)
	//		{
	//			return false;
	//		}
	//	}

	//	public TPrimaryKey InsertAndGetId(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<bool> InsertAsync(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<bool> InsertOrUpdate(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<bool> InsertOrUpdateAsync(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public TEntity Load(TPrimaryKey id)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public long LongCount()
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public long LongCount(Expression<Func<TEntity, bool>> condition)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<long> LongCountAsync()
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> condition)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public TEntity Signle(Expression<Func<TEntity, bool>> condition)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public bool Update(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public bool Update(TPrimaryKey id, Action<TEntity> updateAction)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<bool> UpdateAsync(TEntity entity)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}
	//public class EfResposity<TDbContext, TEntity> : EfResposity<TDbContext, TEntity, int> where TEntity : class, IEntity<int>
	//	where TDbContext : DbContext
	//{
	//	public EfResposity(TDbContext dbContext) : base(dbContext)
	//	{
	//	}
	//}
	//public class TestEfResposity
	//{
	//	public void Test()
	//	{
	//		var resposity = new EfResposity<DbContext, Ef_User>(new DbResposityContext());
	//		var random = new Random();
	//		var md5 = MD5.Create();
	//		var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes("123456"));
	//		StringBuilder sb = new StringBuilder();
	//		foreach (var item in bytes)
	//		{
	//			sb.Append(item.ToString("x2"));
	//		}
	//		resposity.Insert(new Ef_User()
	//		{
	//			UserName = "Colin" + random.Next(1, 100),
	//			Password = sb.ToString()
	//		});
	//		(resposity.GetAll().Where(x => x.UserName.Contains("x")) ?? null).ForEachAsync(x =>
	//		{
	//			Console.WriteLine($"UserName:{x.UserName}");
	//		}).Wait();
	//	}
	//}

	public class UnitOfWorkFailedEventArgs : EventArgs
	{
		public Exception Exception { get; private set; }

		public UnitOfWorkFailedEventArgs(Exception exception)
		{
			Exception = exception;
		}
	}
	public enum TransactionScopeOption
	{
		Required,
		RequiresNew,
		Suppress,
	}
	public enum IsolationLevel
	{
		/// <summary>
		/// 避免脏读、不可重复读，幻读
		/// </summary>
		Serializable,
		/// <summary>
		/// 避免脏读
		/// </summary>
		ReadCommitted,
		/// <summary>
		/// 避免脏读、不可重复读
		/// </summary>
		RepeatableRead,
		/// <summary>
		/// 什么都避免不了
		/// </summary>
		ReadUncommitted,
	}
	public enum TransactionScopeAsyncFlowOption
	{
		Suppress,
		Enabled,
	}
	public class DataFilterConfiguration
	{
		public string FilterName { get; set; }
		public bool IsEnabled { get; set; }
		public IDictionary<string, object> FilterParameters { get; set; }
		public DataFilterConfiguration(string filterName, bool isEnabled)
		{
			FilterName = filterName;
			IsEnabled = isEnabled;
		}

		public DataFilterConfiguration(DataFilterConfiguration filterToClone, bool? isEnabled = null) : this
			(filterToClone.FilterName, isEnabled ?? filterToClone.IsEnabled)
		{
			foreach (var item in filterToClone.FilterParameters)
			{
				FilterParameters[item.Key] = item.Value;
			}
		}
	}
	public interface IUnitOfWorkDefaultOptions
	{
		TransactionScopeOption Scope { get; set; }
		bool IsTransaction { get; set; }
		TimeSpan? TimeOut { get; set; }
		IsolationLevel? IsolationLevel { get; set; }
		IReadOnlyCollection<DataFilterConfiguration> Filters { get; }
		void RegisterFilter(string filterName, bool isEnabledByDefault);
		void OverrideFilter(string filterName, bool IsEnabledByDefault);
	}
	public class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions
	{
		public TransactionScopeOption Scope { get; set; }
		public bool IsTransaction { get; set; }
		public TimeSpan? TimeOut { get; set; }
		public IsolationLevel? IsolationLevel { get; set; }
		public IReadOnlyCollection<DataFilterConfiguration> Filters => _filters;
		private readonly List<DataFilterConfiguration> _filters;
		public UnitOfWorkDefaultOptions()
		{
			_filters = new List<DataFilterConfiguration>();
			IsTransaction = true;
			Scope = TransactionScopeOption.Required;
		}

		public void OverrideFilter(string filterName, bool IsEnabledByDefault)
		{
			_filters.RemoveAll(f => f.FilterName == filterName);
			_filters.Add(new DataFilterConfiguration(filterName, IsEnabledByDefault));
		}

		public void RegisterFilter(string filterName, bool isEnabledByDefault)
		{
			if (_filters.Any(f => f.FilterName == filterName))
				throw new Exception($"There is already a filter with name:{filterName}");
			_filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
		}
	}
	public class UnitOfWorkAttribute : Attribute
	{
		public TransactionScopeOption? Scope { get; set; }
		public bool? IsTransactional { get; set; }
		public TimeSpan? Timeout { get; set; }
		public IsolationLevel? IsolationLevel { get; set; }
		public bool IsDisabled { get; set; }
		public UnitOfWorkAttribute() { }
		public UnitOfWorkAttribute(bool isTransactional)
		{
			IsTransactional = isTransactional;
		}
		public UnitOfWorkAttribute(int timeout)
		{
			Timeout = TimeSpan.FromMilliseconds(timeout);
		}
		public UnitOfWorkAttribute(bool isTransactional, int timeout)
		{
			IsTransactional = isTransactional;
			Timeout = TimeSpan.FromMilliseconds(timeout);
		}
		public UnitOfWorkAttribute(TransactionScopeOption scope, bool isTransactional)
		{
			Scope = scope;
			IsTransactional = isTransactional;
		}
		public UnitOfWorkAttribute(TransactionScopeOption scope, int timeout)
		{
			IsTransactional = true;
			Scope = scope;
			Timeout = TimeSpan.FromMilliseconds(timeout);
		}
		public UnitOfWorkAttribute CreatOptions()
		{
			return new UnitOfWorkAttribute()
			{
				IsTransactional = IsTransactional,
				IsolationLevel = IsolationLevel,
				Timeout = Timeout,
				Scope = Scope
			};
		}
	}
	public static class UnitOfWorkDefaultOptionsExtensions
	{
		public static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(this IUnitOfWorkDefaultOptions unitOfWorkDefaultOptions, MethodInfo methodInfo)
		{
			var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
			if (attrs.Length > 0)
				return attrs[0];
			attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
			if (attrs.Length > 0)
				return attrs[0];
			return null;
		}
	}
	public class UnitOfWorkOptions
	{
		public TransactionScopeOption? Scope { get; set; }
		public bool? IsTransactional { get; set; }
		public TimeSpan? Timeout { get; set; }
		public IsolationLevel? IsolationLevel { get; set; }
		public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }
		public List<DataFilterConfiguration> FilterOverrides { get; }
		public UnitOfWorkOptions()
		{
			FilterOverrides = new List<DataFilterConfiguration>();
		}
		public void FIllDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions)
		{
			if (!IsTransactional.HasValue)
				IsTransactional = defaultOptions.IsTransaction;
			if (!Scope.HasValue)
				Scope = defaultOptions.Scope;
			if (!Timeout.HasValue && !defaultOptions.TimeOut.HasValue)
				Timeout = defaultOptions.TimeOut.Value;
			if (!IsolationLevel.HasValue && !defaultOptions.IsolationLevel.HasValue)
				IsolationLevel = defaultOptions.IsolationLevel.Value;
		}
		public void FillOutUowFiltersForNonProvidedOptions(List<DataFilterConfiguration> filterOverrides)
		{
			foreach (var item in filterOverrides)
			{
				if (FilterOverrides.Any(fo => fo.FilterName == item.FilterName))
					continue;
				FilterOverrides.Add(item);
			}
		}
	}
	public interface IActiveUnitOfWork
	{
		event EventHandler Completed;
		event EventHandler<UnitOfWorkFailedEventArgs> Failed;
		event EventHandler Disposed;
		UnitOfWorkOptions Options { get; }
		IReadOnlyList<DataFilterConfiguration> Filters { get; }
		bool IsDisposed { get; }
		void SaveChanges();
		Task SaveChangesAsync();
		IDisposable DisableFilter(params string[] filterNames);
		IDisposable EnableFilter(params string[] filterNames);
		bool IsFilterEnabled(string filterName);
		IDisposable SetFilterParameter(string filterName, string parameterName, object value);
		IDisposable SetTenantId(int? tenantId);
		IDisposable SetTenantId(int? tenatId, bool switchMustHaveTenateEnableDisable);
		int? GetTenantId();

	}
	public interface IUnitOfWorkCompleteHandle : IDisposable
	{
		void Complete();
		Task CompleteAsync();
	}
	public interface IUnitOfWork : IActiveUnitOfWork, IUnitOfWorkCompleteHandle
	{
		string Id { get; }
		IUnitOfWork Outer { get; set; }
		void Begin(UnitOfWorkOptions options);
	}
	public enum MultiTenancySides
	{
		Tenant = 1,
		Host = 2
	}
	public interface IAbpSession
	{
		long? UserId { get; }
		int? TenantId { get; }
		MultiTenancySides MultiTenancySide { get; }
		long? ImpersonatorUserId { get; }
		int? ImpersonatorTenantId { get; }
		IDisposable Use(int? tenantId, long? userId);
	}
	public interface ITenantResolveContributor
	{
		int? ResolveTenantId();
	}
	public interface IMultiTenancyConfig
	{
		bool IsEnabled { get; set; }
		ITypeList<ITenantResolveContributor> Resolvers { get; }

	}
	public class TypeList : TypeList<object>, ITypeList
	{
	}
	public class TypeList<TBaseType> : ITypeList<TBaseType>
	{
		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count { get { return _typeList.Count; } }

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly { get { return false; } }

		/// <summary>
		/// Gets or sets the <see cref="Type"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public Type this[int index]
		{
			get { return _typeList[index]; }
			set
			{
				CheckType(value);
				_typeList[index] = value;
			}
		}

		private readonly List<Type> _typeList;

		/// <summary>
		/// Creates a new <see cref="TypeList{T}"/> object.
		/// </summary>
		public TypeList()
		{
			_typeList = new List<Type>();
		}

		/// <inheritdoc/>
		public void Add<T>() where T : TBaseType
		{
			_typeList.Add(typeof(T));
		}

		/// <inheritdoc/>
		public void Add(Type item)
		{
			CheckType(item);
			_typeList.Add(item);
		}

		/// <inheritdoc/>
		public void Insert(int index, Type item)
		{
			_typeList.Insert(index, item);
		}

		/// <inheritdoc/>
		public int IndexOf(Type item)
		{
			return _typeList.IndexOf(item);
		}

		/// <inheritdoc/>
		public bool Contains<T>() where T : TBaseType
		{
			return Contains(typeof(T));
		}

		/// <inheritdoc/>
		public bool Contains(Type item)
		{
			return _typeList.Contains(item);
		}

		/// <inheritdoc/>
		public void Remove<T>() where T : TBaseType
		{
			_typeList.Remove(typeof(T));
		}

		/// <inheritdoc/>
		public bool Remove(Type item)
		{
			return _typeList.Remove(item);
		}

		/// <inheritdoc/>
		public void RemoveAt(int index)
		{
			_typeList.RemoveAt(index);
		}

		/// <inheritdoc/>
		public void Clear()
		{
			_typeList.Clear();
		}

		/// <inheritdoc/>
		public void CopyTo(Type[] array, int arrayIndex)
		{
			_typeList.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc/>
		public IEnumerator<Type> GetEnumerator()
		{
			return _typeList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _typeList.GetEnumerator();
		}

		private static void CheckType(Type item)
		{
			if (!typeof(TBaseType).GetTypeInfo().IsAssignableFrom(item))
			{
				throw new ArgumentException("Given item is not type of " + typeof(TBaseType).AssemblyQualifiedName, "item");
			}
		}
	}
	public class MultiTenancyConfig : IMultiTenancyConfig
	{
		public bool IsEnabled { get; set; }

		public ITypeList<ITenantResolveContributor> Resolvers { get; }

		public MultiTenancyConfig()
		{
			Resolvers = new TypeList<ITenantResolveContributor>();
		}
	}
	public interface ITypeList : ITypeList<object>
	{
	}
	public interface ITypeList<in TBaseType> : IList<Type>
	{
		void Add<T>() where T : TBaseType;
		bool Contains<T>() where T : TBaseType;
		void Remove<T>() where T : TBaseType;
	}
	public class SessionOverride
	{
		public long? UserId { get; }
		public int? TenantId { get; }
		public SessionOverride(int? tenantId, long? userId)
		{
			TenantId = tenantId;
			UserId = userId;
		}
	}
	public interface IAmbientScopeProvider<T>
	{
		T GetValue(string contextKey);
		IDisposable BeginScope(string contextKey, T value);
	}
	public abstract class AbpSessionBase : IAbpSession
	{
		public const string SessionOverrideContextKey = "Abp.Runtime.Session.Override";
		public IMultiTenancyConfig MultiTenancy { get; }
		public abstract long? UserId { get; }
		public abstract int? TenantId { get; }
		public abstract long? ImpersonatorUserId { get; }
		public abstract int? ImpersonatorTenantId { get; }
		public virtual MultiTenancySides MultiTenancySide
		{
			get
			{
				return MultiTenancy.IsEnabled && !TenantId.HasValue ? MultiTenancySides.Host : MultiTenancySides.Tenant;
			}
		}
		protected SessionOverride OverridedValue => SessionOverrideScopeProvider.GetValue(SessionOverrideContextKey);
		protected IAmbientScopeProvider<SessionOverride> SessionOverrideScopeProvider { get; }
		public AbpSessionBase(IMultiTenancyConfig multiTenancy, IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
		{
			MultiTenancy = multiTenancy;
			SessionOverrideScopeProvider = sessionOverrideScopeProvider;
		}
		public IDisposable Use(int? tenantId, long? userId)
		{
			return SessionOverrideScopeProvider.BeginScope(SessionOverrideContextKey, new SessionOverride(tenantId, userId));
		}
	}
	public class DataContextAmbientScopeProvider<T> : IAmbientScopeProvider<T>
	{
		public DataContextAmbientScopeProvider(IAmbientDataContext dataContext)
		{
			//Check.NotNull(dataContext, nameof(dataContext));
			//
			//_dataContext = dataContext;

			//Logger = NullLogger.Instance;
		}
		public IDisposable BeginScope(string contextKey, T value)
		{
			throw new NotImplementedException();
		}

		public T GetValue(string contextKey)
		{
			throw new NotImplementedException();
		}
	}
	public interface IAmbientDataContext
	{
		void SetData(string key, object value);
		object GetData(string key);
	}
	public class AsyncLocalAmbientDataContext : IAmbientDataContext
	{
		private static readonly ConcurrentDictionary<string, AsyncLocal<object>> AsyncLocalDictionary =
			new ConcurrentDictionary<string, AsyncLocal<object>>();
		public object GetData(string key)
		{
			var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
			return asyncLocal.Value;
		}

		public void SetData(string key, object value)
		{
			var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
			asyncLocal.Value = value;
		}
	}
	public class NullAbpSession : AbpSessionBase
	{
		public static NullAbpSession Instance { get; } = new NullAbpSession();

		public override long? UserId => null;

		public override int? TenantId => null;
		public override MultiTenancySides MultiTenancySide => MultiTenancySides.Tenant;

		public override long? ImpersonatorUserId => null;
		public override int? ImpersonatorTenantId => null;
		private NullAbpSession() : base(new MultiTenancyConfig(), new DataContextAmbientScopeProvider<SessionOverride>(new AsyncLocalAmbientDataContext()))
		{
		}
	}
	public interface IUnitOfWorkFilterExecuter
	{
		void ApplyDisableFilter(IUnitOfWork unitOfWork, string filterName);
		void ApplyEnableFilter(IUnitOfWork unitOfWork, string filterName);
		void ApplyFilterParmeterValue(IUnitOfWork unitOfWork, string filterName, string paramterName, object value);
	}
	public class ConnectionStringResolveArgs : Dictionary<string, object>
	{
		public MultiTenancySides? MultiTenancySide { get; set; }
		public ConnectionStringResolveArgs(MultiTenancySides? multiTenancySide = null)
		{
			MultiTenancySide = multiTenancySide;
		}
	}
	public interface IConnectionStringResolver
	{
		string GetNameOrConnectionString(ConnectionStringResolveArgs args);
	}
	public interface IAbpStartupConfiguration
	{
		string DefaultNameOrConnectionString { get; set; }
	}
	public class DefaultConnectionStringResolver : IConnectionStringResolver
	{
		private readonly IAbpStartupConfiguration _configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultConnectionStringResolver"/> class.
		/// </summary>
		public DefaultConnectionStringResolver(IAbpStartupConfiguration configuration)
		{
			_configuration = configuration;
		}

		public virtual string GetNameOrConnectionString(ConnectionStringResolveArgs args)
		{
			Check.NotNull(args, nameof(args));

			var defaultConnectionString = _configuration.DefaultNameOrConnectionString;
			if (!string.IsNullOrWhiteSpace(defaultConnectionString))
			{
				return defaultConnectionString;
			}


#if NET46
            if (ConfigurationManager.ConnectionStrings["Default"] != null)
            {
                return "Default";
            }

            if (ConfigurationManager.ConnectionStrings.Count == 1)
            {
                return ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }
#endif

			throw new Exception("Could not find a connection string definition for the application. Set IAbpStartupConfiguration.DefaultNameOrConnectionString or add a 'Default' connection string to application .config file.");
		}
	}
	public static class Check
	{
		public static T NotNull<T>(T value, string ParameterName)
		{
			if (value == null)
				throw new ArgumentNullException(ParameterName);
			return value;
		}
	}
	public static class AbpDataFilters
	{
		public const string SoftDelete = "SoftDelete";
		public const string MustHaveTenant = "MustHaveTenant";
		public const string MayHaveTenant = "MayHaveTenant";
		public static class Parameters
		{
			public const string TenantId = "tenantId";
		}
	}
	public sealed class NullDisposable : IDisposable
	{
		public static NullDisposable Instance { get; } = new NullDisposable();
		private NullDisposable()
		{
		}
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
	public class DisposeAction : IDisposable
	{
		public static readonly DisposeAction Empty = new DisposeAction(null);
		private Action _action;
		/// <summary>
		/// Creates a new <see cref="DisposeAction"/> object.
		/// </summary>
		/// <param name="action">Action to be executed when this object is disposed.</param>
		public DisposeAction(Action action)
		{
			_action = action;
		}
		public void Dispose()
		{
			// Interlocked prevents multiple execution of the _action.
			var action = Interlocked.Exchange(ref _action, null);
			action?.Invoke();
		}
	}
	public static class EventHandlerExtensions
	{
		/// <summary>
		/// Raises given event safely with given arguments.
		/// </summary>
		/// <param name="eventHandler">The event handler</param>
		/// <param name="sender">Source of the event</param>
		public static void InvokeSafely(this EventHandler eventHandler, object sender)
		{
			eventHandler.InvokeSafely(sender, EventArgs.Empty);
		}

		/// <summary>
		/// Raises given event safely with given arguments.
		/// </summary>
		/// <param name="eventHandler">The event handler</param>
		/// <param name="sender">Source of the event</param>
		/// <param name="e">Event argument</param>
		public static void InvokeSafely(this EventHandler eventHandler, object sender, EventArgs e)
		{
			if (eventHandler == null)
			{
				return;
			}

			eventHandler(sender, e);
		}

		/// <summary>
		/// Raises given event safely with given arguments.
		/// </summary>
		/// <typeparam name="TEventArgs">Type of the <see cref="EventArgs"/></typeparam>
		/// <param name="eventHandler">The event handler</param>
		/// <param name="sender">Source of the event</param>
		/// <param name="e">Event argument</param>
		public static void InvokeSafely<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
			where TEventArgs : EventArgs
		{
			if (eventHandler == null)
			{
				return;
			}

			eventHandler(sender, e);
		}
	}
	public abstract class UnitOfWorkBase : IUnitOfWork
	{
		public string Id { get; }
		public IUnitOfWork Outer { get; set; }
		public event EventHandler Completed;
		public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
		public event EventHandler Disposed;
		public UnitOfWorkOptions Options { get; private set; }
		public IReadOnlyList<DataFilterConfiguration> Filters
		{
			get { return _filters.ToImmutableList(); }
		}
		private readonly List<DataFilterConfiguration> _filters;
		protected IUnitOfWorkDefaultOptions DefaultOptions { get; }
		protected IConnectionStringResolver ConnectionStringReslover { get; }
		public bool IsDisposed { get; private set; }
		public IAbpSession AbpSession { get; set; }
		public IUnitOfWorkFilterExecuter FilterExecuter { get; }
		protected IConnectionStringResolver ConnectionStringResolver { get; }
		private bool _isBeginCalledBefore;
		private bool _isCompleteCalledBefore;
		private bool _succeed;
		private Exception _exception;
		private int? _tenantId;
		public UnitOfWorkBase(IConnectionStringResolver connectionStringResolver,
			IUnitOfWorkDefaultOptions defaultOptions,
			IUnitOfWorkFilterExecuter filterExecuter)
		{
			FilterExecuter = filterExecuter;
			DefaultOptions = defaultOptions;
			ConnectionStringReslover = connectionStringResolver;
			Id = Guid.NewGuid().ToString("N");
			_filters = defaultOptions.Filters.ToList();
			AbpSession = NullAbpSession.Instance;

		}
		public void Begin(UnitOfWorkOptions options)
		{
			Check.NotNull(options, nameof(options));
			PreventMultipleBegin();
			Options = options;
			SetFilters(options.FilterOverrides);
			SetTenantId(AbpSession.TenantId, false);
			BeginUow();
		}
		protected virtual void BeginUow()
		{
		}
		private void SetFilters(List<DataFilterConfiguration> filterOverrides)
		{
			for (var i = 0; i < _filters.Count; i++)
			{
				var filterOverride = filterOverrides.FirstOrDefault(f => f.FilterName == _filters[i].FilterName);
				if (filterOverride != null)
				{
					_filters[i] = filterOverride;
				}
				if (AbpSession.TenantId == null)
				{
					ChangeFilterIsEnabledIfNotOverrided(filterOverrides, AbpDataFilters.MustHaveTenant, false);
				}
			}
		}
		private void ChangeFilterIsEnabledIfNotOverrided(List<DataFilterConfiguration> filterOverrides, string filterName, bool isEnabled)
		{
			if (filterOverrides.Any(f => f.FilterName == filterName))
			{
				return;
			}

			var index = _filters.FindIndex(f => f.FilterName == filterName);
			if (index < 0)
			{
				return;
			}

			if (_filters[index].IsEnabled == isEnabled)
			{
				return;
			}

			_filters[index] = new DataFilterConfiguration(filterName, isEnabled);
		}
		private void PreventMultipleBegin()
		{
			if (_isBeginCalledBefore)
			{
				throw new Exception("This unit of work has started before. Can not call Start method more than once.");
			}

			_isBeginCalledBefore = true;
		}
		private void PreventMultipleComplete()
		{
			if (_isCompleteCalledBefore)
			{
				throw new Exception("Complete is called before!");
			}

			_isCompleteCalledBefore = true;
		}
		public void Complete()
		{
			PreventMultipleBegin();
			try
			{
				CompleteUow();
				_succeed = true;
				OnCompleted();
			}
			catch (Exception ex)
			{
				_exception = ex;
				throw;
			}
		}
		protected abstract void CompleteUow();
		protected virtual void OnCompleted()
		{
			Completed.InvokeSafely(this);
		}
		public async Task CompleteAsync()
		{
			PreventMultipleBegin();
			try
			{
				await CompleteUowAsync();
				_succeed = true;
				OnCompleted();
			}
			catch (Exception ex)
			{
				_exception = ex;
				throw;
			}
		}
		protected abstract Task CompleteUowAsync();
		public void Dispose()
		{
			if (!_isBeginCalledBefore || IsDisposed)
			{
				return;
			}

			IsDisposed = true;

			if (!_succeed)
			{
				OnFailed(_exception);
			}

			DisposeUow();
			OnDisposed();
		}
		protected virtual string ResolveConnectionString(ConnectionStringResolveArgs args)
		{
			return ConnectionStringResolver.GetNameOrConnectionString(args);
		}
		protected virtual void OnFailed(Exception exception)
		{
			Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
		}
		protected abstract void DisposeUow();
		protected virtual void OnDisposed()
		{
			Disposed.InvokeSafely(this);
		}
		protected virtual void ApplyDisableFilter(string filterName)
		{
			FilterExecuter.ApplyDisableFilter(this, filterName);
		}
		protected virtual void ApplyEnableFilter(string filterName)
		{
			FilterExecuter.ApplyEnableFilter(this, filterName);
		}
		protected virtual void ApplyFilterParameterValue(string filterName, string parameterName, object value)
		{
			FilterExecuter.ApplyFilterParmeterValue(this, filterName, parameterName, value);
		}
		private int GetFilterIndex(string filterName)
		{
			var filterIndex = _filters.FindIndex(f => f.FilterName == filterName);
			if (filterIndex < 0)
			{
				throw new Exception("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
			}

			return filterIndex;
		}
		public IDisposable DisableFilter(params string[] filterNames)
		{
			var disabledFilters = new List<string>();

			foreach (var filterName in filterNames)
			{
				var filterIndex = GetFilterIndex(filterName);
				if (_filters[filterIndex].IsEnabled)
				{
					disabledFilters.Add(filterName);
					_filters[filterIndex] = new DataFilterConfiguration(_filters[filterIndex], false);
				}
			}

			disabledFilters.ForEach(ApplyDisableFilter);

			return new DisposeAction(() => EnableFilter(disabledFilters.ToArray()));
		}
		public IDisposable EnableFilter(params string[] filterNames)
		{
			var enabledFilters = new List<string>();
			foreach (var filterName in filterNames)
			{
				var filterIndex = GetFilterIndex(filterName);
				if (!_filters[filterIndex].IsEnabled)
				{
					enabledFilters.Add(filterName);
					_filters[filterIndex] = new DataFilterConfiguration(_filters[filterIndex], true);
				}
			}
			enabledFilters.ForEach(ApplyEnableFilter);
			return new DisposeAction(() => DisableFilter(enabledFilters.ToArray()));
		}
		public int? GetTenantId()
		{
			return _tenantId;
		}
		public bool IsFilterEnabled(string filterName)
		{
			return GetFilter(filterName).IsEnabled;
		}
		private DataFilterConfiguration GetFilter(string filterName)
		{
			var filter = _filters.FirstOrDefault(f => f.FilterName == filterName);
			if (filter == null)
			{
				throw new Exception("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
			}

			return filter;
		}
		public abstract void SaveChanges();
		public abstract Task SaveChangesAsync();
		public IDisposable SetFilterParameter(string filterName, string parameterName, object value)
		{
			var filterIndex = GetFilterIndex(filterName);

			var newfilter = new DataFilterConfiguration(_filters[filterIndex]);

			//Store old value
			object oldValue = null;
			var hasOldValue = newfilter.FilterParameters.ContainsKey(parameterName);
			if (hasOldValue)
			{
				oldValue = newfilter.FilterParameters[parameterName];
			}

			newfilter.FilterParameters[parameterName] = value;

			_filters[filterIndex] = newfilter;

			ApplyFilterParameterValue(filterName, parameterName, value);

			return new DisposeAction(() =>
			{
				//Restore old value
				if (hasOldValue)
				{
					SetFilterParameter(filterName, parameterName, oldValue);
				}
			});
		}
		public virtual IDisposable SetTenantId(int? tenantId)
		{
			return SetTenantId(tenantId, true);
		}
		public virtual IDisposable SetTenantId(int? tenantId, bool switchMustHaveTenantEnableDisable)
		{
			var oldTenantId = _tenantId;
			_tenantId = tenantId;


			IDisposable mustHaveTenantEnableChange;
			if (switchMustHaveTenantEnableDisable)
			{
				mustHaveTenantEnableChange = tenantId == null
					? DisableFilter(AbpDataFilters.MustHaveTenant)
					: EnableFilter(AbpDataFilters.MustHaveTenant);
			}
			else
			{
				mustHaveTenantEnableChange = NullDisposable.Instance;
			}

			var mayHaveTenantChange = SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId);
			var mustHaveTenantChange = SetFilterParameter(AbpDataFilters.MustHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId ?? 0);

			return new DisposeAction(() =>
			{
				mayHaveTenantChange.Dispose();
				mustHaveTenantChange.Dispose();
				mustHaveTenantEnableChange.Dispose();
				_tenantId = oldTenantId;
			});
		}
		public override string ToString()
		{
			return $"[UnitOfWork {Id}]";
		}
	}
	public interface IUnitOfWorkManager
	{
		IActiveUnitOfWork Current { get; }
		IUnitOfWorkCompleteHandle Begin();
		IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);
		IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
	}
	public interface ICurrentUnitOfWorkProvider
	{
		IUnitOfWork Current { get; set; }
	}
	public interface IIocResolver
	{
		/// <summary>
		/// Gets an object from IOC container.
		/// Returning object must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <typeparam name="T">Type of the object to get</typeparam>
		/// <returns>The object instance</returns>
		T Resolve<T>();

		/// <summary>
		/// Gets an object from IOC container.
		/// Returning object must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <typeparam name="T">Type of the object to cast</typeparam>
		/// <param name="type">Type of the object to resolve</param>
		/// <returns>The object instance</returns>
		T Resolve<T>(Type type);

		/// <summary>
		/// Gets an object from IOC container.
		/// Returning object must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <typeparam name="T">Type of the object to get</typeparam>
		/// <param name="argumentsAsAnonymousType">Constructor arguments</param>
		/// <returns>The object instance</returns>
		T Resolve<T>(object argumentsAsAnonymousType);

		/// <summary>
		/// Gets an object from IOC container.
		/// Returning object must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <param name="type">Type of the object to get</param>
		/// <returns>The object instance</returns>
		object Resolve(Type type);

		/// <summary>
		/// Gets an object from IOC container.
		/// Returning object must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <param name="type">Type of the object to get</param>
		/// <param name="argumentsAsAnonymousType">Constructor arguments</param>
		/// <returns>The object instance</returns>
		object Resolve(Type type, object argumentsAsAnonymousType);

		/// <summary>
		/// Gets all implementations for given type.
		/// Returning objects must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <typeparam name="T">Type of the objects to resolve</typeparam>
		/// <returns>Object instances</returns>
		T[] ResolveAll<T>();

		/// <summary>
		/// Gets all implementations for given type.
		/// Returning objects must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <typeparam name="T">Type of the objects to resolve</typeparam>
		/// <param name="argumentsAsAnonymousType">Constructor arguments</param>
		/// <returns>Object instances</returns>
		T[] ResolveAll<T>(object argumentsAsAnonymousType);

		/// <summary>
		/// Gets all implementations for given type.
		/// Returning objects must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <param name="type">Type of the objects to resolve</param>
		/// <returns>Object instances</returns>
		object[] ResolveAll(Type type);

		/// <summary>
		/// Gets all implementations for given type.
		/// Returning objects must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <param name="type">Type of the objects to resolve</param>
		/// <param name="argumentsAsAnonymousType">Constructor arguments</param>
		/// <returns>Object instances</returns>
		object[] ResolveAll(Type type, object argumentsAsAnonymousType);

		/// <summary>
		/// Releases a pre-resolved object. See Resolve methods.
		/// </summary>
		/// <param name="obj">Object to be released</param>
		void Release(object obj);

		/// <summary>
		/// Checks whether given type is registered before.
		/// </summary>
		/// <param name="type">Type to check</param>
		bool IsRegistered(Type type);

		/// <summary>
		/// Checks whether given type is registered before.
		/// </summary>
		/// <typeparam name="T">Type to check</typeparam>
		bool IsRegistered<T>();
		
	}
	public class InnerUnitOfWorkCompleteHandle : IUnitOfWorkCompleteHandle
	{
		public const string DidNotCallCompleteMethodExceptionMessage = "Did not call Complete method of a unit of work.";
		private volatile bool _isCompleteCalled;
		private volatile bool _isDisposed;
		public void Complete()
		{

			_isCompleteCalled = true;
		}

		public Task CompleteAsync()
		{
			_isCompleteCalled = true;
			return Task.FromResult(0);
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			_isDisposed = true;

			if (!_isCompleteCalled)
			{
				if (HasException())
				{
					return;
				}

				throw new Exception(DidNotCallCompleteMethodExceptionMessage);
			}
		}
		private static bool HasException()
		{
			try
			{
				return Marshal.GetExceptionCode() != 0;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
	public class UnitOfWorkManager : IUnitOfWorkManager
	{
		private readonly IIocResolver _iocResolver;
		private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
		private readonly IUnitOfWorkDefaultOptions _defaultOptions;
		public IActiveUnitOfWork Current
		{
			get
			{
				return _currentUnitOfWorkProvider.Current;
			}
		}
		public UnitOfWorkManager(IIocResolver iocResolver,
			ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
			IUnitOfWorkDefaultOptions defaultOptions)
		{
			_iocResolver = iocResolver;
			_currentUnitOfWorkProvider = currentUnitOfWorkProvider;
			_defaultOptions = defaultOptions;
		}
		public IUnitOfWorkCompleteHandle Begin()
		{
			return Begin(new UnitOfWorkOptions());
		}
		public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
		{
			return Begin(new UnitOfWorkOptions() { Scope = scope });
		}
		public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
		{
			options.FIllDefaultsForNonProvidedOptions(_defaultOptions);
			var outerUow = _currentUnitOfWorkProvider.Current;
			if (options.Scope == TransactionScopeOption.Required && outerUow != null)
				return new InnerUnitOfWorkCompleteHandle();
			var uow = _iocResolver.Resolve<IUnitOfWork>();
			uow.Completed += (sender, args) =>
			 {
				 _currentUnitOfWorkProvider.Current = null;
			 };
			uow.Failed += (sender, args) =>
			  {
				  _currentUnitOfWorkProvider.Current = null;
			  };
			uow.Disposed += (sender, args) =>
			  {
				  _iocResolver.Release(uow);
			  };
			if (outerUow != null)
				options.FillOutUowFiltersForNonProvidedOptions(outerUow.Filters.ToList());
			uow.Begin(options);
			if (outerUow != null)
			{
				uow.SetTenantId(outerUow.GetTenantId(), false);
			}
			_currentUnitOfWorkProvider.Current = uow;
			return uow;
		}
	}
	public interface IDbContextResolver
	{
		TDbContext Resolve<TDbContext>(string connectionString)
			where TDbContext : DbContext;
		TDbContext Resolve<TDbContext>(IDbConnection existingConnection, bool contextOwnsConnection)
			where TDbContext : DbContext;
	}
	public interface IDbContextTypeMatcher
	{
		void Populate(Type[] dbContextTypes);
		Type GetConcreteType(Type sourceDbContextType);
	}
	public interface IEfTransactionStrategy
	{
		void InitOptions(UnitOfWorkOptions options);
		DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver)
			where TDbContext : DbContext;
		void Commit();
		void Dispose(IIocResolver iocResolver);
	}
	public interface IEfUnitOfWorkFilterExecuter : IUnitOfWorkFilterExecuter
	{
		void ApplyCurrentFilters(IUnitOfWork unitOfWork, DbContext dbContext);
	}
	public static class ObjectExtensions
	{
		/// <summary>
		/// Used to simplify and beautify casting an object to a type. 
		/// </summary>
		/// <typeparam name="T">Type to be casted</typeparam>
		/// <param name="obj">Object to cast</param>
		/// <returns>Casted object</returns>
		public static T As<T>(this object obj)
			where T : class
		{
			return (T)obj;
		}

		/// <summary>
		/// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)"/> method.
		/// </summary>
		/// <param name="obj">Object to be converted</param>
		/// <typeparam name="T">Type of the target object</typeparam>
		/// <returns>Converted object</returns>
		public static T To<T>(this object obj)
			where T : struct
		{
			if (typeof(T) == typeof(Guid))
			{
				return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
			}

			return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Check if an item is in a list.
		/// </summary>
		/// <param name="item">Item to check</param>
		/// <param name="list">List of items</param>
		/// <typeparam name="T">Type of the items</typeparam>
		public static bool IsIn<T>(this T item, params T[] list)
		{
			return list.Contains(item);
		}
	}
	public static class DictionaryExtensions
	{
		/// <summary>
		/// This method is used to try to get a value in a dictionary if it does exists.
		/// </summary>
		/// <typeparam name="T">Type of the value</typeparam>
		/// <param name="dictionary">The collection object</param>
		/// <param name="key">Key</param>
		/// <param name="value">Value of the key (or default value if key not exists)</param>
		/// <returns>True if key does exists in the dictionary</returns>
		internal static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
		{
			object valueObj;
			if (dictionary.TryGetValue(key, out valueObj) && valueObj is T)
			{
				value = (T)valueObj;
				return true;
			}

			value = default(T);
			return false;
		}

		/// <summary>
		/// Gets a value from the dictionary with given key. Returns default value if can not find.
		/// </summary>
		/// <param name="dictionary">Dictionary to check and get</param>
		/// <param name="key">Key to find the value</param>
		/// <typeparam name="TKey">Type of the key</typeparam>
		/// <typeparam name="TValue">Type of the value</typeparam>
		/// <returns>Value if found, default if can not found.</returns>
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue obj;
			return dictionary.TryGetValue(key, out obj) ? obj : default(TValue);
		}

		/// <summary>
		/// Gets a value from the dictionary with given key. Returns default value if can not find.
		/// </summary>
		/// <param name="dictionary">Dictionary to check and get</param>
		/// <param name="key">Key to find the value</param>
		/// <param name="factory">A factory method used to create the value if not found in the dictionary</param>
		/// <typeparam name="TKey">Type of the key</typeparam>
		/// <typeparam name="TValue">Type of the value</typeparam>
		/// <returns>Value if found, default if can not found.</returns>
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
		{
			TValue obj;
			if (dictionary.TryGetValue(key, out obj))
			{
				return obj;
			}

			return dictionary[key] = factory(key);
		}

		/// <summary>
		/// Gets a value from the dictionary with given key. Returns default value if can not find.
		/// </summary>
		/// <param name="dictionary">Dictionary to check and get</param>
		/// <param name="key">Key to find the value</param>
		/// <param name="factory">A factory method used to create the value if not found in the dictionary</param>
		/// <typeparam name="TKey">Type of the key</typeparam>
		/// <typeparam name="TValue">Type of the value</typeparam>
		/// <returns>Value if found, default if can not found.</returns>
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
		{
			return dictionary.GetOrAdd(key, k => factory());
		}
	}
	public static class IsolationLevelExtensions
	{
		/// <summary>
		/// Converts <see cref="System.Transactions.IsolationLevel"/> to <see cref="IsolationLevel"/>.
		/// </summary>
		public static System.Data.IsolationLevel ToSystemDataIsolationLevel(this IsolationLevel isolationLevel)
		{
			switch (isolationLevel)
			{
				case IsolationLevel.ReadCommitted:
					return System.Data.IsolationLevel.ReadCommitted;
				case IsolationLevel.ReadUncommitted:
					return System.Data.IsolationLevel.ReadUncommitted;
				case IsolationLevel.RepeatableRead:
					return System.Data.IsolationLevel.RepeatableRead;
				case IsolationLevel.Serializable:
					return System.Data.IsolationLevel.Serializable;
				default:
					throw new Exception("Unknown isolation level: " + isolationLevel);
			}
		}
	}
	public class ActiveTransactionInfo
	{
		public DbContextTransaction DbContextTransaction { get; }

		public DbContext StarterDbContext { get; }

		public List<DbContext> AttendedDbContexts { get; }

		public ActiveTransactionInfo(DbContextTransaction dbContextTransaction, DbContext starterDbContext)
		{
			DbContextTransaction = dbContextTransaction;
			StarterDbContext = starterDbContext;
			AttendedDbContexts = new List<DbContext>();
		}
	}
	public class ActiveDbContextInfo
	{
		public DbContext DbContext { get; }

		public ActiveDbContextInfo(DbContext dbContext)
		{
			DbContext = dbContext;
		}
	}
	public class DbContextEfTransactionStrategy : IEfTransactionStrategy
	{
		protected UnitOfWorkOptions Options { get; private set; }
		protected IDictionary<string, ActiveTransactionInfo> ActiveTransactions { get; }
		public DbContextEfTransactionStrategy()
		{
			ActiveTransactions = new Dictionary<string, ActiveTransactionInfo>();
		}
		public void InitOptions(UnitOfWorkOptions options)
		{
			Options = options;
		}
		public void Commit()
		{
			foreach (var activeTransaction in ActiveTransactions.Values)
			{
				activeTransaction.DbContextTransaction.Commit();
			}
		}
		public DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver) where TDbContext : DbContext
		{
			DbContext dbContext;
			var activeTransaction = ActiveTransactions.GetOrDefault(connectionString);
			if (activeTransaction == null)
			{
				dbContext = dbContextResolver.Resolve<TDbContext>(connectionString);
				var dbtransaction = dbContext.Database.BeginTransaction((Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
				activeTransaction = new ActiveTransactionInfo(dbtransaction, dbContext);
				ActiveTransactions[connectionString] = activeTransaction;
			}
			else
			{
				dbContext = dbContextResolver.Resolve<TDbContext>(activeTransaction.DbContextTransaction.UnderlyingTransaction.Connection, false);
				dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.UnderlyingTransaction);
				activeTransaction.AttendedDbContexts.Add(dbContext);
			}

			return dbContext;
		}
		public void Dispose(IIocResolver iocResolver)
		{
			foreach (var activeTransaction in ActiveTransactions.Values)
			{
				foreach (var attendedDbContext in activeTransaction.AttendedDbContexts)
				{
					iocResolver.Release(attendedDbContext);
				}

				activeTransaction.DbContextTransaction.Dispose();
				iocResolver.Release(activeTransaction.StarterDbContext);
			}
			ActiveTransactions.Clear();
		}
	}
	internal static class TypeHelper
	{
		public static bool IsFunc(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			var type = obj.GetType();
			if (!type.GetTypeInfo().IsGenericType)
			{
				return false;
			}

			return type.GetGenericTypeDefinition() == typeof(Func<>);
		}

		public static bool IsFunc<TReturn>(object obj)
		{
			return obj != null && obj.GetType() == typeof(Func<TReturn>);
		}

		public static bool IsPrimitiveExtendedIncludingNullable(Type type, bool includeEnums = false)
		{
			if (IsPrimitiveExtended(type, includeEnums))
			{
				return true;
			}

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return IsPrimitiveExtended(type.GenericTypeArguments[0], includeEnums);
			}

			return false;
		}

		private static bool IsPrimitiveExtended(Type type, bool includeEnums)
		{
			if (type.GetTypeInfo().IsPrimitive)
			{
				return true;
			}

			if (includeEnums && type.GetTypeInfo().IsEnum)
			{
				return true;
			}

			return type == typeof(string) ||
				   type == typeof(decimal) ||
				   type == typeof(DateTime) ||
				   type == typeof(DateTimeOffset) ||
				   type == typeof(TimeSpan) ||
				   type == typeof(Guid);
		}
	}
	public class EfDynamicFiltersUnitOfWorkFilterExecuter : IEfUnitOfWorkFilterExecuter
	{
		public void ApplyDisableFilter(IUnitOfWork unitOfWork, string filterName)
		{
			foreach (var activeDbContext in unitOfWork.As<EfUnitOfWork>().GetAllActiveDbContexts())
			{

				activeDbContext.DisableFilter(filterName);
			}
		}
		public void ApplyEnableFilter(IUnitOfWork unitOfWork, string filterName)
		{
			foreach (var activeDbContext in unitOfWork.As<EfUnitOfWork>().GetAllActiveDbContexts())
			{
				activeDbContext.EnableFilter(filterName);
			}
		}
		public void ApplyCurrentFilters(IUnitOfWork unitOfWork, DbContext dbContext)
		{
			foreach (var filter in unitOfWork.Filters)
			{
				if (filter.IsEnabled)
				{
					dbContext.EnableFilter(filter.FilterName);
				}
				else
				{
					dbContext.DisableFilter(filter.FilterName);
				}

				foreach (var filterParameter in filter.FilterParameters)
				{
					if (TypeHelper.IsFunc<object>(filterParameter.Value))
					{
						dbContext.SetFilterScopedParameterValue(filter.FilterName, filterParameter.Key, (Func<object>)filterParameter.Value);
					}
					else
					{
						dbContext.SetFilterScopedParameterValue(filter.FilterName, filterParameter.Key, filterParameter.Value);
					}
				}
			}
		}
		public void ApplyFilterParmeterValue(IUnitOfWork unitOfWork, string filterName, string paramterName, object value)
		{
			foreach (var activeDbContext in unitOfWork.As<EfUnitOfWork>().GetAllActiveDbContexts())
			{
				if (TypeHelper.IsFunc<object>(value))
				{
					activeDbContext.SetFilterScopedParameterValue(filterName, paramterName, (Func<object>)value);
				}
				else
				{
					activeDbContext.SetFilterScopedParameterValue(filterName, paramterName, value);
				}
			}
		}
	}
	public class DefaultDbContextResolver : IDbContextResolver
	{
		private readonly IIocResolver _iocResolver;
		private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

		public DefaultDbContextResolver(IIocResolver iocResolver, IDbContextTypeMatcher dbContextTypeMatcher)
		{
			_iocResolver = iocResolver;
			_dbContextTypeMatcher = dbContextTypeMatcher;
		}

		public TDbContext Resolve<TDbContext>(string connectionString)
			where TDbContext : DbContext
		{
			var dbContextType = GetConcreteType<TDbContext>();
			return (TDbContext)_iocResolver.Resolve(dbContextType, new
			{
				nameOrConnectionString = connectionString
			});
		}
		public TDbContext Resolve<TDbContext>(IDbConnection existingConnection, bool contextOwnsConnection) where TDbContext : DbContext
		{
			var dbContextType = GetConcreteType<TDbContext>();
			return (TDbContext)_iocResolver.Resolve(dbContextType, new
			{
				existingConnection = existingConnection,
				contextOwnsConnection = contextOwnsConnection
			});
		}

		protected virtual Type GetConcreteType<TDbContext>()
		{
			var dbContextType = typeof(TDbContext);
			return !dbContextType.IsAbstract
				? dbContextType
				: _dbContextTypeMatcher.GetConcreteType(dbContextType);
		}
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Interface)]
	public class MultiTenancySideAttribute : Attribute
	{
		/// <summary>
		/// Multitenancy side.
		/// </summary>
		public MultiTenancySides Side { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiTenancySideAttribute"/> class.
		/// </summary>
		/// <param name="side">Multitenancy side.</param>
		public MultiTenancySideAttribute(MultiTenancySides side)
		{
			Side = side;
		}
	}
	[AttributeUsage(AttributeTargets.Class)]
	public class AutoRepositoryTypesAttribute : Attribute
	{
		public Type RepositoryInterface { get; }

		public Type RepositoryInterfaceWithPrimaryKey { get; }

		public Type RepositoryImplementation { get; }

		public Type RepositoryImplementationWithPrimaryKey { get; }

		public bool WithDefaultRepositoryInterfaces { get; set; }

		public AutoRepositoryTypesAttribute(
			Type repositoryInterface,
			Type repositoryInterfaceWithPrimaryKey,
			Type repositoryImplementation,
			Type repositoryImplementationWithPrimaryKey)
		{
			RepositoryInterface = repositoryInterface;
			RepositoryInterfaceWithPrimaryKey = repositoryInterfaceWithPrimaryKey;
			RepositoryImplementation = repositoryImplementation;
			RepositoryImplementationWithPrimaryKey = repositoryImplementationWithPrimaryKey;
		}
	}
	public class DefaultDbContextAttribute : Attribute
	{

	}
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type System.String, using the specified separator between each member.
		/// This is a shortcut for string.Join(...)
		/// </summary>
		/// <param name="source">A collection that contains the strings to concatenate.</param>
		/// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
		/// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty.</returns>
		public static string JoinAsString(this IEnumerable<string> source, string separator)
		{
			return string.Join(separator, source);
		}

		/// <summary>
		/// Concatenates the members of a collection, using the specified separator between each member.
		/// This is a shortcut for string.Join(...)
		/// </summary>
		/// <param name="source">A collection that contains the objects to concatenate.</param>
		/// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
		/// <typeparam name="T">The type of the members of values.</typeparam>
		/// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty.</returns>
		public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
		{
			return string.Join(separator, source);
		}

		/// <summary>
		/// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
		/// </summary>
		/// <param name="source">Enumerable to apply filtering</param>
		/// <param name="condition">A boolean value</param>
		/// <param name="predicate">Predicate to filter the enumerable</param>
		/// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
		public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
		{
			return condition
				? source.Where(predicate)
				: source;
		}

		/// <summary>
		/// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
		/// </summary>
		/// <param name="source">Enumerable to apply filtering</param>
		/// <param name="condition">A boolean value</param>
		/// <param name="predicate">Predicate to filter the enumerable</param>
		/// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
		public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
		{
			return condition
				? source.Where(predicate)
				: source;
		}
	}
	public abstract class DbContextTypeMatcher<TBaseDbContext> : IDbContextTypeMatcher
	{
		private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
		private readonly Dictionary<Type, List<Type>> _dbContextTypes;

		protected DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
		{
			_currentUnitOfWorkProvider = currentUnitOfWorkProvider;
			_dbContextTypes = new Dictionary<Type, List<Type>>();
		}

		public void Populate(Type[] dbContextTypes)
		{
			foreach (var dbContextType in dbContextTypes)
			{
				var types = new List<Type>();

				AddWithBaseTypes(dbContextType, types);

				foreach (var type in types)
				{
					Add(type, dbContextType);
				}
			}
		}

		//TODO: GetConcreteType method can be optimized by extracting/caching MultiTenancySideAttribute attributes for DbContexes.

		public virtual Type GetConcreteType(Type sourceDbContextType)
		{
			//TODO: This can also get MultiTenancySide to filter dbcontexes

			if (!sourceDbContextType.GetTypeInfo().IsAbstract)
			{
				return sourceDbContextType;
			}

			//Get possible concrete types for given DbContext type
			var allTargetTypes = _dbContextTypes.GetOrDefault(sourceDbContextType);

			if (allTargetTypes.IsNullOrEmpty())
			{
				throw new Exception("Could not find a concrete implementation of given DbContext type: " + sourceDbContextType.AssemblyQualifiedName);
			}

			if (allTargetTypes.Count == 1)
			{
				//Only one type does exists, return it
				return allTargetTypes[0];
			}

			CheckCurrentUow();

			var currentTenancySide = GetCurrentTenancySide();

			var multiTenancySideContexes = GetMultiTenancySideContextTypes(allTargetTypes, currentTenancySide);

			if (multiTenancySideContexes.Count == 1)
			{
				return multiTenancySideContexes[0];
			}

			if (multiTenancySideContexes.Count > 1)
			{
				return GetDefaultDbContextType(multiTenancySideContexes, sourceDbContextType, currentTenancySide);
			}

			return GetDefaultDbContextType(allTargetTypes, sourceDbContextType, currentTenancySide);
		}

		private void CheckCurrentUow()
		{
			if (_currentUnitOfWorkProvider.Current == null)
			{
				throw new Exception("GetConcreteType method should be called in a UOW.");
			}
		}

		private MultiTenancySides GetCurrentTenancySide()
		{
			return _currentUnitOfWorkProvider.Current.GetTenantId() == null
					   ? MultiTenancySides.Host
					   : MultiTenancySides.Tenant;
		}

		private static List<Type> GetMultiTenancySideContextTypes(List<Type> dbContextTypes, MultiTenancySides tenancySide)
		{
			return dbContextTypes.Where(type =>
			{
				var attrs = type.GetTypeInfo().GetCustomAttributes(typeof(MultiTenancySideAttribute), true).ToArray();
				if (attrs.IsNullOrEmpty())
				{
					return false;
				}

				return ((MultiTenancySideAttribute)attrs[0]).Side.HasFlag(tenancySide);
			}).ToList();
		}

		private static Type GetDefaultDbContextType(List<Type> dbContextTypes, Type sourceDbContextType, MultiTenancySides tenancySide)
		{
			var filteredTypes = dbContextTypes
				.Where(type => !type.GetTypeInfo().IsDefined(typeof(AutoRepositoryTypesAttribute), true))
				.ToList();

			if (filteredTypes.Count == 1)
			{
				return filteredTypes[0];
			}

			filteredTypes = filteredTypes
				.Where(type => !type.GetTypeInfo().IsDefined(typeof(DefaultDbContextAttribute), true))
				.ToList();

			if (filteredTypes.Count == 1)
			{
				return filteredTypes[0];
			}

			throw new Exception(string.Format(
				"Found more than one concrete type for given DbContext Type ({0}) define MultiTenancySideAttribute with {1}. Found types: {2}.",
				sourceDbContextType,
				tenancySide,
				dbContextTypes.Select(c => c.AssemblyQualifiedName).JoinAsString(", ")
				));
		}

		private static void AddWithBaseTypes(Type dbContextType, List<Type> types)
		{
			types.Add(dbContextType);
			if (dbContextType != typeof(TBaseDbContext))
			{
				AddWithBaseTypes(dbContextType.GetTypeInfo().BaseType, types);
			}
		}

		private void Add(Type sourceDbContextType, Type targetDbContextType)
		{
			if (!_dbContextTypes.ContainsKey(sourceDbContextType))
			{
				_dbContextTypes[sourceDbContextType] = new List<Type>();
			}

			_dbContextTypes[sourceDbContextType].Add(targetDbContextType);
		}
	}
	public static class CollectionExtensions
	{
		/// <summary>
		/// Checks whatever given collection object is null or has no item.
		/// </summary>
		public static bool IsNullOrEmpty<T>(this ICollection<T> source)
		{
			return source == null || source.Count <= 0;
		}

		/// <summary>
		/// Adds an item to the collection if it's not already in the collection.
		/// </summary>
		/// <param name="source">Collection</param>
		/// <param name="item">Item to check and add</param>
		/// <typeparam name="T">Type of the items in the collection</typeparam>
		/// <returns>Returns True if added, returns False if not.</returns>
		public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			if (source.Contains(item))
			{
				return false;
			}

			source.Add(item);
			return true;
		}
	}
	public class EfUnitOfWork : UnitOfWorkBase
	{
		protected IDictionary<string, DbContext> ActiveDbContexts { get; }
		protected IIocResolver IocResolver { get; }
		private readonly IDbContextResolver _dbContextResolver;
		private readonly IDbContextTypeMatcher _dbContextTypeMatcher;
		private readonly IEfTransactionStrategy _transactionStrategy;
		public EfUnitOfWork(IConnectionStringResolver connectionStringResolver, IUnitOfWorkDefaultOptions defaultOptions, IUnitOfWorkFilterExecuter filterExecuter) : base(connectionStringResolver, defaultOptions, filterExecuter)
		{
		}
		public EfUnitOfWork(IIocResolver iocResolver,
			IConnectionStringResolver connectionStringResolver,
			IDbContextResolver dbContextResolver,
			IEfUnitOfWorkFilterExecuter filterExecuter,
			IUnitOfWorkDefaultOptions defaultOptions,
			IDbContextTypeMatcher dbContextTypeMatcher,
			IEfTransactionStrategy transactionStrategy) : base(connectionStringResolver, defaultOptions, filterExecuter)
		{
			IocResolver = iocResolver;
			_dbContextResolver = dbContextResolver;
			_dbContextTypeMatcher = dbContextTypeMatcher;
			_transactionStrategy = transactionStrategy;
			ActiveDbContexts = new Dictionary<string, DbContext>();
		}
		protected override void BeginUow()
		{
			if (Options.IsTransactional == true)
				_transactionStrategy.InitOptions(Options);
		}
		public override void SaveChanges()
		{
			foreach (var dbContext in GetAllActiveDbContexts())
			{
				SaveChangesInDbContext(dbContext);
			}
		}
		public override async Task SaveChangesAsync()
		{
			foreach (var dbContext in GetAllActiveDbContexts())
			{
				await SaveChangesInDbContextAsync(dbContext);
			}
		}
		public IReadOnlyList<DbContext> GetAllActiveDbContexts()
		{
			return ActiveDbContexts.Values.ToImmutableList();
		}
		protected virtual async Task SaveChangesInDbContextAsync(DbContext dbContext)
		{
			await dbContext.SaveChangesAsync();
		}
		public virtual void SaveChangesInDbContext(DbContext dbContext)
		{
			dbContext.SaveChanges();
		}
		protected override void CompleteUow()
		{
			SaveChanges();
			if (Options.IsTransactional == true)
				_transactionStrategy.Commit();
		}
		protected override async Task CompleteUowAsync()
		{
			await SaveChangesAsync();
			if (Options.IsTransactional == true)
			{
				_transactionStrategy.Commit();
			}
		}
		public virtual TDbContext GetOrCreateDbContext<TDbContext>(MultiTenancySides? multiTenancySide = null)
			where TDbContext : DbContext
		{
			var concreteDbContextType = _dbContextTypeMatcher.GetConcreteType(typeof(TDbContext));
			var connectionStringResolveArgs = new ConnectionStringResolveArgs(multiTenancySide);
			connectionStringResolveArgs["DbContextType"] = typeof(TDbContext);
			connectionStringResolveArgs["DbContextConcreteType"] = concreteDbContextType;
			var connectionString = ResolveConnectionString(connectionStringResolveArgs);
			var dbContextKey = concreteDbContextType.FullName + "#" + connectionString;
			DbContext dbContext;
			if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContext))
			{
				if (Options.IsTransactional == true)
				{
					dbContext = _transactionStrategy.CreateDbContext<TDbContext>(connectionString, _dbContextResolver);
				}
				else
				{
					dbContext = _dbContextResolver.Resolve<TDbContext>(connectionString);
				}

				if (Options.Timeout.HasValue && !dbContext.Database.CommandTimeout.HasValue)
				{
					dbContext.Database.CommandTimeout = Options.Timeout.Value.TotalSeconds.To<int>();
				}

				((IObjectContextAdapter)dbContext).ObjectContext.ObjectMaterialized += (sender, args) =>
				{
					ObjectContext_ObjectMaterialized(dbContext, args);
				};

				FilterExecuter.As<IEfUnitOfWorkFilterExecuter>().ApplyCurrentFilters(this, dbContext);

				ActiveDbContexts[dbContextKey] = dbContext;
			}

			return (TDbContext)dbContext;
		}
		private static void ObjectContext_ObjectMaterialized(DbContext dbContext, ObjectMaterializedEventArgs e)
		{
			var entityType = ObjectContext.GetObjectType(e.Entity.GetType());

			dbContext.Configuration.AutoDetectChangesEnabled = false;
			var previousState = dbContext.Entry(e.Entity).State;

			//DateTimePropertyInfoHelper.NormalizeDatePropertyKinds(e.Entity, entityType);

			dbContext.Entry(e.Entity).State = previousState;
			dbContext.Configuration.AutoDetectChangesEnabled = true;
		}
		protected override void DisposeUow()
		{
			if (Options.IsTransactional == true)
				_transactionStrategy.Dispose(IocResolver);
			else
				foreach (var activeDbContext in GetAllActiveDbContexts())
					Release(activeDbContext);
			ActiveDbContexts.Clear();
		}
		protected virtual void Release(DbContext dbContext)
		{
			dbContext.Dispose();
			IocResolver.Release(dbContext);
		}
	}
}
