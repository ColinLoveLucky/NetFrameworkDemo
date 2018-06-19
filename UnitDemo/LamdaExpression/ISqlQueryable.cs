using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.LamdaExpression
{
    public class ISqlQueryable
    {
        public void ShowQuery()
        {
            IDataBase db = new DBSql();
            var names = (from item in db.Source<Staff>()
                         where item.Name == "zhangsan"
                         select item).ToList();


        }
    }
    public interface IDataBase
    {
        List<T> FindAs<T>(Expression<Func<T, bool>> lambdaWhere);
        int Remove<T>(Expression<Func<T, bool>> lambdaWhere);
        IQueryable<T> Source<T>();
    }

    public class DBSql : IDataBase
    {
        public List<T> FindAs<T>(Expression<Func<T, bool>> func)
        {
            BinaryExpression binary = func.Body as BinaryExpression;
            string left = ResovleFunc(binary.Left);
            string right = ResovleLinqToObject(binary.Right);
            string oper = GetOperator(binary.NodeType);
            string sql = string.Format("select * from {0} where {1}", typeof(T).Name, left + oper + right);

            Console.WriteLine(sql);

            return null;
        }
        public string ResovleFunc(Expression express)
        {
            //var inner = express as BinaryExpression;

            //string name = (inner.Left as MemberExpression).Member.Name;

            //object value = (inner.Right as ConstantExpression).Value;

            //var oper = GetOperator(inner.NodeType);

            //string result = string.Format("{0} {1} '{2}'", name, oper, value);

            string result = string.Empty;

            result = (express as MemberExpression).Member.Name;

            return result;
        }
        public string GetOperator(ExpressionType expressionType)
        {

            switch (expressionType)
            {
                case ExpressionType.Add:
                    return "and";
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                    return "or";
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                default:
                    throw new Exception(string.Format("不支持{0 此种运算符查找！", expressionType));
            }
        }
        public string ResovleLinqToObject(Expression expression)
        {
            //var methodCall = expression as MethodCallExpression;

            //var methodName = methodCall.Method.Name;

            //var result = string.Empty;

            //if (methodName == "Contains")
            //{
            //    object temp_Value = (methodCall.Arguments[0] as ConstantExpression).Value;

            //    string value = string.Format("%{0}%", temp_Value);

            //    string name = (methodCall.Object as MemberExpression).Member.Name;

            //    result = string.Format("{0} like '{1}'", name, value);
            //}

            string result = string.Empty;

            result = (expression as ConstantExpression).Value.ToString();

            return result;

        }
        public void ShowConvertSql()
        {
            string[] names = { "Andy", "Amy", "Mike" };

            Expression<Func<Staff, bool>> func =
                x => (!names.Contains(x.Name) && (x.Name == "A") || x.Name.Count() > 5);

            ResolveExpress resolve = new ResolveExpress();

            resolve.ResolveExpression(func);

            Console.WriteLine(resolve.sqlWhere);

            foreach (var item in resolve.paras)
            {
                Console.WriteLine(item.ParameterName + ":" + item.Value);
            }




        }
        public int Remove<T>(Expression<Func<T, bool>> lambdaWhere)
        {
            throw new NotImplementedException();
        }
        public IQueryable<T> Source<T>()
        {
            return new SqlQuery<T>();
        }
    }

    public class SqlQuery<T> : IQueryable<T>
    {
        private Expression _expression;
        private IQueryProvider _provider;
        public SqlQuery()
        {
            _provider = new SqlProvider<T>();
            _expression = Expression.Constant(this);
        }
        public IEnumerator<T> GetEnumerator()
        {
            var result = _provider.Execute<List<T>>(_expression);
            if (result == null)
                yield break;
            foreach (var item in result)
            {
                yield return item;
            }
        }
        public SqlQuery(Expression expression, IQueryProvider provider)
        {
            _expression = expression;
            _provider = provider;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        public Type ElementType
        {
            get
            {
                return typeof(SqlQuery<T>);
            }
        }
        public Expression Expression
        {
            get
            {
                return _expression;
            }
        }
        public IQueryProvider Provider
        {
            get
            {
                return _provider;
            }
        }
    }

    public class SqlProvider<T> : IQueryProvider
    {
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            IQueryable<TElement> query = new SqlQuery<TElement>(expression, this);
            return query;
        }
        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }
        public TResult Execute<TResult>(Expression expression)
        {
            MethodCallExpression methodCall = expression as MethodCallExpression;
            Expression<Func<T, bool>> result = null;
            while (methodCall != null)
            {
                Expression method = methodCall.Arguments[0];
                Expression lambda = methodCall.Arguments[1];
                LambdaExpression right = (lambda as UnaryExpression).Operand as LambdaExpression;
                if (result == null)
                {
                    result = Expression.Lambda<Func<T, bool>>(right.Body, right.Parameters);
                }
                else
                {
                    Expression left = (result as LambdaExpression).Body;
                    Expression temp = Expression.And(right.Body, left);
                    result = Expression.Lambda<Func<T, bool>>(temp, result.Parameters);
                }
                methodCall = method as MethodCallExpression;
            }

            //Expression<Func<T, bool>> result = null;

            var source = new DBSql().FindAs<T>(result);
            dynamic _temp = source;
            TResult t = (TResult)_temp;
            return t;
        }
        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
