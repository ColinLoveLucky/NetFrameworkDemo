using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Struct
{
    public struct PointStruct
    {
        //static PointStruct()
        //{
        //    _x = 10;
        //}

        //public static int _x = 10;

        public int _x;

        //public PointStruct(int x)
        //{
        //    _x = x;
        //}

        public void Show()
        {
      
            Console.WriteLine("Hello World");
        }
    }


    public  class PointClass
    {
      
        public int _x = 10;

        //public PointClass(int x)
        //{
        //    _x = x;
        //}
    }
  
    public enum EnumTtt
    {

        XCODE=0
    }
    

    public class TestEqual
    {
        public void ShowEqual(PP pp,PP pp2)
        {
            pp.Name = "LiSi";
        }
        public void ShowString(string s,string s1)
        {
            Debug.Assert(object.ReferenceEquals(s, s1));
        }
    }

    public class PP
    {

        int[] t = new int[10];
        public string Name
        {
            get;
            set;
        }
    }
}
