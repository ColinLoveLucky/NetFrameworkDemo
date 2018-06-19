using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.LamdaExpression
{
    public class LambdaClass
    {
        public enum ExpressionTreeType
        {
            Add,
            Sub,
            Multiply,
            Div
        }

        public abstract class ExpressionTree
        {
            public Type Type { get; set; }
            public static ParameterExpressionTree Parameter(Type type, string name)
            {
                return new ParameterExpressionTree()
                {
                    Type = type,
                    Name = name
                };
            }
            public static BinaryExpressionTree Multiply(ExpressionTree left, ExpressionTree right)
            {
                //  if(left.Type==right.Type)

                return new SimpleBinaryExoressionTree()
                {
                    ExpressionType = ExpressionTreeType.Multiply,
                    Left = left,
                   // Right = right
                };
            }

            public static ConstExpresionTree Constant(Type type,object value)
            {
                return new ConstExpresionTree()
                {
                    Type = type,
                    Value = value
                };

            }

            public static BinaryExpressionTree Add(ExpressionTree left,ExpressionTree right)
            {
                return new SimpleBinaryExoressionTree()
                {
                    ExpressionType = ExpressionTreeType.Add,
                    Left = left,
                   // Right = right
                };
            }

            //public static ExpressionTree<TDelegate> LambdaExpression(ExpressionTree body,params ParameterExpressionTree [] parameters)
            //{
            //    return mew ExpressionTree<TDelegate>;
            //}
        }

        public class ParameterExpressionTree
        {
            public string Name { get; set; }

            public Type Type { get; set; }
        }

        public class BinaryExpressionTree : ExpressionTree
        {
            public Type Type { get; set; }

            public ExpressionTree Left
            {
                get;
                set;
            }

            public ExpressionTree Right
            {
                get;
                set;
            }
        }

        internal class SimpleBinaryExoressionTree : BinaryExpressionTree
        {
            public ExpressionTreeType ExpressionType { get; set; }

            public ExpressionTree Left { get; set; }

            public ExpressionType Right { get; set; }
        }

        public class ConstExpresionTree:ExpressionTree
        {
            public Type Type { get; set; }

            public object Value { get; set; }
        }
        public void Show()
        {
            List<string> list = new List<string>(){
                "HelloWorld",
                "HelloKitte"
            };
            list.FindAllData(x => x.IndexOf("H") > 0);
        }
        public void ShowLambda()
        {
            ParameterExpression a = Expression.Parameter(typeof(int), "i");   //创建一个表达式树中的参数，作为一个节点，这里是最下层的节点
            ParameterExpression b = Expression.Parameter(typeof(int), "j");
            BinaryExpression be = Expression.Multiply(a, b);    //这里i*j,生成表达式树中的一个节点，比上面节点高一级
            ParameterExpression c = Expression.Parameter(typeof(int), "w");
            ParameterExpression d = Expression.Parameter(typeof(int), "x");
            BinaryExpression be1 = Expression.Multiply(c, d);
            ConstantExpression e = Expression.Constant(10, typeof(Int32));
            BinaryExpression su = Expression.Add(be, be1);   //运算两个中级节点，产生终结点
            BinaryExpression fu = Expression.Add(su, e);
            Expression<Func<int, int, int, int, int>> lambda = Expression.Lambda<Func<int, int, int, int, int>>(fu, a, b, c, d);
            Console.WriteLine(lambda + "");   //打印‘(i,j,w,x)=>((i*j)+(w*x))’，z对应参数b，p对应参数a
            Func<int, int, int, int, int> f = lambda.Compile();  //将表达式树描述
            //的lambda表达式，编译为可执行代码，并生成该lambda表达式的委托；
            Console.WriteLine(f(1, 1, 1, 1) + "");  //打印2
            Console.ReadKey();
        }
        public void ShowExpesssin()
        {
            ExpressionContext context = new ExpressionContext();
            VariableExpressionModule a = new VariableExpressionModule();
            VariableExpressionModule b = new VariableExpressionModule();
            context.AddValue(a, 10);
            context.AddValue(b, 10);
            ExpressionModule addExpression = new AddExpressionModule(a, b);
            ExpressionModule constExp = new ConstantExpressionModule(10);
            ExpressionModule subExp = new SubExpressionModule(addExpression, constExp);
            var value = subExp.interpret(context);
        }
        public long Fibonacci(int n)
        {
            if (n <= 0 || n > 100)
                return 0;
            if (n == 0 || n == 1)
                return n;
            else
                return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
        public long Total(int n)
        {
            if (n <= 0)
                return 0;
            if (n == 1)
                return 1;
            else
                return n + Total(n - 1);
        }
        public long MulTotal(int n)
        {
            if (n <= 0)
                return 0;
            if (n == 1)
                return 1;
            else
                return n * MulTotal(n - 1);
        }
        public int Max(int[] x, int n)
        {
            if (x.Count() == 0)
                return 0;
            else
            {
                if (n <= 0)
                    return 0;
                if (n == 1)
                    return x[0];
                else
                {
                    if (x[n - 1] > Max(x, n - 1))
                        return x[n - 1];
                    else
                        return Max(x, n - 1);
                }
            }
        }
        public long TourData(int n)
        {
            if (n <= 0)
                return 0;
            else
            {
                if (n == 1)
                    return 1;
                else
                    return TourData(n - 1) + n * n;
            }
        }
    }
    public static class Ext
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return null;
        }

        public static List<T> FindAllData<T>(this List<T> value, Predicate<T> match)
        {
            if (match == null)
            {

            }
            List<T> list = new List<T>();
            for (int i = 0; i < value.Count; i++)
            {
                if (match(value[i]))
                {
                    list.Add(value[i]);
                }
            }
            return list;
        }
    }
    public interface ExpressionModule
    {
        int interpret(ExpressionContext context);
    }
    public class ConstantExpressionModule : ExpressionModule
    {
        private int _i;
        public ConstantExpressionModule(int i)
        {
            this._i = i;
        }
        public int interpret(ExpressionContext context)
        {
            return _i;
        }
    }
    public class AddExpressionModule : ExpressionModule
    {
        private ExpressionModule _left, _right;
        public AddExpressionModule(ExpressionModule left, ExpressionModule right)
        {
            _left = left;
            _right = right;
        }
        public int interpret(ExpressionContext context)
        {
            return _left.interpret(context) + _right.interpret(context);
        }
    }
    public class SubExpressionModule : ExpressionModule
    {
        private ExpressionModule _left, _right;

        public SubExpressionModule(ExpressionModule left, ExpressionModule right)
        {
            this._left = left;
            this._right = right;
        }
        public int interpret(ExpressionContext context)
        {
            return _left.interpret(context) + _right.interpret(context);
        }
    }
    public class MulExpressionModule : ExpressionModule
    {
        private ExpressionModule _left, _right;

        public MulExpressionModule(ExpressionModule left, ExpressionModule right)
        {
            _left = left;
            _right = right;
        }


        public int interpret(ExpressionContext context)
        {
            return _left.interpret(context) * _right.interpret(context);
        }
    }
    public class DivExpressionModule : ExpressionModule
    {
        private ExpressionModule _left, _right;

        public DivExpressionModule(ExpressionModule left, ExpressionModule right)
        {
            _left = left;
            _right = right;
        }


        public int interpret(ExpressionContext context)
        {
            return _left.interpret(context) / _right.interpret(context);
        }
    }
    public class VariableExpressionModule : ExpressionModule
    {
        public int interpret(ExpressionContext context)
        {
            return context.LookUpValue(this);
        }
    }
    public class ExpressionContext
    {
        private Hashtable _hash = new Hashtable();

        public void AddValue(VariableExpressionModule x, int y)
        {
            _hash.Add(x, y);
        }

        public int LookUpValue(VariableExpressionModule x)
        {
            return (int)_hash[x];
        }
    }
}
