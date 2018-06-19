using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public partial interface IQuotaAssignService
    {
        /// <summary>
        /// 获取个金部区域信息
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetDistrict();
        /// <summary>
        /// 获取全国剩余可用挂标额度
        /// </summary>
        /// <returns></returns>
        Decimal GetGlobalAvailableAmt(string dateTime);

        /// <summary>
        /// 获取额度分配列表
        /// </summary>
        /// <param name="assignPara">查询参数对象</param>
        /// <returns></returns>
        PageData<QB_AMT_LIMIT_ASSIGN> GetQuotaAssignList(AmtAssignListPara assignPara);

        /// <summary>
        /// 新增额度分配
        /// </summary>
        /// <param name="amtAssign"></param>
        /// <returns></returns>
        string AddQuotaAssign(QB_AMT_LIMIT_ASSIGN amtAssign);

        /// <summary>
        /// 获取单条额度分配信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        QB_AMT_LIMIT_ASSIGN GetAssignQuotaById(string id);

        /// <summary>
        /// 调整额度分配
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="adjustType">调整类型</param>
        /// <param name="assignAmt">调整金额</param>
        /// <returns></returns>
        string AdjustQuotaAssign(string id, string adjustType, string assignAmt);
    }
}
