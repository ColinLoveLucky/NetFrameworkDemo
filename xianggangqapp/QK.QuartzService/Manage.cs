using QK.JobExcute;
//using QK.JobCommon;

namespace QK.QuartzService
{
    public class Manager
    {
        public void OnStart()
        {
            //LogHelper.WriteLog("Service start");
            QuartzUtil.StartScheduler();
            QuartzUtil.SchedulerAllJob();
        }
        public void OnStop()
        {
            //LogHelper.WriteLog("Service stop");
            QuartzUtil.StopScheduler();
        }
    }
}
