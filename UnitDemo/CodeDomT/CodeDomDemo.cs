using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.CodeDomT
{
    public class CodeDomDemo
    {
        // ICodeGenerator
        // IndentedTextWriter
        // CodeThisReferenceExpression
        // CodeTypeOfExpression
        // CodeConstructor
        // CodeMemberMethod
        //  CodeMemberProperty
        //CodeTypeDeclaration
        //CodeNamespaceImport 
        //CodeTypeDeclaration
        //CodeTypeMember
        //CodeCompileUnit
        //CodeCompileUnit
        // CSharpCodeProvider
        // CodeDomProvider
        // System.CodeDom.CodeSnippetExpression

        /// <summary>
        /// 创建命名空间
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public CodeNamespace InitalNamespace(string name)
        {
            CodeNamespace currentNameSpace = new CodeNamespace(name);
            currentNameSpace.Imports.Add(new CodeNamespaceImport("System"));
            currentNameSpace.Imports.Add(new CodeNamespaceImport("System.Text"));
            currentNameSpace.Imports.Add(new CodeNamespaceImport("UnitDemo.CodeDomT"));
            return currentNameSpace;
        }
        /// <summary>
        /// 创建类
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CodeTypeDeclaration CreateClass(string name)
        {
            CodeTypeDeclaration ctd = new CodeTypeDeclaration(name);
            ctd.IsClass = true;
            ctd.Attributes = MemberAttributes.Public;
            return ctd;
        }

        public CodeMemberMethod CreateMethod()
        {
            //CodeMemberMethod method = new CodeMemberMethod();
            //method.Name = "SayHi";
            //var parm1 = new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), "age");
            //var parm2 = new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "name");
            //method.Parameters.AddRange(new CodeParameterDeclarationExpression[] { parm1, parm2 });
            //method.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            CodeEntryPointMethod method = new CodeEntryPointMethod();
            method.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            return method;
        }

        public CodeVariableDeclarationStatement DeclareVariables(Type dataType, string name)
        {
            CodeTypeReference tr = new CodeTypeReference(dataType);
            CodeVariableDeclarationStatement declaration = new CodeVariableDeclarationStatement(tr, name);
            CodeObjectCreateExpression initStatement = new CodeObjectCreateExpression();
            initStatement.CreateType = tr;
            declaration.InitExpression = initStatement;
            return declaration;
        }

        public CodeVariableDeclarationStatement InitializeArray(string name, params char[] characters)
        {
            CodeTypeReference tr = new CodeTypeReference(characters.GetType());
            CodeVariableDeclarationStatement declaration = new CodeVariableDeclarationStatement(tr, name);
            CodePrimitiveExpression[] cpe = new CodePrimitiveExpression[characters.Length];
            for (int i = 0; i < name.Length; i++)
                cpe[i] = new CodePrimitiveExpression(characters[i]);
            CodeArrayCreateExpression array = new CodeArrayCreateExpression(tr, cpe);
            declaration.InitExpression = array;
            return declaration;
        }

        public CodeIterationStatement CreateLoop(string loopControlVariableName)
        {
            CodeVariableDeclarationStatement declaration;
            CodeIterationStatement forLoop = new CodeIterationStatement();
            declaration = new CodeVariableDeclarationStatement(typeof(int), loopControlVariableName);
            declaration.InitExpression = new CodeSnippetExpression("0");
            forLoop.InitStatement = declaration;
            CodeAssignStatement assignment = new CodeAssignStatement(
                new CodeVariableReferenceExpression(loopControlVariableName),
                new CodeSnippetExpression(loopControlVariableName + " + 1"));
            forLoop.IncrementStatement = assignment;
            forLoop.TestExpression = new CodeSnippetExpression(loopControlVariableName + " < Characters.Length");
            return forLoop;
        }

        public CodeArrayIndexerExpression CreateArrayIndex(string arrayName, string indexValue)
        {
            CodeArrayIndexerExpression index = new CodeArrayIndexerExpression();
            index.Indices.Add(new CodeVariableReferenceExpression(indexValue));
            index.TargetObject = new CodeSnippetExpression(arrayName);
            return index;
        }

        public CodeNamespace CodeDomProviderDemo()
        {
            var currentNamespace = InitalNamespace("TestSpace");
            var ctd = CreateClass("HelloWorld");
            //把类加入到名称空间
            currentNamespace.Types.Add(ctd);
            var mtd = CreateMethod();
            //把方法加入到类
            ctd.Members.Add(mtd);
            var variableDecalaration = DeclareVariables(typeof(StringBuilder), "sbMessage");
            mtd.Statements.Add(variableDecalaration);
            //把数组加入到方法
            var array = InitializeArray("Characters", 'H', 'E', 'L', 'L', 'L', 'W', 'O', 'R', 'L', 'd');
            mtd.Statements.Add(array);
            ////把循环加入到方法
            var loop = CreateLoop("intCharacterIndex");
            mtd.Statements.Add(loop);
            var index = CreateArrayIndex("Characters", "intCharacterIndex");
            loop.Statements.Add(new CodeMethodInvokeExpression(
                new CodeSnippetExpression("sbMessage"), "Append", index));
            ////循环结束后，输出所有字符追加到sbMessage对象
            ////后得到的结果
            mtd.Statements.Add(new CodeSnippetExpression("Console.WriteLine(sbMessage.ToString())"));
            mtd.Statements.Add(new CodeSnippetExpression("new CodeTest().SayHi()"));
            return currentNamespace;
        }

        public string GenerateCode(ICodeGenerator codeGenerator)
        {
            CodeGeneratorOptions cop = new CodeGeneratorOptions();
            //指定格式：花括号的位置
            cop.BracingStyle = "C";
            cop.IndentString = "";
            StringBuilder sbCode = new StringBuilder();
            StringWriter sw = new StringWriter(sbCode);
            //生成代码
            var codedom = CodeDomProviderDemo();
            codeGenerator.GenerateCodeFromNamespace(codedom, sw, cop);
            return sbCode.ToString();
        }

        public string VBCode
        {
            get
            {
                VBCodeProvider provider = new VBCodeProvider();
                ICodeGenerator codeGen = provider.CreateGenerator();
                return GenerateCode(codeGen);
            }
        }
        public string CSharpCode
        {
            get
            {
                CSharpCodeProvider provider = new CSharpCodeProvider();
                ICodeGenerator codeGen = provider.CreateGenerator();
                return GenerateCode(codeGen);
            }
        }
    }

    public class CodeTest
    {
        public void SayHi()
        {
            Console.WriteLine("SayHi");
        }
    }

}
