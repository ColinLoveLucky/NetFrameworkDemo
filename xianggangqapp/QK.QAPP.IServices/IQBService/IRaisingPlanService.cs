using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public partial interface IRaisingPlanService
    {
        /// <summary>
        /// 获取募集计划列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        PageData<QB_RAISEPLAN> GetRaisingPlanList(ListViewBase para);

        /// <summary>
        /// 获取募集计划详细信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        QB_RAISEPLAN GetRaisingPlanById(string id);

        /// <summary>
        /// 获取募集计划历史列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        PageData<QB_RAISEPLAN_HISTORY> GetRaisePlanHistory(RaisePlanListPara para);
    }
}
