using System;
using System.Threading.Tasks;
using Quartz;

namespace QuartzDemo
{
    public class HelloJob : IJob
    {
        //public Task Execute(IJobExecutionContext context)
        //{
        //    return Task.Run(() => Console.WriteLine("task is executing!"));
        //}

        void IJob.Execute(IJobExecutionContext context)
        {
            Console.WriteLine("task is executing!");
        }
    }

    public class HelloWorldJob : IJob
    {
        void IJob.Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello World!");
        }
    }

}