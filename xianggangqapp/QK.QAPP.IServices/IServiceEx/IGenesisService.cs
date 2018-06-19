using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.IServices
{
    public interface IGenesisService
    {
        /// <summary>
        /// 从API获取网银认证状态
        /// </summary>
        /// <param name="preAppCode">预申请编号</param>
        /// <returns></returns>
        string GetNetbankStatusFromApi(string preAppCode);

        /// <summary>
        /// 从API获取公积金认证状态
        /// </summary>
        /// <param name="preAppCode"></param>
        /// <returns></returns>
        string GetFundStatusFromApi(string preAppCode);

        /// <summary>
        /// 从API获取Pboc认证状态
        /// </summary>
        /// <param name="preAppCode"></param>
        /// <returns></returns>
        string GetPbocStatusFromApi(string preAppCode);

        /// <summary>
        /// 获取appMain中各种认证状态
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        GenesisStatusEntity GetGenesisStatus(long appId);
    }
}
