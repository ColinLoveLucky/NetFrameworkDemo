using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace UnitDemo.Gereneric
{
    public class CollectionGerneric
    {
        public void Show()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var list = new ArrayList();

            list.Clear();

            var synList = ArrayList.Synchronized(list);

            list.Add(new TestT()
            {
                Name = "List1"
            });
            var list2 = (ArrayList)list.Clone();

            list2[0] = new TestT()
            {
                Name = "List2"
            };
            var list3 = new TestT[list.Count];

            list.CopyTo(list3);

            list3[0] = new TestT()
            {
                Name = "List3"
            };

            Console.WriteLine("--List[0]:{0}---List1[0]:{1}----List3[0]:{2}", (list[0] as TestT).Name,
                (list2[0] as TestT).Name, (list3[0] as TestT).Name);

            //Console.WriteLine((list[0] as TestT).Name);

            //list.Add(1);

            //var list2 =(ArrayList) list.Clone();

            //list2[0] = 2000;

            //var list3=new int [100];

            //list.CopyTo(list3);

            //list3[0] = 1000;

            // Console.WriteLine(list[0] + "--" + list2[0] + "----" + list3[0]);

            stopwatch.Stop();

            Console.WriteLine(string.Format("一共添加了{0},1共消耗了{1},", list.Count, stopwatch.ElapsedMilliseconds));

            Console.ReadKey();

        }
        public class TestT
        {
            IList<int> test = new List<int>();
            public string Name { get; set; }
        }
        public void ShowList()
        {
            var list = new SortedList();
        }
    }


    /// <summary>
    /// Pair Tuple
    /// 泛型的反射
    /// 泛型协变逆变
    /// 泛型委托
    /// 泛型foreach
    /// 泛型约束
    /// 泛型的继承
    /// 泛型的属性索引，操作符，事件，构造器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericT<T> where T : struct
    {
        public delegate void Add<T>(T a);

        public Add<T> _add;

        public event Add<T> _eventAdd;
        static GenericT()
        {
        }

        public static T GetItem(T a, T b)
        {
            var result = default(T);

            //  result = a + b;

            return result;
        }
        public GenericT()
        {
            _add = Show;
        }
        public T this[int index]
        {
            get
            {
                return default(T);
            }
            set
            {
            }
        }

        public T Name { get; set; }

        public string Calc()
        {
            return null;
        }

        public void Show(T t)
        {

        }
    }

    public interface IAddT<T>
    {
        T Add(T a, T b);
    }

    public interface IMultiplyT<T>
    {
        T Multiply(T a, T b);
    }

    public class MatchT<T> : IAddT<T>, IMultiplyT<T>
    {
        public decimal? C
        {
            get;
            set;
        }
        public T Add(T a, T b)
        {
            var result = default(T);

            //if(a>b)
            //{
            //    return result;
            //}

            var assemblyName = new AssemblyName("AssemblyT");

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var module = assembly.DefineDynamicModule("ModuleT");

            var type = module.DefineType("GericT", TypeAttributes.Public);

            var method = type.DefineMethod("AddT", MethodAttributes.Public | MethodAttributes.Static);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Stloc_1);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
            var newType = type.CreateType();
            var newMethod = newType.GetMethod("AddT");
            result = (T)newMethod.Invoke(null, null);
            return result;

        }

        public T Add(IAddT<T> t, T a, T b)
        {
            return t.Add(a, b);
        }

        public T Multiply(T a, T b)
        {

            Dictionary<string, object> e = new Dictionary<string, object>(){
                {"a",C}
            };

            if (e["a"]==null)
            {
                Console.WriteLine("NUll");
            }

            dynamic da = a;
            dynamic db = b;
            return da * db;
        }
    }

    public class ClassTT
    {
        public ClassTT(int a)
        {
            throw new Exception("Test");
        }
    }

    public class GerenicTTTT<T> where T : new()
    {

    }

    /// <summary>
    /// 泛型协变逆变 
    /// </summary>
    /// <typeparam name="T">Out 输出 In 输入</typeparam>
    /// <typeparam name="V"></typeparam>
    public interface IMyList<out T, in V>
    {
        T GetElement();

        void ChangeV(V t);
    }

    public class MyList<T, V> : IMyList<T, V>
    {
        public T GetElement()
        {
            var result = default(T);

            return result;
        }
        public void ChangeV(V t)
        {
            Console.WriteLine(t);
        }
    }

    public abstract class Animal
    {

    }

    public class Dog : Animal
    {
        public void Show()
        {
            Console.WriteLine("Hello World");
        }
    }

    public class GenericT
    {
        public void Show()
        {
            Dog aDog = new Dog();

            aDog.Show();

            Animal animal = aDog;

            IMyList<Dog, Animal> myDog = new MyList<Dog, Animal>();

            // myDog.GetElement();

            IMyList<Animal, Dog> myAnimals = myDog;

            IList<Animal> animal1 = new List<Animal>();

            //  IList<Dog> dog = animal.Select(x => (Dog)x).ToList();
        }

        public void ShowTitleEMIT()
        {
            AssemblyName assemblyName = new AssemblyName("assemblyG");

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var module = assembly.DefineDynamicModule("dyModule");

            var typeModule = module.DefineType("dyType", TypeAttributes.Public);

            var methodModule = typeModule.DefineMethod("dyMethod", MethodAttributes.Public | MethodAttributes.Static);

            var il = methodModule.GetILGenerator();

            Dog aDog = new Dog();

            aDog.Show();

            Animal animal = aDog;

            //il.Emit(OpCodes.Newobj,)
        }
    }

    public class GenericDelegate
    {
        public delegate Animal HandleMethod();//委托协变

        public delegate void DelegaeReverChange(Dog dog);//委托逆变

        public Animal ShowAnimal()
        {
            return null;
        }

        public Dog ShowDog()
        {
            return null;
        }

        public void ShowAnimalRever(Animal animal)
        {

        }

        public void ShwoDogDelegate(Dog _dog)
        {

        }

        public void Show()
        {
            HandleMethod _handleAnimal = ShowAnimal;

            HandleMethod _handleDog = ShowDog;

            DelegaeReverChange _hanldeAnimaldel = ShowAnimalRever;

            DelegaeReverChange _hanldeDogdel = ShwoDogDelegate;
        }

    }

    public class PairDemo
    {
        public void Show()
        {
            Pair pair = new Pair();

            pair.First = "Hello World";

            pair.Second = "Hello Kitte";
        }

        public void ShowTriplet()
        {
            Triplet triplet = new Triplet()
            {
                First = "Hello World",
                Second = "Hello Kitte",
                Third = "Hello Boy"
            };
        }

        public void ShowTuple()
        {
            Tuple<int> t1 = Tuple.Create<int>(10);

            var intValue = t1.Item1;

            Tuple<int, double> t2 = Tuple.Create<int, double>(10, 20);

            var t2Item1 = t2.Item1;

            var t3Item2 = t2.Item2;
        }
    }

    public class GenericForEach
    {

        IList<string> list = null;

        //private T[] items;

        //public IEnumerator GetEnumerator()
        //{
        //    return items.GetEnumerator();
        //}

        //public IEnumerator<T> IEnumerable<T>.GetEnumerator()
        //{
        //    // return items.GetEnumerator();
        //}
    }

    public class DictionaryMap<T> : IEnumerable<T>
    {
        private Dictionary<string, string> _map = new Dictionary<string, string>();

        private T[] _items;
        public IEnumerator<T> GetEnumerator()
        {
            //return _items.GetEnumerator();

            return new EnumeratorT<T>();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class EnumeratorT<T> : IEnumerator<T>
    {
        public T Current
        {
            get { throw new NotImplementedException(); }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        object IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }
        public bool MoveNext()
        {
            throw new NotImplementedException();
        }
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public interface TBase
    {
        void ShowNum();
    }

    public interface TA : TBase
    {
        object Show();
    }

    public interface TB<T> : TA
    {
        new T Show();
    }

    public class TC<T> : TB<T>
    {
        private T _t;
        public TC(T t)
        {
            _t = t;
        }
        public T Show()
        {
            return _t;
        }
        object TA.Show()
        {
            Console.WriteLine("TA::Show");
            return Show();
        }
        public void ShowNum()
        {
            throw new NotImplementedException();
        }
    }

    public class GenricTypeReflection
    {
        public void Show()
        {
            string TbName = "UnitDemo.Gereneric.TB`1";

            Type defByNme = Type.GetType(TbName);

            var tbNameString = TbName + "[System.String]";

            Type fullName = Type.GetType(tbNameString);

            var values = defByNme.MakeGenericType(typeof(string));

            Console.WriteLine(defByNme.Name);

            Console.WriteLine(values.Name);
        }
    }
}
