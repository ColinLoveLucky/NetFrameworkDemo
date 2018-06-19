using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;

namespace QK.QAPP.IServices
{
    public interface IQBBidJobConfigService
    {
        /// <summary>
        /// 标的job配置列表取得
        /// </summary>
        /// <returns></returns>
        List<QB_JOB_CONFIG_INFO> GetBidJobConfigList();

        /// <summary>
        /// 取得要更新的job配置数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        QB_JOB_CONFIG_INFO GetBidJobConfigEditInfo(string id);

        /// <summary>
        /// 创建或者更新job配置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string CreateOrEditJobConfig(QB_JOB_CONFIG_INFO entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteJobConfigInfo(string id);

        /// <summary>
        /// 取得要更新的job配置数据
        /// </summary>
        /// <param name="jobtype"></param>
        /// <returns></returns>
        List<string> GetAmtByJobType(string jobtype);

        /// <summary>
        /// 任务操作（启动，关闭）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool JobOperateConfig(string type, string id);

        /// <summary>
        /// 检查任务是否正在运行
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        List<QB_JOB_CONFIG_INFO> CheckJobRun(List<QB_JOB_CONFIG_INFO> entity);

        /// <summary>
        /// 检查任务是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool CheckJobExist(QB_JOB_CONFIG_INFO entity);
    }
}
