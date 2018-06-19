using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.LamdaExpression
{
    public class LamdaExpressionDynamic
    {
        public void ShowHi()
        {
            //  ConstantExpression constExp = Expression.Constant("Hello World,I Love Programming So Much", typeof(string));

            ParameterExpression pExp = Expression.Parameter(typeof(string), "content");

            MethodCallExpression methodCallExp =
                Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), pExp);

            Expression<Action<string>> consoleLambdaExp = Expression.Lambda<Action<string>>(methodCallExp, pExp);

            var action = consoleLambdaExp.Compile();

            action("I Love Programming");
        }

        public void CalculateMethod()
        {
            ParameterExpression a = Expression.Parameter(typeof(Int32), "i");

            ParameterExpression b = Expression.Parameter(typeof(Int32), "j");

            BinaryExpression be = Expression.Multiply(a, b);

            ParameterExpression c = Expression.Parameter(typeof(Int32), "x");
            ParameterExpression d = Expression.Parameter(typeof(Int32), "y");

            BinaryExpression bel = Expression.Multiply(c, d);

            BinaryExpression su = Expression.Add(be, bel);

            Expression<Func<int, int, int, int, int>> lambda =
                Expression.Lambda<Func<int, int, int, int, int>>(su, a, b, c, d);

            Func<int, int, int, int, int> f = lambda.Compile();

            Console.WriteLine("(1*10)+(25*34)={0}", f(1, 10, 25, 34));
        }

        public void ShowStaticMethod()
        {
            MethodCallExpression methodCallExp =
                Expression.Call(typeof(LamdaExpressionDynamic).GetMethod("CallStaticMethod"));

            Expression<Action> lambda = Expression.Lambda<Action>(methodCallExp);

            lambda.Compile()();

        }

        public void ShowInstanceMethod()
        {
            var p = new LamdaExpressionDynamic();

            MethodCallExpression methodCallExp =
                Expression.Call(Expression.Constant(p), typeof(LamdaExpressionDynamic).GetMethod("CallInstanceMethod"));

            Expression<Action> lambda = Expression.Lambda<Action>(methodCallExp);

            lambda.Compile()();
        }

        public static void CallStaticMethod()
        {
            Console.WriteLine("Expression Test Call StaticMethod");
        }

        public void CallInstanceMethod()
        {
            Console.WriteLine("Expression Call InstanceMethod");
        }

        public void ShowDecrementMethod()
        {
            ConstantExpression constExp = Expression.Constant(5);

            UnaryExpression unaryExp = Expression.Decrement(constExp);

            Expression<Func<int>> lambda = Expression.Lambda<Func<int>>(unaryExp);

            Console.WriteLine("-- Value is {0}", lambda.Compile()());

        }

        public void ShowIncrementMethod()
        {
            ConstantExpression constExp = Expression.Constant(5);

            UnaryExpression unaryExp = Expression.Increment(constExp);

            MethodCallExpression callExp =
                Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(Int32) }), unaryExp);

            Expression<Action> lambda = Expression.Lambda<Action>(callExp);

            lambda.Compile()();


        }

        public void ShowIsTrue()
        {
            ParameterExpression leftExp = Expression.Parameter(typeof(string), "leftParmas");
            ParameterExpression rightExp = Expression.Parameter(typeof(string), "rightParmas");

            BinaryExpression beExp = Expression.Equal(leftExp, rightExp);

            UnaryExpression unaryExp = Expression.IsTrue(beExp);

            Expression<Func<string, string, bool>> lambda =
                Expression.Lambda<Func<string, string, bool>>(unaryExp, new ParameterExpression[] { leftExp, rightExp });

            var fun = lambda.Compile();

            var leftParmas = "Zhangsan";

            var rightParmas = "LiSi";

            Console.WriteLine("Zhangsan Is Equal Zhangsan  is {0}", fun(leftParmas, rightParmas));
        }

        public void ShowAssign()
        {
            //Expression<Func<int, int, int>> lambda1 = (a, b) => a * b;
            //var f = lambda1.Compile();
            //Console.WriteLine(f(1, 2));

            //ParameterExpression x = Expression.Variable(typeof(string), "x");
            //ConstantExpression constExp = Expression.Constant("12");
            //BinaryExpression beExp = Expression.Assign(x, constExp);
            //BlockExpression blockExpression = Expression.Block(new ParameterExpression[]{
            //x
            //}, beExp);
            //Expression<Func<string>> lambda =
            //    Expression.Lambda<Func<string>>(blockExpression);
            //Console.WriteLine("Variable is {0}", lambda.Compile()());

            var pa = Expression.Parameter(typeof(int), "i");
            //本地变量
            var loc = Expression.Variable(typeof(string), "str");
            //创建LabelTarget用来返回值
            LabelTarget labelTarget = Expression.Label(typeof(string), "labStr");
            //调用i.ToString()
            MethodCallExpression med = Expression.Call(pa, typeof(object).GetMethod("ToString", new Type[] { }));
            //将结果赋值给本地字符串变量
            BinaryExpression asn = Expression.Assign(loc, med);
            //创建返回表达式（实际上就是Goto表达式）
            GotoExpression ret = Expression.Return(labelTarget, loc);
            //创建返回表达式的目标Label
            LabelExpression lbl = Expression.Label(labelTarget, Expression.Constant(string.Empty));
            // LabelExpression lbl = Expression.Label(labelTarget,loc);
            //生成BlockExpression
            BlockExpression blocks = Expression.Block(
                new ParameterExpression[] { loc },
                asn,
                ret,
                lbl
                );
            //生成Lambda表达式
            Expression<Func<int, string>> lam = Expression.Lambda<Func<int, string>>(blocks,
                new ParameterExpression[] { pa });
            //运行并输出结果
            Func<int, string> del = lam.Compile();
            Console.WriteLine(del(17));

        }

        public void ShowConvert()
        {
            var person = new Person()
            {
                Name = "zhangSan"
            };
            dynamic p = new { Name = "LiSi" };
            object op = (object)p;
            Console.WriteLine("Person 的Name属性的值是{0}", person.GetProperty("Name"));
            person.SetPropertyValue("Name", "angle");
            Console.WriteLine("Person 的Name属性的值修改后是{0}", person.Name);
            Console.WriteLine("dynamic 的Name属性的值是{0}", op.GetProperty("Name"));
            op.SetPropertyValue("Name", "I Wanna Be Free");
            Console.WriteLine("dynamic 的Name属性的值修改后是{0}", op.GetProperty("Name"));
        }

        public void ShowReturn()
        {
            var paramIExp = Expression.Parameter(typeof(int), "i");
            var paramStrExp = Expression.Variable(typeof(string), "str");
            LabelTarget labTarget = Expression.Label(typeof(string), "labStr");
            MethodCallExpression methodExp =
                Expression.Call(paramIExp, typeof(object).GetMethod("ToString", new Type[] { }));
            BinaryExpression binaryExp = Expression.Assign(paramStrExp, methodExp);
            GotoExpression ret = Expression.Return(labTarget, paramStrExp);
            LabelExpression lbl =
                Expression.Label(labTarget, Expression.Constant(string.Empty));
            BlockExpression blocks = Expression.Block(
                new ParameterExpression[] { paramStrExp },
                binaryExp, ret, lbl
                );
            var lambda = Expression.Lambda<Func<int, string>>(blocks, paramIExp);
            var result = lambda.Compile()(12);
            Console.WriteLine(result);
        }

        public void ShowNew()
        {
            NewExpression newExp = Expression.New(typeof(Person));

            var person = Expression.Variable(typeof(Person), "person");

            var newPerson = Expression.Assign(person, newExp);

            var property = typeof(Person).GetProperties().FirstOrDefault();

            //    MemberExpression memberExp = Expression.Property(newPerson, "Name");

            ParameterExpression paramExp = Expression.Parameter(typeof(string), "i");

            MethodCallExpression methodExp = Expression.Call(person, property.GetSetMethod(), paramExp);

            var resExp = Expression.Variable(typeof(string), "res");

            var getNameExp = Expression.Call(person, property.GetMethod);

            var assignExp = Expression.Assign(resExp, getNameExp);

            var labResExp = Expression.Label(typeof(string), "labRes");

            var returnExp = Expression.Return(labResExp, resExp);

            var labelDefineExp = Expression.Label(labResExp, Expression.Constant(string.Empty));

            var block = Expression.Block(new ParameterExpression[] { resExp, person }, newPerson, methodExp, assignExp, returnExp, labelDefineExp);

            var fun = Expression.Lambda<Func<string, string>>(block, paramExp);

            var result = fun.Compile()("Hello World");

            Console.WriteLine(result);
        }

        public void ShowNewByConstructor()
        {
            var constructorByStudent = typeof(Student).GetConstructor(new Type[] { typeof(string), typeof(int) });
            var paramNameExp = Expression.Parameter(typeof(string), "name");
            var paramSexExp = Expression.Parameter(typeof(int), "sex");
            var stuExp = Expression.New(constructorByStudent, paramNameExp, paramSexExp);
            var methodCallExp = Expression.Call(stuExp, typeof(Student).GetMethod("ShowInformation"));
            var lambda = Expression.Lambda<Action<string, int>>(methodCallExp, paramNameExp, paramSexExp);
            lambda.Compile()("ZhangSan", 0);
        }

        public void ShowArrayIndex()
        {
            string[,] gradeArray = { { "Chemistry", "History", "Chinese", "Match" }, { "100", "90", "100", "120" } };

            var constArrayExp = Expression.Constant(gradeArray);

            var methodCallExp = Expression.ArrayIndex(constArrayExp, Expression.Constant(1), Expression.Constant(3));

            var lambdaFun = Expression.Lambda<Func<string>>(methodCallExp);

            var result = lambdaFun.Compile()();

            Console.WriteLine(result);

        }

        public void ShowBind()
        {
            var constructorStudent = typeof(Student).GetConstructor(new Type[] { typeof(string), typeof(int) });

            var nameParamExp = Expression.Parameter(typeof(string), "name");
            var sexParamExp = Expression.Parameter(typeof(int), "sex");
            var sexParamStringExp = Expression.Parameter(typeof(string), "sexString");

            var newExp = Expression.New(constructorStudent, nameParamExp, sexParamExp);

            var nameMemberInfo = typeof(Student).GetMember("Name").FirstOrDefault();

            var sexMemberInfo = typeof(Student).GetMember("Gender").FirstOrDefault();

            var nameMemberExp = Expression.Bind(nameMemberInfo, nameParamExp);

            var sexMemberExp = Expression.Bind(sexMemberInfo, sexParamStringExp);

            var memberInitExp = Expression.MemberInit(newExp, nameMemberExp, sexMemberExp);

            var lambda = Expression.Lambda<Func<string, int, string, Student>>(memberInitExp, nameParamExp, sexParamExp, sexParamStringExp);

            var result = lambda.Compile()("ZhangSan", 1, "1");

        }

        public void ShowField()
        {
            var student = new Student("LiSi", 1);

            var fieldExp = Expression.Field(Expression.Constant(student), "_name");

            var methodExp =
                Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), fieldExp);

            var lambda = Expression.Lambda<Action>(methodExp);

            lambda.Compile()();

        }

        public void ShowProperty()
        {
            var student = new Student("WangWu", 0);


            var propertyExp = Expression.Property(Expression.Constant(student), "Name");

            var methodExp = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), propertyExp);

            var lambda = Expression.Lambda<Action>(methodExp);

            lambda.Compile()();

        }

        public void ShowPropertyOrField()
        {
            var student = new Student("ZhaoLiu", 0);

            var fieldExp = Expression.PropertyOrField(Expression.Constant(student), "_name");

            var methodExp = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), fieldExp);

            var lamba = Expression.Lambda<Action>(methodExp);

            lamba.Compile()();


        }

        public void ShowCondition()
        {
            int num = 100;
            var conditionExp = Expression.Condition(Expression.Constant(num > 0), Expression.Constant("Consgratulation to you"),
                  Expression.Constant("I'm Sorry to tall you"));

            var lambda = Expression.Lambda<Func<string>>(conditionExp);

            var result = lambda.Compile()();
        }

        public void ShowSwtich()
        {
            var switchConstExp = Expression.Constant(1);

            var switchCaseExp = Expression.Switch(
                   switchConstExp,
                   Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                   Expression.Constant("Default Value")),

                   new SwitchCase[]{

                    Expression.SwitchCase(
                    Expression.Call(typeof(Console).GetMethod("WriteLine",new Type[]{typeof(string)}),
                    Expression.Constant("switch one choose")),
                    Expression.Constant(1)
                    ),
                    
                    Expression.SwitchCase(
                    Expression.Call(typeof(Console).GetMethod("WriteLine",new Type[]{typeof(string)}),
                    Expression.Constant("switch one choose")),
                    Expression.Constant(2)
                    )
                }

                   );

            var lambda = Expression.Lambda<Action>(switchCaseExp);

            lambda.Compile()();
        }

        public void ShowLoop()
        {

            ParameterExpression value = Expression.Parameter(typeof(int), "value");

            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            LabelTarget label = Expression.Label(typeof(int), "labBreak");

            BlockExpression block = Expression.Block(
                new[] { result },
               Expression.Assign(result, Expression.Constant(1)),
               Expression.Loop(
                Expression.IfThenElse(
                Expression.GreaterThan(value, Expression.Constant(1)),
                Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),
                Expression.Break(label, result)
                ),
                label
                ),
                Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }), result)
                );

            var lambda = Expression.Lambda<Action<int>>(block, value);

            lambda.Compile()(4);

        }

        public void ShowQuote()
        {
            var assginExp = Expression.Add(Expression.Constant(10), Expression.Constant(20));

            var lambda = Expression.Lambda<Func<int>>(assginExp);

            var lambdaReulst = lambda.Compile();

            var quoteExp = Expression.Quote(lambda);

            var test = (Expression<Func<int>>)quoteExp.Operand;

            var result = test.Compile()();

        }

        public void ShowListInit()
        {
            string tree1 = "maple";
            string tree2 = "oak";

            var addMethod = typeof(Dictionary<int, string>).GetMethod("Add");

            var elemMapleExp = Expression.ElementInit(
                addMethod,
                Expression.Constant(tree1.Length),
                Expression.Constant(tree1)

                );

            var elemOakExp = Expression.ElementInit(
                addMethod,
                Expression.Constant(tree2.Length),
                Expression.Constant(tree2)
                );

            var newDicExp = Expression.New(typeof(Dictionary<int, string>));

            var listInitExp = Expression.ListInit(
                newDicExp, elemMapleExp, elemOakExp
                );

            var elemInit = listInitExp.Initializers[1];

            Console.WriteLine(elemInit.Arguments[1]);




        }

        public void ShowTryCatch()
        {
            var tryCatchExp = Expression.TryCatch(
                Expression.Block(
                Expression.Throw(Expression.Constant(new DivideByZeroException()))
                //Expression.Constant("Try Block")
                ),
                Expression.Catch(
                typeof(DivideByZeroException),
                Expression.Constant("Catch Block")
                )
                );
            var lambda = Expression.Lambda<Func<string>>(tryCatchExp).Compile()();
        }

        /// 获取字段
        public void ShowDynamic(dynamic aid)
        {
            ParameterExpression paramExpr = Expression.Parameter(typeof(object), "o");
            //获取 CallSite 以支持调用时的运行时。
            //CallSiteBinder 是必需的
            //注意这里用的是   Binder.GetMember    
            CallSiteBinder aiBinder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Id", typeof(Program),
                    new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
            //定义一个动态表达式
            //Func<CallSite, object, object>声明方式中 CallSite 是动态调用站点的基类。 此类型用作动态站点目标的参数类型。
            //就是上面的 aiBinder，第二个参数是要传入的动态类型 用dynamic关键字也是可以的，
            //后面的参数是实际调用和返回的参数
            //不过操作中发现在返回值一定要用 object 不然会编译不过
            //当然也可用Action<CallSite, object,...>,除了没返回值，参数要求和用Func<CallSite, object, object>是一样的。
            DynamicExpression Dynamic2 =
              Expression.MakeDynamic(typeof(Func<CallSite, object, object>),
                 aiBinder, paramExpr
            );
            //编译
            LambdaExpression laExp = Expression.Lambda(
                Expression.Block(
                new ParameterExpression[] { paramExpr },
                Expression.Assign(paramExpr, Expression.Constant(aid))
              , Dynamic2));
            //执行
            Console.WriteLine("AiDynamicDo：" + laExp.ToString());
            Console.WriteLine("结果：" + laExp.Compile().DynamicInvoke());
            Console.WriteLine();
        }

        public void ShowIQuerable()
        {
            var queryable = new QuarableExpression();

            queryable.ShowT();
        }

        public void ShowConvertSql()
        {
            var queryable = new QuarableExpression();

            queryable.ShowConvertSql();

            //  IQueryable 

        }

        public void ShowQuery()
        {
            var sqlQueryable = new ISqlQueryable();

            sqlQueryable.ShowQuery();
        }

        public void ShowFromQuery()
        {
            //IList<Student> items = new List<Student>()
            //{
            //    new Student("zhangsan",1)
            //};

            //IEnumerable<Student> item = from a in items
            //                            where a.Name == "zhangsan"
            //                            select a;

            //var result = items.ToList();


            //Func<bool, string> result = a =>
            //  {
            //      string reValue = string.Empty;

            //      if (a)
            //      {
            //          reValue = "Hello";
            //      }

            //      return reValue;
            //  };


        }

        public IList<Student> ShowFun(IQueryable<Student> query)
        {
            IQueryable<Student> qu = from a in query
                                     where a.Name == "zhangsan"
                                     select a;

            return qu.ToList();

        
        }

        public void Show<T>(Expression<Func<T>> expression)
        {
            var lambda = expression.Compile();

            lambda();
        }

        public void ShowExtend()
        {
            List<string> list = new List<string>(){
                "Hello World",
                "Hello Kitte"
            };
            var lists = list.Select(x => x);
            var Iquery = from a in list select a;
            // Enumerable.OfType
            //Queryable 
        }

        /// <summary>
        /// 字段2
        /// </summary>
        /// <param name="aiD"></param>
        public void AiDynamicDo2(dynamic aiD)
        {
            //  CallSite<T>
            //Binder.Convert
            CallSiteBinder aiBinder = Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(object), typeof(Program));
            ParameterExpression paramExpr = Expression.Parameter(aiD.GetType(), "o");
            MemberExpression namePropExpr = Expression.Field(paramExpr, "Name");
            var Dynamic2 = Expression.MakeDynamic(typeof(Func<CallSite, object, object>)
                , aiBinder
                , namePropExpr
          );
            LambdaExpression laExp = Expression.Lambda(
              Expression.Block(
              new ParameterExpression[] { paramExpr },
              Expression.Block(Expression.Assign(paramExpr, Expression.Constant(aiD))
              )
             , Dynamic2));
            Console.WriteLine("AiDynamicDo2：" + laExp.ToString());
            Console.WriteLine("结果：" + laExp.Compile().DynamicInvoke());
            Console.WriteLine();
        }


        /// <summary>
        /// 引用类型方法调用测试 Binder.Convert
        /// </summary>
        /// <param name="aiD"></param>
        public void AiDynamicDo3(dynamic aiD)
        {
            //Binder.Convert
            CallSiteBinder aiBinder = Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(object), typeof(Program));
            // ParameterExpression aiParamExpr = Expression.Parameter(aiD.GetType(), "o");
            ParameterExpression aiParamExpr1 = Expression.Parameter(typeof(string), "a");
            ParameterExpression aiParamExpr2 = Expression.Parameter(typeof(string), "b");
            MethodCallExpression aiMth = Expression.Call(
                aiD.GetType().GetMethod("AiTest", new Type[] { typeof(string), typeof(string) }), aiParamExpr1, aiParamExpr2);
            var Dynamic2 =
              Expression.MakeDynamic(
              typeof(Func<CallSite, dynamic, object>),
              aiBinder, aiMth

          );
            LambdaExpression laExp = Expression.Lambda(Dynamic2, aiParamExpr1, aiParamExpr2);
            Console.WriteLine("AiDynamicDo3：" + laExp.ToString());
            Console.WriteLine("结果：" + laExp.Compile().DynamicInvoke("AiTest-", "动态调用"));
            Console.WriteLine();
        }


        /// <summary>
        /// 值类型方法调用测试 Binder.Convert
        /// </summary>
        /// <param name="aiD"></param>
        public void AiDynamicDo4(dynamic aiD)
        {
            CallSiteBinder aiBinder = Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(object), typeof(Program));
            //ParameterExpression aiParamExpr = Expression.Parameter(aiD.GetType(), "o");
            ParameterExpression aiParamExpr1 = Expression.Parameter(typeof(int), "a");
            ParameterExpression aiParamExpr2 = Expression.Parameter(typeof(int), "b");

            MethodCallExpression aiMth = Expression.Call(
              aiD.GetType().GetMethod("add", new Type[] { typeof(int), typeof(int) }), aiParamExpr1, aiParamExpr2);
            var Dynamic2 =
              Expression.MakeDynamic(
              typeof(Func<CallSite, dynamic, object>),
             aiBinder,
              Expression.Convert(aiMth, typeof(object)) //！！这里要转换，否刚不能通过

          );
            //LambdaExpression laExp = Expression.Lambda(Dynamic2, paramExpr1, paramExpr2);
            LambdaExpression laExp = Expression.Lambda(
                 Expression.Block(
                new ParameterExpression[] { aiParamExpr1, aiParamExpr2 },
                Expression.Block(Expression.Assign(aiParamExpr1, Expression.Constant(30)),
                Expression.Assign(aiParamExpr2, Expression.Constant(40))
                ),
                Dynamic2
                )
            );
            Console.WriteLine("AiDynamicDo4：" + laExp.ToString());
            Console.WriteLine("结果：" + laExp.Compile().DynamicInvoke());
            Console.WriteLine();
        }

        //与上例不同的是，本过程不是调用的反射方法
        public void AiDynamicDo5(dynamic aiD)
        {
            //当调用方法是void类型时 应该用 CSharpBinderFlags.ResultDiscarded
            //第二个参数是方法名
            //第三个参数要泛型时才结出
            //第五个参数要与表达示参数据相同
            var aiBinder = Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "add2", null, typeof(Program),
               new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                   CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), 
                   CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });

            ParameterExpression aiParamExpr = Expression.Parameter(typeof(object), "o");
            ParameterExpression aiParamExpr1 = Expression.Parameter(typeof(int), "a");
            ParameterExpression aiParamExpr2 = Expression.Parameter(typeof(int), "b");
            //与上例中不同的地方
            DynamicExpression aiDynamic = Expression.MakeDynamic(typeof(Func<CallSite, object, int, int, object>)
              , aiBinder
              , aiParamExpr
              , aiParamExpr1
              , aiParamExpr2

          );
            //下面这种使用方式，在调用时DynamicInvoke(aiD,30,40)
            //LambdaExpression laExp = Expression.Lambda(Dynamic2, paramExpr, paramExpr1, paramExpr2);
            LambdaExpression aiLabExp = Expression.Lambda(
                 Expression.Block(
                new ParameterExpression[] { aiParamExpr, aiParamExpr1, aiParamExpr2 }
                    , Expression.Assign(aiParamExpr, Expression.Constant(aiD))
                    , Expression.Assign(aiParamExpr1, Expression.Constant(30))
                    , Expression.Assign(aiParamExpr2, Expression.Constant(40))
                    , aiDynamic));

            Console.WriteLine("AiDynamicDo5：" + aiLabExp.ToString());
            Console.WriteLine("结果：" + aiLabExp.Compile().DynamicInvoke());
            Console.WriteLine();
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public static class ObjectExtension
    {
        public static object GetProperty<T>(this T t, string propertyName) where T : class,new()
        {
            Type type = t.GetType();

            PropertyInfo p = type.GetProperty(propertyName);

            if (p == null)
                throw new Exception(string.Format("该类型没有名为{0}的属性", propertyName));

            var param_obj = Expression.Parameter(typeof(T));

            var body_obj = Expression.Convert(param_obj, type);

            var body = Expression.Property(body_obj, p);


            var getValue = Expression.Lambda<Func<T, object>>(body, param_obj).Compile();

            return getValue(t);

        }

        public static void SetPropertyValue<T>(this T t, string propertyName, object value) where T : class,new()
        {
            Type type = t.GetType();

            PropertyInfo p = type.GetProperty(propertyName);

            if (p == null)
                throw new Exception(string.Format("该类型没有名为{0}的属性", propertyName));

            var parma_obj = Expression.Parameter(type);

            var parma_val = Expression.Parameter(typeof(object));

            var body_obj = Expression.Convert(parma_obj, type);

            var body_val = Expression.Convert(parma_val, p.PropertyType);

            var setMethod = p.GetSetMethod(true);

            if (setMethod != null)
            {
                var body = Expression.Call(body_obj, p.GetSetMethod(), body_val);

                var setValue = Expression.Lambda<Action<T, object>>(body,
                   parma_obj, parma_val).Compile();

                setValue(t, value);
            }

        }

    }

    public class Student
    {
        private string _name;

        private int _sex;
        public Student(string name, int sex)
        {
            this._name = name;
            this._sex = sex;
        }

        public void ShowInformation()
        {
            Console.WriteLine("Inital Personal Information Name is {0} Sex is {1}",
                _name, Enum.GetName(typeof(Sex), _sex).ToString());
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Gender
        {
            get
            {
                return Enum.GetName(typeof(Sex), _sex).ToString();
            }
            set
            {
                _sex = Convert.ToInt32(value);
            }
        }

        public enum Sex
        {
            Men = 0,
            Women = 1,
        }
    }

    public class AiTestD
    {
        public string Name = "你好！这是 AiTestD";

        public int Id = 123;
        public static string AiTest(string ai1, string ai2)
        {
            return ai1 + ai2;
        }

        public static int add(int a, int b)
        {

            return a + b;
        }

        public int add2(int a, int b)
        {

            return -(a + b);
        }
    }

    public class AiTestD2
    {
        public string Name = "你好！这是 AiTestD2";
        public int Id = 789;
        public static string AiTest(string ai1, string ai2)
        {
            return "**" + ai1 + ai2;
        }
        public static int add(int a, int b)
        {
            return (a + b) * 1000;
        }
        public int add2(int a, int b)
        {
            return -(a + b) * 1000;
        }
    }
}
