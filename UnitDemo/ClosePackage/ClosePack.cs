using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ClosePackage
{
    public class ClosePack
    {
        //public List<Action<int>> _list = new List<Action<int>>();
        public List<Action> _list = new List<Action>();
        public void Show()
        {
            for (int i = 0; i < 5; i++)
            {
                int j = i;
                _list.Add(delegate
                {
                    int counter = j * 10;
                    Console.WriteLine(counter);
                    counter++;
                });
                // _list.Add(ShowA);
            }
            //for (int j = 0; j < 5; j++)
            //{
            //    _list[j](j);
            //}
        }

        public void ShowA(int i)
        {
            //  Console.WriteLine(i);
        }


    }
}
