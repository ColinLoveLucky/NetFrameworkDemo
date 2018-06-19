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

    public class QBBidJobConfigService : IQBBidJobConfigService
    {
        [Dependency]
        public IQBQuartzService qbQuartzService { get; set; }

        /// <summary>
        /// 标的job配置列表取得
        /// </summary>
        /// <returns></returns>
        public List<QB_JOB_CONFIG_INFO> GetBidJobConfigList()
        {
            var rest = new RestApiHelper(GlobalApi.QKAutoJobConfig);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(null);

            return rest.Get<List<QB_JOB_CONFIG_INFO>>("GetJobConfigInfoList", paras);
        }

        /// <summary>
        /// 取得要更新的job配置数据
        /// </summary>
        /// <returns></returns>
        public QB_JOB_CONFIG_INFO GetBidJobConfigEditInfo(string id)
        {
            var rest = new RestApiHelper(GlobalApi.QKAutoJobConfig);
            NameValueCollection para = new NameValueCollection();
            para.Add("id", id);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(para);

            return rest.Get<QB_JOB_CONFIG_INFO>("GetJobConfigInfoById", paras);
        }

        /// <summary>
        /// 创建或者更新job配置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string CreateOrEditJobConfig(QB_JOB_CONFIG_INFO entity)
        {
            var rest = new RestApiHelper(GlobalApi.QKCreateOrUpdateJobConfigInfo);
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(entity));
            var result = rest.Post<String>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(entity));
            return result;

        }

        public string DeleteJobConfigInfo(string id)
        {
            var rest = new RestApiHelper(GlobalApi.QKAutoJobConfig);
            NameValueCollection para = new NameValueCollection();
            para.Add("id", id);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(para);

            var result = rest.Get<String>("/DeleteJobConfigInfo", paras);
            return "";

        }

        /// <summary>
        /// 任务启动 停止 操作
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool JobOperateConfig(string type, string id)
        {
            var result = true;
            var rest = new RestApiHelper(GlobalApi.QKAutoJobConfig);
            NameValueCollection para = new NameValueCollection();
            para.Add("id", id);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(para);

            QB_JOB_CONFIG_INFO model = rest.Get<QB_JOB_CONFIG_INFO>("GetJobConfigInfoById", paras);
            if (model != null)
            {
                if (type == "1")
                {
                    result = qbQuartzService.QuartzAddOneJob(model);
                }
                else
                {
                    result = qbQuartzService.QuartzDeleteOneJob(model);
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        public List<QB_JOB_CONFIG_INFO> CheckJobRun(List<QB_JOB_CONFIG_INFO> entity)
        {
            foreach (QB_JOB_CONFIG_INFO model in entity)
            {
                if (qbQuartzService.QuartzCheckJobsExist(model))
                {
                    model.IS_RUN = "1";
                }
                else
                {
                    model.IS_RUN = "0";
                }
            }
            return entity;
        }

        public bool CheckJobExist(QB_JOB_CONFIG_INFO entity)
        {
            return qbQuartzService.QuartzCheckJobsExist(entity);
        }

        public List<string> GetAmtByJobType(string jobtype)
        {
            var rest = new RestApiHelper(GlobalApi.QKAutoAmtJob);
            NameValueCollection para = new NameValueCollection();
            para.Add("jobtype", jobtype);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(para);

            return rest.Get<List<string>>("GetAmtByJobType", paras);

        }
    }
}
