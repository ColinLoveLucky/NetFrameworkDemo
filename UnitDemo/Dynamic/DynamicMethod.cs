using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection.Emit;
using System.Web.Hosting;
using System.Web;

namespace UnitDemo.Dynamic
{

    public class SomeClass
    {
        private int name;
        private string sname;

        // a=new SimpleWorkerRequest();

        //HttpRuntime a = new HttpRuntime();

        //ApplicationHost

        public SomeClass() { }

        public SomeClass(string name)
        {
            sname = name;

            SimpleWorkerRequest a = null;

            HttpRuntime.ProcessRequest(a);

            // 创建ApplicationHost
            // ApplicationHost.CreateApplicationHost()
        }

        public int Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string SName
        {
            get
            {
                return sname;
            }
            set
            {
                sname = value;
            }
        }
    }
    public class DynamicMethodDemo
    {
        public void TestDymic()
        {
            SomeClass c = new SomeClass();

            FieldInfo field = c.GetType().GetFields()[1];

            Console.WriteLine("begin reflection");

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            for (int i = 0; i < 10000000; i++)
            {
                field.SetValue(c, "test");
            }

            stopWatch.Stop();

            Console.WriteLine(string.Format("The Total Time is {0}", stopWatch.ElapsedMilliseconds));

            //    var dField =new DynamicMethod()
        }
    }
}
