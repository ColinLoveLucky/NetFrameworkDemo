using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.OperateOverride
{
    public class OperateCharOverride
    {

    }

    public class Vector
    {

        public Vector()
        {

        }
        public Vector(double x, double y, double z)
        {
            X = x;

            Y = y;

            Z = z;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public override string ToString()
        {
            return string.Format("X:{0},Y:{1},Z:{2}", X, Y, Z);
        }
        public static Vector operator +(Vector a, Vector b)
        {
            Vector result = null;

            if (a != null && b != null)
                result = new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            return result;

        }
    }
}
