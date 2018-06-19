using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitDemo.TupleDemo;

namespace UnitDemo
{
    public class TupleUnitTest
    {
        public void UnitTuple()
        {
            var tupleDemo = new TupleUnitDemo();

            tupleDemo.ShowLocationByPoint(new Point()
            {

                X = 89.9,
                Y = 98.9,
                Z = 198.9
            });

            var locationTuple = new Tuple<double, double, double>(98.3, 87, 879);
            tupleDemo.ShowLocationByTuple(locationTuple);
            tupleDemo.ShowLocationByTuple(Tuple.Create<double, double, double>(23, 32, 32));

            var location = new TupleUnitDemo();

            location.Location = Tuple.Create<double, double, double>(12, 43, 22);

            location.ForEach(x =>
            {
                Console.WriteLine("The location is({0},{1},{2})", x.Location.Item1, x.Location.Item2, x.Location.Item3);
            });

            location.ShowLocationByTupleAnyType<double, int, string>(Tuple.Create<double, int, string>(12, 2, "HelloWorld"));


            location.ShowLocationByTuple(location.Location);

            var tuple = location.GetLocation();

            Console.WriteLine("The location is({0},{1},{2})", tuple.Item1, tuple.Item2, tuple.Item3);

        }
    }
}
