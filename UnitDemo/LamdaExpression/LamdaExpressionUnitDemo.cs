using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.LamdaExpression
{
    public class LamdaExpressionUnitDemo
    {
        public void LamdaUnitTest()
        {
            var lamda = new LamdaExpressionDynamic();

            lamda.CalculateMethod();

            lamda.ShowHi();

            lamda.ShowStaticMethod();

            lamda.CallInstanceMethod();

            lamda.ShowDecrementMethod();

            lamda.ShowIncrementMethod();

            lamda.ShowIsTrue();

            lamda.ShowAssign();

            lamda.ShowConvert();

            lamda.ShowNew();

            lamda.ShowReturn();

            lamda.ShowNewByConstructor();

            lamda.ShowArrayIndex();

            lamda.ShowBind();

            lamda.ShowField();

            lamda.ShowProperty();

            lamda.ShowPropertyOrField();

            lamda.ShowCondition();

            lamda.ShowSwtich();

            lamda.ShowLoop();

            lamda.ShowQuote();

            lamda.ShowListInit();

            lamda.ShowTryCatch();

            lamda.ShowIQuerable();

            lamda.ShowConvertSql();

            lamda.ShowQuery();

            lamda.ShowFromQuery();

            IList<Student> items = new List<Student>()
            {
               new Student("zhangsan",1),
               new Student("lisi",0)
            };

            var item = from a in items
                       where a.Name == "lisi"
                       select a;

          //  Expression < Action < System.Collections.Generic.List < Student >> d = de => de;


          //  IQueryable<Student> query =

          //  lamda.ShowFun(query);

           // lamda.Show<string>(()="Hello World");

       

        }
    }
}
