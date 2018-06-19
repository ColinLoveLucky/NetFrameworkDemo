using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Finalizer
{
    public class Fina:IDisposable
    {
        public string Name { get; set; }
        public Fina()
        {

        }

        ~Fina()
        {
            Dispose(false);

            Console.WriteLine("-------我是析构函数释放非托管资源----------");
        }

        public void Dispose()
        {

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if(disposing)
            {
                Console.WriteLine("释放托管资源");

                //TODO: 清理托管资源代码
            }

            

            Console.WriteLine("释放非托管资源");

            //TODO 清理非托管资源
        }



    }





}
