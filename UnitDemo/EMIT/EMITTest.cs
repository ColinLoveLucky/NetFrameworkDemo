using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;

namespace UnitDemo.EMIT
{
    public class EMITTest
    {
        public void Show()
        {
            var assmName = new AssemblyName("EmitTest");

            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assmName, AssemblyBuilderAccess.RunAndSave);

            var mdlBldr = asmBuilder.DefineDynamicModule("Main", "Main.exe");

            var typeBldr = mdlBldr.DefineType("Hello", TypeAttributes.Public);

            var methodBldr = typeBldr.DefineMethod("SayHello",

                MethodAttributes.Public | MethodAttributes.Static, null, null);

            var il = methodBldr.GetILGenerator();

            il.Emit(OpCodes.Ldstr, "Hello World");

            il.Emit(OpCodes.Call,typeof(Console).GetMethod("WriteLine",new Type[]{typeof(string)}));

            il.Emit(OpCodes.Call,typeof(Console).GetMethod("ReadLine"));

            il.Emit(OpCodes.Pop);

            il.Emit(OpCodes.Ret);

            var t = typeBldr.CreateType();

            asmBuilder.SetEntryPoint(t.GetMethod("SayHello"));

            asmBuilder.Save("Main.exe");
        }
    }

    public class RealClass
    {
        public RealClass()
        {

        }

        public virtual bool Test()
        {
            return false;
        }
    }

    public class Interceptor
    {
        public object Call(string methodName,MulticastDelegate methodDelegate,params object [] args)
        {
            object obj = null;
            try
            {
                Console.WriteLine("进入拦截器,执行之前的方法");

                obj = methodDelegate.Method.Invoke(methodDelegate.Target, args);

                if ((bool)obj)
                    Console.WriteLine("返回真");
                else
                    Console.WriteLine("返回假");
            }
            catch(ApplicationException ex)
            {
                Console.WriteLine("出现异常");
            }

            return obj;
        }
    }

    public static class DynamicProxyBuilder
    {
        private const string dllName = "DynamicProxy.dll";

        public static object Wrap(Type type)
        {
            Type newType=null;

            try
            {
                Type m_Type = type;

                AppDomain domain = AppDomain.CurrentDomain;

                AssemblyBuilder m_Assembly = domain.DefineDynamicAssembly(new AssemblyName("DynamicModule"), AssemblyBuilderAccess.RunAndSave);

                ModuleBuilder m_ModuleBuilder = m_Assembly.DefineDynamicModule("Module",dllName);

                TypeBuilder m_TypeBuilder = m_ModuleBuilder.DefineType(m_Type.Name + "_proxy_" + m_Type.GetHashCode().ToString(), TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed, m_Type);

                MethodInfo[] methodInfos = m_Type.GetMethods();
                TypeBuilder[] m_NestedTypeBuilders = new TypeBuilder[methodInfos.Length];
                ConstructorBuilder[] m_NestedTypeConstructors = new ConstructorBuilder[methodInfos.Length];
                FieldBuilder m_Interceptor = m_TypeBuilder.DefineField("__Interceptor", typeof(Interceptor), FieldAttributes.Private);
                FieldBuilder[] m_MultiCastDelegates = new FieldBuilder[methodInfos.Length];
                MethodBuilder[] m_CallBackMethods = new MethodBuilder[methodInfos.Length];
                ConstructorBuilder m_ConstructorBuilder = m_TypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(Interceptor) });

                for (Int32 i = 0; i < m_NestedTypeBuilders.Length; i++)
                {
                    m_NestedTypeBuilders[i] = m_TypeBuilder.DefineNestedType("__" + methodInfos[i].Name + "__delegate", TypeAttributes.NestedPrivate | TypeAttributes.Sealed, typeof(MulticastDelegate));
                    m_NestedTypeConstructors[i] = m_NestedTypeBuilders[i].DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(Object), typeof(IntPtr) });
                    m_NestedTypeConstructors[i].SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
                    Type[] argsType = GetParameterTypes(methodInfos[i]);
                    MethodBuilder mb = m_NestedTypeBuilders[i].DefineMethod("Invoke", MethodAttributes.Public, CallingConventions.Standard, methodInfos[i].ReturnType, argsType);
                    mb.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
                }


                for (Int32 i = 0; i < methodInfos.Length; i++)
                {
                    m_MultiCastDelegates[i] = m_TypeBuilder.DefineField(methodInfos[i].Name + "_field", m_NestedTypeBuilders[i], FieldAttributes.Private);
                }

                for (Int32 i = 0; i < methodInfos.Length; i++)
                {
                    Type[] argTypes = GetParameterTypes(methodInfos[i]);
                    m_CallBackMethods[i] = m_TypeBuilder.DefineMethod("callback_" + methodInfos[i].Name, MethodAttributes.Private, CallingConventions.Standard, methodInfos[i].ReturnType, argTypes);
                    ILGenerator ilGenerator = m_CallBackMethods[i].GetILGenerator();
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    for (Int32 j = 0; j < argTypes.Length; j++)
                    {
                        ilGenerator.Emit(OpCodes.Ldarg, j + 1);
                    }
                    ilGenerator.Emit(OpCodes.Call, methodInfos[i]);
                    ilGenerator.Emit(OpCodes.Ret);
                }

                for (Int32 i = 0; i < methodInfos.Length; i++)
                {
                    Type[] argTypes = GetParameterTypes(methodInfos[i]);
                    MethodBuilder mb = m_TypeBuilder.DefineMethod(methodInfos[i].Name, MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard, methodInfos[i].ReturnType, argTypes);
                    ILGenerator ilGenerator = mb.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, m_Interceptor);
                    ilGenerator.Emit(OpCodes.Ldstr, methodInfos[i].Name);
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, m_MultiCastDelegates[i]);
                    LocalBuilder local = ilGenerator.DeclareLocal(typeof(Object[]));
                    ilGenerator.Emit(OpCodes.Ldc_I4, argTypes.Length);
                    ilGenerator.Emit(OpCodes.Newarr, typeof(Object));
                    ilGenerator.Emit(OpCodes.Stloc, local);
                    ilGenerator.Emit(OpCodes.Ldloc, local);
                    for (Int32 j = 0; j < argTypes.Length; j++)
                    {
                        ilGenerator.Emit(OpCodes.Ldc_I4, j);
                        ilGenerator.Emit(OpCodes.Ldarg, j + 1);
                        ilGenerator.Emit(OpCodes.Box, argTypes[j]);
                        ilGenerator.Emit(OpCodes.Stelem_Ref);
                        ilGenerator.Emit(OpCodes.Ldloc, local);
                    }
                    ilGenerator.Emit(OpCodes.Call, typeof(Interceptor).GetMethod("Call", new Type[] { typeof(String), typeof(MulticastDelegate), typeof(Object[]) }));
                    if (methodInfos[i].ReturnType.Equals(typeof(void)))
                    {
                        ilGenerator.Emit(OpCodes.Pop);
                    }
                    else
                    {
                        ilGenerator.Emit(OpCodes.Unbox_Any, methodInfos[i].ReturnType);
                    }
                    ilGenerator.Emit(OpCodes.Ret);
                }

                ILGenerator ilGenerator2 = m_ConstructorBuilder.GetILGenerator();
                ilGenerator2.Emit(OpCodes.Ldarg_0);
                ilGenerator2.Emit(OpCodes.Call, m_Type.GetConstructor(new Type[] { }));
                ilGenerator2.Emit(OpCodes.Ldarg_0);
                ilGenerator2.Emit(OpCodes.Ldarg_1);
                ilGenerator2.Emit(OpCodes.Stfld, m_Interceptor);
                for (Int32 i = 0; i < m_MultiCastDelegates.Length; i++)
                {
                    ilGenerator2.Emit(OpCodes.Ldarg_0);
                    ilGenerator2.Emit(OpCodes.Ldarg_0);
                    ilGenerator2.Emit(OpCodes.Ldftn, m_CallBackMethods[i]);
                    ilGenerator2.Emit(OpCodes.Newobj, m_NestedTypeConstructors[i]);
                    ilGenerator2.Emit(OpCodes.Stfld, m_MultiCastDelegates[i]);
                }
                ilGenerator2.Emit(OpCodes.Ret);

                newType = m_TypeBuilder.CreateType();

                foreach (TypeBuilder tb in m_NestedTypeBuilders)
                {
                    tb.CreateType();
                }

                m_Assembly.Save(dllName);

            }
            catch (Exception err)
            {
                throw err;
            }

            return Activator.CreateInstance(newType, new Interceptor());
        }

        internal static Type[] GetParameterTypes(MethodInfo methodInfo)
        {
            ParameterInfo[] args = methodInfo.GetParameters();
            Type[] argsType = new Type[args.Length];
            for (Int32 j = 0; j < args.Length; j++)
            {
                argsType[j] = args[j].ParameterType;
            }
            return argsType;
        }
    }
}
