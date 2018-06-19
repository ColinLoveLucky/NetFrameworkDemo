using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.LamdaExpression
{
    public class QuarableExpression
    {
        public void ShowT()
        {
           // FindAs<Staff>(x => x.Code == "ZhangSan" && x.Name.Contains("Zhang"));

            FindAs<Staff>(x => x.Code == "ZhangSan" );
        }

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
            var inner = express as BinaryExpression;

            string name = (inner.Left as MemberExpression).Member.Name;

            object value = (inner.Right as ConstantExpression).Value;

            var oper = GetOperator(inner.NodeType);

            string result = string.Format("{0} {1} '{2}'", name, oper, value);

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
            var methodCall = expression as MethodCallExpression;

            var methodName = methodCall.Method.Name;

            var result = string.Empty;

            if (methodName == "Contains")
            {
                object temp_Value = (methodCall.Arguments[0] as ConstantExpression).Value;

                string value = string.Format("%{0}%", temp_Value);

                string name = (methodCall.Object as MemberExpression).Member.Name;

                result = string.Format("{0} like '{1}'", name, value);
            }

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
    }

    public class Staff
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Code { get; set; }

        public DateTime? Birthday { get; set; }

        public bool Deletion { get; set; }
    }

    public class ResolveExpress
    {
        public Dictionary<string, object> argument;

        public string sqlWhere;

        public SqlParameter[] paras;

        public void ResolveExpression(Expression expression)
        {
            this.argument = new Dictionary<string, object>();

            this.sqlWhere = Resolve(expression);

            this.paras = argument.Select(x => new SqlParameter(x.Key, x.Value)).ToArray();
        }

        private string Resolve(Expression expression)
        {

            if (expression is LambdaExpression)
            {
                LambdaExpression lambda = expression as LambdaExpression;

                expression = lambda.Body;

                return Resolve(expression);
            }

            if (expression is BinaryExpression)
            {
                BinaryExpression binary = expression as BinaryExpression;

                if (binary.Left is MemberExpression && binary.Right is ConstantExpression)
                {
                    return ResolveFunc(binary.Left, binary.Right, binary.NodeType);
                }

                if (binary.Left is MethodCallExpression && binary.Right is ConstantExpression)
                {
                    object value = (binary.Right as ConstantExpression).Value;
                    return ResolveLinqToObject(binary.Left, value, binary.NodeType);
                }
                if (binary.Left is MemberExpression && binary.Right is MemberExpression)
                {
                    LambdaExpression lambda = Expression.Lambda(binary.Right);

                    Delegate fn = lambda.Compile();

                    ConstantExpression value = Expression.Constant(fn.DynamicInvoke(null), binary.Right.Type);

                    return ResolveFunc(binary.Left, value, binary.NodeType);

                }
            }

            if (expression is UnaryExpression)
            {
                UnaryExpression unary = expression as UnaryExpression;

                if (unary.Operand is MethodCallExpression)
                    return ResolveLinqToObject(unary.Operand, false);
                if (unary.Operand is MemberExpression && unary.NodeType == ExpressionType.Not)
                {
                    ConstantExpression constant = Expression.Constant(false);

                    return ResolveFunc(unary.Operand, constant, ExpressionType.Equal);
                }
            }

            if (expression is MemberExpression && expression.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression member = expression as MemberExpression;

                ConstantExpression constant = Expression.Constant(true);

                return ResolveFunc(member, constant, ExpressionType.Equal);
            }

            if (expression is MethodCallExpression)
            {
                MethodCallExpression methodCall = expression as MethodCallExpression;

                return ResolveLinqToObject(methodCall, true);
            }

            var body = expression as BinaryExpression;

            if (body == null)

                throw new Exception("无法解析" + expression);

            var operat = GetOperator(body.NodeType);

            var left = Resolve(body.Left);

            var right = Resolve(body.Right);

            string result = string.Format("{0} {1} {2}", left, operat, right);

            return result;
        }

        private string ResolveFunc(Expression left, Expression right, ExpressionType expressinType)
        {
            var name = (left as MemberExpression).Member.Name;

            var value = (right as ConstantExpression).Value;

            var operate = GetOperator(expressinType);

            string compName = SetArgument(name, value.ToString());

            string result = string.Format("{0} {1} {2}", name, operate, compName);

            return result;
        }

        private string ResolveLinqToObject(Expression expression, object value, ExpressionType? expressionType = null)
        {
            var methodCall = expression as MethodCallExpression;

            var methodName = methodCall.Method.Name;

            switch (methodName)
            {
                case "Contains":
                    if (methodCall.Object != null)
                        return Like(methodCall);
                    return In(methodCall, true);
                case "Count":
                    return Len(methodCall, value, expressionType.Value);
                case "LongCount":
                    return Len(methodCall, value, expressionType);
                default:
                    throw new Exception(string.Format("不支持{0}方法查找", expressionType));
            }
        }

        private string GetOperator(ExpressionType? expressionType)
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

        private string SetArgument(string name, string value)
        {
            name = "@" + name;

            string temp = name;

            while (argument.ContainsKey(temp))
            {
                int code = Guid.NewGuid().GetHashCode();

                if (code < 0)
                    code *= -1;
                temp = name + code;
            }

            argument[temp] = value;

            return temp;
        }

        private string Like(MethodCallExpression expression)
        {
            object temp_Vale = (expression.Arguments[0] as ConstantExpression).Value;

            string value = string.Format("%{0}%", temp_Vale);

            string name = (expression.Object as MemberExpression).Member.Name;

            string compName = SetArgument(name, value);

            string result = string.Format("{0} like {1}", name, compName);

            return result;
        }

        private string In(MethodCallExpression expression, object isTrue)
        {
            var argument1 = (expression.Arguments[0] as MemberExpression).Expression as ConstantExpression;

            var argument2 = expression.Arguments[1] as MemberExpression;

            var fieldArray = argument1.Value.GetType().GetFields().First();

            object[] array = fieldArray.GetValue(argument1.Value) as object[];

            List<string> setInPara = new List<string>();

            for (int i = 0; i < array.Length; i++)
            {
                string name_para = "InParameter" + i;
                string value = array[i].ToString();
                string key = SetArgument(name_para, value);
                setInPara.Add(key);
            }

            string name = argument2.Member.Name;

            string operat = Convert.ToBoolean(isTrue) ? "in" : "not in";

            string compName = string.Join(",", setInPara);

            string result = string.Format("{0} {1} {2}", name, operat, compName);

            return result;
        }

        public string Len(MethodCallExpression expression, object value, ExpressionType? expressionType)
        {
            object name = (expression.Arguments[0] as MemberExpression).Member.Name;

            string operat = GetOperator(expressionType);

            string compName = SetArgument(name.ToString(), value.ToString());

            string result = string.Format("len {0} {1} {2}", name, operat, compName);

            return result;
        }
    }
}
