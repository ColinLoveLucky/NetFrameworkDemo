using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.CodeDomT
{
    public class CodeComplier
    {
        private CompilerParameters _comparameters;
        private CodeDomProvider _provider;
        private bool _isCompiler;
        private Assembly _compiledAssembly;
        private string _compilerContent;

        public CodeComplier()
        {
            CodeDomDemo test = new CodeDomDemo();
            _compilerContent = test.CSharpCode;
        }

        //创建编译器
        public void CreateCompiler(string language, bool debugModel, string assemblyFileName)
        {
            _comparameters = new CompilerParameters();
            _comparameters.OutputAssembly =
                System.IO.Path.Combine(System.IO.Path.GetTempPath(), assemblyFileName + ".dll");
            _comparameters.GenerateExecutable = false;
            _comparameters.GenerateInMemory = true;
            if (debugModel)
            {
                _comparameters.IncludeDebugInformation = true;
                _comparameters.CompilerOptions += "/define:TRACE=1 /define:DEBUG=1";
            }
            else
            {
                _comparameters.IncludeDebugInformation = false;
                _comparameters.CompilerOptions += "/define:TRACE=1";
            }
            AddReference("System.dll");
            AddReference("System.Data.dll");
            AddReference("System.Xml.dll");
            AddReference(@"D:\FrameWork4.0TestDemo\UnitDemo\bin\Debug\UnitDemo.exe");
            language = language.ToLower();
            var listVb = new List<string>(){
                "visualbasic","vb"
            };
            var listCSharp = new List<string>(){
                "csharp","cs","C#"
            };
            if (listVb.Contains(language))
            {
                _provider = new Microsoft.VisualBasic.VBCodeProvider();
                if (debugModel)
                {
                    _comparameters.CompilerOptions +=
                        "/debug:full /optimize- /optionexplicit+ /optionstrict+ /optioncompare:text" +
                       " /imports:Microsoft.VisualBasic,System,System.Collections,System.Diagnostics ";
                    _comparameters.CompilerOptions += "/optimize /optionexplicit+ /optionstrict+ " +
                        "/optioncompare:text " +
                        "/imports:Microsoft.VisualBasic,System,System.Collections,System.Diagnostics ";
                    AddReference("Microsoft.VisualBasic.dll");
                }
            }
            else if (listCSharp.Contains(language))
            {
                _provider = new Microsoft.CSharp.CSharpCodeProvider();
                if (!debugModel)
                    _comparameters.CompilerOptions += "/optimize";
            }
            else
                throw new Exception("指定的脚本语言不支持");

        }
        public void AddReference(string assemblyName)
        {
            _comparameters.ReferencedAssemblies.Add(assemblyName);
        }

        /// <summary>
        /// 编译代码
        /// </summary>
        /// <returns></returns>
        public bool Complie()
        {
            var result = false;
            CompilerResults compilerResult = _provider.CompileAssemblyFromSource(_comparameters, _compilerContent);
            int nativeCompilper = 0;
            if (compilerResult.NativeCompilerReturnValue == nativeCompilper)
            {
                result = true;
                _compiledAssembly = compilerResult.CompiledAssembly;
            }
            StringBuilder compilerInfo = new StringBuilder();
            foreach (CompilerError err in compilerResult.Errors)
            {
                compilerInfo.AppendLine(err.ToString());
            }
            _isCompiler = result;
            return result;
        }

        public object Invoke(string module, string method, object[] arguments)
        {
            if (!_isCompiler || _compiledAssembly == null)
                throw new Exception("脚本还没有编译成功");
            Type moduleType = _compiledAssembly.GetType(module);
            if (moduleType == null)
                throw new Exception(string.Format("指定的类或模块{0}未定义", module));
            MethodInfo methodInfo = moduleType.GetMethod(method);
            if (methodInfo == null)
                throw new Exception(string.Format("指定的方法({0}::{1})未定义", module, method));
            try
            {
                return methodInfo.Invoke(null, arguments);
            }
            catch
            {
                throw new System.Exception(string.Format("指定的方法 ({0}:{1}) 参数错误。", module, method));
            }
        }
    }
}
