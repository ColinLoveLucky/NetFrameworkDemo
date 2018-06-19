using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.OperateOverride
{
    public class OperateCharOverrideUnitDemo
    {
        public void TestOperateOverrid()
        {
            var vectorX = new Vector()
            {
                X = 19.8,
                Y = 209.8,
                Z = 98
            };

            var vectorY = new Vector(19, 28, 98);
            Console.WriteLine("VertorX Value is {0}", vectorX.ToString());
            Console.WriteLine("VertorX Value is {0}", vectorY.ToString());

            var vectorXY = vectorX + vectorY;

            Console.WriteLine("X Vercor + Y Vector is (X:{0},Y:{1},Z:{2})", vectorXY.X, vectorXY.Y, vectorXY.Z);
        }
    }
}
