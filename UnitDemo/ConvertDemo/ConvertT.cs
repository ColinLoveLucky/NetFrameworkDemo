using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ConvertDemo
{
    public class ConvertD
    {
        public void ConvertObjetToA()
        {
            object obj = new A{ Name = "HelloObj" };
            A dd = (A)obj;
            Console.WriteLine("object A Covert Name Value {0}", dd.Name);
            dd.Name = "ChangeHelloObj";
            Console.WriteLine("object A Covert Name Value AName:{0}, --  objectName:{1}", dd.Name,((A)obj).Name);            
            //var varObj = new { Name = "HelloVar" };
            //A varA = (A)varObj;
        }

        public void ConvertDynamicToA()
        {
            dynamic dyObj = new { Name ="Hello"};
            A dyA = (A)dyObj;
            Console.WriteLine("Dynamic A Covert Name Value {0}", dyA.Name);
        }

        public void ConvertDynamicToA2()
        {
            dynamic dyObj = new A{ Name = "HelloDynamicA" };
            SendArg(dyObj);
            dynamic dyObjB = new B { Name = "HelloDynaimcB" };
            SendArg(dyObjB);
            dynamic dyObC = new { Name = "HelloObject" };
            SendArg(dyObC);
            object objectD = new  { Name = "Hello" };
            SendArg(objectD);
            //A dyA = (A)dyObj;
            //Console.WriteLine("Dynamic A Covert Name Value {0}", dyA.Name);
        }
        public void ConvertDynamicToA3()
        {     
        }
        public void SendArg(A a)
        {
            Console.WriteLine("SendA is A");
        }
        public void SendArg(B a)
        {
            Console.WriteLine("SendA is B");
        }
        public void SendArg(object a)
        {
            Console.WriteLine("SendA is Object");
        }
    }
    public class A
    {
        public string Name { get; set; }
        public static implicit operator A(string b)
        {

            A a = new A();

            a.Name = b;

            return a;
        }
    }
    public class B
    {
        public static explicit operator A(B b)
        {
            A a = new A();

            a.Name = b.Name;

            return a;
        }
        public string Name { get; set; }
    }
}
