using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 审批阶段标志枚举
    /// </summary>
    public enum ApproveFlagType
    {
        /// <summary>
        /// 申请提交
        /// </summary>
        PHASE_SUBMIT = 0,

        /// <summary>
        /// 录入提交
        /// </summary>
        PHASE_ENTRY,

        /// <summary>
        /// 录入补充资料
        /// </summary>
        PHASE_ENTRY_SD,

        /// <summary>
        /// 初审补充资料
        /// </summary>
        PHASE_ENTRY_APPR,

        /// <summary>
        /// 录入补件完成
        /// </summary>
        PHASE_ENTRY_SD_OK,

        /// <summary>
        /// 初审补件完成
        /// </summary>
        PHASE_ENTRY_APPR_OK,

        /// <summary>
        /// 初审提交
        /// </summary>
        PHASE_APPR,

        /// <summary>
        /// 初审提交黑名单
        /// </summary>
        PHASE_APPR_2_FRAUD,

        /// <summary>
        /// 初审打回黑名单
        /// </summary>
        PHASE_APPR_BK_FRARUD,

        /// <summary>
        /// 初审打回终审
        /// </summary>
        PHASE_APPR_BK_FAPPR,

        /// <summary>
        /// 高级初审打回
        /// </summary>
        PHASE_APPR_BK,

        /// <summary>
        /// 终审提交
        /// </summary>
        PHASE_FAPPR,

        /// <summary>
        /// 终审提交黑名单
        /// </summary>
        PHASE_FFAPPR_2_FRAUD,

        /// <summary>
        /// 终审打回初审
        /// </summary>
        PHASE_FAPPR_BK_APPR,

        /// <summary>
        /// 终审打回黑名单
        /// </summary>
        PHASE_FAPPR_BK_FRAUD
    }
}
