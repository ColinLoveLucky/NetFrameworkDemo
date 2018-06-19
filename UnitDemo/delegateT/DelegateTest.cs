using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.delegateT
{
    public class DelegateTest
    {
        public Action<string> _action = ((x => Console.WriteLine(x)));
        public Predicate<string> _predicate = (x => x == "Hello World");
        public Func<string, string> _fun = (x => x);
        public delegate int Add(int x, int y);
        private Add _add;
        private Add _addB;
        private Add _addCombine;
        private Add _AddRemove;
        public void Show()
        {
            _add += Calculate;
            _addB += CalculateB;
            _addCombine = (Add)Delegate.Combine(_add, _addB);
            _AddRemove = (Add)Delegate.Remove(_addCombine, _addB);
            foreach (Delegate item in _addCombine.GetInvocationList())
            {
                Console.WriteLine(item.DynamicInvoke(10, 10));
            }
        }
        public int Calculate(int a, int b)
        {
            return a + b;
        }
        public int CalculateB(int c, int d)
        {
            return c + d + 20;
        }
        public void CalculateAdd(int a, int b)
        {

        }
        public void AsyncCallBackFun(IAsyncResult result)
        {

        }
    }
}
