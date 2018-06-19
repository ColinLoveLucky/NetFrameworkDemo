using System.Text;
using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Log4Net;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public class RepositoryBase<R> : IRepositoryBase<R>, IDisposable where R : class
    {
        //private IUnitOfWork unitOfWork;

        internal DbConnection _connection;
        internal bool _proxyCreationEnabled = false;
        internal bool _rethrowExceptions = true;
        internal bool _saveLastExcecutedMethodInfo = false;
        internal int _commandTimeout = 300;
        internal string _connectionString = string.Empty;
        internal MethodBase _lastExecutedMethod = null;
        public event RepositoryBaseExceptionHandler RepositoryBaseExceptionRaised;
        public delegate void RepositoryBaseExceptionHandler(Exception exception);
        public delegate void RepositoryBaseRollBackOccuredHandler(MethodBase lastExecutedMethod);
        internal static bool _staticLazyConnectionOverrideUsed = false;
        internal static bool _lazyConnection = false;
        //internal void InitializeRepository(bool LazyLoadingEnabled=true)
        //{
        //    if (this.Model!=null)
        //    {
        //        Model.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
        //    }
        //}

        //internal void InitializeConnection()
        //{
        //    if (Model != null)
        //    {
        //        if (!string.IsNullOrEmpty(_connectionString))
        //        {
        //            Model.Database.Connection.ConnectionString = _connectionString;
        //        }

        //        _connection = ((IObjectContextAdapter)Model).ObjectContext.Connection;
        //        _connection.Open();
        //    }
        //}

        //public RepositoryBase(DbContext _model)
        //{
        //    if (this.Model==null)
        //    {
        //        this.Model = _model;
        //    }
        //    InitializeRepository();
        //}

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            //var nameOrConnectionString = System.Configuration.ConfigurationManager.AppSettings["MainDataBasenameOrConnectionString"].ToString();
            ////string nameOrConnectionString = config;
            //if (!string.IsNullOrWhiteSpace(nameOrConnectionString))
            //{
            //    if (this.Model == null)
            //    {
            //        Type t = typeof(DbContext);
            //        this.Model =
            //            (DbContext)Activator.CreateInstance(t, nameOrConnectionString);
            //    }
            //}
            _unitOfWork = unitOfWork;
            //this.Model = unitOfWork.GetContext();
            //InitializeRepository();
        }

        //public RepositoryBase(string nameOrConnectionString)
        //{
        //    if (!string.IsNullOrWhiteSpace(nameOrConnectionString))
        //    {
        //        this.Model = new DbContext(nameOrConnectionString);
        //    }
        //    InitializeRepository();
        //}

        public void SequenceInitializer(R obj)
        {
            try
            {
                var props = from prop in obj.GetType().GetProperties()
                            let attrs = prop.GetCustomAttributes(typeof(SequenceAttribute), false)
                            where attrs.Any()
                            select new { Property = prop, Attr = ((SequenceAttribute)attrs.First()) };
                foreach (var pair in props)
                {
                    //判断是否已经存在主键ID值
                    var _val = pair.Property.GetValue(obj);
                    if (_val.ToDouble() == 0)
                    {
                        var SequenceName = pair.Attr.SequenceName;
                        Int64 SequenceValue = 0;
                        var sql = String.Format("SELECT {0}.NEXTVAL FROM DUAL", SequenceName);
                        SequenceValue = ((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext.ExecuteStoreQuery<Int64>(sql, null).FirstOrDefault();
                        Int64 val = SequenceValue;
                        pair.Property.SetValue(obj, val, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Add(R entity)
        {
            SequenceInitializer(entity);
            bool result = false;
            SetEntity().Add(entity);
            result = true;
            return result;
        }

        public bool AddMultiple(List<R> entities)
        {
            bool result = false;
            entities.ForEach(e => result = Add(e));
            return result;
        }

        public bool AddOrUpdate(R entity)
        {
            bool result = false;
            var entry = SetEntry(entity);
            if (entry != null)
            {
                if (entry.State == EntityState.Detached)
                {
                    entry.State = EntityState.Added;
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
            else
            {
                SequenceInitializer(entity);
                _unitOfWork.GetContext().Set<R>().Attach(entity);
            }
            result = true;
            return result;
        }

        public bool AddOrUpdateMultiple(List<R> entities)
        {
            bool result = false;
            entities.ForEach(e => result = AddOrUpdate(e));
            return result;
        }

        public Int32 Count()
        {
            return _unitOfWork.GetContext().Set<R>().Count();
        }

        public bool Delete(R entity)
        {
            bool result = false;
            var entry = SetEntry(entity);
            if (entry != null)
            {
                //6.0
                //entry.State = System.Data.Entity.EntityState.Deleted;
                //5.0
                entry.State = EntityState.Deleted;
            }
            else
            {
                _unitOfWork.GetContext().Set<R>().Attach(entity);
            }

            _unitOfWork.GetContext().Set<R>().Remove(entity);
            result = true;
            return result;
        }

        public bool DeleteMultiple(List<R> entities)
        {
            bool result = false;

            entities.ForEach(e => result = Delete(e));

            return result;
        }

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext;
            var entry = _unitOfWork.GetContext().Entry(entity);

            if (entry.State != EntityState.Detached)
                objectContext.Detach(entity);
        }

        public void Detach(List<object> entities)
        {
            entities.ForEach(e => Detach(e));
        }

        public IQueryable<R> Find(Expression<Func<R, bool>> where, bool istacking = true)
        {
            IQueryable<R> entities = default(IQueryable<R>);
            entities = istacking ? SetEntities().Where(where) : SetEntitiesNoTracking().Where(where);
            return entities;
        }

        public IQueryable<R> Find(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes)
        {
            IQueryable<R> entities = SetEntities();

            if (includes != null)
            {
                entities = ApplyIncludesToQuery(entities, includes);
            }

            entities = entities.Where(where);

            return entities;
        }

        public R First(Expression<Func<R, bool>> where)
        {
            IQueryable<R> entities = SetEntities();

            R entity = default(R);

            entity = entities
                    .First(where);

            return entity;
        }

        public R FirstOrDefault(Expression<Func<R, bool>> where, bool isTracking = true, params Expression<Func<R, object>>[] includes)
        {
            try
            {
                IQueryable<R> entities = isTracking ? SetEntities() : SetEntitiesNoTracking();

                R entity = default(R);

                if (where != null)
                    entities = entities.Where(where);

                if (includes != null)
                {
                    entities = ApplyIncludesToQuery(entities, includes);
                }

                entity = entities.FirstOrDefault();

                return entity;
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public IQueryable<R> GetAll()
        {
            IQueryable<R> entities = default(IQueryable<R>);

            entities = SetEntities();

            return entities;
        }

        public IQueryable<R> GetAll(params Expression<Func<R, object>>[] includes)
        {
            IQueryable<R> entities = SetEntities();
            entities = ApplyIncludesToQuery(entities, includes);
            return entities;
        }

        public DbConnection GetConnection()
        {
            return _connection;
        }

        public void SetIdentityCommand()
        {
            List<EntitySetBase> sets;

            var container =
                   ((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext.MetadataWorkspace
                      .GetEntityContainer(
                            ((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext.DefaultContainerName,
                            DataSpace.CSpace);

            sets = container.BaseEntitySets.ToList();

            foreach (EntitySetBase set in sets)
            {
                string command = string.Format("SET IDENTITY_INSERT {0} {1}", set.Name, "ON");
                ((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext.ExecuteStoreCommand(command);
            }
        }

        public void SetConnectionString(string connectionString)
        {
            if (_lazyConnection == true)
            {
                _connectionString = connectionString;
                //InitializeConnection();
            }
        }

        public void SetRethrowExceptions(bool rehtrowExceptions)
        {
            _rethrowExceptions = rehtrowExceptions;
        }


        public R Single(Expression<Func<R, bool>> where)
        {
            IQueryable<R> entities = SetEntities();

            R entity = default(R);

            entity = entities.Single(where);

            return entity;
        }

        public R SingleOrDefault(Expression<Func<R, bool>> where)
        {
            IQueryable<R> entities = SetEntities();

            R entity = default(R);

            entity = entities
                    .SingleOrDefault(where);

            return entity;
        }

        public R SingleOrDefault(Expression<Func<R, bool>> where, Expression<Func<R, object>> include)
        {
            IQueryable<R> entities = SetEntities();

            R entity = default(R);

            entity = entities
                    .Include(include)
                    .SingleOrDefault(where);

            return entity;
        }

        public bool Update(R entity)
        {
            bool result = false;

            var entry = SetEntry(entity);

            if (entry != null)
            {
                entry.State = EntityState.Modified;
            }
            else
            {
                _unitOfWork.GetContext().Set<R>().Attach(entity);
            }

            return result;
        }

        public bool UpdateProperty(R entity, params Expression<Func<R, object>>[] properties)
        {
            bool result = false;

            var entry = SetEntry(entity);

            if (entry != null)
            {
                _unitOfWork.GetContext().Set<R>().Attach(entity);

                foreach (var property in properties)
                {
                    MemberExpression body = property.Body as MemberExpression;

                    if (body == null)
                    {
                        UnaryExpression ubody = (UnaryExpression)property.Body;
                        body = ubody.Operand as MemberExpression;
                    }

                    entry.Property(body.Member.Name).IsModified = true;
                }
            }
            else
            {
                _unitOfWork.GetContext().Set<R>().Attach(entity);
            }

            result = true;

            return result;
        }

        public bool UpdateMultiple(List<R> entities)
        {
            bool result = false;

            entities.ForEach(e => result = Update(e));

            return result;
        }

        public static void UseLazyConnection(bool useLazyConnection)
        {
            _lazyConnection = useLazyConnection;
            _staticLazyConnectionOverrideUsed = true;
        }

        internal IQueryable<R> ApplyIncludesToQuery(IQueryable<R> entities, Expression<Func<R, object>>[] includes)
        {
            if (includes != null)
                entities = includes.Aggregate(entities, (current, include) => current.Include(include));

            return entities;
        }


        internal IQueryable<R> SetEntities()
        {
            IQueryable<R> entities = _unitOfWork.GetContext().Set<R>();

            return entities;
        }

        /// <summary>
        /// 返回无跟踪状态的查询对象
        /// </summary>
        /// <returns></returns>
        internal IQueryable<R> SetEntitiesNoTracking()
        {
            IQueryable<R> entities = _unitOfWork.GetContext().Set<R>();
            return entities.AsNoTracking();
        }

        internal DbSet<R> SetEntity()
        {
            DbSet<R> entity = _unitOfWork.GetContext().Set<R>();

            return entity;
        }

        internal DbEntityEntry SetEntry(R entity)
        {
            DbEntityEntry entry = _unitOfWork.GetContext().Entry(entity);

            return entry;
        }


        protected void OnRepositoryBaseExceptionRaised(Exception e)
        {
            RepositoryBaseExceptionHandler handler = RepositoryBaseExceptionRaised;
            if (handler != null)
            {
                handler(e);
            }
        }


        private string GetEntityName()
        {
            string entitySetName = ((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext
                .MetadataWorkspace
                .GetEntityContainer(((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext.DefaultContainerName, DataSpace.CSpace)
                                    .BaseEntitySets.Where(bes => bes.ElementType.Name == typeof(R).Name).First().Name;
            return string.Format("{0}.{1}", ((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext.DefaultContainerName, entitySetName);
        }

        public IQueryable<R> GetQuery()
        {
            var entityName = GetEntityName();
            return ((IObjectContextAdapter)_unitOfWork.GetContext()).ObjectContext.CreateQuery<R>(entityName);
        }

        public IQueryable<R> GetQuery(Expression<Func<R, bool>> predicate)
        {
            return GetQuery().Where(predicate);
        }

        public IQueryable<R> GetQuery(ISpecification<R> criteria)
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery());
        }

        public IEnumerable<R> Get<TOrderBy>(Expression<Func<R, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<R> Get<TOrderBy>(Expression<Func<R, bool>> criteria, Expression<Func<R, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery(criteria).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery(criteria).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        //public IQueryable<R> Get<R>(Expression<Func<R, bool>> criteria, string orderBy, int pageIndex, int pageSize, string sord)
        //{
        //    return GetQuery(criteria).Sort(pageSize, sord).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        //}
        public IEnumerable<R> Get<TOrderBy>(ISpecification<R> specification, Expression<Func<R, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return specification.SatisfyingEntitiesFrom(GetQuery()).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return specification.SatisfyingEntitiesFrom(GetQuery()).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public int Count(Expression<Func<R, bool>> criteria)
        {
            return GetQuery().Count(criteria);
        }

        public int Count(ISpecification<R> criteria)
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery()).Count();
        }

        public void Dispose()
        {

            //System.Diagnostics.Trace.Write(new Exception("repository:" + this.ToString()), "error");
            //this.unitOfWork.CommitTransaction();
            // Check if this can be deleted safely
            //if (this.Model!=null)
            //{
            //    Model.Dispose();
            //    GC.SuppressFinalize(false);
            //    //System.Diagnostics.Trace.Write(new Exception("repository:" + this.ToString()), "error");
            //}
        }

        private IUnitOfWork _unitOfWork;

        public IUnitOfWork UnitOfWork { get { return _unitOfWork; } }

        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            _unitOfWork.GetContext().Database.ExecuteSqlCommand(sql, parameters);
        }

        public System.Collections.IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters)
        {
            return _unitOfWork.GetContext().Database.SqlQuery(elementType, sql, parameters);
        }

        public IEnumerable<R> SqlQuery(string sql, params object[] parameters)
        {
            return _unitOfWork.GetContext().Database.SqlQuery<R>(sql, parameters);
        }
    }
}
