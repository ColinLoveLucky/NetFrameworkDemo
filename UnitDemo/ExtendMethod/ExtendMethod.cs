using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UnitDemo.ExtendMethod
{
    public interface IExtendMethod
    {
        string GetValue();

        // void Show(IExtendMethod method, string value);

    }

    public class ExtendClass : IExtendMethod
    {
        public string GetValue()
        {
            return "Hello World";
        }

        public string GetValue(string value, string value1)
        {
            return GetValue();
        }

        public static void Show(string value)
        {
            Console.WriteLine("Hi");
        }
    }

    public interface IGenericExtend<T>
    {

    }

    public class ImplementExtend<T> : IGenericExtend<T>
    {

    }

    public interface IGenericEx
    {

    }

    public class GenericEx : IGenericEx
    {

    }

    /// <summary>
    /// 扩展方法 必须是静态的?
    /// 扩展方法 扩展泛型静态类?
    /// 扩展方法 的执行原理?
    /// </summary>
    public static class ExtendMethod
    {
        static ExtendMethod()
        {
        }

        public static void Show(this string value)
        {
            Console.WriteLine(value);
        }

        public static void Show(this IExtendMethod extendMethod, string value)
        {
            Console.WriteLine(value);
        }

        public static void Show(this IGenericEx extend, string value)
        {
            Console.WriteLine(value);
        }
        public static T ShowT<T>(this IGenericExtend<T> extend, string value)
        {
            Console.WriteLine(value);
            return default(T);
        }

        public static void ShowTT<T>(this IGenericExtend<T> extend, string value)
        {
            Console.WriteLine(value + "TTT");
        }
    }

    public class IEnumableTTT
    {
        IList<string> list = new List<string>();
        public void Show()
        {
            list.Where(x => x == "Hello");
        }
    }
}
