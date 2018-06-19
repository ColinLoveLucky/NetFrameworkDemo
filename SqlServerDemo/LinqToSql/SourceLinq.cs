using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDemo.LinqToSql
{
    public class SourceLinq
    {
        //        DataContext context;
        //        MetaModel metaModel;
        //        IdentityManager identifier;
        //        ChangeTracker tracker;
        //        ChangeDirector director;
        //        bool hasCachedObjects;
        //        Dictionary<MetaDataMember, IDeferredSourceFactory> factoryMap;

        //        internal CommonDataServices(DataContext context, MetaModel model)
        //        {
        //            this.context = context;
        //            this.metaModel = model;
        //            bool asReadOnly = !context.ObjectTrackingEnabled;
        //            this.identifier = IdentityManager.CreateIdentityManager(asReadOnly);
        //            this.tracker = ChangeTracker.CreateChangeTracker(this, asReadOnly);
        //            this.director = ChangeDirector.CreateChangeDirector(context);
        //            this.factoryMap = new Dictionary<MetaDataMember, IDeferredSourceFactory>();
        //        }

        //        public DataContext Context
        //        {
        //            get { return this.context; }
        //        }

        //        public MetaModel Model
        //        {
        //            get { return this.metaModel; }
        //        }

        //        internal void SetModel(MetaModel model)
        //        {
        //            this.metaModel = model;
        //        }

        //        internal IdentityManager IdentityManager
        //        {
        //            get { return this.identifier; }
        //        }

        //        internal ChangeTracker ChangeTracker
        //        {
        //            get { return this.tracker; }
        //        }

        //        internal ChangeDirector ChangeDirector
        //        {
        //            get { return this.director; }
        //        }

        //        internal IEnumerable<RelatedItem> GetParents(MetaType type, object item)
        //        {
        //            return this.GetRelations(type, item, true);
        //        }

        //        internal IEnumerable<RelatedItem> GetChildren(MetaType type, object item)
        //        {
        //            return this.GetRelations(type, item, false);
        //        }

        //        private IEnumerable<RelatedItem> GetRelations(MetaType type, object item, bool isForeignKey)
        //        {
        //            foreach (MetaDataMember mm in type.PersistentDataMembers)
        //            {
        //                if (mm.IsAssociation)
        //                {
        //                    MetaType otherType = mm.Association.OtherType;
        //                    if (mm.Association.IsForeignKey == isForeignKey)
        //                    {
        //                        object value = null;
        //                        if (mm.IsDeferred)
        //                        {
        //                            value = mm.DeferredValueAccessor.GetBoxedValue(item);
        //                        }
        //                        else
        //                        {
        //                            value = mm.StorageAccessor.GetBoxedValue(item);
        //                        }
        //                        if (value != null)
        //                        {
        //                            if (mm.Association.IsMany)
        //                            {
        //                                IEnumerable list = (IEnumerable)value;
        //                                foreach (object otherItem in list)
        //                                {
        //                                    yield return new RelatedItem(otherType.GetInheritanceType(otherItem.GetType()), otherItem);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                yield return new RelatedItem(otherType.GetInheritanceType(value.GetType()), value);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        internal void ResetServices()
        //        {
        //            hasCachedObjects = false;
        //            bool asReadOnly = !context.ObjectTrackingEnabled;
        //            this.identifier = IdentityManager.CreateIdentityManager(asReadOnly);
        //            this.tracker = ChangeTracker.CreateChangeTracker(this, asReadOnly);
        //            this.factoryMap = new Dictionary<MetaDataMember, IDeferredSourceFactory>();
        //        }

        //        internal static object[] GetKeyValues(MetaType type, object instance)
        //        {
        //            List<object> keyValues = new List<object>();
        //            foreach (MetaDataMember mm in type.IdentityMembers)
        //            {
        //                keyValues.Add(mm.MemberAccessor.GetBoxedValue(instance));
        //            }
        //            return keyValues.ToArray();
        //        }

        //        internal static object[] GetForeignKeyValues(MetaAssociation association, object instance)
        //        {
        //            List<object> keyValues = new List<object>();
        //            foreach (MetaDataMember mm in association.ThisKey)
        //            {
        //                keyValues.Add(mm.MemberAccessor.GetBoxedValue(instance));
        //            }
        //            return keyValues.ToArray();
        //        }

        //        internal object GetCachedObject(MetaType type, object[] keyValues)
        //        {
        //            if (type == null)
        //            {
        //                throw Error.ArgumentNull("type");
        //            }
        //            if (!type.IsEntity)
        //            {
        //                return null;
        //            }
        //            return this.identifier.Find(type, keyValues);
        //        }

        //        internal object GetCachedObjectLike(MetaType type, object instance)
        //        {
        //            if (type == null)
        //            {
        //                throw Error.ArgumentNull("type");
        //            }
        //            if (!type.IsEntity)
        //            {
        //                return null;
        //            }
        //            return this.identifier.FindLike(type, instance);
        //        }

        //        public bool IsCachedObject(MetaType type, object instance)
        //        {
        //            if (type == null)
        //            {
        //                throw Error.ArgumentNull("type");
        //            }
        //            if (!type.IsEntity)
        //            {
        //                return false;
        //            }
        //            return this.identifier.FindLike(type, instance) == instance;
        //        }

        //        public object InsertLookupCachedObject(MetaType type, object instance)
        //        {
        //            if (type == null)
        //            {
        //                throw Error.ArgumentNull("type");
        //            }
        //            hasCachedObjects = true;  // flag that we have cached objects
        //            if (!type.IsEntity)
        //            {
        //                return instance;
        //            }
        //            return this.identifier.InsertLookup(type, instance);
        //        }

        //        public bool RemoveCachedObjectLike(MetaType type, object instance)
        //        {
        //            if (type == null)
        //            {
        //                throw Error.ArgumentNull("type");
        //            }
        //            if (!type.IsEntity)
        //            {
        //                return false;
        //            }
        //            return this.identifier.RemoveLike(type, instance);
        //        }

        //        public void OnEntityMaterialized(MetaType type, object instance)
        //        {
        //            if (type == null)
        //            {
        //                throw Error.ArgumentNull("type");
        //            }
        //            this.tracker.FastTrack(instance);

        //            if (type.HasAnyLoadMethod)
        //            {
        //                SendOnLoaded(type, instance);
        //            }
        //        }

        //        private static void SendOnLoaded(MetaType type, object item)
        //        {
        //            if (type != null)
        //            {
        //                SendOnLoaded(type.InheritanceBase, item);

        //                if (type.OnLoadedMethod != null)
        //                {
        //                    try
        //                    {
        //                        type.OnLoadedMethod.Invoke(item, new object[] { });
        //                    }
        //                    catch (TargetInvocationException tie)
        //                    {
        //                        if (tie.InnerException != null)
        //                        {
        //                            throw tie.InnerException;
        //                        }

        //                        throw;
        //                    }
        //                }
        //            }
        //        }


        //        / <summary>
        //        / Returns a query for the entity indicated by the specified key.
        //        / </summary>
        //        internal Expression GetObjectQuery(MetaType type, object[] keyValues)
        //        {
        //            if (type == null)
        //            {
        //                throw Error.ArgumentNull("type");
        //            }
        //            if (keyValues == null)
        //            {
        //                throw Error.ArgumentNull("keyValues");
        //            }
        //            return this.GetObjectQuery(type, BuildKeyExpressions(keyValues, type.IdentityMembers));
        //        }

        //        internal Expression GetObjectQuery(MetaType type, Expression[] keyValues)
        //        {
        //            ITable table = this.context.GetTable(type.InheritanceRoot.Type);
        //            ParameterExpression serverItem = Expression.Parameter(table.ElementType, "p");

        //            create a where expression including all the identity members
        //           Expression whereExpression = null;
        //            for (int i = 0, n = type.IdentityMembers.Count; i < n; i++)
        //            {
        //                MetaDataMember metaMember = type.IdentityMembers[i];
        //                Expression memberExpression = (metaMember.Member is FieldInfo)
        //                    ? Expression.Field(serverItem, (FieldInfo)metaMember.Member)
        //                    : Expression.Property(serverItem, (PropertyInfo)metaMember.Member);
        //                Expression memberEqualityExpression = Expression.Equal(memberExpression, keyValues[i]);
        //                whereExpression = (whereExpression != null)
        //                    ? Expression.And(whereExpression, memberEqualityExpression)
        //                    : memberEqualityExpression;
        //            }
        //            return Expression.Call(typeof(Queryable), "Where", new Type[] { table.ElementType }, table.Expression, Expression.Lambda(whereExpression, serverItem));
        //        }

        //        internal Expression GetDataMemberQuery(MetaDataMember member, Expression[] keyValues)
        //        {
        //            if (member == null)
        //                throw Error.ArgumentNull("member");
        //            if (keyValues == null)
        //                throw Error.ArgumentNull("keyValues");
        //            if (member.IsAssociation)
        //            {
        //                MetaAssociation association = member.Association;
        //                Type rootType = association.ThisMember.DeclaringType.InheritanceRoot.Type;
        //                Expression thisSource = Expression.Constant(context.GetTable(rootType));
        //                if (rootType != association.ThisMember.DeclaringType.Type)
        //                {
        //                    thisSource = Expression.Call(typeof(Enumerable), "Cast", new Type[] { association.ThisMember.DeclaringType.Type }, thisSource);
        //                }
        //                Expression thisInstance = Expression.Call(typeof(Enumerable), "FirstOrDefault", new Type[] { association.ThisMember.DeclaringType.Type },
        //                    System.Data.Linq.SqlClient.Translator.WhereClauseFromSourceAndKeys(thisSource, association.ThisKey.ToArray(), keyValues)
        //                    );
        //                Expression otherSource = Expression.Constant(context.GetTable(association.OtherType.InheritanceRoot.Type));
        //                if (association.OtherType.Type != association.OtherType.InheritanceRoot.Type)
        //                {
        //                    otherSource = Expression.Call(typeof(Enumerable), "Cast", new Type[] { association.OtherType.Type }, otherSource);
        //                }
        //                Expression expr = System.Data.Linq.SqlClient.Translator.TranslateAssociation(
        //                    this.context, association, otherSource, keyValues, thisInstance
        //                    );
        //                return expr;
        //            }
        //            else
        //            {
        //                Expression query = this.GetObjectQuery(member.DeclaringType, keyValues);
        //                Type elementType = System.Data.Linq.SqlClient.TypeSystem.GetElementType(query.Type);
        //                ParameterExpression p = Expression.Parameter(elementType, "p");
        //                Expression e = p;
        //                if (elementType != member.DeclaringType.Type)
        //                    e = Expression.Convert(e, member.DeclaringType.Type);
        //                Expression mem = (member.Member is PropertyInfo)
        //                    ? Expression.Property(e, (PropertyInfo)member.Member)
        //                    : Expression.Field(e, (FieldInfo)member.Member);
        //                LambdaExpression selector = Expression.Lambda(mem, p);
        //                return Expression.Call(typeof(Queryable), "Select", new Type[] { elementType, selector.Body.Type }, query, selector);
        //            }
        //        }

        //        private static Expression[] BuildKeyExpressions(object[] keyValues, ReadOnlyCollection<MetaDataMember> keyMembers)
        //        {
        //            Expression[] keyValueExpressions = new Expression[keyValues.Length];
        //            for (int i = 0, n = keyMembers.Count; i < n; i++)
        //            {
        //                MetaDataMember metaMember = keyMembers[i];
        //                Expression keyValueExpression = Expression.Constant(keyValues[i], metaMember.Type);
        //                keyValueExpressions[i] = keyValueExpression;
        //            }
        //            return keyValueExpressions;
        //        }

        //        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        //        public IDeferredSourceFactory GetDeferredSourceFactory(MetaDataMember member)
        //        {
        //            if (member == null)
        //            {
        //                throw Error.ArgumentNull("member");
        //            }
        //            IDeferredSourceFactory factory;
        //            if (this.factoryMap.TryGetValue(member, out factory))
        //            {
        //                return factory;
        //            }
        //            Type elemType = member.IsAssociation && member.Association.IsMany
        //                ? System.Data.Linq.SqlClient.TypeSystem.GetElementType(member.Type)
        //                : member.Type;
        //            factory = (IDeferredSourceFactory)Activator.CreateInstance(
        //                typeof(DeferredSourceFactory<>).MakeGenericType(elemType),
        //                BindingFlags.Instance | BindingFlags.NonPublic, null,
        //                new object[] { member, this }, null
        //                );
        //            this.factoryMap.Add(member, factory);
        //            return factory;
        //        }

        //        class DeferredSourceFactory<T> : IDeferredSourceFactory
        //        {
        //            MetaDataMember member;
        //            CommonDataServices services;
        //            ICompiledQuery query;
        //            bool refersToPrimaryKey;
        //            T[] empty;

        //            internal DeferredSourceFactory(MetaDataMember member, CommonDataServices services)
        //            {
        //                this.member = member;
        //                this.services = services;
        //                this.refersToPrimaryKey = this.member.IsAssociation && this.member.Association.OtherKeyIsPrimaryKey;
        //                this.empty = new T[] { };
        //            }

        //            public IEnumerable CreateDeferredSource(object instance)
        //            {
        //                if (instance == null)
        //                    throw Error.ArgumentNull("instance");
        //                return new DeferredSource(this, instance);
        //            }

        //            public IEnumerable CreateDeferredSource(object[] keyValues)
        //            {
        //                if (keyValues == null)
        //                    throw Error.ArgumentNull("keyValues");
        //                return new DeferredSource(this, keyValues);
        //            }

        //            private IEnumerator<T> Execute(object instance)
        //            {
        //                ReadOnlyCollection<MetaDataMember> keys = null;
        //                if (this.member.IsAssociation)
        //                {
        //                    keys = this.member.Association.ThisKey;
        //                }
        //                else
        //                {
        //                    keys = this.member.DeclaringType.IdentityMembers;
        //                }
        //                object[] keyValues = new object[keys.Count];
        //                for (int i = 0, n = keys.Count; i < n; i++)
        //                {
        //                    object value = keys[i].StorageAccessor.GetBoxedValue(instance);
        //                    keyValues[i] = value;
        //                }

        //                if (this.HasNullForeignKey(keyValues))
        //                {
        //                    return ((IEnumerable<T>)this.empty).GetEnumerator();
        //                }

        //                T cached;
        //                if (this.TryGetCachedObject(keyValues, out cached))
        //                {
        //                    return ((IEnumerable<T>)(new T[] { cached })).GetEnumerator();
        //                }

        //                if (this.member.LoadMethod != null)
        //                {
        //                    try
        //                    {
        //                        object result = this.member.LoadMethod.Invoke(this.services.Context, new object[] { instance });
        //                        if (typeof(T).IsAssignableFrom(this.member.LoadMethod.ReturnType))
        //                        {
        //                            return ((IEnumerable<T>)new T[] { (T)result }).GetEnumerator();
        //                        }
        //                        else
        //                        {
        //                            return ((IEnumerable<T>)result).GetEnumerator();
        //                        }
        //                    }
        //                    catch (TargetInvocationException tie)
        //                    {
        //                        if (tie.InnerException != null)
        //                        {
        //                            throw tie.InnerException;
        //                        }
        //                        throw;
        //                    }
        //                }
        //                else
        //                {
        //                    return this.ExecuteKeyQuery(keyValues);
        //                }
        //            }

        //            private IEnumerator<T> ExecuteKeys(object[] keyValues)
        //            {
        //                if (this.HasNullForeignKey(keyValues))
        //                {
        //                    return ((IEnumerable<T>)this.empty).GetEnumerator();
        //                }

        //                T cached;
        //                if (this.TryGetCachedObject(keyValues, out cached))
        //                {
        //                    return ((IEnumerable<T>)(new T[] { cached })).GetEnumerator();
        //                }

        //                return this.ExecuteKeyQuery(keyValues);
        //            }

        //            private bool HasNullForeignKey(object[] keyValues)
        //            {
        //                if (this.refersToPrimaryKey)
        //                {
        //                    bool keyHasNull = false;
        //                    for (int i = 0, n = keyValues.Length; i < n; i++)
        //                    {
        //                        keyHasNull |= keyValues[i] == null;
        //                    }
        //                    if (keyHasNull)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            }

        //            private bool TryGetCachedObject(object[] keyValues, out T cached)
        //            {
        //                cached = default(T);
        //                if (this.refersToPrimaryKey)
        //                {
        //                    look to see if we already have this object in the identity cache
        //                   MetaType mt = this.member.IsAssociation ? this.member.Association.OtherType : this.member.DeclaringType;
        //                    object obj = this.services.GetCachedObject(mt, keyValues);
        //                    if (obj != null)
        //                    {
        //                        cached = (T)obj;
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            }

        //            private IEnumerator<T> ExecuteKeyQuery(object[] keyValues)
        //            {
        //                if (this.query == null)
        //                {
        //                    ParameterExpression p = Expression.Parameter(typeof(object[]), "keys");
        //                    Expression[] keyExprs = new Expression[keyValues.Length];
        //                    ReadOnlyCollection<MetaDataMember> members = this.member.IsAssociation ? this.member.Association.OtherKey : this.member.DeclaringType.IdentityMembers;
        //                    for (int i = 0, n = keyValues.Length; i < n; i++)
        //                    {
        //                        MetaDataMember mm = members[i];
        //                        keyExprs[i] = Expression.Convert(
        //#pragma warning disable 618 // Disable the 'obsolete' warning
        //                                          Expression.ArrayIndex(p, Expression.Constant(i)),
        //#pragma warning restore 618
        //                                          mm.Type
        //                                      );
        //                    }
        //                    Expression q = this.services.GetDataMemberQuery(this.member, keyExprs);
        //                    LambdaExpression lambda = Expression.Lambda(q, p);
        //                    this.query = this.services.Context.Provider.Compile(lambda);
        //                }
        //                return ((IEnumerable<T>)this.query.Execute(this.services.Context.Provider, new object[] { keyValues }).ReturnValue).GetEnumerator();
        //            }

        //            class DeferredSource : IEnumerable<T>, IEnumerable
        //            {
        //                DeferredSourceFactory<T> factory;
        //                object instance;

        //                internal DeferredSource(DeferredSourceFactory<T> factory, object instance)
        //                {
        //                    this.factory = factory;
        //                    this.instance = instance;
        //                }

        //                public IEnumerator<T> GetEnumerator()
        //                {
        //                    object[] keyValues = this.instance as object[];
        //                    if (keyValues != null)
        //                    {
        //                        return this.factory.ExecuteKeys(keyValues);
        //                    }
        //                    return this.factory.Execute(this.instance);
        //                }

        //                IEnumerator IEnumerable.GetEnumerator()
        //                {
        //                    return this.GetEnumerator();
        //                }
        //            }
        //        }

        //        / <summary>
        //        / Returns true if any objects have been added to the identity cache.If
        //        / object tracking is disabled, this still returns true if any attempts
        //        / where made to cache an object.  Thus regardless of object tracking mode,
        //        / this can be used as an indicator as to whether any result returning queries
        //        / have been executed.
        //        / </summary>
        //        internal bool HasCachedObjects
        //        {
        //            get
        //            {
        //                return this.hasCachedObjects;
        //            }
        //        }

        //        public object GetCachedObject(Expression query)
        //        {
        //            if (query == null)
        //                return null;
        //            MethodCallExpression mc = query as MethodCallExpression;
        //            if (mc == null || mc.Arguments.Count < 1 || mc.Arguments.Count > 2)
        //                return null;
        //            if (mc.Method.DeclaringType != typeof(Queryable))
        //            {
        //                return null;
        //            }
        //            switch (mc.Method.Name)
        //            {
        //                case "Where":
        //                case "First":
        //                case "FirstOrDefault":
        //                case "Single":
        //                case "SingleOrDefault":
        //                    break;
        //                default:
        //                    return null;
        //            }
        //            if (mc.Arguments.Count == 1)
        //            {
        //                If it is something like
        //                      context.Customers.Where(c => c.ID = 123).First()
        //                 then it is equivalent of
        //                      context.Customers.First(c => c.ID = 123)
        //                 hence reduce to context.Customers.Where(c => c.ID = 123) and process the remaining query
        //                return GetCachedObject(mc.Arguments[0]);
        //            }
        //            UnaryExpression quote = mc.Arguments[1] as UnaryExpression;
        //            if (quote == null || quote.NodeType != ExpressionType.Quote)
        //                return null;
        //            LambdaExpression pred = quote.Operand as LambdaExpression;
        //            if (pred == null)
        //                return null;
        //            ConstantExpression cex = mc.Arguments[0] as ConstantExpression;
        //            if (cex == null)
        //                return null;
        //            ITable t = cex.Value as ITable;
        //            if (t == null)
        //                return null;
        //            Type elementType = System.Data.Linq.SqlClient.TypeSystem.GetElementType(query.Type);
        //            if (elementType != t.ElementType)
        //                return null;
        //            MetaTable metaTable = this.metaModel.GetTable(t.ElementType);
        //            object[] keyValues = this.GetKeyValues(metaTable.RowType, pred);
        //            if (keyValues != null)
        //            {
        //                return this.GetCachedObject(metaTable.RowType, keyValues);
        //            }
        //            return null;
        //        }

        //        internal object[] GetKeyValues(MetaType type, LambdaExpression predicate)
        //        {
        //            if (predicate == null)
        //                throw Error.ArgumentNull("predicate");
        //            if (predicate.Parameters.Count != 1)
        //                return null;
        //            Dictionary<MetaDataMember, object> keys = new Dictionary<MetaDataMember, object>();
        //            if (this.GetKeysFromPredicate(type, keys, predicate.Body)
        //                && keys.Count == type.IdentityMembers.Count)
        //            {
        //                object[] values = keys.OrderBy(kv => kv.Key.Ordinal).Select(kv => kv.Value).ToArray();
        //                return values;
        //            }
        //            return null;
        //        }

        //        private bool GetKeysFromPredicate(MetaType type, Dictionary<MetaDataMember, object> keys, Expression expr)
        //        {
        //            BinaryExpression bex = expr as BinaryExpression;
        //            if (bex == null)
        //            {
        //                MethodCallExpression mex = expr as MethodCallExpression;
        //                if (mex != null && mex.Method.Name == "op_Equality" && mex.Arguments.Count == 2)
        //                {
        //                    bex = Expression.Equal(mex.Arguments[0], mex.Arguments[1]);
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            switch (bex.NodeType)
        //            {
        //                case ExpressionType.And:
        //                    return this.GetKeysFromPredicate(type, keys, bex.Left) &&
        //                           this.GetKeysFromPredicate(type, keys, bex.Right);
        //                case ExpressionType.Equal:
        //                    return GetKeyFromPredicate(type, keys, bex.Left, bex.Right) ||
        //                           GetKeyFromPredicate(type, keys, bex.Right, bex.Left);
        //                default:
        //                    return false;
        //            }
        //        }

        //        private static bool GetKeyFromPredicate(MetaType type, Dictionary<MetaDataMember, object> keys, Expression mex, Expression vex)
        //        {
        //            MemberExpression memex = mex as MemberExpression;
        //            if (memex == null || memex.Expression == null ||
        //                memex.Expression.NodeType != ExpressionType.Parameter || memex.Expression.Type != type.Type)
        //            {
        //                return false;
        //            }
        //            if (!type.Type.IsAssignableFrom(memex.Member.ReflectedType) && !memex.Member.ReflectedType.IsAssignableFrom(type.Type))
        //            {
        //                return false;
        //            }
        //            MetaDataMember mm = type.GetDataMember(memex.Member);
        //            if (!mm.IsPrimaryKey)
        //            {
        //                return false;
        //            }
        //            if (keys.ContainsKey(mm))
        //            {
        //                return false;
        //            }
        //            ConstantExpression cex = vex as ConstantExpression;
        //            if (cex != null)
        //            {
        //                keys.Add(mm, cex.Value);
        //                return true;
        //            }
        //            InvocationExpression ie = vex as InvocationExpression;
        //            if (ie != null && ie.Arguments != null && ie.Arguments.Count == 0)
        //            {
        //                ConstantExpression ce = ie.Expression as ConstantExpression;
        //                if (ce != null)
        //                {
        //                    keys.Add(mm, ((Delegate)ce.Value).DynamicInvoke(new object[] { }));
        //                    return true;
        //                }
        //            }
        //            return false;
        //        }

        //        / <summary>
        //        / Either returns the object from cache if it is in cache, or
        //        / queries for it.
        //        / </summary> 
        //        internal object GetObjectByKey(MetaType type, object[] keyValues)
        //        {
        //            first check the cache
        //            object target = GetCachedObject(type, keyValues);
        //            if (target == null)
        //            {
        //                no cached value, so query for it
        //               target = ((IEnumerable)this.context.Provider.Execute(this.GetObjectQuery(type, keyValues)).ReturnValue).OfType<object>().SingleOrDefault();
        //            }
        //            return target;
        //        }
    }
    public class DataContextDemo : IDisposable
    {
        CommonDataServices services;
        IProvider provider;
        Dictionary<MetaTable, ITable> tables;
        bool objectTrackingEnabled = true;
        bool deferredLoadingEnabled = true;
        bool disposed;
        bool isInSubmitChanges;
        DataLoadOptions loadOptions;
        ChangeConflictCollection conflicts;
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class CommonDataServices : IDataServices
    {
        DataContext context;
        MetaModel metaModel;
        IdentityManager identifier;
        ChangeTracker tracker;
        ChangeDirector director;
        bool hasCachedObjects;
        Dictionary<MetaDataMember, IDeferredSourceFactory> factoryMap;
        public DataContext Context
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public MetaModel Model
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        //public IDeferredSourceFactory GetDeferredSourceFactory(MetaDataMember member)
        //{
        //    throw new NotImplementedException();
        //}

        public object GetCachedObject(Expression query)
        {
            throw new NotImplementedException();
        }

        public bool IsCachedObject(MetaType type, object instance)
        {
            throw new NotImplementedException();
        }

        public object InsertLookupCachedObject(MetaType type, object instance)
        {
            throw new NotImplementedException();
        }

        public void OnEntityMaterialized(MetaType type, object instance)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class IdentityManager
    {
        internal abstract object InsertLookup(MetaType type, object instance);
        internal abstract bool RemoveLike(MetaType type, object instance);
        internal abstract object Find(MetaType type, object[] keyValues);
        internal abstract object FindLike(MetaType type, object instance);

        internal static IdentityManager CreateIdentityManager(bool asReadOnly)
        {
            if (asReadOnly)
            {
                return new ReadOnlyIdentityManager();
            }
            else
            {
                return new StandardIdentityManager();
            }
        }

        class StandardIdentityManager : IdentityManager
        {
            Dictionary<MetaType, IdentityCache> caches;
            IdentityCache currentCache;
            MetaType currentType;

            internal StandardIdentityManager()
            {
                this.caches = new Dictionary<MetaType, IdentityCache>();
            }

            internal override object InsertLookup(MetaType type, object instance)
            {
                this.SetCurrent(type);
                return this.currentCache.InsertLookup(instance);
            }

            internal override bool RemoveLike(MetaType type, object instance)
            {
                this.SetCurrent(type);
                return this.currentCache.RemoveLike(instance);
            }

            internal override object Find(MetaType type, object[] keyValues)
            {
                this.SetCurrent(type);
                return this.currentCache.Find(keyValues);
            }

            internal override object FindLike(MetaType type, object instance)
            {
                this.SetCurrent(type);
                return this.currentCache.FindLike(instance);
            }

            //   [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            private void SetCurrent(MetaType type)
            {
                type = type.InheritanceRoot;
                if (this.currentType != type)
                {
                    if (!this.caches.TryGetValue(type, out this.currentCache))
                    {
                        KeyManager km = GetKeyManager(type);
                        this.currentCache = (IdentityCache)Activator.CreateInstance(
                            typeof(IdentityCache<,>).MakeGenericType(type.Type, km.KeyType),
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                            new object[] { km }, null
                            );
                        this.caches.Add(type, this.currentCache);
                    }
                    this.currentType = type;
                }
            }

            // [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            static KeyManager GetKeyManager(MetaType type)
            {
                int n = type.IdentityMembers.Count;
                MetaDataMember mm = type.IdentityMembers[0];

                KeyManager km = (KeyManager)Activator.CreateInstance(
                            typeof(SingleKeyManager<,>).MakeGenericType(type.Type, mm.Type),
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                            new object[] { mm.StorageAccessor, 0 }, null
                            );
                for (int i = 1; i < n; i++)
                {
                    mm = type.IdentityMembers[i];
                    km = (KeyManager)
                        Activator.CreateInstance(
                            typeof(MultiKeyManager<,,>).MakeGenericType(type.Type, mm.Type, km.KeyType),
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                            new object[] { mm.StorageAccessor, i, km }, null
                            );
                }

                return km;
            }

            #region Nested type definitions
            // These types are internal rather than private to work around
            // CLR 


            internal abstract class KeyManager
            {
                internal abstract Type KeyType { get; }
            }

            internal abstract class KeyManager<T, K> : KeyManager
            {
                internal abstract K CreateKeyFromInstance(T instance);
                internal abstract bool TryCreateKeyFromValues(object[] values, out K k);
                internal abstract IEqualityComparer<K> Comparer { get; }
            }

            internal class SingleKeyManager<T, V> : KeyManager<T, V>
            {
                bool isKeyNullAssignable;
                MetaAccessor<T, V> accessor;
                int offset;
                IEqualityComparer<V> comparer;

                internal SingleKeyManager(MetaAccessor<T, V> accessor, int offset)
                {
                    this.accessor = accessor;
                    this.offset = offset;
                    //   this.isKeyNullAssignable = System.Data.Linq.SqlClient.TypeSystem.IsNullAssignable(typeof(V));
                }

                internal override V CreateKeyFromInstance(T instance)
                {
                    return this.accessor.GetValue(instance);
                }

                internal override bool TryCreateKeyFromValues(object[] values, out V v)
                {
                    object o = values[this.offset];
                    if (o == null && !this.isKeyNullAssignable)
                    {
                        v = default(V);
                        return false;
                    }
                    v = (V)o;
                    return true;
                }

                internal override Type KeyType
                {
                    get { return typeof(V); }
                }

                internal override IEqualityComparer<V> Comparer
                {
                    get
                    {
                        if (this.comparer == null)
                        {
                            this.comparer = EqualityComparer<V>.Default;
                        }
                        return this.comparer;
                    }
                }
            }

            internal class MultiKeyManager<T, V1, V2> : KeyManager<T, MultiKey<V1, V2>>
            {
                MetaAccessor<T, V1> accessor;
                int offset;
                KeyManager<T, V2> next;
                IEqualityComparer<MultiKey<V1, V2>> comparer;

                internal MultiKeyManager(MetaAccessor<T, V1> accessor, int offset, KeyManager<T, V2> next)
                {
                    this.accessor = accessor;
                    this.next = next;
                    this.offset = offset;
                }

                internal override MultiKey<V1, V2> CreateKeyFromInstance(T instance)
                {
                    return new MultiKey<V1, V2>(
                        this.accessor.GetValue(instance),
                        this.next.CreateKeyFromInstance(instance)
                        );
                }

                internal override bool TryCreateKeyFromValues(object[] values, out MultiKey<V1, V2> k)
                {
                    System.Diagnostics.Debug.Assert(this.offset < values.Length, "offset is outside the bounds of the values array");

                    object o = values[this.offset];
                    if (o == null && typeof(V1).IsValueType)
                    {
                        k = default(MultiKey<V1, V2>);
                        return false;
                    }
                    V2 v2;
                    if (!this.next.TryCreateKeyFromValues(values, out v2))
                    {
                        k = default(MultiKey<V1, V2>);
                        return false;
                    }
                    k = new MultiKey<V1, V2>((V1)o, v2);
                    return true;
                }

                internal override Type KeyType
                {
                    get { return typeof(MultiKey<V1, V2>); }
                }

                internal override IEqualityComparer<MultiKey<V1, V2>> Comparer
                {
                    get
                    {
                        if (this.comparer == null)
                        {
                            this.comparer = new MultiKey<V1, V2>.Comparer(EqualityComparer<V1>.Default, next.Comparer);
                        }
                        return this.comparer;
                    }
                }
            }

            internal struct MultiKey<T1, T2>
            {
                T1 value1;
                T2 value2;

                internal MultiKey(T1 value1, T2 value2)
                {
                    this.value1 = value1;
                    this.value2 = value2;
                }

                internal class Comparer : IEqualityComparer<MultiKey<T1, T2>>, IEqualityComparer
                {
                    IEqualityComparer<T1> comparer1;
                    IEqualityComparer<T2> comparer2;

                    internal Comparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2)
                    {
                        this.comparer1 = comparer1;
                        this.comparer2 = comparer2;
                    }

                    public bool Equals(MultiKey<T1, T2> x, MultiKey<T1, T2> y)
                    {
                        return this.comparer1.Equals(x.value1, y.value1) &&
                               this.comparer2.Equals(x.value2, y.value2);
                    }

                    public int GetHashCode(MultiKey<T1, T2> x)
                    {
                        return this.comparer1.GetHashCode(x.value1) ^ this.comparer2.GetHashCode(x.value2);
                    }

                    bool IEqualityComparer.Equals(object x, object y)
                    {
                        return this.Equals((MultiKey<T1, T2>)x, (MultiKey<T1, T2>)y);
                    }

                    int IEqualityComparer.GetHashCode(object x)
                    {
                        return this.GetHashCode((MultiKey<T1, T2>)x);
                    }
                }
            }

            internal abstract class IdentityCache
            {
                internal abstract object Find(object[] keyValues);
                internal abstract object FindLike(object instance);
                internal abstract object InsertLookup(object instance);
                internal abstract bool RemoveLike(object instance);
            }

            internal class IdentityCache<T, K> : IdentityCache
            {
                int[] buckets;
                Slot[] slots;
                int count;
                int freeList;
                KeyManager<T, K> keyManager;
                IEqualityComparer<K> comparer;

                public IdentityCache(KeyManager<T, K> keyManager)
                {
                    this.keyManager = keyManager;
                    this.comparer = keyManager.Comparer;
                    buckets = new int[7];
                    slots = new Slot[7];
                    freeList = -1;
                }

                internal override object InsertLookup(object instance)
                {
                    T value = (T)instance;
                    K key = this.keyManager.CreateKeyFromInstance(value);
                    Find(key, ref value, true);
                    return value;
                }

                internal override bool RemoveLike(object instance)
                {
                    T value = (T)instance;
                    K key = this.keyManager.CreateKeyFromInstance(value);

                    int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
                    int bucket = hashCode % buckets.Length;
                    int last = -1;
                    for (int i = buckets[bucket] - 1; i >= 0; last = i, i = slots[i].next)
                    {
                        if (slots[i].hashCode == hashCode && comparer.Equals(slots[i].key, key))
                        {
                            if (last < 0)
                            {
                                buckets[bucket] = slots[i].next + 1;
                            }
                            else
                            {
                                slots[last].next = slots[i].next;
                            }
                            slots[i].hashCode = -1;
                            slots[i].value = default(T);
                            slots[i].next = freeList;
                            freeList = i;
                            return true;
                        }
                    }
                    return false;
                }

                internal override object Find(object[] keyValues)
                {
                    K key;
                    if (this.keyManager.TryCreateKeyFromValues(keyValues, out key))
                    {
                        T value = default(T);
                        if (Find(key, ref value, false))
                            return value;
                    }
                    return null;
                }

                internal override object FindLike(object instance)
                {
                    T value = (T)instance;
                    K key = this.keyManager.CreateKeyFromInstance(value);
                    if (Find(key, ref value, false))
                        return value;
                    return null;
                }

                bool Find(K key, ref T value, bool add)
                {
                    int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
                    for (int i = buckets[hashCode % buckets.Length] - 1; i >= 0; i = slots[i].next)
                    {
                        if (slots[i].hashCode == hashCode && comparer.Equals(slots[i].key, key))
                        {
                            value = slots[i].value;
                            return true;
                        }
                    }
                    if (add)
                    {
                        int index;
                        if (freeList >= 0)
                        {
                            index = freeList;
                            freeList = slots[index].next;
                        }
                        else
                        {
                            if (count == slots.Length) Resize();
                            index = count;
                            count++;
                        }
                        int bucket = hashCode % buckets.Length;
                        slots[index].hashCode = hashCode;
                        slots[index].key = key;
                        slots[index].value = value;
                        slots[index].next = buckets[bucket] - 1;
                        buckets[bucket] = index + 1;
                    }
                    return false;
                }

                void Resize()
                {
                    int newSize = checked(count * 2 + 1);
                    int[] newBuckets = new int[newSize];
                    Slot[] newSlots = new Slot[newSize];
                    Array.Copy(slots, 0, newSlots, 0, count);
                    for (int i = 0; i < count; i++)
                    {
                        int bucket = newSlots[i].hashCode % newSize;
                        newSlots[i].next = newBuckets[bucket] - 1;
                        newBuckets[bucket] = i + 1;
                    }
                    buckets = newBuckets;
                    slots = newSlots;
                }

                internal struct Slot
                {
                    internal int hashCode;
                    internal K key;
                    internal T value;
                    internal int next;
                }
            }
            #endregion
        }

        /// <summary>
        /// This is the noop implementation used when object tracking is disabled.
        /// </summary>
        class ReadOnlyIdentityManager : IdentityManager
        {
            internal ReadOnlyIdentityManager() { }
            internal override object InsertLookup(MetaType type, object instance) { return instance; }
            internal override bool RemoveLike(MetaType type, object instance) { return false; }
            internal override object Find(MetaType type, object[] keyValues) { return null; }
            internal override object FindLike(MetaType type, object instance) { return null; }
        }
    }

    public abstract class ChangeTracker
    {
        /// <summary>
        /// Starts tracking an object as 'unchanged'
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal abstract TrackedObject Track(object obj);
        /// <summary>
        /// Starts tracking an object as 'unchanged', and optionally
        /// 'weakly' tracks all other referenced objects recursively.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="recurse">True if all untracked objects in the graph
        /// should be tracked recursively.</param>
        /// <returns></returns>
        internal abstract TrackedObject Track(object obj, bool recurse);
        /// <summary>
        /// Fast-tracks an object that is already in identity cache
        /// </summary>
        /// <param name="obj"></param>
        internal abstract void FastTrack(object obj);
        internal abstract bool IsTracked(object obj);
        internal abstract TrackedObject GetTrackedObject(object obj);
        internal abstract void StopTracking(object obj);
        internal abstract void AcceptChanges();
        internal abstract IEnumerable<TrackedObject> GetInterestingObjects();

        internal static ChangeTracker CreateChangeTracker(CommonDataServices dataServices, bool asReadOnly)
        {
            if (asReadOnly)
            {
                return new ReadOnlyChangeTracker();
            }
            else
            {
                return new StandardChangeTracker(dataServices);
            }
        }

        class StandardChangeTracker : ChangeTracker
        {
            Dictionary<object, StandardTrackedObject> items;
            //  PropertyChangingEventHandler onPropertyChanging;
            CommonDataServices services;

            internal StandardChangeTracker(CommonDataServices services)
            {
                this.services = services;
                this.items = new Dictionary<object, StandardTrackedObject>();
                // this.onPropertyChanging = new PropertyChangingEventHandler(this.OnPropertyChanging);
            }

            /// <summary>
            /// Given a type root and a discriminator, return the type that would be instantiated.
            /// </summary>
            private static MetaType TypeFromDiscriminator(MetaType root, object discriminator)
            {
                foreach (MetaType type in root.InheritanceTypes)
                {
                    if (IsSameDiscriminator(discriminator, type.InheritanceCode))
                        return type;
                }
                return root.InheritanceDefault;
            }

            private static bool IsSameDiscriminator(object discriminator1, object discriminator2)
            {
                if (discriminator1 == discriminator2)
                {
                    return true;
                }
                if (discriminator1 == null || discriminator2 == null)
                {
                    return false;
                }
                return discriminator1.Equals(discriminator2);
            }

            internal override TrackedObject Track(object obj)
            {
                return Track(obj, false);
            }

            internal override TrackedObject Track(object obj, bool recurse)
            {
                MetaType type = this.services.Model.GetMetaType(obj.GetType());
                Dictionary<object, object> visited = new Dictionary<object, object>();
                return Track(type, obj, visited, recurse, 1);
            }

            private TrackedObject Track(MetaType mt, object obj, Dictionary<object, object> visited, bool recurse, int level)
            {
                StandardTrackedObject tracked = (StandardTrackedObject)this.GetTrackedObject(obj);
                if (tracked != null || visited.ContainsKey(obj))
                {
                    return tracked;
                }

                // The root object tracked is tracked normally - all other objects
                // in the reference graph are weakly tracked.
                bool weaklyTrack = level > 1;
                tracked = new StandardTrackedObject(this, mt, obj, obj, weaklyTrack);
                if (tracked.HasDeferredLoaders)
                {
                    ///   throw Error.CannotAttachAddNonNewEntities();
                }
                this.items.Add(obj, tracked);
                this.Attach(obj);
                visited.Add(obj, obj);

                if (recurse)
                {
                    // track parents (objects we are dependent on)
                    //foreach (RelatedItem parent in this.services.GetParents(mt, obj))
                    //{
                    //    this.Track(parent.Type, parent.Item, visited, recurse, level + 1);
                    //}

                    //// track children (objects that are dependent on us)
                    //foreach (RelatedItem child in this.services.GetChildren(mt, obj))
                    //{
                    //    this.Track(child.Type, child.Item, visited, recurse, level + 1);
                    //}
                }

                return tracked;
            }

            internal override void FastTrack(object obj)
            {
                // assumes object is already in identity cache
                this.Attach(obj);
            }

            internal override void StopTracking(object obj)
            {
                this.Detach(obj);
                this.items.Remove(obj);
            }

            internal override bool IsTracked(object obj)
            {
                return this.items.ContainsKey(obj) || this.IsFastTracked(obj);
            }

            private bool IsFastTracked(object obj)
            {
                MetaType type = this.services.Model.GetTable(obj.GetType()).RowType;
                return this.services.IsCachedObject(type, obj);
            }

            internal override TrackedObject GetTrackedObject(object obj)
            {
                StandardTrackedObject ti;
                if (!this.items.TryGetValue(obj, out ti))
                {
                    if (this.IsFastTracked(obj))
                    {
                        return this.PromoteFastTrackedObject(obj);
                    }
                }
                return ti;
            }

            private StandardTrackedObject PromoteFastTrackedObject(object obj)
            {
                Type type = obj.GetType();
                MetaType metaType = this.services.Model.GetTable(type).RowType.GetInheritanceType(type);
                return this.PromoteFastTrackedObject(metaType, obj);
            }

            private StandardTrackedObject PromoteFastTrackedObject(MetaType type, object obj)
            {
                StandardTrackedObject ti = new StandardTrackedObject(this, type, obj, obj);
                this.items.Add(obj, ti);
                return ti;
            }

            private void Attach(object obj)
            {
                //INotifyPropertyChanging notifier = obj as INotifyPropertyChanging;
                //if (notifier != null)
                //{
                //    notifier.PropertyChanging += this.onPropertyChanging;
                //}
                //else
                //{
                //    // if has no notifier, consider it modified already
                //    this.OnPropertyChanging(obj, null);
                //}
            }

            private void Detach(object obj)
            {
                ////INotifyPropertyChanging notifier = obj as INotifyPropertyChanging;
                //if (notifier != null)
                //{
                //  //  notifier.PropertyChanging -= this.onPropertyChanging;
                //}
            }

            //private void OnPropertyChanging(object sender, PropertyChangingEventArgs args)
            //{
            //    StandardTrackedObject ti;
            //    if (this.items.TryGetValue(sender, out ti))
            //    {
            //        ti.StartTracking();
            //    }
            //    else if (this.IsFastTracked(sender))
            //    {
            //        ti = this.PromoteFastTrackedObject(sender);
            //        ti.StartTracking();
            //    }
            //}

            internal override void AcceptChanges()
            {
                List<StandardTrackedObject> list = new List<StandardTrackedObject>((IEnumerable<StandardTrackedObject>)this.items.Values);
                foreach (TrackedObject item in list)
                {
                    item.AcceptChanges();
                }
            }

            internal override IEnumerable<TrackedObject> GetInterestingObjects()
            {
                foreach (StandardTrackedObject ti in this.items.Values)
                {
                    if (ti.IsInteresting)
                    {
                        yield return ti;
                    }
                }
            }

            public class StandardTrackedObject : TrackedObject
            {
                private StandardChangeTracker tracker;
                private MetaType type;
                private object current;
                private object original;
                private State state;
                private BitArray dirtyMemberCache;
                private bool haveInitializedDeferredLoaders;
                private bool isWeaklyTracked;

                enum State
                {
                    New,
                    Deleted,
                    PossiblyModified,
                    Modified,
                    Removed,
                    Dead
                }

                public override string ToString()
                {
                    return type.Name + ":" + GetState();
                }

                private string GetState()
                {
                    switch (this.state)
                    {
                        case State.New:
                        case State.Deleted:
                        case State.Dead:
                        case State.Removed:
                            return this.state.ToString();
                        default:
                            if (this.IsModified)
                            {
                                return "Modified";
                            }
                            else
                            {
                                return "Unmodified";
                            }
                    }
                }

                internal StandardTrackedObject(StandardChangeTracker tracker, MetaType type, object current, object original)
                {
                    if (current == null)
                    {
                        // throw Error.ArgumentNull("current");
                    }
                    this.tracker = tracker;
                    this.type = type.GetInheritanceType(current.GetType());
                    this.current = current;
                    this.original = original;
                    this.state = State.PossiblyModified;
                    dirtyMemberCache = new BitArray(this.type.DataMembers.Count);
                }

                internal StandardTrackedObject(StandardChangeTracker tracker, MetaType type, object current, object original, bool isWeaklyTracked)
                    : this(tracker, type, current, original)
                {
                    this.isWeaklyTracked = isWeaklyTracked;
                }

                internal override bool IsWeaklyTracked
                {
                    get { return isWeaklyTracked; }
                }

                internal override MetaType Type
                {
                    get { return this.type; }
                }

                internal override object Current
                {
                    get { return this.current; }
                }

                internal override object Original
                {
                    get { return this.original; }
                }

                internal override bool IsNew
                {
                    get { return this.state == State.New; }
                }

                internal override bool IsDeleted
                {
                    get { return this.state == State.Deleted; }
                }

                internal override bool IsRemoved
                {
                    get { return this.state == State.Removed; }
                }

                internal override bool IsDead
                {
                    get { return this.state == State.Dead; }
                }

                internal override bool IsModified
                {
                    get { return this.state == State.Modified || (this.state == State.PossiblyModified && this.current != this.original && this.HasChangedValues()); }
                }

                internal override bool IsUnmodified
                {
                    get { return this.state == State.PossiblyModified && (this.current == this.original || !this.HasChangedValues()); }
                }

                internal override bool IsPossiblyModified
                {
                    get { return this.state == State.Modified || this.state == State.PossiblyModified; }
                }

                internal override bool CanInferDelete()
                {
                    // A delete can be inferred iff there is a non-nullable singleton association that has 
                    // been set to null, and the association has DeleteOnNull = true.
                    if (this.state == State.Modified || this.state == State.PossiblyModified)
                    {
                        foreach (MetaAssociation assoc in Type.Associations)
                        {
                            if (assoc.DeleteOnNull && assoc.IsForeignKey && !assoc.IsNullable && !assoc.IsMany &&
                                assoc.ThisMember.StorageAccessor.HasAssignedValue(Current) &&
                                assoc.ThisMember.StorageAccessor.GetBoxedValue(Current) == null)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }

                internal override bool IsInteresting
                {
                    get
                    {
                        return this.state == State.New ||
                               this.state == State.Deleted ||
                               this.state == State.Modified ||
                               (this.state == State.PossiblyModified && this.current != this.original) ||
                               CanInferDelete();
                    }
                }

                internal override void ConvertToNew()
                {
                    // must be new or unmodified or removed to convert to new
                    System.Diagnostics.Debug.Assert(this.IsNew || this.IsRemoved || this.IsUnmodified);
                    this.original = null;
                    this.state = State.New;
                }

                internal override void ConvertToPossiblyModified()
                {
                    System.Diagnostics.Debug.Assert(this.IsPossiblyModified || this.IsDeleted);
                    this.state = State.PossiblyModified;
                    this.isWeaklyTracked = false;
                }

                internal override void ConvertToModified()
                {
                    System.Diagnostics.Debug.Assert(this.IsPossiblyModified);
                    System.Diagnostics.Debug.Assert(this.type.VersionMember != null || !this.type.HasUpdateCheck);
                    this.state = State.Modified;
                    this.isWeaklyTracked = false;
                }

                internal override void ConvertToPossiblyModified(object originalState)
                {
                    // must be modified or unmodified to convert to modified
                    System.Diagnostics.Debug.Assert(this.IsNew || this.IsPossiblyModified);
                    System.Diagnostics.Debug.Assert(originalState != null);
                    System.Diagnostics.Debug.Assert(originalState.GetType() == this.type.Type);
                    this.state = State.PossiblyModified;
                    this.original = this.CreateDataCopy(originalState);
                    this.isWeaklyTracked = false;
                }

                internal override void ConvertToDeleted()
                {
                    // must be modified or unmodified to be deleted
                    System.Diagnostics.Debug.Assert(this.IsDeleted || this.IsPossiblyModified);
                    this.state = State.Deleted;
                    this.isWeaklyTracked = false;
                }

                internal override void ConvertToDead()
                {
                    System.Diagnostics.Debug.Assert(this.IsDead || this.IsDeleted);
                    this.state = State.Dead;
                    this.isWeaklyTracked = false;
                }

                internal override void ConvertToRemoved()
                {
                    System.Diagnostics.Debug.Assert(this.IsRemoved || this.IsNew);
                    this.state = State.Removed;
                    this.isWeaklyTracked = false;
                }

                internal override void ConvertToUnmodified()
                {
                    System.Diagnostics.Debug.Assert(this.IsNew || this.IsPossiblyModified);
                    // reset to unmodified
                    this.state = State.PossiblyModified;
                    //if (this.current is INotifyPropertyChanging)
                    //{
                    //    this.original = this.current;
                    //}
                    //else
                    //{
                    //    this.original = this.CreateDataCopy(this.current);
                    //}
                    this.ResetDirtyMemberTracking();
                    this.isWeaklyTracked = false;
                }

                internal override void AcceptChanges()
                {
                    if (IsWeaklyTracked)
                    {
                        InitializeDeferredLoaders();
                        isWeaklyTracked = false;
                    }
                    if (this.IsDeleted)
                    {
                        this.ConvertToDead();
                    }
                    else if (this.IsNew)
                    {
                        this.InitializeDeferredLoaders();
                        this.ConvertToUnmodified();
                    }
                    else if (this.IsPossiblyModified)
                    {
                        this.ConvertToUnmodified();
                    }
                }

                private void AssignMember(object instance, MetaDataMember mm, object value)
                {
                    // In the unnotified case, directly use the storage accessor
                    // for everything because there are not events to be fired.
                    //if (!(this.current is INotifyPropertyChanging))
                    //{
                    //    mm.StorageAccessor.SetBoxedValue(ref instance, value);
                    //}
                    //else
                    //{
                    //    // Go through the member accessor to fire events.
                    //    mm.MemberAccessor.SetBoxedValue(ref instance, value);
                    //}
                }

                /// <summary>
                /// Certain state is saved during change tracking to enable modifications
                /// to be detected taking refresh operations into account.  When changes
                /// are reverted or accepted, this state must be reset.
                /// </summary>
                private void ResetDirtyMemberTracking()
                {
                    this.dirtyMemberCache.SetAll(false);
                }

                /// <summary>
                /// Refresh internal tracking state using the original value and mode
                /// specified.
                /// </summary>        
                internal override void Refresh(RefreshMode mode, object freshInstance)
                {
                    this.SynchDependentData();

                    // This must be done prior to updating original values
                    this.UpdateDirtyMemberCache();

                    // Apply the refresh strategy to each data member
                    Type instanceType = freshInstance.GetType();
                    foreach (MetaDataMember mm in type.PersistentDataMembers)
                    {
                        var memberMode = mm.IsDbGenerated ? RefreshMode.OverwriteCurrentValues : mode;
                        if (memberMode != RefreshMode.KeepCurrentValues)
                        {
                            if (!mm.IsAssociation && (this.Type.Type == instanceType || mm.DeclaringType.Type.IsAssignableFrom(instanceType)))
                            {
                                object freshValue = mm.StorageAccessor.GetBoxedValue(freshInstance);
                                this.RefreshMember(mm, memberMode, freshValue);
                            }
                        }
                    }

                    // Make the new data the current original value
                    this.original = this.CreateDataCopy(freshInstance);

                    if (mode == RefreshMode.OverwriteCurrentValues)
                    {
                        this.ResetDirtyMemberTracking();
                    }
                }

                /// <summary>
                /// Using the last saved comparison baseline, figure out which members have
                /// changed since the last refresh, and save that information.  This must be
                /// done BEFORE any merge operations modify the current values.
                /// </summary>
                private void UpdateDirtyMemberCache()
                {
                    // iterate over all members, and if they differ from 
                    // last read values, mark as dirty           
                    foreach (MetaDataMember mm in type.PersistentDataMembers)
                    {
                        if (mm.IsAssociation && mm.Association.IsMany)
                        {
                            continue;
                        }
                        if (!this.dirtyMemberCache.Get(mm.Ordinal) && this.HasChangedValue(mm))
                        {
                            this.dirtyMemberCache.Set(mm.Ordinal, true);
                        }
                    }
                }

                internal override void RefreshMember(MetaDataMember mm, RefreshMode mode, object freshValue)
                {
                    System.Diagnostics.Debug.Assert(!mm.IsAssociation);

                    if (mode == RefreshMode.KeepCurrentValues)
                    {
                        return;
                    }

                    bool hasUserChange = this.HasChangedValue(mm);

                    // we don't want to overwrite any modified values, unless
                    // the mode is original wins                
                    if (hasUserChange && mode != RefreshMode.OverwriteCurrentValues)
                        return;

                    object currentValue = mm.StorageAccessor.GetBoxedValue(this.current);
                    if (!object.Equals(freshValue, currentValue))
                    {
                        mm.StorageAccessor.SetBoxedValue(ref this.current, freshValue);

                        // update all singleton associations that are affected by a change to this member
                        foreach (MetaDataMember am in this.GetAssociationsForKey(mm))
                        {
                            if (!am.Association.IsMany)
                            {
                                //IEnumerable ds = this.tracker.services.GetDeferredSourceFactory(am).CreateDeferredSource(this.current);
                                //if (am.StorageAccessor.HasValue(this.current))
                                //{
                                //    this.AssignMember(this.current, am, ds.Cast<Object>().SingleOrDefault());
                                //}
                            }
                        }
                    }
                }

                private IEnumerable<MetaDataMember> GetAssociationsForKey(MetaDataMember key)
                {
                    foreach (MetaDataMember mm in this.type.PersistentDataMembers)
                    {
                        if (mm.IsAssociation && mm.Association.ThisKey.Contains(key))
                        {
                            yield return mm;
                        }
                    }
                }

                internal override object CreateDataCopy(object instance)
                {
                    System.Diagnostics.Debug.Assert(instance != null);
                    Type instanceType = instance.GetType();
                    System.Diagnostics.Debug.Assert(instance.GetType() == this.type.Type);

                    object copy = Activator.CreateInstance(this.Type.Type);

                    MetaType rootMetaType = this.tracker.services.Model.GetTable(instanceType).RowType.InheritanceRoot;
                    foreach (MetaDataMember mm in rootMetaType.GetInheritanceType(instanceType).PersistentDataMembers)
                    {
                        if (this.Type.Type != instanceType && !mm.DeclaringType.Type.IsAssignableFrom(instanceType))
                        {
                            continue;
                        }
                        if (mm.IsDeferred)
                        {
                            // do not copy associations
                            if (!mm.IsAssociation)
                            {
                                if (mm.StorageAccessor.HasValue(instance))
                                {
                                    object value = mm.DeferredValueAccessor.GetBoxedValue(instance);
                                    mm.DeferredValueAccessor.SetBoxedValue(ref copy, value);
                                }
                                else
                                {
                                    //IEnumerable ds = this.tracker.services.GetDeferredSourceFactory(mm).CreateDeferredSource(copy);
                                    //mm.DeferredSourceAccessor.SetBoxedValue(ref copy, ds);
                                }
                            }
                        }
                        else
                        {
                            // otherwise assign the value as-is to the backup instance
                            object value = mm.StorageAccessor.GetBoxedValue(instance);
                            // assumes member values are immutable or will communicate changes to entity
                            // note: byte[] and char[] don't do this. 
                            mm.StorageAccessor.SetBoxedValue(ref copy, value);
                        }
                    }
                    return copy;
                }

                internal void StartTracking()
                {
                    if (this.original == this.current)
                    {
                        this.original = this.CreateDataCopy(this.current);
                    }
                }

                // Return value indicates whether or not any data was actually sync'd
                internal override bool SynchDependentData()
                {
                    bool valueWasSet = false;

                    // set foreign key fields
                    foreach (MetaAssociation assoc in this.Type.Associations)
                    {
                        MetaDataMember mm = assoc.ThisMember;
                        if (assoc.IsForeignKey)
                        {
                            bool hasAssigned = mm.StorageAccessor.HasAssignedValue(this.current);
                            bool hasLoaded = mm.StorageAccessor.HasLoadedValue(this.current);
                            if (hasAssigned || hasLoaded)
                            {
                                object parent = mm.StorageAccessor.GetBoxedValue(this.current);
                                if (parent != null)
                                {
                                    // copy parent's current primary key into this instance's foreign key fields
                                    for (int i = 0, n = assoc.ThisKey.Count; i < n; i++)
                                    {
                                        MetaDataMember accThis = assoc.ThisKey[i];
                                        MetaDataMember accParent = assoc.OtherKey[i];
                                        object parentValue = accParent.StorageAccessor.GetBoxedValue(parent);
                                        accThis.StorageAccessor.SetBoxedValue(ref this.current, parentValue);
                                        valueWasSet = true;
                                    }
                                }
                                else if (assoc.IsNullable)
                                {
                                    if (mm.IsDeferred || (this.original != null && mm.MemberAccessor.GetBoxedValue(this.original) != null))
                                    {
                                        // no known parent? set to null
                                        for (int i = 0, n = assoc.ThisKey.Count; i < n; i++)
                                        {
                                            MetaDataMember accThis = assoc.ThisKey[i];
                                            if (accThis.CanBeNull)
                                            {
                                                if (this.original != null && this.HasChangedValue(accThis))
                                                {
                                                    if (accThis.StorageAccessor.GetBoxedValue(this.current) != null)
                                                    {
                                                        //   throw Error.InconsistentAssociationAndKeyChange(accThis.Member.Name, mm.Member.Name);
                                                    }
                                                }
                                                else
                                                {
                                                    accThis.StorageAccessor.SetBoxedValue(ref this.current, null);
                                                    valueWasSet = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (!hasLoaded)
                                {
                                    //Else the parent association has been set to null; but the ID is not nullable so
                                    //the value can not be set
                                    StringBuilder keys = new StringBuilder();
                                    foreach (MetaDataMember key in assoc.ThisKey)
                                    {
                                        if (keys.Length > 0)
                                        {
                                            keys.Append(", ");
                                        }
                                        keys.AppendFormat("{0}.{1}", this.Type.Name.ToString(), key.Name);
                                    }
                                    // throw Error.CouldNotRemoveRelationshipBecauseOneSideCannotBeNull(assoc.OtherType.Name, this.Type.Name, keys);
                                }
                            }
                        }
                    }

                    /// Explicitly set any inheritance discriminator for item.
                    if (this.type.HasInheritance)
                    {
                        if (this.original != null)
                        {
                            object currentDiscriminator = type.Discriminator.MemberAccessor.GetBoxedValue(this.current);
                            MetaType currentTypeFromDiscriminator = TypeFromDiscriminator(this.type, currentDiscriminator);
                            object dbDiscriminator = type.Discriminator.MemberAccessor.GetBoxedValue(this.original);
                            MetaType dbTypeFromDiscriminator = TypeFromDiscriminator(this.type, dbDiscriminator);

                            // Would the discriminator change also change the type? If so, its not allowed.
                            if (currentTypeFromDiscriminator != dbTypeFromDiscriminator)
                            {
                                //throw Error.CannotChangeInheritanceType(dbDiscriminator,
                                //    currentDiscriminator, original.GetType().Name, currentTypeFromDiscriminator);
                            }
                        }
                        else
                        {
                            // No db value means this is an 'Add'. Set the discriminator.
                            MetaType currentType = type.GetInheritanceType(this.current.GetType());
                            if (currentType.HasInheritanceCode)
                            {
                                object code = currentType.InheritanceCode;
                                this.type.Discriminator.MemberAccessor.SetBoxedValue(ref current, code);
                                valueWasSet = true;
                            }
                        }
                    }
                    return valueWasSet;
                }

                internal override bool HasChangedValue(MetaDataMember mm)
                {
                    if (this.current == this.original)
                    {
                        return false;
                    }
                    if (mm.IsAssociation && mm.Association.IsMany)
                    {
                        return mm.StorageAccessor.HasAssignedValue(this.original);
                    }
                    if (mm.StorageAccessor.HasValue(this.current))
                    {
                        if (this.original != null && mm.StorageAccessor.HasValue(this.original))
                        {
                            // If the member has ever been in a modified state
                            // in the past, it is considered modified
                            if (dirtyMemberCache.Get(mm.Ordinal))
                            {
                                return true;
                            }
                            object baseline = mm.MemberAccessor.GetBoxedValue(this.original);
                            object currentValue = mm.MemberAccessor.GetBoxedValue(this.current);
                            if (!object.Equals(currentValue, baseline))
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (mm.IsDeferred && mm.StorageAccessor.HasAssignedValue(this.current))
                        {
                            return true;
                        }
                    }
                    return false;
                }

                internal override bool HasChangedValues()
                {
                    if (this.current == this.original)
                    {
                        return false;
                    }
                    if (this.IsNew)
                    {
                        return true;
                    }
                    foreach (MetaDataMember mm in this.type.PersistentDataMembers)
                    {
                        if (!mm.IsAssociation && this.HasChangedValue(mm))
                        {
                            return true;
                        }
                    }
                    return false;
                }

                //internal override IEnumerable<ModifiedMemberInfo> GetModifiedMembers()
                //{
                //    foreach (MetaDataMember mm in this.type.PersistentDataMembers)
                //    {
                //        if (this.IsModifiedMember(mm))
                //        {
                //            object currentValue = mm.MemberAccessor.GetBoxedValue(this.current);
                //            if (this.original != null && mm.StorageAccessor.HasValue(this.original))
                //            {
                //                //object originalValue = mm.MemberAccessor.GetBoxedValue(this.original);
                //                //yield return new ModifiedMemberInfo(mm.Member, currentValue, originalValue);
                //            }
                //            else if (this.original == null || (mm.IsDeferred && !mm.StorageAccessor.HasLoadedValue(this.current)))
                //            {
                //               // yield return new ModifiedMemberInfo(mm.Member, currentValue, null);
                //            }
                //        }
                //    }
                //}

                private bool IsModifiedMember(MetaDataMember member)
                {
                    return !member.IsAssociation &&
                           !member.IsPrimaryKey &&
                           !member.IsVersion &&
                           !member.IsDbGenerated &&
                            member.StorageAccessor.HasAssignedValue(this.current) &&
                           (this.state == State.Modified ||
                           (this.state == State.PossiblyModified && this.HasChangedValue(member)));
                }

                internal override bool HasDeferredLoaders
                {
                    get
                    {
                        foreach (MetaAssociation assoc in this.Type.Associations)
                        {
                            if (HasDeferredLoader(assoc.ThisMember))
                            {
                                return true;
                            }
                        }
                        IEnumerable<MetaDataMember> deferredMembers = this.Type.PersistentDataMembers.Where(p => p.IsDeferred && !p.IsAssociation);
                        foreach (MetaDataMember deferredMember in deferredMembers)
                        {
                            if (HasDeferredLoader(deferredMember))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }

                private bool HasDeferredLoader(MetaDataMember deferredMember)
                {
                    if (!deferredMember.IsDeferred)
                    {
                        return false;
                    }

                    MetaAccessor acc = deferredMember.StorageAccessor;
                    if (acc.HasAssignedValue(this.current) || acc.HasLoadedValue(this.current))
                    {
                        return false;
                    }
                    MetaAccessor dsacc = deferredMember.DeferredSourceAccessor;
                    IEnumerable loader = (IEnumerable)dsacc.GetBoxedValue(this.current);

                    return loader != null;
                }

                /// <summary>
                /// Called to initialize deferred loaders for New or Attached entities.
                /// </summary>
                internal override void InitializeDeferredLoaders()
                {
                    if (this.tracker.services.Context.DeferredLoadingEnabled)
                    {
                        foreach (MetaAssociation assoc in this.Type.Associations)
                        {
                            // don't set loader on association that is dependent on unrealized generated values
                            if (!this.IsPendingGeneration(assoc.ThisKey))
                            {
                                InitializeDeferredLoader(assoc.ThisMember);
                            }
                        }
                        IEnumerable<MetaDataMember> deferredMembers = this.Type.PersistentDataMembers.Where(p => p.IsDeferred && !p.IsAssociation);
                        foreach (MetaDataMember deferredMember in deferredMembers)
                        {
                            // don't set loader on member that is dependent on unrealized generated values
                            if (!this.IsPendingGeneration(Type.IdentityMembers))
                            {
                                InitializeDeferredLoader(deferredMember);
                            }
                        }
                        haveInitializedDeferredLoaders = true;
                    }
                }

                private void InitializeDeferredLoader(MetaDataMember deferredMember)
                {
                    MetaAccessor acc = deferredMember.StorageAccessor;
                    if (!acc.HasAssignedValue(this.current) && !acc.HasLoadedValue(this.current))
                    {
                        MetaAccessor dsacc = deferredMember.DeferredSourceAccessor;
                        IEnumerable loader = (IEnumerable)dsacc.GetBoxedValue(this.current);
                        // don't reset loader on any deferred member that already has one
                        if (loader == null)
                        {
                            // IDeferredSourceFactory factory = this.tracker.services.GetDeferredSourceFactory(deferredMember);
                            //  loader = factory.CreateDeferredSource(this.current);
                            // dsacc.SetBoxedValue(ref this.current, loader);

                        }
                        else if (loader != null && !haveInitializedDeferredLoaders)
                        {
                            // If loader is present but wasn't generated by us, then
                            // an attempt to Attach or Add an entity from another context
                            // has been made, which is not supported.
                            //  throw Error.CannotAttachAddNonNewEntities();
                        }
                    }
                }

                internal override bool IsPendingGeneration(IEnumerable<MetaDataMember> key)
                {
                    if (this.IsNew)
                    {
                        foreach (MetaDataMember member in key)
                        {
                            if (IsMemberPendingGeneration(member))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }

                internal override bool IsMemberPendingGeneration(MetaDataMember keyMember)
                {
                    if (this.IsNew && keyMember.IsDbGenerated)
                    {
                        return true;
                    }
                    // look for any FK association that has this key member (should only be one)
                    foreach (MetaAssociation assoc in type.Associations)
                    {
                        if (assoc.IsForeignKey)
                        {
                            int index = assoc.ThisKey.IndexOf(keyMember);
                            if (index > -1)
                            {
                                // we must have a reference to this other object to know if its side of 
                                // the association is generated or not
                                object otherItem = null;
                                if (assoc.ThisMember.IsDeferred)
                                {
                                    otherItem = assoc.ThisMember.DeferredValueAccessor.GetBoxedValue(this.current);
                                }
                                else
                                {
                                    otherItem = assoc.ThisMember.StorageAccessor.GetBoxedValue(this.current);
                                }
                                if (otherItem != null)
                                {
                                    if (assoc.IsMany)
                                    {
                                        // Can't be pending generation for a value that would have to be the same
                                        // across many rows.
                                        continue;
                                    }
                                    else
                                    {
                                        StandardTrackedObject trackedOther = (StandardTrackedObject)this.tracker.GetTrackedObject(otherItem);
                                        if (trackedOther != null)
                                        {
                                            MetaDataMember otherMember = assoc.OtherKey[index];
                                            return trackedOther.IsMemberPendingGeneration(otherMember);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return false;
                }

                internal override IEnumerable<ModifiedMemberInfo> GetModifiedMembers()
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// This is the implementation used when change tracking is disabled.
        /// </summary>
        class ReadOnlyChangeTracker : ChangeTracker
        {
            internal override TrackedObject Track(object obj) { return null; }
            internal override TrackedObject Track(object obj, bool recurse) { return null; }
            internal override void FastTrack(object obj) { }
            internal override bool IsTracked(object obj) { return false; }
            internal override TrackedObject GetTrackedObject(object obj) { return null; }
            internal override void StopTracking(object obj) { }
            internal override void AcceptChanges() { }
            internal override IEnumerable<TrackedObject> GetInterestingObjects() { return new TrackedObject[0]; }
        }
    }

    public abstract class TrackedObject
    {
        internal abstract MetaType Type { get; }
        /// <summary>
        /// The current client value.
        /// </summary>
        internal abstract object Current { get; }
        /// <summary>
        /// The last read database value.  This is updated whenever the
        /// item is refreshed.
        /// </summary>
        internal abstract object Original { get; }
        internal abstract bool IsInteresting { get; } // new, deleted or possibly changed
        internal abstract bool IsNew { get; }
        internal abstract bool IsDeleted { get; }
        internal abstract bool IsModified { get; }
        internal abstract bool IsUnmodified { get; }
        internal abstract bool IsPossiblyModified { get; }
        internal abstract bool IsRemoved { get; }
        internal abstract bool IsDead { get; }
        /// <summary>
        /// True if the object is being tracked (perhaps during a recursive
        /// attach operation) but can be transitioned to other states.
        /// </summary>
        internal abstract bool IsWeaklyTracked { get; }
        internal abstract bool HasDeferredLoaders { get; }
        internal abstract bool HasChangedValues();
        internal abstract IEnumerable<ModifiedMemberInfo> GetModifiedMembers();
        internal abstract bool HasChangedValue(MetaDataMember mm);
        internal abstract bool CanInferDelete();
        internal abstract void AcceptChanges();
        internal abstract void ConvertToNew();
        internal abstract void ConvertToPossiblyModified();
        internal abstract void ConvertToPossiblyModified(object original);
        internal abstract void ConvertToUnmodified();
        internal abstract void ConvertToModified();
        internal abstract void ConvertToDeleted();
        internal abstract void ConvertToRemoved();
        internal abstract void ConvertToDead();
        /// <summary>
        /// Refresh the item by making the value passed in the current 
        /// Database value, and refreshing the current values using the
        /// mode specified.
        /// </summary>       
        internal abstract void Refresh(RefreshMode mode, object freshInstance);
        /// <summary>
        /// Does the refresh operation for a single member.  This method does not 
        /// update the baseline 'original' value.  You must call 
        /// Refresh(RefreshMode.KeepCurrentValues, freshInstance) to finish the refresh 
        /// after refreshing individual members.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="mode"></param>
        /// <param name="freshValue"></param>
        internal abstract void RefreshMember(MetaDataMember member, RefreshMode mode, object freshValue);
        /// <summary>
        /// Create a data-member only copy of the instance (no associations)
        /// </summary>
        /// <returns></returns>
        internal abstract object CreateDataCopy(object instance);

        internal abstract bool SynchDependentData();

        internal abstract bool IsPendingGeneration(IEnumerable<MetaDataMember> keyMembers);
        internal abstract bool IsMemberPendingGeneration(MetaDataMember keyMember);

        internal abstract void InitializeDeferredLoaders();

    }

    public abstract class ChangeDirector
    {
        internal abstract int Insert(TrackedObject item);
        internal abstract int DynamicInsert(TrackedObject item);
        internal abstract void AppendInsertText(TrackedObject item, StringBuilder appendTo);

        internal abstract int Update(TrackedObject item);
        internal abstract int DynamicUpdate(TrackedObject item);
        internal abstract void AppendUpdateText(TrackedObject item, StringBuilder appendTo);

        internal abstract int Delete(TrackedObject item);
        internal abstract int DynamicDelete(TrackedObject item);
        internal abstract void AppendDeleteText(TrackedObject item, StringBuilder appendTo);

        internal abstract void RollbackAutoSync();
        internal abstract void ClearAutoSyncRollback();

        internal static ChangeDirector CreateChangeDirector(DataContext context)
        {
            return new StandardChangeDirector(context);
        }

        /// <summary>
        /// Implementation of ChangeDirector which calls user code if possible 
        /// and othewise falls back to creating SQL for 'INSERT', 'UPDATE' and 'DELETE'.
        /// </summary>
        internal class StandardChangeDirector : ChangeDirector
        {
            private enum UpdateType { Insert, Update, Delete };
            private enum AutoSyncBehavior { ApplyNewAutoSync, RollbackSavedValues }

            DataContext context;
            // [SuppressMessage("Microsoft.MSInternal", "CA908:AvoidTypesThatRequireJitCompilationInPrecompiledAssemblies", Justification = "Microsoft: FxCop bug Dev10:423110 -- List<KeyValuePair<object, object>> is not supposed to be flagged as a violation.")]
            List<KeyValuePair<TrackedObject, object[]>> syncRollbackItems;

            internal StandardChangeDirector(DataContext context)
            {
                this.context = context;
            }

            //[SuppressMessage("Microsoft.MSInternal", "CA908:AvoidTypesThatRequireJitCompilationInPrecompiledAssemblies", Justification = "Microsoft: FxCop bug Dev10:423110 -- List<KeyValuePair<object, object>> is not supposed to be flagged as a violation.")]
            private List<KeyValuePair<TrackedObject, object[]>> SyncRollbackItems
            {
                get
                {
                    if (syncRollbackItems == null)
                    {
                        syncRollbackItems = new List<KeyValuePair<TrackedObject, object[]>>();
                    }
                    return syncRollbackItems;
                }
            }

            internal override int Insert(TrackedObject item)
            {
                if (item.Type.Table.InsertMethod != null)
                {
                    try
                    {
                        item.Type.Table.InsertMethod.Invoke(this.context, new object[] { item.Current });
                    }
                    catch (TargetInvocationException tie)
                    {
                        if (tie.InnerException != null)
                        {
                            throw tie.InnerException;
                        }
                        throw;
                    }
                    return 1;
                }
                else
                {
                    return DynamicInsert(item);
                }
            }

            internal override int DynamicInsert(TrackedObject item)
            {
                Expression cmd = this.GetInsertCommand(item);
                if (cmd.Type == typeof(int))
                {
                    // return (int)this.context.Provider.Execute(cmd).ReturnValue;
                }
                else
                {
                    //IEnumerable<object> facts = (IEnumerable<object>)this.context.Provider.Execute(cmd).ReturnValue;
                    //object[] syncResults = (object[])facts.FirstOrDefault();
                    //if (syncResults != null)
                    //{
                    //    // sync any auto gen or computed members
                    //    AutoSyncMembers(syncResults, item, UpdateType.Insert, AutoSyncBehavior.ApplyNewAutoSync);
                    //    return 1;
                    //}
                    //else
                    //{
                    //    throw Error.InsertAutoSyncFailure();
                    //}
                }

                return 0;
            }

            internal override void AppendInsertText(TrackedObject item, StringBuilder appendTo)
            {
                //if (item.Type.Table.InsertMethod != null)
                //{
                //    appendTo.Append(Strings.InsertCallbackComment);
                //}
                //else
                //{
                //    Expression cmd = this.GetInsertCommand(item);
                //    appendTo.Append(this.context.Provider.GetQueryText(cmd));
                //    appendTo.AppendLine();
                //}
            }

            /// <summary>
            /// Update the item, returning 0 if the update fails, 1 if it succeeds.
            /// </summary>        
            internal override int Update(TrackedObject item)
            {
                if (item.Type.Table.UpdateMethod != null)
                {
                    // create a copy - don't allow the override to modify our
                    // internal original values
                    try
                    {
                        item.Type.Table.UpdateMethod.Invoke(this.context, new object[] { item.Current });
                    }
                    catch (TargetInvocationException tie)
                    {
                        if (tie.InnerException != null)
                        {
                            throw tie.InnerException;
                        }
                        throw;
                    }
                    return 1;
                }
                else
                {
                    return DynamicUpdate(item);
                }
            }

            internal override int DynamicUpdate(TrackedObject item)
            {
                Expression cmd = this.GetUpdateCommand(item);
                if (cmd.Type == typeof(int))
                {
                    // return (int)this.context.Provider.Execute(cmd).ReturnValue;
                }
                else
                {
                    //IEnumerable<object> facts = (IEnumerable<object>)this.context.Provider.Execute(cmd).ReturnValue;
                    //object[] syncResults = (object[])facts.FirstOrDefault();
                    //if (syncResults != null)
                    //{
                    //    // sync any auto gen or computed members
                    //    AutoSyncMembers(syncResults, item, UpdateType.Update, AutoSyncBehavior.ApplyNewAutoSync);
                    //    return 1;
                    //}
                    //else
                    //{
                    //    return 0;
                    //}
                }

                return 0;
            }

            internal override void AppendUpdateText(TrackedObject item, StringBuilder appendTo)
            {
                if (item.Type.Table.UpdateMethod != null)
                {
                    // appendTo.Append(Strings.UpdateCallbackComment);
                }
                else
                {
                    //Expression cmd = this.GetUpdateCommand(item);
                    //appendTo.Append(this.context.Provider.GetQueryText(cmd));
                    //appendTo.AppendLine();
                }
            }

            internal override int Delete(TrackedObject item)
            {
                if (item.Type.Table.DeleteMethod != null)
                {
                    try
                    {
                        item.Type.Table.DeleteMethod.Invoke(this.context, new object[] { item.Current });
                    }
                    catch (TargetInvocationException tie)
                    {
                        if (tie.InnerException != null)
                        {
                            throw tie.InnerException;
                        }
                        throw;
                    }
                    return 1;
                }
                else
                {
                    return DynamicDelete(item);
                }
            }

            internal override int DynamicDelete(TrackedObject item)
            {
                Expression cmd = this.GetDeleteCommand(item);
                // int ret = (int)this.context.Provider.Execute(cmd).ReturnValue;
                //if (ret == 0)
                //{
                //    // we don't yet know if the delete failed because the check constaint did not match
                //    // or item was already deleted.  Verify the item exists
                //    cmd = this.GetDeleteVerificationCommand(item);
                //   // ret = ((int?)this.context.Provider.Execute(cmd).ReturnValue) ?? -1;
                //}
                //return ret;

                return 0;
            }

            internal override void AppendDeleteText(TrackedObject item, StringBuilder appendTo)
            {
                if (item.Type.Table.DeleteMethod != null)
                {
                    // appendTo.Append(Strings.DeleteCallbackComment);
                }
                else
                {
                    Expression cmd = this.GetDeleteCommand(item);
                    // appendTo.Append(this.context.Provider.GetQueryText(cmd));
                    appendTo.AppendLine();
                }
            }

            // [SuppressMessage("Microsoft.MSInternal", "CA908:AvoidTypesThatRequireJitCompilationInPrecompiledAssemblies", Justification = "Microsoft: FxCop bug Dev10:423110 -- List<KeyValuePair<object, object>> is not supposed to be flagged as a violation.")]
            internal override void RollbackAutoSync()
            {
                // Rolls back any AutoSync values that may have been set already
                // Those values are no longer valid since the transaction will be rolled back on the server
                if (this.syncRollbackItems != null)
                {
                    foreach (KeyValuePair<TrackedObject, object[]> rollbackItemPair in this.SyncRollbackItems)
                    {
                        TrackedObject rollbackItem = rollbackItemPair.Key;
                        object[] rollbackValues = rollbackItemPair.Value;

                        AutoSyncMembers(
                            rollbackValues,
                            rollbackItem,
                            rollbackItem.IsNew ? UpdateType.Insert : UpdateType.Update,
                            AutoSyncBehavior.RollbackSavedValues);
                    }
                }
            }

            //   [SuppressMessage("Microsoft.MSInternal", "CA908:AvoidTypesThatRequireJitCompilationInPrecompiledAssemblies", Justification = "Microsoft: FxCop bug Dev10:423110 -- List<KeyValuePair<object, object>> is not supposed to be flagged as a violation.")]
            internal override void ClearAutoSyncRollback()
            {
                this.syncRollbackItems = null;
            }

            private Expression GetInsertCommand(TrackedObject item)
            {
                MetaType mt = item.Type;

                // bind to InsertFacts if there are any members to syncronize
                List<MetaDataMember> membersToSync = GetAutoSyncMembers(mt, UpdateType.Insert);
                ParameterExpression p = Expression.Parameter(item.Type.Table.RowType.Type, "p");
                if (membersToSync.Count > 0)
                {
                    Expression autoSync = this.CreateAutoSync(membersToSync, p);
                    LambdaExpression resultSelector = Expression.Lambda(autoSync, p);
                    return Expression.Call(typeof(DataManipulation), "Insert", new Type[] { item.Type.InheritanceRoot.Type, resultSelector.Body.Type }, Expression.Constant(item.Current), resultSelector);
                }
                else
                {
                    return Expression.Call(typeof(DataManipulation), "Insert", new Type[] { item.Type.InheritanceRoot.Type }, Expression.Constant(item.Current));
                }
            }

            /// <summary>
            /// For the meta members specified, create an array initializer for each and bind to
            /// an output array.
            /// </summary>
            private Expression CreateAutoSync(List<MetaDataMember> membersToSync, Expression source)
            {
                System.Diagnostics.Debug.Assert(membersToSync.Count > 0);
                int i = 0;
                Expression[] initializers = new Expression[membersToSync.Count];
                foreach (MetaDataMember mm in membersToSync)
                {
                    initializers[i++] = Expression.Convert(this.GetMemberExpression(source, mm.Member), typeof(object));
                }
                return Expression.NewArrayInit(typeof(object), initializers);
            }

            private static List<MetaDataMember> GetAutoSyncMembers(MetaType metaType, UpdateType updateType)
            {
                List<MetaDataMember> membersToSync = new List<MetaDataMember>();
                foreach (MetaDataMember metaMember in metaType.PersistentDataMembers.OrderBy(m => m.Ordinal))
                {
                    // add all auto generated members for the specified update type to the auto-sync list
                    if ((updateType == UpdateType.Insert && metaMember.AutoSync == AutoSync.OnInsert) ||
                        (updateType == UpdateType.Update && metaMember.AutoSync == AutoSync.OnUpdate) ||
                         metaMember.AutoSync == AutoSync.Always)
                    {
                        membersToSync.Add(metaMember);
                    }
                }
                return membersToSync;
            }

            /// <summary>
            /// Synchronize the specified item by copying in data from the specified results.
            /// Used to sync members after successful insert or update, but also used to rollback to previous values if a failure
            /// occurs on other entities in the same SubmitChanges batch.
            /// </summary>
            /// <param name="autoSyncBehavior">
            /// If AutoSyncBehavior.ApplyNewAutoSync, the current value of the property is saved before the sync occurs. This is used for normal synchronization after a successful update/insert.
            /// Otherwise, the current value is not saved. This is used for rollback operations when something in the SubmitChanges batch failed, rendering the previously-sync'd values invalid.
            /// </param>
           // [SuppressMessage("Microsoft.MSInternal", "CA908:AvoidTypesThatRequireJitCompilationInPrecompiledAssemblies", Justification = "Microsoft: FxCop bug Dev10:423110 -- List<KeyValuePair<object, object>> is not supposed to be flagged as a violation.")]
            private void AutoSyncMembers(object[] syncResults, TrackedObject item, UpdateType updateType, AutoSyncBehavior autoSyncBehavior)
            {
                System.Diagnostics.Debug.Assert(item != null);
                System.Diagnostics.Debug.Assert(item.IsNew || item.IsPossiblyModified, "AutoSyncMembers should only be called for new and modified objects.");
                object[] syncRollbackValues = null;
                if (syncResults != null)
                {
                    int idx = 0;
                    List<MetaDataMember> membersToSync = GetAutoSyncMembers(item.Type, updateType);
                    System.Diagnostics.Debug.Assert(syncResults.Length == membersToSync.Count);
                    if (autoSyncBehavior == AutoSyncBehavior.ApplyNewAutoSync)
                    {
                        syncRollbackValues = new object[syncResults.Length];
                    }
                    foreach (MetaDataMember mm in membersToSync)
                    {
                        object value = syncResults[idx];
                        object current = item.Current;
                        MetaAccessor accessor =
                            (mm.Member is PropertyInfo && ((PropertyInfo)mm.Member).CanWrite)
                                ? mm.MemberAccessor
                                : mm.StorageAccessor;

                        if (syncRollbackValues != null)
                        {
                            syncRollbackValues[idx] = accessor.GetBoxedValue(current);
                        }
                        accessor.SetBoxedValue(ref current, DBConvert.ChangeType(value, mm.Type));
                        idx++;
                    }
                }
                if (syncRollbackValues != null)
                {
                    this.SyncRollbackItems.Add(new KeyValuePair<TrackedObject, object[]>(item, syncRollbackValues));
                }
            }

            private Expression GetUpdateCommand(TrackedObject tracked)
            {
                object database = tracked.Original;
                MetaType rowType = tracked.Type.GetInheritanceType(database.GetType());
                MetaType rowTypeRoot = rowType.InheritanceRoot;

                ParameterExpression p = Expression.Parameter(rowTypeRoot.Type, "p");
                Expression pv = p;
                if (rowType != rowTypeRoot)
                {
                    pv = Expression.Convert(p, rowType.Type);
                }

                Expression check = this.GetUpdateCheck(pv, tracked);
                if (check != null)
                {
                    check = Expression.Lambda(check, p);
                }

                // bind to out array if there are any members to synchronize
                List<MetaDataMember> membersToSync = GetAutoSyncMembers(rowType, UpdateType.Update);
                if (membersToSync.Count > 0)
                {
                    Expression autoSync = this.CreateAutoSync(membersToSync, pv);
                    LambdaExpression resultSelector = Expression.Lambda(autoSync, p);
                    if (check != null)
                    {
                        return Expression.Call(typeof(DataManipulation), "Update", new Type[] { rowTypeRoot.Type, resultSelector.Body.Type }, Expression.Constant(tracked.Current), check, resultSelector);
                    }
                    else
                    {
                        return Expression.Call(typeof(DataManipulation), "Update", new Type[] { rowTypeRoot.Type, resultSelector.Body.Type }, Expression.Constant(tracked.Current), resultSelector);
                    }
                }
                else if (check != null)
                {
                    return Expression.Call(typeof(DataManipulation), "Update", new Type[] { rowTypeRoot.Type }, Expression.Constant(tracked.Current), check);
                }
                else
                {
                    return Expression.Call(typeof(DataManipulation), "Update", new Type[] { rowTypeRoot.Type }, Expression.Constant(tracked.Current));
                }
            }

            private Expression GetUpdateCheck(Expression serverItem, TrackedObject tracked)
            {
                MetaType mt = tracked.Type;
                if (mt.VersionMember != null)
                {
                    return Expression.Equal(
                        this.GetMemberExpression(serverItem, mt.VersionMember.Member),
                        this.GetMemberExpression(Expression.Constant(tracked.Current), mt.VersionMember.Member)
                        );
                }
                else
                {
                    Expression expr = null;
                    foreach (MetaDataMember mm in mt.PersistentDataMembers)
                    {
                        if (!mm.IsPrimaryKey)
                        {
                            UpdateCheck check = mm.UpdateCheck;
                            if (check == UpdateCheck.Always ||
                                (check == UpdateCheck.WhenChanged && tracked.HasChangedValue(mm)))
                            {
                                object memberValue = mm.MemberAccessor.GetBoxedValue(tracked.Original);
                                Expression eq =
                                    Expression.Equal(
                                        this.GetMemberExpression(serverItem, mm.Member),
                                        Expression.Constant(memberValue, mm.Type)
                                        );
                                expr = (expr != null) ? Expression.And(expr, eq) : eq;
                            }
                        }
                    }
                    return expr;
                }
            }

            private Expression GetDeleteCommand(TrackedObject tracked)
            {
                MetaType rowType = tracked.Type;
                MetaType rowTypeRoot = rowType.InheritanceRoot;
                ParameterExpression p = Expression.Parameter(rowTypeRoot.Type, "p");
                Expression pv = p;
                if (rowType != rowTypeRoot)
                {
                    pv = Expression.Convert(p, rowType.Type);
                }
                object original = tracked.CreateDataCopy(tracked.Original);
                Expression check = this.GetUpdateCheck(pv, tracked);
                if (check != null)
                {
                    check = Expression.Lambda(check, p);
                    return Expression.Call(typeof(DataManipulation), "Delete", new Type[] { rowTypeRoot.Type }, Expression.Constant(original), check);
                }
                else
                {
                    return Expression.Call(typeof(DataManipulation), "Delete", new Type[] { rowTypeRoot.Type }, Expression.Constant(original));
                }
            }

            private Expression GetDeleteVerificationCommand(TrackedObject tracked)
            {
                ITable table = this.context.GetTable(tracked.Type.InheritanceRoot.Type);
                System.Diagnostics.Debug.Assert(table != null);
                ParameterExpression p = Expression.Parameter(table.ElementType, "p");
                Expression pred = Expression.Lambda(Expression.Equal(p, Expression.Constant(tracked.Current)), p);
                Expression where = Expression.Call(typeof(Queryable), "Where", new Type[] { table.ElementType }, table.Expression, pred);
                Expression selector = Expression.Lambda(Expression.Constant(0, typeof(int?)), p);
                Expression select = Expression.Call(typeof(Queryable), "Select", new Type[] { table.ElementType, typeof(int?) }, where, selector);
                Expression singleOrDefault = Expression.Call(typeof(Queryable), "SingleOrDefault", new Type[] { typeof(int?) }, select);
                return singleOrDefault;
            }

            //  [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
            private Expression GetMemberExpression(Expression exp, MemberInfo mi)
            {
                FieldInfo fi = mi as FieldInfo;
                if (fi != null)
                    return Expression.Field(exp, fi);
                PropertyInfo pi = (PropertyInfo)mi;
                return Expression.Property(exp, pi);
            }
        }
    }

    interface IProvider : IDisposable
    {
        /// <summary>
        /// Initializes the database provider with the data services object and connection.
        /// </summary>
        /// <param name="dataServices"></param>
        /// <param name="connection">A connection string, connection object or transaction object 
        /// used to seed the provider with database connection information.</param>
        void Initialize(IDataServices dataServices, object connection);

        /// <summary>
        /// The text writer used by the provider to output information such as query and commands
        /// being executed.
        /// </summary>
        TextWriter Log { get; set; }

        /// <summary>
        /// The connection object used by the provider when executing queries and commands.
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// The transaction object used by the provider when executing queries and commands.
        /// </summary>
        DbTransaction Transaction { get; set; }

        /// <summary>
        /// The command timeout setting to use for command execution.
        /// </summary>
        int CommandTimeout { get; set; }

        /// <summary>
        /// Clears the connection of any current activity.
        /// </summary>
        void ClearConnection();

        /// <summary>
        /// Creates a new database instance (catalog or file) at the location specified by the connection
        /// using the metadata encoded within the entities or mapping file.
        /// </summary>
        void CreateDatabase();

        /// <summary>
        /// Deletes the database instance at the location specified by the connection.
        /// </summary>
        void DeleteDatabase();

        /// <summary>
        /// Returns true if the database specified by the connection object exists.
        /// </summary>
        /// <returns></returns>
        bool DatabaseExists();

        /// <summary>
        /// Executes the query specified as a LINQ expression tree.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A result object from which you can obtain the return value and output parameters.</returns>
        IExecuteResult Execute(Expression query);

        /// <summary>
        /// Compiles the query specified as a LINQ expression tree.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A compiled query instance.</returns>
        ICompiledQuery Compile(Expression query);

        /// <summary>
        /// Translates a DbDataReader into a sequence of objects (entity or projection) by mapping
        /// columns of the data reader to object members by name.
        /// </summary>
        /// <param name="elementType">The type of the resulting objects.</param>
        /// <param name="reader"></param>
        /// <returns></returns>
        IEnumerable Translate(Type elementType, DbDataReader reader);

        /// <summary>
        /// Translates an IDataReader containing multiple result sets into sequences of objects
        /// (entity or projection) by mapping columns of the data reader to object members by name.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        IMultipleResults Translate(DbDataReader reader);

        /// <summary>
        /// Returns the query text in the database server's native query language
        /// that would need to be executed to perform the specified query.
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns></returns>
        string GetQueryText(Expression query);

        /// <summary>
        /// Return an IDbCommand object representing the translation of specified query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DbCommand GetCommand(Expression query);
    }

    interface ICompiledQuery
    {
        /// <summary>
        /// Executes the compiled query using the specified provider and a set of arguments.
        /// </summary>
        /// <param name="provider">The provider that will execute the compiled query.</param>
        /// <param name="arguments">Argument values to supply to the parameters of the compiled query, 
        /// when the query is specified as a LambdaExpression.</param>
        /// <returns></returns>
        IExecuteResult Execute(IProvider provider, object[] arguments);
    }

    internal static class DataManipulation
    {
        /// <summary>
        /// The method signature used to encode an Insert command.
        /// The method will throw a NotImplementedException if called directly.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="item"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static TResult Insert<TEntity, TResult>(TEntity item, Func<TEntity, TResult> resultSelector)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// The method signature used to encode an Insert command.
        /// The method will throw a NotImplementedException if called directly.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>

        public static int Insert<TEntity>(TEntity item)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// The method signature used to encode an Update command.
        /// The method will throw a NotImplementedException if called directly.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="item"></param>
        /// <param name="check"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>

        public static TResult Update<TEntity, TResult>(TEntity item, Func<TEntity, bool> check, Func<TEntity, TResult> resultSelector)
        {
            throw new NotImplementedException();
        }
    }

    interface IDataServices
    {
        DataContext Context { get; }
        MetaModel Model { get; }
        //   IDeferredSourceFactory GetDeferredSourceFactory(MetaDataMember member);
        object GetCachedObject(Expression query);
        bool IsCachedObject(MetaType type, object instance);
        object InsertLookupCachedObject(MetaType type, object instance);
        void OnEntityMaterialized(MetaType type, object instance);
    }

    interface IDeferredSourceFactory
    {
        IEnumerable CreateDeferredSource(object instance);
        IEnumerable CreateDeferredSource(object[] keyValues);
    }

    internal struct RelatedItem
    {
        internal MetaType Type;
        internal object Item;
        internal RelatedItem(MetaType type, object item)
        {
            this.Type = type;
            this.Item = item;
        }
    }
}


