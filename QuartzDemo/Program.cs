using log4net;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace QuartzDemo
{
	/// <summary>
	/// https://www.quartz-scheduler.net
	/// http://docs.topshelf-project.com/en/latest/configuration/quickstart.html
	/// http://www.cnblogs.com/mushroom/p/4067558.html
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			string name = "Hi";
			name.GetType();

			//ArrayList
			//IList
			//ISet
			//IDictionary

			//string a = "123";
			////string b = a;
			////a = "789";
			////Console.WriteLine("a value is:{0} b value is :{1}", a, b);

			//string a = "123456abc    ";
			//string bSubstring=a.Substring(0,2);
			//Console.WriteLine("a value is:{0} bSubstring value is:{1}", a, bSubstring);

			//string cTrim=a.Trim();
			//Console.WriteLine("a value is:{0} cTrim value is：{1}", a, cTrim);

			//string dUpper=a.ToUpper();
			//Console.WriteLine("a value is:{0} dUpper value is：{1}", a, dUpper);

			//string eLower = a.ToLower();
			//Console.WriteLine("a value is:{0} eLower value is：{1}", a, eLower);

			//string fRemove = a.Remove(0, 1);
			//Console.WriteLine("a value is:{0} fRemove value is：{1}", a, fRemove);

			//string gReplace = a.Replace('1', 'x');
			//Console.WriteLine("a value is:{0} gReplace value is：{1}", a, gReplace);


			//string a = "hello";
			//string b = "world";
			//string c = string.Intern(a + b);
			//string d = "helloworld";
			//Console.WriteLine("c ==d is :{0}", ReferenceEquals(c, d));

			//string a = "111" + 10.ToString() + "dddd" + "eeee" + 100.ToString();

			//string a = "1";
			//string b = "2";
			//string c = "3";
			//string d = "4";

			//string e = a + b + c + "5" + d + "6";

			//StringBuilder builder = new StringBuilder(16);
			//builder.Append(10);

			//Vector

			//log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

			//HostFactory.Run(x =>
			//{
			//	x.Service<ServiceRunner>();
			//	x.SetDescription("QuartzDemo服务描述");
			//	x.SetDisplayName("QuartzDemo服务显示名称");
			//	x.SetServiceName("QuartzDemo服务名称");
			//	x.EnablePauseAndContinue();
			//});
			//ConfigurationQuartz test = new ConfigurationQuartz();
			//test.XmlConfiguration();
			//Console.Read();
			//HostFactory.Run(x =>                                 //1
			//{
			//    x.Service<TownCrier>(s =>                        //2
			//    {
			//        s.ConstructUsing(name => new TownCrier());     //3
			//        s.WhenStarted(tc => tc.Start());              //4
			//        s.WhenStopped(tc => tc.Stop());               //5
			//    });
			//    x.RunAsLocalSystem();                            //6
			//    x.SetDescription("Sample Topshelf Host");        //7
			//    x.SetDisplayName("Stuff");                       //8
			//    x.SetServiceName("Stuff");                       //9
			//});
			//HostFactory.Run(x =>
			//{
			//    x.SetDescription("Remote Service");
			//    x.SetDisplayName("PrintRemote");
			//    x.SetServiceName("PrintRemote");
			//    x.Service<RemotePrint>();
			//    x.EnablePauseAndContinue();
			//    x.StartAutomatically();
			//    x.RunAsLocalSystem();

			//});


			//	DbQuartz test = new DbQuartz();
			//test.Test();


			//Console.Read();
		}
	}
}
