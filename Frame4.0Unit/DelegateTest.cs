using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Frame4._0Unit
{
    public class DelegateTest
    {
        //MulticastDelegate

        public delegate void Calculate();
        
        //EventHandler

        public Calculate _calcuDelegate;

        public void AddA()
        {
            Console.WriteLine("AddA");
        }

        public void AddB()
        {
            Console.WriteLine("AddB");
        }

        public event Calculate _calcuEvent;


    }
}
