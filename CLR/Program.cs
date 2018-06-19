using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CLR
{
    //程序集的概念
    //System.CodeDom.
    class Program
    {
        readonly int A = 10;
        static Program()
        {

        }

        public  delegate void AddDelegate(int x, int Y);

       // public static event AddDelegate AddEvent;
        static void Main(string[] args)
        {
            //FirstModule test = new FirstModule();
            ////test.Show();
            //var name = Assembly.GetAssembly(typeof(Program)).FullName;
            //Console.WriteLine(Assembly.GetAssembly(typeof(Program)).FullName);
            //Assembly assembly = Assembly.Load(name);
            //if (assembly != null)
            //    Console.WriteLine("Success");
            //new Program().Show(19, 10);

            // RuntimeTypeHandle

            //Type type = typeof(Program);

            //RuntimeTypeHandle typHandle = type.TypeHandle;

            //Console.WriteLine(typHandle);

            //Type type2 = Type.GetTypeFromHandle(typHandle);

            //var members = typeof(Program).GetMembers();

            //foreach (var member in members)
            //    Console.WriteLine("Name {0} Type {1}", member.Name, member.MemberType);

            //var array = new int[2] { 1, 2 };

            //foreach (int item in array)
            //{
            //    Change(array);
            //}


          //  AddDelegate += Show;


            Console.WriteLine("Hello World");

            var assemblyFullName = AppDomain.CurrentDomain.FriendlyName;

            var s = Assembly.Load(new AssemblyName("CLR")).FullName;

            Console.WriteLine(s);


        }

        private static void Change( int [] value)
        {
            value[1] = 20;
        }
        private int initRef;
        public void Show(ref int value, out int valueOut)
        {
            valueOut = 10;
        }
        public void Show(int value, int valueOut)
        {
           
        }
        public void ShowTest()
        {
            //AddEvent += AddShow;

           // AddEvent -= AddShow;
        }
        public void AddShow(int x, int y)
        {
            int z = x + y;
        }
    }
    public interface intA
    {
    }
    public abstract class intB
    {
    }
    public class TestAttribute:Attribute
    {
        public TestAttribute()
        {

        }
    }
    [Test]
    public class A
    {
    }
    public class B : A
    {

    }
}
