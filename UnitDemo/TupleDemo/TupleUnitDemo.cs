using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.TupleDemo
{
    public class Point
    {
        public double X
        {
            get;
            set;
        }

        public double Y
        {
            get;
            set;
        }

        public double Z
        {
            get;
            set;
        }

    }

    public class TupleUnitDemo
    {
        public Tuple<double, double, double> Location
        {
            get;
            set;
        }
        public void ShowLocationByPoint(Point p)
        {
            Console.WriteLine("Current Location is ({0},{1},{2})", p.X, p.Y, p.Z);
        }

        public void ShowLocationByTuple(Tuple<double, double, double> location)
        {
            Console.WriteLine("Current Location is ({0},{1},{2})", location.Item1, location.Item2, location.Item3);
        }

        public void  ShowLocationByTupleAnyType<T,Y,S>(Tuple<T,Y,S> tuple)
        {
            Console.WriteLine("T Type is:{0},Y Type is {1},S Type is {2}",typeof(T),typeof(Y),typeof(S));
        }

        public void ShowLocation<T>(Action<T> action)
        {
            T obj = default(T);

            action(obj);

            //action(parmas);
        }

        public Tuple<double,string,int> GetLocation()
        {
            Tuple<double, string, int> result = Tuple.Create<double, string, int>(12.22, "Hello World", 12);

            return result;
        }
    }

    public static class TupleUnitExtension
    {
        public static void ForEach(this TupleUnitDemo tupleUnit, Action<TupleUnitDemo> action)
        {
            action(tupleUnit);
        }

    }
}
