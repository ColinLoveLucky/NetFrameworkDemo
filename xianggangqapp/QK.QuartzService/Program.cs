using Topshelf;
//using QK.JobCommon;

namespace QK.QuartzService
{
    public class Program
    {
        static void Main(string[] args)
        {
            //LogHelper.SetConfig();

            HostFactory.Run(x =>
            {
                x.Service<Manager>(s =>
                {
                    s.ConstructUsing(name => new Manager());
                    s.WhenStarted(tc => tc.OnStart());
                    s.WhenStopped(tc => tc.OnStop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("QK AutoJob QuartzService");
                x.SetDisplayName("QuartzAutoJobService");
                x.SetServiceName("QuartzAutoJobService");
            });
        }
    }
}
