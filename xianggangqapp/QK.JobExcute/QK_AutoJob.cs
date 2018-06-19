using Quartz;
using System;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;
//using QK.JobCommon;
using QK.QAPP.Entity;
using System.Configuration;

namespace QK.JobExcute
{
    /// <summary>
    /// 自动任务
    /// </summary>
    public class QK_AutoJob : IJob
    {
        public const string QAPP = "qapp";
        public const string QKEY = "qk2016";

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var strQBAPI = ConfigurationSettings.AppSettings["QBAPIADDRESS"].ToString();

                QB_JOB_CONFIG_INFO model = (QB_JOB_CONFIG_INFO)context.JobDetail.JobDataMap.Get("JobConfigModel");

                // 取得接口名称
                string JobFunction = model.JOB_TYPE;

                // 取得api地址
                var rest = new RestApiHelper(strQBAPI + GlobalApi.QKAutoJob + "/" + JobFunction);


                // post对象转dictionary
                var paras = Serializer.ObjToDictionary(model);
                var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(model), "", QAPP, QKEY);

                // 调用post方法
                var result = rest.Post<String>(rest.GetUrlParam(securityKey), paras);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("error QK_AutoJob", ex);
            }
        }
    }
}
