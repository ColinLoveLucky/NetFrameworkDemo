using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo
{
    public class WeakRef
    {
        public class TestWeak
        {
            public string Name { get; set; }
        }


        public void ShowWeak()
        {
            var obj = new UnitDemo.WeakRef.TestWeak();

            WeakReference weak = new WeakReference(obj);

            if (weak.IsAlive)
            {
                var testweak = weak.Target as UnitDemo.WeakRef.TestWeak;

                Console.WriteLine(testweak.Name);
            }
            else
            {

            }
        }
    }


}
