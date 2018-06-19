using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Fomratter
{
    public class MyFormatter : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else if (formatType == typeof(DateTimeFormatInfo))
                return this;
            else if (formatType == typeof(NumberFormatInfo))
                return this;
            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                if (arg is IFormattable)
                    return ((IFormattable)arg).ToString(format, formatProvider);
                return arg.ToString();
            }
            else
            {
                if (format == "MyFormatter")
                    return "**" + arg.ToString();
                else
                {
                    if (arg is IFormattable)
                        return ((IFormattable)arg).ToString(format, formatProvider);
                    return arg.ToString();
                }
            }
        }
    }

    public class MyVector : IFormattable
    {
        public MyVector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                return base.ToString();
            }
            else if (format == "MyVector")
            {
                return "(X:" + X + ",Y:" + Y + ",Z:" + Z + ")";
            }
            else
                return base.ToString();
        }

        public override string ToString()
        {
            return "X:" + X;
        }
    }

    public class FormatterDemo
    {
        public void Show()
        {
            int i = 100;
            string printString = string.Empty;
            MyFormatter myFormatter = new MyFormatter();
            printString = string.Format(myFormatter, "{0}", i);
            Console.WriteLine("{0}", printString);
            printString = string.Format(myFormatter, "{0:C}", i);
            Console.WriteLine("{0}", printString);
            printString = string.Format(myFormatter, "{0:MyFormatter} {1:C}", i, 200);
            Console.WriteLine("{0}", printString);
            MyVector myVectory = new MyVector(10, 20, 50);
            Console.WriteLine(string.Format("{0:MyVector}", myVectory));
            Console.WriteLine(myVectory.ToString("MyVector", null));
            var d = DateTime.Now.ToString("D", DateTimeFormatInfo.CurrentInfo);
            var d1 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.CurrentInfo);
            var d2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            var d3 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
    }
}
