using CLR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace UnitDemo.ReflectionT
{
    /// <summary>
    /// 反射
    /// Lambda
    /// Expression Tree(Relection\Queryalbe<T></T>
    /// Emit(IOC,Reflection) 其它的IOC
    /// CodeDom
    /// Generic/Collection Hash Dictionary Sort 排序算法效率
    /// Task/Await
    /// Thread(Pareller,Lock)
    /// Dynamic
    /// Stream
    /// DDD
    /// MVC 源码
    /// WEB API 思路源码
    /// EF 源码（TransactionScope)
    /// 加密算法
    /// 把私有 dll 添加的Nuget实现dll管理
    /// MSMQ其它的队列
    /// .net remoting /wcf
    /// .Net Socket
    /// .net Core 
    /// .net 类型隐士转换的机制
    /// 类型显示转换原理
    /// </summary>
    public class ReflectionTest
    {
        public void TestReflection()
        {
            //Assembly
            //  IntPtr
        }
    }

    public interface InterT
    {

    }

    public interface InterB
    {

    }

    public delegate void Say();

    [Serializable]
    public class ReflectionClass : InterT, InterB
    {

        public event Say _sayEvent;
        private static int _staticField;
        static ReflectionClass()
        {
            _staticField = 10;

            Console.WriteLine(_staticField);
        }
        public int myField = 10;
        public ReflectionClass(int field)
        {
            myField = field;
        }
        public static void ShowHi()
        {
            Console.WriteLine("Hello World");
        }
        public void SayHi()
        {
            Console.WriteLine("Say Hi");
        }
        public static void ShowStatic()
        {
            Console.WriteLine("Hello World Static");
        }
        public string GetName(string name)
        {
            return name + Name;
        }
        public string Name
        {
            get;
            set;
        }

        public void Triggle()
        {
            if (_sayEvent != null)
            {
                _sayEvent();
            }
        }
    }

    public interface InterG
    {

    }
    public class GenericTB<T>
    {
        public void Show<T>(T t)
        {
            A a = new A();
            B b = new B();
            TF<B> tf = b.GetB;
            Dele<B, A> del = a.GetA;
        }
    }

    public delegate T TF<out T>();

    public delegate D Dele<in T, out D>(T a);
    public interface Generic<in T, out D> where T : new()
    {
        D Show(T t);
    }
    public interface GenericIn<in T> where T : new() {
        void Show(T t);
        
    }
    public interface GenericOut<out T> where T : new() {

        T ReturnT();
    }
    public class A
    {
        public B GetA(A a)
        {
            return new B();
        }
        public B GetB()
        {
            return new B();
        }
    }
    public class B:A
    {

    }
    public class GenericA<T,D> where T:Generic<T, D>,new()
    {
        public D Show(T t)
        {
            return default(D);
        }
    }
    public class GenericInA<T> where T : GenericIn<T>,new()
    {
        public void Show(T t)
        {
            throw new NotImplementedException();
        }
    }
    public class GenericOutA<T> where T : GenericOut<T>,new ()
    {
        T ReturnT()
        {
            return default(T);
        }
    }

    public class ReflectionAssembly
    {
        protected class TNest
        {

        }
        public void ShowAppDomain()
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.ResourceResolve += CurrentDomain_ResourceResolve;
            AppDomain.CurrentDomain.TypeResolve += CurrentDomain_TypeResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var assemblys = AppDomain.CurrentDomain.CreateInstance("UnitDemo", "UnitDemo.ReflectionT.ReflectionClass");
            ReflectionClass test = (ReflectionClass)assemblys.Unwrap();
            test.SayHi();
            string filePath = @"D:\FrameWork4.0TestDemo\CLR\bin\Debug\CLR.exe";
            AppDomain.CurrentDomain.ExecuteAssembly(filePath);
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);

            AppDomainSetup appDomainSetup = new AppDomainSetup();
            appDomainSetup.LoaderOptimization = LoaderOptimization.SingleDomain;
            appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            //  appDomainSetup.PrivateBinPath = AppDomain.CurrentDomain.BaseDirectory;// @"D:\FrameWork4.0TestDemo\CLR\bin";
            Evidence evidence = AppDomain.CurrentDomain.Evidence;
            var appDomain = AppDomain.CreateDomain("UnitDemoCLR", evidence, appDomainSetup);
            var type = appDomain.CreateInstance("UnitDemo", "UnitDemo.ReflectionT.ReflectionClass").Unwrap().GetType();
            type.InvokeMember("ShowHi", BindingFlags.InvokeMethod, null, null, null);
            Console.WriteLine(appDomain.FriendlyName);

            AppDomain MySampleDomain = AppDomain.CreateDomain("MySampleDomain");
            Assembly ass = Assembly.Load(new AssemblyName("UnitDemo"));
            Object obj = MySampleDomain.CreateInstanceAndUnwrap(ass.FullName, "UnitDemo.ReflectionT.ReflectionClass");
            obj.GetType().GetMethod("ShowHi").Invoke(obj, null);
        }
        //public void ShowAssembly()
        //{

        //    // 借鉴文章http://www.cnblogs.com/dajiang02/archive/2011/11/10/2243997.html
        //    //从GAC 里面读取代码
        //    //dll 注册public Key
        //    //gacutil -if  Name
        //    //gacutil - /lr name 查看
        //    //默认是不会查找从全局GAC查找 
        //    //需要添加  <runtime>
        //    //  <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
        //    //    <qualifyAssembly partialName="CLR" fullName="CLR, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9bcc9a282fca3818"/>
        //    //  </assemblyBinding>
        //    //</runtime>
        //    //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        //    Assembly assembly;
        //    try
        //    {
        //        //Assembly.Load()http://www.cnblogs.com/danielWise/archive/2011/09/07/2170042.html
        //        //<!--<disableCachingBindingFailures enabled="1" />--> 默认load 会缓存起来加载成功，如果失败也会把失败的结果缓存起来，第二次加载不执行了
        //        //启用标识 绝句缓存
        //        //AssemblyName name = new AssemblyName("CLR");
        //        //assembly = Assembly.Load(name);
        //    }
        //    catch (Exception ex)
        //    {
        //        AssemblyName name = new AssemblyName("CLR");
        //        assembly = Assembly.Load(name);
        //    }
        //    //  Console.WriteLine(assembly.FullName);
        //    //Console.WriteLine("所有可用探索目录");
        //    //foreach (var s in GetAvailablePath(AppDomain.CurrentDomain.SetupInformation))
        //    //    Console.WriteLine(s);
        //    //Assembly.Load 是加载所有的依赖程序集？为Assembly.Load 是将整个程序集以及其相关的依赖程序集全部加载进来
        //    //  Assembly.ReflectionOnlyLoad()

        //    //AssemblyName assName = new AssemblyName("TestMain");
        //    //var assTestMain = Assembly.Load(assName);
        //    //var module = assTestMain.GetModule("TestCla.netmodule");
        //    //var obj = assTestMain.CreateInstance("CLR.TestCla");
        //    //var type = obj.GetType();
        //    //type.GetMethod("Show").Invoke(obj, null);
        //    //LoadFile

        //    //Load File 只加载单个的dll，不会加载引用的dll

        //    //Load File 会根据路径不通重复加载

        //    //string path = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\CLR.exe";

        //    //var loadFileAssembly = Assembly.LoadFile(path);

        //    //var loadModules = loadFileAssembly.GetModules();

        //    //var objLoadFile = loadFileAssembly.CreateInstance("CLR.TestCla");

        //    //var objType = objLoadFile.GetType();

        //    //var methodsByFrom1 = objType.GetMethods();

        //    //objType.GetMethod("Show").Invoke(objLoadFile, null);

        //    //string path2 = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe";

        //    //var loadFileAssembly2 = Assembly.LoadFile(path2);

        //    //var loadModules2 = loadFileAssembly2.GetModules();

        //    //var objLoadFile2 = loadFileAssembly2.CreateInstance("CLR.TestCla");

        //    //var objType2 = objLoadFile2.GetType();

        //    //var methodsByFrom3 = objType2.GetMethods();

        //    //objType2.GetMethod("Show").Invoke(objLoadFile2, null);

        //    //Load From

        //    //Load From 也会根据路径不同重新加载新的dll，不存在说程序集的AssemblyName一样会不加载新的

        //    //string fromPath = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\CLR.exe";
        //    //var loadFromAssembly = Assembly.LoadFrom(fromPath);
        //    //var loadModulesByFrom = loadFromAssembly.GetModules();
        //    //var objLoadFileByFrom = loadFromAssembly.CreateInstance("CLR.TestCla");
        //    //var objTypeByFrom = objLoadFileByFrom.GetType();
        //    //var methodsByFrom = objTypeByFrom.GetMethods();
        //    //objTypeByFrom.GetMethod("Show").Invoke(objLoadFileByFrom, null);

        //    //string fromPath2 = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe";
        //    //var loadFromAssembly2 = Assembly.LoadFrom(fromPath2);
        //    //var loadModulesByFrom2 = loadFromAssembly2.GetModules();
        //    //var objLoadFileByFrom2 = loadFromAssembly2.CreateInstance("CLR.TestCla");
        //    //var objTypeByFrom2 = objLoadFileByFrom2.GetType();
        //    //var methodsByFrom2 = objTypeByFrom2.GetMethods();
        //    //objTypeByFrom2.GetMethod("Show").Invoke(objLoadFileByFrom2, null);

        //    //string fromPath3 = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\TestMain.dll";
        //    //var assName3 = new AssemblyName("TestMain");
        //    //var lodAssembly3 = Assembly.LoadFile(fromPath3);
        //    //var loadModulesByFrom3 = lodAssembly3.GetModules();
        //    //var typess = lodAssembly3.GetTypes();


        //    //string reflectionLoadPath = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe";
        //    //Byte[] bytes;
        //    //using (Stream stream = new FileStream(reflectionLoadPath, FileMode.Open))
        //    //{
        //    //    long length = stream.Length;
        //    //    bytes = new Byte[length];
        //    //    stream.Read(bytes, 0, (int)length);

        //    //    stream.Close();
        //    //}
        //    //bytes = File.ReadAllBytes(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe");
        //    //var t = Assembly.LoadFrom(reflectionLoadPath);
        //    //var ttt = Assembly.LoadFile(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe");
        //    //var tttt = Assembly.Load(bytes);


        //    //var isEqualFromAndFile = (t == ttt);
        //    //var isEqualFromAndLoad = t == tttt;
        //    var assems=AppDomain.CurrentDomain.GetAssemblies();
        //   // var isEqualUnitDemo = t == assems[1];
        //  //  var tt = Assembly.ReflectionOnlyLoad(bytes);
        //   // var isReflectEqual = t == tt;

        //    var assemblyReflectionLoad = Assembly.ReflectionOnlyLoadFrom(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe");

        //    foreach (Assembly ass in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
        //    {
        //        Console.WriteLine(ass.Location);
        //    }

        //    var typesByReflectionLoad = assemblyReflectionLoad.GetTypes();

        //}
        public void ShowAssembly()
        {
            //宗下所述：程序集的加载时根据路径 加上 程序集的 FullName来确定的

            var assed22 = AppDomain.CurrentDomain.GetAssemblies();
            // var test = new CLR.TestCla();
            // test.Show();
            var assed1 = AppDomain.CurrentDomain.GetAssemblies();

            //1：.net 加载程序集是按需要加载的，如果要想程序集清单里面包含引用的程序集，必须在代码里面调用 该程序集里面的方法，.net 
            //   IDE 才会把起相应的引用添加入 程序集清单里面
            //2:但是在默认加载的时候是不会被加载进入当前的应用程序域的，只有在调用到引用里面的方法的时候才会把程序集添加到当前的
            //运用程序域里面

            var t = AppDomain.CurrentDomain.GetAssemblies();
            var ttt = Assembly.LoadFrom(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\64\TestMain.exe");
            var modules = ttt.Modules;
            var types = ttt.GetTypes();
            var ttb = AppDomain.CurrentDomain.GetAssemblies();

            //程序集是动态加载的按需加载的
            //1:默认引用的程序集不会被加载，只有在调用时候被加载
            //2:调用时运行时加载如果配置了<dependentAssembly>
            //  <assemblyIdentity type='win32' name='CLR' version='3.0.0.0'  publicKeyToken='9bcc9a282fca3818' />
            //  <codeBase version="3.0.0.0" href="file:///D:/FrameWork4.0TestDemo/UnitDemo/bin/Debug/32/CLR.exe" />
            //  <bindingRedirect oldVersion="2.0.0.0" newVersion="3.0.0.0"/>
            //</dependentAssembly>
            //文件会直接根据路径加载不同的程序集
            //3:然而通过Lode等方法还是会按照 程序集的加载顺序来加载 程序集
            //比如程序集里面有就得clr 还是会调用旧的clr

            // var ass= Assembly.LoadFile(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\TestCla.netmodule");
            //LoadRelection

            //var reflectionLoad1 = Assembly.ReflectionOnlyLoad("CLR");
            //var reflectionAsss1 = AppDomain.CurrentDomain.GetAssemblies();
            //var st = Assembly.LoadFrom(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe");

            //1:ReflectionLoad 加载的也是缓存结果

            //LoadRelectionFrom

            //var pathloadFromReflection = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\CLR.exe";
            //var reflectionLoadFrom = Assembly.ReflectionOnlyLoadFrom(pathloadFromReflection);
            //var reflectionAsssFrom = AppDomain.CurrentDomain.GetAssemblies();

            //var pathloadFromReflection1 = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\CLR.exe";
            //var reflectionLoadFrom1 = Assembly.ReflectionOnlyLoadFrom(pathloadFromReflection1);
            //var reflectionAsssFrom1 = AppDomain.CurrentDomain.GetAssemblies();

            //1:从已加载的路径再次加载
            //2:根据fullName 加载 ，如果已加载，再从其它地方加载baocuo
            //3:加载的程序集没有放在AppDomain.CUrrentDomain.程序集集合里面

            //测试Assembly加载的程序集

            var assems = AppDomain.CurrentDomain.GetAssemblies();
            //1:
            //测试结果显示 程序集是按需加载的，无论是添加引用，还是在config里面配置，默认都是不加载的
            //只有在调用的时候才会加载

            //Load
            var loadAssembly = Assembly.Load(new AssemblyName("UnitDemo"));
            var assemsnewsLoad = AppDomain.CurrentDomain.GetAssemblies();
            var isEqualCache = assemsnewsLoad[1] == loadAssembly;

            //1： Load 加载顺序是 先GAC ，后CodeBase，后Prohibing
            // 这种前提是无论你用何种加载的方法都是按照这个顺序加载的
            //2:Load 加载的程序会主动缓存，推断IDE 生成加载在AppDomain中的时候用的是Load 因为其缓存了结果
            //但是其加载的程序集比IDE加载的要多几个

            var bytes = File.ReadAllBytes(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\CLR.exe");
            var loadAssembly1 = Assembly.Load(bytes);
            var assemsnewsLoad1 = AppDomain.CurrentDomain.GetAssemblies();

            var bytes2 = File.ReadAllBytes(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe");
            var loadAssembly2 = Assembly.Load(bytes);
            var assemsnewsLoad2 = AppDomain.CurrentDomain.GetAssemblies();
            //  var isLoad2 = assemsnewsLoad2[5] == loadAssembly2;

            //3:Load 根据路径不同加载 不通用byte方法 ，但是加载的结果都写入UnitDemo总结果集了

            //LoadFrom

            var path = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\UnitDemo.exe";
            var lodeFile = Assembly.LoadFile(path);
            var assemblyFile = AppDomain.CurrentDomain.GetAssemblies();
            var isEqualFile = lodeFile == assemblyFile[1];
            //1：LoadFile 里面也缓存了结果
            //2:LodeFile 加载同一个结果集的时候加载的结果比load多

            var pathFile2 = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\CLR.exe";
            var lodeFile2 = Assembly.LoadFile(pathFile2);
            var assemblyFile2 = AppDomain.CurrentDomain.GetAssemblies();

            var pathFile3 = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe";
            var lodeFile3 = Assembly.LoadFile(pathFile3);
            var assemblyFile3 = AppDomain.CurrentDomain.GetAssemblies();
            var isEqualFile3 = lodeFile3 == assemblyFile3[4];

            //3:LoadFile 没有Gac的时候 根据路径可以加载不通的内容，但是加载的结果没有合并到总结果集


            var pathFrom = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\UnitDemo.exe";
            var loadFrom = Assembly.LoadFrom(pathFrom);
            var assemblyFrom = AppDomain.CurrentDomain.GetAssemblies();

            //1：LoadFrom 也是会从缓存结果取值

            var pathFrom2 = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\CLR.exe";
            var loadFromFile2 = Assembly.LoadFile(pathFrom2);
            var assemblyLoadFrom2 = AppDomain.CurrentDomain.GetAssemblies();

            //1:loadeFrom 加载的路径一样时，会从缓存里面查找，加载的结果跟LodeFile加载时候一样，所以不会出现重复加载的情况

            var pathFrom3 = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\CLR.exe";
            var loadFromFile3 = Assembly.LoadFile(pathFrom3);
            var assemblyLoadFrom3 = AppDomain.CurrentDomain.GetAssemblies();

        }
        /// <summary>
        /// 为什么没有manifest
        /// PE头的查看
        /// </summary>
        public void ShowModule()
        {
            string loadModulePath = @"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\32\TestCla.netmodule";

            var loadModule = Assembly.LoadFile(loadModulePath);
        }

        //Bingding
        //BingdingFlags
        //Declaring Type
        //RelectionType
        public void ShowType()
        {

            ///得到类型的几种方法

            //var qualifiedName = Assembly.CreateQualifiedName("UnitDemo", "UnitDemo.ReflectionT.ReflectionClass");
            //var refT = Type.ReflectionOnlyGetType(qualifiedName, false, true);
            //Type tTypeof = typeof(ReflectionClass);
            //Type typeGetType = Type.GetType("UnitDemo.ReflectionT.ReflectionClass");

            //TestCla testCLa = new TestCla();
            //var typesArrary = Type.GetTypeArray(new object[] { testCLa, refT });

            //var typeHandler = tTypeof.TypeHandle;
            //var typeHandlerObj = Type.GetTypeFromHandle(typeHandler);

            //var inT = tTypeof.GetInterface("InterT");
            //var ins = tTypeof.GetInterfaces();

            //int[] arra = new int[10];

            //var ty = arra.GetType();

            //int tPrimitive = 0;

            //var tTypeByPrimitive = tPrimitive.GetType();

            ////Type 元数据

            //#region《Type》

            //Console.WriteLine("ReflectionClass Is abstract {0}", tTypeof.IsAbstract);

            //Console.WriteLine("ReflectionClass Is public {0}", tTypeof.IsPublic);

            //Console.WriteLine("ReflectionClass Is HasElementType {0}", ty.HasElementType);

            //Console.WriteLine("ReflectionClass Is IsAnsiClass {0}", tTypeof.IsAnsiClass);

            //Console.WriteLine("ReflectionClass Is IsAutoClass {0}", tTypeof.IsAutoClass);//?

            //Console.WriteLine("ReflectionClass Is IsSealed {0}", tTypeof.IsSealed);

            //Console.WriteLine("ReflectionClass Is IsClass {0}", tTypeof.IsClass);

            //Console.WriteLine("ReflectionClass Is IsInterface {0}", tTypeof.IsInterface);

            //Console.WriteLine("ReflectionClass Is isArray {0}", ty.IsArray);

            //Console.WriteLine("ReflectionClass Is isValueTyoe {0}", ty.IsValueType);

            //Console.WriteLine("ReflectionClass Is IsPromitive {0}", tTypeByPrimitive.IsPrimitive);

            //Console.WriteLine("ReflectionClass Is isSerilize {0}", tTypeof.IsSerializable);

            //Console.WriteLine("ReflectionClass Is IsMarshalByRef {0}", tTypeof.IsMarshalByRef);

            //Console.WriteLine("ReflectionClass Is IsNotPullic {0}", tTypeof.IsNotPublic);

            //Console.WriteLine("ReflectionClass Is IsVisible {0}", tTypeof.IsVisible);

            //Console.WriteLine("ReflectionClass Is IsNested {0}", new TNest().GetType().IsNested);

            //Console.WriteLine("ReflectionClass Is IsNestedAssembly {0}", new TNest().GetType().IsNestedAssembly);

            //Console.WriteLine("ReflectionClass Is IsNestedFamily {0}", new TNest().GetType().IsNestedFamily);

            //Console.WriteLine("ReflectionClass Is IsNestedFamANdAssem {0}", new TNest().GetType().IsNestedFamANDAssem);

            //Console.WriteLine("ReflectionClass Is IsNestedPublic {0}", new TNest().GetType().IsNestedPublic);

            //var tGeneric = Type.GetType("UnitDemo.ReflectionT.GenericTB`1");

            //GenericTB<int> tInt = new GenericTB<int>();

            //Console.WriteLine("ReflectionClass Is IsGenericType {0}", tInt.GetType().IsGenericType);

            //Console.WriteLine("ReflectionClass Is IsGenericDefintion {0}", tGeneric.IsGenericTypeDefinition);

            //Console.WriteLine("ReflectionClass Is ContainsGenericParmeters {0}", tGeneric.ContainsGenericParameters);

            //Console.WriteLine("ReflectionClass Is GenericParmeters {0}", tGeneric.IsGenericParameter);//?

            ////  Console.WriteLine("ReflectionClass Generic ParameterPosition {0}", tInt.GetType().GenericParameterPosition);?

            //#endregion

            ////Constructor
            ReflectionClass myClass = new ReflectionClass(10);
            Type myClassType = myClass.GetType();
            //var ctr = myClassType.GetConstructor(new Type[] { typeof(Int32) });
            //var menberTYpe = ctr.MemberType;
            //// ctr.get
            //var constr = Type.GetType("UnitDemo.ReflectionT.ReflectionClass").TypeInitializer;
            //ctr.Invoke(new object[] { 20 });

            //var constructors = myClassType.GetConstructors();
            //constructors[0].Invoke(new object[] { 1500 });
            //var members = myClassType.GetMembers();

            //foreach (var item in members)
            //{
            //    if (item.MemberType == MemberTypes.Constructor)
            //    {
            //        Console.WriteLine("Constructor");
            //    }
            //}

            ////Field

            //var myFiled = myClassType.GetField("myField");

            //myFiled.SetValue(myClass, 100);

            //Console.WriteLine(myFiled.GetValue(myClass));

            ////Property

            //var property = myClassType.GetProperty("Name");

            //Console.WriteLine("Property is Write {0}", property.CanWrite);

            //property.SetValue(myClass, "Hello Zhangsan");

            //MethodInfo
            
            var methods = myClassType.GetMethods();

            // ParameterInfo[] parms;

            //foreach (var method in methods)
            //{
            //    parms = method.GetParameters();
            //    if (parms.Count() > 0)
            //    {
            //        foreach (ParameterInfo item in parms)
            //        {
            //            if(item.ParameterType==typeof(System.String))
            //            {
            //                object[] args = new object[1];

            //                args[0] = "Hello Args";

            //                method.Invoke(myClass, args);
            //            }
            //        }

            //    }
            //}

            //event

            //Caculate caculate = new Caculate();
            //ReflectionClass tc = new ReflectionClass(10);
            //Say d = (Say)tc.GetType().GetMethod("SayHi").CreateDelegate(typeof(Say), tc);
            //d.Invoke();

            //EventInfo eventSay = tc.GetType().GetEvent("_sayEvent");
            //eventSay.AddEventHandler(tc, new Say(tc.SayHi));
            //tc.Triggle();

            //Activator

            // var actor =(ReflectionClass)Activator.CreateInstance(typeof(ReflectionClass), new object[] { 12 });

            // actor.SayHi();

            //BindingFlags

            var sayMethod = myClassType.GetMethod("SayHi", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            var showHiMethod = myClassType.GetMethod("ShowHi", BindingFlags.Static | BindingFlags.Public);

            //Binder

            //  Type.DefaultBinder

            typeof(Geric<>).MakeGenericType(typeof(Caculate));
        }
        static IEnumerable<string> GetAvailablePath(AppDomainSetup set)
        {
            if (set.ApplicationBase == null)
                return Enumerable.Empty<string>();
            if (set.PrivateBinPath != null)
                return set.PrivateBinPath.Split(';').Select(s => Path.Combine(set.ApplicationBase, s));
            return new string[] { set.ApplicationBase };
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("当某个异常没有捕获");
        }
        Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine("类型解析失败");

            return null;
        }
        Assembly CurrentDomain_ResourceResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine("解析不是有效的资源");

            return null;
        }
        void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Console.WriteLine("First Exception");
        }
        private void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            Console.WriteLine("Loding UnLoad");
        }
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);
            var baseDirectory = @"D:\FrameWork4.0TestDemo\CLR\bin\Debug";
            return Assembly.LoadFrom(Path.Combine(baseDirectory, "CLR.exe"));
        }
        void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Console.WriteLine("Loading Assembly");
        }
    }

    public delegate int DelegateCaculate(int a, int b);
    public class Caculate
    {
        public int Add(int num1, int num2)
        {
            return num1 + num2;
        }
        public static int Subtract(int num1, int num2)
        {
            return num2 - num1;
        }
    }

    public class Geric<T> where T:new ()
    {
    }

  

}
