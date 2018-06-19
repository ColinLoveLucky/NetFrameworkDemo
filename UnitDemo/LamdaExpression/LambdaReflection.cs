using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.LamdaExpression
{
    public static class LambdaReflection
    {
        /// <summary>
        /// 得到属性的名称
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">类型</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static object GetRefPropertyValue<T>(this T value, string name)
        {
            object result;

            var expProperty = Expression.Property(Expression.Constant(value), name);

            var lambda = Expression.Lambda<Func<object>>(expProperty);

            result = lambda.Compile()();

            return result;
        }
        /// <summary>
        ///设置属性的数值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">类型数值</param>
        /// <param name="name">属性名称</param>
        /// <param name="data">属性值</param>
        public static void SetRefPropertyValue<T>(this T value, string name, object data)
        {
            PropertyInfo p = value.GetType().GetProperty(name);
            var param_obj = Expression.Parameter(value.GetType());
            var paramExp = Expression.Parameter(typeof(object), p.Name);
            var conExp = Expression.Convert(paramExp, p.PropertyType);
            var callExp = Expression.Call(param_obj, p.GetSetMethod(), conExp);
            var lambda = Expression.Lambda<Action<T, object>>(callExp, param_obj, paramExp);
        }
        public static void SetDyamicPropertyValue<T>(this T value,string name,object data)
        {
            PropertyInfo p = value.GetType().GetProperty(name);
            var param_obj = Expression.Parameter(value.GetType());
            var paramExp = Expression.Parameter(typeof(object), p.Name);
            var conExp = Expression.Convert(paramExp, p.PropertyType);
          
            var binder = Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, p.Name, typeof(Program),
                new CSharpArgumentInfo[]
                {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
                });
            var dynaimucExp = Expression.MakeDynamic(typeof(Func<CallSite, object, string, object>),
                  binder, param_obj, conExp
                  );
            var lambdaDynamic = Expression.Lambda(dynaimucExp, param_obj, paramExp);

            var obj = lambdaDynamic.Compile().DynamicInvoke(value, data);
        }
    }

    public class LambdaRefPerson
    {
        public string Name { set; get; }
    }
}
