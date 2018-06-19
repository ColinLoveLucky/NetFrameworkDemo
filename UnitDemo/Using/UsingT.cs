using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Using
{
    public class UsingT
    {
        public void Show()
        {
            using (Stream stream = new FileStream("/t/t", FileMode.CreateNew))
            {
                byte[] array = new byte[100];

                stream.Read(array, 0, array.Count());
            }
        }
    }
}
