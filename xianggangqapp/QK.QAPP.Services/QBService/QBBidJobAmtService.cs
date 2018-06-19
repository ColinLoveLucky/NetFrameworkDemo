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
    public class QBBidJobAmtService : IQBBidJobAmtService
    {
        /// <summary>
        /// 标的job配置列表取得
        /// </summary>
        /// <returns></returns>
        public List<JobAmtInfo> GetJobAmtInfoList()
        {
            var rest = new RestApiHelper(GlobalApi.QKAutoAmtJob);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(null);

            return rest.Get<List<JobAmtInfo>>("GetJobAmtInfoList", paras);
        }

        /// <summary>
        /// 取得要更新的job配置数据
        /// </summary>
        /// <returns></returns>
        public JobAmtInfo GetBidJobEditInfoByType(string amttype)
        {
            var rest = new RestApiHelper(GlobalApi.QKAutoAmtJob);
            NameValueCollection para = new NameValueCollection();
            para.Add("amttype", amttype);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(para);

            return rest.Get<JobAmtInfo>("GetJobAmtInfoByType", paras);
        }

        /// <summary>
        /// 创建或者更新额度任务配置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string CreateOrEditJobAmt(JobAmtInfo entity)
        {
            var rest = new RestApiHelper(GlobalApi.QKCreateOrUpdateJobAmtInfo);
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(null);
            var result = rest.Post<JobAmtInfo>(rest.GetUrlParam(securityKey), entity);
            return "";

        }

        public string DeleteJobAmtInfo(string amttype)
        {
            var rest = new RestApiHelper(GlobalApi.QKDeleteJobAmtInfo);
            NameValueCollection para = new NameValueCollection();
            para.Add("amttype", amttype);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(para);

            var result = rest.Get<string>(string.Empty, paras);
            return "";

        }
    }
}
