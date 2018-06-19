using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.LamdaExpression
{
    public class QueryableDemo:IEnumerable
    {
        public void TestQuery()
        {
        }

        public void LinqToObject()
        {
            List<string> names = new List<string>(){
               "zhangsan","lisi"
           };

       

            var name = names.Select(x => x == "lisi");

        }

        public void ForeachDemo()
        {
            List<string> names =new List<string>{
                                   "zhangsan","lisi"
                               };
            foreach(var item in names)
            {
                Console.WriteLine(item);
            }
        }
        public IEnumerator GetEnumerator()
        {
            List<string> names = new List<string>{
                                   "zhangsan","lisi"
                               };
            foreach (var item in names)
            {
                yield return item;
            }
        }

        //public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        //{
        //    if (source == null)
        //    {
        //       // throw Error.ArgumentNull("source");
        //    }
        //    if (selector == null)
        //    {
        //       // throw Error.ArgumentNull("selector");
        //    }
        //    return source.Provider.CreateQuery<TResult>(Expression.Call(null,
        //        ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource), typeof(TResult) }), 
        //        new Expression[] { source.Expression, Expression.Quote(selector) }));
        //}  

        public void DbQueryDemo()
        {
            using (DbQuery<Order> dbquery = new DbQuery<Order>())
            {
                var OrderList = from order in dbquery where order.OrderName == "111" select order;
                OrderList.Provider.Execute<List<Order>>(OrderList.Expression);//立即执行  
                foreach (var o in OrderList)
                {
                    //延迟执行  
                }
            } 
        }
    }

    public class OrderCollection : IEnumerable<Order>
    {
        List<Order> orderList;
        public OrderCollection()
        {
            orderList = new List<Order>() {   
               new Order(){ OrderCode=Guid.NewGuid(),OrderName="订单1", OrderTime=DateTime.Now},   
               new Order(){ OrderCode=Guid.NewGuid(),OrderName="订单2", OrderTime=DateTime.Now},   
               new Order(){ OrderCode=Guid.NewGuid(),OrderName="订单3", OrderTime=DateTime.Now}   
           };
        }

        public IEnumerator<Order> GetEnumerator()
        {
            foreach (var order in orderList)
            {
                yield return order;
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var order in orderList)
            {
                yield return order;
            }
        }
    }

    public class DbQuery<T> : IQueryable<T>, IDisposable
    {
        public DbQuery()
        {
            Provider = new DbQueryProvider();
            Expression = Expression.Constant(this);//最后一个表达式将是第一IQueryable对象的引用。  
        }
        public DbQuery(Expression expression)
        {
            Provider = new DbQueryProvider();
            Expression = expression;
        }

        public Type ElementType
        {
            get { return typeof(T); }
            private set { ElementType = value; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get;
            private set;
        }

        public IQueryProvider Provider
        {
            get;
            private set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (Provider.Execute<T>(Expression) as IEnumerable<T>).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Provider.Execute(Expression) as IEnumerable).GetEnumerator();
        }

        public void Dispose()
        {

        }
    }

    public class DbQueryProvider : IQueryProvider
    {
        public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
        {
            return new DbQuery<TElement>();
        }

        public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
        {
            //这里牵扯到对表达式树的分析，就不多说了。  
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
        {
            return default(TResult);//强类型数据集  
        }

        public object Execute(System.Linq.Expressions.Expression expression)
        {
            return new List<object>();//弱类型数据集  
        }
    }
}
