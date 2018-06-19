using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.DataTableExtensions
{
    public abstract class StaticExtension
    {
        public static void Show()
        {
            Console.WriteLine("Hello World");
        }
    }

    public class SonExt:StaticExtension
    {

    }

    public sealed class SealedClass
    {
        public static void Show()
        {
            Console.WriteLine("Hello World");
        }
    }

    public partial class PartialClass
    {
        public static void Show()
        {
            Console.WriteLine("Hello World");
        }
    }

    public class QianTaoClass
    {
        public class QianTao
        {
            public void Show()
            {
                Console.WriteLine("Hello World");
            }
        }
    }

    public static class StaticClass
    {
        public static void Show()
        {
            Console.WriteLine("Hello World");
        }
    }

    public class GenericT<T>
    {
        public void Show()
        {
            Console.WriteLine("Hello World");
        }
    }

    public static class ExtClass
    {
        public static void ShowHello(this StaticExtension value,string name)
        {
            Console.WriteLine(name);
        }

        public static void ShowSealedHello(this SealedClass value,string name)
        {
            Console.WriteLine(name);
        }

        public static void ShowPatialHello(this PartialClass value,string name)
        {
            Console.WriteLine(name);
        }

        public static void ShowQianTaoHello(this UnitDemo.DataTableExtensions.QianTaoClass.QianTao value,string name)
        {
            Console.WriteLine(name);
        }
     
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="name"></param>
        //public static void ShowStaticHello(this StaticClass value,string name)
        //{
        //    Console.WriteLine(name);
        //}

        public static void ShowGeneric<T>(this GenericT<T> value,string name)
        {
            Console.WriteLine(name);
        }
    }
}
