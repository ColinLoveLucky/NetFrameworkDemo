using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestAPIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // string a = "hello";
            // string b = "hello";
            // int c = 10; int d = 10;
            //Console.WriteLine(object.ReferenceEquals(c, d));
            //Console.WriteLine(object.Equals(c, d));
            //  Console.WriteLine(c == d);
            // var aa = new A();
            // var bb = new A();
            // var cc = aa;
            //  Console.WriteLine(aa == cc);


            string result = string.Empty;
            List<string> list = new List<string>();
            //string strSql = @"select a.rule_attribute from app_rule_attribute a 
            //                inner join app_main b
            //                on b.app_code=a.app_code
            //                and b.id=:appid";
            //  var data = this.SqlQuery<string>(strSql, appid);
            var data = "{\"rows\": [{\"group\": \"政策\", \"name\":\"PY005\", \"value\": \"绿线城市\"}, {\"group\": \"政策\", \"name\":      \"PY008\", \"value\": \"绿线城市拒绝原因\"}], \"total\": 2}";
            string regexPattern = "\"name\":\\s*\"(.+?)\",";
            //if (data != null)
            //    data.Each(x =>
            //    {
            //        foreach (var item in Regex.Match(x, regexPattern).Groups)
            //        {
            //            list.Add(item.ToString());
            //        }
            //    });
            foreach (Match item in Regex.Matches(data, regexPattern))
            {
                // list.Add(item.ToString());
                // list.Add()
                //list.Add(item.Groups);
                list.Add(item.Groups[1].ToString());
             
            }
            if (list.Count > 0)
                result = string.Join(",", list);

        }


        public class A { }

    }
}
