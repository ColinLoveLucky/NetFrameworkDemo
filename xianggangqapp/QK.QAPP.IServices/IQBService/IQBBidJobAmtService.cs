using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;

namespace QK.QAPP.IServices
{
    public interface IQBBidJobAmtService
    {
        /// <summary>
        /// 标的job配置列表取得
        /// </summary>
        /// <returns></returns>
        List<JobAmtInfo> GetJobAmtInfoList();

        /// <summary>
        /// 取得要更新的job配置数据
        /// </summary>
        /// <param name="amttype"></param>
        /// <returns></returns>
        JobAmtInfo GetBidJobEditInfoByType(string amttype);

        /// <summary>
        /// 创建或者更新额度任务配置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string CreateOrEditJobAmt(JobAmtInfo entity);

        /// <summary>
        /// 删除额度任务配置信息
        /// </summary>
        /// <param name="amttype"></param>
        /// <returns></returns>
        string DeleteJobAmtInfo(string amttype);
    }
}
