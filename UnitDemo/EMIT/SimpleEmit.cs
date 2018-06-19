using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.EMIT
{
    public class SimpleEmit
    {
        public void Show()
        {
            Console.WriteLine("Hello World");
        }
        public void ShowEmit()
        {
            var assemblyName = new AssemblyName("SimpleEmit");
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var mouldleBuilder = assembly.DefineDynamicModule("TestEmit");
            var typeBuilder = mouldleBuilder.DefineType("EmitTest", TypeAttributes.Public);
            var methodBuilder = typeBuilder.DefineMethod("SayHi", MethodAttributes.Public | MethodAttributes.Static);
            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Hello World");
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ret);
            var newType = typeBuilder.CreateType();
            var method = newType.GetMethod("SayHi");
            method.Invoke(null, null);
        }
    }

    public class SimpleGerenic<T> 
    {
        static SimpleGerenic()
        {
            Console.WriteLine("Hello World");
        }

    }
}
