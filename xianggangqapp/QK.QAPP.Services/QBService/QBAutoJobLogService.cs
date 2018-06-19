using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;
using Microsoft.Practices.Unity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.Infrastructure.MessageQueue;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Web;

namespace QK.QAPP.Services
{
    public class QBAutoJobLogService : IQBAutoJobLogService
    {
        /// <summary>
        /// 获取自动任务日志列表
        /// </summary>
        /// <param name="jobloglistpara">查询参数对象</param>
        /// <returns></returns>
        public PageData<QB_AUTO_JOB_LOG> GetAutoJobLogList(AutoJobLogListPara jobloglistpara)
        {
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(jobloglistpara));
            var rest = new RestApiHelper(GlobalApi.QKAutoJobLog);
            var result = rest.Post<PageData<QB_AUTO_JOB_LOG>>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(jobloglistpara));
            if (result == null) { return new PageData<QB_AUTO_JOB_LOG>(); }
            return result;

        }
    }
}
