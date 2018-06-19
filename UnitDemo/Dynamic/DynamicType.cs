using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Dynamic
{
    //使用ExpandoObject
    //使用DynamicObject
    //实现IDynamicMetaObjectProvider接口.
    public class DynamicType
    {
        public void Show()
        {
            dynamic p = new { Name = "LiSi" };

            // var d = p.Name;
            // var result = d + "Hello World";
            //  Console.WriteLine(result);

            //CallSite<Func<CallSite, object, object>> p__Site1 = CallSite<Func<CallSite, object, object>>.Create(
            //      Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(DynamicType), new CSharpArgumentInfo[]
            //    {
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //    }));
            //object d = p__Site1.Target(p__Site1, p);
            //CallSite<Func<CallSite, object, string, object>> p__Site2 =
            //    CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None,
            //    ExpressionType.Add, typeof(DynamicType), new CSharpArgumentInfo[]
            //    {
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
            //    }));
            //object result = p__Site2.Target(p__Site2, d, "Hello World");
            //Console.WriteLine(result);

            p.Name = "wangwu";
        }

        public void ShowDynamicObject()
        {
            dynamic dynProduct = new DynamicObjectDemo();
            dynProduct.name = "n1"; //调用TrySetMember方法
            dynProduct.Id = 1;
            dynProduct.Id = dynProduct.Id + 3;
            dynProduct.ShowProduct = new Action(() => Console.WriteLine("Hello World"));
            dynProduct.ShowProduct();
            Console.ReadLine();
        }

        public void ShowExpandoObject()
        {
            dynamic dynEO = new ExpandoObject();
            dynEO.number = 10;
            dynEO.Increment = new Action(() => { dynEO.number++; });

            Console.WriteLine(dynEO.number);
            dynEO.Increment();
            Console.WriteLine(dynEO.number);

            ((INotifyPropertyChanged)dynEO).PropertyChanged += new PropertyChangedEventHandler(Program_PropertyChanged);
            dynEO.Name = "changed";
            dynEO.Name = "another";

        }

        public void Program_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("属性{0} 已更改", e.PropertyName);
        }

    }

    public class DynamicObjectDemo : DynamicObject
    {
        ConcurrentDictionary<string, object> properties = new ConcurrentDictionary<string, object>();

        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            return properties.TryGetValue(binder.Name, out result);
        }
        public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
        {
            properties[binder.Name] = value;
            return true;
        }

        public ConcurrentDictionary<string, object> GetProperties()
        {
            return properties;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--- Person attributes ---");

            foreach (var key in properties.Keys)
            {
                //We use the chaining property of the StringBuilder methods  
                sb.Append(key).Append(": ").AppendLine(properties[key].ToString());
            }

            return sb.ToString();
        }
    }
}
