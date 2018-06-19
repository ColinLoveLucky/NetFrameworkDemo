using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.inheritT
{
    public class First
    {
        public virtual void Show()
        {
            Console.WriteLine("first show");
        }
    }

    public class Second : First
    {
        public new virtual void Show()
        {
            Console.WriteLine("Second Show");
        }

        public virtual void ShowB()
        {
            Console.WriteLine("Second ShowB");
        }
    }

    public class Third : Second
    {
        public override void Show()
        {
            Console.WriteLine("Third Show");
        }

        public override void ShowB()
        {
            Console.WriteLine("Third ShowB");
        }
    }

    public class Base
    {
        public virtual string Name { get; set; }

        public Base()
        {
            Console.WriteLine("Base Constructor");
        }
        public virtual void Show()
        {
            Console.WriteLine("Base Method");
        }
    }
    public class Inherit : Base
    {
        public Inherit()
        {
            Console.WriteLine("Inherit Constructor");
        }

        public override void Show()
        {
            Console.WriteLine("Child Method");
        }
    }

    public struct MyStructA
    {
        private int age;

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }

        public MyStructA(int a)
        {
            age = a;
        }

        public void Show()
        {

        }
    }

    public struct MyStructB
    {
        // private int b;

        public void Show()
        {
            Console.WriteLine("test");
            //Console.WriteLine(b);
        }
    }

    public class MyClass
    {
        // private int b;

        public void Show()
        {
            Console.WriteLine("test");
            //Console.WriteLine(b);
        }
    }

    public abstract class MyAbstractClass
    {
        public MyAbstractClass()
        {
            Console.WriteLine("Myabstract Class");
        }

        public string _name;
        public string Name
        {
            get;
            set;
        }
        public abstract void Show();

        public virtual void ShowB()
        {
        }

        public void ShowA()
        {
        }
    }

    public class ImplictAbstractClass : MyAbstractClass
    {
        public override void Show()
        {
            Console.WriteLine("Show Implict");
        }

        public override void ShowB()
        {
            Console.WriteLine("Show Implict B");
        }
    }

    public interface MyInterface
    {
        void Show();
    }

    public class T
    {
        public virtual void Show()
        {

        }
    }
    public class MySealdClass : T
    {
        public sealed override void Show()
        {

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public  class RefT
    {
        public static string Name { get; set; }
        //public static void Show()
        //{

        //}
       // public string Name { get; set; }
    }

}
